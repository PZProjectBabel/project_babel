namespace Common;

/// <summary>
/// Content review status shared across pipeline stages.
/// </summary>
public enum ContentCheckStatus
{
    /// <summary>Not yet reviewed.</summary>
    UNKNOWN,
    /// <summary>Content is safe to translate.</summary>
    ACCEPTED,
    /// <summary>Content has been rejected (harmful / policy-violating).</summary>
    REJECTED,
    /// <summary>Needs manual review before proceeding.</summary>
    NEEDVERIFICATION
}

/// <summary>
/// Common task result returned by pipeline stages.
/// </summary>
public struct TaskResult
{
    /// <summary>Whether the task completed without fatal errors.</summary>
    public bool isSuccess { get; set; }
    /// <summary>Number of errors encountered.</summary>
    public int errorCount { get; set; }
    /// <summary>Number of non-fatal warnings.</summary>
    public int warningCount { get; set; }
    /// <summary>Machine-readable summary in JSON format.</summary>
    public string summaryJson { get; set; }
    /// <summary>Diagnostic JSON for unexpected/faulty results.</summary>
    public string fuckSummaryJson { get; set; }

    public TaskResult()
    {
        isSuccess = true;
        errorCount = 0;
        warningCount = 0;
        summaryJson = "";
        fuckSummaryJson = "";
    }
}

/// <summary>
/// Mod metadata cached locally and passed across modules.
/// </summary>
public struct ModInfo
{
    /// <summary>Steam Workshop published file ID.</summary>
    public string modId { get; set; }
    /// <summary>Human-readable mod name from Steam.</summary>
    public string modName { get; set; }
    /// <summary>Mod creator / author name.</summary>
    public string creator { get; set; }
    /// <summary>Mod's primary language (ISO code), if known.</summary>
    public string? language { get; set; }
    /// <summary>Local filesystem path where the downloaded mod resides.</summary>
    public string localDownloadedPath { get; set; }
    /// <summary>UTC timestamp of the mod's last update on Steam Workshop.</summary>
    public DateTime timeModUpdated { get; set; }
    /// <summary>UTC timestamp when the mod was originally published.</summary>
    public DateTime timeModCreated { get; set; }
    /// <summary>When this metadata was last fetched from Steam.</summary>
    public DateTime timeLastChecked { get; set; }
    /// <summary>Number of active subscribers on Steam Workshop.</summary>
    public int subscription { get; set; }
    /// <summary>Number of users who favorited this mod.</summary>
    public int favorite { get; set; }
    /// <summary>Cleaned mod description text (Steam BBCode stripped).</summary>
    public string description { get; set; }
    /// <summary>Steam consumer App ID (108600 = Project Zomboid).</summary>
    public int consumerAppId { get; set; }
    /// <summary>Current content safety review status.</summary>
    public ContentCheckStatus contentCheckStatus { get; set; }
    /// <summary>Whether the mod's files need re-downloading and re-extracting.</summary>
    public bool needsUpdate { get; set; }
    /// <summary>Whether the mod should be re-reviewed for content safety.</summary>
    public bool needsContentCheck { get; set; }
    /// <summary>Next scheduled content check date (UTC).</summary>
    public DateTime timeNextContentCheck { get; set; }
    /// <summary>Whether the mod is still accessible on Steam Workshop.</summary>
    public bool isAvailable { get; set; }
    /// <summary>Status string from the last Steam fetch attempt.</summary>
    public string lastFetchStatus { get; set; }
    /// <summary>Content review confidence score (0.0-1.0).</summary>
    public double contentCheckConfidence { get; set; }
    /// <summary>Whether the content review flagged this for human review.</summary>
    public bool contentCheckNeedHumanReview { get; set; }
    /// <summary>Risk level assigned by the content checker (safe/low/medium/high).</summary>
    public string contentCheckRiskLevel { get; set; }
    /// <summary>Human-readable reason for the content review decision.</summary>
    public string contentCheckReason { get; set; }
    /// <summary>JSON array of violated policy rule IDs.</summary>
    public string contentCheckViolatedRulesJson { get; set; }

    public ModInfo()
    {
        modId = "";
        modName = "";
        creator = "";
        language = null;
        localDownloadedPath = "";
        timeModUpdated = DateTime.MinValue;
        timeModCreated = DateTime.MinValue;
        timeLastChecked = DateTime.MinValue;
        subscription = 0;
        favorite = 0;
        description = "";
        consumerAppId = 108600;
        contentCheckStatus = ContentCheckStatus.UNKNOWN;
        needsUpdate = false;
        needsContentCheck = false;
        timeNextContentCheck = DateTime.MinValue;
        isAvailable = true;
        lastFetchStatus = "unknown";
        contentCheckConfidence = 0;
        contentCheckNeedHumanReview = false;
        contentCheckRiskLevel = "";
        contentCheckReason = "";
        contentCheckViolatedRulesJson = "";
    }
}

/// <summary>
/// Translation entries grouped for downstream translation stages.
/// </summary>
public class TranslationBatch
{
    /// <summary>Sequential batch identifier within the run.</summary>
    public int batchId { get; set; }
    /// <summary>Priority score (higher = translate first).</summary>
    public int priority { get; set; }
    /// <summary>Mod that owns these translation entries.</summary>
    public string modId { get; set; } = "";
    /// <summary>Entries to translate in this batch.</summary>
    public List<TranslationEntry> translationEntries { get; set; } = [];
    /// <summary>Source language for the entries in this batch.</summary>
    public string baseLang { get; set; } = "en";
    /// <summary>Target language to translate into.</summary>
    public string targetLang { get; set; } = "zh-hans";
}

/// <summary>
/// Language metadata record from supported_languages.json.
/// </summary>
public class LangInfoData
{
    /// <summary>In-game language code used by Project Zomboid (e.g. "CN", "EN").</summary>
    public string ingameCode { get; set; }
    /// <summary>Language name in Chinese.</summary>
    public string chineseName { get; set; }
    /// <summary>Language name in English.</summary>
    public string englishName { get; set; }
    /// <summary>Language name in its native script.</summary>
    public string nativeName { get; set; }
    /// <summary>Standard ISO 639-1 code (e.g. "zh-hans", "en").</summary>
    public string isoCode { get; set; }

    public LangInfoData()
    {
        ingameCode = "";
        chineseName = "";
        englishName = "";
        nativeName = "";
        isoCode = "";
    }
}

/// <summary>
/// Extracted translation entry keyed by mod and translation key.
/// </summary>
public class TranslationEntry
{
    /// <summary>Mod this entry belongs to.</summary>
    public string modId { get; set; } = "";
    /// <summary>Master key derived from the source file's top-level Lua table name.</summary>
    public string masterKey { get; set; } = "";
    /// <summary>Translation key within the mod (e.g. "IGUI_Test").</summary>
    public string translationKey { get; set; } = "";
    /// <summary>All language texts keyed by ISO code.</summary>
    public Dictionary<string, TranslationData> translationValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    /// <summary>Source/base language for this entry.</summary>
    public string baseLang { get; set; } = "en";
    /// <summary>SHA-256 hash of the embedding input text.</summary>
    public string embeddingHash { get; set; } = "";
    /// <summary>Primary embedding vector (float32, typically 384-dim).</summary>
    public float[] embeddingVector { get; set; } = [];
    /// <summary>Kind of source used to generate the primary embedding.</summary>
    public string embeddingSourceKind { get; set; } = "";
    /// <summary>Target language associated with the primary embedding.</summary>
    public string embeddingTargetLang { get; set; } = "";
    /// <summary>All embeddings for this entry, keyed by source-kind and target-lang.</summary>
    public Dictionary<string, TranslationEmbedding> embeddingValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    /// <summary>Whether this entry is still active (present in latest mod file).</summary>
    public bool isActive { get; set; } = true;
    /// <summary>When the entry was last observed in the mod files.</summary>
    public DateTime lastSeenAt { get; set; } = DateTime.MinValue;
    /// <summary>The mod's timeModUpdated value when this entry was last seen.</summary>
    public DateTime lastSeenModUpdated { get; set; } = DateTime.MinValue;
    /// <summary>Hash of the source text for change detection.</summary>
    public string sourceHash { get; set; } = "";
    /// <summary>
    /// Optional output JSON file stem selected from the winning mod source file.
    /// This is routing metadata only and is not part of source or embedding identity.
    /// </summary>
    public string outputFileStem { get; set; } = "";
    /// <summary>List of files containing this translation entry.</summary>
    public List<ContainingFileInfo> containingFileInfos { get; set; } = [];

    /// <summary>Returns the base-language text for this entry, or empty if missing.</summary>
    public TranslationSourceText GetBaseTextStrict(string? requestedBaseLang = null)
    {
        var lang = NormalizeLang(string.IsNullOrWhiteSpace(requestedBaseLang) ? baseLang : requestedBaseLang!);
        return translationValues.TryGetValue(lang, out var data)
            ? new TranslationSourceText(lang, data.text, true)
            : new TranslationSourceText(lang, "", true);
    }

    /// <summary>
    /// Finds the best available source text: preferred base lang, then any verified translation, then any text.
    /// Returns a TranslationSourceText with the lang, text, and whether it's the true base language.
    /// </summary>
    public TranslationSourceText GetSourceText(
        string? requestedBaseLang = null,
        IEnumerable<string>? excludedFallbackLangs = null)
    {
        var baseCandidates = new List<string>();
        AddLangCandidate(baseCandidates, requestedBaseLang);
        AddLangCandidate(baseCandidates, baseLang);

        foreach (var lang in baseCandidates)
        {
            if (translationValues.TryGetValue(lang, out var data)
                && !string.IsNullOrWhiteSpace(data.text))
            {
                return new TranslationSourceText(NormalizeLang(lang), data.text, true);
            }
        }

        var excluded = new HashSet<string>(baseCandidates, StringComparer.OrdinalIgnoreCase);
        if (excludedFallbackLangs != null)
        {
            foreach (var lang in excludedFallbackLangs)
                AddLangCandidate(excluded, lang);
        }

        foreach (var (lang, data) in translationValues)
        {
            if (!excluded.Contains(lang)
                && IsVerified(data)
                && !string.IsNullOrWhiteSpace(data.text))
            {
                return new TranslationSourceText(NormalizeLang(lang), data.text, false);
            }
        }

        foreach (var (lang, data) in translationValues)
        {
            if (!excluded.Contains(lang)
                && !string.IsNullOrWhiteSpace(data.text))
            {
                return new TranslationSourceText(NormalizeLang(lang), data.text, false);
            }
        }

        var emptyLang = baseCandidates.Count > 0 ? baseCandidates[0] : "en";
        return new TranslationSourceText(NormalizeLang(emptyLang), "", true);
    }

    private static void AddLangCandidate(ICollection<string> candidates, string? lang)
    {
        if (string.IsNullOrWhiteSpace(lang)
            || candidates.Contains(lang, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        candidates.Add(lang);
    }

    private static bool IsVerified(TranslationData data)
    {
        return data.isVerified || string.Equals(data.status, "verified", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeLang(string lang)
    {
        return string.IsNullOrWhiteSpace(lang) ? "en" : lang.ToLowerInvariant();
    }
}

/// <summary>Holds a source text snapshot with its language and whether it is the base language.</summary>
public sealed record TranslationSourceText(string lang, string text, bool isBaseLang);

/// <summary>An embedding vector for a specific source-kind and target-language pair.</summary>
public class TranslationEmbedding
{
    /// <summary>Kind of source text used ("normal_base_text", "ref_target_text", etc.).</summary>
    public string sourceKind { get; set; } = "";
    /// <summary>Target language this embedding was computed for.</summary>
    public string targetLang { get; set; } = "";
    /// <summary>SHA-256 hash of the input text used to generate this embedding.</summary>
    public string hash { get; set; } = "";
    /// <summary>The embedding vector (float32 array).</summary>
    public float[] vector { get; set; } = [];
}

/// <summary>Translation text plus verification &amp; processing metadata.</summary>
public class TranslationData
{
    /// <summary>The translated or source text.</summary>
    public string text { get; set; } = "";
    /// <summary>Whether this translation has been human-verified.</summary>
    public bool isVerified { get; set; }
    /// <summary>LLM confidence score for the translation (0.0-1.0).</summary>
    public float? confidence { get; set; }
    /// <summary>Verification status: "verified" or "unverified".</summary>
    public string status { get; set; } = "unverified";
    /// <summary>Processing status: "processed" or "unprocessed".</summary>
    public string processStatus { get; set; } = "unprocessed";
    /// <summary>Diagnostic/context comments attached to this translation.</summary>
    public List<string> comments { get; set; } = [];

    /// <summary>Whether this translation has been processed by the LLM pipeline.</summary>
    public bool IsProcessed =>
        string.Equals(processStatus, "processed", StringComparison.OrdinalIgnoreCase)
        || isVerified
        || string.Equals(status, "verified", StringComparison.OrdinalIgnoreCase);

    /// <summary>Whether this translation has been verified (human or otherwise).</summary>
    public bool IsVerified =>
        isVerified || string.Equals(status, "verified", StringComparison.OrdinalIgnoreCase);
}

/// <summary>Information about a source file containing a translation entry.</summary>
public class ContainingFileInfo
{
    /// <summary>File name (e.g. "UI_EN.txt").</summary>
    public string fileName { get; set; } = "";
    /// <summary>Relative path from the mod root.</summary>
    public string filePath { get; set; } = "";
    /// <summary>Name of the sub-mod (first directory under mods/).</summary>
    public string subModName { get; set; } = "";
    /// <summary>Game major version from the directory structure.</summary>
    public int gameMajorVersion { get; set; }
    /// <summary>Game minor version from the directory structure.</summary>
    public int gameMinorVersion { get; set; }
}

/// <summary>
/// Shared pipeline configuration loaded from config.json and secrets.
/// </summary>
public class PipelineConfig
{
    // Runtime base directory.
    public string baseDir { get; set; } = "..";

    // Persistent folders.
    public string configDir { get; set; } = "";
    public string dataDir { get; set; } = "";
    public string tempDir { get; set; } = "";

    // Runtime temp folders (set by folder initializer).
    public string runTempDir { get; set; } = "";
    public string downloadedModsTempDir { get; set; } = "";
    public string extractedContentsTempDir { get; set; } = "";
    public string downloadingBatchesTempDir { get; set; } = "";
    public string extractedReferencesTempDir { get; set; } = "";
    public string contentCheckingPromptsTempDir { get; set; } = "";
    public string contentCheckingResultsTempDir { get; set; } = "";
    public string embeddingsTempDir { get; set; } = "";
    public string translationBatchesTempDir { get; set; } = "";
    public string translationResultsTempDir { get; set; } = "";
    public string warningsTempDir { get; set; } = "";

    // Settings.
    public string priorityLanguage { get; set; } = "zh-hans";
    public string baseLanguage { get; set; } = "EN";

    // ── LLM settings ──
    public string llmApiEndpoint { get; set; } = "https://api.deepseek.com/chat/completions";
    public string llmModel { get; set; } = "deepseek-v4-flash";
    public string llmReasoningEffort { get; set; } = "low";
    public float llmTemperature { get; set; } = 0.1f;
    public int llmMaxTokens { get; set; } = 380000;
    public int llmBatchSize { get; set; } = 30;
    public int llmBatchTokenBudget { get; set; } = 2000;
    public int llmRequestTimeoutSeconds { get; set; } = 300;
    public int llmConcurrencyInitial { get; set; } = 0;
    public int llmConcurrencyMaximum { get; set; } = 0;
    public int llmConcurrencyMinimum { get; set; } = 1;
    public int llmConcurrencyMaxRetries { get; set; } = 5;
    public int llmConcurrencyFailureStreakToDecrease { get; set; } = 3;
    public int llmConcurrencyRetryBaseDelayMs { get; set; } = 1000;
    public int llmConcurrencyRetryMaxDelayMs { get; set; } = 60000;
    public int llmFixedConcurrency { get; set; } = 16;

    // ── RAG settings ──
    public float ragSimilarityThreshold { get; set; } = 0.75f;
    public int ragTopK { get; set; } = 10;
    public string ragIndexDir { get; set; } = "data/rag_index";

    // ── AsOne settings ──
    public bool asOneEnabled { get; set; } = true;
    public string asOneBaseUrl { get; set; } = "https://www.asone.fun/";
    public string asOnePublicModListPath { get; set; } = "api/Home/GetAllModinfo";
    public string asOneModInfoFileName { get; set; } = "modInfo.txt";

    // ── Steam settings ──
    public int steamApiChunkSize { get; set; } = 20;
    public int steamRequestTimeoutSeconds { get; set; } = 30;
    public int steamMaxRetries { get; set; } = 3;

    // ── Workflow settings ──
    public int maxJobs { get; set; } = 16;

    // ── Pipeline settings ──
    public int pipelineBatchSize { get; set; } = 20;

    // ── Content check settings ──
    public bool contentCheckEnabled { get; set; } = true;
    public int contentCheckIntervalDays { get; set; } = 90;

    // ── Secrets (loaded from secrets.json / env vars) ──
    public string embeddingHost { get; set; } = "127.0.0.1";
    public int embeddingPort { get; set; } = 8000;
    public string steamApiKey { get; set; } = "";
    public string embeddingKey { get; set; } = "";
    public string llmKey { get; set; } = "";

    // ── Supported languages ──
    public List<LangInfoData> supportedLanguages { get; set; } = new List<LangInfoData>();

    /// <summary>Reference translation mods loaded from config/ref_translation_mods.json.</summary>
    public List<ModInfo> referenceTranslationMods { get; set; } = new List<ModInfo>();
}

/// <summary>
/// Workshop IDs that are hardcoded exclusions: never translated, downloaded, embedded,
/// or used as reference mods.
/// </summary>
public static class PipelineExclusions
{
    /// <summary>Workshop ID of the project's own mod "project_babel".</summary>
    public const string ProjectBabelWorkshopId = "3759583822";

    /// <summary>Returns true for workshop IDs that must be excluded from the pipeline.</summary>
    public static bool IsExcluded(string workshopId)
        => string.Equals(workshopId, ProjectBabelWorkshopId, StringComparison.Ordinal);
}
