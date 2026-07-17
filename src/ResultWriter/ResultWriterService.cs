using System.Text.Json;
using Common;

namespace ResultWriter;

/// <summary>
/// Writes translation results, embeddings, and modinfo back to the file system.
/// </summary>
public class ResultWriterService
{
    private readonly PipelineConfig _config;

    public ResultWriterService(PipelineConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Writes embeddings and modinfos to data/ folder. Call once per pipeline run.
    /// </summary>
    public Task<TaskResult> WriteDataAsync(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, ModInfo>? refModInfoDict = null,
        Dictionary<string, TranslationEntry>? refTranslationEntryDict = null)
    {
        if (modInfoDict.Count > 0 || translationEntryDict.Count > 0)
        {
            WriteModInfos(modInfoDict);
            WriteEntryMetadata(translationEntryDict, Path.Combine(_config.dataDir, "entry_metadata"), "data/entry_metadata");
            WriteEmbeddings(translationEntryDict);
        }
        WriteRefData(refModInfoDict, refTranslationEntryDict);
        return Task.FromResult(new TaskResult { isSuccess = true });
    }

    public Task<TaskResult> WriteRefDataAsync(
        Dictionary<string, ModInfo> refModInfoDict,
        Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        WriteRefData(refModInfoDict, refTranslationEntryDict);
        return Task.FromResult(new TaskResult { isSuccess = true });
    }

    /// <summary>
    /// Writes translation results to the target location.
    /// </summary>
    public Task<TaskResult> WriteResultsAsync(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> refTranslationEntryDict,
        Dictionary<string, TranslationEntry> translationEntryDict)
    {
        return WriteResultsAsync(modInfoDict, refTranslationEntryDict, translationEntryDict, _config.priorityLanguage);
    }

    public Task<TaskResult> WriteResultsAsync(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> refTranslationEntryDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        string targetLanguage)
    {
        _ = refTranslationEntryDict;
        var targetLang = NormalizeLanguage(targetLanguage);
        var baseLang = NormalizeLanguage(_config.baseLanguage);
        var fileSafeLang = targetLang;

        // --- Write translation .txt to data/translations/<target_iso>/<modid>.txt ---
        var translationsDir = Path.Combine(_config.dataDir, "translations", targetLang);
        Directory.CreateDirectory(translationsDir);

        var writtenMods = 0;
        var writtenEntries = 0;

        foreach (var modGroup in translationEntryDict.Values
            .Where(entry => ShouldWriteEntry(entry, targetLang, baseLang))
            .OrderBy(entry => entry.modId, StringComparer.Ordinal)
            .ThenBy(entry => entry.translationKey, StringComparer.Ordinal)
            .GroupBy(entry => entry.modId, StringComparer.Ordinal))
        {
            var lines = new List<string>();
            foreach (var entry in modGroup)
            {
                var source = GetSourceText(entry, targetLang);
                var targetData = GetTranslationValue(entry, targetLang);

                // Source/base line
                lines.Add($"{entry.translationKey}::{source.lang} = \"{Escape(source.text)}\",");

                // Target line (only when target != source lang)
                if (!string.Equals(targetLang, source.lang, StringComparison.OrdinalIgnoreCase))
                {
                    var processed = GetProcessStatus(targetData);
                    var verified = GetVerifyStatus(targetData);
                    lines.Add($"{entry.translationKey}::{targetLang}::{processed}::{verified} = \"{Escape(targetData.text)}\",");
                }
            }

            var outputPath = Path.Combine(translationsDir, $"{modGroup.Key}.txt");
            var tmpPath = outputPath + ".tmp";
            File.WriteAllLines(tmpPath, lines, Utf8NoBom.Encoding);
            MoveFileAtomic(tmpPath, outputPath);

            writtenMods++;
            writtenEntries += modGroup.Count();
        }

        var summary = new
        {
            writtenMods,
            writtenEntries,
            translationsDir,
            dataDir = _config.dataDir
        };
        Console.WriteLine($"  Result summary: mods={writtenMods}, entries={writtenEntries}");

        return Task.FromResult(new TaskResult
        {
            isSuccess = true,
            summaryJson = Utf8NoBom.SerializeJson(summary)
        });
    }

    // ---- translation_ref/ writes ----

    private void WriteRefData(
        Dictionary<string, ModInfo>? refModInfoDict,
        Dictionary<string, TranslationEntry>? refTranslationEntryDict)
    {
        if (refModInfoDict != null && refModInfoDict.Count > 0)
            WriteRefModInfos(refModInfoDict);
        if (refTranslationEntryDict != null && refTranslationEntryDict.Count > 0)
        {
            WriteEntryMetadata(refTranslationEntryDict, Path.Combine(_config.baseDir, "translation_ref", "entry_metadata"), "translation_ref/entry_metadata");
            WriteRefEmbeddings(refTranslationEntryDict);
            WriteRefTranslations(refTranslationEntryDict);
        }
    }

    private void WriteRefModInfos(Dictionary<string, ModInfo> refModInfoDict)
    {
        var refDir = Path.Combine(_config.baseDir, "translation_ref");
        Directory.CreateDirectory(refDir);
        var modinfosPath = Path.Combine(refDir, "modinfos.json");
        var tmpPath = modinfosPath + ".tmp";

        var payload = refModInfoDict.Values
            .OrderBy(m => m.modId, StringComparer.Ordinal)
            .Select(m => BuildModInfoPayload(m.modId, m))
            .ToList();

        Utf8NoBom.WriteAllText(tmpPath, Utf8NoBom.SerializeIndentedJson(payload));
        MoveFileAtomic(tmpPath, modinfosPath);
        Console.WriteLine($"  Written translation_ref/modinfos.json: {payload.Count} ref mods");
    }

    private void WriteRefEmbeddings(Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        var embeddingsDir = Path.Combine(_config.baseDir, "translation_ref", "embeddings");
        Directory.CreateDirectory(embeddingsDir);

        var modGroups = refTranslationEntryDict.Values
            .Where(e => e.embeddingVector is { Length: > 0 })
            .GroupBy(e => e.modId, StringComparer.Ordinal);

        int written = 0;
        foreach (var modGroup in modGroups)
        {
            var records = new List<BinaryEmbeddingSerializer.Record>();
            foreach (var entry in modGroup.OrderBy(e => e.translationKey, StringComparer.Ordinal))
            {
                foreach (var embedding in GetEmbeddings(entry).Where(e => e.vector.Length > 0).OrderBy(e => e.targetLang, StringComparer.Ordinal).ThenBy(e => e.sourceKind, StringComparer.Ordinal))
                    records.Add(ToRecord(entry.translationKey, embedding));
            }

            var outputPath = Path.Combine(embeddingsDir, $"{modGroup.Key}.bin");
            var tmpPath = outputPath + ".tmp";
            BinaryEmbeddingSerializer.WriteCompressed(tmpPath, records);
            MoveFileAtomic(tmpPath, outputPath);
            written++;
        }
        Console.WriteLine($"  Written translation_ref/embeddings: {written} ref mod(s)");
    }

    private void WriteRefTranslations(Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        var translationsRoot = Path.Combine(_config.baseDir, "translation_ref", "translations");
        Directory.CreateDirectory(translationsRoot);

        // Group by (modId, lang) — collect all entries writing to same per-lang .txt
        var langModGroups = new Dictionary<(string modId, string lang), List<TranslationEntry>>();
        foreach (var entry in refTranslationEntryDict.Values)
        {
            var baseLang = NormalizeLanguage(entry.baseLang);
            // Write source line + target lines for every lang in translationValues
            foreach (var (lang, data) in entry.translationValues)
            {
                var normLang = NormalizeLanguage(lang);
                if (string.IsNullOrWhiteSpace(data.text)) continue;
                var key = (entry.modId, normLang);
                if (!langModGroups.TryGetValue(key, out var list))
                {
                    list = new List<TranslationEntry>();
                    langModGroups[key] = list;
                }
                list.Add(entry);
            }
        }

        int writtenMods = 0;
        foreach (var ((modId, lang), entries) in langModGroups.OrderBy(g => g.Key.modId, StringComparer.Ordinal).ThenBy(g => g.Key.lang, StringComparer.Ordinal))
        {
            var langDir = Path.Combine(translationsRoot, lang);
            Directory.CreateDirectory(langDir);

            var lines = new List<string>();
            foreach (var entry in entries.OrderBy(e => e.translationKey, StringComparer.Ordinal))
            {
                var targetData = entry.translationValues[lang];
                var processed = GetProcessStatus(targetData);
                var verified = GetVerifyStatus(targetData);
                lines.Add($"{entry.translationKey}::{lang}::{processed}::{verified} = \"{Escape(targetData.text)}\",");
            }

            var outputPath = Path.Combine(langDir, $"{modId}.txt");
            var tmpPath = outputPath + ".tmp";
            File.WriteAllLines(tmpPath, lines, Utf8NoBom.Encoding);
            MoveFileAtomic(tmpPath, outputPath);
            writtenMods++;
        }
        Console.WriteLine($"  Written translation_ref/translations: {writtenMods} mod-lang file(s)");
    }

    // ---- data/ writes ----

    private void WriteModInfos(Dictionary<string, ModInfo> modInfoDict)
    {
        var modinfosPath = Path.Combine(_config.dataDir, "modinfos.json");
        var tmpPath = modinfosPath + ".tmp";

        var payload = modInfoDict.Values
            .OrderBy(m => m.modId, StringComparer.Ordinal)
            .Select(m => BuildModInfoPayload(m.modId, m))
            .ToList();

        Utf8NoBom.WriteAllText(tmpPath, Utf8NoBom.SerializeIndentedJson(payload));
        MoveFileAtomic(tmpPath, modinfosPath);
        Console.WriteLine($"  Written data/modinfos.json: {payload.Count} mods");
    }

    private void WriteEmbeddings(Dictionary<string, TranslationEntry> translationEntryDict)
    {
        var embeddingsDir = Path.Combine(_config.dataDir, "embeddings");
        Directory.CreateDirectory(embeddingsDir);

        var modGroups = translationEntryDict.Values
            .Where(e => e.embeddingVector is { Length: > 0 })
            .GroupBy(e => e.modId, StringComparer.Ordinal);

        int written = 0;
        foreach (var modGroup in modGroups)
        {
            var records = new List<BinaryEmbeddingSerializer.Record>();
            foreach (var entry in modGroup.OrderBy(e => e.translationKey, StringComparer.Ordinal))
            {
                foreach (var embedding in GetEmbeddings(entry).Where(e => e.vector.Length > 0).OrderBy(e => e.sourceKind, StringComparer.Ordinal))
                    records.Add(ToRecord(entry.translationKey, embedding));
            }

            var outputPath = Path.Combine(embeddingsDir, $"{modGroup.Key}.bin");
            var tmpPath = outputPath + ".tmp";
            BinaryEmbeddingSerializer.WriteCompressed(tmpPath, records);
            MoveFileAtomic(tmpPath, outputPath);
            written++;
        }
        Console.WriteLine($"  Written data/embeddings: {written} mod(s)");
    }

    private static BinaryEmbeddingSerializer.Record ToRecord(string translationKey, TranslationEmbedding emb)
    {
        return new BinaryEmbeddingSerializer.Record(
            translationKey,
            emb.sourceKind,
            emb.targetLang,
            HashHexToRaw(emb.hash),
            emb.vector
        );
    }

    private static byte[] HashHexToRaw(string hex)
    {
        if (hex.Length < HASH_RAW_HEX_CHARS) hex = hex.PadRight(HASH_RAW_HEX_CHARS, '0');
        return Convert.FromHexString(hex[..HASH_RAW_HEX_CHARS]);
    }

    private const int HASH_RAW_HEX_CHARS = BinaryEmbeddingSerializer.HASH_RAW_BYTES * 2; // 64

    // ---- helpers ----

    private static string Escape(string text)
    {
        return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    private static bool ShouldWriteEntry(TranslationEntry entry, string targetLang, string baseLang)
    {
        return entry.translationValues.ContainsKey(targetLang)
            || entry.translationValues.ContainsKey(baseLang)
            || entry.translationValues.Values.Any(data => !string.IsNullOrWhiteSpace(data.text));
    }

    private static TranslationData GetTranslationValue(TranslationEntry entry, string lang)
    {
        return entry.translationValues.TryGetValue(lang, out var data)
            ? data
            : new TranslationData();
    }

    private string NormalizeLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return "en";

        var matched = _config.supportedLanguages.FirstOrDefault(lang =>
            string.Equals(lang.ingameCode, language, StringComparison.OrdinalIgnoreCase)
            || string.Equals(lang.isoCode, language, StringComparison.OrdinalIgnoreCase));

        return string.IsNullOrWhiteSpace(matched?.isoCode)
            ? language.ToLowerInvariant()
            : matched.isoCode.ToLowerInvariant();
    }

    private string ResolveFileSafeLanguageName(string targetLanguage)
    {
        return NormalizeLanguage(targetLanguage);
    }

    private TranslationSourceText GetSourceText(TranslationEntry entry, string targetLang)
    {
        var baseLang = NormalizeLanguage(_config.baseLanguage);
        return entry.GetBaseTextStrict(baseLang);
    }

    private void WriteDebugCopy(string modId, Dictionary<string, object?> payload, string targetLanguage)
    {
        if (string.IsNullOrWhiteSpace(_config.translationResultsTempDir))
            return;

        Directory.CreateDirectory(_config.translationResultsTempDir);
        var outputDir = ShouldUseTargetLanguageSubdir()
            ? Path.Combine(_config.translationResultsTempDir, ResolveFileSafeLanguageName(targetLanguage))
            : _config.translationResultsTempDir;
        Directory.CreateDirectory(outputDir);
        var path = Path.Combine(outputDir, $"{modId}.json");
        Utf8NoBom.WriteAllText(path, Utf8NoBom.SerializeIndentedJson(payload));
    }

    private static Dictionary<string, object?> BuildModInfoPayload(string modId, ModInfo modInfo)
    {
        var violatedRules = new List<string>();
        if (!string.IsNullOrWhiteSpace(modInfo.contentCheckViolatedRulesJson))
        {
            try
            {
                violatedRules = JsonSerializer.Deserialize<List<string>>(modInfo.contentCheckViolatedRulesJson) ?? [];
            }
            catch { }
        }

        return new Dictionary<string, object?>
        {
            ["mod_id"] = string.IsNullOrWhiteSpace(modInfo.modId) ? modId : modInfo.modId,
            ["mod_name"] = modInfo.modName,
            ["creator"] = modInfo.creator,
            ["language"] = modInfo.language,
            ["time_mod_updated"] = FormatDateTime(modInfo.timeModUpdated),
            ["time_mod_created"] = FormatDateTime(modInfo.timeModCreated),
            ["time_last_checked"] = FormatDateTime(modInfo.timeLastChecked),
            ["subscription"] = modInfo.subscription,
            ["favorite"] = modInfo.favorite,
            ["description"] = modInfo.description,
            ["consumer_app_id"] = modInfo.consumerAppId,
            ["content_check_status"] = modInfo.contentCheckStatus.ToString(),
            ["needs_update"] = modInfo.needsUpdate,
            ["needs_content_check"] = modInfo.needsContentCheck,
            ["time_next_content_check"] = FormatDateTime(modInfo.timeNextContentCheck),
            ["is_available"] = modInfo.isAvailable,
            ["last_fetch_status"] = modInfo.lastFetchStatus,
            ["content_check_confidence"] = modInfo.contentCheckConfidence,
            ["content_check_need_human_review"] = modInfo.contentCheckNeedHumanReview,
            ["content_check_risk_level"] = modInfo.contentCheckRiskLevel,
            ["content_check_reason"] = modInfo.contentCheckReason,
            ["content_check_violated_rules"] = violatedRules
        };
    }

    private static string GetProcessStatus(TranslationData data)
    {
        return data.IsProcessed ? "processed" : "unprocessed";
    }

    private static string GetVerifyStatus(TranslationData data)
    {
        return data.IsVerified ? "verified" : "unverified";
    }

    private static IEnumerable<TranslationEmbedding> GetEmbeddings(TranslationEntry entry)
    {
        if (entry.embeddingValues.Count > 0)
            return entry.embeddingValues.Values;

        if (entry.embeddingVector.Length == 0)
            return [];

        return [new TranslationEmbedding
        {
            sourceKind = string.IsNullOrWhiteSpace(entry.embeddingSourceKind) ? "normal_base_text" : entry.embeddingSourceKind,
            targetLang = entry.embeddingTargetLang,
            hash = entry.embeddingHash,
            vector = entry.embeddingVector
        }];
    }

    private static void WriteEntryMetadata(Dictionary<string, TranslationEntry> entries, string dir, string label)
    {
        Directory.CreateDirectory(dir);
        var writtenMods = 0;
        var writtenEntries = 0;

        foreach (var modGroup in entries.Values
            .OrderBy(e => e.modId, StringComparer.Ordinal)
            .ThenBy(e => e.translationKey, StringComparer.Ordinal)
            .GroupBy(e => e.modId, StringComparer.Ordinal))
        {
            var payload = modGroup
                .Select(e => new Dictionary<string, object?>
                {
                    ["mod_id"] = e.modId,
                    ["translation_key"] = e.translationKey,
                    ["is_active"] = e.isActive,
                    ["last_seen_at"] = FormatDateTime(e.lastSeenAt),
                    ["last_seen_mod_updated"] = FormatDateTime(e.lastSeenModUpdated),
                    ["source_hash"] = e.sourceHash
                })
                .ToList();

            var path = Path.Combine(dir, $"{modGroup.Key}.json");
            var tmpPath = path + ".tmp";
            Utf8NoBom.WriteAllText(tmpPath, Utf8NoBom.SerializeIndentedJson(payload));
            MoveFileAtomic(tmpPath, path);
            writtenMods++;
            writtenEntries += payload.Count;
        }

        Console.WriteLine($"  Written {label}: {writtenEntries} entries in {writtenMods} mod file(s)");
    }

    private static string? FormatDateTime(DateTime value)
    {
        return value == default || value == DateTime.MinValue
            ? null
            : value.ToString("o");
    }

    private bool ShouldUseTargetLanguageSubdir()
    {
        var baseLang = NormalizeLanguage(_config.baseLanguage);
        return _config.supportedLanguages
            .Select(lang => NormalizeLanguage(lang.isoCode))
            .Where(lang => !string.Equals(lang, baseLang, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count() > 1;
    }

    private static void MoveFileAtomic(string source, string dest)
    {
        for (int i = 0; ; i++)
        {
            try { File.Move(source, dest, overwrite: true); return; }
            catch (Exception ex) when ((ex is IOException || ex is UnauthorizedAccessException) && i < 3)
            {
                System.Threading.Thread.Sleep(100 * (i + 1));
            }
        }
    }
}
