using Common;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;

namespace EmbeddingFetcher;

/// <summary>
/// Generates vector embeddings for content used by RAG retrieval.
/// Protocol ported from ref EmbeddingService: UDP knock → AES-256-GCM encrypt → HTTP POST.
/// </summary>
public class EmbeddingFetcherService
{
    private const string Model = "bge-small-en-v1.5";
    private const int MaxUtf8Chars = 500;
    private const int MaxRetries = 3;
    private const int BatchSize = 32;
    private const int VerboseProgressBatchLimit = 20;
    private const int ProgressBatchInterval = 100;
    private static readonly TimeSpan ProgressLogInterval = TimeSpan.FromSeconds(30);
    private const int MaxConsecutiveFailedBatches = 3;
    private const int EndpointProbeTimeoutSeconds = 3;

    private const int BackfillLimit = 10000000;

    private readonly PipelineConfig _config;
    private readonly HttpClient? _httpClient;
    private readonly byte[] _aesKey;

    public EmbeddingFetcherService(PipelineConfig config, HttpClient? httpClient = null)
    {
        _config = config;
        _httpClient = httpClient;
        _aesKey = string.IsNullOrWhiteSpace(config.embeddingKey)
            ? []
            : SHA256.HashData(Utf8NoBom.Encoding.GetBytes(config.embeddingKey));
    }

    /// <summary>
    /// Generates embeddings for the given content list.
    /// </summary>
    public Task<TaskResult> FetchEmbeddingsAsync(
        Dictionary<string, ModInfo> modInfoDict,
        Dictionary<string, TranslationEntry> diffTranslationEntryDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, ModInfo> refModInfoDict,
        Dictionary<string, TranslationEntry> refTranslationEntryDict)
    {
        _ = modInfoDict;
        _ = refModInfoDict;
        // translationEntryDict: fill missing embeddings for ACCEPTED-mod entries not in work queue.
        // Prevents massive embedding backlog when new target languages are added later.
        return FetchEmbeddingsCoreAsync(diffTranslationEntryDict, refTranslationEntryDict, translationEntryDict);
    }

    private async Task<TaskResult> FetchEmbeddingsCoreAsync(
        Dictionary<string, TranslationEntry> diffTranslationEntryDict,
        Dictionary<string, TranslationEntry> refTranslationEntryDict,
        Dictionary<string, TranslationEntry>? allTranslationEntryDict = null)
    {
        var candidates = BuildCandidates(diffTranslationEntryDict.Values, isReference: false)
            .Concat(BuildCandidates(refTranslationEntryDict.Values, isReference: true))
            .ToList();

        // Also backfill embeddings for entries not in the current work queue.
        // This prevents a massive one-time embedding spike when new target languages are enabled.
        if (allTranslationEntryDict is { Count: > 0 })
        {
            var diffKeySet = new HashSet<string>(diffTranslationEntryDict.Keys, StringComparer.Ordinal);
            var backfill = BuildCandidates(
                    allTranslationEntryDict
                        .Where(kvp => !diffKeySet.Contains(kvp.Key))
                        .Select(kvp => kvp.Value),
                    isReference: false)
                .Where(c => !c.Entry.embeddingValues.TryGetValue(c.EmbeddingKey, out var ex)
                    || ex.hash != c.Hash || ex.vector.Length == 0)
                .Take(BackfillLimit)
                .ToList();
            if (backfill.Count > 0)
                Console.WriteLine($"  Embedding backfill: {backfill.Count} additional entries missing embeddings.");
            candidates.AddRange(backfill);
        }

        foreach (var candidate in candidates)
        {
            if (candidate.Entry.embeddingValues.TryGetValue(candidate.EmbeddingKey, out var existing)
                && existing.hash == candidate.Hash
                && existing.vector.Length > 0)
                continue;

            candidate.Entry.embeddingValues.Remove(candidate.EmbeddingKey);
        }

        candidates = candidates
            .Where(candidate => !candidate.Entry.embeddingValues.TryGetValue(candidate.EmbeddingKey, out var existing)
                || existing.hash != candidate.Hash
                || existing.vector.Length == 0)
            .ToList();

        if (candidates.Count == 0)
        {
            Console.WriteLine("  Embedding summary: no entries need embeddings.");
            return new TaskResult();
        }

        var embeddedCount = 0;
        var warningCount = 0;
        var failedEntries = new List<object>();

        using var ownedClient = _httpClient == null ? new HttpClient() : null;
        var client = _httpClient ?? ownedClient!;
        if (ownedClient != null)
            client.Timeout = TimeSpan.FromSeconds(Math.Max(1, _config.steamRequestTimeoutSeconds));

        var totalBatches = (candidates.Count + BatchSize - 1) / BatchSize;
        Console.WriteLine($"  Embedding queue: {candidates.Count} item(s), {totalBatches} batch(es), batchSize={BatchSize}");

        if (ownedClient != null && !await IsEmbeddingEndpointReachableAsync())
        {
            var reason = $"Embedding endpoint {_config.embeddingHost}:{_config.embeddingPort} is not reachable.";
            warningCount += candidates.Count;
            AddFailedEntries(failedEntries, candidates, reason);
            GitHubActions.Warning($"{reason} Skipping {candidates.Count} embedding request(s).", "EmbeddingFetcher");
            WriteDebugSummary(candidates, failedEntries);
            var skippedSummary = new { requestedCount = candidates.Count, embeddedCount, warningCount };
            Console.WriteLine($"  Embedding summary: requested={candidates.Count}, embedded={embeddedCount}, warnings={warningCount}");

            return new TaskResult
            {
                isSuccess = true,
                warningCount = warningCount,
                summaryJson = Utf8NoBom.SerializeJson(skippedSummary)
            };
        }

        var processedCount = 0;
        var consecutiveFailedBatches = 0;
        var lastProgressLogUtc = DateTime.MinValue;
        foreach (var (chunk, batchIndex) in candidates.Chunk(BatchSize).Select((chunk, index) => (chunk, index + 1)))
        {
            var entries = chunk.ToList();
            var texts = entries.Select(e => TruncateUtf8(e.Input)).ToList();
            try
            {
                var vectors = await FetchEmbeddingBatchAsync(client, texts);
                if (vectors == null || vectors.Count != entries.Count)
                    throw new InvalidDataException($"Embedding count mismatch: expected {entries.Count}, got {vectors?.Count ?? 0}");

                for (var i = 0; i < entries.Count; i++)
                {
                    ApplyEmbedding(entries[i], vectors[i]);
                    embeddedCount++;
                }
                processedCount += entries.Count;
                consecutiveFailedBatches = 0;
                if (ShouldLogProgress(batchIndex, totalBatches, ref lastProgressLogUtc))
                    Console.WriteLine($"  Embedding progress: batch [{batchIndex}/{totalBatches}] ok, {processedCount}/{candidates.Count} done");
            }
            catch (Exception ex)
            {
                warningCount += entries.Count;
                processedCount += entries.Count;
                consecutiveFailedBatches++;
                AddFailedEntries(failedEntries, entries, ex.Message);
                GitHubActions.Warning($"Embedding batch failed: {ex.Message}", "EmbeddingFetcher");
                Console.WriteLine($"  Embedding batch [{batchIndex}/{totalBatches}] failed: {processedCount}/{candidates.Count} done, consecutiveFailures={consecutiveFailedBatches}");

                if (consecutiveFailedBatches >= MaxConsecutiveFailedBatches)
                {
                    var remaining = candidates.Skip(processedCount).ToList();
                    if (remaining.Count > 0)
                    {
                        var reason = $"Skipped after {consecutiveFailedBatches} consecutive failed embedding batch(es).";
                        warningCount += remaining.Count;
                        processedCount += remaining.Count;
                        AddFailedEntries(failedEntries, remaining, reason);
                        GitHubActions.Warning($"Stopping embedding fetch after {consecutiveFailedBatches} consecutive failed batch(es); skipped {remaining.Count} item(s).", "EmbeddingFetcher");
                        Console.WriteLine($"  Embedding batch stop: {processedCount}/{candidates.Count} done");
                    }
                    break;
                }
            }
        }

        WriteDebugSummary(candidates, failedEntries);
        var summary = new { requestedCount = candidates.Count, embeddedCount, warningCount };
        Console.WriteLine($"  Embedding summary: requested={candidates.Count}, embedded={embeddedCount}, warnings={warningCount}");

        return new TaskResult
        {
            isSuccess = true,
            warningCount = warningCount,
            summaryJson = Utf8NoBom.SerializeJson(summary)
        };
    }

    private async Task<bool> IsEmbeddingEndpointReachableAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_config.embeddingKey))
                UdpKnock(_config.embeddingKey, _config.embeddingHost, _config.embeddingPort);

            using var tcpClient = new TcpClient();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(EndpointProbeTimeoutSeconds));
            await tcpClient.ConnectAsync(_config.embeddingHost, _config.embeddingPort, cts.Token);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Embedding endpoint probe failed: {ex.Message}");
            return false;
        }
    }

    private static void AddFailedEntries(List<object> failedEntries, IEnumerable<EmbeddingCandidate> entries, string reason)
    {
        foreach (var entry in entries)
            failedEntries.Add(new { entry.Entry.modId, entry.Entry.translationKey, entry.SourceKind, entry.TargetLang, reason });
    }

    /// <summary>Port of ref EmbeddingClient.SendBatchAsync: UDP knock → AES encrypt → POST with retry.</summary>
    private async Task<List<float[]>?> FetchEmbeddingBatchAsync(HttpClient client, List<string> texts)
    {
        var json = Utf8NoBom.SerializeJson(new { input = texts, model = Model });
        var host = _config.embeddingHost;
        var port = _config.embeddingPort;
        var apiKey = _config.embeddingKey;
        var endpoint = $"http://{host}:{port}/v1/embeddings";

        for (var attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                // 1. UDP knock (ref: UdpKnock)
                if (!string.IsNullOrWhiteSpace(apiKey))
                    UdpKnock(apiKey, host, port);

                // 2. AES-256-GCM encrypt request (ref: EncryptRequestBody)
                HttpContent content;
                if (_aesKey.Length > 0 && !string.IsNullOrWhiteSpace(apiKey))
                {
                    var bodyB64 = EncryptRequestBody(apiKey, json);
                    content = new ByteArrayContent(Utf8NoBom.Encoding.GetBytes(bodyB64));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                }
                else
                {
                    content = new StringContent(json, Utf8NoBom.Encoding, "application/json");
                }

                // 3. HTTP POST
                using var response = await client.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return ParseEmbeddingResponse(responseJson);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"  Embedding attempt {attempt} failed: {ex.Message}");
            }

            if (attempt < MaxRetries)
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        }

        return null;
    }

    // ── Ref: UdpKnock ──
    private static void UdpKnock(string apiKey, string host, int port)
    {
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var knockHex = Convert.ToHexString(
            SHA256.HashData(Utf8NoBom.Encoding.GetBytes(apiKey + ts))
        ).ToLowerInvariant();
        using var udp = new UdpClient();
        udp.Send(Utf8NoBom.Encoding.GetBytes(knockHex), host, port);
    }

    // ── Ref: EncryptRequestBody ──
    private string EncryptRequestBody(string apiKey, string jsonBody)
    {
        var plaintext = Utf8NoBom.Encoding.GetBytes(apiKey + jsonBody);
        var nonce = new byte[12];
        RandomNumberGenerator.Fill(nonce);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[16];
        using var aes = new AesGcm(_aesKey, 16);
        aes.Encrypt(nonce, plaintext, ciphertext, tag);
        var body = new byte[12 + ciphertext.Length + 16];
        Buffer.BlockCopy(nonce, 0, body, 0, 12);
        Buffer.BlockCopy(ciphertext, 0, body, 12, ciphertext.Length);
        Buffer.BlockCopy(tag, 0, body, 12 + ciphertext.Length, 16);
        return Convert.ToBase64String(body);
    }

    // ── Ref: TruncateUtf8 ──
    private static string TruncateUtf8(string text)
    {
        if (text.Length <= MaxUtf8Chars) return text;
        var pos = MaxUtf8Chars;
        while (pos > 0 && char.IsLowSurrogate(text[pos]))
            pos--;
        return text[..pos];
    }

    /// <summary>SHA-256 over mod/key/full source text identity.</summary>
    internal static string ComputeEmbeddingHash(TranslationEntry entry)
    {
        return BuildNormalCandidate(entry, "en").Hash;
    }

    private static string ComputeSha256(string text)
    {
        var hash = SHA256.HashData(Utf8NoBom.Encoding.GetBytes(text));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    // ── Kept from existing ──
    private static List<float[]> ParseEmbeddingResponse(string responseText)
    {
        using var doc = JsonDocument.Parse(responseText);
        if (!doc.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException("Embedding response missing data array.");

        var vectors = new List<float[]>();
        foreach (var item in data.EnumerateArray())
        {
            if (!item.TryGetProperty("embedding", out var embedding) || embedding.ValueKind != JsonValueKind.Array)
                throw new InvalidDataException("Embedding response item missing embedding array.");
            vectors.Add(embedding.EnumerateArray().Select(v => v.GetSingle()).ToArray());
        }
        return vectors;
    }

    private void WriteDebugSummary(List<EmbeddingCandidate> candidates, List<object> failedEntries)
    {
        if (string.IsNullOrWhiteSpace(_config.embeddingsTempDir)) return;
        Directory.CreateDirectory(_config.embeddingsTempDir);
        var debugPath = Path.Combine(_config.embeddingsTempDir, "embedding_summary.json");
        var payload = new
        {
            entries = candidates.Select(candidate => new
            {
                candidate.Entry.modId,
                candidate.Entry.translationKey,
                candidate.SourceKind,
                candidate.TargetLang,
                sourceLength = candidate.Input.Length,
                vectorDim = candidate.Entry.embeddingValues.TryGetValue(candidate.EmbeddingKey, out var existing) ? existing.vector.Length : 0
            }),
            failedEntries
        };
        Utf8NoBom.WriteAllText(debugPath, Utf8NoBom.SerializeIndentedJson(payload));
    }

    private IEnumerable<EmbeddingCandidate> BuildCandidates(IEnumerable<TranslationEntry> entries, bool isReference)
    {
        foreach (var entry in entries)
        {
            if (isReference)
            {
                // Ref mods: only embed target_lang text (ref mods have no source, only translations).
                // target_lang here = the ref mod's own translation language(s), NOT pipeline target languages.
                foreach (var (lang, data) in entry.translationValues)
                {
                    if (string.IsNullOrWhiteSpace(data.text))
                        continue;
                    yield return BuildRefCandidate(entry, lang, data.text);
                }
                continue;
            }

            // Diff mods: only embed base_lang_text, key-only fallback when base_lang missing.
            yield return BuildNormalCandidate(entry, NormalizeLanguage(_config.baseLanguage));
        }
    }

    private static EmbeddingCandidate BuildNormalCandidate(TranslationEntry entry, string baseLang)
    {
        var source = entry.GetBaseTextStrict(baseLang);
        var sourceKind = string.IsNullOrWhiteSpace(source.text)
            ? "normal_key_only"
            : "normal_base_text";
        var input = $"{entry.modId}::{entry.translationKey} = \"{source.text}\"";
        var hash = ComputeSha256($"{sourceKind}::{input}");
        return new EmbeddingCandidate(entry, sourceKind, "", input, hash);
    }

    private static EmbeddingCandidate BuildRefCandidate(TranslationEntry entry, string targetLang, string text)
    {
        targetLang = targetLang.ToLowerInvariant();
        var sourceKind = "ref_target_text";
        var input = $"{entry.modId}::{entry.translationKey} = \"{text}\"";
        var hash = ComputeSha256($"{sourceKind}::{targetLang}::{input}");
        return new EmbeddingCandidate(entry, sourceKind, targetLang, input, hash);
    }

    private static void ApplyEmbedding(EmbeddingCandidate candidate, float[] vector)
    {
        var embedding = new TranslationEmbedding
        {
            sourceKind = candidate.SourceKind,
            targetLang = candidate.TargetLang,
            hash = candidate.Hash,
            vector = vector
        };
        candidate.Entry.embeddingValues[candidate.EmbeddingKey] = embedding;
        candidate.Entry.embeddingSourceKind = candidate.SourceKind;
        candidate.Entry.embeddingTargetLang = candidate.TargetLang;
        candidate.Entry.embeddingHash = candidate.Hash;
        candidate.Entry.embeddingVector = vector;
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

    private sealed record EmbeddingCandidate(
        TranslationEntry Entry,
        string SourceKind,
        string TargetLang,
        string Input,
        string Hash)
    {
        public string EmbeddingKey => string.IsNullOrWhiteSpace(TargetLang) ? SourceKind : $"{TargetLang}::{SourceKind}";
    }

    private static bool ShouldLogProgress(int batchIndex, int totalBatches, ref DateTime lastProgressLogUtc)
    {
        if (totalBatches <= VerboseProgressBatchLimit
            || batchIndex == 1
            || batchIndex == totalBatches
            || batchIndex % ProgressBatchInterval == 0)
        {
            lastProgressLogUtc = DateTime.UtcNow;
            return true;
        }

        var nowUtc = DateTime.UtcNow;
        if (nowUtc - lastProgressLogUtc < ProgressLogInterval)
            return false;

        lastProgressLogUtc = nowUtc;
        return true;
    }
}
