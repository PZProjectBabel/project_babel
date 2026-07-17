using Common;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace RagContextRetriever;

/// <summary>
/// Retrieves reference context from embeddings for RAG-assisted translation.
/// Embedding service returns L2-normalized vectors, so dot product = cosine similarity.
/// </summary>
public class RagContextRetrieverService
{
    private readonly PipelineConfig _config;

    public RagContextRetrieverService(PipelineConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Retrieves relevant translation context from embeddings.
    /// </summary>
    public Task<TaskResult> RetrieveContextsAsync(
        Dictionary<string, TranslationEntry> refTranslationEntryDict,
        Dictionary<string, TranslationEntry> diffTranslationEntryDict,
        List<TranslationBatch> translationBatches,
        Dictionary<string, List<Dictionary<string, object?>>> ragContextByEntryKey)
    {
        return RetrieveContextsAsync(
            refTranslationEntryDict,
            diffTranslationEntryDict,
            diffTranslationEntryDict,
            translationBatches,
            ragContextByEntryKey,
            _config.priorityLanguage);
    }

    /// <summary>
    /// Retrieves relevant translation context from reference entries and all known translation entries.
    /// </summary>
    public Task<TaskResult> RetrieveContextsAsync(
        Dictionary<string, TranslationEntry> refTranslationEntryDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, TranslationEntry> diffTranslationEntryDict,
        List<TranslationBatch> translationBatches,
        Dictionary<string, List<Dictionary<string, object?>>> ragContextByEntryKey)
    {
        return RetrieveContextsAsync(
            refTranslationEntryDict,
            translationEntryDict,
            diffTranslationEntryDict,
            translationBatches,
            ragContextByEntryKey,
            _config.priorityLanguage);
    }

    public Task<TaskResult> RetrieveContextsAsync(
        Dictionary<string, TranslationEntry> refTranslationEntryDict,
        Dictionary<string, TranslationEntry> translationEntryDict,
        Dictionary<string, TranslationEntry> diffTranslationEntryDict,
        List<TranslationBatch> translationBatches,
        Dictionary<string, List<Dictionary<string, object?>>> ragContextByEntryKey,
        string targetLanguage)
    {
        _ = diffTranslationEntryDict;
        ragContextByEntryKey.Clear();

        var targetLang = NormalizeLanguage(targetLanguage);

        var referenceEntries = BuildReferences(refTranslationEntryDict.Values, translationEntryDict.Values, targetLang);
        var exactRefEntriesByKey = BuildExactReferenceLookup(refTranslationEntryDict.Values, targetLang);
        var queryEntries = translationBatches
            .SelectMany(batch => batch.translationEntries)
            .Where(entry => NeedsTargetProcessing(entry, targetLang)
                && !string.IsNullOrWhiteSpace(entry.GetBaseTextStrict(_config.baseLanguage).text))
            .ToList();
        var contextsByIndex = new List<Dictionary<string, object?>>?[queryEntries.Count];

        var queriedCount = 0;
        var contextCount = 0;
        var dimensionSkippedCount = 0;

        if (referenceEntries.Count == 0 && exactRefEntriesByKey.Count == 0)
        {
            Console.WriteLine("  RAG summary: no reference embeddings available.");
            WriteDebugContexts(ragContextByEntryKey, targetLanguage);
            return Task.FromResult(new TaskResult());
        }

        Parallel.For(
            0,
            queryEntries.Count,
            new ParallelOptions { MaxDegreeOfParallelism = ResolveMaxDegreeOfParallelism() },
            index =>
        {
            var entry = queryEntries[index];
            Interlocked.Increment(ref queriedCount);
            var entryKey = BuildEntryKey(entry);
            var queryEmbedding = GetQueryEmbedding(entry);
            var exactContexts = BuildExactReferenceContexts(exactRefEntriesByKey, entry, targetLang);
            if (queryEmbedding.Length == 0)
            {
                contextsByIndex[index] = exactContexts;
                Interlocked.Add(ref contextCount, exactContexts.Count);
                return;
            }

            var candidates = BuildTopCandidates(
                queryEmbedding,
                entryKey,
                referenceEntries,
                out var skippedDimensions);
            Interlocked.Add(ref dimensionSkippedCount, skippedDimensions);

            if (candidates.Count == 0)
            {
                contextsByIndex[index] = exactContexts;
                Interlocked.Add(ref contextCount, exactContexts.Count);
                return;
            }

            var contexts = exactContexts
                .Concat(BuildTargetContexts(candidates, targetLang))
                .DistinctBy(ctx => $"{ctx.GetValueOrDefault("mod_id")}::{ctx.GetValueOrDefault("key")}::{ctx.GetValueOrDefault("translation")}")
                .ToList();

            contextsByIndex[index] = contexts;
            Interlocked.Add(ref contextCount, contexts.Count);
        });

        for (var i = 0; i < queryEntries.Count; i++)
        {
            var entry = queryEntries[i];
            var entryKey = BuildEntryKey(entry);
            ragContextByEntryKey[entryKey] = contextsByIndex[i] ?? [];
        }

        WriteDebugContexts(ragContextByEntryKey, targetLanguage);
        var summary = new
        {
            queriedCount,
            contextCount,
            referenceCount = referenceEntries.Count,
            dimensionSkippedCount,
            ragTopK = Math.Max(0, _config.ragTopK)
        };
        Console.WriteLine($"  RAG summary: queried={queriedCount}, contexts={contextCount}, references={referenceEntries.Count}, dimSkipped={dimensionSkippedCount}, topK={Math.Max(0, _config.ragTopK)}");

        return Task.FromResult(new TaskResult
        {
            isSuccess = true,
            summaryJson = Utf8NoBom.SerializeJson(summary)
        });
    }

    private List<RagScoredCandidate> BuildTopCandidates(
        float[] queryEmbedding,
        string queryEntryKey,
        List<RagReference> referenceEntries,
        out int dimensionSkippedCount)
    {
        dimensionSkippedCount = 0;
        var queryVec = queryEmbedding;
        var dim = queryVec.Length;
        var topK = Math.Max(0, _config.ragTopK);
        var threshold = _config.ragSimilarityThreshold;
        var queue = new PriorityQueue<RagScoredCandidate, float>();

        foreach (var referenceEntry in referenceEntries)
        {
            if (string.Equals(referenceEntry.EntryKey, queryEntryKey, StringComparison.Ordinal))
                continue;

            if (referenceEntry.Vector.Length != dim)
            {
                dimensionSkippedCount++;
                continue;
            }

            if (topK == 0)
                continue;

            var score = DotProductAccelerated(queryVec, referenceEntry.Vector, dim);
            if (score < threshold)
                continue;

            var candidate = new RagScoredCandidate(referenceEntry, score);
            if (queue.Count < topK)
            {
                queue.Enqueue(candidate, score);
                continue;
            }

            if (queue.TryPeek(out _, out var minScore) && score > minScore)
            {
                queue.Dequeue();
                queue.Enqueue(candidate, score);
            }
        }

        var candidates = new List<RagScoredCandidate>(queue.Count);
        while (queue.TryDequeue(out var candidate, out _))
            candidates.Add(candidate);
        candidates.Reverse();
        return candidates;
    }

    private List<Dictionary<string, object?>> BuildTargetContexts(
        List<RagScoredCandidate> candidates,
        string targetLang)
    {
        var contexts = new List<Dictionary<string, object?>>();
        foreach (var candidate in candidates)
        {
            if (contexts.Count >= _config.ragTopK)
                break;

            var context = BuildContext(candidate.Reference.Entry, targetLang, candidate.Score);
            if (context != null)
                contexts.Add(context);
        }

        return contexts;
    }

    private Dictionary<string, object?>? BuildContext(TranslationEntry entry, string targetLang, float score)
    {
        var source = entry.GetBaseTextStrict(_config.baseLanguage);
        if (HasMissingOriginalTextMarker(source.text))
            return null;

        if (!TryGetTargetText(entry, targetLang, out var targetData))
            return null;

        return new Dictionary<string, object?>
        {
            ["mod_id"] = entry.modId,
            ["key"] = entry.translationKey,
            [source.lang] = source.text,
            ["translation"] = targetData.text,
            ["verified"] = targetData.isVerified || string.Equals(targetData.status, "verified", StringComparison.OrdinalIgnoreCase),
            ["score"] = score
        };
    }

    // AVX256 hot path with cross-platform System.Numerics/scalar fallback.
    private static float DotProductAccelerated(float[] query, float[] stored, int dim)
    {
        if (Avx.IsSupported && dim >= Vector256<float>.Count)
            return DotProductAvx256(query, stored, dim);

        return DotProductVector(query, stored, dim);
    }

    private static float DotProductAvx256(float[] query, float[] stored, int dim)
    {
        var acc = Vector256<float>.Zero;
        int i = 0;
        int simdEnd = dim - dim % Vector256<float>.Count;
        for (; i < simdEnd; i += Vector256<float>.Count)
        {
            var q = Vector256.LoadUnsafe(ref query[i]);
            var s = Vector256.LoadUnsafe(ref stored[i]);
            acc = Avx.Add(acc, Avx.Multiply(q, s));
        }

        float sum = 0;
        for (int lane = 0; lane < Vector256<float>.Count; lane++)
            sum += acc.GetElement(lane);

        for (; i < dim; i++)
            sum += query[i] * stored[i];
        return sum;
    }

    private static float DotProductVector(float[] query, float[] stored, int dim)
    {
        float sum = 0;
        int i = 0;
        if (Vector.IsHardwareAccelerated && Vector<float>.Count > 1)
        {
            var acc = Vector<float>.Zero;
            int simdWidth = Vector<float>.Count;
            int simdEnd = dim - dim % simdWidth;
            for (; i < simdEnd; i += simdWidth)
            {
                var q = new Vector<float>(query, i);
                var s = new Vector<float>(stored, i);
                acc += q * s;
            }
            sum = Vector.Dot(acc, Vector<float>.One);
        }
        for (; i < dim; i++)
            sum += query[i] * stored[i];
        return sum;
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

    private static string BuildEntryKey(TranslationEntry entry)
    {
        return $"{entry.modId}::{entry.translationKey}";
    }

    private static TranslationSourceText GetSourceText(TranslationEntry entry)
    {
        return entry.GetBaseTextStrict();
    }

    private static bool TryGetTargetText(TranslationEntry entry, string targetLang, out TranslationData targetData)
    {
        if (entry.translationValues.TryGetValue(targetLang, out targetData!)
            && !string.IsNullOrWhiteSpace(targetData.text))
        {
            return true;
        }

        targetData = new TranslationData();
        return false;
    }

    private static bool HasMissingOriginalTextMarker(string text)
    {
        return text.Contains("Original Text Missing", StringComparison.OrdinalIgnoreCase);
    }

    private int ResolveMaxDegreeOfParallelism()
    {
        if (_config.maxJobs <= 0)
            return Environment.ProcessorCount;

        return Math.Clamp(_config.maxJobs, 1, Environment.ProcessorCount);
    }

    private void WriteDebugContexts(Dictionary<string, List<Dictionary<string, object?>>> ragContextByEntryKey, string targetLanguage)
    {
        var ragContextsTempDir = string.IsNullOrWhiteSpace(_config.runTempDir)
            ? ""
            : Path.Combine(_config.runTempDir, "rag_contexts");
        if (!string.IsNullOrWhiteSpace(ragContextsTempDir) && ShouldUseTargetLanguageSubdir())
            ragContextsTempDir = Path.Combine(ragContextsTempDir, ResolveFileSafeLanguageName(targetLanguage));
        if (string.IsNullOrWhiteSpace(ragContextsTempDir))
            return;

        Directory.CreateDirectory(ragContextsTempDir);
        var path = Path.Combine(ragContextsTempDir, "rag_contexts.json");
        Utf8NoBom.WriteAllText(path, Utf8NoBom.SerializeIndentedJson(ragContextByEntryKey));
    }

    private bool ShouldUseTargetLanguageSubdir()
    {
        var baseLang = NormalizeLanguage(_config.baseLanguage);
        return _config.supportedLanguages
            .Select(lang => NormalizeLanguage(lang.isoCode))
            .Where(lang => !string.Equals(lang, baseLang, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count() > 1;
    }

    private string ResolveFileSafeLanguageName(string targetLanguage)
    {
        return NormalizeLanguage(targetLanguage);
    }

    private readonly record struct RagScoredCandidate(RagReference Reference, float Score);

    private List<RagReference> BuildReferences(IEnumerable<TranslationEntry> refEntries, IEnumerable<TranslationEntry> normalEntries, string targetLang)
    {
        var result = new List<RagReference>();
        foreach (var entry in refEntries)
        {
            if (!TryGetTargetText(entry, targetLang, out _))
                continue;

            var entryKey = BuildEntryKey(entry);
            var hasTargetEmbedding = entry.embeddingValues.Values.Any(embedding =>
                string.Equals(embedding.sourceKind, "ref_target_text", StringComparison.OrdinalIgnoreCase)
                && string.Equals(embedding.targetLang, targetLang, StringComparison.OrdinalIgnoreCase)
                && embedding.vector.Length > 0);
            if (!hasTargetEmbedding && entry.embeddingVector.Length > 0)
                result.Add(new RagReference(entryKey, entry, entry.embeddingVector));

            foreach (var embedding in entry.embeddingValues.Values)
            {
                if (!string.Equals(embedding.sourceKind, "ref_target_text", StringComparison.OrdinalIgnoreCase)
                    || !string.Equals(embedding.targetLang, targetLang, StringComparison.OrdinalIgnoreCase)
                    || embedding.vector.Length == 0)
                    continue;
                result.Add(new RagReference(entryKey, entry, embedding.vector));
            }
        }

        foreach (var entry in normalEntries)
        {
            if (!TryGetTargetText(entry, targetLang, out _))
                continue;

            var embedding = GetNormalEmbedding(entry);
            if (embedding.Length == 0)
                continue;
            result.Add(new RagReference(BuildEntryKey(entry), entry, embedding));
        }

        return result;
    }

    private static float[] GetQueryEmbedding(TranslationEntry entry)
    {
        var embedding = GetNormalEmbedding(entry);
        return embedding.Length > 0 ? embedding : entry.embeddingVector;
    }

    private static float[] GetNormalEmbedding(TranslationEntry entry)
    {
        if (entry.embeddingValues.TryGetValue("normal_base_text", out var baseEmbedding))
            return baseEmbedding.vector;
        if (entry.embeddingValues.TryGetValue("normal_key_only", out var keyEmbedding))
            return keyEmbedding.vector;
        if (string.IsNullOrWhiteSpace(entry.embeddingSourceKind)
            || string.Equals(entry.embeddingSourceKind, "normal_base_text", StringComparison.OrdinalIgnoreCase)
            || string.Equals(entry.embeddingSourceKind, "normal_key_only", StringComparison.OrdinalIgnoreCase))
        {
            return entry.embeddingVector;
        }
        return [];
    }

    private Dictionary<string, List<TranslationEntry>> BuildExactReferenceLookup(IEnumerable<TranslationEntry> refEntries, string targetLang)
    {
        var lookup = new Dictionary<string, List<TranslationEntry>>(StringComparer.Ordinal);
        foreach (var entry in refEntries)
        {
            if (!TryGetTargetText(entry, targetLang, out _))
                continue;

            if (!lookup.TryGetValue(entry.translationKey, out var entries))
            {
                entries = [];
                lookup[entry.translationKey] = entries;
            }

            entries.Add(entry);
        }

        return lookup;
    }

    private List<Dictionary<string, object?>> BuildExactReferenceContexts(
        Dictionary<string, List<TranslationEntry>> refEntriesByKey,
        TranslationEntry query,
        string targetLang)
    {
        if (!refEntriesByKey.TryGetValue(query.translationKey, out var refEntries))
            return [];

        return refEntries
            .Select(entry => BuildContext(entry, targetLang, 1.0f))
            .Where(context => context != null)
            .Select(context => context!)
            .DistinctBy(context => $"{context.GetValueOrDefault("mod_id")}::{context.GetValueOrDefault("translation")}")
            .ToList();
    }

    private static bool NeedsTargetProcessing(TranslationEntry entry, string targetLang)
    {
        if (!entry.translationValues.TryGetValue(targetLang, out var data))
            return true;
        if (data.IsProcessed)
            return false;
        if (string.IsNullOrWhiteSpace(data.text))
            return true;
        return false;
    }

    private readonly record struct RagReference(string EntryKey, TranslationEntry Entry, float[] Vector);
}
