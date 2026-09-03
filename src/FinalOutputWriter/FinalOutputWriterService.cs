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
    // File names are case-insensitive on the Windows target used by the game,
    // but the CI pipeline also runs on Linux.  Keep one deterministic canonical
    // spelling for every known logical output file so both platforms emit the
    // same path when a mod uses e.g. ItemName and Itemname.
    private Dictionary<string, string> _canonicalFileStemByName = new(StringComparer.OrdinalIgnoreCase);
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
        EnsureAnimationScanDirectories();

        var refModIdSet = refModInfoDict.Keys.ToHashSet(StringComparer.Ordinal);
        var routeWarnings = new RouteWarningSummary();
        var conflictWarnings = new OutputConflictSummary();
        int totalFiles = 0;
        int totalEntries = 0;

        foreach (var lang in outputLanguages)
        {
            var gameCode = lang.ingameCode;
            if (string.IsNullOrWhiteSpace(gameCode))
                gameCode = lang.isoCode.ToUpperInvariant();

            // Collect entries: non-empty target text, not ref mod, not base game key.
            // A translation key can occur in more than one mod.  The group-level
            // conflict resolver below selects by stable entry identity, so the
            // caller's ordering remains available for bounded diagnostics while
            // the emitted value stays independent of Dictionary insertion order.
            var fileGroups = new Dictionary<string, OutputFileGroup>(StringComparer.OrdinalIgnoreCase);
            var outputValuesByKey = new Dictionary<string, OutputValue>(StringComparer.Ordinal);
            foreach (var entry in translationEntryDict.Values)
            {
                if (refModIdSet.Contains(entry.modId))
                    continue;
                if (_baseGameKeys.Contains(entry.translationKey))
                    continue;

                var targetText = GetTargetText(entry, lang.isoCode);
                if (string.IsNullOrWhiteSpace(targetText))
                    continue;

                var fileStem = ResolveFileStem(entry, routeWarnings);
                if (fileStem == null)
                    continue;

                var candidate = new OutputValue(entry, targetText, fileStem);
                if (outputValuesByKey.TryGetValue(entry.translationKey, out var existingValue)
                    && !string.Equals(existingValue.Text, candidate.Text, StringComparison.Ordinal))
                {
                    conflictWarnings.Add(lang.isoCode, entry.translationKey, existingValue, candidate);
                }
                else
                {
                    outputValuesByKey[entry.translationKey] = candidate;
                }

                if (!fileGroups.TryGetValue(fileStem, out var group))
                {
                    group = new OutputFileGroup(fileStem);
                    fileGroups[fileStem] = group;
                }

                group.Add(candidate, conflictWarnings, lang.isoCode);
            }

            // Write to 42.20 first.
            var outDir4220 = Path.Combine(_finalOutputBase, "42.20", "media", "lua", "shared", "Translate", gameCode);
            Directory.CreateDirectory(outDir4220);

            foreach (var group in fileGroups.Values.OrderBy(item => item.FileName, StringComparer.Ordinal))
            {
                var jsonPath = Path.Combine(outDir4220, group.FileName + ".json");
                var tmpPath = jsonPath + ".tmp";
                var output = group.ToOutputDictionary();
                RemoveForbiddenModNameField(group.FileName, output);
                var json = Utf8NoBom.SerializeIndentedJson(output);
                Utf8NoBom.WriteAllText(tmpPath, json);
                RemoveCaseVariantFiles(outDir4220, group.FileName + ".json");
                MoveFileAtomic(tmpPath, jsonPath);
            }

            // Copy to 42 (identical content).
            var outDir42 = Path.Combine(_finalOutputBase, "42", "media", "lua", "shared", "Translate", gameCode);
            CopyDirectory(outDir4220, outDir42);
            foreach (var group in fileGroups.Values)
                RemoveCaseVariantFiles(outDir42, group.FileName + ".json");

            totalFiles += fileGroups.Count;
            totalEntries += fileGroups.Sum(g => g.Value.Count);

            Console.WriteLine($"  Final output [{gameCode}]: {fileGroups.Count} files, {fileGroups.Sum(g => g.Value.Count)} entries");
        }

        if (routeWarnings.Count > 0)
            GitHubActions.Warning(routeWarnings.FormatMessage(), "FinalOutputWriter");
        if (conflictWarnings.Count > 0)
            GitHubActions.Warning(conflictWarnings.FormatMessage(), "FinalOutputWriter");

        Console.WriteLine($"  Final output done: {totalFiles} files, {totalEntries} entries across {outputLanguages.Count} languages");
        return Task.FromResult(new TaskResult { isSuccess = true });
    }

    private void EnsureAnimationScanDirectories()
    {
        foreach (var versionDir in new[] { "common", "42", "42.20" })
        {
            var mediaDir = Path.Combine(_finalOutputBase, versionDir, "media");
            Directory.CreateDirectory(Path.Combine(mediaDir, "AnimSets"));
            Directory.CreateDirectory(Path.Combine(mediaDir, "actiongroups"));
        }
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

        var jsonFiles = Directory.GetFiles(_baseGameKeysDir, "*.json")
            .OrderBy(path => Path.GetFileName(path), StringComparer.Ordinal)
            .ToArray();
        // Collect prefix→file votes.
        var prefixVotes = new Dictionary<string, Dictionary<string, int>>(StringComparer.Ordinal);

        foreach (var filePath in jsonFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var validFileStem = ValidateOutputFileStem(fileName);
            if (validFileStem != null)
            {
                if (!_canonicalFileStemByName.TryGetValue(validFileStem, out var current)
                    || string.CompareOrdinal(validFileStem, current) < 0)
                {
                    _canonicalFileStemByName[validFileStem] = validFileStem;
                }
            }
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
            var validBestFile = ValidateOutputFileStem(bestFile);
            if (validBestFile != null)
                _prefixToFile[prefix] = CanonicalizeFileStem(validBestFile);
        }

        Console.WriteLine($"  Base game keys: {_baseGameKeys.Count} keys, {_prefixToFile.Count} prefixes from {jsonFiles.Length} files");
    }

    /// <summary>
    /// Resolve which JSON file a translation key belongs to. An explicit route
    /// captured from the winning mod file takes precedence over the base-game
    /// prefix mapping; invalid routes fall back safely to that mapping.
    /// </summary>
    private string? ResolveFileStem(TranslationEntry entry, RouteWarningSummary routeWarnings)
    {
        var explicitStem = ValidateOutputFileStem(entry.outputFileStem);
        if (explicitStem != null)
            return CanonicalizeFileStem(explicitStem);

        var prefix = ExtractPrefix(entry.translationKey);
        if (prefix != null && _prefixToFile.TryGetValue(prefix, out var mappedStem))
        {
            var validatedMappedStem = ValidateOutputFileStem(mappedStem);
            if (validatedMappedStem != null)
                return CanonicalizeFileStem(validatedMappedStem);
        }

        routeWarnings.Add(entry);
        return null;
    }

    private string CanonicalizeFileStem(string stem)
    {
        if (_canonicalFileStemByName.TryGetValue(stem, out var knownCanonical))
            return knownCanonical;

        return stem;
    }

    /// <summary>
    /// One logical output file. The parent dictionary uses OrdinalIgnoreCase,
    /// while this object keeps the deterministic spelling and performs all key
    /// aggregation before a file is written.
    /// </summary>
    private sealed class OutputFileGroup
    {
        private readonly Dictionary<string, OutputValue> _values = new(StringComparer.Ordinal);

        public OutputFileGroup(string fileStem)
        {
            FileName = fileStem;
        }

        public string FileName { get; private set; }
        public int Count => _values.Count;

        public void Add(OutputValue candidate, OutputConflictSummary conflicts, string targetLang)
        {
            // For stems that are not present in base_game_keys, select the
            // ordinally smallest spelling. This is stable across input order,
            // Windows, and Linux. Known game file names are canonicalized before
            // they reach this object and therefore take precedence over this rule.
            if (string.CompareOrdinal(candidate.FileStem, FileName) < 0)
                FileName = candidate.FileStem;

            if (!_values.TryGetValue(candidate.Entry.translationKey, out var existing))
            {
                _values[candidate.Entry.translationKey] = candidate;
                return;
            }

            if (string.Equals(existing.Text, candidate.Text, StringComparison.Ordinal))
                return;

            var winner = CompareOutputValues(existing, candidate) <= 0 ? existing : candidate;
            conflicts.Add(targetLang, candidate.Entry.translationKey, existing, candidate, winner);
            _values[candidate.Entry.translationKey] = winner;
        }

        public Dictionary<string, string> ToOutputDictionary()
        {
            var output = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var (key, value) in _values.OrderBy(pair => pair.Key, StringComparer.Ordinal))
                output[key] = value.Text;
            return output;
        }

        private static int CompareOutputValues(OutputValue left, OutputValue right)
        {
            var identityComparison = string.CompareOrdinal(left.Identity, right.Identity);
            return identityComparison != 0
                ? identityComparison
                : string.CompareOrdinal(left.Text, right.Text);
        }
    }

    private sealed record OutputValue(TranslationEntry Entry, string Text, string FileStem)
    {
        public string Identity => $"{Entry.modId}::{Entry.translationKey}";
    }

    private sealed class OutputConflictSummary
    {
        private const int MaxExamples = 8;
        private readonly HashSet<string> _identities = new(StringComparer.Ordinal);
        private readonly List<string> _examples = [];

        public int Count => _identities.Count;

        public void Add(
            string targetLang,
            string translationKey,
            OutputValue left,
            OutputValue right,
            OutputValue? winner = null)
        {
            var leftFirst = string.CompareOrdinal(left.Identity, right.Identity) <= 0;
            var first = leftFirst ? left : right;
            var second = leftFirst ? right : left;
            var identity = string.Join(
                '\u001f',
                targetLang,
                translationKey,
                first.Identity,
                first.Text,
                second.Identity,
                second.Text);
            if (!_identities.Add(identity) || _examples.Count >= MaxExamples)
                return;

            var selected = winner ?? first;
            _examples.Add(
                $"{targetLang}:{translationKey} ({first.Identity}={Sanitize(first.Text)}; "
                + $"{second.Identity}={Sanitize(second.Text)}; selected={selected.Identity})");
        }

        public string FormatMessage()
        {
            var remaining = Count - _examples.Count;
            var more = remaining > 0 ? $"; and {remaining} more" : "";
            return $"Output translation value conflicts detected for {Count} key/language pairs; "
                + "same-file conflicts retain the ordinally first entry; cross-file values remain visible for review. "
                + $"Examples: {string.Join(", ", _examples)}{more}.";
        }

        private static string Sanitize(string value)
        {
            const int maxLength = 80;
            var sanitized = value
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace('\t', ' ');
            return sanitized.Length <= maxLength
                ? sanitized
                : sanitized[..(maxLength - 3)] + "...";
        }
    }

    /// <summary>
    /// Validates a file stem before it is combined with an output directory.
    /// Rejects rooted paths, separators, traversal segments, invalid filename
    /// characters, and an explicit .json extension (the writer appends .json).
    /// </summary>
    internal static string? ValidateOutputFileStem(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var stem = value.Trim();
        if (stem.Length > 128
            || stem is "." or ".."
            || stem.Contains("..", StringComparison.Ordinal)
            || stem.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
            || IsReservedDeviceName(stem))
            return null;

        // Keep this allowlist deliberately ASCII and independent of the host OS.
        // It covers current PZ translation file names (including map names such as
        // "Brandenburg, KY") while rejecting separators, control characters,
        // device names, and platform-specific filename punctuation.
        foreach (var character in stem)
        {
            var isAsciiLetter = character is >= 'A' and <= 'Z' or >= 'a' and <= 'z';
            var isAsciiDigit = character is >= '0' and <= '9';
            if (!isAsciiLetter
                && !isAsciiDigit
                && character is not (' ' or '_' or '-' or '.' or ',' or '(' or ')'))
            {
                return null;
            }
        }

        return stem;
    }

    private static bool IsReservedDeviceName(string stem)
    {
        var firstDot = stem.IndexOf('.');
        var deviceName = firstDot >= 0 ? stem[..firstDot] : stem;
        return deviceName.Equals("CON", StringComparison.OrdinalIgnoreCase)
            || deviceName.Equals("PRN", StringComparison.OrdinalIgnoreCase)
            || deviceName.Equals("AUX", StringComparison.OrdinalIgnoreCase)
            || deviceName.Equals("NUL", StringComparison.OrdinalIgnoreCase)
            || (deviceName.Length == 4
                && (deviceName.StartsWith("COM", StringComparison.OrdinalIgnoreCase)
                    || deviceName.StartsWith("LPT", StringComparison.OrdinalIgnoreCase))
                && deviceName[3] is >= '1' and <= '9');
    }

    private sealed class RouteWarningSummary
    {
        private const int MaxExamples = 5;
        private readonly HashSet<string> _identities = new(StringComparer.Ordinal);
        private readonly List<string> _examples = [];

        public int Count => _identities.Count;

        public void Add(TranslationEntry entry)
        {
            var identity = $"{entry.modId}::{entry.translationKey}";
            if (!_identities.Add(identity) || _examples.Count >= MaxExamples)
                return;

            _examples.Add(SanitizeExample(identity));
        }

        public string FormatMessage()
        {
            var remaining = Count - _examples.Count;
            var more = remaining > 0 ? $"; and {remaining} more" : "";
            return $"No valid output file route for {Count} translated entries; "
                + $"examples: {string.Join(", ", _examples)}{more}.";
        }

        private static string SanitizeExample(string identity)
        {
            const int maxLength = 120;
            var sanitized = identity
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace('\t', ' ');
            return sanitized.Length <= maxLength
                ? sanitized
                : sanitized[..(maxLength - 3)] + "...";
        }
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

    private static void RemoveForbiddenModNameField(string fileStem, Dictionary<string, string> output)
    {
        if (!string.Equals(fileStem, "mod", StringComparison.OrdinalIgnoreCase))
            return;

        foreach (var key in output.Keys
                     .Where(key => string.Equals(key, "name", StringComparison.OrdinalIgnoreCase))
                     .ToArray())
        {
            output.Remove(key);
        }
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

    /// <summary>
    /// Remove stale case variants of a managed output file. Linux permits both
    /// ItemName.json and Itemname.json while Windows maps them to one path; the
    /// cleanup keeps a rerun from leaving platform-specific duplicate files.
    /// </summary>
    private static void RemoveCaseVariantFiles(string directory, string canonicalFileName)
    {
        if (!Directory.Exists(directory))
            return;

        foreach (var path in Directory.GetFiles(directory)
                     .Where(path => string.Equals(Path.GetExtension(path), ".json", StringComparison.OrdinalIgnoreCase)))
        {
            var fileName = Path.GetFileName(path);
            if (!string.Equals(fileName, canonicalFileName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(fileName, canonicalFileName, StringComparison.Ordinal))
            {
                continue;
            }

            try
            {
                File.Delete(path);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                GitHubActions.Warning(
                    $"Failed to remove stale case-variant output {fileName}: {ex.Message}",
                    "FinalOutputWriter");
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
