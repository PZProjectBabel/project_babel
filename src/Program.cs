using ConfigReader;
using ContentChecker;
using ContentExtractor;
using EmbeddingFetcher;
using FinalOutputWriter;
using LLMTranslator;
using ModDownloader;
using ModIdCollector;
using ModInfoFetcher;
using RagContextRetriever;
using RepoDataLoader;
using ResultWriter;
using TranslationBatcher;
using Common;

Console.OutputEncoding = Utf8NoBom.Encoding;
Console.InputEncoding = Utf8NoBom.Encoding;

try
{
    var pipeline = new PipelineRunner();
    await pipeline.RunAsync();
}
catch (Exception ex)
{
    GitHubActions.Error(ex.Message, "Fatal pipeline error");
    if (!string.IsNullOrWhiteSpace(ex.StackTrace))
        Console.Error.WriteLine(ex.StackTrace);
    Environment.Exit(1);
}

/// <summary>
/// Translation Pipeline Runner - Orchestrates the execution of various modules in sequence.
/// </summary>
public class PipelineRunner
{
    /// <summary>Enable debug mode to limit mods and languages for fast testing.</summary>
    private const bool DebugOn = false;
    /// <summary>Maximum number of mods to process in debug mode.</summary>
    private const int DebugModLimit = 1400;
    /// <summary>Number of additional target languages in debug mode (beyond base + zh-hans).</summary>
    private const int DebugAdditionalTargetLanguageCount = 3;

    /// <summary>
    /// Executes all pipeline phases in sequence: Config → Reference mods → Collect IDs → Fetch info →
    /// Download/Extract → Content check → Embeddings → Batching → RAG + LLM → Write results → Final output.
    /// </summary>
    public async Task RunAsync()
    {
        int currentStep = 1;
        var allTaskResults = new List<TaskResult>();

        // --- Phase 1: Config ---
        Console.WriteLine($"[{currentStep++}] Reading and validating configuration...");
        var configReader = new ConfigReaderService();
        PipelineConfig config;
        try
        {
            config = configReader.LoadConfig(FindRepositoryRoot());
            if (DebugOn)
                ApplyDebugSupportedLanguageSubset(config, DebugAdditionalTargetLanguageCount);
            var targetLanguages = GetTargetLanguages(config);
            Console.WriteLine($"  [OK] Configuration ready: model={config.llmModel}, base={config.baseLanguage}, targets={string.Join(", ", targetLanguages.Select(lang => lang.isoCode))}");

            var steamCmdBootstrapper = new SteamCmdBootstrapper.SteamCmdBootstrapperService(config);
            await steamCmdBootstrapper.BootstrapAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Configuration failed.", ex);
        }
        var enabledTargetLanguages = GetTargetLanguages(config);
        var outputLanguages = GetOutputLanguages(config);
        int totalSteps = 15 + enabledTargetLanguages.Count * 2 + outputLanguages.Count; // base steps + per-target RAG/LLM + write outputs + final output + progress report

        // Init dictionaries.
        var refModInfoDict = config.referenceTranslationMods
            .Where(mod => !string.IsNullOrWhiteSpace(mod.modId))
            .Where(mod => !PipelineExclusions.IsExcluded(mod.modId))
            .GroupBy(mod => mod.modId, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);
        var modInfoDict = new Dictionary<string, ModInfo>(StringComparer.Ordinal);
        var refTranslationEntryDict = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        var translationEntryDict = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        var cachedTranslationEntryDict = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        var diffTranslationEntryDict = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        var translationBatches = new List<TranslationBatch>();
        var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);
        var baseGameTranslationKeys = new HashSet<string>(StringComparer.Ordinal);
        _ = baseGameTranslationKeys;
        Console.WriteLine();

        // --- Phase 2: Reference Translation Mods ---
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Loading & syncing reference translation mods...");
        var repoLoader = new RepoDataLoaderService(config);

        // 2a. Load ref cache.
        repoLoader.LoadRefModCache(refModInfoDict);
        repoLoader.LoadRefTranslationCache(refTranslationEntryDict);
        repoLoader.LoadRefEntryMetadataCache(refTranslationEntryDict);
        repoLoader.LoadRefEmbeddingCache(refTranslationEntryDict);
        MergeConfiguredRefMods(config.referenceTranslationMods, refModInfoDict);
        PurgeExcludedMod(PipelineExclusions.ProjectBabelWorkshopId, refModInfoDict, refTranslationEntryDict);
        MarkReferenceEntriesVerified(refTranslationEntryDict);

        // 2b. Fetch latest Steam state for ref mods.
        var refInfoFetcher = new ModInfoFetcherService(config);
        allTaskResults.Add(await refInfoFetcher.FetchModInfosAsync(refModInfoDict));
        var staleRefModIds = CollectStaleReferenceModIds(refModInfoDict, refTranslationEntryDict);

        // 2c. Download & extract & embedding for changed ref mods.
        if (staleRefModIds.Count > 0)
        {
            string refBatchFolder = Path.Combine(config.downloadingBatchesTempDir, "ref_batch");
            Directory.CreateDirectory(refBatchFolder);

            // Build a dict for just the stale mods.
            var staleRefDict = staleRefModIds
                .ToDictionary(id => id, id =>
                {
                    var m = refModInfoDict.TryGetValue(id, out var existing) ? existing : new ModInfo { modId = id };
                    m.needsUpdate = true;
                    return m;
                }, StringComparer.Ordinal);

            var refDownloader = new ModDownloaderService(config);
            allTaskResults.Add(await refDownloader.DownloadModsAsync(staleRefModIds.ToList(), staleRefDict, refBatchFolder));
            foreach (var (modId, info) in staleRefDict)
                refModInfoDict[modId] = info;

            // Snapshot cached entries for stale mods before extraction, so unchanged texts can keep their embeddings.
            var staleRefEntrySnapshot = refTranslationEntryDict
                .Where(kvp => staleRefModIds.Contains(kvp.Value.modId))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.Ordinal);

            var refConfig = new PipelineConfig
            {
                baseDir = config.baseDir,
                extractedContentsTempDir = config.extractedReferencesTempDir,
                runTempDir = config.runTempDir,
                warningsTempDir = config.warningsTempDir,
                baseLanguage = config.baseLanguage,
                supportedLanguages = config.supportedLanguages
            };
            var refExtractor = new ContentExtractorService(refConfig);
            try
            {
                var extractionResult = await refExtractor.ExtractContentsAsync(staleRefDict, refTranslationEntryDict, "ref");
                allTaskResults.Add(extractionResult);
                if (extractionResult.isSuccess)
                    ClearHandledUpdateFlags(staleRefModIds.ToList(), refModInfoDict);

                RestoreUnchangedRefEntries(refTranslationEntryDict, staleRefEntrySnapshot);
                MarkReferenceEntriesVerified(refTranslationEntryDict);
            }
            finally
            {
                CleanupDownloadedBatch(config, refBatchFolder);
            }
            Console.WriteLine($"  [OK] Refreshed {staleRefModIds.Count} ref mod(s), total ref entries: {refTranslationEntryDict.Count}");

            // 2d. Compute embeddings for refreshed ref entries.
            var refEmbedder = new EmbeddingFetcherService(config);
            allTaskResults.Add(await refEmbedder.FetchEmbeddingsAsync(
                modInfoDict, diffTranslationEntryDict, translationEntryDict, refModInfoDict, refTranslationEntryDict));
            ClearHandledUpdateFlags(staleRefModIds.ToList(), refModInfoDict);
            Console.WriteLine($"  [OK] Ref embeddings updated.");
        }
        else
        {
            Console.WriteLine($"  [OK] All {refModInfoDict.Count} ref mod(s) up to date.");
        }

        // 2e. Write ref data back.
        var refWriter = new ResultWriterService(config);
        allTaskResults.Add(await refWriter.WriteRefDataAsync(refModInfoDict, refTranslationEntryDict));
        Console.WriteLine();

        // --- Phase 3: Translation Mods ---
        // 3a. Load translation cache.
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Loading cached translation data...");
        repoLoader.LoadModCache(modInfoDict);
        repoLoader.LoadTranslationCache(translationEntryDict);
        repoLoader.LoadEntryMetadataCache(translationEntryDict);
        repoLoader.LoadEmbeddingCache(translationEntryDict);
        PurgeExcludedMod(PipelineExclusions.ProjectBabelWorkshopId, modInfoDict, translationEntryDict);
        // Keep cached entries as a shallow snapshot; deep-copying embeddings doubles retained memory.
        foreach (var (key, entry) in translationEntryDict)
            cachedTranslationEntryDict[key] = entry;
        Console.WriteLine($"  [OK] Loaded {modInfoDict.Count} mod(s), {translationEntryDict.Count} translation entries from data/.");
        Console.WriteLine();

        // 3b. Collect Mod IDs.
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Collecting Mod IDs...");
        var modIdCollector = new ModIdCollectorService(config);
        allTaskResults.Add(await modIdCollector.CollectModIdsAsync(modInfoDict));
        Console.WriteLine();

        var persistedModInfoDict = modInfoDict;
        modInfoDict = CreateDebugModSubset(modInfoDict, DebugOn ? DebugModLimit : 0);

        // 3c. Fetch Mod Infos from Steam (marks needsUpdate).
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Fetching Mod Information...");
        var modInfoFetcher = new ModInfoFetcherService(config);
        allTaskResults.Add(await modInfoFetcher.FetchModInfosAsync(modInfoDict));
        MarkUnavailableModEntriesInactive(modInfoDict, translationEntryDict);

        // Force needsUpdate for available mods that have never been extracted (zero entries in cache).
        // Skip mods already marked ACCEPTED — confirmed to have no translatable content.
        var modIdsWithEntries = translationEntryDict.Values
            .Select(e => e.modId)
            .Distinct(StringComparer.Ordinal)
            .ToHashSet(StringComparer.Ordinal);
        int forcedUpdates = 0;
        foreach (var (modId, info) in modInfoDict)
        {
            if (info.isAvailable
                && !modIdsWithEntries.Contains(modId)
                && !info.needsUpdate
                && info.contentCheckStatus != ContentCheckStatus.ACCEPTED)
            {
                var updated = info;
                updated.needsUpdate = true;
                modInfoDict[modId] = updated;
                forcedUpdates++;
            }
        }
        if (forcedUpdates > 0)
            Console.WriteLine($"  [OK] Forced update for {forcedUpdates} mod(s) with no cached entries.");

        var needsUpdateCount = modInfoDict.Values.Count(m => m.needsUpdate);
        Console.WriteLine($"  [OK] {needsUpdateCount}/{modInfoDict.Count} mod(s) need update.");
        Console.WriteLine();

        // 3d. Download & extract only needsUpdate mods.
        var updateModIds = modInfoDict
            .Where(kvp => kvp.Value.needsUpdate)
            .Select(kvp => kvp.Key)
            .ToList();
        var freshEntries = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        if (updateModIds.Count > 0)
        {
            Console.WriteLine($"Downloading and extracting {updateModIds.Count} updated mod(s) in batches.");
            var downloadBatches = updateModIds.Chunk(config.pipelineBatchSize).Select((b, i) => (batch: b, idx: i)).ToList();
            int totalBatches = downloadBatches.Count;
            foreach (var (batch, idx) in downloadBatches)
            {
                string batchTempFolder = Path.Combine(config.downloadingBatchesTempDir, $"batch_{idx + 1}");
                Directory.CreateDirectory(batchTempFolder);
                var batchIds = batch.ToList();
                var batchModInfoDict = batchIds.ToDictionary(id => id, id => modInfoDict[id], StringComparer.Ordinal);

                Console.WriteLine($"[{currentStep}/{totalSteps}] Downloading mods [batch {idx + 1}/{totalBatches}]: {batchIds.Count} mod(s)...");
                var modDownloader = new ModDownloaderService(config);
                allTaskResults.Add(await modDownloader.DownloadModsAsync(batchIds, batchModInfoDict, batchTempFolder));
                foreach (var (modId, info) in batchModInfoDict)
                    modInfoDict[modId] = info;
                Console.WriteLine();

                Console.WriteLine($"  Extracting content [batch {idx + 1}/{totalBatches}]...");
                var contentExtractor = new ContentExtractorService(config);
                try
                {
                    var extractionResult = await contentExtractor.ExtractContentsAsync(batchModInfoDict, freshEntries, $"batch_{idx + 1}");
                    allTaskResults.Add(extractionResult);
                    if (extractionResult.isSuccess)
                        ClearHandledUpdateFlags(batchIds, modInfoDict);
                }
                finally
                {
                    CleanupDownloadedBatch(config, batchTempFolder);
                }
                Console.WriteLine();
            }
            currentStep++; // download consumed
            currentStep++; // extract consumed
        }
        else
        {
            Console.WriteLine($"[{currentStep}/{totalSteps}] Downloading mods...");
            Console.WriteLine($"  [OK] No mods to update, skipping download.");
            currentStep++;
            Console.WriteLine($"[{currentStep}/{totalSteps}] Extracting translatable content...");
            Console.WriteLine($"  [OK] No mods to update, skipping extraction.");
            currentStep++;
        }
        Console.WriteLine();

        // Mark force-updated mods that still produced 0 entries → ACCEPTED (empty mod, no content to review).
        foreach (var modId in updateModIds)
        {
            bool hasEntries = freshEntries.Values.Any(e => e.modId == modId)
                              || translationEntryDict.Values.Any(e => e.modId == modId);
            if (!hasEntries && modInfoDict.TryGetValue(modId, out var mi) && mi.contentCheckStatus == ContentCheckStatus.UNKNOWN)
            {
                mi.contentCheckStatus = ContentCheckStatus.ACCEPTED;
                mi.needsContentCheck = false;
                modInfoDict[modId] = mi;
            }
        }

        // 3e. Merge & diff entries (fresh vs cached snapshot).
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Diffing translation entries...");
        var updatedModIdSet = updateModIds.ToHashSet(StringComparer.Ordinal);
        var baseLangIso = ResolveLanguage(config.supportedLanguages, config.baseLanguage)?.isoCode.ToLowerInvariant()
            ?? config.baseLanguage.ToLowerInvariant();
        RepoDataLoaderService.MarkMissingFreshEntriesInactive(cachedTranslationEntryDict, freshEntries, updatedModIdSet);
        var enabledTargetLangSet = enabledTargetLanguages
            .Select(lang => lang.isoCode)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        diffTranslationEntryDict = RepoDataLoaderService.DiffTranslationEntries(
            freshEntries, cachedTranslationEntryDict, baseLangIso, modInfoDict, enabledTargetLangSet);
        // Merge: for unchanged entries, restore cached (with translations + embeddings).
        foreach (var (key, cached) in cachedTranslationEntryDict)
        {
            if (!freshEntries.ContainsKey(key))
                translationEntryDict[key] = cached; // mod not in this update batch, keep cached
            else if (!diffTranslationEntryDict.ContainsKey(key))
            {
                // Preserve cached translations/embeddings for unchanged content,
                // while allowing newly extracted routing metadata to catch up.
                // This helper intentionally changes only outputFileStem.
                BackfillOutputRoute(cached, freshEntries[key]);
                cached.isActive = true;
                cached.lastSeenAt = DateTime.UtcNow;
                if (modInfoDict.TryGetValue(cached.modId, out var modInfo))
                    cached.lastSeenModUpdated = modInfo.timeModUpdated;
                if (string.IsNullOrWhiteSpace(cached.sourceHash))
                    cached.sourceHash = RepoDataLoaderService.ComputeSourceHash(cached, baseLangIso);
                translationEntryDict[key] = cached; // unchanged, keep cached translations
            }
        }
        // Add all fresh (diff) entries to main dict.
        foreach (var (key, entry) in diffTranslationEntryDict)
            translationEntryDict[key] = entry;
        var freshEntryCount = freshEntries.Count;
        Console.WriteLine($"  [OK] {diffTranslationEntryDict.Count}/{freshEntryCount} entry(s) changed/new.");
        freshEntries.Clear();
        cachedTranslationEntryDict.Clear();
        Console.WriteLine();

        // 3f. Content check (review mods that need it, then filter diff entries).
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Checking content...");
        var contentChecker = new ContentCheckerService(config);
        var targetWorkQueue = CollectTargetWorkQueue(
            translationEntryDict,
            enabledTargetLanguages.Select(lang => lang.isoCode),
            modInfoDict);
        Console.WriteLine($"  Target work queue: entries={targetWorkQueue.Count}; {FormatTargetQueueCounts(targetWorkQueue, enabledTargetLanguages.Select(lang => lang.isoCode))}");
        var checkedTargetWorkQueue = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        allTaskResults.Add(await contentChecker.CheckContentsAsync(modInfoDict, translationEntryDict, checkedTargetWorkQueue));
        // Filter to only entries that actually need translation.
        var refModIdSet = refModInfoDict.Keys.ToHashSet(StringComparer.Ordinal);
        diffTranslationEntryDict = checkedTargetWorkQueue
            .Where(kvp => targetWorkQueue.ContainsKey(kvp.Key))
            .Where(kvp => !refModIdSet.Contains(kvp.Value.modId))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.Ordinal);
        targetWorkQueue.Clear();
        Console.WriteLine();

        // 3g. Embeddings (target work queue + ref entries).
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Fetching Embeddings...");
        var embeddingFetcher = new EmbeddingFetcherService(config);
        allTaskResults.Add(await embeddingFetcher.FetchEmbeddingsAsync(
            modInfoDict, diffTranslationEntryDict, translationEntryDict, refModInfoDict, refTranslationEntryDict));
        Console.WriteLine();

        // 3h. Batching (checked target work queue).
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Creating translation batches...");
        var translationBatcher = new TranslationBatcherService(config);
        allTaskResults.Add(await translationBatcher.CreateBatchesAsync(modInfoDict, diffTranslationEntryDict, translationBatches));
        diffTranslationEntryDict.Clear();
        Console.WriteLine();

        // 3i. RAG + LLM per target language (checked target work queue).
        var ragRetriever = new RagContextRetrieverService(config);
        var llmTranslator = new LLMTranslatorService(config);
        foreach (var (targetLanguage, targetIndex) in enabledTargetLanguages.Select((lang, index) => (lang, index)))
        {
            Console.WriteLine($"  Target [{targetIndex + 1}/{enabledTargetLanguages.Count}]: {targetLanguage.englishName} ({targetLanguage.isoCode})");

            if (!HasPendingTargetEntries(translationBatches, targetLanguage.isoCode))
            {
                Console.WriteLine($"[{currentStep++}/{totalSteps}] Retrieving RAG contexts & preparing LLM prompts...");
                Console.WriteLine("  [OK] No pending entries for target, skipping RAG.");
                Console.WriteLine();

                Console.WriteLine($"[{currentStep++}/{totalSteps}] Performing LLM translation...");
                Console.WriteLine("  [OK] No pending entries for target, skipping LLM translation.");
                Console.WriteLine();
                continue;
            }

            Console.WriteLine($"[{currentStep++}/{totalSteps}] Retrieving RAG contexts & preparing LLM prompts...");
            allTaskResults.Add(await ragRetriever.RetrieveContextsAsync(
                refTranslationEntryDict, translationEntryDict, diffTranslationEntryDict,
                translationBatches, ragContextByEntryKey, targetLanguage.isoCode));
            var translationPlan = await llmTranslator.PrepareTranslationPlanAsync(
                modInfoDict, translationBatches, ragContextByEntryKey, targetLanguage.isoCode);
            ragContextByEntryKey.Clear();
            Console.WriteLine();

            Console.WriteLine($"[{currentStep++}/{totalSteps}] Performing LLM translation...");
            allTaskResults.Add(await llmTranslator.ExecuteTranslationPlansAsync([translationPlan]));
            Console.WriteLine();
        }

        // --- Phase 4: Write Back ---
        // 4a. Write data (merged).
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Writing data...");
        ClearHandledUpdateFlags(updateModIds, modInfoDict);
        MergeUpdatedModInfos(modInfoDict, persistedModInfoDict);
        var resultWriter = new ResultWriterService(config);
        allTaskResults.Add(await resultWriter.WriteDataAsync(persistedModInfoDict, translationEntryDict, refModInfoDict, refTranslationEntryDict));
        Console.WriteLine();

        // 4b. Write results per target language.
        foreach (var (targetLanguage, targetIndex) in outputLanguages.Select((lang, index) => (lang, index)))
        {
            Console.WriteLine($"  Output [{targetIndex + 1}/{outputLanguages.Count}]: {targetLanguage.englishName} ({targetLanguage.isoCode})");
            Console.WriteLine($"[{currentStep++}/{totalSteps}] Writing translation results...");
            allTaskResults.Add(await resultWriter.WriteResultsAsync(
                persistedModInfoDict, refTranslationEntryDict, translationEntryDict, targetLanguage.isoCode));
            Console.WriteLine();
        }

        // 4c. Final output (PZ mod distribution format).
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Writing final mod output...");
        var finalOutputWriter = new FinalOutputWriterService(config);
        allTaskResults.Add(await finalOutputWriter.WriteFinalOutputAsync(
            translationEntryDict, refModInfoDict, outputLanguages));
        Console.WriteLine();

        // 4d. Summary.
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Summarizing warning files...");
        await SummarizeWarningFilesAsync(config, allTaskResults);
        Console.WriteLine();

        // 4e. Progress report.
        Console.WriteLine($"[{currentStep++}/{totalSteps}] Generating progress report...");
        var progressReporter = new ProgressReporter.ProgressReporterService(config);
        await progressReporter.GenerateAsync(persistedModInfoDict, translationEntryDict, refModInfoDict);
        Console.WriteLine();

        Console.WriteLine("Pipeline complete.");
    }

    /// <summary>
    /// Copies only the fresh output route onto a cached entry. Route metadata is
    /// deliberately excluded from source hashes, translation work, and embeddings.
    /// An empty fresh route never erases a previously known route.
    /// </summary>
    public static void BackfillOutputRoute(TranslationEntry cached, TranslationEntry fresh)
    {
        if (!string.IsNullOrWhiteSpace(fresh.outputFileStem))
            cached.outputFileStem = fresh.outputFileStem;
    }

    /// <summary>Merges configured reference mod metadata into the runtime ref mod dictionary.</summary>
    private static void MergeConfiguredRefMods(List<ModInfo> configured, Dictionary<string, ModInfo> refModInfoDict)
    {
        foreach (var cfg in configured)
        {
            if (string.IsNullOrWhiteSpace(cfg.modId))
                continue;

            var info = refModInfoDict.TryGetValue(cfg.modId, out var existing) ? existing : new ModInfo { modId = cfg.modId };
            if (!string.IsNullOrWhiteSpace(cfg.modName))
                info.modName = cfg.modName;
            if (!string.IsNullOrWhiteSpace(cfg.language))
                info.language = cfg.language;
            refModInfoDict[cfg.modId] = info;
        }
    }

    /// <summary>
    /// Restores cached ref entries whose translation texts are unchanged by the refresh,
    /// preserving their cached embeddings so only actually-changed entries are re-embedded.
    /// </summary>
    private static void RestoreUnchangedRefEntries(
        Dictionary<string, TranslationEntry> refTranslationEntryDict,
        Dictionary<string, TranslationEntry> staleRefEntrySnapshot)
    {
        if (staleRefEntrySnapshot.Count == 0)
            return;

        var restored = 0;
        foreach (var (key, cached) in staleRefEntrySnapshot)
        {
            if (!refTranslationEntryDict.TryGetValue(key, out var fresh))
                continue;
            if (!RefEntryTextsEqual(fresh, cached))
                continue;

            refTranslationEntryDict[key] = cached;
            restored++;
        }

        if (restored > 0)
            Console.WriteLine($"  Ref merge: kept {restored} unchanged cached entry(s), reusing their embeddings.");
    }

    /// <summary>Compares two ref entries by per-language translation text only.</summary>
    private static bool RefEntryTextsEqual(TranslationEntry a, TranslationEntry b)
    {
        if (a.translationValues.Count != b.translationValues.Count)
            return false;

        foreach (var (lang, data) in a.translationValues)
        {
            if (!b.translationValues.TryGetValue(lang, out var other))
                return false;
            if (!string.Equals(data.text, other.text, StringComparison.Ordinal))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Removes a hardcoded-excluded workshop mod (and all of its entries) from the runtime
    /// dictionaries so it is never fetched, downloaded, translated, embedded, or used as a reference.
    /// </summary>
    private static void PurgeExcludedMod(
        string workshopId,
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> translationEntryDict)
    {
        if (modInfoDict.Remove(workshopId))
            Console.WriteLine($"  Excluded mod {workshopId} removed from processing.");

        var removedKeys = translationEntryDict
            .Where(kvp => string.Equals(kvp.Value.modId, workshopId, StringComparison.Ordinal))
            .Select(kvp => kvp.Key)
            .ToList();
        foreach (var key in removedKeys)
            translationEntryDict.Remove(key);
    }

    /// <summary>Marks all reference translation entries as active and verified.</summary>
    private static void MarkReferenceEntriesVerified(Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        foreach (var entry in refTranslationEntryDict.Values)
        {
            entry.isActive = true;
            foreach (var data in entry.translationValues.Values)
            {
                if (string.IsNullOrWhiteSpace(data.text))
                    continue;
                data.isVerified = true;
                data.status = "verified";
                data.processStatus = "processed";
            }
        }
    }

    /// <summary>
    /// Returns the set of reference mod IDs that need re-downloading:
    /// available mods with no cached translation entries or flagged for update.
    /// </summary>
    public static HashSet<string> CollectStaleReferenceModIds(
        Dictionary<string, ModInfo> refModInfoDict,
        Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        var modIdsWithEntries = refTranslationEntryDict.Values
            .Where(entry => entry.translationValues.Values.Any(data => !string.IsNullOrWhiteSpace(data.text)))
            .Select(entry => entry.modId)
            .ToHashSet(StringComparer.Ordinal);

        return refModInfoDict
            .Where(kvp => kvp.Value.isAvailable
                && (kvp.Value.needsUpdate || !modIdsWithEntries.Contains(kvp.Key)))
            .Select(kvp => kvp.Key)
            .ToHashSet(StringComparer.Ordinal);
    }

    /// <summary>
    /// Builds a work queue of entries that need translation for the given target languages.
    /// Skips entries belonging to REJECTED mods.
    /// </summary>
    public static Dictionary<string, TranslationEntry> CollectTargetWorkQueue(
        Dictionary<string, TranslationEntry> translationEntryDict,
        IEnumerable<string> targetLanguages,
        Dictionary<string, ModInfo>? modInfoDict = null)
    {
        var targets = targetLanguages.Select(lang => lang.ToLowerInvariant()).ToList();
        var queue = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        foreach (var (key, entry) in translationEntryDict)
        {
            if (modInfoDict != null)
            {
                if (!modInfoDict.TryGetValue(entry.modId, out var modInfo))
                    continue;
                // Skip REJECTED mods — their entries will never be translated.
                if (modInfo.contentCheckStatus == ContentCheckStatus.REJECTED)
                    continue;
            }

            var needsTarget = targets.Any(target => NeedsTargetProcessing(entry, target));
            if (!needsTarget)
                continue;

            queue[key] = entry;
        }
        return queue;
    }

    /// <summary>Formats per-target-language entry counts for console display.</summary>
    private static string FormatTargetQueueCounts(
        Dictionary<string, TranslationEntry> queue,
        IEnumerable<string> targetLanguages)
    {
        return string.Join(", ", targetLanguages
            .Select(lang => lang.ToLowerInvariant())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(target => $"{target}={queue.Values.Count(entry => NeedsTargetProcessing(entry, target))}"));
    }

    /// <summary>Checks whether any batch has pending translation entries for the given target language.</summary>
    public static bool HasPendingTargetEntries(
        IEnumerable<TranslationBatch> translationBatches,
        string targetLanguage)
    {
        var target = targetLanguage.ToLowerInvariant();
        return translationBatches.Any(batch =>
            batch.translationEntries.Any(entry => NeedsTargetProcessing(entry, target)));
    }

    /// <summary>Determines if an entry still needs translation for the given target language.</summary>
    private static bool NeedsTargetProcessing(TranslationEntry entry, string targetLang)
    {
        if (!entry.translationValues.TryGetValue(targetLang, out var data))
            return true;
        if (data.IsProcessed && !string.IsNullOrWhiteSpace(data.text))
            return false;
        return true;
    }

    /// <summary>Clears the needsUpdate flag for mods that were successfully downloaded and extracted.</summary>
    private static void ClearHandledUpdateFlags(List<string> updateModIds, Dictionary<string, ModInfo> modInfoDict)
    {
        foreach (var modId in updateModIds)
        {
            if (!modInfoDict.TryGetValue(modId, out var info))
                continue;
            if (!string.Equals(info.lastFetchStatus, "ok", StringComparison.OrdinalIgnoreCase))
                continue;
            if (!Directory.Exists(info.localDownloadedPath))
                continue;

            info.needsUpdate = false;
            modInfoDict[modId] = info;
        }
    }

    /// <summary>
    /// Removes downloaded mod content and the per-batch SteamCMD workspace after extraction.
    /// When extraction succeeds, the update flags are cleared before this method is called
    /// because the downloaded directories are intentionally no longer available afterwards.
    /// </summary>
    private static void CleanupDownloadedBatch(
        PipelineConfig config,
        string batchTempFolder)
    {
        var runTempDir = config.runTempDir;
        if (string.IsNullOrWhiteSpace(runTempDir))
            return;

        var deletedMods = 0;
        if (!string.IsNullOrWhiteSpace(config.downloadedModsTempDir)
            && Directory.Exists(config.downloadedModsTempDir))
        {
            if (!IsChildPathOf(config.downloadedModsTempDir, runTempDir))
            {
                GitHubActions.Warning($"Skipping downloaded mod cleanup outside run temp: {config.downloadedModsTempDir}", "Cleanup");
            }
            else
            {
                try
                {
                    foreach (var modPath in Directory.EnumerateDirectories(config.downloadedModsTempDir))
                    {
                        if (TryDeleteRunTempPath(modPath, runTempDir))
                            deletedMods++;
                    }
                }
                catch (Exception ex)
                {
                    GitHubActions.Warning($"Could not enumerate downloaded mod directories: {ex.Message}", "Cleanup");
                }
            }
        }

        if (TryDeleteRunTempPath(batchTempFolder, runTempDir))
            Console.WriteLine($"  [OK] Removed downloaded workspace: {batchTempFolder}");

        if (deletedMods > 0)
            Console.WriteLine($"  [OK] Removed {deletedMods} downloaded mod director{(deletedMods == 1 ? "y" : "ies")} after extraction.");
    }

    private static bool TryDeleteRunTempPath(string path, string runTempDir)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            return false;

        string fullPath;
        try
        {
            fullPath = Path.GetFullPath(path);
        }
        catch
        {
            return false;
        }

        if (!IsChildPathOf(fullPath, runTempDir))
        {
            GitHubActions.Warning($"Skipping workspace cleanup outside run temp: {fullPath}", "Cleanup");
            return false;
        }

        try
        {
            Directory.Delete(fullPath, recursive: true);
            return true;
        }
        catch (Exception ex)
        {
            GitHubActions.Warning($"Could not remove downloaded workspace '{fullPath}': {ex.Message}", "Cleanup");
            return false;
        }
    }

    private static bool IsChildPathOf(string path, string parent)
    {
        string fullPath;
        string fullParent;
        try
        {
            fullPath = Path.GetFullPath(path);
            fullParent = Path.GetFullPath(parent);
        }
        catch
        {
            return false;
        }

        var relative = Path.GetRelativePath(fullParent, fullPath);
        if (relative == "." || relative == ".." || Path.IsPathRooted(relative))
            return false;

        var comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        return !relative.StartsWith(".." + Path.DirectorySeparatorChar, comparison)
            && !relative.StartsWith(".." + Path.AltDirectorySeparatorChar, comparison);
    }

    /// <summary>Marks all entries belonging to unavailable mods as inactive.</summary>
    private static void MarkUnavailableModEntriesInactive(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> translationEntryDict)
    {
        var unavailable = modInfoDict
            .Where(kvp => !kvp.Value.isAvailable)
            .Select(kvp => kvp.Key)
            .ToHashSet(StringComparer.Ordinal);
        if (unavailable.Count == 0)
            return;

        foreach (var entry in translationEntryDict.Values)
        {
            if (unavailable.Contains(entry.modId))
                entry.isActive = false;
        }
    }

    /// <summary>
    /// Creates a debug subset from the full mod dictionary, limited to the specified count.
    /// Returns the full dictionary if the limit is zero or exceeds the actual count.
    /// </summary>
    public static Dictionary<string, ModInfo> CreateDebugModSubset(Dictionary<string, ModInfo> fullModInfoDict, int limit)
    {
        if (limit <= 0 || fullModInfoDict.Count <= limit)
        {
            Console.WriteLine($"  Debug mod subset: using all {fullModInfoDict.Count} mod(s).");
            return fullModInfoDict;
        }

        var subset = fullModInfoDict
            .Take(limit)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.Ordinal);
        Console.WriteLine($"  Debug mod subset: using {subset.Count}/{fullModInfoDict.Count} mod(s).");
        return subset;
    }

    /// <summary>Copies all entries from the updated mod info dict into the persisted dict.</summary>
    public static void MergeUpdatedModInfos(
        Dictionary<string, ModInfo> updatedModInfoDict,
        Dictionary<string, ModInfo> persistedModInfoDict)
    {
        foreach (var (modId, info) in updatedModInfoDict)
            persistedModInfoDict[modId] = info;
    }

    /// <summary>
    /// Replaces supportedLanguages with a debug subset: base lang + zh-hans + N additional targets.
    /// </summary>
    public static void ApplyDebugSupportedLanguageSubset(PipelineConfig config, int additionalTargetLanguageCount)
    {
        var fullLanguages = config.supportedLanguages.ToList();
        if (fullLanguages.Count == 0)
            throw new InvalidOperationException("No supported languages are configured.");

        var baseLanguage = ResolveLanguage(fullLanguages, config.baseLanguage)
            ?? throw new InvalidOperationException($"Base language '{config.baseLanguage}' is not in supported languages.");
        var simplifiedChinese = ResolveLanguage(fullLanguages, "zh-hans")
            ?? ResolveLanguage(fullLanguages, "CN")
            ?? throw new InvalidOperationException("Simplified Chinese (zh-hans/CN) is not in supported languages.");

        var requiredLanguages = new List<LangInfoData>();
        AddUniqueLanguage(requiredLanguages, baseLanguage);
        AddUniqueLanguage(requiredLanguages, simplifiedChinese);

        var maxAdditional = Math.Max(0, fullLanguages.Count - requiredLanguages.Count);
        if (additionalTargetLanguageCount < 0 || additionalTargetLanguageCount > maxAdditional)
            throw new ArgumentOutOfRangeException(nameof(additionalTargetLanguageCount), additionalTargetLanguageCount, $"Expected 0..{maxAdditional}.");

        var extraLanguages = fullLanguages
            .Where(lang => !ContainsLanguage(requiredLanguages, lang))
            .Take(additionalTargetLanguageCount);

        var enabledLanguages = requiredLanguages.ToList();
        foreach (var lang in extraLanguages)
            AddUniqueLanguage(enabledLanguages, lang);

        config.supportedLanguages = enabledLanguages;
        config.priorityLanguage = simplifiedChinese.isoCode;

        var enabledTargets = GetTargetLanguages(config);
        Console.WriteLine(
            $"  Debug supported languages: enabled {enabledLanguages.Count}/{fullLanguages.Count} language(s); targets={string.Join(", ", enabledTargets.Select(lang => lang.isoCode))}; extraTargets={additionalTargetLanguageCount}/{maxAdditional}.");
    }

    /// <summary>Returns all supported languages except the base language (translation targets).</summary>
    private static List<LangInfoData> GetTargetLanguages(PipelineConfig config)
    {
        var baseLanguage = ResolveLanguage(config.supportedLanguages, config.baseLanguage)
            ?? throw new InvalidOperationException($"Base language '{config.baseLanguage}' is not in supported languages.");
        return config.supportedLanguages
            .Where(lang => !IsSameLanguage(lang, baseLanguage))
            .ToList();
    }

    /// <summary>Returns all supported languages including the base language (for output generation).</summary>
    private static List<LangInfoData> GetOutputLanguages(PipelineConfig config)
    {
        var baseLanguage = ResolveLanguage(config.supportedLanguages, config.baseLanguage)
            ?? throw new InvalidOperationException($"Base language '{config.baseLanguage}' is not in supported languages.");
        var outputLanguages = new List<LangInfoData>();
        AddUniqueLanguage(outputLanguages, baseLanguage);
        foreach (var lang in config.supportedLanguages)
            AddUniqueLanguage(outputLanguages, lang);
        return outputLanguages;
    }

    /// <summary>Finds a LangInfoData by ISO code or in-game code (case-insensitive).</summary>
    private static LangInfoData? ResolveLanguage(IEnumerable<LangInfoData> languages, string language)
    {
        return languages.FirstOrDefault(lang =>
            string.Equals(lang.ingameCode, language, StringComparison.OrdinalIgnoreCase)
            || string.Equals(lang.isoCode, language, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Adds a language to the list if an equivalent entry does not already exist.</summary>
    private static void AddUniqueLanguage(List<LangInfoData> languages, LangInfoData language)
    {
        if (!ContainsLanguage(languages, language))
            languages.Add(language);
    }

    /// <summary>Returns true if the language list already contains an equivalent language entry.</summary>
    private static bool ContainsLanguage(IEnumerable<LangInfoData> languages, LangInfoData language)
    {
        return languages.Any(existing => IsSameLanguage(existing, language));
    }

    /// <summary>Determines if two LangInfoData refer to the same language (by ISO code or in-game code).</summary>
    private static bool IsSameLanguage(LangInfoData left, LangInfoData right)
    {
        return string.Equals(left.isoCode, right.isoCode, StringComparison.OrdinalIgnoreCase)
            || (!string.IsNullOrWhiteSpace(left.ingameCode)
                && string.Equals(left.ingameCode, right.ingameCode, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Walks up from working directory and app directory to find the repo root (containing config/config.json).</summary>
    private static string FindRepositoryRoot()
    {
        var currentRoot = FindRepositoryRoot(Directory.GetCurrentDirectory());
        if (currentRoot != null)
            return currentRoot;

        var appRoot = FindRepositoryRoot(AppContext.BaseDirectory);
        if (appRoot != null)
            return appRoot;

        throw new DirectoryNotFoundException("Repository root with config/config.json was not found.");
    }

    /// <summary>Walks up the directory tree looking for config/config.json.</summary>
    private static string? FindRepositoryRoot(string startDirectory)
    {
        var directory = new DirectoryInfo(startDirectory);
        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "config", "config.json")))
                return directory.FullName;

            directory = directory.Parent;
        }

        return null;
    }

    /// <summary>Aggregates warning file counts and task results into a console summary.</summary>
    private static Task SummarizeWarningFilesAsync(PipelineConfig config, List<TaskResult> allTaskResults)
    {
        var warningFiles = Directory.Exists(config.warningsTempDir)
            ? Directory.GetFiles(config.warningsTempDir, "*.json")
            : [];
        var successCount = allTaskResults.Count(result => result.isSuccess);
        var errorCount = allTaskResults.Sum(result => result.errorCount);
        var warningCount = allTaskResults.Sum(result => result.warningCount);

        Console.WriteLine("  ------SUMMARY------");
        Console.WriteLine($"  Task results: {successCount}/{allTaskResults.Count} successful");
        Console.WriteLine($"  Errors: {errorCount} | Warnings: {warningCount} | Warning files: {warningFiles.Length}");
        return Task.CompletedTask;
    }

}
