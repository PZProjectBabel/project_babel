using System.Text.Json;
using Common;

namespace FinalOutputWriter;

/// <summary>
/// Writes final mod translation JSON files for PZ mod distribution.
/// Outputs to final_outputs/project_babel/.../Translate/<gamecode>/*.json
/// </summary>
public class FinalOutputWriterService
{
    private readonly PipelineConfig _config;
    private readonly string _baseGameKeysDir;
    private readonly string _finalOutputBase;

    // Built once from base_game_keys.
    private HashSet<string> _baseGameKeys = new(StringComparer.Ordinal);
    private Dictionary<string, string> _prefixToFile = new(StringComparer.Ordinal); // prefix→filename (no .json)
    private bool _baseGameKeysLoaded;

    public FinalOutputWriterService(PipelineConfig config)
    {
        _config = config;
        _baseGameKeysDir = Path.Combine(config.baseDir, "base_game_keys");
        _finalOutputBase = Path.Combine(config.baseDir, "final_outputs", "project_babel",
            "contents", "mods", "project_babel");
    }

    /// <summary>
    /// Write final mod translation files for all target languages.
    /// </summary>
    public Task<TaskResult> WriteFinalOutputAsync(
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, ModInfo> refModInfoDict,
        List<LangInfoData> outputLanguages)
    {
        LoadBaseGameKeys();

        var refModIdSet = refModInfoDict.Keys.ToHashSet(StringComparer.Ordinal);
        int totalFiles = 0;
        int totalEntries = 0;

        foreach (var lang in outputLanguages)
        {
            var gameCode = lang.ingameCode;
            if (string.IsNullOrWhiteSpace(gameCode))
                gameCode = lang.isoCode.ToUpperInvariant();

            // Collect entries: non-empty target text, not ref mod, not base game key.
            var fileGroups = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
            foreach (var entry in translationEntryDict.Values)
            {
                if (refModIdSet.Contains(entry.modId))
                    continue;
                if (_baseGameKeys.Contains(entry.translationKey))
                    continue;

                var targetText = GetTargetText(entry, lang.isoCode);
                if (string.IsNullOrWhiteSpace(targetText))
                    continue;

                var fileName = ResolveFileName(entry.translationKey);
                if (fileName == null)
                    continue;

                if (!fileGroups.TryGetValue(fileName, out var dict))
                {
                    dict = new Dictionary<string, string>(StringComparer.Ordinal);
                    fileGroups[fileName] = dict;
                }
                dict[entry.translationKey] = targetText;
            }

            // Write to 42.20 first.
            var outDir4220 = Path.Combine(_finalOutputBase, "42.20", "media", "lua", "shared", "Translate", gameCode);
            Directory.CreateDirectory(outDir4220);

            foreach (var (fileName, dict) in fileGroups)
            {
                var jsonPath = Path.Combine(outDir4220, fileName);
                var tmpPath = jsonPath + ".tmp";
                var json = Utf8NoBom.SerializeIndentedJson(dict);
                Utf8NoBom.WriteAllText(tmpPath, json);
                MoveFileAtomic(tmpPath, jsonPath);
            }

            // Copy to 42 (identical content).
            var outDir42 = Path.Combine(_finalOutputBase, "42", "media", "lua", "shared", "Translate", gameCode);
            CopyDirectory(outDir4220, outDir42);

            totalFiles += fileGroups.Count;
            totalEntries += fileGroups.Sum(g => g.Value.Count);

            Console.WriteLine($"  Final output [{gameCode}]: {fileGroups.Count} files, {fileGroups.Sum(g => g.Value.Count)} entries");
        }

        Console.WriteLine($"  Final output done: {totalFiles} files, {totalEntries} entries across {outputLanguages.Count} languages");
        return Task.FromResult(new TaskResult { isSuccess = true });
    }

    private void LoadBaseGameKeys()
    {
        if (_baseGameKeysLoaded)
            return;
        _baseGameKeysLoaded = true;

        if (!Directory.Exists(_baseGameKeysDir))
        {
            GitHubActions.Warning($"base_game_keys/ not found at {_baseGameKeysDir}, no keys excluded.", "Setup warning");
            return;
        }

        var jsonFiles = Directory.GetFiles(_baseGameKeysDir, "*.json");
        // Collect prefix→file votes.
        var prefixVotes = new Dictionary<string, Dictionary<string, int>>(StringComparer.Ordinal);

        foreach (var filePath in jsonFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            try
            {
                using var doc = JsonDocument.Parse(Utf8NoBom.ReadAllText(filePath));
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    _baseGameKeys.Add(prop.Name);

                    // Extract prefix (text before first '_')
                    var prefix = ExtractPrefix(prop.Name);
                    if (prefix == null)
                        continue;

                    if (!prefixVotes.TryGetValue(prefix, out var fileVotes))
                    {
                        fileVotes = new Dictionary<string, int>(StringComparer.Ordinal);
                        prefixVotes[prefix] = fileVotes;
                    }
                    fileVotes.TryGetValue(fileName, out var count);
                    fileVotes[fileName] = count + 1;
                }
            }
            catch (Exception ex)
            {
                GitHubActions.Warning($"Failed to read base_game_keys/{Path.GetFileName(filePath)}: {ex.Message}", "Parse warning");
            }
        }

        // Resolve prefix→file: pick file with most keys for each prefix.
        foreach (var (prefix, fileVotes) in prefixVotes)
        {
            var bestFile = fileVotes.MaxBy(kvp => kvp.Value).Key;
            _prefixToFile[prefix] = bestFile;
        }

        Console.WriteLine($"  Base game keys: {_baseGameKeys.Count} keys, {_prefixToFile.Count} prefixes from {jsonFiles.Length} files");
    }

    /// <summary>
    /// Resolve which JSON file a translation key belongs to.
    /// Uses prefix→file mapping from base_game_keys.
    /// </summary>
    private string? ResolveFileName(string key)
    {
        var prefix = ExtractPrefix(key);
        if (prefix != null && _prefixToFile.TryGetValue(prefix, out var fileName))
            return fileName + ".json";
        return null;
    }

    /// <summary>
    /// Extract prefix: text before first '_'. Returns null if no underscore.
    /// </summary>
    private static string? ExtractPrefix(string key)
    {
        var idx = key.IndexOf('_');
        return idx > 0 ? key[..idx] : null;
    }

    /// <summary>
    /// Get translated text for target language from entry, using iso code lookup.
    /// Returns empty string if no translation.
    /// </summary>
    private static string GetTargetText(TranslationEntry entry, string targetIso)
    {
        var normIso = targetIso.ToLowerInvariant();
        if (entry.translationValues.TryGetValue(normIso, out var td)
            && !string.IsNullOrWhiteSpace(td.text))
            return td.text;

        // Try case-insensitive fallback.
        foreach (var (lang, td2) in entry.translationValues)
        {
            if (string.Equals(lang, normIso, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(td2.text))
                return td2.text;
        }

        return "";
    }

    private static void MoveFileAtomic(string source, string dest)
    {
        for (int i = 0; ; i++)
        {
            try { File.Move(source, dest, overwrite: true); return; }
            catch (Exception ex) when ((ex is IOException || ex is UnauthorizedAccessException) && i < 3)
            {
                Thread.Sleep(100 * (i + 1));
            }
        }
    }

    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, overwrite: true);
        }
    }
}
