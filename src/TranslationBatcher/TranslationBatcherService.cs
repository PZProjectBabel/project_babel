using Common;

namespace TranslationBatcher;

/// <summary>
/// Splits translatable content into batches sized for translation.
/// </summary>
public class TranslationBatcherService
{
    private readonly PipelineConfig _config;

    public TranslationBatcherService(PipelineConfig config)
    {
        _config = config;
    }
    /// <summary>
    /// Splits content into batches suitable for LLM processing.
    /// </summary>
    public Task<TaskResult> CreateBatchesAsync(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> diffTranslationEntryDict,
        List<TranslationBatch> translationBatches)
    {
        translationBatches.Clear();

        var batchId = 1;
        var baseLang = NormalizeLanguage(_config.baseLanguage);
        var initialTargetLang = NormalizeLanguage(_config.priorityLanguage);
        // Sort mods by priority desc (ref: BatchGenerator priority)
        var sortedMods = diffTranslationEntryDict.Values
            .GroupBy(entry => entry.modId, StringComparer.Ordinal)
            .OrderByDescending(g =>
            {
                modInfoDict.TryGetValue(g.Key, out var modInfo);
                return g.Any(entry => entry.isActive) ? CalculatePriorityInt(modInfo) : -1;
            })
            .ThenBy(g => g.Key, StringComparer.Ordinal);

        foreach (var modGroup in sortedMods)
        {
            modInfoDict.TryGetValue(modGroup.Key, out var modInfo);
            var priority = modGroup.Any(entry => entry.isActive) ? CalculatePriorityInt(modInfo) : -1;

            var entries = modGroup
                .OrderBy(entry => entry.translationKey, StringComparer.Ordinal)
                .ToList();

            var currentEntries = new List<TranslationEntry>();
            var currentTokens = 0;

            foreach (var entry in entries)
            {
                var entryTokens = EstimateEnglishTokens(GetSourceText(entry, baseLang).text);
                var wouldExceedCount = currentEntries.Count >= _config.llmBatchSize;
                var wouldExceedTokens = _config.llmBatchTokenBudget > 0
                    && currentEntries.Count > 0
                    && currentTokens + entryTokens > _config.llmBatchTokenBudget;

                if (wouldExceedCount || wouldExceedTokens)
                {
                    AddBatch(translationBatches, batchId++, modGroup.Key, priority, currentEntries, baseLang, initialTargetLang);
                    currentEntries = [];
                    currentTokens = 0;
                }

                currentEntries.Add(entry);
                currentTokens += entryTokens;
            }

            if (currentEntries.Count > 0)
                AddBatch(translationBatches, batchId++, modGroup.Key, priority, currentEntries, baseLang, initialTargetLang);
        }

        WriteDebugBatches(translationBatches);
        var priorityStats = translationBatches
            .GroupBy(b => b.modId)
            .Select(g => new { modId = g.Key, priority = g.First().priority })
            .OrderByDescending(x => x.priority)
            .ToList();
        var summary = new
        {
            batchCount = translationBatches.Count,
            entryCount = translationBatches.Sum(batch => batch.translationEntries.Count),
            topPriorities = priorityStats.Take(5).Select(x => $"{x.modId}({x.priority})")
        };
        Console.WriteLine($"  Translation batch summary: batches={summary.batchCount}, sourceEntries={summary.entryCount}");
        if (priorityStats.Count > 0)
            Console.WriteLine($"  Priority range: {priorityStats[^1].priority} .. {priorityStats[0].priority}");

        return Task.FromResult(new TaskResult
        {
            isSuccess = true,
            summaryJson = Utf8NoBom.SerializeJson(summary)
        });
    }

    private void AddBatch(
        List<TranslationBatch> translationBatches,
        int batchId,
        string modId,
        int priority,
        List<TranslationEntry> entries,
        string baseLang,
        string targetLang)
    {
        translationBatches.Add(new TranslationBatch
        {
            batchId = batchId,
            priority = priority,
            modId = modId,
            translationEntries = entries,
            baseLang = baseLang,
            targetLang = targetLang
        });
    }

    // ── Ref: BatchGenerator/priority.py:calculate_priority_int ──
    /// <summary>Calculate batch priority from subscriptions + age. Higher = more important.</summary>
    internal static int CalculatePriorityInt(ModInfo modInfo)
    {
        double subs = Math.Max(modInfo.subscription, 0);
        double subPriority = Math.Log10(subs + 0.9);

        double weeks = 0.0;
        if (modInfo.timeModCreated != default && modInfo.timeModCreated != DateTime.MinValue)
        {
            var age = DateTime.UtcNow - modInfo.timeModCreated.ToUniversalTime();
            weeks = age.TotalDays / 7.0;
        }
        double timePriority = Math.Min(Math.Log2(Math.Max(weeks, 0) + 0.9), 9.0);

        double total = Math.Max(0.0, subPriority + timePriority / 6.0);
        return (int)Math.Round(total * 100);
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

    private static int EstimateEnglishTokens(string text)
    {
        return Math.Max(1, (int)Math.Ceiling((text?.Length ?? 0) / 4.0));
    }

    private static TranslationSourceText GetSourceText(TranslationEntry entry, string baseLang)
    {
        return entry.GetBaseTextStrict(baseLang);
    }

    private void WriteDebugBatches(List<TranslationBatch> translationBatches)
    {
        if (string.IsNullOrWhiteSpace(_config.translationBatchesTempDir))
            return;

        foreach (var batch in translationBatches)
        {
            var modDir = Path.Combine(_config.translationBatchesTempDir, batch.modId);
            Directory.CreateDirectory(modDir);
            var path = Path.Combine(modDir, $"batch_{batch.batchId:000}.json");
            var payload = new
            {
                batch.batchId,
                batch.priority,
                batch.modId,
                batch.baseLang,
                batch.targetLang,
                entries = batch.translationEntries.Select(entry => BuildDebugEntry(entry, batch.baseLang))
            };
            Utf8NoBom.WriteAllText(path, Utf8NoBom.SerializeIndentedJson(payload));
        }
    }

    private static Dictionary<string, object?> BuildDebugEntry(TranslationEntry entry, string baseLang)
    {
        var source = GetSourceText(entry, baseLang);
        return new Dictionary<string, object?>
        {
            ["translationKey"] = entry.translationKey,
            [source.lang] = source.text
        };
    }
}
