using System.Text.Json;
using Common;

namespace ConfigReader;

/// <summary>
/// Pipeline phase 0 service that loads and validates config.json plus secrets.
/// Fatal validation errors are thrown so the caller can stop the pipeline.
/// </summary>
public class ConfigReaderService
{
    /// <summary>
    /// Loads the complete configuration from the base directory and throws on validation failure.
    /// </summary>
    public PipelineConfig LoadConfig(string baseDirectory)
    {
        var config = new PipelineConfig { baseDir = baseDirectory };
        InitializeFolders(config);

        // Required files.
        string configFile = Path.Combine(config.configDir, "config.json");
        string secretsFile = Path.Combine(config.configDir, "secrets.json");

        if (!File.Exists(configFile))
            throw new FileNotFoundException($"Config file not found: {configFile}");
        Console.WriteLine($"  [OK] Found config/config.json at {configFile}");

        // Read config.json.
        try
        {
            using var doc = JsonDocument.Parse(Utf8NoBom.ReadAllText(configFile));
            var root = doc.RootElement;

            if (root.TryGetProperty("Settings", out var settings))
            {
                config.priorityLanguage = GetString(settings, "priority_language", config.priorityLanguage);
                config.baseLanguage = GetString(settings, "base_language", config.baseLanguage);
            }
            if (root.TryGetProperty("LLM", out var llm))
            {
                config.llmApiEndpoint = GetString(llm, "api_endpoint", config.llmApiEndpoint);
                config.llmModel = GetString(llm, "model", config.llmModel);
                config.llmReasoningEffort = GetString(llm, "reasoning_effort", config.llmReasoningEffort);
                config.llmTemperature = GetFloat(llm, "temperature", config.llmTemperature);
                config.llmMaxTokens = GetInt(llm, "max_tokens", config.llmMaxTokens);
                config.llmBatchSize = GetInt(llm, "batch_size", config.llmBatchSize);
                config.llmBatchTokenBudget = GetInt(llm, "batch_token_budget", config.llmBatchTokenBudget);
                config.llmRequestTimeoutSeconds = GetInt(llm, "request_timeout_seconds", config.llmRequestTimeoutSeconds);
                if (llm.TryGetProperty("concurrency", out var concurrency))
                {
                    config.llmConcurrencyInitial = GetInt(concurrency, "initial", config.llmConcurrencyInitial);
                    config.llmConcurrencyMaximum = GetInt(concurrency, "maximum", config.llmConcurrencyMaximum);
                    config.llmConcurrencyMinimum = GetInt(concurrency, "minimum", config.llmConcurrencyMinimum);
                    config.llmConcurrencyMaxRetries = GetInt(concurrency, "max_retries", config.llmConcurrencyMaxRetries);
                    config.llmConcurrencyFailureStreakToDecrease = GetInt(concurrency, "failure_streak_to_decrease", config.llmConcurrencyFailureStreakToDecrease);
                    config.llmConcurrencyRetryBaseDelayMs = GetInt(concurrency, "retry_base_delay_ms", config.llmConcurrencyRetryBaseDelayMs);
                    config.llmConcurrencyRetryMaxDelayMs = GetInt(concurrency, "retry_max_delay_ms", config.llmConcurrencyRetryMaxDelayMs);
                    config.llmFixedConcurrency = GetInt(concurrency, "fixed_concurrency", config.llmFixedConcurrency);
                }
            }
            if (root.TryGetProperty("Embedding", out var emb))
            {
                config.embeddingHost = GetString(emb, "host", config.embeddingHost);
                config.embeddingPort = GetInt(emb, "port", config.embeddingPort);
            }
            if (root.TryGetProperty("RAG", out var rag))
            {
                config.ragSimilarityThreshold = GetFloat(rag, "similarity_threshold", config.ragSimilarityThreshold);
                config.ragTopK = GetInt(rag, "top_k", config.ragTopK);
                config.ragIndexDir = GetString(rag, "index_dir", config.ragIndexDir);
            }
            if (root.TryGetProperty("AsOne", out var asOne))
            {
                config.asOneEnabled = GetBool(asOne, "enabled", config.asOneEnabled);
                config.asOneBaseUrl = GetString(asOne, "base_url", config.asOneBaseUrl);
                config.asOnePublicModListPath = GetString(asOne, "public_mod_list_path", config.asOnePublicModListPath);
                config.asOneModInfoFileName = GetString(asOne, "mod_info_file_name", config.asOneModInfoFileName);
            }
            if (root.TryGetProperty("Steam", out var steam))
            {
                config.steamApiChunkSize = GetInt(steam, "api_chunk_size", config.steamApiChunkSize);
                config.steamRequestTimeoutSeconds = GetInt(steam, "request_timeout_seconds", config.steamRequestTimeoutSeconds);
                config.steamMaxRetries = GetInt(steam, "max_retries", config.steamMaxRetries);
            }
            if (root.TryGetProperty("Workflow", out var wf))
            {
                config.maxJobs = GetInt(wf, "max_jobs", config.maxJobs);
            }
            if (root.TryGetProperty("Pipeline", out var pipeline))
            {
                config.pipelineBatchSize = GetInt(pipeline, "batch_size", config.pipelineBatchSize);
            }
            if (root.TryGetProperty("ContentCheck", out var contentCheck))
            {
                config.contentCheckEnabled = GetBool(contentCheck, "enabled", config.contentCheckEnabled);
                config.contentCheckIntervalDays = GetInt(contentCheck, "check_interval_days", config.contentCheckIntervalDays);
            }
            Console.WriteLine("  [OK] config.json parsed");
        }
        catch (Exception ex) when (ex is not DirectoryNotFoundException && ex is not FileNotFoundException)
        {
            throw new InvalidOperationException($"Failed to read config.json: {ex.Message}", ex);
        }

        // Read secrets from secrets.json first, then fall back to environment variables.
        if (File.Exists(secretsFile))
        {
            try
            {
                var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(Utf8NoBom.ReadAllText(secretsFile));
                if (secrets != null)
                {
                    secrets.TryGetValue("STEAM_KEY", out var steamKey);
                    secrets.TryGetValue("EMBEDDING_HOST", out var embHost);
                    secrets.TryGetValue("EMBEDDING_PORT", out var embPort);
                    secrets.TryGetValue("EMBEDDING_KEY", out var embKey);
                    secrets.TryGetValue("LLM_KEY", out var llmKey);

                    if (!string.IsNullOrEmpty(steamKey)) config.steamApiKey = steamKey;
                    if (!string.IsNullOrEmpty(embHost)) config.embeddingHost = embHost;
                    if (!string.IsNullOrEmpty(embPort) && int.TryParse(embPort, out var port)) config.embeddingPort = port;
                    if (!string.IsNullOrEmpty(embKey)) config.embeddingKey = embKey;
                    if (!string.IsNullOrEmpty(llmKey)) config.llmKey = llmKey;
                }
                config.steamApiKey = EnvOr(config.steamApiKey, "STEAM_KEY", "STEAM_API_KEY");
                config.embeddingHost = EnvOr(config.embeddingHost, "EMBEDDING_HOST");
                config.embeddingPort = EnvOrInt(config.embeddingPort, "EMBEDDING_PORT");
                config.embeddingKey = EnvOr(config.embeddingKey, "EMBEDDING_KEY", "EMBEDDING_API_KEY");
                config.llmKey = EnvOr(config.llmKey, "LLM_KEY", "DEEPSEEK_API_KEY");
                Console.WriteLine("  [OK] secrets.json loaded");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read secrets.json: {ex.Message}", ex);
            }
        }
        else
        {
            Console.WriteLine("  [INFO] secrets.json not found, falling back to environment variables");
            config.steamApiKey = EnvOr(config.steamApiKey, "STEAM_KEY", "STEAM_API_KEY");
            config.embeddingHost = EnvOr(config.embeddingHost, "EMBEDDING_HOST");
            config.embeddingPort = EnvOrInt(config.embeddingPort, "EMBEDDING_PORT");
            config.embeddingKey = EnvOr(config.embeddingKey, "EMBEDDING_KEY", "EMBEDDING_API_KEY");
            config.llmKey = EnvOr(config.llmKey, "LLM_KEY", "DEEPSEEK_API_KEY");
        }

        // Required secret checks.
        int fatalCount = 0;
        if (string.IsNullOrEmpty(config.steamApiKey))
        {
            GitHubActions.Error("STEAM_KEY is missing. Provide it in secrets.json or environment variables.", "Missing secret");
            fatalCount++;
        }
        if (string.IsNullOrEmpty(config.embeddingKey))
        {
            GitHubActions.Error("EMBEDDING_KEY is missing. Provide it in secrets.json or environment variables.", "Missing secret");
            fatalCount++;
        }
        if (string.IsNullOrEmpty(config.llmKey))
        {
            GitHubActions.Error("LLM_KEY is missing. Provide it in secrets.json or environment variables.", "Missing secret");
            fatalCount++;
        }
        fatalCount += ValidateConfigValues(config);
        if (fatalCount > 0)
            throw new InvalidOperationException($"{fatalCount} required configuration check(s) failed.");

        // Load supported languages from config file.
        string supportedLangsFile = Path.Combine(config.configDir, "supported_languages.json");
        if (!File.Exists(supportedLangsFile))
        {
            GitHubActions.Error($"Supported languages file not found: {supportedLangsFile}", "Missing config file");
            throw new FileNotFoundException($"Supported languages file not found: {supportedLangsFile}");
        }
        try
        {
            var opts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };
            var langs = JsonSerializer.Deserialize<List<LangInfoData>>(Utf8NoBom.ReadAllText(supportedLangsFile), opts);
            if (langs == null || langs.Count == 0)
            {
                GitHubActions.Error("Supported languages file is empty or invalid.", "Invalid config file");
                throw new InvalidOperationException("Supported languages file is empty or invalid.");
            }
            config.supportedLanguages = langs;
            Console.WriteLine($"  [OK] Loaded {langs.Count} supported languages from supported_languages.json");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to read supported_languages.json: {ex.Message}", ex);
        }

        // Optional input checks.
        string requestFile = Path.Combine(config.configDir, "request_for_translation.txt");
        if (!File.Exists(requestFile))
            GitHubActions.Warning("request_for_translation.txt not found; local mod ID list will be skipped.", "Optional input missing");
        else
            Console.WriteLine("  [OK] request_for_translation.txt");

        // Optional: reference translation mods.
        string refModsFile = Path.Combine(config.configDir, "ref_translation_mods.json");
        if (!File.Exists(refModsFile))
        {
            GitHubActions.Warning("ref_translation_mods.json not found, reference translation mods will be skipped.", "Optional config missing");
        }
        else
        {
            try
            {
                config.referenceTranslationMods = ReadReferenceTranslationMods(refModsFile);
                Console.WriteLine($"  [OK] Loaded {config.referenceTranslationMods.Count} reference translation mod(s) from ref_translation_mods.json");
            }
            catch (Exception ex)
            {
                GitHubActions.Warning($"Failed to read ref_translation_mods.json: {ex.Message}", "Config warning");
                config.referenceTranslationMods = new List<ModInfo>();
            }
        }

        return config;
    }

    // JSON helper methods.
    private static string GetString(JsonElement el, string prop, string fallback) =>
        el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString()! : fallback;

    private static int GetInt(JsonElement el, string prop, int fallback) =>
        el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out var n) ? n : fallback;

    private static float GetFloat(JsonElement el, string prop, float fallback) =>
        el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetSingle(out var f) ? f : fallback;

    private static bool GetBool(JsonElement el, string prop, bool fallback) =>
        el.TryGetProperty(prop, out var v) && (v.ValueKind == JsonValueKind.True || v.ValueKind == JsonValueKind.False) ? v.GetBoolean() : fallback;

    private static List<ModInfo> ReadReferenceTranslationMods(string refModsFile)
    {
        var result = new List<ModInfo>();
        using var doc = JsonDocument.Parse(Utf8NoBom.ReadAllText(refModsFile));
        if (doc.RootElement.ValueKind != JsonValueKind.Array)
            return result;

        foreach (var item in doc.RootElement.EnumerateArray())
        {
            var modInfo = new ModInfo();
            if (TryReadString(item, "mod_id", out var modId))
                modInfo.modId = modId;
            if (TryReadString(item, "mod_name", out var modName))
                modInfo.modName = modName;
            if (TryReadString(item, "language", out var language))
                modInfo.language = language;
            if (TryReadString(item, "mod_update_time", out var updateTime)
                && long.TryParse(updateTime, out var updateTimeSeconds))
            {
                modInfo.timeModUpdated = DateTimeOffset.FromUnixTimeSeconds(updateTimeSeconds).UtcDateTime;
            }
            if (TryReadString(item, "last_check_time", out var lastCheckTime)
                && DateTime.TryParse(lastCheckTime, out var parsedLastCheckTime))
            {
                modInfo.timeLastChecked = parsedLastCheckTime;
            }
            if (!string.IsNullOrWhiteSpace(modInfo.modId))
                result.Add(modInfo);
        }

        return result;
    }

    private static bool TryReadString(JsonElement item, string prop, out string value)
    {
        value = "";
        if (!item.TryGetProperty(prop, out var element))
            return false;

        if (element.ValueKind == JsonValueKind.String)
        {
            value = element.GetString() ?? "";
            return true;
        }

        if (element.ValueKind == JsonValueKind.Number)
        {
            value = element.GetRawText();
            return true;
        }

        return false;
    }

    private static string EnvOr(string fallback, params string[] names)
    {
        foreach (var name in names)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (!string.IsNullOrEmpty(value))
                return value;
        }

        return fallback;
    }

    private static int EnvOrInt(int fallback, string name) =>
        int.TryParse(Environment.GetEnvironmentVariable(name), out var n) ? n : fallback;

    private static int ValidateConfigValues(PipelineConfig config)
    {
        var errors = 0;

        errors += RequireUri(config.llmApiEndpoint, "LLM.api_endpoint");
        errors += RequireUri(config.asOneBaseUrl, "AsOne.base_url");
        errors += RequirePositive(config.llmMaxTokens, "LLM.max_tokens");
        errors += RequirePositive(config.llmBatchSize, "LLM.batch_size");
        errors += RequirePositive(config.llmBatchTokenBudget, "LLM.batch_token_budget");
        errors += RequireNonNegative(config.llmConcurrencyInitial, "LLM.concurrency.initial");
        errors += RequireNonNegative(config.llmConcurrencyMaximum, "LLM.concurrency.maximum");
        errors += RequirePositive(config.llmConcurrencyMinimum, "LLM.concurrency.minimum");
        errors += RequireNonNegative(config.llmConcurrencyMaxRetries, "LLM.concurrency.max_retries");
        errors += RequirePositive(config.llmConcurrencyFailureStreakToDecrease, "LLM.concurrency.failure_streak_to_decrease");
        errors += RequirePositive(config.llmConcurrencyRetryBaseDelayMs, "LLM.concurrency.retry_base_delay_ms");
        errors += RequirePositive(config.llmConcurrencyRetryMaxDelayMs, "LLM.concurrency.retry_max_delay_ms");
        errors += RequireNonNegative(config.llmFixedConcurrency, "LLM.concurrency.fixed_concurrency");
        errors += RequirePositive(config.embeddingPort, "Embedding.port");
        errors += RequirePositive(config.ragTopK, "RAG.top_k");
        errors += RequirePositive(config.pipelineBatchSize, "Pipeline.batch_size");
        errors += RequirePositive(config.steamApiChunkSize, "Steam.api_chunk_size");
        errors += RequirePositive(config.steamRequestTimeoutSeconds, "Steam.request_timeout_seconds");
        errors += RequireNonNegative(config.steamMaxRetries, "Steam.max_retries");
        errors += RequirePositive(config.contentCheckIntervalDays, "ContentCheck.check_interval_days");
        errors += RequireNonEmpty(config.priorityLanguage, "Settings.priority_language");
        errors += RequireNonEmpty(config.baseLanguage, "Settings.base_language");
        if (config.llmConcurrencyMaximum > 0 && config.llmConcurrencyMinimum > config.llmConcurrencyMaximum)
        {
            GitHubActions.Error("LLM.concurrency.minimum must not exceed LLM.concurrency.maximum.", "Invalid config value");
            errors++;
        }
        if (config.llmConcurrencyMaximum > 0 && config.llmConcurrencyInitial > config.llmConcurrencyMaximum)
        {
            GitHubActions.Error("LLM.concurrency.initial must not exceed LLM.concurrency.maximum.", "Invalid config value");
            errors++;
        }

        return errors;
    }

    private static int RequireUri(string value, string name)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out _))
            return 0;

        GitHubActions.Error($"{name} must be an absolute URI.", "Invalid config value");
        return 1;
    }

    private static int RequirePositive(int value, string name)
    {
        if (value > 0)
            return 0;

        GitHubActions.Error($"{name} must be greater than zero.", "Invalid config value");
        return 1;
    }

    private static int RequireNonNegative(int value, string name)
    {
        if (value >= 0)
            return 0;

        GitHubActions.Error($"{name} must be zero or greater.", "Invalid config value");
        return 1;
    }

    private static int RequireNonEmpty(string value, string name)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return 0;

        GitHubActions.Error($"{name} must not be empty.", "Invalid config value");
        return 1;
    }

    /// <summary>
    /// Creates the run temp folder and all subfolders, storing paths on the config.
    /// Folder paths on config are then available to all downstream modules.
    /// </summary>
    public void InitializeFolders(PipelineConfig config)
    {
        config.configDir = Path.Combine(config.baseDir, "config");
        if (!Directory.Exists(config.configDir))
        {
            Directory.CreateDirectory(config.configDir);
            GitHubActions.Warning($"config/ directory not found, created empty at {config.configDir}. Please add config.json and secrets.json before running the pipeline.", "Setup warning");
        }
        else
        {
            Console.WriteLine($"  [OK] config/ directory found at {config.configDir}");
        }
        config.dataDir = Path.Combine(config.baseDir, "data");
        if (!Directory.Exists(config.dataDir))
        {
            Directory.CreateDirectory(config.dataDir);
        }
        else
        {
            Console.WriteLine($"  [OK] data/ directory found at {config.dataDir}");
        }
        config.tempDir = Path.Combine(config.baseDir, "temp");
        if (!Directory.Exists(config.tempDir))
        {
            Directory.CreateDirectory(config.tempDir);
        }
        else
        {
            Console.WriteLine($"  [OK] temp/ directory found at {config.tempDir}");
        }

        // Create a unique run folder for this pipeline execution, and subfolders for each type of intermediate data. Store paths on the
        config.runTempDir = Path.Combine(config.baseDir, "temp", $"run_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString("N").Substring(0, 6)}");
        Directory.CreateDirectory(config.runTempDir);
        config.downloadedModsTempDir = MkSubDir(config.runTempDir, "downloaded_mods");
        config.extractedContentsTempDir = MkSubDir(config.runTempDir, "extracted_contents");
        config.downloadingBatchesTempDir = MkSubDir(config.runTempDir, "downloading_batches");
        config.extractedReferencesTempDir = MkSubDir(config.runTempDir, "extracted_references");
        config.contentCheckingPromptsTempDir = MkSubDir(config.runTempDir, "content_checking_prompts");
        config.contentCheckingResultsTempDir = MkSubDir(config.runTempDir, "content_checking_results");
        config.embeddingsTempDir = MkSubDir(config.runTempDir, "embeddings");
        config.translationBatchesTempDir = MkSubDir(config.runTempDir, "translation_batches");
        config.translationResultsTempDir = MkSubDir(config.runTempDir, "translation_results");
        config.warningsTempDir = MkSubDir(config.runTempDir, "warnings");

        Console.WriteLine($"  [OK] Temp folders created under {config.runTempDir}");
    }

    private static string MkSubDir(string parent, string name)
    {
        string p = Path.Combine(parent, name);
        Directory.CreateDirectory(p);
        return p;
    }
}
