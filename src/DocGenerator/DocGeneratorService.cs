using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Common;

namespace DocGenerator;

/// <summary>
/// Documentation generator — translates Chinese template docs into all supported languages via LLM.
/// Target languages are loaded from config/supported_languages_example.json (excluding zh-hans source).
/// </summary>
public sealed partial class DocGeneratorService
{
    private const int MaxLinesPerBatch = 30;
    private const int MaxConcurrency = 128;
    private const int MaxRetries = 3;
    private static readonly TimeSpan RetryBaseDelay = TimeSpan.FromSeconds(2);

    /// <summary>Regex to parse a TOC link: [text](#anchor)</summary>
    [GeneratedRegex(@"\[([^\]]*?)\]\(#([^)]*?)\)", RegexOptions.Compiled)]
    private static partial Regex TocLinkRegex();

    /// <summary>Regex to detect a markdown heading line.</summary>
    [GeneratedRegex(@"^(#{2,4})\s+(.+)$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex HeadingRegex();

    /// <summary>Regex to detect a table row (starts with optional whitespace then |).</summary>
    [GeneratedRegex(@"^\s*\|", RegexOptions.Compiled)]
    private static partial Regex TableRowRegex();

    private readonly string _repoRoot;
    private readonly string _templatesDir;
    private readonly string _outputDir;
    private readonly string _debugDir;
    private readonly HttpClient _httpClient;

    /// <summary>Target languages loaded from config/supported_languages.json (excluding zh-hans source).</summary>
    private readonly string[] _targetLanguages;

    public DocGeneratorService(string repoRoot, HttpClient? httpClient = null)
    {
        _repoRoot = repoRoot;
        _templatesDir = Path.Combine(repoRoot, "docs", "templates");
        _outputDir = Path.Combine(repoRoot, "temp", "docgen");
        _debugDir = Path.Combine(_outputDir, "debug");
        _httpClient = httpClient ?? CreateDefaultClient();
        _targetLanguages = LoadTargetLanguages(repoRoot);
        Console.WriteLine($"[DocGen] Target languages: {string.Join(", ", _targetLanguages)}");
    }

    /// <summary>
    /// Load target language ISO codes from config/supported_languages_example.json, excluding zh-hans (source).
    /// </summary>
    private static string[] LoadTargetLanguages(string repoRoot)
    {
        var path = Path.Combine(repoRoot, "config", "supported_languages_example.json");
        if (!File.Exists(path))
            throw new FileNotFoundException($"supported_languages.json not found: {path}");

        var json = Utf8NoBom.ReadAllText(path);
        var opts = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true
        };
        var langs = JsonSerializer.Deserialize<List<LangInfoData>>(json, opts)
                     ?? throw new InvalidOperationException("Failed to parse supported_languages.json");

        return langs
            .Select(l => l.isoCode)
            .Where(code => code != "zh-hans")
            .ToArray();
    }

    /// <summary>
    /// Main entry: process all template files.
    /// </summary>
    public async Task RunAsync(CancellationToken ct = default)
    {
        Directory.CreateDirectory(_outputDir);
        Directory.CreateDirectory(_debugDir);

        // Load prompt header and tail templates.
        var promptHeader = await Utf8NoBom.ReadAllTextAsync(
            Path.Combine(_templatesDir, "prompt_header.md"), ct);
        var promptTail = await Utf8NoBom.ReadAllTextAsync(
            Path.Combine(_templatesDir, "prompt_tail.md"), ct);

        // Find all template markdown files.
        var templateFiles = Directory.GetFiles(_templatesDir, "*_template.md")
            .OrderBy(f => f)
            .ToList();

        Console.WriteLine($"[DocGen] Found {templateFiles.Count} template file(s):");
        foreach (var f in templateFiles)
            Console.WriteLine($"  - {Path.GetFileName(f)}");

        // Process each template file serially.
        foreach (var templatePath in templateFiles)
        {
            var templateName = Path.GetFileNameWithoutExtension(templatePath)
                .Replace("_template", "");
            Console.WriteLine($"\n[DocGen] === Processing: {templateName} ===");

            await ProcessTemplateAsync(templatePath, templateName, promptHeader, promptTail, ct);
        }

        Console.WriteLine("\n[DocGen] All templates processed.");

        // ── Step 6: Copy outputs to final locations ──
        CopyOutputsToDocs();
    }

    /// <summary>
    /// Copy generated docs from temp/docgen/ to final locations.
    /// readme_zh-hans.md → README.md (repo root);
    /// readme_{lang}.md → docs/readme/README_{lang}.md;
    /// {name}_{lang}.md → docs/{name}/{name}_{lang}.md.
    /// </summary>
    private void CopyOutputsToDocs()
    {
        if (!Directory.Exists(_outputDir))
        {
            Console.WriteLine("  [Step 6] Output dir not found, skip copy.");
            return;
        }

        var files = Directory.GetFiles(_outputDir, "*_*.md");
        Console.WriteLine($"  [Step 6] Copying {files.Length} output(s) to docs/ ...");

        foreach (var src in files)
        {
            var fileName = Path.GetFileName(src);
            // Parse: {templateName}_{lang}.md
            var lastUnderscore = fileName.LastIndexOf('_');
            if (lastUnderscore < 0) continue;
            var templateName = fileName[..lastUnderscore];
            var lang = fileName[(lastUnderscore + 1)..^3]; // strip .md

            string destPath;
            if (templateName == "readme" && lang == "zh-hans")
            {
                // Chinese README → repository root.
                destPath = Path.Combine(_repoRoot, "README.md");
            }
            else if (templateName == "readme")
            {
                // Other readme → docs/readme/README_{lang}.md (uppercase README).
                destPath = Path.Combine(_repoRoot, "docs", "readme",
                    $"README_{lang}.md");
            }
            else
            {
                // Other docs → docs/{name}/{name}_{lang}.md.
                destPath = Path.Combine(_repoRoot, "docs", templateName,
                    $"{templateName}_{lang}.md");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);
            File.Copy(src, destPath, overwrite: true);
            Console.WriteLine($"    {fileName} → {Path.GetRelativePath(_repoRoot, destPath)}");
        }

        Console.WriteLine("  [Step 6] Copy done.");
    }

    /// <summary>
    /// Process a single template document end-to-end.
    /// </summary>
    private async Task ProcessTemplateAsync(
        string templatePath,
        string templateName,
        string promptHeader,
        string promptTail,
        CancellationToken ct)
    {
        // ── Step 1: Parse template lines ──
        var rawLines = await Utf8NoBom.ReadAllLinesAsync(templatePath, ct);
        var templateLines = new List<DocTemplateLine>(rawLines.Length);
        for (int i = 0; i < rawLines.Length; i++)
        {
            var line = DocTemplateLine.FromLine(i + 1, rawLines[i]);
            // For translatable lines, mark all target languages as needing translation initially.
            if (line.Category == TemplateLineCategory.Translatable)
            {
                foreach (var lang in _targetLanguages)
                    line.NeedsTranslation[lang] = true;
            }
            else
            {
                foreach (var lang in _targetLanguages)
                    line.NeedsTranslation[lang] = false;
            }
            templateLines.Add(line);
        }
        Console.WriteLine($"  [Step 1] Parsed {templateLines.Count} lines: "
            + $"translatable={templateLines.Count(l => l.Category == TemplateLineCategory.Translatable)}, "
            + $"blank={templateLines.Count(l => l.Category == TemplateLineCategory.Blank)}, "
            + $"markdown={templateLines.Count(l => l.Category == TemplateLineCategory.MarkdownTag)}, "
            + $"placeholders={templateLines.Count(l => l.Category == TemplateLineCategory.Placeholder)}");

        // ── Step 1b: Load links mapping for placeholder resolution ──
        var linksMappingPath = Path.Combine(_templatesDir, $"{templateName}_links_mapping.json");
        var linksMapping = await LoadLinksMappingAsync(linksMappingPath, ct);

        // ── Step 2: Load cache ──
        var cachePath = Path.Combine(_templatesDir, $"{templateName}_template_cache.json");
        var cache = await LoadCacheAsync(cachePath, ct);
        var cacheByHash = cache.ToDictionary(c => c.Sha256, StringComparer.Ordinal);
        Console.WriteLine($"  [Step 2] Loaded cache: {cache.Count} entries.");

        // ── Step 3: Compare & collect lines needing translation ──
        var linesToTranslate = new Dictionary<string, List<DocTemplateLine>>(StringComparer.OrdinalIgnoreCase);
        foreach (var lang in _targetLanguages)
            linesToTranslate[lang] = [];

        foreach (var line in templateLines)
        {
            if (line.Category != TemplateLineCategory.Translatable)
                continue; // Blanks, pure markdown, placeholders never need translation.

            foreach (var lang in _targetLanguages)
            {
                // Check if we have a cache hit by hash.
                if (cacheByHash.TryGetValue(line.Sha256, out var cached))
                {
                    // Restore the LLM-determined needs-translation flag.
                    if (cached.NeedsTranslation.TryGetValue(lang, out var needs))
                        line.NeedsTranslation[lang] = needs;

                    // If cache says "no translation needed" or has valid translation → skip.
                    if (cached.IsExplicitlySkipped(lang) || cached.HasTranslation(lang))
                        continue;
                }

                linesToTranslate[lang].Add(line);
            }
        }

        foreach (var lang in _targetLanguages)
            Console.WriteLine($"  [Step 3] Lines to translate ({lang}): {linesToTranslate[lang].Count}");

        // ── Step 4: Translate all languages concurrently ──
        // Build full text context once (identical for all target languages).
        var fullText = BuildFullText(templateLines);
        var headerWithContext = promptHeader.Replace("{{FULL_TEXT}}", fullText);

        // Warmup: send header once per template (same context for all languages).
        Console.WriteLine($"  [Step 4a] Sending warmup...");
        var warmupOk = await SendWarmupAsync(headerWithContext, "en", templateName, ct);
        Console.WriteLine($"  [Step 4a] Warmup {(warmupOk ? "OK" : "FAILED")}.");

        // Build all batches for all languages upfront.
        var allBatches = new List<DocTranslationBatch>();
        foreach (var lang in _targetLanguages)
        {
            var batchLines = linesToTranslate[lang];
            if (batchLines.Count == 0)
            {
                Console.WriteLine($"  [Step 4] No lines to translate for {lang}, skipping.");
                continue;
            }

            var totalBatches = (int)Math.Ceiling((double)batchLines.Count / MaxLinesPerBatch);
            for (int i = 0; i < totalBatches; i++)
            {
                var slice = batchLines.Skip(i * MaxLinesPerBatch).Take(MaxLinesPerBatch).ToList();
                if (slice.Count > MaxLinesPerBatch)
                    throw new InvalidOperationException(
                        $"BUG: batch {i + 1} has {slice.Count} lines (limit={MaxLinesPerBatch})");
                allBatches.Add(new DocTranslationBatch
                {
                    TargetLang = lang,
                    BatchIndex = i + 1,
                    TotalBatches = totalBatches,
                    Lines = slice
                });
            }
        }

        if (allBatches.Count == 0)
        {
            Console.WriteLine("  [Step 4] No batches to translate.");
        }
        else
        {
            Console.WriteLine($"  [Step 4] Translating {allBatches.Count} batch(es) across {_targetLanguages.Length} languages, maxConcurrency={MaxConcurrency}");

            // All languages share one semaphore; all batches fly concurrently.
            var semaphore = new SemaphoreSlim(MaxConcurrency);
            var batchTasks = allBatches.Select(batch =>
                TranslateBatchWithRetryAsync(
                    headerWithContext, promptTail, batch, semaphore, templateName, ct));
            var batchResults = await Task.WhenAll(batchTasks);

            // Apply results back to cache, grouped by language for logging.
            foreach (var lang in _targetLanguages)
            {
                var langResults = batchResults.Where(r => r.Batch.TargetLang == lang).ToList();
                if (langResults.Count == 0) continue;

                var translatedCount = 0;
                var failedCount = 0;
                foreach (var result in langResults)
                {
                    if (result.Results == null)
                    {
                        failedCount += result.Batch.Lines.Count;
                        continue;
                    }

                    foreach (var lineResult in result.Results)
                    {
                        var line = result.Batch.Lines.FirstOrDefault(l => l.LineNumber == lineResult.LineNumber);
                        if (line == null) continue;

                        if (!cacheByHash.TryGetValue(line.Sha256, out var cacheEntry))
                        {
                            cacheEntry = new TemplateCacheEntry
                            {
                                Sha256 = line.Sha256,
                                SourceText = line.Text
                            };
                            cache.Add(cacheEntry);
                            cacheByHash[line.Sha256] = cacheEntry;
                        }

                        if (!string.IsNullOrEmpty(cacheEntry.SourceText)
                            && cacheEntry.SourceText != line.Text)
                        {
                            Console.Error.WriteLine(
                                $"  [CACHE WARN] SHA256 collision or reuse: " +
                                $"existing='{Truncate(cacheEntry.SourceText, 50)}' " +
                                $"new='{Truncate(line.Text, 50)}' " +
                                $"sha256={cacheEntry.Sha256[..12]}...");
                        }

                        cacheEntry.SourceText = line.Text;
                        cacheEntry.Translations[lang] = lineResult.TranslatedText;
                        cacheEntry.NeedsTranslation[lang] = line.NeedsTranslation[lang];

                        if (!string.IsNullOrWhiteSpace(lineResult.TranslatedText))
                            translatedCount++;
                        else
                            cacheEntry.NeedsTranslation[lang] = false;
                    }
                }

                Console.WriteLine($"  [Step 4] {lang}: translated={translatedCount}, failed={failedCount}");
            }
        }

        // ── Step 5: Write outputs ──
        // 5a: Save cache.
        SaveCache(cachePath, cache);
        Console.WriteLine($"  [Step 5a] Cache saved: {cachePath}");

        // 5b: Assemble final document per language, resolve placeholders, and write to temp.
        foreach (var lang in _targetLanguages)
        {
            var outputPath = Path.Combine(_outputDir, $"{templateName}_{lang}.md");
            AssembleFinalDoc(templateLines, cacheByHash, lang, outputPath);

            // Post-process: resolve placeholders and generate TOC.
            var raw = await Utf8NoBom.ReadAllTextAsync(outputPath, ct);
            var resolved = ResolveAllPlaceholders(raw, linksMapping, lang);
            await Utf8NoBom.WriteAllTextAsync(outputPath, resolved, ct);

            Console.WriteLine($"  [Step 5b] Output ({lang}): {outputPath}");
        }

        // Also write the source (zh-hans) with placeholders resolved.
        {
            var zhPath = Path.Combine(_outputDir, $"{templateName}_zh-hans.md");
            var zhRaw = string.Join("\n", rawLines) + "\n";
            var zhResolved = ResolveAllPlaceholders(zhRaw, linksMapping, "zh-hans");
            await Utf8NoBom.WriteAllTextAsync(zhPath, zhResolved, ct);
            Console.WriteLine($"  [Step 5b] Output (zh-hans): {zhPath}");
        }
    }

    /// <summary>
    /// Build the FULL_TEXT context from all non-empty template lines.
    /// </summary>
    private static string BuildFullText(List<DocTemplateLine> lines)
    {
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line.Text))
                continue;
            sb.AppendLine(line.Text);
        }
        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Send a warmup request with just the header to warm the LLM cache.
    /// Waits for a complete response (must contain "READY") before returning.
    /// </summary>
    private async Task<bool> SendWarmupAsync(
        string headerWithContext, string targetLang, string templateName, CancellationToken ct)
    {
        try
        {
            var warmupMsg = headerWithContext + "\n\n# 预热确认\n请回复\"READY\"。";
            var (success, responseText, _) = await SendLlmRequestAsync(warmupMsg, ct);
            if (!success || string.IsNullOrWhiteSpace(responseText))
            {
                Console.Error.WriteLine($"  [WARN] Warmup for {templateName}: empty or failed response.");
                return false;
            }
            if (!responseText.Contains("READY", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine($"  [WARN] Warmup for {templateName}: response missing READY confirmation. Got: {Truncate(responseText, 100)}");
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"  [WARN] Warmup failed for {templateName}/{targetLang}: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Translate a batch with up to MaxRetries retries on failure.
    /// </summary>
    private async Task<BatchTranslateResult> TranslateBatchWithRetryAsync(
        string headerWithContext,
        string promptTail,
        DocTranslationBatch batch,
        SemaphoreSlim semaphore,
        string templateName,
        CancellationToken ct)
    {
        // Build the tail part for this batch.
        var batchText = BuildBatchText(batch);
        var tail = promptTail
            .Replace("{{TARGET_LANG}}", batch.TargetLang)
            .Replace("{{LINE_COUNT}}", batch.Lines.Count.ToString())
            .Replace("{{TEXT}}", batchText);
        var fullPrompt = headerWithContext + "\n\n" + tail;

        // Save prompt for debugging.
        await SaveDebugFileAsync(
            $"prompt_{templateName}_{batch.TargetLang}_b{batch.BatchIndex}.txt",
            fullPrompt, ct);

        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                await semaphore.WaitAsync(ct);
                try
                {
                    var (success, responseText, _) = await SendLlmRequestAsync(fullPrompt, ct);

                    // Save response for debugging.
                    await SaveDebugFileAsync(
                        $"response_{templateName}_{batch.TargetLang}_b{batch.BatchIndex}_a{attempt}.txt",
                        responseText ?? "(null)", ct);

                    if (!success || string.IsNullOrWhiteSpace(responseText))
                    {
                        if (attempt < MaxRetries)
                        {
                            Console.Error.WriteLine(
                                $"  [RETRY] Batch {batch.BatchIndex}/{batch.TotalBatches} ({batch.TargetLang}) attempt {attempt}: empty response");
                            await Task.Delay(RetryBaseDelay * attempt, ct);
                            continue;
                        }
                        return new BatchTranslateResult(batch, null);
                    }

                    var results = ParseTranslationResponse(responseText, batch);

                    // Validate table rows: check for missing leading | and column count mismatch.
                    var (tableValid, tableErrors) = ValidateTableResults(results, batch);
                    if (!tableValid && attempt < MaxRetries)
                    {
                        Console.Error.WriteLine(
                            $"  [TABLE RETRY] Batch {batch.BatchIndex}/{batch.TotalBatches} ({batch.TargetLang}) attempt {attempt}: {tableErrors}");
                        await Task.Delay(RetryBaseDelay * attempt, ct);
                        continue;
                    }
                    if (!tableValid)
                    {
                        Console.Error.WriteLine(
                            $"  [TABLE WARN] Batch {batch.BatchIndex}/{batch.TotalBatches} ({batch.TargetLang}) final attempt: table issues unfixed. {tableErrors}");
                    }

                    return new BatchTranslateResult(batch, results);
                }
                finally
                {
                    semaphore.Release();
                }
            }
            catch (Exception ex) when (attempt < MaxRetries)
            {
                Console.Error.WriteLine(
                    $"  [RETRY] Batch {batch.BatchIndex}/{batch.TotalBatches} ({batch.TargetLang}) attempt {attempt}: {ex.Message}");
                await Task.Delay(RetryBaseDelay * attempt, ct);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"  [FAIL] Batch {batch.BatchIndex}/{batch.TotalBatches} ({batch.TargetLang}) after {MaxRetries} attempts: {ex.Message}");
                return new BatchTranslateResult(batch, null);
            }
        }

        return new BatchTranslateResult(batch, null);
    }

    /// <summary>
    /// Build the {{TEXT}} block for a batch: "&lt;index&gt; &lt;text&gt;" per line.
    /// </summary>
    private static string BuildBatchText(DocTranslationBatch batch)
    {
        var sb = new StringBuilder();
        foreach (var line in batch.Lines)
        {
            sb.Append(line.LineNumber);
            sb.Append(' ');
            sb.AppendLine(line.Text);
        }
        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Parse LLM translation response: tab-separated &lt;index&gt; &lt;translation&gt; &lt;confidence&gt; [comment].
    /// </summary>
    private static List<LlmLineResult> ParseTranslationResponse(string responseText, DocTranslationBatch batch)
    {
        var results = new List<LlmLineResult>();
        var lineSet = new HashSet<int>(batch.Lines.Select(l => l.LineNumber));

        // Remove markdown code fences if present.
        var clean = Regex.Replace(responseText, @"^```[\s\S]*?```", "", RegexOptions.Multiline).Trim();

        foreach (var rawLine in clean.Split('\n', StringSplitOptions.None))
        {
            var trimmed = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            // Expected format: <index>\t<translation>\t<confidence>\t[comment]
            var parts = trimmed.Split('\t', 4);
            if (parts.Length < 1)
                continue;

            if (!int.TryParse(parts[0].Trim(), out var lineNumber))
                continue;

            if (!lineSet.Contains(lineNumber))
                continue;

            var result = new LlmLineResult { LineNumber = lineNumber };

            if (parts.Length >= 2)
                result.TranslatedText = parts[1].Trim();
            if (parts.Length >= 3 && float.TryParse(parts[2].Trim(),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var conf))
            {
                result.Confidence = conf;
            }
            if (parts.Length >= 4)
                result.Comment = parts[3].Trim();

            results.Add(result);
        }

        return results;
    }

    /// <summary>
    /// Fix missing leading | in table rows within the assembled document.
    /// </summary>
    private static string FixTableRows(string document)
    {
        var lines = document.Replace("\r", "").Split('\n');
        var result = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.Trim();

            var hasPipes = trimmed.Contains('|');
            if (!hasPipes)
            {
                result.Add(line);
                continue;
            }

            var prevIsTable = i > 0 && lines[i - 1].TrimStart().StartsWith('|');
            var nextIsTable = i + 1 < lines.Length
                && lines[i + 1].TrimStart().StartsWith('|');
            var isTableSeparator = trimmed.StartsWith('|')
                && trimmed.Replace("|", "").Replace("-", "").Replace(":", "").Replace(" ", "").Length == 0;

            if ((prevIsTable || nextIsTable || isTableSeparator) && !trimmed.StartsWith("| "))
            {
                if (trimmed.Length > 0 && trimmed[0] != '|' && !trimmed.StartsWith("|-"))
                {
                    result.Add("| " + trimmed);
                    continue;
                }
            }
            result.Add(line);
        }
        return string.Join('\n', result);
    }

    /// <summary>Public testable wrapper for FixListItems (dev only).</summary>
    public static string FixListItemsPublic(string document) => FixListItems(document);

    /// <summary>
    /// Fix list items that lost or mangled their leading list markers during translation.
    /// Handles: (a) completely dropped "- " / "* ", (b) malformed "*- " → "- ".
    /// Only fixes lines that are in a list context (previous line is a list item).
    /// </summary>
    private static string FixListItems(string document)
    {
        // Normalize line endings: strip \r to avoid indent miscalculation.
        var lines = document.Replace("\r", "").Split('\n');
        var result = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.Trim();
            var leadingWs = line.Length - trimmed.Length; // indentation to preserve

            // Already a proper list item — pass through.
            if (trimmed.StartsWith("- ") || trimmed.StartsWith("* ")
                || Regex.IsMatch(trimmed, @"^\d+\.\s"))
            {
                result.Add(line);
                continue;
            }

            // Not in a list context — pass through.
            var prevTrimmed = i > 0 ? lines[i - 1].TrimStart() : "";
            var prevIsListItem = i > 0 && (
                prevTrimmed.StartsWith("- ")
                || prevTrimmed.StartsWith("* ")
                || Regex.IsMatch(prevTrimmed, @"^\d+\.\s"));
            if (!prevIsListItem)
            {
                result.Add(line);
                continue;
            }

            // Determine the expected list marker from the previous line.
            var marker = prevTrimmed.StartsWith("- ") ? "- "
                : prevTrimmed.StartsWith("* ") ? "* "
                : "- ";

            // Check for malformed marker patterns: *-, --, * -, - -
            string cleanContent;
            if (trimmed.StartsWith("*- "))
                cleanContent = trimmed[3..].TrimStart();
            else if (trimmed.StartsWith("-- "))
                cleanContent = trimmed[3..].TrimStart();
            else if (trimmed.StartsWith("* - "))
                cleanContent = trimmed[4..].TrimStart();
            else if (trimmed.StartsWith("- - "))
                cleanContent = trimmed[4..].TrimStart();
            else if (trimmed.StartsWith("**") || trimmed.StartsWith("*")
                    || trimmed.StartsWith("`") || trimmed.StartsWith("_"))
                cleanContent = trimmed;
            else
            {
                result.Add(line);
                continue;
            }

            var indent = leadingWs > 0 ? line[..leadingWs] : "";
            result.Add(indent + marker + cleanContent);
        }

        return string.Join('\n', result);
    }

    /// <summary>
    /// Validate translated table rows: check leading | presence and column counts.
    /// Returns (isValid, errorMessage). If fixable (missing leading |), fixes in-place.
    /// </summary>
    private static (bool isValid, string? error) ValidateTableResults(
        List<LlmLineResult> results, DocTranslationBatch batch)
    {
        // Build a lookup from line number to source line and result.
        var sourceByNum = batch.Lines.ToDictionary(l => l.LineNumber);
        var resultByNum = results.ToDictionary(r => r.LineNumber);

        // Group consecutive table rows in the batch.
        var tableGroups = new List<List<int>>();
        List<int>? currentGroup = null;
        foreach (var line in batch.Lines.OrderBy(l => l.LineNumber))
        {
            var sourceText = line.Text;
            var isTableRow = TableRowRegex().IsMatch(sourceText);
            if (isTableRow)
            {
                currentGroup ??= [];
                currentGroup.Add(line.LineNumber);
            }
            else
            {
                if (currentGroup is { Count: > 0 })
                {
                    tableGroups.Add(currentGroup);
                    currentGroup = null;
                }
            }
        }
        if (currentGroup is { Count: > 0 })
            tableGroups.Add(currentGroup);

        var errors = new List<string>();
        foreach (var group in tableGroups)
        {
            // Determine the expected column count from the source.
            var expectedCols = group
                .Select(num => sourceByNum[num].Text.Split('|').Length)
                .Max();

            foreach (var lineNum in group)
            {
                if (!resultByNum.TryGetValue(lineNum, out var result))
                    continue;
                var translated = result.TranslatedText;
                if (string.IsNullOrWhiteSpace(translated))
                    continue; // Skipped line — not our problem.

                var sourceText = sourceByNum[lineNum].Text;

                // Check 1: Does source have leading `| ` but translation doesn't?
                var srcHasLeadingPipe = TableRowRegex().IsMatch(sourceText);
                var tgtHasLeadingPipe = TableRowRegex().IsMatch(translated);
                if (srcHasLeadingPipe && !tgtHasLeadingPipe)
                {
                    // Fix: prepend "| " if the translation starts with non-pipe content.
                    if (translated.Length > 0 && translated[0] != '|')
                    {
                        result.TranslatedText = "| " + translated;
                    }
                }

                // Check 2: Column count.
                var srcCols = sourceText.Split('|').Length;
                var tgtCols = result.TranslatedText.Split('|').Length;
                if (srcCols != tgtCols)
                {
                    errors.Add(
                        $"line {lineNum}: source has {srcCols} cols, translated has {tgtCols} cols");
                }
            }
        }

        if (errors.Count > 0)
            return (false, string.Join("; ", errors));
        return (true, null);
    }

    /// <summary>
    /// Fix TOC anchors in the assembled document: map Chinese anchors → English anchors
    /// by matching headings by position (they appear in the same order in both documents).
    /// </summary>
    private static string FixTocAnchors(string document, List<DocTemplateLine> sourceLines)
    {
        // Extract Chinese headings from source in order.
        var cnHeadings = new List<(int LineNum, string Heading, string Anchor)>();
        for (int i = 0; i < sourceLines.Count; i++)
        {
            var lineText = sourceLines[i].Text;
            var m = Regex.Match(lineText, @"^#{2,4}\s+(.+)$");
            if (m.Success)
            {
                var headingText = m.Groups[1].Value.Trim();
                cnHeadings.Add((i + 1, headingText, GenerateGitHubAnchor(headingText)));
            }
        }

        // Extract English headings from the generated document in order.
        var enHeadings = new List<(int Index, string Heading, string Anchor)>();
        var docLines = document.Split('\n');
        int enIdx = 0;
        for (int i = 0; i < docLines.Length; i++)
        {
            var m = Regex.Match(docLines[i], @"^#{2,4}\s+(.+)$");
            if (m.Success)
            {
                var headingText = m.Groups[1].Value.Trim();
                enHeadings.Add((enIdx, headingText, GenerateGitHubAnchor(headingText)));
                enIdx++;
            }
        }

        // Build anchor map: Chinese GitHub anchor → English GitHub anchor.
        // Headings appear in the same order; match by position index.
        var anchorMap = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int i = 0; i < cnHeadings.Count && i < enHeadings.Count; i++)
        {
            anchorMap[cnHeadings[i].Anchor] = enHeadings[i].Anchor;
            // Also map the raw Chinese heading text itself (for anchors that use unprocessed text).
            anchorMap[cnHeadings[i].Heading] = enHeadings[i].Anchor;
        }

        if (anchorMap.Count == 0)
            return document;

        // Replace TOC anchors.
        return TocLinkRegex().Replace(document, match =>
        {
            var linkText = match.Groups[1].Value;
            var anchor = match.Groups[2].Value;
            var decodedAnchor = Uri.UnescapeDataString(anchor);

            if (anchorMap.TryGetValue(decodedAnchor, out var newAnchor))
                return $"[{linkText}](#{newAnchor})";

            // No mapping found — keep original.
            return match.Value;
        });
    }

    /// <summary>
    /// Generate a GitHub-style heading anchor from heading text.
    /// Converts to lowercase, keeps only letters/digits/spaces/hyphens, spaces→hyphens.
    /// </summary>
    private static string GenerateGitHubAnchor(string heading)
    {
        return Regex.Replace(
            Regex.Replace(heading.ToLowerInvariant(), @"[^\w\s-]", ""),
            @"\s+", "-").Trim('-');
    }

    /// <summary>
    /// Assemble the final multilingual document by substituting translated lines.
    /// Lines that don't need translation or have empty translation keep the original text.
    /// Then apply TOC anchor fix and table row fix post-processing.
    /// </summary>
    private static void AssembleFinalDoc(
        List<DocTemplateLine> templateLines,
        Dictionary<string, TemplateCacheEntry> cacheByHash,
        string lang,
        string outputPath)
    {
        var sb = new StringBuilder();
        foreach (var line in templateLines)
        {
            if (line.Category == TemplateLineCategory.Translatable
                && cacheByHash.TryGetValue(line.Sha256, out var cached)
                && cached.Translations.TryGetValue(lang, out var translated)
                && !string.IsNullOrWhiteSpace(translated))
            {
                sb.AppendLine(translated);
            }
            else
            {
                sb.AppendLine(line.Text);
            }
        }

        var rawDoc = sb.ToString();

        // Post-processing: fix TOC anchors, table rows, and list item markers.
        var fixedDoc = FixTocAnchors(rawDoc, templateLines);
        fixedDoc = FixTableRows(fixedDoc);
        fixedDoc = FixListItems(fixedDoc);

        Utf8NoBom.WriteAllText(outputPath, fixedDoc);
    }

    // ── Cache I/O ──

    private static async Task<List<TemplateCacheEntry>> LoadCacheAsync(
        string cachePath, CancellationToken ct)
    {
        if (!File.Exists(cachePath))
            return [];

        try
        {
            var json = await Utf8NoBom.ReadAllTextAsync(cachePath, ct);
            return JsonSerializer.Deserialize<List<TemplateCacheEntry>>(json, Utf8NoBom.JsonOptions) ?? [];
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"  [WARN] Failed to load cache {cachePath}: {ex.Message}. Starting fresh.");
            return [];
        }
    }

    private static void SaveCache(string cachePath, List<TemplateCacheEntry> cache)
    {
        var json = Utf8NoBom.SerializeIndentedJson(cache);
        Utf8NoBom.WriteAllText(cachePath, json);
    }

    private async Task SaveDebugFileAsync(string fileName, string content, CancellationToken ct)
    {
        var path = Path.Combine(_debugDir, fileName);
        await Utf8NoBom.WriteAllTextAsync(path, content, ct);
    }

    // ── LLM HTTP Client ──

    private static HttpClient CreateDefaultClient()
    {
        var handler = new SocketsHttpHandler
        {
            MaxConnectionsPerServer = MaxConcurrency,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        };
        return new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(5) };
    }

    private async Task<(bool success, string? responseText, string? error)> SendLlmRequestAsync(
        string prompt, CancellationToken ct)
    {
        var apiKey = ResolveApiKey();
        var endpoint = ResolveEndpoint();
        var model = ResolveModel();

        var requestBody = new
        {
            model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            temperature = 0.1f,
            max_tokens = 8192
        };

        var json = Utf8NoBom.SerializeJson(requestBody);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = content
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        using var response = await _httpClient.SendAsync(request, ct);
        var responseBody = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
            return (false, null, $"HTTP {response.StatusCode}: {Truncate(responseBody)}");

        try
        {
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;
            if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var message = choices[0].GetProperty("message");
                var text = message.GetProperty("content").GetString();
                return (true, text, null);
            }
            return (false, null, "No choices in response");
        }
        catch (Exception ex)
        {
            return (false, null, $"Parse error: {ex.Message}");
        }
    }

    private static string ResolveApiKey()
    {
        var key = Environment.GetEnvironmentVariable("LLM_KEY")
               ?? Environment.GetEnvironmentVariable("DEEPSEEK_API_KEY");
        if (string.IsNullOrWhiteSpace(key))
        {
            // Try reading from secrets.json.
            var secretsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..",
                "config", "secrets.json");
            try
            {
                if (File.Exists(secretsPath))
                {
                    using var doc = JsonDocument.Parse(File.ReadAllText(secretsPath));
                    if (doc.RootElement.TryGetProperty("LLM_KEY", out var prop))
                        key = prop.GetString();
                }
            }
            catch { /* fall through */ }
        }
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("LLM_KEY not found in environment or secrets.json");
        return key;
    }

    private static string ResolveEndpoint() =>
        Environment.GetEnvironmentVariable("LLM_ENDPOINT")
        ?? "https://api.deepseek.com/chat/completions";

    private static string ResolveModel() =>
        Environment.GetEnvironmentVariable("LLM_MODEL")
        ?? "deepseek-v4-flash";

    private static string Truncate(string text, int maxLen = 200) =>
        text.Length <= maxLen ? text : text[..maxLen] + "...";

    // ── Placeholder Resolution & TOC Generation ──

    /// <summary>
    /// Load the *_links_mapping.json file for a template.
    /// Returns an empty mapping if the file doesn't exist (not all templates have one).
    /// </summary>
    private static async Task<LinksMapping> LoadLinksMappingAsync(
        string mappingPath, CancellationToken ct)
    {
        if (!File.Exists(mappingPath))
            return new LinksMapping();

        try
        {
            var json = await Utf8NoBom.ReadAllTextAsync(mappingPath, ct);
            return JsonSerializer.Deserialize<LinksMapping>(json, Utf8NoBom.JsonOptions)
                   ?? new LinksMapping();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"  [WARN] Failed to load links mapping {mappingPath}: {ex.Message}");
            return new LinksMapping();
        }
    }

    /// <summary>
    /// Resolve all {{placeholder}} patterns in the assembled document.
    /// Handles: {{TABLE_OF_CONTENTS}}, {{multi_lang_file_links_block}},
    /// {{md_link_N}}, {{url_N}}, and named links from the mapping.
    /// </summary>
    private static string ResolveAllPlaceholders(
        string document, LinksMapping mapping, string lang)
    {
        // Step 1: Generate TOC first (so it's in place before other replacements).
        document = GenerateTableOfContents(document);

        // Step 2: Replace named links and other simple placeholders.
        document = Regex.Replace(document, @"\{\{(.+?)\}\}", match =>
        {
            var key = match.Groups[1].Value;

            // multi_lang_file_links_block: generate the full language-switcher block.
            if (key == "multi_lang_file_links_block" && mapping.multi_lang_file_links_block != null)
                return BuildMultiLangLinksBlock(mapping.multi_lang_file_links_block, lang);

            // TABLE_OF_CONTENTS is handled above; if still present, leave as-is.
            if (key == "TABLE_OF_CONTENTS")
                return match.Value;

            // md_link_N → [text](url) markdown link.
            if (key.StartsWith("md_link_")
                && int.TryParse(key["md_link_".Length..], out var mdIdx)
                && mapping.MdLinkDefs.TryGetValue(mdIdx.ToString(), out var mdDef))
            {
                var url = SubstituteLang(mdDef.url, lang);
                return $"[{mdDef.text}]({url})";
            }

            // url_N → raw URL (mapping uses numeric keys, strip "url_" prefix).
            if (key.StartsWith("url_")
                && mapping.url_blocks != null)
            {
                var urlKey = key["url_".Length..];
                if (mapping.url_blocks.TryGetValue(urlKey, out var rawUrl)
                    || mapping.url_blocks.TryGetValue(key, out rawUrl))
                {
                    return SubstituteLang(rawUrl, lang);
                }
            }

            // named_links fallback: {{progress_link}}, {{contributing_link}}, etc.
            if (mapping.named_links != null
                && mapping.named_links.TryGetValue(key, out var namedVal))
            {
                return SubstituteLang(namedVal, lang);
            }

            // Unknown placeholder — keep as-is.
            return match.Value;
        });

        return document;
    }

    /// <summary>
    /// Replace {lang} and {cc_locale} tokens in a URL with the actual language code.
    /// </summary>
    private static string SubstituteLang(string value, string lang)
    {
        return value
            .Replace("{lang}", lang)
            .Replace("{cc_locale}", GetCcLocale(lang));
    }

    /// <summary>
    /// Map ISO language code to Creative Commons locale code.
    /// </summary>
    private static string GetCcLocale(string lang) => lang switch
    {
        "zh-hans" => "zh-Hans",
        "zh-hant" => "zh-Hant",
        "pt-br" => "pt_BR",
        _ => lang.Split('-')[0] // e.g., "en", "ja", "fr"
    };

    /// <summary>
    /// Find {{TABLE_OF_CONTENTS}} in the document and replace it with a
    /// programmatically generated TOC from the document's ## and ### headings.
    /// </summary>
    private static string GenerateTableOfContents(string document)
    {
        if (!document.Contains("{{TABLE_OF_CONTENTS}}"))
            return document;

        // Collect all ## and ### headings after the TOC placeholder.
        var headings = new List<(int Level, string Text, string Anchor)>();
        var lines = document.Replace("\r", "").Split('\n');
        bool pastToc = false;

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Detect the TOC placeholder line.
            if (trimmed == "{{TABLE_OF_CONTENTS}}")
            {
                pastToc = true;
                continue;
            }

            if (!pastToc)
                continue;

            var m = Regex.Match(trimmed, @"^(#{2,4})\s+(.+)$");
            if (!m.Success)
                continue;

            var level = m.Groups[1].Value.Length; // 2, 3, or 4
            var text = m.Groups[2].Value.Trim();
            var anchor = GitHubAnchor(text);
            headings.Add((level, text, anchor));
        }

        // Build TOC.
        var sb = new StringBuilder();
        foreach (var (level, text, anchor) in headings)
        {
            var indent = new string(' ', (level - 2) * 2);
            sb.Append(indent);
            sb.Append("- [");
            sb.Append(text);
            sb.Append("](#");
            sb.Append(anchor);
            sb.AppendLine(")");
        }

        var toc = sb.ToString().TrimEnd();
        return document.Replace("{{TABLE_OF_CONTENTS}}\n", toc + "\n")
                       .Replace("{{TABLE_OF_CONTENTS}}", toc);
    }

    /// <summary>
    /// Generate a GitHub-style heading anchor: lowercase, keep alphanumeric/space/hyphen,
    /// spaces→hyphens, strip leading/trailing hyphens.
    /// </summary>
    private static string GitHubAnchor(string heading)
    {
        return Regex.Replace(
            Regex.Replace(heading.ToLowerInvariant(), @"[^\w\s-]", ""),
            @"\s+", "-").Trim('-');
    }

    /// <summary>
    /// Build the multi-language file links block for top-of-document language switcher.
    /// Rules:
    /// - English doc → primary link shows [简体中文] pointing to Chinese (zh-hans).
    /// - Chinese doc → primary link shows [English] pointing to English (en).
    /// - Other languages → primary links show both [English] and [简体中文].
    /// - zh-hans doc lives at repo root (README.md), others at docs/readme/README_{lang}.md.
    ///   URLs in mapping are relative to docs/readme/; for zh-hans root doc, prepend docs/readme/.
    /// </summary>
    private static string BuildMultiLangLinksBlock(MultiLangBlock block, string currentLang)
    {
        // zh-hans README is at repo root; all other readme files are under docs/readme/.
        // Mapping URLs are relative to docs/readme/. For root README, prepend docs/readme/.
        string ResolveUrl(string url)
        {
            if (currentLang == "zh-hans" && !url.StartsWith("http") && !url.StartsWith("../"))
                return "docs/readme/" + url;
            return url;
        }

        var sb = new StringBuilder();
        sb.Append("> ");

        // Get both primary link URLs from the mapping.
        block.primary_links ??= new Dictionary<string, LangLinkDef>();
        block.primary_links.TryGetValue("en", out var enPrimary);
        block.primary_links.TryGetValue("zh-hans", out var zhPrimary);

        if (currentLang == "en")
        {
            // English doc: show [简体中文] → link to Chinese.
            sb.Append("[简体中文](");
            sb.Append(ResolveUrl(zhPrimary?.url ?? ""));
            sb.Append(")");
        }
        else if (currentLang == "zh-hans")
        {
            // Chinese doc: show [English] → link to English.
            sb.Append("[English](");
            sb.Append(ResolveUrl(enPrimary?.url ?? ""));
            sb.Append(")");
        }
        else
        {
            // Other languages: show both [English] and [简体中文] as primary links.
            if (enPrimary != null)
            {
                sb.Append("[English](");
                sb.Append(ResolveUrl(enPrimary.url));
                sb.Append(")");
            }
            if (enPrimary != null && zhPrimary != null)
            {
                sb.Append(" | ");
            }
            if (zhPrimary != null)
            {
                sb.Append("[简体中文](");
                sb.Append(ResolveUrl(zhPrimary.url));
                sb.Append(")");
            }
        }

        sb.Append(" <details><summary>");

        // Summary text: "其它语言" for zh-hans source, "Other Languages" otherwise.
        var summaryText = currentLang == "zh-hans" ? "其它语言" : "Other Languages";
        sb.Append(summaryText);
        sb.Append("</summary>");

        if (block.language_links != null)
        {
            foreach (var link in block.language_links)
            {
                sb.Append('[');
                sb.Append(link.text);
                sb.Append("](");
                sb.Append(ResolveUrl(link.url));
                sb.Append(") | ");
            }
            // Remove trailing " | ".
            if (block.language_links.Count > 0)
                sb.Length -= 3;
        }

        sb.Append("</details>");
        return sb.ToString();
    }
}

/// <summary>
/// Result from a batch translation attempt.
/// </summary>
internal sealed record BatchTranslateResult(
    DocTranslationBatch Batch,
    List<LlmLineResult>? Results);
