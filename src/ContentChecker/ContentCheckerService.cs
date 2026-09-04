using Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ContentChecker;

/// <summary>
/// Checks mod-level content safety and filters entries for downstream translation.
/// </summary>
public class ContentCheckerService
{
    private const int MaxSampleCount = 1000;
    private const int MaxSampleChars = 60000;
    private const int MaxSingleTextChars = 1600;
    private const string ContentCheckModel = "deepseek-v4-flash";

    private readonly PipelineConfig _config;
    private readonly HttpClient? _httpClient;

    public ContentCheckerService(PipelineConfig config, HttpClient? httpClient = null)
    {
        _config = config;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Reviews mods when needed and fills the diff dictionary with approved untranslated entries.
    /// </summary>
    public async Task<TaskResult> CheckContentsAsync(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, TranslationEntry> diffTranslationEntryDict)
    {
        diffTranslationEntryDict.Clear();

        var nowUtc = DateTime.UtcNow;
        var baseLang = NormalizeLanguage(_config.baseLanguage);
        var warningCount = 0;
        var errorCount = 0;
        var totalModCount = 0;
        var reviewedModCount = 0;
        var acceptedModCount = 0;
        var rejectedModCount = 0;
        var pendingModCount = 0;
        var cachedModCount = 0;
        var disabledModCount = 0;
        var modResults = new List<object>();

        var entriesByMod = translationEntryDict.Values
            .Where(entry => !string.IsNullOrWhiteSpace(entry.modId))
            .GroupBy(entry => entry.modId, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);

        foreach (var (modId, entries) in entriesByMod.OrderBy(kvp => kvp.Key, StringComparer.Ordinal))
        {
            totalModCount++;
            if (!modInfoDict.TryGetValue(modId, out var modInfo))
                modInfo = new ModInfo { modId = modId };

            if (!_config.contentCheckEnabled)
            {
                disabledModCount++;
                var disabledLog = new
                {
                    modId,
                    modName = modInfo.modName,
                    action = "disabled",
                    status = modInfo.contentCheckStatus.ToString()
                };
                modResults.Add(disabledLog);
                LogModResult(disabledLog.action, modInfo, disabledLog.status, null, null, null, null);
                modInfoDict[modId] = modInfo;
                continue;
            }

            if (!NeedsContentReview(modInfo, nowUtc))
            {
                cachedModCount++;
                modInfoDict[modId] = modInfo;
                continue;
            }

            reviewedModCount++;
            try
            {
                var review = await ReviewModAsync(modInfo, entries, baseLang);
                ApplyReviewResult(ref modInfo, review, nowUtc);
                if (review.isHarmful)
                    rejectedModCount++;
                else
                    acceptedModCount++;

                modResults.Add(new
                {
                    modId,
                    modName = modInfo.modName,
                    action = "reviewed",
                    status = modInfo.contentCheckStatus.ToString(),
                    isHarmful = review.isHarmful,
                    review.confidence,
                    review.needHumanReview,
                    review.riskLevel,
                    review.reason
                });
                LogModResult("reviewed", modInfo, modInfo.contentCheckStatus.ToString(), review.riskLevel, review.confidence, review.reason, null);
            }
            catch (Exception ex)
            {
                errorCount++;
                warningCount++;
                pendingModCount++;
                modInfo.contentCheckStatus = ContentCheckStatus.NEEDVERIFICATION;
                modInfo.needsContentCheck = true;
                modInfo.timeNextContentCheck = nowUtc;
                modResults.Add(new
                {
                    modId,
                    modName = modInfo.modName,
                    action = "pending",
                    status = modInfo.contentCheckStatus.ToString(),
                    reason = ex.Message
                });
                LogModResult("pending", modInfo, modInfo.contentCheckStatus.ToString(), null, null, ex.Message, null);
                WarningFileWriter.Write(
                    _config,
                    "ContentChecker",
                    null,
                    new PipelineWarning
                    {
                        ModuleName = "ContentChecker",
                        ModId = modId,
                        ModName = modInfo.modName,
                        ErrorType = ex.GetType().Name,
                        Message = ex.Message
                    });
                GitHubActions.Warning($"Content review failed for mod {modId}: {ex.Message}", "ContentChecker");
            }

            modInfoDict[modId] = modInfo;
        }

        var queuedCount = FillTranslationQueue(modInfoDict, translationEntryDict, diffTranslationEntryDict, baseLang);

        var summary = new
        {
            totalModCount,
            reviewedModCount,
            acceptedModCount,
            rejectedModCount,
            pendingModCount,
            cachedModCount,
            disabledModCount,
            warningCount,
            errorCount,
            baseLang,
            queuedCount
        };

        WriteDebugSummary(summary, modResults);
        Console.WriteLine(
            $"  Content review summary: mods={totalModCount}, reviewed={reviewedModCount}, accepted={acceptedModCount}, rejected={rejectedModCount}, pending={pendingModCount}, cached={cachedModCount}, disabled={disabledModCount}, queued={queuedCount}, warnings={warningCount}, errors={errorCount}");

        return new TaskResult
        {
            isSuccess = errorCount == 0,
            errorCount = errorCount,
            warningCount = warningCount,
            summaryJson = Utf8NoBom.SerializeJson(summary)
        };
    }

    private bool NeedsContentReview(ModInfo modInfo, DateTime nowUtc)
    {
        // Rule 2: UNKNOWN + unavailable (delisted / Steam removed) — freeze, never re-review.
        // Available mods with UNKNOWN status are newly added and never content-checked → review them.
        if (modInfo.contentCheckStatus == ContentCheckStatus.UNKNOWN && !modInfo.isAvailable)
            return false;

        // NEEDVERIFICATION always triggers re-review.
        if (modInfo.contentCheckStatus == ContentCheckStatus.NEEDVERIFICATION)
            return true;

        bool expired = modInfo.timeNextContentCheck == DateTime.MinValue
            || NormalizeUtc(modInfo.timeNextContentCheck) <= nowUtc;

        // Rule 1: Missing details — reason & confidence both empty → incomplete review data.
        // Only check when expired (old cache may lack detail fields; future nextCheck = still valid).
        if (expired)
        {
            int missing = 0;
            if (string.IsNullOrWhiteSpace(modInfo.contentCheckReason)) missing++;
            if (modInfo.contentCheckConfidence <= 0) missing++;
            if (missing >= 2) return true;
        }

        // Rule 3: Has details, expired → re-review.
        return expired;
    }

    private bool CanTranslateMod(ModInfo modInfo)
    {
        if (!_config.contentCheckEnabled)
            return modInfo.contentCheckStatus != ContentCheckStatus.REJECTED;

        return modInfo.contentCheckStatus == ContentCheckStatus.ACCEPTED;
    }

    private int FillTranslationQueue(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, TranslationEntry> diffTranslationEntryDict,
        string baseLang)
    {
        var queuedCount = 0;
        foreach (var (entryKey, entry) in translationEntryDict.OrderBy(kvp => kvp.Key, StringComparer.Ordinal))
        {
            if (string.IsNullOrWhiteSpace(entry.modId)
                || string.IsNullOrWhiteSpace(entry.translationKey)
                || !modInfoDict.TryGetValue(entry.modId, out var modInfo)
                || !CanTranslateMod(modInfo))
            {
                continue;
            }

            diffTranslationEntryDict[entryKey] = entry;
            queuedCount++;
        }

        return queuedCount;
    }

    private async Task<ContentReviewResult> ReviewModAsync(
        ModInfo modInfo,
        List<TranslationEntry> entries,
        string baseLang)
    {
        var sample = BuildTextSample(modInfo.modId, entries, baseLang);
        var systemPrompt = LoadSystemPrompt();
        var userPrompt = BuildUserPrompt(modInfo, baseLang, sample);
        WritePromptDebug(modInfo, systemPrompt, userPrompt);

        var responseText = await SendContentReviewAsync(systemPrompt, userPrompt);
        var review = ParseReviewResponse(responseText);
        WriteResultDebug(modInfo, review, responseText);
        return review;
    }

    private async Task<string> SendContentReviewAsync(string systemPrompt, string userPrompt)
    {
        using var ownedClient = _httpClient == null ? new HttpClient() : null;
        var client = _httpClient ?? ownedClient!;
        if (ownedClient != null)
            client.Timeout = TimeSpan.FromSeconds(Math.Max(30, _config.steamRequestTimeoutSeconds));

        using var request = new HttpRequestMessage(HttpMethod.Post, _config.llmApiEndpoint);
        if (!string.IsNullOrWhiteSpace(_config.llmKey))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.llmKey);

        request.Content = new StringContent(
            Utf8NoBom.SerializeJson(new
            {
                model = ContentCheckModel,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                reasoning_effort = _config.llmReasoningEffort,
                thinking = new { type = "enabled" },
                temperature = _config.llmTemperature,
                max_tokens = Math.Clamp(_config.llmMaxTokens, 1, 4096)
            }),
            Utf8NoBom.Encoding,
            "application/json");

        using var response = await client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"LLM API returned {(int)response.StatusCode}: {responseBody}");

        using var doc = JsonDocument.Parse(responseBody);
        var choices = doc.RootElement.GetProperty("choices");
        if (choices.ValueKind != JsonValueKind.Array || choices.GetArrayLength() == 0)
            throw new InvalidDataException("LLM response missing choices.");

        var message = choices[0].GetProperty("message");
        if (message.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.String)
            return content.GetString() ?? "";

        throw new InvalidDataException("LLM response missing message content.");
    }

    private ContentReviewResult ParseReviewResponse(string responseText)
    {
        using var doc = JsonDocument.Parse(ExtractJsonObject(responseText));
        var root = doc.RootElement;

        var isHarmful = root.TryGetProperty("is_harmful", out var harmfulElement)
            && harmfulElement.ValueKind is JsonValueKind.True or JsonValueKind.False
            && harmfulElement.GetBoolean();
        var confidence = root.TryGetProperty("confidence", out var confidenceElement)
            && confidenceElement.ValueKind == JsonValueKind.Number
            && confidenceElement.TryGetDouble(out var parsedConfidence)
                ? parsedConfidence
                : 0.0;
        var needHumanReview = root.TryGetProperty("need_human_review", out var reviewElement)
            && reviewElement.ValueKind is JsonValueKind.True or JsonValueKind.False
            && reviewElement.GetBoolean();
        if (confidence < 0.7)
            needHumanReview = true;

        var riskLevel = root.TryGetProperty("risk_level", out var riskElement) && riskElement.ValueKind == JsonValueKind.String
            ? riskElement.GetString() ?? "safe"
            : isHarmful ? "high" : "safe";
        var reason = root.TryGetProperty("reason", out var reasonElement) && reasonElement.ValueKind == JsonValueKind.String
            ? reasonElement.GetString() ?? ""
            : "";
        var violatedRules = root.TryGetProperty("violated_rules", out var rulesElement) && rulesElement.ValueKind == JsonValueKind.Array
            ? rulesElement.EnumerateArray()
                .Where(item => item.ValueKind == JsonValueKind.String)
                .Select(item => item.GetString() ?? "")
                .Where(rule => !string.IsNullOrWhiteSpace(rule))
                .ToList()
            : [];

        return new ContentReviewResult(isHarmful, confidence, needHumanReview, riskLevel, reason, violatedRules);
    }

    private void ApplyReviewResult(ref ModInfo modInfo, ContentReviewResult review, DateTime nowUtc)
    {
        modInfo.contentCheckStatus = review.isHarmful
            ? ContentCheckStatus.REJECTED
            : ContentCheckStatus.ACCEPTED;
        modInfo.needsContentCheck = false;
        modInfo.timeNextContentCheck = nowUtc.AddDays(Math.Max(1, _config.contentCheckIntervalDays));
        modInfo.contentCheckConfidence = review.confidence;
        modInfo.contentCheckNeedHumanReview = review.needHumanReview;
        modInfo.contentCheckRiskLevel = review.riskLevel;
        modInfo.contentCheckReason = review.reason;
        modInfo.contentCheckViolatedRulesJson = Utf8NoBom.SerializeJson(review.violatedRules);
    }

    private static void LogModResult(
        string action,
        ModInfo modInfo,
        string status,
        string? riskLevel,
        double? confidence,
        string? reason,
        string? nextCheckUtc)
    {
        var name = string.IsNullOrWhiteSpace(modInfo.modName) ? modInfo.modId : modInfo.modName;
        var parts = new List<string>
        {
            $"  [content:{action}]",
            modInfo.modId,
            Shorten(name, 80),
            $"status={status}"
        };

        if (!string.IsNullOrWhiteSpace(riskLevel))
            parts.Add($"risk={riskLevel}");
        if (confidence != null)
            parts.Add($"confidence={confidence:0.###}");
        if (!string.IsNullOrWhiteSpace(nextCheckUtc))
            parts.Add($"next={nextCheckUtc}");
        if (!string.IsNullOrWhiteSpace(reason))
            parts.Add($"reason={Shorten(reason, 120)}");

        Console.WriteLine(string.Join(" | ", parts));
    }

    private List<ContentReviewText> BuildTextSample(string modId, List<TranslationEntry> entries, string baseLang)
    {
        var normalized = entries
            .OrderBy(entry => entry.translationKey, StringComparer.Ordinal)
            .Select(entry =>
            {
                var source = GetSourceText(entry, baseLang);
                return new ContentReviewText(entry.translationKey, source.lang, TrimSingleText(source.text));
            })
            .Where(item => !string.IsNullOrWhiteSpace(item.text))
            .ToList();

        if (normalized.Count > MaxSampleCount)
        {
            var rng = new Random(StableSeed(modId));
            normalized = normalized
                .OrderBy(_ => rng.Next())
                .Take(MaxSampleCount)
                .OrderBy(item => item.key, StringComparer.Ordinal)
                .ToList();
        }

        var totalChars = normalized.Sum(EstimatePromptChars);
        if (totalChars <= MaxSampleChars)
            return normalized;

        var rng2 = new Random(StableSeed(modId));
        var selected = new List<ContentReviewText>();
        var usedChars = 0;
        foreach (var item in normalized.OrderBy(_ => rng2.Next()))
        {
            var chars = EstimatePromptChars(item);
            if (usedChars + chars > MaxSampleChars)
                continue;

            selected.Add(item);
            usedChars += chars;
        }

        return selected.Count > 0
            ? selected.OrderBy(item => item.key, StringComparer.Ordinal).ToList()
            : [normalized[0] with { text = normalized[0].text[..Math.Min(normalized[0].text.Length, MaxSampleChars)] }];
    }

    private static int EstimatePromptChars(ContentReviewText item)
    {
        return item.key.Length + item.text.Length + 8;
    }

    private static string TrimSingleText(string text)
    {
        text = text.Trim();
        if (text.Length <= MaxSingleTextChars)
            return text;

        return text[..MaxSingleTextChars] + "\n[truncated]";
    }

    private static int StableSeed(string value)
    {
        unchecked
        {
            var hash = 17;
            foreach (var ch in value)
                hash = hash * 31 + ch;
            return hash;
        }
    }

    private string BuildUserPrompt(ModInfo modInfo, string baseLang, List<ContentReviewText> sample)
    {
        var payload = new
        {
            mod_id = modInfo.modId,
            mod_name = modInfo.modName,
            mod_description = CleanDescription(modInfo.description),
            base_language = baseLang,
            text_count = sample.Count,
            texts = sample.Select(item => new { key = item.key, lang = item.lang, text = item.text }).ToList()
        };

        return "请按系统规则审核以下 Project Zomboid 模组内容，只输出JSON。\n"
            + Utf8NoBom.SerializeIndentedJson(payload);
    }

    private string LoadSystemPrompt()
    {
        var path = Path.Combine(_config.baseDir, "src", "prompt_templates", "content_verification.txt");
        if (!File.Exists(path))
            throw new FileNotFoundException($"Content verification prompt not found: {path}");

        return Utf8NoBom.ReadAllText(path).Trim();
    }

    private void WritePromptDebug(ModInfo modInfo, string systemPrompt, string userPrompt)
    {
        if (string.IsNullOrWhiteSpace(_config.contentCheckingPromptsTempDir))
            return;

        var dir = Path.Combine(_config.contentCheckingPromptsTempDir, SafeFileName(modInfo.modId));
        Directory.CreateDirectory(dir);
        var messages = new[]
        {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = userPrompt }
        };
        Utf8NoBom.WriteAllText(
            Path.Combine(dir, "content_review_prompt.json"),
            Utf8NoBom.SerializeIndentedJson(messages));

        var md = new StringBuilder();
        md.AppendLine("# Content Review Prompt");
        md.AppendLine();
        md.AppendLine($"mod_id: {modInfo.modId}");
        md.AppendLine($"mod_name: {modInfo.modName}");
        md.AppendLine();
        md.AppendLine("## System Prompt");
        md.AppendLine("```text");
        md.AppendLine(systemPrompt);
        md.AppendLine("```");
        md.AppendLine();
        md.AppendLine("## User Prompt");
        md.AppendLine("```text");
        md.AppendLine(userPrompt);
        md.AppendLine("```");
        Utf8NoBom.WriteAllText(Path.Combine(dir, "content_review_prompt.md"), md.ToString());
    }

    private void WriteResultDebug(ModInfo modInfo, ContentReviewResult review, string rawContent)
    {
        if (string.IsNullOrWhiteSpace(_config.contentCheckingResultsTempDir))
            return;

        Directory.CreateDirectory(_config.contentCheckingResultsTempDir);
        var payload = new
        {
            mod_id = modInfo.modId,
            mod_name = modInfo.modName,
            is_harmful = review.isHarmful,
            confidence = review.confidence,
            need_human_review = review.needHumanReview,
            risk_level = review.riskLevel,
            reason = review.reason,
            violated_rules = review.violatedRules,
            checked_at_utc = DateTime.UtcNow.ToString("o"),
            raw_content = rawContent
        };

        Utf8NoBom.WriteAllText(
            Path.Combine(_config.contentCheckingResultsTempDir, $"{SafeFileName(modInfo.modId)}.json"),
            Utf8NoBom.SerializeIndentedJson(payload));

        var md = new StringBuilder();
        md.AppendLine("# Content Review Result");
        md.AppendLine();
        md.AppendLine($"mod_id: {modInfo.modId}");
        md.AppendLine($"mod_name: {modInfo.modName}");
        md.AppendLine($"checked_at_utc: {payload.checked_at_utc}");
        md.AppendLine($"status: {(review.isHarmful ? ContentCheckStatus.REJECTED : ContentCheckStatus.ACCEPTED)}");
        md.AppendLine($"is_harmful: {review.isHarmful}");
        md.AppendLine($"confidence: {review.confidence}");
        md.AppendLine($"need_human_review: {review.needHumanReview}");
        md.AppendLine($"risk_level: {review.riskLevel}");
        md.AppendLine();
        md.AppendLine("## Reason");
        md.AppendLine(review.reason);
        md.AppendLine();
        md.AppendLine("## Violated Rules");
        if (review.violatedRules.Count == 0)
            md.AppendLine("- none");
        foreach (var rule in review.violatedRules)
            md.AppendLine($"- {rule}");
        md.AppendLine();
        md.AppendLine("## Raw LLM Content");
        md.AppendLine("```json");
        md.AppendLine(rawContent);
        md.AppendLine("```");
        Utf8NoBom.WriteAllText(
            Path.Combine(_config.contentCheckingResultsTempDir, $"{SafeFileName(modInfo.modId)}.md"),
            md.ToString());
    }

    private void WriteDebugSummary(object summary, List<object> modResults)
    {
        if (string.IsNullOrWhiteSpace(_config.contentCheckingResultsTempDir))
            return;

        Directory.CreateDirectory(_config.contentCheckingResultsTempDir);
        var debugPath = Path.Combine(_config.contentCheckingResultsTempDir, "content_check_summary.json");
        var payload = new
        {
            summary,
            modResults
        };
        Utf8NoBom.WriteAllText(debugPath, Utf8NoBom.SerializeIndentedJson(payload));
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

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    private static TranslationSourceText GetSourceText(TranslationEntry entry, string baseLang)
    {
        return entry.GetBaseTextStrict(baseLang);
    }

    private static string ExtractJsonObject(string text)
    {
        var trimmed = StripFence(text.Trim());
        if (trimmed.StartsWith("{", StringComparison.Ordinal))
            return trimmed;

        var start = trimmed.IndexOf('{');
        var end = trimmed.LastIndexOf('}');
        if (start >= 0 && end > start)
            return trimmed[start..(end + 1)];

        throw new InvalidDataException("Content review response did not contain a JSON object.");
    }

    private static string StripFence(string text)
    {
        if (!text.StartsWith("```", StringComparison.Ordinal))
            return text;

        var lines = text.Split('\n').ToList();
        if (lines.Count > 0 && lines[0].StartsWith("```", StringComparison.Ordinal))
            lines.RemoveAt(0);
        if (lines.Count > 0 && lines[^1].Trim() == "```")
            lines.RemoveAt(lines.Count - 1);
        return string.Join('\n', lines).Trim();
    }

    private static string CleanDescription(string description)
    {
        return DescriptionCleaner.Clean(description);
    }

    private static string SafeFileName(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var chars = value.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray();
        var safe = new string(chars).Trim('_');
        return string.IsNullOrWhiteSpace(safe) ? "unknown" : safe;
    }

    private static string Shorten(string value, int maxLength)
    {
        value = value.Replace('\r', ' ').Replace('\n', ' ').Trim();
        return value.Length <= maxLength
            ? value
            : value[..Math.Max(0, maxLength - 3)] + "...";
    }

    private sealed record ContentReviewText(string key, string lang, string text);

    private sealed record ContentReviewResult(
        bool isHarmful,
        double confidence,
        bool needHumanReview,
        string riskLevel,
        string reason,
        List<string> violatedRules);
}
