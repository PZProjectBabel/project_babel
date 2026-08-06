using Common;
using PercentNormalizer;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LLMTranslator;

public sealed class LlmTranslationPlan
{
    private string _warmupPromptPath = "";
    private bool _deleteWarmupPromptFileAfterUse;

    internal LlmTranslationPlan(string targetLang, string fileSafeLanguageName, string targetDisplayName)
    {
        TargetLang = targetLang;
        FileSafeLanguageName = fileSafeLanguageName;
        TargetDisplayName = targetDisplayName;
    }

    public string TargetLang { get; }
    public string FileSafeLanguageName { get; }
    public string TargetDisplayName { get; }
    public int RequestCount => WorkItems.Count;
    public int EmptyWriteCount => EmptyWrites.Count;
    public bool HasWarmupPrompt => !string.IsNullOrWhiteSpace(_warmupPromptPath);

    internal List<LlmTranslationWorkItem> WorkItems { get; } = [];
    internal List<LlmTargetWrite> EmptyWrites { get; } = [];
    internal string WarmupPromptPath => _warmupPromptPath;

    internal void SetWarmupPrompt(string path, bool deleteAfterUse)
    {
        _warmupPromptPath = path;
        _deleteWarmupPromptFileAfterUse = deleteAfterUse;
    }

    internal void CleanupWarmupPromptFile()
    {
        if (!_deleteWarmupPromptFileAfterUse || string.IsNullOrWhiteSpace(_warmupPromptPath))
            return;

        try
        {
            if (File.Exists(_warmupPromptPath))
                File.Delete(_warmupPromptPath);
        }
        catch
        {
            // Best-effort cleanup for fallback temp prompt files.
        }
    }
}

public sealed record LlmConcurrencySettings(
    int Initial,
    int Maximum,
    int Minimum,
    int MaxRetries,
    int FailureStreakToDecrease,
    int RetryBaseDelayMs,
    int RetryMaxDelayMs,
    string Profile,
    string? RunnerOs,
    string OperatingSystem);

internal sealed class LlmTranslationWorkItem
{
    public LlmTranslationWorkItem(
        string targetLang,
        string fileSafeLanguageName,
        int batchId,
        int totalBatches,
        string modId,
        string modName,
        List<TranslationEntry> entries,
        string promptPath,
        bool deletePromptFileAfterUse,
        ModInfo modInfo)
    {
        TargetLang = targetLang;
        FileSafeLanguageName = fileSafeLanguageName;
        BatchId = batchId;
        TotalBatches = totalBatches;
        ModId = modId;
        ModName = modName;
        Entries = entries;
        PromptPath = promptPath;
        DeletePromptFileAfterUse = deletePromptFileAfterUse;
        ModInfo = modInfo;
    }

    public string TargetLang { get; }
    public string FileSafeLanguageName { get; }
    public int BatchId { get; }
    public int TotalBatches { get; }
    public string ModId { get; }
    public string ModName { get; }
    public List<TranslationEntry> Entries { get; }
    public string PromptPath { get; }
    public bool DeletePromptFileAfterUse { get; }
    public ModInfo ModInfo { get; }

    public void CleanupPromptFile()
    {
        if (!DeletePromptFileAfterUse || string.IsNullOrWhiteSpace(PromptPath))
            return;

        try
        {
            if (File.Exists(PromptPath))
                File.Delete(PromptPath);
        }
        catch
        {
            // Best-effort cleanup for fallback temp prompt files.
        }
    }
}

internal sealed record LlmTargetWrite(
    string TargetLang,
    TranslationEntry Entry,
    string Text,
    float? Confidence,
    string Status,
    string? Comment);

/// <summary>
/// Uses an LLM to translate checked batches with RAG context, glossary, JSON mode retry.
/// Ported from ref: prompt_builder + llm_client + response_parser + glossary_prompt_builder.
/// </summary>
public partial class LLMTranslatorService
{
    private const int VerboseBatchResultLimit = 50;
    private const int BatchResultLogInterval = 10;
    private static readonly TimeSpan BatchResultLogIntervalTime = TimeSpan.FromSeconds(30);
    private const int WarmupBatchThreshold = 5;
    private const string PromptTailReminder = "只返回符合上述输出规则的纯文本, 不输出任何额外字符。";
    private const long LowAvailableMemoryBytes = 512L * 1024 * 1024;
    private const long CriticalAvailableMemoryBytes = 256L * 1024 * 1024;
    private const double LowAvailableMemoryRatio = 0.10;
    private const double CriticalAvailableMemoryRatio = 0.05;
    private static readonly TimeSpan MemoryPressureCheckInterval = TimeSpan.FromSeconds(2);
    private readonly PipelineConfig _config;
    private readonly HttpClient? _httpClient;
    private readonly string _fallbackPromptRoot = Path.Combine(Path.GetTempPath(), "project_babel_llm_prompts", Guid.NewGuid().ToString("N"));
    private readonly Dictionary<string, Dictionary<string, string>[]?> _dictionaryCacheByTargetLang = new(StringComparer.OrdinalIgnoreCase);
    private DateTime _lastProgressLogUtc = DateTime.MinValue;
    private readonly object _progressLogLock = new();

    public LLMTranslatorService(PipelineConfig config, HttpClient? httpClient = null)
    {
        _config = config;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Translates checked content batches for the current configured target language.
    /// </summary>
    public async Task<TaskResult> TranslateAsync(
        Dictionary<string, ModInfo> modInfoDict,
        List<TranslationBatch> translationBatches,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, List<Dictionary<string, object?>>> ragContextByEntryKey)
    {
        _ = translationEntryDict;
        var plan = await PrepareTranslationPlanAsync(modInfoDict, translationBatches, ragContextByEntryKey, _config.priorityLanguage);
        return await ExecuteTranslationPlansAsync([plan]);
    }

    public Task<LlmTranslationPlan> PrepareTranslationPlanAsync(
        Dictionary<string, ModInfo> modInfoDict,
        List<TranslationBatch> translationBatches,
        Dictionary<string, List<Dictionary<string, object?>>> ragContextByEntryKey,
        string targetLanguage)
    {
        var targetLang = NormalizeLanguage(targetLanguage);
        var plan = new LlmTranslationPlan(
            targetLang,
            targetLang,
            ResolveTargetLanguageDisplayName(targetLanguage));
        var totalBatches = translationBatches.Count;

        foreach (var batch in translationBatches)
        {
            var sourceEntries = batch.translationEntries
                .Where(entry => NeedsTargetProcessing(entry, targetLang))
                .ToList();

            if (sourceEntries.Count == 0)
                continue;

            // Empty source text → skip (no LLM call needed, even with RAG)
            var translatable = new List<TranslationEntry>();
            foreach (var entry in sourceEntries)
            {
                var srcText = entry.GetBaseTextStrict(_config.baseLanguage).text;
                if (!string.IsNullOrWhiteSpace(srcText))
                {
                    translatable.Add(entry);
                    continue;
                }
                plan.EmptyWrites.Add(new LlmTargetWrite(targetLang, entry, "", 1.0f, "unverified", null));
            }

            if (translatable.Count == 0)
                continue;

            var promptItems = BuildPromptItems(translatable, ragContextByEntryKey, targetLang);
            var modInfo = modInfoDict.GetValueOrDefault(batch.modId);
            var modName = !string.IsNullOrWhiteSpace(modInfo.modName) ? modInfo.modName : batch.modId;
            var prompt = BuildPrompt(batch, promptItems, modInfo, targetLanguage);
            var promptFile = WritePromptFile(batch, prompt, targetLanguage);
            prompt = "";
            promptItems.Clear();

            plan.WorkItems.Add(new LlmTranslationWorkItem(
                targetLang,
                plan.FileSafeLanguageName,
                batch.batchId,
                totalBatches,
                batch.modId,
                modName,
                translatable,
                promptFile.Path,
                promptFile.DeleteAfterUse,
                modInfo));
        }

        if (plan.RequestCount > WarmupBatchThreshold)
        {
            var warmupPrompt = BuildWarmupPrompt(targetLanguage);
            var warmupPromptFile = WriteWarmupPromptFile(warmupPrompt, targetLanguage);
            plan.SetWarmupPrompt(warmupPromptFile.Path, warmupPromptFile.DeleteAfterUse);
        }

        Console.WriteLine($"  [LLM] Prepared target={plan.TargetDisplayName}: requests={plan.RequestCount}, empty={plan.EmptyWriteCount}");
        return Task.FromResult(plan);
    }

    public async Task<TaskResult> ExecuteTranslationPlansAsync(IEnumerable<LlmTranslationPlan> translationPlans)
    {
        var plans = translationPlans.ToList();
        var settings = ResolveConcurrencySettings(_config);
        var serialSettings = settings with { Initial = 1, Maximum = 1, Minimum = 1 };
        var totalWorkItemCount = plans.Sum(plan => plan.WorkItems.Count);
        var totalEmptyWriteCount = plans.Sum(plan => plan.EmptyWrites.Count);

        Console.WriteLine(
            $"  [LLM] Starting serial translation: requests={totalWorkItemCount}, empty={totalEmptyWriteCount}, warmupThreshold={WarmupBatchThreshold}, profile={settings.Profile}");

        var translatedCount = 0;
        var skippedCount = totalEmptyWriteCount;
        var warningCount = 0;
        var failedPromptCount = 0;
        var warmupRequestCount = 0;
        var failedWarmupCount = 0;
        var emptyTranslationCount = 0;
        var processedResultCount = 0;
        var retriedAttemptCount = 0;
        var maxObservedConcurrency = 0;
        var memoryThrottleCount = 0;
        var finalConcurrency = serialSettings.Initial;
        var stopAllPlans = false;

        for (var planIndex = 0; planIndex < plans.Count && !stopAllPlans; planIndex++)
        {
            var plan = plans[planIndex];
            foreach (var emptyWrite in plan.EmptyWrites)
                ApplyTargetWrite(emptyWrite);

            if (plan.HasWarmupPrompt)
            {
                warmupRequestCount++;
                var warmup = await ExecuteWarmupAsync(plan, serialSettings);
                retriedAttemptCount += Math.Max(0, warmup.AttemptCount - 1);
                if (!warmup.IsSuccess)
                {
                    warningCount++;
                    failedWarmupCount++;
                    WriteWarmupWarning(plan, warmup.AttemptCount, warmup.Exception);
                    if (warmup.IsAccountFatal)
                    {
                        var unscheduled = plan.WorkItems
                            .Concat(plans.Skip(planIndex + 1).SelectMany(remainingPlan => remainingPlan.WorkItems))
                            .ToList();
                        foreach (var item in unscheduled)
                            item.CleanupPromptFile();
                        failedPromptCount += unscheduled.Count;
                        warningCount += unscheduled.Count;
                        WriteTaskPoolStopWarning(unscheduled.Count, warmup.Exception?.Message);
                        stopAllPlans = true;
                        break;
                    }
                }
            }

            if (stopAllPlans)
                break;

            LlmTaskPoolResult execution;
            if (plan.WorkItems.Count == 0)
            {
                execution = new LlmTaskPoolResult(0, 0, 0, 0, 0, [], [], null);
            }
            else if (_config.llmFixedConcurrency > 0)
            {
                execution = await ExecuteWorkItemsFixedWindowAsync(plan.WorkItems, _config.llmFixedConcurrency);
            }
            else
            {
                execution = await ExecuteWorkItemsAsync(plan.WorkItems, serialSettings);
            }

            retriedAttemptCount += execution.RetriedAttemptCount;
            maxObservedConcurrency = Math.Max(maxObservedConcurrency, execution.MaxObservedConcurrency);
            memoryThrottleCount += execution.MemoryThrottleCount;
            finalConcurrency = execution.FinalConcurrency;

            foreach (var result in execution.Results)
            {
                processedResultCount++;
                if (!result.IsSuccess || result.Translations == null)
                {
                    warningCount++;
                    failedPromptCount++;
                    WriteWorkFailureWarning(result.WorkItem, result.AttemptCount, result.Exception);
                    continue;
                }

                var batchTranslated = 0;
                var batchWarnings = 0;
                foreach (var entry in result.WorkItem.Entries)
                {
                    var entryKey = entry.translationKey;
                    var translation = result.Translations[entryKey];
                    if (string.IsNullOrWhiteSpace(translation.text))
                    {
                        warningCount++;
                        emptyTranslationCount++;
                        batchWarnings++;
                        WriteTranslationWarning(entry, result.WorkItem.TargetLang, "LLM returned an empty translation.");
                        continue;
                    }

                    ApplyTargetWrite(new LlmTargetWrite(
                        result.WorkItem.TargetLang,
                        entry,
                        translation.text,
                        translation.confidence,
                        "unverified",
                        translation.comment));
                    translatedCount++;
                    batchTranslated++;
                }

                if (ShouldLogBatchResult(processedResultCount, totalWorkItemCount, batchWarnings))
                {
                    lock (_progressLogLock)
                        _lastProgressLogUtc = DateTime.UtcNow;
                    Console.WriteLine(
                        $"  [LLM] progress {processedResultCount}/{totalWorkItemCount}: target={result.WorkItem.TargetLang} batch {result.WorkItem.BatchId}/{result.WorkItem.TotalBatches} | OK | {batchTranslated}/{result.WorkItem.Entries.Count} translated | warnings={batchWarnings} | attempts={result.AttemptCount} | {result.Elapsed.TotalSeconds:F1}s");
                }
            }

            var unscheduledItems = execution.UnscheduledItems.ToList();
            if (!string.IsNullOrWhiteSpace(execution.StopReason))
            {
                var remainingItems = plans
                    .Skip(planIndex + 1)
                    .SelectMany(remainingPlan => remainingPlan.WorkItems)
                    .ToList();
                foreach (var item in remainingItems)
                    item.CleanupPromptFile();
                unscheduledItems.AddRange(remainingItems);
                stopAllPlans = true;
            }

            if (unscheduledItems.Count > 0)
            {
                failedPromptCount += unscheduledItems.Count;
                warningCount += unscheduledItems.Count;
                WriteTaskPoolStopWarning(unscheduledItems.Count, execution.StopReason);
            }
        }

        var summary = new
        {
            translatedCount,
            skippedCount,
            warningCount,
            failedPromptCount,
            warmupRequestCount,
            failedWarmupCount,
            emptyTranslationCount,
            requestCount = totalWorkItemCount,
            retriedAttemptCount,
            maxObservedConcurrency,
            initialConcurrency = serialSettings.Initial,
            maximumConcurrency = serialSettings.Maximum,
            finalConcurrency,
            memoryThrottleCount,
            settings.Profile,
            settings.RunnerOs,
            settings.OperatingSystem
        };
        Console.WriteLine($"  LLM summary: translated={translatedCount}, skipped={skippedCount}, warnings={warningCount}, failedPrompts={failedPromptCount}, warmups={warmupRequestCount}, failedWarmups={failedWarmupCount}, retriedAttempts={retriedAttemptCount}, finalConcurrency={finalConcurrency}, memoryThrottles={memoryThrottleCount}");

        return new TaskResult
        {
            isSuccess = true,
            errorCount = 0,
            warningCount = warningCount,
            summaryJson = Utf8NoBom.SerializeJson(summary)
        };
    }

    public static LlmConcurrencySettings ResolveConcurrencySettings(
        PipelineConfig config,
        Func<string, string?>? getEnvironmentVariable = null)
    {
        getEnvironmentVariable ??= Environment.GetEnvironmentVariable;
        var isGitHubActions = string.Equals(getEnvironmentVariable("GITHUB_ACTIONS"), "true", StringComparison.OrdinalIgnoreCase);
        var runnerOs = getEnvironmentVariable("RUNNER_OS");
        var model = config.llmModel.ToLowerInvariant();
        var endpoint = config.llmApiEndpoint.ToLowerInvariant();

        var autoInitial = 16;
        var autoMaximum = 128;
        var profile = "unknown";

        if (isGitHubActions)
        {
            autoInitial = 4;
            autoMaximum = 32;
            profile = "github-actions";
        }
        else if (endpoint.Contains("deepseek", StringComparison.OrdinalIgnoreCase)
            || model.Contains("deepseek", StringComparison.OrdinalIgnoreCase))
        {
            if (model.Contains("v4-flash", StringComparison.OrdinalIgnoreCase))
            {
                autoInitial = 128;
                autoMaximum = 2000;
                profile = "deepseek-v4-flash";
            }
            else if (model.Contains("v4-pro", StringComparison.OrdinalIgnoreCase))
            {
                autoInitial = 64;
                autoMaximum = 400;
                profile = "deepseek-v4-pro";
            }
            else
            {
                profile = "deepseek-unknown";
            }
        }

        var minimum = Math.Max(1, config.llmConcurrencyMinimum);
        var maximum = config.llmConcurrencyMaximum > 0
            ? Math.Max(minimum, config.llmConcurrencyMaximum)
            : Math.Max(minimum, autoMaximum);
        var initial = config.llmConcurrencyInitial > 0
            ? config.llmConcurrencyInitial
            : autoInitial;
        initial = Math.Clamp(initial, minimum, maximum);

        return new LlmConcurrencySettings(
            initial,
            maximum,
            minimum,
            Math.Max(0, config.llmConcurrencyMaxRetries),
            Math.Max(1, config.llmConcurrencyFailureStreakToDecrease),
            Math.Max(1, config.llmConcurrencyRetryBaseDelayMs),
            Math.Max(1, config.llmConcurrencyRetryMaxDelayMs),
            profile,
            runnerOs,
            Environment.OSVersion.Platform.ToString());
    }

    private async Task<LlmTaskPoolResult> ExecuteWorkItemsAsync(
        List<LlmTranslationWorkItem> workItems,
        LlmConcurrencySettings settings)
    {
        using var ownedHandler = _httpClient == null
            ? new SocketsHttpHandler { MaxConnectionsPerServer = Math.Max(1, settings.Maximum) }
            : null;
        using var ownedClient = _httpClient == null
            ? new HttpClient(ownedHandler!)
            : null;
        var client = _httpClient ?? ownedClient!;
        if (ownedClient != null)
            client.Timeout = TimeSpan.FromSeconds(Math.Max(30, _config.llmRequestTimeoutSeconds));

        var totalWorkItems = workItems.Count;
        var pending = new Queue<LlmTranslationWorkItem>(workItems);
        var running = new Dictionary<Task<LlmWorkResult>, LlmTranslationWorkItem>();
        var results = new List<LlmWorkResult>();
        var currentLimit = settings.Initial;
        var maxObservedConcurrency = 0;
        var retriedAttemptCount = 0;
        var successStreak = 0;
        var pressureFailureStreak = 0;
        var memoryThrottleCount = 0;
        var lastMemoryCheckUtc = DateTime.MinValue;
        LlmMemorySnapshot? lastMemorySnapshot = null;
        var stopScheduling = false;
        string? stopReason = null;

        while (pending.Count > 0 || running.Count > 0)
        {
            var memoryPressure = TryApplyMemoryThrottle(
                settings,
                ref currentLimit,
                ref memoryThrottleCount,
                ref lastMemoryCheckUtc,
                ref lastMemorySnapshot);

            while (!stopScheduling && pending.Count > 0 && running.Count < currentLimit)
            {
                memoryPressure = TryApplyMemoryThrottle(
                    settings,
                    ref currentLimit,
                    ref memoryThrottleCount,
                    ref lastMemoryCheckUtc,
                    ref lastMemorySnapshot);
                if (memoryPressure && running.Count >= currentLimit)
                    break;

                var item = pending.Dequeue();
                var task = TranslateWorkItemWithRetriesAsync(client, item, settings);
                running[task] = item;
                maxObservedConcurrency = Math.Max(maxObservedConcurrency, running.Count);
            }

            if (running.Count == 0)
                break;

            var completed = await Task.WhenAny(running.Keys);
            running.Remove(completed);
            var result = await completed;
            results.Add(result);
            retriedAttemptCount += Math.Max(0, result.AttemptCount - 1);
            Console.WriteLine(
                $"  [LLM] {results.Count}/{totalWorkItems}: batch {result.WorkItem.BatchId}/{result.WorkItem.TotalBatches} | {(result.IsSuccess ? "OK" : "FAIL")} | {result.Elapsed.TotalSeconds:F1}s | attempts={result.AttemptCount}");

            if (result.IsAccountFatal)
            {
                stopScheduling = true;
                stopReason = result.Exception?.Message ?? "Account-level LLM failure.";
                successStreak = 0;
                pressureFailureStreak = 0;
                continue;
            }

            if (result.IsSuccess)
            {
                pressureFailureStreak = 0;
                successStreak++;
                var successThreshold = Math.Clamp(currentLimit, 10, 100);
                if (pending.Count > 0 && currentLimit < settings.Maximum && successStreak >= successThreshold)
                {
                    var stillMemoryPressure = TryApplyMemoryThrottle(
                        settings,
                        ref currentLimit,
                        ref memoryThrottleCount,
                        ref lastMemoryCheckUtc,
                        ref lastMemorySnapshot,
                        force: true);
                    successStreak = 0;
                    if (!stillMemoryPressure)
                    {
                        var oldLimit = currentLimit;
                        currentLimit = Math.Min(settings.Maximum, currentLimit + Math.Max(1, (int)Math.Ceiling(currentLimit * 0.25)));
                        Console.WriteLine($"  [LLM:pool] concurrency increased {oldLimit} -> {currentLimit}");
                    }
                }
                continue;
            }

            successStreak = 0;
            if (result.HasPressureSignal)
            {
                pressureFailureStreak++;
                if (pressureFailureStreak >= settings.FailureStreakToDecrease && currentLimit > settings.Minimum)
                {
                    var oldLimit = currentLimit;
                    currentLimit = Math.Max(settings.Minimum, currentLimit / 2);
                    pressureFailureStreak = 0;
                    Console.WriteLine($"  [LLM:pool] concurrency decreased {oldLimit} -> {currentLimit}");
                }
            }
        }

        var unscheduledItems = pending.ToList();
        foreach (var item in unscheduledItems)
            item.CleanupPromptFile();

        return new LlmTaskPoolResult(
            settings.Initial,
            currentLimit,
            maxObservedConcurrency,
            retriedAttemptCount,
            memoryThrottleCount,
            results,
            unscheduledItems,
            stopReason);
    }

    private async Task<LlmTaskPoolResult> ExecuteWorkItemsFixedWindowAsync(
        List<LlmTranslationWorkItem> workItems,
        int windowSize)
    {
        const int maxRetries = 3;
        using var ownedHandler = _httpClient == null
            ? new SocketsHttpHandler { MaxConnectionsPerServer = Math.Max(1, windowSize) }
            : null;
        using var ownedClient = _httpClient == null
            ? new HttpClient(ownedHandler!)
            : null;
        var client = _httpClient ?? ownedClient!;
        if (ownedClient != null)
            client.Timeout = TimeSpan.FromSeconds(Math.Max(30, _config.llmRequestTimeoutSeconds));

        var totalWorkItems = workItems.Count;
        var results = new List<LlmWorkResult>();
        var retriedAttemptCount = 0;
        var fixedSettings = new LlmConcurrencySettings(
            windowSize, windowSize, 1, maxRetries, 3, 1000, 60000,
            "fixed-window", null, Environment.OSVersion.Platform.ToString());
        var windowIndex = 0;
        var totalWindows = (int)Math.Ceiling((double)workItems.Count / windowSize);
        Console.WriteLine($"  [LLM:fixed] concurrency={windowSize}, windows={totalWindows}, items={totalWorkItems}");

        for (var offset = 0; offset < workItems.Count; offset += windowSize)
        {
            windowIndex++;
            var window = workItems.Skip(offset).Take(windowSize).ToList();
            var windowStarted = Stopwatch.GetTimestamp();
            var windowTasks = window.Select(item => TranslateWorkItemWithRetriesAsync(client, item, fixedSettings)).ToList();
            var windowResults = await Task.WhenAll(windowTasks);
            var windowElapsed = Stopwatch.GetElapsedTime(windowStarted);

            var windowOk = 0;
            var windowFail = 0;
            foreach (var result in windowResults)
            {
                results.Add(result);
                retriedAttemptCount += Math.Max(0, result.AttemptCount - 1);
                if (result.IsSuccess) windowOk++; else windowFail++;
                Console.WriteLine(
                    $"  [LLM] {results.Count}/{totalWorkItems}: batch {result.WorkItem.BatchId}/{result.WorkItem.TotalBatches} | {(result.IsSuccess ? "OK" : "FAIL")} | {result.Elapsed.TotalSeconds:F1}s | attempts={result.AttemptCount}");
            }
            Console.WriteLine($"  [LLM:fixed] window {windowIndex}/{totalWindows}: ok={windowOk} fail={windowFail} | {windowElapsed.TotalSeconds:F1}s");
        }

        Console.WriteLine($"  [LLM:fixed] done: {results.Count}/{totalWorkItems} items, retriedAttempts={retriedAttemptCount}");
        return new LlmTaskPoolResult(
            windowSize, windowSize, windowSize,
            retriedAttemptCount, 0, results, [], null);
    }

    private async Task<LlmWarmupResult> ExecuteWarmupAsync(
        LlmTranslationPlan plan,
        LlmConcurrencySettings settings)
    {
        using var ownedHandler = _httpClient == null
            ? new SocketsHttpHandler { MaxConnectionsPerServer = 1 }
            : null;
        using var ownedClient = _httpClient == null
            ? new HttpClient(ownedHandler!)
            : null;
        var client = _httpClient ?? ownedClient!;
        if (ownedClient != null)
            client.Timeout = TimeSpan.FromSeconds(Math.Max(30, _config.llmRequestTimeoutSeconds));

        return await SendWarmupWithRetriesAsync(client, plan, settings);
    }

    private async Task<LlmWarmupResult> SendWarmupWithRetriesAsync(
        HttpClient client,
        LlmTranslationPlan plan,
        LlmConcurrencySettings settings)
    {
        var maxAttempts = settings.MaxRetries + 1;
        var started = Stopwatch.GetTimestamp();
        Exception? lastException = null;
        var hadPressureSignal = false;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var prompt = await Utf8NoBom.ReadAllTextAsync(plan.WarmupPromptPath);
                var (responseText, _) = await SendChatJsonModeAsync(client, prompt);
                ValidateWarmupResponse(responseText);
                plan.CleanupWarmupPromptFile();
                Console.WriteLine($"  [LLM:warmup] target={plan.TargetLang} | OK | attempts={attempt} | {Stopwatch.GetElapsedTime(started).TotalSeconds:F1}s");
                return LlmWarmupResult.Success(attempt, Stopwatch.GetElapsedTime(started), hadPressureSignal);
            }
            catch (Exception ex)
            {
                lastException = ex;
                hadPressureSignal |= IsPressureFailure(ex);
                var accountFatal = IsAccountFatal(ex);
                var shouldRetry = !accountFatal && attempt < maxAttempts && ShouldRetry(ex);
                var state = shouldRetry ? "RETRY" : "FAIL";
                Console.WriteLine($"  [LLM:warmup] target={plan.TargetLang} | {state} attempt={attempt}/{maxAttempts} | {ex.GetType().Name}: {ex.Message}");

                if (accountFatal)
                {
                    plan.CleanupWarmupPromptFile();
                    return LlmWarmupResult.AccountFatal(attempt, Stopwatch.GetElapsedTime(started), ex, hadPressureSignal);
                }

                if (!shouldRetry)
                {
                    plan.CleanupWarmupPromptFile();
                    return LlmWarmupResult.Failed(attempt, Stopwatch.GetElapsedTime(started), ex, hadPressureSignal);
                }

                await Task.Delay(CalculateRetryDelay(ex, attempt, settings));
            }
        }

        plan.CleanupWarmupPromptFile();
        return LlmWarmupResult.Failed(maxAttempts, Stopwatch.GetElapsedTime(started), lastException, hadPressureSignal);
    }

    private static bool TryApplyMemoryThrottle(
        LlmConcurrencySettings settings,
        ref int currentLimit,
        ref int memoryThrottleCount,
        ref DateTime lastMemoryCheckUtc,
        ref LlmMemorySnapshot? lastMemorySnapshot,
        bool force = false)
    {
        var now = DateTime.UtcNow;
        if (!force && now - lastMemoryCheckUtc < MemoryPressureCheckInterval)
            return IsMemoryPressure(lastMemorySnapshot);

        lastMemoryCheckUtc = now;
        if (!TryReadMemorySnapshot(out var snapshot))
            return false;

        lastMemorySnapshot = snapshot;
        if (!IsMemoryPressure(snapshot))
            return false;

        var oldLimit = currentLimit;
        var newLimit = snapshot.IsCritical
            ? settings.Minimum
            : Math.Max(settings.Minimum, currentLimit / 2);
        currentLimit = Math.Max(settings.Minimum, Math.Min(currentLimit, newLimit));

        if (currentLimit < oldLimit)
        {
            memoryThrottleCount++;
            Console.WriteLine(
                $"  [LLM:pool] memory pressure: concurrency decreased {oldLimit} -> {currentLimit} | available={FormatBytes(snapshot.AvailableBytes)} ({snapshot.AvailableRatio:P0}) | load={FormatBytes(snapshot.MemoryLoadBytes)}/{FormatBytes(snapshot.TotalAvailableBytes)} | workingSet={FormatBytes(snapshot.ProcessWorkingSetBytes)}");
        }

        return true;
    }

    private static bool TryReadMemorySnapshot(out LlmMemorySnapshot snapshot)
    {
        snapshot = default!;
        try
        {
            var info = GC.GetGCMemoryInfo();
            var totalAvailable = info.TotalAvailableMemoryBytes;
            if (totalAvailable <= 0)
                return false;

            var memoryLoad = Math.Clamp(info.MemoryLoadBytes, 0, totalAvailable);
            var available = Math.Max(0, totalAvailable - memoryLoad);
            var processWorkingSet = Process.GetCurrentProcess().WorkingSet64;
            var ratio = totalAvailable == 0 ? 1.0 : available / (double)totalAvailable;
            snapshot = new LlmMemorySnapshot(totalAvailable, memoryLoad, available, ratio, processWorkingSet);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsMemoryPressure(LlmMemorySnapshot? snapshot)
    {
        return snapshot != null && (snapshot.IsLow || snapshot.IsCritical);
    }

    private static string FormatBytes(long bytes)
    {
        const double kb = 1024.0;
        const double mb = kb * 1024.0;
        const double gb = mb * 1024.0;

        return bytes switch
        {
            >= (long)gb => $"{bytes / gb:F1} GiB",
            >= (long)mb => $"{bytes / mb:F0} MiB",
            >= (long)kb => $"{bytes / kb:F0} KiB",
            _ => $"{bytes} B"
        };
    }

    private async Task<LlmWorkResult> TranslateWorkItemWithRetriesAsync(
        HttpClient client,
        LlmTranslationWorkItem item,
        LlmConcurrencySettings settings)
    {
        var maxAttempts = settings.MaxRetries + 1;
        var started = Stopwatch.GetTimestamp();
        Exception? lastException = null;
        var hadPressureSignal = false;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var prompt = await Utf8NoBom.ReadAllTextAsync(item.PromptPath);
                var (responseText, reasoning) = await SendChatJsonModeAsync(client, prompt);
                WriteResponseDebug(item, responseText);
                WriteReasoningDebug(item, reasoning);
                var parsed = ParseTranslationResponse(responseText, item.Entries);
                item.CleanupPromptFile();
                return LlmWorkResult.Success(item, parsed, attempt, Stopwatch.GetElapsedTime(started), hadPressureSignal);
            }
            catch (Exception ex)
            {
                lastException = ex;
                hadPressureSignal |= IsPressureFailure(ex);
                var accountFatal = IsAccountFatal(ex);
                var shouldRetry = !accountFatal && attempt < maxAttempts && ShouldRetry(ex);
                var state = shouldRetry ? "RETRY" : "FAIL";
                Console.WriteLine($"  [LLM] target={item.TargetLang} batch {item.BatchId}/{item.TotalBatches} | {state} attempt={attempt}/{maxAttempts} | {ex.GetType().Name}: {ex.Message}");

                if (accountFatal)
                {
                    GitHubActions.Warning($"LLM account-level failure for target {item.TargetLang}, batch {item.BatchId}: {ex.Message}", "LLMTranslator");
                    item.CleanupPromptFile();
                    return LlmWorkResult.AccountFatal(item, attempt, Stopwatch.GetElapsedTime(started), ex, hadPressureSignal);
                }

                if (!shouldRetry)
                {
                    item.CleanupPromptFile();
                    return LlmWorkResult.Failed(item, attempt, Stopwatch.GetElapsedTime(started), ex, hadPressureSignal);
                }

                await Task.Delay(CalculateRetryDelay(ex, attempt, settings));
            }
        }

        item.CleanupPromptFile();
        return LlmWorkResult.Failed(item, maxAttempts, Stopwatch.GetElapsedTime(started), lastException, hadPressureSignal);
    }

    private async Task<(string content, string? reasoning)> SendChatJsonModeAsync(HttpClient client, string prompt)
    {
        return await SendChatAsync(client, prompt);
    }

    private async Task<(string content, string? reasoning)> SendChatAsync(HttpClient client, string prompt)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, _config.llmApiEndpoint);
        if (!string.IsNullOrWhiteSpace(_config.llmKey))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.llmKey);

        var body = new Dictionary<string, object?>
        {
            ["model"] = _config.llmModel,
            ["messages"] = new[] { new { role = "system", content = prompt } },
            ["temperature"] = _config.llmTemperature,
            ["max_tokens"] = _config.llmMaxTokens
        };

        request.Content = new StringContent(Utf8NoBom.SerializeJson(body), Utf8NoBom.Encoding, "application/json");

        var apiStart = Stopwatch.GetTimestamp();
        using var response = await client.SendAsync(request);
        var apiLatency = Stopwatch.GetElapsedTime(apiStart);
        var responseBody = await response.Content.ReadAsStringAsync();
        var verboseApiLog = IsVerboseApiLogEnabled();
        if (verboseApiLog)
            Console.WriteLine($"    [LLM:api] {apiLatency.TotalSeconds:F1}s | resp_len={responseBody.Length} | status={(int)response.StatusCode}");

        if (!response.IsSuccessStatusCode)
            throw new LlmApiException(response.StatusCode, responseBody, ReadRetryAfter(response));

        using var doc = JsonDocument.Parse(responseBody);
        var choices = doc.RootElement.GetProperty("choices");
        if (choices.ValueKind != JsonValueKind.Array || choices.GetArrayLength() == 0)
            throw new InvalidDataException("LLM response missing choices.");

        if (doc.RootElement.TryGetProperty("usage", out var usage))
        {
            var promptTokens = usage.TryGetProperty("prompt_tokens", out var pt) ? pt.GetInt32() : 0;
            var completionTokens = usage.TryGetProperty("completion_tokens", out var ct) ? ct.GetInt32() : 0;
            if (verboseApiLog)
                Console.WriteLine($"    [LLM:api] tokens: prompt={promptTokens} completion={completionTokens}");
        }

        var message = choices[0].GetProperty("message");
        string? reasoning = null;
        if (message.TryGetProperty("reasoning_content", out var rct) && rct.ValueKind == JsonValueKind.String)
            reasoning = rct.GetString();
        if (string.IsNullOrWhiteSpace(reasoning))
            reasoning = null;

        if (message.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.String)
        {
            var text = content.GetString();
            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidDataException("LLM response message content was empty.");
            return (text, reasoning);
        }

        throw new InvalidDataException("LLM response missing message content.");
    }

    // ── Ref: response_parser.py:parse_response + _validate_entry ──
    // New format: tab-separated plain text per line "Tn\t<translation>\t<confidence>\t[comment]"
    private static Dictionary<string, (string text, float? confidence, string? comment)> ParseTranslationResponse(
        string responseText,
        List<TranslationEntry> expectedEntries)
    {
        var lines = ParseTranslationLines(responseText);
        if (lines.Count != expectedEntries.Count)
            throw new InvalidDataException($"LLM entry count mismatch: expected {expectedEntries.Count}, got {lines.Count}.");

        var result = new Dictionary<string, (string text, float? confidence, string? comment)>(StringComparer.Ordinal);
        for (var i = 0; i < lines.Count; i++)
        {
            var fields = lines[i].Split('\t');
            if (fields.Length < 3)
            {
                // Fallback: LLM may use spaces instead of tabs; also handles Tn conf (missing empty translation)
                var altFields = System.Text.RegularExpressions.Regex.Split(lines[i], @"\s+");
                if (altFields.Length >= 3)
                    fields = altFields;
                else if (altFields.Length == 2)
                    fields = new[] { altFields[0], "", altFields[1] };
                else
                    throw new InvalidDataException($"LLM line[{i + 1}] missing fields.");
            }

            var idxPart = fields[0].Trim();
            if (!idxPart.StartsWith("T") || !int.TryParse(idxPart.AsSpan(1), out var tIdx) || tIdx != i + 1)
                throw new InvalidDataException($"LLM line[{i + 1}] index mismatch: expected T{i + 1}, got '{idxPart}'.");

            var translation = fields[1].Trim();
            if (!float.TryParse(fields[2].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var confidence))
                throw new InvalidDataException($"LLM line[{i + 1}] confidence is not numeric: '{fields[2]}'.");

            string? comment = null;
            const float conflictConfidence = -1.0f;
            if (Math.Abs(confidence - conflictConfidence) < 0.0001f)
            {
                if (fields.Length >= 4 && !string.IsNullOrWhiteSpace(fields[3]))
                    comment = fields[3].Trim();
                else
                    Console.WriteLine($"  [LLM:warn] line[{i + 1}] confidence=-1.0 missing comment; accepted.");
            }

            var entry = expectedEntries[i];
            result[entry.translationKey] = (translation, confidence, comment);
        }

        return result;
    }

    private static List<string> ParseTranslationLines(string responseText)
    {
        var text = responseText.Trim();
        // Strip optional ``` fences
        if (text.StartsWith("```", StringComparison.Ordinal) && text.EndsWith("```", StringComparison.Ordinal))
        {
            var inner = text.AsSpan(3, text.Length - 3 - 3).Trim();
            var withoutTag = TrimLangTag(inner);
            text = withoutTag.ToString();
        }
        var rawLines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .ToList();
        // Reassemble: merge continuation lines (non-Tn) into previous Tn-line; strict Tn detection
        var merged = new List<string>();
        foreach (var line in rawLines)
        {
            if (line.Length > 1 && line[0] == 'T' && char.IsDigit(line[1]))
                merged.Add(line);
            else if (merged.Count > 0)
                merged[merged.Count - 1] += " " + line;
        }
        return merged;
    }

    private static ReadOnlySpan<char> TrimLangTag(ReadOnlySpan<char> span)
    {
        var nl = span.IndexOf('\n');
        return nl >= 0 ? span[(nl + 1)..] : span;
    }

    private static void ValidateWarmupResponse(string responseText)
    {
        var lines = ParseTranslationLines(responseText);
        // Warmup validates model connectivity; any valid tab-separated response is sufficient.
    }

    private List<Dictionary<string, object?>> BuildPromptItems(
        List<TranslationEntry> entries,
        Dictionary<string, List<Dictionary<string, object?>>> ragContextByEntryKey,
        string targetLang)
    {
        return entries.Select(entry =>
        {
            var source = entry.GetBaseTextStrict(_config.baseLanguage);
            var sourceKind = string.IsNullOrWhiteSpace(source.text)
                ? "key_only"
                : "base_text";
            return new Dictionary<string, object?>
            {
                ["key"] = entry.translationKey,
                ["key_hint"] = entry.translationKey,
                ["source_kind"] = sourceKind,
                [source.lang] = source.text,
                ["multi_lang_context"] = BuildTargetLangContext(entry, targetLang),
                ["rag_context"] = ragContextByEntryKey.GetValueOrDefault(BuildEntryKey(entry), [])
            };
        }).ToList();
    }

    private static List<Dictionary<string, object?>> BuildTargetLangContext(TranslationEntry entry, string targetLang)
    {
        var contexts = new List<Dictionary<string, object?>>();
        if (!entry.translationValues.TryGetValue(targetLang, out var data)
            || string.IsNullOrWhiteSpace(data.text))
            return contexts;

        contexts.Add(new Dictionary<string, object?>
        {
            [targetLang] = data.text,
            ["verified"] = data.isVerified || string.Equals(data.status, "verified", StringComparison.OrdinalIgnoreCase)
        });
        return contexts;
    }

    // ── Ref: prompt_builder.py:build_prompt with glossary injected ──
    private string BuildPrompt(
        TranslationBatch batch,
        List<Dictionary<string, object?>> promptItems,
        ModInfo modInfo,
        string targetLanguage)
    {
        var modPayload = new Dictionary<string, object?>
        {
            ["mod_id"] = batch.modId,
            ["mod_name"] = modInfo.modName,
            ["mod_author"] = modInfo.creator,
            ["mod_description"] = modInfo.description
        };

        return string.Join('\n', new[]
        {
            BuildFixedPromptPrefix(targetLanguage),
            "# Mod Info\n" + Utf8NoBom.SerializeJson(modPayload),
            BuildInputSection(promptItems),
            PromptTailReminder
        });
    }

    private string BuildWarmupPrompt(string targetLanguage)
    {
        return BuildFixedPromptPrefix(targetLanguage);
    }

    internal string BuildPromptForItems(List<Dictionary<string, object?>> promptItems)
    {
        return BuildPromptForItems(promptItems, _config.priorityLanguage);
    }

    internal string BuildPromptForItems(List<Dictionary<string, object?>> promptItems, string targetLanguage)
    {
        return string.Join('\n', new[]
        {
            BuildFixedPromptPrefix(targetLanguage),
            BuildInputSection(promptItems),
            PromptTailReminder
        });
    }

    private string BuildFixedPromptPrefix(string targetLanguage)
    {
        var targetLangName = ResolveFileSafeLanguageName(targetLanguage);
        var targetDisplayName = ResolveTargetLanguageDisplayName(targetLanguage);
        var parts = new List<string>
        {
            LoadTemplate("system_prompt_translate_engine.txt").Replace("{{TARGET_LANG}}", targetDisplayName, StringComparison.Ordinal),
        };

        var schema = LoadTemplate(Path.Combine(targetLangName, $"translation_schema_{targetLangName}.md"));
        if (!string.IsNullOrWhiteSpace(schema))
            parts.Add("# Translation Rules\n" + schema);

        var terminologyText = BuildTerminologySection(targetLanguage);
        if (!string.IsNullOrWhiteSpace(terminologyText))
            parts.Add(terminologyText);

        var outputRules = LoadTemplate("translation_output.md");
        if (!string.IsNullOrWhiteSpace(outputRules))
            parts.Add("# Output Rules\n" + outputRules);

        return string.Join('\n', parts);
    }

    private static string BuildInputSection(List<Dictionary<string, object?>> promptItems)
    {
        // ── Collect & dedup all RAG contexts across the batch ──
        var ragDedup = new Dictionary<string, (string key, string lang, string text, string translation, bool verified)>(StringComparer.Ordinal);
        var ragOrder = new List<string>(); // dedup keys in encounter order
        foreach (var item in promptItems)
        {
            if (!item.TryGetValue("rag_context", out var raw) || raw is not List<Dictionary<string, object?>> ragList)
                continue;
            foreach (var rag in ragList)
            {
                if (!TryGetRagFields(rag, out var fields))
                    continue;
                if (string.IsNullOrWhiteSpace(fields.text) || string.IsNullOrWhiteSpace(fields.translation))
                    continue;
                var dk = $"{fields.key}\x00{fields.text}\x00{fields.translation}";
                if (ragDedup.ContainsKey(dk))
                    continue;
                ragDedup[dk] = fields;
                ragOrder.Add(dk);
            }
        }

        // Assign R-indices
        var dkToRidx = new Dictionary<string, int>(StringComparer.Ordinal);
        for (int i = 0; i < ragOrder.Count; i++)
            dkToRidx[ragOrder[i]] = i + 1;

        var sb = new StringBuilder();
        sb.Append("# Input Count\n");
        sb.Append(promptItems.Count);
        sb.Append('\n');

        // ── RAG Result section ──
        if (ragOrder.Count > 0)
        {
            sb.Append("# RAG Result\n");
            foreach (var dk in ragOrder)
            {
                var f = ragDedup[dk];
                sb.Append('R');
                sb.Append(dkToRidx[dk]);
                sb.Append('\t');
                sb.Append(EscapeTab(f.key));
                sb.Append('\t');
                sb.Append(f.lang);
                sb.Append('\t');
                sb.Append(EscapeTab(f.text));
                sb.Append('\t');
                sb.Append(EscapeTab(f.translation));
                sb.Append('\t');
                sb.Append(f.verified ? "true" : "false");
                sb.Append('\n');
            }
        }

        // ── Translation Entry section ──
        sb.Append("# Translation Entry\n");
        for (int i = 0; i < promptItems.Count; i++)
        {
            var item = promptItems[i];
            var key = item.TryGetValue("key", out var k) ? (k as string ?? "") : "";
            var (srcLang, srcText) = FindSourceField(item);
            var (refText, refVerified) = FindReferenceTranslation(item);

            sb.Append('T');
            sb.Append(i + 1);
            sb.Append('\t');
            sb.Append(EscapeTab(key));
            sb.Append('\t');
            sb.Append(srcLang);
            sb.Append('\t');
            sb.Append(EscapeTab(srcText));
            sb.Append('\t');
            sb.Append(EscapeTab(refText));
            sb.Append('\t');
            sb.Append(refVerified ? "true" : "false");

            // rag refs (max 3)
            if (item.TryGetValue("rag_context", out var raw2) && raw2 is List<Dictionary<string, object?>> ragList2 && ragList2.Count > 0)
            {
                var ragRefs = new List<int>();
                foreach (var rag in ragList2)
                {
                    if (!TryGetRagFields(rag, out var rf))
                        continue;
                    if (string.IsNullOrWhiteSpace(rf.text) || string.IsNullOrWhiteSpace(rf.translation))
                        continue;
                    var dk = $"{rf.key}\x00{rf.text}\x00{rf.translation}";
                    if (dkToRidx.TryGetValue(dk, out var ridx) && ridx > 0)
                        ragRefs.Add(ridx);
                }
                // dedup & limit to 3
                ragRefs = ragRefs.Distinct().Take(3).ToList();
                ragRefs.Sort();
                if (ragRefs.Count > 0)
                {
                    sb.Append('\t');
                    sb.Append(string.Join(" ", ragRefs.Select(r => $"R{r}")));
                }
            }

            sb.Append('\n');
        }

        return sb.ToString();
    }

    private static bool TryGetRagFields(Dictionary<string, object?> rag, out (string key, string lang, string text, string translation, bool verified) fields)
    {
        fields = default;
        if (!rag.TryGetValue("key", out var keyObj) || keyObj is not string key)
            return false;

        // Find source lang field (not mod_id, key, translation, verified, score)
        string? lang = null;
        string? text = null;
        foreach (var kv in rag)
        {
            if (kv.Key is "mod_id" or "key" or "translation" or "verified" or "score")
                continue;
            if (kv.Value is string s && !string.IsNullOrWhiteSpace(s))
            {
                lang = kv.Key;
                text = s;
                break;
            }
        }
        if (lang == null || text == null)
            return false;

        if (!rag.TryGetValue("translation", out var transObj) || transObj is not string translation)
            return false;

        var verified = rag.TryGetValue("verified", out var vObj) && vObj is bool b && b;

        fields = (key, lang, text, translation, verified);
        return true;
    }

    private static (string lang, string text) FindSourceField(Dictionary<string, object?> item)
    {
        foreach (var kv in item)
        {
            if (kv.Key is "key" or "key_hint" or "source_kind" or "multi_lang_context" or "rag_context")
                continue;
            if (kv.Value is string s)
                return (kv.Key, s ?? "");
        }
        return ("", "");
    }

    private static (string text, bool verified) FindReferenceTranslation(Dictionary<string, object?> item)
    {
        if (!item.TryGetValue("multi_lang_context", out var raw) || raw is not List<Dictionary<string, object?>> mlc || mlc.Count == 0)
            return ("", false);
        var first = mlc[0];
        var verified = first.TryGetValue("verified", out var vObj) && vObj is bool b && b;
        foreach (var kv in first)
        {
            if (kv.Key == "verified")
                continue;
            if (kv.Value is string s)
                return (s ?? "", verified);
        }
        return ("", verified);
    }

    private static string EscapeTab(string s)
    {
        return s.Contains('\t') ? s.Replace("\t", "<TAB_MARK>") : s;
    }

    // ── Ref: glossary_prompt_builder.py — full target dictionary kept in the cacheable prefix ──
    private string BuildTerminologySection(string targetLanguage)
    {
        var dictionaryEntries = LoadDictionary(targetLanguage);
        if (dictionaryEntries == null || dictionaryEntries.Length == 0)
            return "";

        var sourceLang = FindSourceLanguageKey(dictionaryEntries);
        var lines = new List<string> { $"# Terminology\n```\n{sourceLang} {targetLanguage}" };
        foreach (var entry in dictionaryEntries)
        {
            if (entry.TryGetValue(sourceLang, out var src) && entry.TryGetValue("translated", out var tgt)
                && !string.IsNullOrWhiteSpace(src) && !string.IsNullOrWhiteSpace(tgt))
                lines.Add($"{src}\t{tgt}");
        }
        lines.Add("```");
        return string.Join('\n', lines);
    }

    private static string FindSourceLanguageKey(Dictionary<string, string>[] entries)
    {
        if (entries.Length > 0)
        {
            foreach (var kv in entries[0])
                if (!string.Equals(kv.Key, "translated", StringComparison.OrdinalIgnoreCase))
                    return kv.Key;
        }
        return "en";
    }

    // ── Ref: prompt_builder.py:load_dictionary ──
    private Dictionary<string, string>[]? LoadDictionary(string targetLanguage)
    {
        var targetLang = NormalizeLanguage(targetLanguage);
        if (_dictionaryCacheByTargetLang.TryGetValue(targetLang, out var cached))
            return cached;

        try
        {
            var targetLangName = ResolveFileSafeLanguageName(targetLanguage);
            var dictPath = Path.Combine(_config.baseDir, "src", "prompt_templates", targetLangName,
                $"translation_dictionary_{targetLangName}.json");
            if (!File.Exists(dictPath))
            {
                _dictionaryCacheByTargetLang[targetLang] = null;
                return null;
            }

            var json = Utf8NoBom.ReadAllText(dictPath);
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>[]>(json);
            _dictionaryCacheByTargetLang[targetLang] = dictionary;
            return dictionary;
        }
        catch
        {
            _dictionaryCacheByTargetLang[targetLang] = null;
            return null;
        }
    }

    private string LoadTemplate(string relativePath)
    {
        var basePath = Path.Combine(_config.baseDir, "src", "prompt_templates", relativePath);
        if (File.Exists(basePath))
            return Utf8NoBom.ReadAllText(basePath).Trim();
        return "";
    }

    private string ResolveFileSafeLanguageName(string targetLanguage)
    {
        var matched = _config.supportedLanguages.FirstOrDefault(lang =>
            string.Equals(lang.ingameCode, targetLanguage, StringComparison.OrdinalIgnoreCase)
            || string.Equals(lang.isoCode, targetLanguage, StringComparison.OrdinalIgnoreCase));
        return string.IsNullOrWhiteSpace(matched?.isoCode)
            ? targetLanguage.ToLowerInvariant()
            : matched.isoCode.ToLowerInvariant();
    }

    private string ResolveTargetLanguageDisplayName(string targetLanguage)
    {
        var matched = _config.supportedLanguages.FirstOrDefault(lang =>
            string.Equals(lang.ingameCode, targetLanguage, StringComparison.OrdinalIgnoreCase)
            || string.Equals(lang.isoCode, targetLanguage, StringComparison.OrdinalIgnoreCase));
        if (matched == null)
            return targetLanguage;

        var englishName = string.IsNullOrWhiteSpace(matched.englishName)
            ? matched.isoCode
            : matched.englishName;
        return $"{englishName} ({matched.isoCode})";
    }

    private string NormalizeLanguage(string language)
    {
        var matched = _config.supportedLanguages.FirstOrDefault(lang =>
            string.Equals(lang.ingameCode, language, StringComparison.OrdinalIgnoreCase)
            || string.Equals(lang.isoCode, language, StringComparison.OrdinalIgnoreCase));
        return string.IsNullOrWhiteSpace(matched?.isoCode)
            ? language.ToLowerInvariant()
            : matched.isoCode.ToLowerInvariant();
    }

    private void ApplyTargetWrite(LlmTargetWrite targetWrite)
    {
        var targetData = targetWrite.Entry.translationValues.GetValueOrDefault(targetWrite.TargetLang) ?? new TranslationData();
        // Every text entering the translation database must be in the canonical
        // PZ Build 42.20.1+ percent format.
        targetData.text = PercentNormalizerService.Normalize(targetWrite.Text);
        targetData.confidence = targetWrite.Confidence;
        targetData.status = targetWrite.Status;
        targetData.isVerified = string.Equals(targetWrite.Status, "verified", StringComparison.OrdinalIgnoreCase);
        targetData.processStatus = "processed";
        if (!string.IsNullOrWhiteSpace(targetWrite.Comment))
            targetData.comments.Add(targetWrite.Comment);
        targetWrite.Entry.translationValues[targetWrite.TargetLang] = targetData;
    }

    private LlmPromptFile WritePromptFile(TranslationBatch batch, string prompt, string targetLanguage)
    {
        var deleteAfterUse = string.IsNullOrWhiteSpace(_config.runTempDir);
        var root = deleteAfterUse ? _fallbackPromptRoot : Path.Combine(_config.runTempDir, "prompts");
        var promptRoot = (deleteAfterUse || ShouldUseTargetLanguageSubdir())
            ? Path.Combine(root, NormalizeLanguage(targetLanguage))
            : root;
        var promptDir = Path.Combine(promptRoot, SafePathSegment(batch.modId));
        Directory.CreateDirectory(promptDir);
        var path = Path.Combine(promptDir, $"prompt_{batch.batchId:000}.md");
        Utf8NoBom.WriteAllText(path, prompt);
        return new LlmPromptFile(path, deleteAfterUse);
    }

    private LlmPromptFile WriteWarmupPromptFile(string prompt, string targetLanguage)
    {
        var deleteAfterUse = string.IsNullOrWhiteSpace(_config.runTempDir);
        var root = deleteAfterUse ? _fallbackPromptRoot : Path.Combine(_config.runTempDir, "prompts");
        var promptRoot = (deleteAfterUse || ShouldUseTargetLanguageSubdir())
            ? Path.Combine(root, NormalizeLanguage(targetLanguage))
            : root;
        Directory.CreateDirectory(promptRoot);
        var path = Path.Combine(promptRoot, "warmup.md");
        Utf8NoBom.WriteAllText(path, prompt);
        return new LlmPromptFile(path, deleteAfterUse);
    }

    private void WriteResponseDebug(LlmTranslationWorkItem item, string responseText)
    {
        if (string.IsNullOrWhiteSpace(_config.runTempDir) || item.Entries.Count == 0) return;
        var responseRoot = ShouldUseTargetLanguageSubdir()
            ? Path.Combine(_config.runTempDir, "llm_responses", item.FileSafeLanguageName)
            : Path.Combine(_config.runTempDir, "llm_responses");
        var responseDir = Path.Combine(responseRoot, SafePathSegment(item.ModId));
        Directory.CreateDirectory(responseDir);
        var target = SafePathSegment(item.TargetLang);
        var key = SafePathSegment(item.Entries[0].translationKey);
        Utf8NoBom.WriteAllText(Path.Combine(responseDir, $"{target}_{item.BatchId:000}_{key}_{Guid.NewGuid():N}.json"), responseText);
    }

    private void WriteReasoningDebug(LlmTranslationWorkItem item, string? reasoning)
    {
        if (string.IsNullOrWhiteSpace(reasoning) || string.IsNullOrWhiteSpace(_config.runTempDir) || item.Entries.Count == 0) return;
        var reasoningRoot = ShouldUseTargetLanguageSubdir()
            ? Path.Combine(_config.runTempDir, "llm_responses", item.FileSafeLanguageName)
            : Path.Combine(_config.runTempDir, "llm_responses");
        var reasoningDir = Path.Combine(reasoningRoot, SafePathSegment(item.ModId));
        Directory.CreateDirectory(reasoningDir);
        var target = SafePathSegment(item.TargetLang);
        var key = SafePathSegment(item.Entries[0].translationKey);
        Utf8NoBom.WriteAllText(Path.Combine(reasoningDir, $"{target}_{item.BatchId:000}_{key}_{Guid.NewGuid():N}_reasoning.txt"), reasoning);
    }

    private static string SafePathSegment(string value, int maxLength = 120)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "unknown";

        var invalid = Path.GetInvalidFileNameChars();
        var builder = new StringBuilder(Math.Min(value.Length, maxLength));
        foreach (var c in value)
        {
            builder.Append(c == '\0' || char.IsControl(c) || invalid.Contains(c) ? '_' : c);
            if (builder.Length >= maxLength)
                break;
        }

        var safe = builder.ToString().Trim(' ', '.');
        return string.IsNullOrWhiteSpace(safe) ? "unknown" : safe;
    }

    private void WriteTranslationWarning(TranslationEntry entry, string targetLang, string message)
    {
        WarningFileWriter.Write(_config, "LLMTranslator", null, new PipelineWarning
        {
            ModuleName = "LLMTranslator",
            ModId = entry.modId,
            TargetLang = targetLang,
            ErrorType = "EmptyTranslation",
            Message = message
        });
    }

    private void WriteWorkFailureWarning(LlmTranslationWorkItem item, int attemptCount, Exception? ex)
    {
        WarningFileWriter.Write(
            _config,
            "LLMTranslator",
            $"batch_{item.BatchId:000}",
            new PipelineWarning
            {
                ModuleName = "LLMTranslator",
                BatchId = $"batch_{item.BatchId:000}",
                ModId = item.ModId,
                ModName = item.ModInfo.modName,
                TargetLang = item.TargetLang,
                AttemptCount = attemptCount,
                ErrorType = ex?.GetType().Name ?? "LLMTranslationFailed",
                Message = ex?.Message ?? "LLM translation failed."
            });
        GitHubActions.Warning($"LLM translation failed for target {item.TargetLang}, batch {item.BatchId}: {ex?.Message}", "LLMTranslator");
    }

    private void WriteWarmupWarning(LlmTranslationPlan plan, int attemptCount, Exception? ex)
    {
        WarningFileWriter.Write(
            _config,
            "LLMTranslator",
            $"warmup_{plan.TargetLang}",
            new PipelineWarning
            {
                ModuleName = "LLMTranslator",
                BatchId = "warmup",
                TargetLang = plan.TargetLang,
                AttemptCount = attemptCount,
                ErrorType = ex?.GetType().Name ?? "LLMWarmupFailed",
                Message = ex?.Message ?? "LLM warmup failed."
            });
        GitHubActions.Warning($"LLM warmup failed for target {plan.TargetLang}: {ex?.Message}", "LLMTranslator");
    }

    private void WriteTaskPoolStopWarning(int unscheduledCount, string? stopReason)
    {
        WarningFileWriter.Write(
            _config,
            "LLMTranslator",
            "task_pool",
            new PipelineWarning
            {
                ModuleName = "LLMTranslator",
                BatchId = "task_pool",
                ErrorType = "AccountFatalStop",
                Message = $"Stopped scheduling {unscheduledCount} LLM request(s). Reason: {stopReason ?? "unknown"}"
            });
        GitHubActions.Warning($"Stopped scheduling {unscheduledCount} LLM request(s): {stopReason}", "LLMTranslator");
    }

    private static bool ShouldRetry(Exception ex)
    {
        if (ex is LlmApiException api)
            return api.IsRetryable;

        return ex is TaskCanceledException
            or HttpRequestException
            or IOException
            or JsonException
            or InvalidDataException
            or KeyNotFoundException
            or InvalidOperationException;
    }

    private static bool IsAccountFatal(Exception ex)
    {
        return ex is LlmApiException { IsAccountFatal: true };
    }

    private static bool IsPressureFailure(Exception ex)
    {
        return ex is LlmApiException { IsPressureSignal: true }
            || ex is TaskCanceledException
            || ex is HttpRequestException
            || ex is IOException;
    }

    private static TimeSpan CalculateRetryDelay(Exception ex, int attempt, LlmConcurrencySettings settings)
    {
        if (ex is LlmApiException { RetryAfter: { } retryAfter } && retryAfter > TimeSpan.Zero)
            return retryAfter < TimeSpan.FromMilliseconds(settings.RetryMaxDelayMs)
                ? retryAfter
                : TimeSpan.FromMilliseconds(settings.RetryMaxDelayMs);

        var exponential = settings.RetryBaseDelayMs * Math.Pow(2, Math.Max(0, attempt - 1));
        var capped = Math.Min(settings.RetryMaxDelayMs, exponential);
        var jitter = Random.Shared.Next(0, Math.Max(1, (int)Math.Ceiling(capped * 0.25)));
        return TimeSpan.FromMilliseconds(capped + jitter);
    }

    private static TimeSpan? ReadRetryAfter(HttpResponseMessage response)
    {
        var retryAfter = response.Headers.RetryAfter;
        if (retryAfter?.Delta != null)
            return retryAfter.Delta.Value;

        if (retryAfter?.Date != null)
        {
            var delta = retryAfter.Date.Value - DateTimeOffset.UtcNow;
            return delta > TimeSpan.Zero ? delta : TimeSpan.Zero;
        }

        return null;
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

    private static string BuildEntryKey(TranslationEntry entry) => $"{entry.modId}::{entry.translationKey}";

    private static bool NeedsTargetProcessing(TranslationEntry entry, string targetLang)
    {
        if (!entry.translationValues.TryGetValue(targetLang, out var data))
            return true;
        if (data.IsProcessed && !string.IsNullOrWhiteSpace(data.text))
            return false;
        return true;
    }

    private bool ShouldLogBatchResult(int index, int total, int warningCount)
    {
        if (warningCount > 0
            || total <= VerboseBatchResultLimit
            || index == 1
            || index == total
            || index % BatchResultLogInterval == 0)
            return true;

        lock (_progressLogLock)
            return DateTime.UtcNow - _lastProgressLogUtc >= BatchResultLogIntervalTime;
    }

    private static bool IsVerboseApiLogEnabled()
    {
        var value = Environment.GetEnvironmentVariable("BABEL_VERBOSE_LLM_API_LOGS");
        return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
    }

    private sealed class LlmApiException : Exception
    {
        public LlmApiException(HttpStatusCode statusCode, string responseBody, TimeSpan? retryAfter)
            : base($"LLM API returned {(int)statusCode}: {responseBody}")
        {
            StatusCode = statusCode;
            RetryAfter = retryAfter;
        }

        public HttpStatusCode StatusCode { get; }
        public TimeSpan? RetryAfter { get; }
        public bool IsAccountFatal => StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.PaymentRequired;
        public bool IsRetryable => IsPressureSignal;
        public bool IsPressureSignal => StatusCode == HttpStatusCode.TooManyRequests || (int)StatusCode >= 500;
    }

    private sealed record LlmTaskPoolResult(
        int InitialConcurrency,
        int FinalConcurrency,
        int MaxObservedConcurrency,
        int RetriedAttemptCount,
        int MemoryThrottleCount,
        List<LlmWorkResult> Results,
        List<LlmTranslationWorkItem> UnscheduledItems,
        string? StopReason);

    private sealed record LlmPromptFile(string Path, bool DeleteAfterUse);

    private sealed record LlmMemorySnapshot(
        long TotalAvailableBytes,
        long MemoryLoadBytes,
        long AvailableBytes,
        double AvailableRatio,
        long ProcessWorkingSetBytes)
    {
        public bool IsLow =>
            AvailableBytes <= LowAvailableMemoryBytes
            || AvailableRatio <= LowAvailableMemoryRatio;

        public bool IsCritical =>
            AvailableBytes <= CriticalAvailableMemoryBytes
            || AvailableRatio <= CriticalAvailableMemoryRatio;
    }

    private sealed record LlmWorkResult(
        LlmTranslationWorkItem WorkItem,
        Dictionary<string, (string text, float? confidence, string? comment)>? Translations,
        int AttemptCount,
        TimeSpan Elapsed,
        Exception? Exception,
        bool HasPressureSignal,
        bool IsAccountFatal)
    {
        public bool IsSuccess => Exception == null && Translations != null;

        public static LlmWorkResult Success(
            LlmTranslationWorkItem workItem,
            Dictionary<string, (string text, float? confidence, string? comment)> translations,
            int attemptCount,
            TimeSpan elapsed,
            bool hasPressureSignal) =>
            new(workItem, translations, attemptCount, elapsed, null, hasPressureSignal, false);

        public static LlmWorkResult Failed(
            LlmTranslationWorkItem workItem,
            int attemptCount,
            TimeSpan elapsed,
            Exception? exception,
            bool hasPressureSignal) =>
            new(workItem, null, attemptCount, elapsed, exception, hasPressureSignal, false);

        public static LlmWorkResult AccountFatal(
            LlmTranslationWorkItem workItem,
            int attemptCount,
            TimeSpan elapsed,
            Exception exception,
            bool hasPressureSignal) =>
            new(workItem, null, attemptCount, elapsed, exception, hasPressureSignal, true);
    }

    private sealed record LlmWarmupResult(
        int AttemptCount,
        TimeSpan Elapsed,
        Exception? Exception,
        bool HasPressureSignal,
        bool IsAccountFatal)
    {
        public bool IsSuccess => Exception == null;

        public static LlmWarmupResult Success(
            int attemptCount,
            TimeSpan elapsed,
            bool hasPressureSignal) =>
            new(attemptCount, elapsed, null, hasPressureSignal, false);

        public static LlmWarmupResult Failed(
            int attemptCount,
            TimeSpan elapsed,
            Exception? exception,
            bool hasPressureSignal) =>
            new(attemptCount, elapsed, exception, hasPressureSignal, false);

        public static LlmWarmupResult AccountFatal(
            int attemptCount,
            TimeSpan elapsed,
            Exception exception,
            bool hasPressureSignal) =>
            new(attemptCount, elapsed, exception, hasPressureSignal, true);
    }
}
