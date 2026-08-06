using Common;
using PercentNormalizer;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ContentExtractor;

/// <summary>
/// Extracts translatable text from downloaded mod files.
/// </summary>
public class ContentExtractorService
{
    private readonly PipelineConfig _config;

    private static readonly string[] DefaultGameCodes =
    [
        "AR", "CA", "CH", "CN", "CS", "DA", "DE", "EN", "ES", "FI",
        "FR", "HU", "ID", "IT", "JP", "KO", "NL", "NO", "PH", "PL",
        "PT", "PTBR", "RO", "RU", "TH", "TR", "UA"
    ];

    private static readonly Dictionary<string, string> DefaultIsoByGameCode = new(StringComparer.OrdinalIgnoreCase)
    {
        ["AR"] = "ar",
        ["CA"] = "ca",
        ["CH"] = "zh-hant",
        ["CN"] = "zh-hans",
        ["CS"] = "cs",
        ["DA"] = "da",
        ["DE"] = "de",
        ["EN"] = "en",
        ["ES"] = "es",
        ["FI"] = "fi",
        ["FR"] = "fr",
        ["HU"] = "hu",
        ["ID"] = "id",
        ["IT"] = "it",
        ["JP"] = "ja",
        ["KO"] = "ko",
        ["NL"] = "nl",
        ["NO"] = "no",
        ["PH"] = "tl",
        ["PL"] = "pl",
        ["PT"] = "pt",
        ["PTBR"] = "pt-br",
        ["RO"] = "ro",
        ["RU"] = "ru",
        ["TH"] = "th",
        ["TR"] = "tr",
        ["UA"] = "uk"
    };

    private static readonly string[] SkippedFileNames =
    [
        "translationnotes", "translationby", "code - txt", "credits", "language"
    ];

    public ContentExtractorService(PipelineConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Extracts translation entries and writes old-format extracted output files.
    /// </summary>
    public Task<TaskResult> ExtractContentsAsync(
        Dictionary<string, ModInfo> batchModInfoDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        string? batchId = null)
    {
        var langMap = BuildLanguageMap();
        var baseIso = NormalizeToIso(_config.baseLanguage, langMap);
        var warningCount = 0;

        foreach (var (modId, modInfo) in batchModInfoDict)
        {
            if (!Directory.Exists(modInfo.localDownloadedPath))
            {
                GitHubActions.Warning($"Downloaded mod folder not found: {modInfo.localDownloadedPath}", "ContentExtractor");
                warningCount++;
                continue;
            }

            var modEntries = ExtractMod(modId, modInfo, batchId, langMap, baseIso);
            foreach (var (key, value) in modEntries)
                translationEntryDict[key] = value;
        }

        return Task.FromResult(new TaskResult
        {
            isSuccess = true,
            warningCount = warningCount
        });
    }

    private Dictionary<string, TranslationEntry> ExtractMod(
        string modId,
        ModInfo mod,
        string? batchId,
        Dictionary<string, string> langMap,
        string baseIso)
    {
        var entries = new Dictionary<string, TranslationEntry>(StringComparer.Ordinal);
        var winners = new Dictionary<string, WinnerInfo>(StringComparer.OrdinalIgnoreCase);
        var diagnostics = new List<string>();
        var translateDirs = FindTranslateDirectories(mod.localDownloadedPath);

        if (translateDirs.Count == 0)
        {
            Console.WriteLine($"  {modId}: no Translate folder found");
            return entries;
        }

        using var fuckWriter = CreateFuckWriter();

        foreach (var translateDir in translateDirs)
        {
            foreach (var langDir in FindLanguageSubdirs(translateDir, langMap.Keys))
            {
                var gameCode = Path.GetFileName(langDir).ToUpperInvariant();
                var isoCode = langMap.GetValueOrDefault(gameCode, gameCode.ToLowerInvariant());

                foreach (var file in Directory.GetFiles(langDir).OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
                {
                    var ext = Path.GetExtension(file);
                    if (!ext.Equals(".txt", StringComparison.OrdinalIgnoreCase)
                        && !ext.Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        var fileInfo = BuildContainingFileInfo(file, mod.localDownloadedPath);
                        var sourceKind = ext.Equals(".json", StringComparison.OrdinalIgnoreCase)
                            ? SourceKind.Json
                            : SourceKind.Text;
                        var rawEntries = sourceKind == SourceKind.Json
                            ? ParseJsonFile(file, modId, gameCode, isoCode, fileInfo)
                            : ParseTextFile(file, modId, gameCode, isoCode, fileInfo, diagnostics, fuckWriter);

                        foreach (var raw in rawEntries)
                            MergeRawEntry(entries, winners, raw, sourceKind, baseIso);
                    }
                    catch (Exception ex)
                    {
                        WriteRawModWarning(mod, file, batchId, ex);
                        GitHubActions.Warning($"Failed to parse {file}: {ex.Message}", "ContentExtractor");
                    }
                }
            }
        }

        WriteDiagnostics(diagnostics);
        WriteOldFormatOutputs(modId, entries, winners, baseIso);
        Console.WriteLine($"  {modId}: extracted {entries.Count} translation entries to {GetOutputBase()}");
        return entries;
    }

    // Directory discovery.

    private static List<string> FindTranslateDirectories(string modRoot)
    {
        return Directory
            .EnumerateDirectories(modRoot, "*", SearchOption.AllDirectories)
            .Where(dir => string.Equals(Path.GetFileName(dir), "Translate", StringComparison.OrdinalIgnoreCase))
            .OrderBy(dir => dir, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static List<string> FindLanguageSubdirs(string translateDir, IEnumerable<string> knownCodes)
    {
        var codeSet = knownCodes.ToHashSet(StringComparer.OrdinalIgnoreCase);
        return Directory
            .GetDirectories(translateDir)
            .Where(dir => codeSet.Contains(Path.GetFileName(dir)))
            .OrderBy(dir => dir, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // JSON parser.

    private static List<RawTranslationEntry> ParseJsonFile(
        string file,
        string modId,
        string gameCode,
        string isoCode,
        ContainingFileInfo fileInfo)
    {
        var result = new List<RawTranslationEntry>();
        var masterKey = BuildMasterKeyFromFile(file, gameCode);
        var jsonText = Utf8NoBom.ReadAllText(file);
        using var doc = JsonDocument.Parse(jsonText, new JsonDocumentOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        });

        foreach (var (key, value) in EnumerateJsonPairs(doc.RootElement))
        {
            if (string.IsNullOrWhiteSpace(key))
                continue;

            result.Add(new RawTranslationEntry(
                modId,
                masterKey,
                key.Trim(),
                isoCode,
                value,
                fileInfo));
        }

        return result;
    }

    private static IEnumerable<(string Key, string Value)> EnumerateJsonPairs(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
            yield break;

        foreach (var prop in element.EnumerateObject())
        {
            if (prop.Value.ValueKind == JsonValueKind.String)
            {
                yield return (prop.Name, prop.Value.GetString() ?? "");
            }
            else if (prop.Value.ValueKind == JsonValueKind.Object)
            {
                foreach (var nested in EnumerateJsonPairs(prop.Value))
                    yield return nested;
            }
        }
    }

    // TXT parser. Keep the layered shape from todo.txt.

    private static List<RawTranslationEntry> ParseTextFile(
        string file,
        string modId,
        string gameCode,
        string isoCode,
        ContainingFileInfo fileInfo,
        List<string> diagnostics,
        StreamWriter? fuckWriter)
    {
        var fileName = Path.GetFileName(file);
        var lines = Utf8NoBom.ReadAllLines(file);
        if (!FilterTranslationFilesAndFindMasterKey(fileName, gameCode, lines, out var masterKey, out var startLine))
            return [];

        var entries = ExtractTranslationKeys(
            modId,
            fileName,
            lines,
            masterKey,
            startLine,
            isoCode,
            fileInfo,
            out var failedLines,
            fuckWriter);

        diagnostics.AddRange(FilterLuaCommentBlocks(failedLines));
        return entries;
    }

    private static bool FilterTranslationFilesAndFindMasterKey(
        string fileName,
        string gameCode,
        string[] allLines,
        out string masterKey,
        out int startLine)
    {
        masterKey = "";
        startLine = -1;

        foreach (var skipped in SkippedFileNames)
        {
            if (fileName.Contains(skipped, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        var keyWithBraceRegex = new Regex(
            @"^\s*(?<fullKey>[A-Za-z_]\w*(?:\.[A-Za-z_]\w*)*(?:\[""[^""]*""\])*)\s*(?:=\s*)?\{",
            RegexOptions.Compiled);
        var keyOnlyRegex = new Regex(
            @"^\s*(?<fullKey>[A-Za-z_]\w*(?:\.[A-Za-z_]\w*)*(?:\[""[^""]*""\])*)\s*$",
            RegexOptions.Compiled);
        var jsonStartRegex = new Regex(@"^\s*\{\s*$", RegexOptions.Compiled);
        var emptyTableRegex = new Regex(@"^\s*\{\s*\}\s*$", RegexOptions.Compiled);

        for (int i = 0; i < allLines.Length; i++)
        {
            var braceMatch = keyWithBraceRegex.Match(allLines[i]);
            if (braceMatch.Success)
            {
                masterKey = NormalizeMasterKey(braceMatch.Groups["fullKey"].Value, gameCode);
                startLine = i + 1;
                return true;
            }

            var keyMatch = keyOnlyRegex.Match(allLines[i]);
            if (!keyMatch.Success)
                continue;

            int j = i + 1;
            while (j < allLines.Length && string.IsNullOrWhiteSpace(allLines[j]))
                j++;
            if (j >= allLines.Length)
                continue;

            if (Regex.IsMatch(allLines[j], @"^\s*\{"))
            {
                masterKey = NormalizeMasterKey(keyMatch.Groups["fullKey"].Value, gameCode);
                startLine = j + 1;
                return true;
            }

            if (LooksLikeTranslationEntry(allLines[j]))
            {
                masterKey = NormalizeMasterKey(keyMatch.Groups["fullKey"].Value, gameCode);
                startLine = j;
                return true;
            }
        }

        for (int i = 0; i < allLines.Length; i++)
        {
            if (jsonStartRegex.IsMatch(allLines[i]) || emptyTableRegex.IsMatch(allLines[i]))
            {
                masterKey = BuildMasterKeyFromFile(fileName, gameCode);
                startLine = i + 1;
                return true;
            }
        }

        int firstContentLine = -1;
        for (int i = 0; i < allLines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(allLines[i]))
                continue;
            if (Regex.IsMatch(allLines[i], @"^\s*(--|//|/\*|\*)"))
                continue;
            firstContentLine = i;
            break;
        }

        if (firstContentLine >= 0 && LooksLikeTranslationEntry(allLines[firstContentLine]))
        {
            masterKey = BuildMasterKeyFromFile(fileName, gameCode);
            startLine = firstContentLine;
            return true;
        }

        return false;
    }

    private static List<RawTranslationEntry> ExtractTranslationKeys(
        string modId,
        string fileName,
        string[] allLines,
        string masterKey,
        int startLine,
        string isoCode,
        ContainingFileInfo fileInfo,
        out List<string> failedLines,
        StreamWriter? fuckWriter)
    {
        var entries = new List<RawTranslationEntry>();
        failedLines = [];

        var closingBraceRegex = new Regex(@"^\s*\}\s*$", RegexOptions.Compiled);
        var openingBraceRegex = new Regex(@"^\s*\{\s*$", RegexOptions.Compiled);
        var pureClosingRegex = new Regex(@"^\s*[)}\);,]+\s*,?\s*$", RegexOptions.Compiled);
        var braceWithCommentRegex = new Regex(@"^\s*\}\s*--", RegexOptions.Compiled);
        var isolatedCommentRegex = new Regex(@"^\s*;", RegexOptions.Compiled);

        for (int i = startLine; i < allLines.Length; i++)
        {
            var line = allLines[i];

            if (closingBraceRegex.IsMatch(line))
                break;
            if (string.IsNullOrWhiteSpace(line))
                continue;
            if (Regex.IsMatch(line, @"^\s*(--|//|/\*|\*)"))
                continue;
            if (pureClosingRegex.IsMatch(line))
                continue;
            if (braceWithCommentRegex.IsMatch(line))
                continue;
            if (isolatedCommentRegex.IsMatch(line))
                continue;

            var match = KVRegex.PzEntry.Match(line);
            if (match.Success)
            {
                AddRawEntry(entries, modId, masterKey, match.Groups["key"].Value, isoCode, match.Groups["value"].Value, fileInfo);
                continue;
            }

            match = KVRegex.JsonEntry.Match(line);
            if (match.Success)
            {
                AddRawEntry(entries, modId, masterKey, match.Groups["key"].Value, isoCode, match.Groups["value"].Value, fileInfo);
                continue;
            }

            var luaStart = KVRegex.LuaConcatStart.Match(line);
            if (luaStart.Success)
            {
                var key = luaStart.Groups["key"].Value.Trim();
                var sb = new StringBuilder(luaStart.Groups["value"].Value);
                var foundEnd = false;
                i++;

                while (i < allLines.Length)
                {
                    var nextLine = allLines[i].Trim();
                    if (string.IsNullOrEmpty(nextLine))
                    {
                        i++;
                        continue;
                    }

                    var endMatch = KVRegex.LuaConcatEnd.Match(nextLine);
                    if (endMatch.Success)
                    {
                        sb.Append(endMatch.Groups["value"].Value);
                        foundEnd = true;
                        break;
                    }

                    var contMatch = KVRegex.LuaConcatContinue.Match(nextLine);
                    if (contMatch.Success)
                    {
                        sb.Append(contMatch.Groups["value"].Value);
                        i++;
                        continue;
                    }

                    break;
                }

                if (!string.IsNullOrEmpty(key) && foundEnd)
                    AddRawEntry(entries, modId, masterKey, key, isoCode, sb.ToString(), fileInfo);
                else
                    failedLines.Add($"[{fileName}:{i + 1}] {line.Trim()} (Lua concat format error)");
                continue;
            }

            var switchKeyMatch = Regex.Match(
                line,
                @"^\s*(?<key>[A-Za-z_]\w*(?:\.[A-Za-z_]\w*)*)\s*(?:=\s*)?\{\s*$");
            if (switchKeyMatch.Success)
            {
                var candidateKey = switchKeyMatch.Groups["key"].Value.Trim();
                if (candidateKey.Contains('_') && candidateKey.Length > 1)
                {
                    masterKey = candidateKey;
                    continue;
                }
            }

            if (TryMatchRelaxed(line, out var relaxedKey, out var relaxedValue))
            {
                AddRawEntry(entries, modId, masterKey, relaxedKey, isoCode, relaxedValue, fileInfo);
                fuckWriter?.WriteLine($"FUCK: {modId}");
                fuckWriter?.WriteLine($"\tFUCK: {fileName}");
                fuckWriter?.WriteLine($"\tFUCK: {line.Trim()}");
                continue;
            }

            if (!openingBraceRegex.IsMatch(line))
                failedLines.Add($"[{fileName}:{i + 1}] {line.Trim()}");
        }

        return entries;
    }

    private static void AddRawEntry(
        List<RawTranslationEntry> entries,
        string modId,
        string masterKey,
        string key,
        string isoCode,
        string value,
        ContainingFileInfo fileInfo)
    {
        key = key.Trim();
        if (string.IsNullOrEmpty(key))
            return;

        entries.Add(new RawTranslationEntry(modId, masterKey, key, isoCode, value, fileInfo));
    }

    private static bool TryMatchRelaxed(string line, out string key, out string value)
    {
        key = "";
        value = "";

        var m = KVRegex.RelaxedDoubleEquals.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value; return true; }

        m = KVRegex.RelaxedColon.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value; return true; }

        m = KVRegex.RelaxedHtmlComment.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value; return true; }

        m = KVRegex.RelaxedTrailingJunk.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value; return true; }

        m = KVRegex.RelaxedMissingEndQuote.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value.Trim(); return true; }

        m = KVRegex.RelaxedMissingStartQuote.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value.Trim(); return true; }

        m = KVRegex.RelaxedNoQuotes.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value.Trim(); return true; }

        m = KVRegex.RelaxedNoEquals.Match(line);
        if (m.Success) { key = m.Groups["key"].Value.Trim(); value = m.Groups["value"].Value; return true; }

        return false;
    }

    private static List<string> FilterLuaCommentBlocks(List<string> failedLines)
    {
        var parseRegex = new Regex(@"^\[(?<file>[^\]]+):(?<lineNum>\d+)\]\s+(?<content>.*)$", RegexOptions.Compiled);
        var commentOpenRegex = new Regex(@"^\s*;\s*=+\s*$", RegexOptions.Compiled);
        var groups = new List<(string fileName, List<int> indices, List<string> contents)>();

        for (int i = 0; i < failedLines.Count; i++)
        {
            var pm = parseRegex.Match(failedLines[i]);
            if (!pm.Success)
                continue;

            var fname = pm.Groups["file"].Value;
            var content = pm.Groups["content"].Value;
            var groupIndex = groups.FindIndex(g => g.fileName == fname);
            if (groupIndex < 0)
            {
                groups.Add((fname, [], []));
                groupIndex = groups.Count - 1;
            }

            groups[groupIndex].indices.Add(i);
            groups[groupIndex].contents.Add(content);
        }

        var removeSet = new HashSet<int>();
        foreach (var (_, indices, contents) in groups)
        {
            for (int j = 0; j < contents.Count; j++)
            {
                if (!commentOpenRegex.IsMatch(contents[j]))
                    continue;

                int closeIdx = -1;
                for (int k = j + 1; k < contents.Count; k++)
                {
                    if (commentOpenRegex.IsMatch(contents[k]))
                    {
                        closeIdx = k;
                        break;
                    }

                    if (!Regex.IsMatch(contents[k], @"^\s*;"))
                        break;
                }

                if (closeIdx > j)
                {
                    for (int d = j; d <= closeIdx; d++)
                        removeSet.Add(indices[d]);
                    j = closeIdx;
                }
            }
        }

        return failedLines.Where((_, i) => !removeSet.Contains(i)).ToList();
    }

    private static bool LooksLikeTranslationEntry(string line)
    {
        return KVRegex.PzEntry.IsMatch(line)
            || KVRegex.JsonEntry.IsMatch(line)
            || KVRegex.LuaConcatStart.IsMatch(line);
    }

    // Merge and output.

    private static void MergeRawEntry(
        Dictionary<string, TranslationEntry> entries,
        Dictionary<string, WinnerInfo> winners,
        RawTranslationEntry raw,
        SourceKind sourceKind,
        string baseIso)
    {
        var indexKey = $"{raw.ModId}::{raw.TranslationKey}";
        if (!entries.TryGetValue(indexKey, out var entry))
        {
            entry = new TranslationEntry
            {
                modId = raw.ModId,
                masterKey = raw.MasterKey,
                translationKey = raw.TranslationKey,
                baseLang = baseIso,
                embeddingVector = [],
                isActive = true,
                lastSeenAt = DateTime.UtcNow
            };
            entries[indexKey] = entry;
        }

        AddContainingFileInfo(entry, raw.FileInfo);

        var winnerKey = $"{indexKey}::{raw.IsoCode}";
        var candidate = new WinnerInfo(raw.ModId, raw.TranslationKey, raw.IsoCode, raw.MasterKey, raw.FileInfo, sourceKind);
        var shouldReplace = !winners.TryGetValue(winnerKey, out var current)
            || candidate.SourceKind > current.SourceKind
            || (candidate.SourceKind == current.SourceKind && CompareVersion(candidate.FileInfo, current.FileInfo) > 0);

        if (!shouldReplace)
            return;

        winners[winnerKey] = candidate;
        entry.translationValues[raw.IsoCode] = new TranslationData
        {
            text = PercentNormalizerService.Normalize(raw.Text),
            isVerified = false,
            status = "unverified",
            processStatus = string.Equals(raw.IsoCode, baseIso, StringComparison.OrdinalIgnoreCase) ? "processed" : "unprocessed",
            comments = []
        };

        if (string.IsNullOrWhiteSpace(entry.masterKey) || string.Equals(raw.IsoCode, baseIso, StringComparison.OrdinalIgnoreCase))
            entry.masterKey = raw.MasterKey;
    }

    private void WriteOldFormatOutputs(
        string modId,
        Dictionary<string, TranslationEntry> entries,
        Dictionary<string, WinnerInfo> winners,
        string baseIso)
    {
        var outBase = GetOutputBase();
        var entriesForMod = entries.Values
            .Where(entry => string.Equals(entry.modId, modId, StringComparison.Ordinal))
            .OrderBy(entry => entry.translationKey, StringComparer.Ordinal)
            .ToList();

        var isoCodes = entriesForMod
            .SelectMany(entry => entry.translationValues.Keys)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(iso => string.Equals(iso, baseIso, StringComparison.OrdinalIgnoreCase) ? "" : iso, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var iso in isoCodes)
        {
            var langOutDir = Path.Combine(outBase, iso);
            Directory.CreateDirectory(langOutDir);
            var outFile = Path.Combine(langOutDir, $"{modId}.txt");
            var sb = new StringBuilder();

            foreach (var entry in entriesForMod)
            {
                if (!entry.translationValues.TryGetValue(iso, out var data))
                    continue;

                if (string.Equals(iso, baseIso, StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine($"{entry.translationKey}::{baseIso} = \"{EscapeValue(data.text)}\"");
                    continue;
                }

                if (entry.translationValues.TryGetValue(baseIso, out var baseData))
                    sb.AppendLine($"{entry.translationKey}::{baseIso} = \"{EscapeValue(baseData.text)}\"");

                var state = data.isVerified ? "verified" : "unverified";
                sb.AppendLine($"{entry.translationKey}::{iso}::{state} = \"{EscapeValue(data.text)}\"");
            }

            Utf8NoBom.WriteAllText(outFile, sb.ToString());
        }

        var mapDir = Path.Combine(outBase, "translation_key_to_file_mapping");
        Directory.CreateDirectory(mapDir);
        var keyFileMap = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var entry in entriesForMod)
        {
            var winner = GetBestWinner(winners, entry, baseIso);
            keyFileMap[entry.translationKey] = winner?.FileStem ?? "";
        }

        var mapFile = Path.Combine(mapDir, $"{modId}.json");
        Utf8NoBom.WriteAllText(mapFile, Utf8NoBom.SerializeIndentedJson(keyFileMap));
    }

    private static WinnerInfo? GetBestWinner(Dictionary<string, WinnerInfo> winners, TranslationEntry entry, string baseIso)
    {
        var baseWinnerKey = $"{entry.modId}::{entry.translationKey}::{baseIso}";
        if (winners.TryGetValue(baseWinnerKey, out var baseWinner))
            return baseWinner;

        return entry.translationValues.Keys
            .Select(iso => winners.GetValueOrDefault($"{entry.modId}::{entry.translationKey}::{iso}"))
            .FirstOrDefault(winner => winner != null);
    }

    private static void AddContainingFileInfo(TranslationEntry entry, ContainingFileInfo fileInfo)
    {
        if (entry.containingFileInfos.Any(info =>
            string.Equals(info.filePath, fileInfo.filePath, StringComparison.OrdinalIgnoreCase)
            && info.gameMajorVersion == fileInfo.gameMajorVersion
            && info.gameMinorVersion == fileInfo.gameMinorVersion))
        {
            return;
        }

        entry.containingFileInfos.Add(fileInfo);
    }

    private static int CompareVersion(ContainingFileInfo left, ContainingFileInfo right)
    {
        var major = left.gameMajorVersion.CompareTo(right.gameMajorVersion);
        return major != 0 ? major : left.gameMinorVersion.CompareTo(right.gameMinorVersion);
    }

    // Path, language, diagnostics helpers.

    private Dictionary<string, string> BuildLanguageMap()
    {
        var map = new Dictionary<string, string>(DefaultIsoByGameCode, StringComparer.OrdinalIgnoreCase);
        foreach (var lang in _config.supportedLanguages)
        {
            if (!string.IsNullOrWhiteSpace(lang.ingameCode) && !string.IsNullOrWhiteSpace(lang.isoCode))
                map[lang.ingameCode.ToUpperInvariant()] = lang.isoCode.ToLowerInvariant();
        }

        return map;
    }

    private static string NormalizeToIso(string language, Dictionary<string, string> langMap)
    {
        if (string.IsNullOrWhiteSpace(language))
            return "en";

        if (langMap.TryGetValue(language, out var iso))
            return iso.ToLowerInvariant();

        return language.ToLowerInvariant();
    }

    private string GetOutputBase()
    {
        if (!string.IsNullOrWhiteSpace(_config.extractedContentsTempDir))
            return _config.extractedContentsTempDir;

        if (!string.IsNullOrWhiteSpace(_config.runTempDir))
            return Path.Combine(_config.runTempDir, "extracted_contents");

        return Path.Combine(Path.GetTempPath(), "project_babel_extracted_contents");
    }

    private StreamWriter? CreateFuckWriter()
    {
        try
        {
            var txtDir = GetTxtDiagnosticDir();
            Directory.CreateDirectory(txtDir);
            return Utf8NoBom.CreateStreamWriter(Path.Combine(txtDir, "fuck.txt"), append: true);
        }
        catch (Exception ex)
        {
            GitHubActions.Warning($"Failed to open fuck.txt: {ex.Message}", "ContentExtractor");
            return null;
        }
    }

    private void WriteDiagnostics(List<string> diagnostics)
    {
        if (diagnostics.Count == 0)
            return;

        try
        {
            var txtDir = GetTxtDiagnosticDir();
            Directory.CreateDirectory(txtDir);
            Utf8NoBom.AppendAllLines(Path.Combine(txtDir, "failed_lines.txt"), diagnostics);
        }
        catch (Exception ex)
        {
            GitHubActions.Warning($"Failed to write TXT diagnostics: {ex.Message}", "ContentExtractor");
        }
    }

    private string GetTxtDiagnosticDir()
    {
        return !string.IsNullOrWhiteSpace(_config.runTempDir)
            ? Path.Combine(_config.runTempDir, "txt")
            : Path.Combine(GetOutputBase(), "txt");
    }

    private static ContainingFileInfo BuildContainingFileInfo(string file, string modLocalPath)
    {
        var relPath = Path.GetRelativePath(modLocalPath, file).Replace('\\', '/');
        var parts = relPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var info = new ContainingFileInfo
        {
            fileName = Path.GetFileName(file),
            filePath = relPath
        };

        var modsIndex = Array.FindIndex(parts, part => string.Equals(part, "mods", StringComparison.OrdinalIgnoreCase));
        if (modsIndex < 0 || modsIndex + 1 >= parts.Length)
            return info;

        info.subModName = parts[modsIndex + 1];
        var versionIndex = modsIndex + 2;
        if (versionIndex < parts.Length && !string.Equals(parts[versionIndex], "media", StringComparison.OrdinalIgnoreCase))
            ApplyVersion(parts[versionIndex], info);

        return info;
    }

    private static void ApplyVersion(string segment, ContainingFileInfo info)
    {
        if (string.Equals(segment, "common", StringComparison.OrdinalIgnoreCase))
        {
            info.gameMajorVersion = 1;
            info.gameMinorVersion = 0;
            return;
        }

        var pieces = segment.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (pieces.Length > 0 && int.TryParse(pieces[0], out var major))
            info.gameMajorVersion = major;
        if (pieces.Length > 1 && int.TryParse(pieces[1], out var minor))
            info.gameMinorVersion = minor;
    }

    private static string BuildMasterKeyFromFile(string fileNameOrPath, string gameCode)
    {
        var stem = Path.GetFileNameWithoutExtension(fileNameOrPath);
        return $"{StripAnyLangSuffix(stem)}_{gameCode.ToUpperInvariant()}";
    }

    private static string NormalizeMasterKey(string fullKey, string gameCode)
    {
        return $"{StripAnyLangSuffix(fullKey)}_{gameCode.ToUpperInvariant()}";
    }

    private static string StripAnyLangSuffix(string value)
    {
        foreach (var code in GetAllLanguageSuffixes())
        {
            var tag1 = $"<{code}>";
            if (value.EndsWith(tag1, StringComparison.OrdinalIgnoreCase))
                return value[..^tag1.Length];

            var tag2 = $"_{tag1}";
            if (value.EndsWith(tag2, StringComparison.OrdinalIgnoreCase))
                return value[..^tag2.Length];

            var tag3 = $"_{code}";
            if (value.EndsWith(tag3, StringComparison.OrdinalIgnoreCase) && value.Length > tag3.Length)
                return value[..^tag3.Length];
        }

        return value;
    }

    private static IEnumerable<string> GetAllLanguageSuffixes()
    {
        foreach (var code in DefaultGameCodes)
            yield return code;
        foreach (var iso in DefaultIsoByGameCode.Values)
            yield return iso;
    }

    private static string EscapeValue(string s)
    {
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"")
            .Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
    }

    private void WriteRawModWarning(ModInfo mod, string file, string? batchId, Exception ex)
    {
        WarningFileWriter.Write(
            _config,
            "ContentExtractor",
            batchId,
            new PipelineWarning
            {
                ModuleName = "ContentExtractor",
                BatchId = batchId,
                ModId = mod.modId,
                ModName = mod.modName,
                FilePath = file,
                LineNumber = ex is JsonException json ? json.LineNumber : null,
                BytePositionInLine = ex is JsonException jsonEx ? jsonEx.BytePositionInLine : null,
                ErrorType = ex.GetType().Name,
                Message = ex.Message
            });
    }

    private enum SourceKind
    {
        Text = 0,
        Json = 1
    }

    private sealed record RawTranslationEntry(
        string ModId,
        string MasterKey,
        string TranslationKey,
        string IsoCode,
        string Text,
        ContainingFileInfo FileInfo);

    private sealed record WinnerInfo(
        string ModId,
        string TranslationKey,
        string IsoCode,
        string MasterKey,
        ContainingFileInfo FileInfo,
        SourceKind SourceKind)
    {
        public string FileStem => StripAnyLangSuffix(Path.GetFileNameWithoutExtension(FileInfo.fileName));
    }

    private static class KVRegex
    {
        public static readonly Regex PzEntry = new(
            @"^\s*(?<key>[^=]+?)\s*=\s*[""\u201c](?<value>.*)[""\u201d]\s*(?:[,;]?\s*(?:/\*.*?\*/)?\s*)?$",
            RegexOptions.Compiled);
        public static readonly Regex JsonEntry = new(
            @"^\s*""(?<key>[^""]*)""\s*:\s*[""\u201c](?<value>.*)[""\u201d]\s*,?\s*$",
            RegexOptions.Compiled);
        public static readonly Regex LuaConcatStart = new(
            @"^\s*(?<key>[^=]+?)\s*=\s*[""\u201c](?<value>.*)[""\u201d]\s*\.\.\s*$",
            RegexOptions.Compiled);
        public static readonly Regex LuaConcatContinue = new(
            @"^\s*[""\u201c](?<value>.+)[""\u201d]\s*\.\.\s*$",
            RegexOptions.Compiled);
        public static readonly Regex LuaConcatEnd = new(
            @"^\s*[""\u201c](?<value>.+)[""\u201d]\s*(?:\.\.\s*,|,)?\s*$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedDoubleEquals = new(
            @"^\s*(?<key>.+?)\s*={2,}\s*[""\u201c](?<value>[^""\u201d]*)[""\u201d]\s*(?:,?\s*.*)?$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedColon = new(
            @"^\s*(?<key>.+?)\s*:\s*[""\u201c](?<value>[^""\u201d]*)[""\u201d]\s*(?:,?\s*.*)?$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedHtmlComment = new(
            @"^\s*<!--\s*(?<key>.+?)\s*=\s*[""\u201c](?<value>[^""\u201d]*)[""\u201d]\s*-->\s*$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedTrailingJunk = new(
            @"^\s*(?<key>.+?)\s*=\s*[""\u201c](?<value>[^""\u201d]*)[""\u201d]\s*(?:,?\s*.*)?$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedMissingEndQuote = new(
            @"^\s*(?<key>.+?)\s*=\s*[""\u201c](?<value>.+)\s*,?\s*$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedMissingStartQuote = new(
            @"^\s*(?<key>.+?)\s*=\s*(?<value>[^""\u201d]+)[""\u201d]\s*,?\s*$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedNoQuotes = new(
            @"^\s*(?<key>[A-Za-z_]\w*(?:\.\w*)*)\s*=\s*(?<value>.+?)\s*[;,]?\s*$",
            RegexOptions.Compiled);
        public static readonly Regex RelaxedNoEquals = new(
            @"^\s*(?<key>[A-Za-z0-9_.]+)\s+[""\u201c](?<value>[^""\u201d]*)[""\u201d]\s*,?\s*$",
            RegexOptions.Compiled);
    }
}
