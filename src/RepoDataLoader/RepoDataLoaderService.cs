using Common;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;

namespace RepoDataLoader;

/// <summary>
/// Loads cached translation/mod/embedding data from data/ and translation_ref/.
/// Also provides diff helpers for incremental update detection.
/// Only depends on Common; refresh orchestration lives in Program.cs.
/// </summary>
public class RepoDataLoaderService
{
    private readonly PipelineConfig _config;

    public RepoDataLoaderService(PipelineConfig config)
    {
        _config = config;
    }

    // --- data/ loading ---

    /// <summary>Load data/modinfos.json → modInfoDict. Missing file = no-op.</summary>
    public void LoadModCache(Dictionary<string, ModInfo> modInfoDict)
    {
        var path = Path.Combine(_config.dataDir, "modinfos.json");
        if (!File.Exists(path)) { Console.WriteLine("  data/modinfos.json not found, skip cache load."); return; }
        var list = JsonSerializer.Deserialize<List<ModInfoPayload>>(Utf8NoBom.ReadAllText(path), JsonOpts);
        if (list == null) return;
        foreach (var p in list)
        {
            if (string.IsNullOrWhiteSpace(p.mod_id)) continue;
            var m = new ModInfo
            {
                modId = p.mod_id,
                modName = p.mod_name ?? "",
                creator = p.creator ?? "",
                language = p.language,
                timeModUpdated = ParseDt(p.time_mod_updated),
                timeModCreated = ParseDt(p.time_mod_created),
                timeLastChecked = ParseDt(p.time_last_checked),
                subscription = p.subscription,
                favorite = p.favorite,
                description = p.description ?? "",
                consumerAppId = p.consumer_app_id,
                contentCheckStatus = Enum.TryParse<ContentCheckStatus>(p.content_check_status, true, out var s) ? s : ContentCheckStatus.UNKNOWN,
                needsUpdate = p.needs_update,
                needsContentCheck = p.needs_content_check,
                timeNextContentCheck = ParseDt(p.time_next_content_check),
                isAvailable = p.is_available ?? true,
                lastFetchStatus = p.last_fetch_status ?? "unknown",
                contentCheckConfidence = p.content_check_confidence ?? 0,
                contentCheckNeedHumanReview = p.content_check_need_human_review ?? false,
                contentCheckRiskLevel = p.content_check_risk_level ?? "",
                contentCheckReason = p.content_check_reason ?? "",
                contentCheckViolatedRulesJson = Utf8NoBom.SerializeJson(p.content_check_violated_rules ?? [])
            };
            modInfoDict[m.modId] = m;
        }
        Console.WriteLine($"  Loaded {modInfoDict.Count} mod(s) from data/modinfos.json");
    }

    /// <summary>Load data/translations/<lang>/*.txt → translationEntryDict. Merges per lang.</summary>
    public void LoadTranslationCache(Dictionary<string, TranslationEntry> translationEntryDict)
    {
        var transDir = Path.Combine(_config.dataDir, "translations");
        if (!Directory.Exists(transDir)) { Console.WriteLine("  data/translations/ not found, skip."); return; }
        int loaded = 0;
        foreach (var langDir in Directory.GetDirectories(transDir))
        {
            var lang = Path.GetFileName(langDir);
            foreach (var file in Directory.GetFiles(langDir, "*.txt"))
            {
                var modId = Path.GetFileNameWithoutExtension(file);
                foreach (var line in File.ReadLines(file, Utf8NoBom.Encoding))
                {
                    var (key, entryLang, statuses, text) = ParseTranslationLine(line);
                    if (key == null || entryLang == null || text == null) continue;
                    var entryKey = $"{modId}::{key}";
                    if (!translationEntryDict.TryGetValue(entryKey, out var entry))
                    {
                        entry = new TranslationEntry { modId = modId, translationKey = key, masterKey = key };
                        translationEntryDict[entryKey] = entry;
                    }
                    var td = new TranslationData { text = text };
                    ApplyStatuses(td, statuses);
                    entry.translationValues[entryLang] = td;
                    loaded++;
                }
            }
        }
        Console.WriteLine($"  Loaded {loaded} translation value(s) from data/translations/");
    }

    /// <summary>Load data/embeddings/*.bin (zstd-compressed binary) → fill embeddingVector+hash on existing entries.</summary>
    public void LoadEmbeddingCache(Dictionary<string, TranslationEntry> translationEntryDict)
    {
        var embDir = Path.Combine(_config.dataDir, "embeddings");
        if (!Directory.Exists(embDir)) { Console.WriteLine("  data/embeddings/ not found, skip."); return; }
        var tempDir = Path.Combine(_config.runTempDir, "embeddings_decompressed");
        int loaded = 0;
        foreach (var file in Directory.GetFiles(embDir, "*.bin"))
        {
            var modId = Path.GetFileNameWithoutExtension(file);
            var records = BinaryEmbeddingSerializer.ReadCompressed(file, tempDir);
            foreach (var rec in records)
            {
                var entryKey = $"{modId}::{rec.TranslationKey}";
                if (translationEntryDict.TryGetValue(entryKey, out var entry))
                {
                    var hashHex = Convert.ToHexString(rec.Hash).ToLowerInvariant();
                    SetEmbedding(entry, rec.SourceKind, rec.TargetLang, hashHex, rec.Vector);
                    loaded++;
                }
            }
        }
        Console.WriteLine($"  Loaded {loaded} embedding(s) from data/embeddings/");
    }

    public void LoadEntryMetadataCache(Dictionary<string, TranslationEntry> translationEntryDict)
    {
        LoadEntryMetadataStore(_config.dataDir, "data", translationEntryDict);
    }

    // --- translation_ref/ loading ---

    /// <summary>Load translation_ref/modinfos.json → refModInfoDict.</summary>
    public void LoadRefModCache(Dictionary<string, ModInfo> refModInfoDict)
    {
        var path = Path.Combine(_config.baseDir, "translation_ref", "modinfos.json");
        if (!File.Exists(path)) { Console.WriteLine("  translation_ref/modinfos.json not found, skip."); return; }
        var list = JsonSerializer.Deserialize<List<ModInfoPayload>>(Utf8NoBom.ReadAllText(path), JsonOpts);
        if (list == null) return;
        foreach (var p in list)
        {
            if (string.IsNullOrWhiteSpace(p.mod_id)) continue;
            var m = new ModInfo
            {
                modId = p.mod_id,
                modName = p.mod_name ?? "",
                creator = p.creator ?? "",
                language = p.language,
                timeModUpdated = ParseDt(p.time_mod_updated),
                timeModCreated = ParseDt(p.time_mod_created),
                timeLastChecked = ParseDt(p.time_last_checked),
                subscription = p.subscription,
                favorite = p.favorite,
                description = p.description ?? "",
                consumerAppId = p.consumer_app_id,
                contentCheckStatus = Enum.TryParse<ContentCheckStatus>(p.content_check_status, true, out var rs) ? rs : ContentCheckStatus.UNKNOWN,
                isAvailable = p.is_available ?? true,
                lastFetchStatus = p.last_fetch_status ?? "unknown",
                contentCheckConfidence = p.content_check_confidence ?? 0,
                contentCheckNeedHumanReview = p.content_check_need_human_review ?? false,
                contentCheckRiskLevel = p.content_check_risk_level ?? "",
                contentCheckReason = p.content_check_reason ?? "",
                contentCheckViolatedRulesJson = Utf8NoBom.SerializeJson(p.content_check_violated_rules ?? [])
            };
            refModInfoDict[m.modId] = m;
        }
        Console.WriteLine($"  Loaded {refModInfoDict.Count} ref mod(s) from translation_ref/modinfos.json");
    }

    /// <summary>Load translation_ref/translations/ → refTranslationEntryDict.</summary>
    public void LoadRefTranslationCache(Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        var transDir = Path.Combine(_config.baseDir, "translation_ref", "translations");
        if (!Directory.Exists(transDir)) { Console.WriteLine("  translation_ref/translations/ not found, skip."); return; }
        int loaded = 0;
        foreach (var langDir in Directory.GetDirectories(transDir))
        {
            var lang = Path.GetFileName(langDir);
            foreach (var file in Directory.GetFiles(langDir, "*.txt"))
            {
                var modId = Path.GetFileNameWithoutExtension(file);
                foreach (var line in File.ReadLines(file, Utf8NoBom.Encoding))
                {
                    var (key, entryLang, statuses, text) = ParseTranslationLine(line);
                    if (key == null || entryLang == null || text == null) continue;
                    var entryKey = $"{modId}::{key}";
                    if (!refTranslationEntryDict.TryGetValue(entryKey, out var entry))
                    {
                        entry = new TranslationEntry { modId = modId, translationKey = key, masterKey = key };
                        refTranslationEntryDict[entryKey] = entry;
                    }
                    var td = new TranslationData { text = text };
                    ApplyStatuses(td, statuses);
                    entry.translationValues[entryLang] = td;
                    loaded++;
                }
            }
        }
        Console.WriteLine($"  Loaded {loaded} ref translation value(s) from translation_ref/translations/");
    }

    /// <summary>Load translation_ref/embeddings/ (zstd-compressed binary) → fill embedding on ref entries.</summary>
    public void LoadRefEmbeddingCache(Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        var embDir = Path.Combine(_config.baseDir, "translation_ref", "embeddings");
        if (!Directory.Exists(embDir)) { Console.WriteLine("  translation_ref/embeddings/ not found, skip."); return; }
        var tempDir = Path.Combine(_config.runTempDir, "embeddings_decompressed");
        int loaded = 0;
        foreach (var file in Directory.GetFiles(embDir, "*.bin"))
        {
            var modId = Path.GetFileNameWithoutExtension(file);
            var records = BinaryEmbeddingSerializer.ReadCompressed(file, tempDir);
            foreach (var rec in records)
            {
                var entryKey = $"{modId}::{rec.TranslationKey}";
                if (refTranslationEntryDict.TryGetValue(entryKey, out var entry))
                {
                    var hashHex = Convert.ToHexString(rec.Hash).ToLowerInvariant();
                    SetEmbedding(entry, rec.SourceKind, rec.TargetLang, hashHex, rec.Vector);
                    loaded++;
                }
            }
        }
        Console.WriteLine($"  Loaded {loaded} ref embedding(s) from translation_ref/embeddings/");
    }

    public void LoadRefEntryMetadataCache(Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        LoadEntryMetadataStore(Path.Combine(_config.baseDir, "translation_ref"), "translation_ref", refTranslationEntryDict);
    }

    // --- Diff ---

    /// <summary>Compare ref_translation_mods.json entries vs cached refModInfoDict. Returns modIds needing download.</summary>
    public HashSet<string> DiffRefMods(List<ModInfo> configured, Dictionary<string, ModInfo> cached)
    {
        var needRefresh = new HashSet<string>(StringComparer.Ordinal);
        foreach (var cfg in configured)
        {
            if (string.IsNullOrWhiteSpace(cfg.modId)) continue;
            if (!cached.TryGetValue(cfg.modId, out var cachedMod))
            {
                needRefresh.Add(cfg.modId);
                continue;
            }
            // compare timeModUpdated (from ref_translation_mods.json vs cache)
            if (cfg.timeModUpdated > cachedMod.timeModUpdated)
                needRefresh.Add(cfg.modId);
            // update metadata even if not re-downloading
            cachedMod.modName = cfg.modName;
            cachedMod.language = cfg.language;
        }
        Console.WriteLine($"  Ref diff: {needRefresh.Count}/{configured.Count} need refresh");
        return needRefresh;
    }

    /// <summary>
    /// Compare fresh entries against cached snapshot.
    /// Returns entries that are new or changed. Modifies freshEntry objects in-place (resets embedding on changes).
    /// Caller is responsible for merging unchanged entries from cache back into the main dict.
    /// </summary>
    public static Dictionary<string, TranslationEntry> DiffTranslationEntries(
        Dictionary<string, TranslationEntry> freshEntries,
        Dictionary<string, TranslationEntry> cachedEntries,
        string baseLang = "en",
        Dictionary<string, ModInfo>? modInfoDict = null)
    {
        var diff = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        var nowUtc = DateTime.UtcNow;
        foreach (var (key, fresh) in freshEntries)
        {
            fresh.isActive = true;
            fresh.lastSeenAt = nowUtc;
            if (modInfoDict != null && modInfoDict.TryGetValue(fresh.modId, out var modInfo))
                fresh.lastSeenModUpdated = modInfo.timeModUpdated;
            fresh.sourceHash = ComputeSourceHash(fresh, baseLang);

            if (!cachedEntries.TryGetValue(key, out var cached))
            {
                diff[key] = fresh; // new entry
                continue;
            }
            var cachedSourceHash = string.IsNullOrWhiteSpace(cached.sourceHash)
                ? ComputeSourceHash(cached, baseLang)
                : cached.sourceHash;
            if (!string.Equals(fresh.sourceHash, cachedSourceHash, StringComparison.Ordinal))
            {
                MergeTargetTranslations(fresh, cached, invalidateTargets: true);
                fresh.embeddingVector = [];
                fresh.embeddingHash = "";
                fresh.embeddingValues.Clear();
                diff[key] = fresh;
                continue;
            }
            MergeTargetTranslations(fresh, cached, invalidateTargets: false);
            // Check for new target languages (not in cached), excluding baseLang.
            var hasNewTargetLangs = fresh.translationValues.Keys
                .Any(lang => !string.Equals(lang, baseLang, StringComparison.OrdinalIgnoreCase)
                    && !cached.translationValues.ContainsKey(lang));
            if (hasNewTargetLangs)
            {
                fresh.embeddingVector = [];
                fresh.embeddingHash = "";
                diff[key] = fresh;
            }
            // else: unchanged → caller keeps cached entry (with translations + embeddings)
        }
        Console.WriteLine($"  Entry diff: {diff.Count}/{freshEntries.Count} changed/new");
        return diff;
    }

    public static void MarkMissingFreshEntriesInactive(
        Dictionary<string, TranslationEntry> cachedEntries,
        Dictionary<string, TranslationEntry> freshEntries,
        HashSet<string> updatedModIds)
    {
        foreach (var (key, cached) in cachedEntries)
        {
            if (!updatedModIds.Contains(cached.modId) || freshEntries.ContainsKey(key))
                continue;

            cached.isActive = false;
        }
    }

    // --- Helpers ---

    // ── Line parsers ──

    /// <summary>Parse "key::lang[::status...] = "value"," → (key, lang, statuses[], text).</summary>
    internal static (string? key, string? lang, string[]? statuses, string? text) ParseTranslationLine(string line)
    {
        line = line.TrimEnd();
        if (string.IsNullOrWhiteSpace(line)) return default;
        // Find value: last = " ... ",
        var eqIdx = line.LastIndexOf(" = \"", StringComparison.Ordinal);
        if (eqIdx < 0) return default;
        var head = line[..eqIdx];
        var valueStr = line[(eqIdx + 4)..]; // skip " = \""
        if (valueStr.Length < 2 || !valueStr.EndsWith("\",")) return default;
        var text = valueStr[..^2]; // drop ",
        text = Unescape(text);

        // Parse head: key::lang[::status1::status2...]
        var parts = head.Split("::");
        if (parts.Length < 2) return default;
        var key = StripNullBytes(parts[0]);
        var lang = parts[1].ToLowerInvariant();
        var statuses = parts.Length > 2 ? parts[2..] : null;
        return (key, lang, statuses, text);
    }

    private static void ApplyStatuses(TranslationData td, string[]? statuses)
    {
        if (statuses == null || statuses.Length == 0)
            return;

        var first = statuses[0].ToLowerInvariant();
        if (first is "processed" or "unprocessed")
            td.processStatus = first;
        else
        {
            td.status = first;
            if (first is "verified" or "unverified")
                td.isVerified = first == "verified";
        }

        if (statuses.Length >= 2)
        {
            var second = statuses[1].ToLowerInvariant();
            if (second is "verified" or "unverified")
            {
                td.status = second;
                td.isVerified = second == "verified";
            }
            else if (bool.TryParse(statuses[1], out var v))
            {
                td.isVerified = v;
                td.status = v ? "verified" : "unverified";
            }
        }

        if (td.isVerified)
            td.processStatus = "processed";
    }

    private static void SetEmbedding(TranslationEntry entry, string? sourceKind, string? targetLang, string hash, float[] vec)
    {
        sourceKind = string.IsNullOrWhiteSpace(sourceKind) ? "normal_base_text" : sourceKind;
        targetLang = targetLang?.ToLowerInvariant() ?? "";
        var key = BuildEmbeddingKey(sourceKind, targetLang);
        entry.embeddingValues[key] = new TranslationEmbedding
        {
            sourceKind = sourceKind,
            targetLang = targetLang,
            hash = hash,
            vector = vec
        };
        entry.embeddingHash = hash;
        entry.embeddingSourceKind = sourceKind;
        entry.embeddingTargetLang = targetLang;
        entry.embeddingVector = vec;
    }

    private static string BuildEmbeddingKey(string sourceKind, string targetLang)
    {
        return string.IsNullOrWhiteSpace(targetLang) ? sourceKind : $"{targetLang}::{sourceKind}";
    }

    private static void LoadEntryMetadataStore(string rootDir, string label, Dictionary<string, TranslationEntry> entries)
    {
        var loaded = 0;
        var metadataDir = Path.Combine(rootDir, "entry_metadata");

        if (!Directory.Exists(metadataDir))
        {
            Console.WriteLine($"  {label}/entry_metadata not found, skip.");
            return;
        }

        foreach (var file in Directory.GetFiles(metadataDir, "*.json").OrderBy(Path.GetFileName, StringComparer.Ordinal))
            loaded += LoadEntryMetadataFile(file, entries);

        Console.WriteLine($"  Loaded {loaded} entry metadata row(s) from {label}/entry_metadata");
    }

    private static int LoadEntryMetadataFile(string path, Dictionary<string, TranslationEntry> entries)
    {
        var list = JsonSerializer.Deserialize<List<EntryMetadataPayload>>(Utf8NoBom.ReadAllText(path), JsonOpts);
        if (list == null) return 0;
        var loaded = 0;
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.mod_id) || string.IsNullOrWhiteSpace(item.translation_key))
                continue;
            var cleanKey = StripNullBytes(item.translation_key);
            var entryKey = $"{item.mod_id}::{cleanKey}";
            if (!entries.TryGetValue(entryKey, out var entry))
                continue;
            entry.isActive = item.is_active ?? true;
            entry.lastSeenAt = ParseDt(item.last_seen_at);
            entry.lastSeenModUpdated = ParseDt(item.last_seen_mod_updated);
            entry.sourceHash = item.source_hash ?? "";
            loaded++;
        }
        return loaded;
    }

    private static void MergeTargetTranslations(TranslationEntry fresh, TranslationEntry cached, bool invalidateTargets)
    {
        foreach (var (lang, cachedData) in cached.translationValues)
        {
            if (string.Equals(lang, fresh.baseLang, StringComparison.OrdinalIgnoreCase))
                continue;

            var copied = fresh.translationValues.TryGetValue(lang, out var freshData)
                ? freshData
                : CloneData(cachedData);
            if (invalidateTargets && !string.Equals(lang, fresh.baseLang, StringComparison.OrdinalIgnoreCase))
            {
                copied.processStatus = "unprocessed";
                copied.status = "unverified";
                copied.isVerified = false;
                copied.confidence = null;
            }
            fresh.translationValues[lang] = copied;
        }
    }

    private static TranslationData CloneData(TranslationData src)
    {
        return new TranslationData
        {
            text = src.text,
            isVerified = src.isVerified,
            confidence = src.confidence,
            status = src.status,
            processStatus = src.processStatus,
            comments = new List<string>(src.comments)
        };
    }

    public static string ComputeSourceHash(TranslationEntry entry, string baseLang)
    {
        var source = entry.GetBaseTextStrict(baseLang);
        return ComputeSha128($"{entry.modId}::{entry.translationKey}::{source.lang}=\"{source.text}\"");
    }

    private static string ComputeSha128(string text)
    {
        var hash = SHA256.HashData(Utf8NoBom.Encoding.GetBytes(text));
        return Convert.ToHexString(hash, 0, 16).ToLowerInvariant();
    }

    private static string Unescape(string s)
    {
        return s.Replace("\\\"", "\"").Replace("\\\\", "\\");
    }

    /// <summary>Strips NUL bytes commonly found when UTF-16 LE data was read as UTF-8.</summary>
    private static string StripNullBytes(string s)
    {
        if (s.IndexOf('\0') < 0)
            return s;
        return s.Replace("\0", "");
    }

    // ── JSON helpers ──

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private sealed record ModInfoPayload(
        string? mod_id,
        string? mod_name,
        string? creator,
        string? language,
        string? time_mod_updated,
        string? time_mod_created,
        string? time_last_checked,
        int subscription,
        int favorite,
        string? description,
        int consumer_app_id,
        string? content_check_status,
        bool needs_update,
        bool needs_content_check,
        string? time_next_content_check,
        bool? is_available,
        string? last_fetch_status,
        double? content_check_confidence,
        bool? content_check_need_human_review,
        string? content_check_risk_level,
        string? content_check_reason,
        List<string>? content_check_violated_rules
    );

    private sealed record EntryMetadataPayload(
        string? mod_id,
        string? translation_key,
        bool? is_active,
        string? last_seen_at,
        string? last_seen_mod_updated,
        string? source_hash
    );

    private static DateTime ParseDt(string? v)
    {
        if (string.IsNullOrWhiteSpace(v)) return DateTime.MinValue;
        return DateTime.TryParse(v, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dt)
            ? dt : DateTime.MinValue;
    }
}
