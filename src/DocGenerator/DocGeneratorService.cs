using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Common;

namespace DocGenerator;

/// <summary>
/// Documentation generator — translates Chinese template docs into multiple languages via LLM.
/// Currently dev/test phase: only zh-hans → en, outputs to temp folder.
/// </summary>
public sealed class DocGeneratorService
{
    private const int MaxLinesPerBatch = 30;
    private const int MaxConcurrency = 128;
    private const int MaxRetries = 3;
    private static readonly TimeSpan RetryBaseDelay = TimeSpan.FromSeconds(2);

    private readonly string _repoRoot;
    private readonly string _templatesDir;
    private readonly string _outputDir;
    private readonly string _debugDir;
    private readonly HttpClient _httpClient;

    // Currently only en is supported in dev phase.
    private static readonly string[] TargetLanguages = ["en"];

    public DocGeneratorService(string repoRoot, HttpClient? httpClient = null)
    {
        _repoRoot = repoRoot;
        _templatesDir = Path.Combine(repoRoot, "docs", "templates");
        _outputDir = Path.Combine(repoRoot, "temp", "docgen");
        _debugDir = Path.Combine(_outputDir, "debug");
        _httpClient = httpClient ?? CreateDefaultClient();
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
                foreach (var lang in TargetLanguages)
                    line.NeedsTranslation[lang] = true;
            }
            else
            {
                foreach (var lang in TargetLanguages)
                    line.NeedsTranslation[lang] = false;
            }
            templateLines.Add(line);
        }
        Console.WriteLine($"  [Step 1] Parsed {templateLines.Count} lines: "
            + $"translatable={templateLines.Count(l => l.Category == TemplateLineCategory.Translatable)}, "
            + $"blank={templateLines.Count(l => l.Category == TemplateLineCategory.Blank)}, "
            + $"markdown={templateLines.Count(l => l.Category == TemplateLineCategory.MarkdownTag)}, "
            + $"placeholders={templateLines.Count(l => l.Category == TemplateLineCategory.Placeholder)}");

        // ── Step 2: Load cache ──
        var cachePath = Path.Combine(_templatesDir, $"{templateName}_template_cache.json");
        var cache = await LoadCacheAsync(cachePath, ct);
        var cacheByHash = cache.ToDictionary(c => c.Sha256, StringComparer.Ordinal);
        Console.WriteLine($"  [Step 2] Loaded cache: {cache.Count} entries.");

        // ── Step 3: Compare & collect lines needing translation ──
        var linesToTranslate = new Dictionary<string, List<DocTemplateLine>>(StringComparer.OrdinalIgnoreCase);
        foreach (var lang in TargetLanguages)
            linesToTranslate[lang] = [];

        foreach (var line in templateLines)
        {
            if (line.Category != TemplateLineCategory.Translatable)
                continue; // Blanks, pure markdown, placeholders never need translation.

            foreach (var lang in TargetLanguages)
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

        foreach (var lang in TargetLanguages)
            Console.WriteLine($"  [Step 3] Lines to translate ({lang}): {linesToTranslate[lang].Count}");

        // ── Step 4: Translate per language ──
        foreach (var lang in TargetLanguages)
        {
            var batchLines = linesToTranslate[lang];
            if (batchLines.Count == 0)
            {
                Console.WriteLine($"  [Step 4] No lines to translate for {lang}, skipping.");
                continue;
            }

            // Build batches (max 30 lines per batch).
            var batches = new List<DocTranslationBatch>();
            var totalBatches = (int)Math.Ceiling((double)batchLines.Count / MaxLinesPerBatch);
            for (int i = 0; i < totalBatches; i++)
            {
                var slice = batchLines.Skip(i * MaxLinesPerBatch).Take(MaxLinesPerBatch).ToList();
                batches.Add(new DocTranslationBatch
                {
                    TargetLang = lang,
                    BatchIndex = i + 1,
                    TotalBatches = totalBatches,
                    Lines = slice
                });
            }
            Console.WriteLine($"  [Step 4] Translating {lang}: {batches.Count} batch(es), maxConcurrency={MaxConcurrency}");

            // Build full text context (all non-empty lines from the template).
            var fullText = BuildFullText(templateLines);

            // Build the complete prompt: header (with FULL_TEXT) + tail (with TARGET_LANG, LINE_COUNT, TEXT filled later).
            var headerWithContext = promptHeader.Replace("{{FULL_TEXT}}", fullText);

            // Warmup: send header alone to warm the LLM context.
            Console.WriteLine($"  [Step 4a] Sending warmup for {lang}...");
            var warmupOk = await SendWarmupAsync(headerWithContext, lang, templateName, ct);
            Console.WriteLine($"  [Step 4a] Warmup {(warmupOk ? "OK" : "FAILED")}.");

            // Translate batches concurrently within this language.
            var semaphore = new SemaphoreSlim(MaxConcurrency);
            var batchTasks = batches.Select(batch =>
                TranslateBatchWithRetryAsync(
                    headerWithContext, promptTail, batch, semaphore, templateName, ct));
            var batchResults = await Task.WhenAll(batchTasks);

            // Apply results back to cache.
            var translatedCount = 0;
            var failedCount = 0;
            foreach (var result in batchResults)
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

                    // Update or create cache entry.
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

                    cacheEntry.SourceText = line.Text;
                    cacheEntry.Translations[lang] = lineResult.TranslatedText;
                    cacheEntry.NeedsTranslation[lang] = line.NeedsTranslation[lang];

                    if (!string.IsNullOrWhiteSpace(lineResult.TranslatedText))
                        translatedCount++;
                }
            }

            Console.WriteLine($"  [Step 4] {lang}: translated={translatedCount}, failed={failedCount}");
        }

        // ── Step 5: Write outputs ──
        // 5a: Save cache.
        SaveCache(cachePath, cache);
        Console.WriteLine($"  [Step 5a] Cache saved: {cachePath}");

        // 5b: Assemble final document per language and write to temp.
        foreach (var lang in TargetLanguages)
        {
            var outputPath = Path.Combine(_outputDir, $"{templateName}_{lang}.md");
            AssembleFinalDoc(templateLines, cacheByHash, lang, outputPath);
            Console.WriteLine($"  [Step 5b] Output ({lang}): {outputPath}");
        }

        // Also write the source (zh-hans) for reference.
        {
            var zhPath = Path.Combine(_outputDir, $"{templateName}_zh-hans.md");
            await Utf8NoBom.WriteAllTextAsync(
                zhPath, string.Join("\n", rawLines) + "\n", ct);
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
    /// </summary>
    private async Task<bool> SendWarmupAsync(
        string headerWithContext, string targetLang, string templateName, CancellationToken ct)
    {
        try
        {
            var warmupMsg = headerWithContext + "\n\n# 预热确认\n请回复\"READY\"。";
            var (success, _, _) = await SendLlmRequestAsync(warmupMsg, ct);
            return success;
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
    /// Assemble the final multilingual document by substituting translated lines.
    /// Lines that don't need translation or have empty translation keep the original text.
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
        Utf8NoBom.WriteAllText(outputPath, sb.ToString());
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
}

/// <summary>
/// Result from a batch translation attempt.
/// </summary>
internal sealed record BatchTranslateResult(
    DocTranslationBatch Batch,
    List<LlmLineResult>? Results);
