using System.Text;
using System.Text.Json;
using Common;

namespace ProgressReporter;

/// <summary>
/// Generates translation progress reports for all planned languages.
/// Uses templates from src/prompt_templates/progress/ with {{PLACEHOLDER}} substitution.
/// Translation stats only shown for supported languages.
/// </summary>
public class ProgressReporterService
{
    private readonly PipelineConfig _config;
    private readonly string _progressDir;
    private readonly string _templateDir;

    public ProgressReporterService(PipelineConfig config)
    {
        _config = config;
        _progressDir = Path.Combine(_config.baseDir, "docs", "progress");
        _templateDir = Path.Combine(_config.baseDir, "src", "prompt_templates", "progress");
    }

    public async Task GenerateAsync(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, ModInfo> refModInfoDict)
    {
        var now = DateTime.Now;
        var dateStr = now.ToString("yyyy-MM-dd");
        var baseLang = NormalizeLang(_config.baseLanguage);

        // --- Load planned + supported languages ---
        var plannedLangs = LoadPlannedLanguages();
        var supportedIsoSet = _config.supportedLanguages
            .Select(l => l.isoCode.ToLowerInvariant())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // --- Compute stats for supported target languages ---
        var refModIds = refModInfoDict.Keys.ToHashSet(StringComparer.Ordinal);
        var acceptedMods = modInfoDict.Values
            .Where(m => m.contentCheckStatus == ContentCheckStatus.ACCEPTED && !refModIds.Contains(m.modId))
            .ToList();
        var acceptedModIds = acceptedMods.Select(m => m.modId).ToHashSet(StringComparer.Ordinal);
        var acceptedEntries = translationEntryDict.Values
            .Where(e => acceptedModIds.Contains(e.modId))
            .ToList();

        var targetLangs = _config.supportedLanguages
            .Where(l => !string.Equals(l.isoCode, baseLang, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var langModStats = new Dictionary<string, Dictionary<string, (int total, int translated, int pending, int untranslatable)>>(StringComparer.OrdinalIgnoreCase);
        foreach (var lang in targetLangs)
        {
            var li = lang.isoCode.ToLowerInvariant();
            langModStats[li] = new(StringComparer.Ordinal);
        }
        // Also track base language stats for priority language summary
        langModStats[baseLang] = new(StringComparer.Ordinal);

        foreach (var entry in acceptedEntries)
        {
            var baseText = entry.GetBaseTextStrict().text;
            var hasBaseText = !string.IsNullOrWhiteSpace(baseText);
            foreach (var lang in targetLangs)
            {
                var li = lang.isoCode.ToLowerInvariant();
                if (!langModStats[li].ContainsKey(entry.modId))
                    langModStats[li][entry.modId] = (0, 0, 0, 0);
                var s = langModStats[li][entry.modId];
                s.total++;
                if (!hasBaseText) s.untranslatable++;
                else if (entry.translationValues.TryGetValue(li, out var td) && td.IsProcessed && !string.IsNullOrWhiteSpace(td.text))
                    s.translated++;
                else s.pending++;
                langModStats[li][entry.modId] = s;
            }
            // Count base language entries
            if (!langModStats[baseLang].ContainsKey(entry.modId))
                langModStats[baseLang][entry.modId] = (0, 0, 0, 0);
            var bs = langModStats[baseLang][entry.modId];
            bs.total++;
            if (!hasBaseText) bs.untranslatable++;
            else bs.translated++;
            langModStats[baseLang][entry.modId] = bs;
        }

        var priLang = string.IsNullOrWhiteSpace(_config.priorityLanguage) ? "zh-hans" : _config.priorityLanguage.ToLowerInvariant();
        int totalMods = modInfoDict.Count;
        int acceptedCount = modInfoDict.Values.Count(m => m.contentCheckStatus == ContentCheckStatus.ACCEPTED);
        int rejectedCount = modInfoDict.Values.Count(m => m.contentCheckStatus == ContentCheckStatus.REJECTED);
        int unknownCount = modInfoDict.Values.Count(m => m.contentCheckStatus is ContentCheckStatus.UNKNOWN or ContentCheckStatus.NEEDVERIFICATION);
        int totalPriEntries = langModStats.TryGetValue(priLang, out var ps) ? ps.Values.Sum(x => x.total) : 0;
        int translatedPri = langModStats.TryGetValue(priLang, out ps) ? ps.Values.Sum(x => x.translated) : 0;
        int pendingPri = langModStats.TryGetValue(priLang, out ps) ? ps.Values.Sum(x => x.pending) : 0;
        int untranslatablePri = langModStats.TryGetValue(priLang, out ps) ? ps.Values.Sum(x => x.untranslatable) : 0;
        double progressPct = (totalPriEntries - untranslatablePri) > 0
            ? Math.Round(100.0 * translatedPri / (totalPriEntries - untranslatablePri), 1)
            : 0;

        Directory.CreateDirectory(_progressDir);

        // --- Pre-build table data rows (no headers; headers in templates) ---
        var rejectedRowsZh = BuildRejectedTableRows(modInfoDict, true);
        var rejectedRowsEn = BuildRejectedTableRows(modInfoDict, false);
        var allReviewRowsZh = BuildAllReviewTableRows(modInfoDict, true);
        var allReviewRowsEn = BuildAllReviewTableRows(modInfoDict, false);
        var overviewRowsZh = BuildOverviewTableRows(modInfoDict, acceptedEntries, langModStats, targetLangs, baseLang, true);
        var overviewRowsEn = BuildOverviewTableRows(modInfoDict, acceptedEntries, langModStats, targetLangs, baseLang, false);
        var perLangRowsZh = BuildPerLangRows(modInfoDict, langModStats, targetLangs, true);
        var perLangRowsEn = BuildPerLangRows(modInfoDict, langModStats, targetLangs, false);

        // --- Generate one progress file per planned language ---
        foreach (var lang in plannedLangs)
        {
            var iso = lang.isoCode.ToLowerInvariant();
            var isZhHans = iso == "zh-hans";
            var isZhHant = iso == "zh-hant";
            var isZh = isZhHans || isZhHant;

            var template = await LoadTemplateAsync(iso);
            if (template == null) continue;

            var rejectedRows = isZh ? rejectedRowsZh : rejectedRowsEn;
            var allReviewRows = isZh ? allReviewRowsZh : allReviewRowsEn;
            var overviewRows = isZh ? overviewRowsZh : overviewRowsEn;
            var perLangRows = isZh ? perLangRowsZh : perLangRowsEn;
            var noData = isZhHans ? "暂无数据。" : (isZhHant ? "暫無數據。" : "No data available.");
            var noRejected = isZhHans ? "无拒绝模组。" : (isZhHant ? "無拒絕模組。" : "No rejected mods.");

            var langLinks = BuildLanguageLinks(plannedLangs, iso);
            var perLangHeader = isZhHans ? "| 模组ID | 模组名称 | 总条目 | 待翻译 | 已翻译 |\n|--------|----------|--------|--------|--------|" : (isZhHant ? "| 模組ID | 模組名稱 | 總條目 | 待翻譯 | 已翻譯 |\n|--------|----------|--------|--------|--------|" : "| Mod ID | Mod Name | Total | Pending | Translated |\n|--------|----------|-------|---------|------------|");
            var perLangSections = BuildPerLangSections(targetLangs, perLangRows, isZh, noData, perLangHeader);

            var output = template
                .Replace("{{LANGUAGE_LINKS}}", langLinks)
                .Replace("{{DATE}}", dateStr)
                .Replace("{{TOTAL_ENTRIES}}", totalPriEntries.ToString())
                .Replace("{{UNTRANSLATABLE_ENTRIES}}", untranslatablePri.ToString())
                .Replace("{{TRANSLATED_ENTRIES}}", translatedPri.ToString())
                .Replace("{{PENDING_ENTRIES}}", pendingPri.ToString())
                .Replace("{{PROGRESS_PCT}}", progressPct.ToString())
                .Replace("{{TOTAL_MODS}}", totalMods.ToString())
                .Replace("{{ACCEPTED_MODS}}", acceptedCount.ToString())
                .Replace("{{REJECTED_MODS}}", rejectedCount.ToString())
                .Replace("{{UNKNOWN_MODS}}", unknownCount.ToString())
                .Replace("{{REJECTED_TABLE}}", rejectedRows.Length > 0 ? rejectedRows : noRejected)
                .Replace("{{ALL_REVIEW_TABLE}}", allReviewRows.Length > 0 ? allReviewRows : noData)
                .Replace("{{OVERVIEW_TABLE}}", overviewRows.Length > 0 ? overviewRows : noData)
                .Replace("{{PER_LANG_SECTIONS}}", perLangSections);

            var outPath = Path.Combine(_progressDir, $"progress_{iso}.md");
            await File.WriteAllTextAsync(outPath, output, Utf8NoBom.Encoding);
        }

        Console.WriteLine($"  [OK] Progress reports written to docs/progress/ ({plannedLangs.Count} files)");
    }

    // ======================== Template / Language helpers ========================

    private List<LangInfoData> LoadPlannedLanguages()
    {
        var path = Path.Combine(_config.configDir, "supported_languages_example.json");
        if (!File.Exists(path))
        {
            Console.WriteLine("  [WARN] supported_languages_example.json not found, using supported languages as fallback.");
            return _config.supportedLanguages;
        }
        try
        {
            var opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, PropertyNameCaseInsensitive = true };
            var langs = JsonSerializer.Deserialize<List<LangInfoData>>(Utf8NoBom.ReadAllText(path), opts);
            return langs ?? _config.supportedLanguages;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [WARN] Failed to load planned languages: {ex.Message}");
            return _config.supportedLanguages;
        }
    }

    private async Task<string?> LoadTemplateAsync(string isoCode)
    {
        var path = Path.Combine(_templateDir, $"progress_template_{isoCode}.md");
        if (!File.Exists(path))
        {
            Console.WriteLine($"  [WARN] Template not found for {isoCode}, skipping.");
            return null;
        }
        return await File.ReadAllTextAsync(path, Utf8NoBom.Encoding);
    }

    /// <summary>
    /// Multi-language links: [简体中文](../../README.md) | [English](progress_en.md) | &lt;details&gt;...
    /// </summary>
    private static string BuildLanguageLinks(List<LangInfoData> plannedLangs, string currentIso)
    {
        var sb = new StringBuilder();
        sb.Append("[简体中文](../../README.md)");
        if (!string.Equals(currentIso, "en", StringComparison.OrdinalIgnoreCase))
            sb.Append(" | [English](progress_en.md)");
        var others = plannedLangs
            .Where(l => !string.Equals(l.isoCode, "zh-hans", StringComparison.OrdinalIgnoreCase)
                     && !string.Equals(l.isoCode, "en", StringComparison.OrdinalIgnoreCase)
                     && !string.Equals(l.isoCode, currentIso, StringComparison.OrdinalIgnoreCase))
            .Select(l => $"[{l.nativeName}](progress_{l.isoCode.ToLowerInvariant()}.md)")
            .ToList();
        if (others.Count > 0)
        {
            sb.Append(" <details><summary>其它语言</summary>");
            sb.Append(string.Join(" | ", others));
            sb.Append("</details>");
        }
        sb.AppendLine();
        return sb.ToString();
    }

    // ======================== Per-language sections ========================

    private static string BuildPerLangSections(
        List<LangInfoData> targetLangs,
        Dictionary<string, string> perLangRows,
        bool isZh,
        string noData,
        string perLangHeader)
    {
        var sb = new StringBuilder();
        foreach (var lang in targetLangs)
        {
            var li = lang.isoCode.ToLowerInvariant();
            var name = isZh ? $"{li} - {lang.chineseName} ({lang.englishName})" : $"{li} - {lang.englishName} ({lang.nativeName})";
            var rows = perLangRows.TryGetValue(li, out var r) ? r : "";
            sb.AppendLine("<details>");
            sb.AppendLine($"<summary>{name}</summary>");
            sb.AppendLine();
            if (!string.IsNullOrEmpty(rows))
            {
                sb.AppendLine(perLangHeader);
                sb.AppendLine(rows);
            }
            else
            {
                sb.AppendLine(noData);
            }
            sb.AppendLine();
            sb.AppendLine("</details>");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static Dictionary<string, string> BuildPerLangRows(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, Dictionary<string, (int total, int translated, int pending, int untranslatable)>> langModStats,
        List<LangInfoData> targetLangs,
        bool isZh)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var lang in targetLangs)
        {
            var li = lang.isoCode.ToLowerInvariant();
            result[li] = BuildPerLangTableRows(modInfoDict, langModStats, li, isZh);
        }
        return result;
    }

    // ======================== Table builders ========================

    private string NormalizeLang(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang)) return "en";
        var m = _config.supportedLanguages.FirstOrDefault(l =>
            string.Equals(l.ingameCode, lang, StringComparison.OrdinalIgnoreCase)
            || string.Equals(l.isoCode, lang, StringComparison.OrdinalIgnoreCase));
        return string.IsNullOrWhiteSpace(m?.isoCode) ? lang.ToLowerInvariant() : m.isoCode.ToLowerInvariant();
    }

    private static List<string> ParseViolatedRules(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];
        try { return JsonSerializer.Deserialize<List<string>>(json) ?? []; }
        catch { return []; }
    }

    private static string BuildRejectedTableRows(Dictionary<string, ModInfo> modInfoDict, bool isZh)
    {
        var rejected = modInfoDict.Values
            .Where(m => m.contentCheckStatus == ContentCheckStatus.REJECTED)
            .OrderBy(m => m.modId, StringComparer.Ordinal)
            .ToList();
        if (rejected.Count == 0) return "";

        var sb = new StringBuilder();
        foreach (var mod in rejected)
        {
            var hasDetail = !string.IsNullOrWhiteSpace(mod.contentCheckReason) || mod.contentCheckConfidence > 0;
            var violated = ParseViolatedRules(mod.contentCheckViolatedRulesJson);
            var keywords = violated.Count > 0 ? string.Join(", ", violated) : (hasDetail ? "-" : (isZh ? "(历史记录)" : "(historical)"));
            var reason = hasDetail && !string.IsNullOrWhiteSpace(mod.contentCheckReason) ? mod.contentCheckReason : (hasDetail ? "-" : (isZh ? "(历史记录，详情已过期)" : "(historical, detail expired)"));
            if (reason.Length > 100) reason = reason[..97] + "...";
            var confidence = hasDetail && mod.contentCheckConfidence > 0 ? mod.contentCheckConfidence.ToString("0.##") : "-";
            var needReview = hasDetail && mod.contentCheckNeedHumanReview ? (isZh ? "是" : "Yes") : (isZh ? "否" : "No");
            var nextCheck = mod.timeNextContentCheck != DateTime.MinValue ? mod.timeNextContentCheck.ToString("yyyy-MM-dd") : "-";
            var name = string.IsNullOrWhiteSpace(mod.modName) ? mod.modId : mod.modName;
            if (name.Length > 40) name = name[..37] + "...";
            sb.AppendLine($"| {mod.modId} | {EscapeMd(name)} | {EscapeMd(keywords)} | {EscapeMd(reason)} | {confidence} | {needReview} | {nextCheck} |");
        }
        return sb.ToString();
    }

    private static string BuildAllReviewTableRows(Dictionary<string, ModInfo> modInfoDict, bool isZh)
    {
        var all = modInfoDict.Values.OrderBy(m => m.modId, StringComparer.Ordinal).ToList();
        if (all.Count == 0) return "";

        var sb = new StringBuilder();
        foreach (var mod in all)
        {
            var status = mod.contentCheckStatus.ToString();
            var risk = string.IsNullOrWhiteSpace(mod.contentCheckRiskLevel) ? "-" : mod.contentCheckRiskLevel;
            var confidence = mod.contentCheckConfidence > 0 ? mod.contentCheckConfidence.ToString("0.##") : "-";
            var reason = string.IsNullOrWhiteSpace(mod.contentCheckReason) ? "-" : mod.contentCheckReason;
            if (reason.Length > 80) reason = reason[..77] + "...";
            var nextCheck = mod.timeNextContentCheck != DateTime.MinValue ? mod.timeNextContentCheck.ToString("yyyy-MM-dd") : "-";
            var name = string.IsNullOrWhiteSpace(mod.modName) ? mod.modId : mod.modName;
            if (name.Length > 40) name = name[..37] + "...";
            sb.AppendLine($"| {mod.modId} | {EscapeMd(name)} | {status} | {EscapeMd(risk)} | {confidence} | {EscapeMd(reason)} | {nextCheck} |");
        }
        return sb.ToString();
    }

    private static string BuildOverviewTableRows(
        Dictionary<string, ModInfo> modInfoDict,
        List<TranslationEntry> acceptedEntries,
        Dictionary<string, Dictionary<string, (int total, int translated, int pending, int untranslatable)>> langModStats,
        List<LangInfoData> targetLangs,
        string baseLanguage,
        bool isZh)
    {
        var modEntryStatus = new Dictionary<string, (int fullyTranslated, int hasPending)>(StringComparer.Ordinal);
        foreach (var group in acceptedEntries.GroupBy(e => e.modId))
        {
            int fullyTranslated = 0, hasPending = 0;
            foreach (var entry in group)
            {
                var baseText = entry.GetBaseTextStrict().text;
                if (string.IsNullOrWhiteSpace(baseText)) continue;
                bool allDone = true, anyPending = false;
                foreach (var lang in targetLangs)
                {
                    var li = lang.isoCode.ToLowerInvariant();
                    if (entry.translationValues.TryGetValue(li, out var td) && td.IsProcessed && !string.IsNullOrWhiteSpace(td.text))
                    { }
                    else { allDone = false; anyPending = true; }
                }
                if (allDone) fullyTranslated++;
                else if (anyPending) hasPending++;
            }
            modEntryStatus[group.Key] = (fullyTranslated, hasPending);
        }

        var modEntryCounts = acceptedEntries.GroupBy(e => e.modId).ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
        var acceptedMods = modInfoDict.Values
            .Where(m => m.contentCheckStatus == ContentCheckStatus.ACCEPTED)
            .OrderByDescending(m => modEntryCounts.TryGetValue(m.modId, out var c) ? c : 0)
            .ToList();
        if (acceptedMods.Count == 0) return "";

        var sb = new StringBuilder();
        foreach (var mod in acceptedMods)
        {
            var name = string.IsNullOrWhiteSpace(mod.modName) ? mod.modId : mod.modName;
            if (name.Length > 30) name = name[..27] + "...";
            var origLang = !string.IsNullOrWhiteSpace(mod.language) ? mod.language : baseLanguage;
            var total = modEntryCounts.TryGetValue(mod.modId, out var c) ? c : 0;
            var (fullyTranslated, hasPending) = modEntryStatus.TryGetValue(mod.modId, out var es) ? es : (0, 0);
            sb.AppendLine($"| {mod.modId} | {EscapeMd(name)} | {origLang} | {total} | {hasPending} | {fullyTranslated} | {mod.subscription} |");
        }
        return sb.ToString();
    }

    private static string BuildPerLangTableRows(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, Dictionary<string, (int total, int translated, int pending, int untranslatable)>> langModStats,
        string langIso,
        bool isZh)
    {
        if (!langModStats.TryGetValue(langIso, out var stats)) return "";

        var acceptedMods = modInfoDict.Values
            .Where(m => m.contentCheckStatus == ContentCheckStatus.ACCEPTED)
            .ToList();
        var ordered = acceptedMods
            .Select(m => (mod: m, s: stats.TryGetValue(m.modId, out var st) ? st : (total: 0, translated: 0, pending: 0, untranslatable: 0)))
            .Where(x => x.s.total > 0)
            .OrderByDescending(x => x.s.total)
            .ToList();
        if (ordered.Count == 0) return "";

        var sb = new StringBuilder();
        foreach (var (mod, s) in ordered)
        {
            var name = string.IsNullOrWhiteSpace(mod.modName) ? mod.modId : mod.modName;
            if (name.Length > 40) name = name[..37] + "...";
            sb.AppendLine($"| {mod.modId} | {EscapeMd(name)} | {s.total} | {s.pending} | {s.translated} |");
        }
        return sb.ToString();
    }

    private static string EscapeMd(string text)
    {
        return text.Replace("|", "\\|").Replace("\n", " ").Replace("\r", " ");
    }

    private sealed class TableHeaders
    {
        public string Rejected { get; set; } = "";
        public string AllReview { get; set; } = "";
        public string Overview { get; set; } = "";
        public string PerLang { get; set; } = "";
    }
}
