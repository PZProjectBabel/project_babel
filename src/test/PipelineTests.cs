using Common;
using ConfigReader;
using ContentChecker;
using ContentExtractor;
using EmbeddingFetcher;
using LLMTranslator;
using ModDownloader;
using ModIdCollector;
using ModInfoFetcher;
using RagContextRetriever;
using RepoDataLoader;
using ResultWriter;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using TranslationBatcher;

namespace TranslationPipeline.Tests;

/// <summary>Provides default PipelineConfig instances and temp folder setup for tests.</summary>
public static class TestConfig
{
    /// <summary>Creates a PipelineConfig with test defaults (disabled content check, stub API keys).</summary>
    public static PipelineConfig Create() => new()
    {
        baseDir = "..",
        llmApiEndpoint = "https://test.example.com/chat/completions",
        llmModel = "test-model",
        asOneEnabled = false,
        steamApiKey = "test-steam-key",
        embeddingKey = "test-emb-key",
        llmKey = "test-llm-key",
        contentCheckEnabled = false,
        supportedLanguages =
        [
            new() { ingameCode = "EN", englishName = "English", isoCode = "en" },
            new() { ingameCode = "CN", englishName = "Chinese Simplified", isoCode = "zh-hans" }
        ]
    };

    /// <summary>Creates temp directory structure including required prompt template files.</summary>
    public static void ConfigureTempFolders(PipelineConfig config, string tempDir)
    {
        config.baseDir = tempDir;
        config.runTempDir = Path.Combine(tempDir, "run");
        config.contentCheckingPromptsTempDir = Path.Combine(config.runTempDir, "content_checking_prompts");
        config.contentCheckingResultsTempDir = Path.Combine(config.runTempDir, "content_checking_results");
        config.embeddingsTempDir = Path.Combine(config.runTempDir, "embeddings");
        config.translationBatchesTempDir = Path.Combine(config.runTempDir, "translation_batches");
        config.translationResultsTempDir = Path.Combine(config.runTempDir, "translation_results");
        config.warningsTempDir = Path.Combine(config.runTempDir, "warnings");
        Directory.CreateDirectory(config.runTempDir);
        Directory.CreateDirectory(config.warningsTempDir);
        config.dataDir = Path.Combine(tempDir, "data");
        var promptDir = Path.Combine(tempDir, "src", "prompt_templates");
        Directory.CreateDirectory(promptDir);
        Utf8NoBom.WriteAllText(Path.Combine(promptDir, "content_verification.txt"), "Return JSON.");
        Utf8NoBom.WriteAllText(Path.Combine(promptDir, "system_prompt_translate_engine.txt"), "Translate to {{TARGET_LANG}}.");
        Utf8NoBom.WriteAllText(Path.Combine(promptDir, "translation_output.md"), """
        ========CRITICAL OUTPUT RULES========
        Output must be plain text, one translation per line.

        ========EXPECTED OUTPUT EXAMPLE========
        T1	Target translation 1	0.99
        T2	Target translation 2	0.95
        T3	Target translation 3	-1.00	Target-language conflict note
        """);
    }
}

/// <summary>Convenience factory for creating TranslationEntry instances in tests.</summary>
public static class TestTranslations
{
    /// <summary>Creates a single TranslationEntry with the given key and text.</summary>
    public static TranslationEntry Entry(string key, string text, string iso = "en") => new()
    {
        modId = "1",
        masterKey = "UI_EN",
        translationKey = key,
        baseLang = "en",
        translationValues = new Dictionary<string, TranslationData>(StringComparer.OrdinalIgnoreCase)
        {
            [iso] = new() { text = text }
        }
    };

    /// <summary>Creates a dictionary of translation entries from text values, keyed as "1::Key_0", "1::Key_1", etc.</summary>
    public static Dictionary<string, TranslationEntry> Entries(params string[] texts)
    {
        return texts
            .Select((text, index) => Entry($"Key_{index}", text))
            .ToDictionary(entry => $"{entry.modId}::{entry.translationKey}", entry => entry);
    }
}

/// <summary>Stub HTTP handler that returns a fixed response for testing.</summary>
public sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly string _response;
    private readonly HttpStatusCode _statusCode;
    /// <summary>The last HTTP request received.</summary>
    public HttpRequestMessage? LastRequest { get; private set; }
    /// <summary>The body of the last HTTP request.</summary>
    public string LastRequestBody { get; private set; } = "";
    /// <summary>How many requests have been sent.</summary>
    public int RequestCount { get; private set; }

    public StubHttpMessageHandler(string response, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _response = response;
        _statusCode = statusCode;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        LastRequestBody = request.Content == null
            ? ""
            : await request.Content.ReadAsStringAsync(cancellationToken);
        RequestCount++;
        return new HttpResponseMessage(_statusCode)
        {
            Content = new StringContent(_response, Utf8NoBom.Encoding, "application/json")
        };
    }
}

/// <summary>HTTP handler that simulates timeouts for testing fallback behavior.</summary>
public sealed class TimeoutHttpMessageHandler : HttpMessageHandler
{
    /// <summary>How many requests have been attempted.</summary>
    public int RequestCount { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        RequestCount++;
        throw new TaskCanceledException("simulated timeout");
    }
}

/// <summary>Verifies Utf8NoBom JSON serialization preserves Unicode and omits BOM.</summary>
public class Utf8NoBomTests
{
    [Fact]
    /// <summary>JSON output must contain raw CJK characters, not \\u escapes, and no BOM header.</summary>
    public void SerializeAndWrite_ShouldPreserveUnicodeAndOmitBom()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".json");

        try
        {
            var json = Utf8NoBom.SerializeIndentedJson(new { text = "你好" });
            Utf8NoBom.WriteAllText(tempFile, json);
            var bytes = File.ReadAllBytes(tempFile);

            Assert.Contains("你好", json);
            Assert.DoesNotContain("\\u4f60", json, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("\\u597d", json, StringComparison.OrdinalIgnoreCase);
            Assert.False(bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }
}

/// <summary>
/// Verifies BinaryEmbeddingSerializer's .bin format is byte-stable across Windows/Linux:
/// explicit little-endian ints/halves, raw UTF-8 keys, raw SHA-256 bytes, zstd container.
/// </summary>
public class BinaryEmbeddingSerializerTests
{
    private static float[] Vector(params float[] first)
    {
        var vec = new float[BinaryEmbeddingSerializer.EMBEDDING_DIM];
        for (int i = 0; i < first.Length && i < vec.Length; i++)
            vec[i] = first[i];
        return vec;
    }

    private static byte[] HashBytes(int seed)
    {
        var hash = new byte[BinaryEmbeddingSerializer.HASH_RAW_BYTES];
        for (int i = 0; i < hash.Length; i++)
            hash[i] = (byte)((seed + i) & 0xFF);
        return hash;
    }

    private static void AssertVectorsClose(float[] expected, float[] actual)
    {
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
            Assert.True(MathF.Abs(expected[i] - actual[i]) <= 2e-3f,
                $"Vector mismatch at index {i}: {expected[i]} vs {actual[i]}");
    }

    [Fact]
    public void RoundTrip_PreservesKeysHashAndVector()
    {
        var rand = new Random(42);
        var records = new List<BinaryEmbeddingSerializer.Record>();
        for (int i = 0; i < 50; i++)
        {
            var vec = new float[BinaryEmbeddingSerializer.EMBEDDING_DIM];
            for (int j = 0; j < vec.Length; j++)
                vec[j] = (float)(rand.NextDouble() * 2 - 1);
            records.Add(new BinaryEmbeddingSerializer.Record(
                i % 3 == 0 ? "翻译_Key_" + i : "Key_" + i,
                i % 2 == 0 ? "ref_target_text" : "normal_base_text",
                i % 3 == 0 ? "zh-hans" : (i % 3 == 1 ? "ja" : ""),
                HashBytes(i),
                vec));
        }

        var parsed = BinaryEmbeddingSerializer.Deserialize(BinaryEmbeddingSerializer.Serialize(records));

        Assert.Equal(records.Count, parsed.Count);
        for (int i = 0; i < records.Count; i++)
        {
            Assert.Equal(records[i].TranslationKey, parsed[i].TranslationKey);
            Assert.Equal(records[i].SourceKind, parsed[i].SourceKind);
            Assert.Equal(records[i].TargetLang, parsed[i].TargetLang);
            Assert.Equal(records[i].Hash, parsed[i].Hash);
            AssertVectorsClose(records[i].Vector, parsed[i].Vector);
        }
    }

    [Fact]
    public void ByteLayout_IsLittleEndianUtf8AndFp16()
    {
        var records = new List<BinaryEmbeddingSerializer.Record>
        {
            new("Key_0", "normal_base_text", "", HashBytes(1), Vector(1.0f, -1.0f))
        };

        var raw = BinaryEmbeddingSerializer.Serialize(records);
        var expectedKey = "Key_0|normal_base_text|";
        var expectedKeyLen = System.Text.Encoding.UTF8.GetByteCount(expectedKey);

        // int32 keyLen (little-endian) + UTF-8 key + 32 raw hash bytes + 768 fp16 bytes.
        Assert.Equal(4 + expectedKeyLen + BinaryEmbeddingSerializer.HASH_RAW_BYTES + BinaryEmbeddingSerializer.FP16_VEC_BYTES, raw.Length);
        Assert.Equal(expectedKeyLen, raw[0] | (raw[1] << 8) | (raw[2] << 16) | (raw[3] << 24));
        Assert.Equal(expectedKey, System.Text.Encoding.UTF8.GetString(raw, 4, expectedKeyLen));

        int hashOffset = 4 + expectedKeyLen;
        for (int i = 0; i < BinaryEmbeddingSerializer.HASH_RAW_BYTES; i++)
            Assert.Equal(HashBytes(1)[i], raw[hashOffset + i]);

        int vecOffset = hashOffset + BinaryEmbeddingSerializer.HASH_RAW_BYTES;
        // fp16 little-endian: 1.0f -> 0x3C00 -> [0x00, 0x3C], -1.0f -> 0xBC00 -> [0x00, 0xBC].
        Assert.Equal(0x00, raw[vecOffset]);
        Assert.Equal(0x3C, raw[vecOffset + 1]);
        Assert.Equal(0x00, raw[vecOffset + 2]);
        Assert.Equal(0xBC, raw[vecOffset + 3]);

        // Deserializing the exact raw bytes must recover the original record.
        var parsed = BinaryEmbeddingSerializer.Deserialize(raw);
        Assert.Single(parsed);
        Assert.Equal("Key_0", parsed[0].TranslationKey);
        Assert.Equal("normal_base_text", parsed[0].SourceKind);
        Assert.Equal("", parsed[0].TargetLang);
        Assert.Equal(HashBytes(1), parsed[0].Hash);
        AssertVectorsClose(records[0].Vector, parsed[0].Vector);
    }

    [Fact]
    public void CompressedFileRoundTrip_MatchesRawBytes()
    {
        var records = new List<BinaryEmbeddingSerializer.Record>
        {
            new("Key_A", "ref_target_text", "zh-hans", HashBytes(7), Vector(0.5f)),
            new("Key_B", "normal_base_text", "", HashBytes(9), Vector(-0.25f, 0.125f))
        };

        var tempDir = Path.Combine(Path.GetTempPath(), "babel_ser_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var filePath = Path.Combine(tempDir, "test.bin");
            BinaryEmbeddingSerializer.WriteCompressed(filePath, records);

            var loaded = BinaryEmbeddingSerializer.ReadCompressed(filePath, Path.Combine(tempDir, "decomp"));

            Assert.Equal(records.Count, loaded.Count);
            for (int i = 0; i < records.Count; i++)
            {
                Assert.Equal(records[i].TranslationKey, loaded[i].TranslationKey);
                Assert.Equal(records[i].SourceKind, loaded[i].SourceKind);
                Assert.Equal(records[i].TargetLang, loaded[i].TargetLang);
                Assert.Equal(records[i].Hash, loaded[i].Hash);
                AssertVectorsClose(records[i].Vector, loaded[i].Vector);
            }
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ReadsCommittedEmbeddingBinFiles()
    {
        // Walk up from the test output dir to the repo root, then parse committed .bin files
        // that may have been written on Windows or Linux.
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && (!Directory.Exists(Path.Combine(dir.FullName, "data", "embeddings"))
            || !Directory.Exists(Path.Combine(dir.FullName, "translation_ref", "embeddings"))))
        {
            dir = dir.Parent;
        }
        if (dir == null)
            return; // repo files not present, skip.

        var tempDir = Path.Combine(Path.GetTempPath(), "babel_ser_read_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            foreach (var embDir in new[] { "data/embeddings", "translation_ref/embeddings" })
            {
                var fullDir = Path.Combine(dir.FullName, embDir);
                var files = Directory.GetFiles(fullDir, "*.bin");
                Assert.NotEmpty(files);
                foreach (var file in files)
                {
                    var records = BinaryEmbeddingSerializer.ReadCompressed(file, tempDir);
                    Assert.NotEmpty(records);
                    foreach (var rec in records)
                    {
                        Assert.False(string.IsNullOrEmpty(rec.TranslationKey));
                        Assert.Equal(BinaryEmbeddingSerializer.HASH_RAW_BYTES, rec.Hash.Length);
                        Assert.Equal(BinaryEmbeddingSerializer.EMBEDDING_DIM, rec.Vector.Length);
                    }
                }
            }
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }
}
/// <summary>Tests ConfigReaderService parsing of config.json, secrets, and language files.</summary>
public class ConfigReaderTests
{
    [Fact]
    /// <summary>Validates content check settings, concurrency, and reference mod parsing.</summary>
    public void LoadConfig_ShouldReadContentCheckAndReferenceMods()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var configDir = Path.Combine(tempDir, "config");
        Directory.CreateDirectory(configDir);
        Directory.CreateDirectory(Path.Combine(tempDir, "data"));

        try
        {
            Utf8NoBom.WriteAllText(Path.Combine(configDir, "config.json"), """
            {
              "Settings": { "priority_language": "CN", "base_language": "EN" },
              "LLM": {
                "api_endpoint": "https://test.example.com/chat/completions",
                "model": "test-model",
                "reasoning_effort": "low",
                "temperature": 0.1,
                "max_tokens": 1000,
                "batch_size": 10,
                "batch_token_budget": 100,
                "concurrency": {
                  "initial": 3,
                  "maximum": 9,
                  "minimum": 2,
                  "max_retries": 4,
                  "failure_streak_to_decrease": 2,
                  "retry_base_delay_ms": 7,
                  "retry_max_delay_ms": 70
                }
              },
              "RAG": { "similarity_threshold": 0.5, "top_k": 3, "index_dir": "data/rag_index" },
              "AsOne": { "enabled": false, "base_url": "https://www.asone.fun/", "public_mod_list_path": "api/Home/GetAllModinfo" },
              "Steam": { "api_chunk_size": 20, "request_timeout_seconds": 10, "max_retries": 1 },
              "Pipeline": { "batch_size": 5 },
              "ContentCheck": { "enabled": false, "check_interval_days": 42 }
            }
            """);
            Utf8NoBom.WriteAllText(Path.Combine(configDir, "secrets.json"), """
            {
              "STEAM_KEY": "steam",
              "EMBEDDING_KEY": "embedding",
              "LLM_KEY": "llm"
            }
            """);
            Utf8NoBom.WriteAllText(Path.Combine(configDir, "supported_languages.json"), """
            [
              { "ingame_code": "EN", "chinese_name": "英语", "english_name": "English", "native_name": "English", "iso_code": "en" },
              { "ingame_code": "CN", "chinese_name": "简体中文", "english_name": "Chinese Simplified", "native_name": "简体中文", "iso_code": "zh-hans" }
            ]
            """);
            Utf8NoBom.WriteAllText(Path.Combine(configDir, "ref_translation_mods.json"), """
            [
              {
                "mod_id": "3556544454",
                "mod_name": "Ref Mod",
                "language": "zh-hans",
                "mod_update_time": "1700000000",
                "last_check_time": "2026-06-19T07:00:03Z"
              }
            ]
            """);

            var config = new ConfigReaderService().LoadConfig(tempDir);

            Assert.False(config.contentCheckEnabled);
            Assert.Equal(42, config.contentCheckIntervalDays);
            Assert.Equal("low", config.llmReasoningEffort);
            Assert.Equal(3, config.llmConcurrencyInitial);
            Assert.Equal(9, config.llmConcurrencyMaximum);
            Assert.Equal(2, config.llmConcurrencyMinimum);
            Assert.Equal(4, config.llmConcurrencyMaxRetries);
            Assert.Equal(2, config.llmConcurrencyFailureStreakToDecrease);
            Assert.Equal(7, config.llmConcurrencyRetryBaseDelayMs);
            Assert.Equal(70, config.llmConcurrencyRetryMaxDelayMs);
            var refMod = Assert.Single(config.referenceTranslationMods);
            Assert.Equal("3556544454", refMod.modId);
            Assert.Equal("Ref Mod", refMod.modName);
            Assert.Equal("zh-hans", refMod.language);
            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1700000000).UtcDateTime, refMod.timeModUpdated);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}

/// <summary>Tests LLM concurrency profile resolution for GitHub Actions, DeepSeek, and unknown environments.</summary>
public class LlmConcurrencySettingsTests
{
    [Fact]
    /// <summary>Verifies correct concurrency profiles for GitHub Actions, DeepSeek v4-flash, v4-pro, and fallback.</summary>
    public void ResolveConcurrencySettings_ShouldUseExpectedProfiles()
    {
        var config = TestConfig.Create();
        config.llmApiEndpoint = "https://api.deepseek.com/chat/completions";
        config.llmModel = "deepseek-v4-flash";

        var github = LLMTranslatorService.ResolveConcurrencySettings(config, name => name switch
        {
            "GITHUB_ACTIONS" => "true",
            "RUNNER_OS" => "Linux",
            _ => null
        });
        Assert.Equal(4, github.Initial);
        Assert.Equal(32, github.Maximum);
        Assert.Equal("github-actions", github.Profile);

        var flash = LLMTranslatorService.ResolveConcurrencySettings(config, _ => null);
        Assert.Equal(128, flash.Initial);
        Assert.Equal(2000, flash.Maximum);
        Assert.Equal("deepseek-v4-flash", flash.Profile);

        config.llmModel = "deepseek-v4-pro";
        var pro = LLMTranslatorService.ResolveConcurrencySettings(config, _ => null);
        Assert.Equal(64, pro.Initial);
        Assert.Equal(400, pro.Maximum);
        Assert.Equal("deepseek-v4-pro", pro.Profile);

        config.llmApiEndpoint = "https://test.example.com/chat/completions";
        config.llmModel = "custom-model";
        var unknown = LLMTranslatorService.ResolveConcurrencySettings(config, _ => null);
        Assert.Equal(16, unknown.Initial);
        Assert.Equal(128, unknown.Maximum);
        Assert.Equal("unknown", unknown.Profile);
    }
}

/// <summary>Tests mod ID collection from local files and remote AsOne with timeout handling.</summary>
public class ModIdCollectorTests
{
    [Fact]
    /// <summary>Verifies local request_for_translation.txt IDs are loaded into the mod info dict.</summary>
    public async Task CollectModIds_ShouldFillInputDictionary()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(tempDir, "config"));
        Directory.CreateDirectory(Path.Combine(tempDir, "data"));
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(tempDir, "config", "request_for_translation.txt"), "1234567890");

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            var service = new ModIdCollectorService(config);
            var modInfoDict = new Dictionary<string, ModInfo>();

            var result = await service.CollectModIdsAsync(modInfoDict);

            Assert.True(result.isSuccess);
            Assert.Contains("1234567890", modInfoDict.Keys);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    /// <summary>Verifies AsOne fallback after three consecutive timeouts, while local IDs are still collected.</summary>
    public async Task CollectModIds_ShouldSkipAsOneAfterThreeTimeouts()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(tempDir, "config"));
        Directory.CreateDirectory(Path.Combine(tempDir, "data"));
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(tempDir, "config", "request_for_translation.txt"), "1234567890");

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            config.asOneEnabled = true;

            var handler = new TimeoutHttpMessageHandler();
            using var httpClient = new HttpClient(handler);
            var service = new ModIdCollectorService(config, httpClient);
            var modInfoDict = new Dictionary<string, ModInfo>();

            var result = await service.CollectModIdsAsync(modInfoDict);

            Assert.True(result.isSuccess);
            Assert.Equal(4, handler.RequestCount);
            Assert.Contains("1234567890", modInfoDict.Keys);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}

/// <summary>Tests translation state caching: hashing, load/save, metadata round-tripping.</summary>
public class TranslationStateCacheTests
{
    [Fact]
    /// <summary>Source hash should be stable and ignore non-base-language text changes.</summary>
    public void ComputeSourceHash_ShouldIgnoreFallbackLanguageText()
    {
        var entry = TestTranslations.Entry("Key_Ko_Only", "旧文本", "ko");

        var firstHash = RepoDataLoaderService.ComputeSourceHash(entry, "en");
        entry.translationValues["ko"].text = "新文本";
        var secondHash = RepoDataLoaderService.ComputeSourceHash(entry, "en");

        Assert.Equal(firstHash, secondHash);
    }

    [Fact]
    /// <summary>Translation line parser should correctly read process and verify statuses.</summary>
    public void LoadTranslationCache_ShouldReadProcessAndVerifyStatus()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            var langDir = Path.Combine(config.dataDir, "translations", "zh-hans");
            Directory.CreateDirectory(langDir);
            Utf8NoBom.WriteAllText(Path.Combine(langDir, "1.txt"), """
            Key_0::en = "Hello",
            Key_0::zh-hans::processed::verified = "你好",
            """);

            var entries = new Dictionary<string, TranslationEntry>();
            new RepoDataLoaderService(config).LoadTranslationCache(entries);

            var data = entries["1::Key_0"].translationValues["zh-hans"];
            Assert.Equal("processed", data.processStatus);
            Assert.True(data.isVerified);
            Assert.Equal("verified", data.status);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    /// <summary>Result writer should output unprocessed translation text without confidence.</summary>
    public async Task WriteResults_ShouldWriteUnprocessedTextWithoutConfidence()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(config.dataDir);
            var entry = TestTranslations.Entry("Key_0", "Hello");
            entry.translationValues["zh-hans"] = new()
            {
                text = "你好",
                processStatus = "unprocessed",
                status = "unverified"
            };
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };

            Assert.True((await new ResultWriterService(config).WriteResultsAsync([], [], entries, "zh-hans")).isSuccess);

            var text = Utf8NoBom.ReadAllText(Path.Combine(config.dataDir, "translations", "zh-hans", "1.txt"));
            Assert.Contains("Key_0::zh-hans::unprocessed::unverified = \"你好\",", text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    /// <summary>Result writer should omit the target line entirely for missing translations.</summary>
    public async Task WriteResults_ShouldWriteEmptyTargetForMissingTranslation()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(config.dataDir);
            config.supportedLanguages.Add(new() { ingameCode = "AR", englishName = "Arabic", isoCode = "ar" });

            var entry = TestTranslations.Entry("Key_0", "Hello");
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };

            Assert.True((await new ResultWriterService(config).WriteResultsAsync([], [], entries, "ar")).isSuccess);

            var text = Utf8NoBom.ReadAllText(Path.Combine(config.dataDir, "translations", "ar", "1.txt"));
            Assert.Contains("Key_0::en = \"Hello\",", text);
            Assert.DoesNotContain("Key_0::ar::unprocessed::unverified", text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    /// <summary>Result writer should emit base-language-only output when target equals base.</summary>
    public async Task WriteResults_ShouldWriteBaseLanguageOutput()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(config.dataDir);

            var entry = TestTranslations.Entry("Key_0", "Hello");
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };

            Assert.True((await new ResultWriterService(config).WriteResultsAsync([], [], entries, "en")).isSuccess);

            var text = Utf8NoBom.ReadAllText(Path.Combine(config.dataDir, "translations", "en", "1.txt"));
            Assert.Contains("Key_0::en = \"Hello\",", text);
            Assert.DoesNotContain("Key_0::en::", text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    /// <summary>Entry metadata should be written per-mod to the entry_metadata directory.</summary>
    public async Task WriteData_ShouldWriteEntryMetadataPerMod()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(config.dataDir);

            var entry = TestTranslations.Entry("Key_0", "Hello");
            entry.sourceHash = "normal-hash";
            var refEntry = TestTranslations.Entry("Ref_Key", "你好", "zh-hans");
            refEntry.modId = "ref";
            refEntry.sourceHash = "ref-hash";

            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };
            var refEntries = new Dictionary<string, TranslationEntry> { ["ref::Ref_Key"] = refEntry };

            Assert.True((await new ResultWriterService(config).WriteDataAsync([], entries, [], refEntries)).isSuccess);

            var dataMetadataPath = Path.Combine(config.dataDir, "entry_metadata", "1.json");
            var refMetadataPath = Path.Combine(tempDir, "translation_ref", "entry_metadata", "ref.json");
            Assert.True(File.Exists(dataMetadataPath));
            Assert.True(File.Exists(refMetadataPath));
            Assert.False(File.Exists(Path.Combine(config.dataDir, "entry_metadata.json")));

            using var doc = JsonDocument.Parse(Utf8NoBom.ReadAllText(dataMetadataPath));
            var row = doc.RootElement.EnumerateArray().Single();
            Assert.Equal("1", row.GetProperty("mod_id").GetString());
            Assert.Equal("Key_0", row.GetProperty("translation_key").GetString());
            Assert.Equal("normal-hash", row.GetProperty("source_hash").GetString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void LoadEntryMetadataCache_ShouldIgnoreLegacySingleFile()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(config.dataDir);
            var metadataDir = Path.Combine(config.dataDir, "entry_metadata");
            Directory.CreateDirectory(metadataDir);

            Utf8NoBom.WriteAllText(Path.Combine(config.dataDir, "entry_metadata.json"), """
            [
              { "mod_id": "1", "translation_key": "Key_0", "is_active": false, "source_hash": "legacy" }
            ]
            """);
            var entry = TestTranslations.Entry("Key_0", "Hello");
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };

            new RepoDataLoaderService(config).LoadEntryMetadataCache(entries);

            Assert.True(entry.isActive);
            Assert.Equal("", entry.sourceHash);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}

/// <summary>Tests Steam Workshop mod info fetching with mock HTTP responses.</summary>
public class ModInfoFetcherTests
{
    [Fact]
    /// <summary>ModInfoFetcher should correctly update the input dictionary with fetched metadata.</summary>
    public async Task FetchModInfos_ShouldUpdateInputDictionary()
    {
        const string steamResponse = """
        {
          "response": {
            "publishedfiledetails": [
              {
                "publishedfileid": "1234567890",
                "result": 1,
                "title": "First Mod",
                "creator": "creator-a",
                "time_created": 1700000000,
                "time_updated": 1700000100,
                "subscriptions": 12,
                "favorited": 3,
                "description": "A test mod",
                "consumer_app_id": 108600
              }
            ]
          }
        }
        """;
        var handler = new StubHttpMessageHandler(steamResponse);
        using var httpClient = new HttpClient(handler);
        var service = new ModInfoFetcherService(TestConfig.Create(), httpClient);
        var modInfoDict = new Dictionary<string, ModInfo>
        {
            ["1234567890"] = new() { modId = "1234567890" },
        };

        var result = await service.FetchModInfosAsync(modInfoDict);

        Assert.True(result.isSuccess);
        Assert.Equal("First Mod", modInfoDict["1234567890"].modName);
        Assert.Equal("creator-a", modInfoDict["1234567890"].creator);
        Assert.Equal(HttpMethod.Post, handler.LastRequest?.Method);
    }

    [Fact]
    public async Task FetchModInfos_ShouldPreserveQueuedUpdateWhenMetadataIsUnchanged()
    {
        const string steamResponse = """
        {
          "response": {
            "publishedfiledetails": [
              {
                "publishedfileid": "1234567890",
                "result": 1,
                "title": "First Mod",
                "time_updated": 1700000100,
                "consumer_app_id": 108600
              }
            ]
          }
        }
        """;
        var handler = new StubHttpMessageHandler(steamResponse);
        using var httpClient = new HttpClient(handler);
        var service = new ModInfoFetcherService(TestConfig.Create(), httpClient);
        var modInfoDict = new Dictionary<string, ModInfo>
        {
            ["1234567890"] = new()
            {
                modId = "1234567890",
                timeModUpdated = DateTimeOffset.FromUnixTimeSeconds(1700000100).UtcDateTime,
                needsUpdate = true
            }
        };

        var result = await service.FetchModInfosAsync(modInfoDict);

        Assert.True(result.isSuccess);
        Assert.True(modInfoDict["1234567890"].needsUpdate);
    }

    [Fact]
    public async Task FetchModInfos_ShouldStopAfterFiveConsecutiveFailures()
    {
        var config = TestConfig.Create();
        config.steamApiChunkSize = 1;
        var handler = new StubHttpMessageHandler("{}", HttpStatusCode.MethodNotAllowed);
        using var httpClient = new HttpClient(handler);
        var service = new ModInfoFetcherService(config, httpClient);
        var modInfoDict = Enumerable.Range(1, 7)
            .ToDictionary(i => i.ToString(), i => new ModInfo { modId = i.ToString() });

        var result = await service.FetchModInfosAsync(modInfoDict);

        Assert.False(result.isSuccess);
        Assert.Equal(7, modInfoDict.Count);
        Assert.Equal(5, handler.RequestCount);
    }
}

/// <summary>Tests steamcmd download progress event parsing from raw output text.</summary>
public class ModDownloaderTests
{
    [Fact]
    /// <summary>Downloading should update localDownloadedPath on ModInfo after completion.</summary>
    public async Task DownloadMods_ShouldWriteLocalPathToModInfo()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            config.downloadedModsTempDir = Path.Combine(tempDir, "downloaded");
            var service = new ModDownloaderService(config);
            var infos = new Dictionary<string, ModInfo>
            {
                ["1234567890"] = new() { modId = "1234567890", modName = "Test" }
            };

            var result = await service.DownloadModsAsync(["1234567890"], infos, tempDir);

            Assert.False(result.isSuccess);
            Assert.Equal(Path.Combine(config.downloadedModsTempDir, "1234567890"), infos["1234567890"].localDownloadedPath);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ParseDownloadResults_ShouldReturnExpectedSuccessfulIds()
    {
        const string output = """
        Downloading item 1234567890 ...
        Success. Downloaded item 1234567890 to "x" (2048 bytes)
        Success. Downloaded item 9999999999 to "x" (1 bytes)
        """;

        var succeeded = ModDownloaderService.ParseDownloadResults(output, ["1234567890", "2222222222"]);

        Assert.Contains("1234567890", succeeded);
        Assert.DoesNotContain("9999999999", succeeded);
        Assert.DoesNotContain("2222222222", succeeded);
    }

    [Fact]
    public void ParseDownloadPaths_ShouldReturnReportedPathForExpectedId()
    {
        const string output = "Success. Downloaded item 1234567890 to \"/home/runner/.steam/steam/steamapps/workshop/content/108600/1234567890\" (2048 bytes)";

        var paths = ModDownloaderService.ParseDownloadPaths(output, ["1234567890", "2222222222"]);

        Assert.Equal("/home/runner/.steam/steam/steamapps/workshop/content/108600/1234567890", paths["1234567890"]);
        Assert.DoesNotContain("2222222222", paths);
    }

    [Fact]
    public void ParseDownloadProgressEvents_ShouldReadSteamCmdLogLines()
    {
        const string log = """
        [2026-06-16 21:12:17] Downloading item 2286124931 ...
        [2026-06-16 21:12:27] Success. Downloaded item 2286124931 to "x" (2018670 bytes)
        [2026-06-16 21:12:28] [AppID 108600] Starting Workshop download job (requested item 2335368829 )
        """;

        var events = ModDownloaderService.ParseDownloadProgressEvents(log);

        Assert.Equal(3, events.Count);
        Assert.Equal(new SteamCmdDownloadProgress("2286124931", false, 0), events[0]);
        Assert.Equal(new SteamCmdDownloadProgress("2286124931", true, 2018670), events[1]);
        Assert.Equal(new SteamCmdDownloadProgress("2335368829", false, 0), events[2]);
    }

    [Fact]
    public void TryParseSteamCmdUpdateProgress_ShouldReadLocalizedUpdateLines()
    {
        Assert.True(ModDownloaderService.TryParseSteamCmdUpdateProgress(
            "[ 19%] 正在下载更新 (已下载 3,721，共 19,002 KB)...",
            out var chineseProgress));
        Assert.Equal(19, chineseProgress.Percent);
        Assert.Equal("downloading update", chineseProgress.Phase);
        Assert.Equal(3721, chineseProgress.DownloadedKilobytes);
        Assert.Equal(19002, chineseProgress.TotalKilobytes);

        Assert.True(ModDownloaderService.TryParseSteamCmdUpdateProgress(
            "[----] Verifying installation...",
            out var englishProgress));
        Assert.Null(englishProgress.Percent);
        Assert.Equal("verifying installation", englishProgress.Phase);

        Assert.False(ModDownloaderService.TryParseSteamCmdUpdateProgress(
            "[----] !!! Fatal Error: Steamcmd needs to be online to update.",
            out _));
    }
}

/// <summary>Dynamic HTTP handler: delegates response generation to a user-provided function; tracks concurrency.</summary>
public sealed class DynamicHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<string, int, Task<(HttpStatusCode statusCode, string responseBody)>> _responder;
    private int _requestCount;
    private int _inFlight;
    private int _maxInFlight;

    public DynamicHttpMessageHandler(Func<string, int, Task<(HttpStatusCode statusCode, string responseBody)>> responder)
    {
        _responder = responder;
    }

    /// <summary>Total requests sent.</summary>
    public int RequestCount => _requestCount;
    /// <summary>Maximum concurrent in-flight requests observed.</summary>
    public int MaxInFlight => _maxInFlight;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content == null
            ? ""
            : await request.Content.ReadAsStringAsync(cancellationToken);
        var requestNumber = Interlocked.Increment(ref _requestCount);
        var inFlight = Interlocked.Increment(ref _inFlight);
        UpdateMaxInFlight(inFlight);
        try
        {
            var (statusCode, responseBody) = await _responder(body, requestNumber);
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(responseBody, Utf8NoBom.Encoding, "application/json")
            };
        }
        finally
        {
            Interlocked.Decrement(ref _inFlight);
        }
    }

    /// <summary>CAS-updates max-in-flight when the new value exceeds the current maximum.</summary>
    private void UpdateMaxInFlight(int value)
    {
        while (true)
        {
            var current = _maxInFlight;
            if (value <= current)
                return;
            if (Interlocked.CompareExchange(ref _maxInFlight, value, current) == current)
                return;
        }
    }
}

/// <summary>Tests JSON and TXT content extraction from mod translation files.</summary>
public class ContentExtractorTests
{
    [Fact]
    /// <summary>Extraction should correctly fill the output dictionary with parsed entries.</summary>
    public async Task ExtractContents_ShouldFillOutputDictionary()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var enDir = Path.Combine(tempDir, "media", "lua", "shared", "Translate", "EN");
        var cnDir = Path.Combine(tempDir, "media", "lua", "shared", "Translate", "CN");
        var warningDir = Path.Combine(tempDir, "warnings");
        var outDir = Path.Combine(tempDir, "out");
        var runDir = Path.Combine(tempDir, "run");
        Directory.CreateDirectory(enDir);
        Directory.CreateDirectory(cnDir);
        Directory.CreateDirectory(warningDir);
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(enDir, "IG_UI_EN.json"), """{"IGUI_Test":"Hello","Nested":{"Nested_Key":"World"}}""");
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(cnDir, "IG_UI_CN.json"), """{"IGUI_Test":"你好"}""");
        var brokenPath = Path.Combine(enDir, "Broken_EN.json");
        await Utf8NoBom.WriteAllTextAsync(brokenPath, """{"Broken_Key": """);

        var config = TestConfig.Create();
        config.warningsTempDir = warningDir;
        config.extractedContentsTempDir = outDir;
        config.runTempDir = runDir;
        var service = new ContentExtractorService(config);
        var modInfoDict = new Dictionary<string, ModInfo>
        {
            ["1"] = new() { modId = "1", modName = "Test", localDownloadedPath = tempDir }
        };
        var translationEntryDict = new Dictionary<string, TranslationEntry>();

        try
        {
            var result = await service.ExtractContentsAsync(modInfoDict, translationEntryDict, "testbatch");

            Assert.True(result.isSuccess);
            Assert.Equal(2, translationEntryDict.Count);
            var igui = translationEntryDict["1::IGUI_Test"];
            Assert.Equal("Hello", igui.translationValues["en"].text);
            Assert.Equal("你好", igui.translationValues["zh-hans"].text);
            Assert.Equal("World", translationEntryDict["1::Nested_Key"].translationValues["en"].text);
            Assert.Contains("IGUI_Test::en = \"Hello\"", await Utf8NoBom.ReadAllTextAsync(Path.Combine(outDir, "en", "1.txt")));

            var warningFile = Assert.Single(Directory.GetFiles(warningDir, "ContentExtractor_testbatch_*.json"));
            using var doc = JsonDocument.Parse(await Utf8NoBom.ReadAllTextAsync(warningFile));
            var root = doc.RootElement;
            Assert.Equal("ContentExtractor", root.GetProperty("ModuleName").GetString());
            Assert.Equal("testbatch", root.GetProperty("BatchId").GetString());
            Assert.Equal("1", root.GetProperty("ModId").GetString());
            Assert.Equal("Test", root.GetProperty("ModName").GetString());
            Assert.Equal(brokenPath, root.GetProperty("FilePath").GetString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task ExtractContents_ShouldMergeTxtJsonAndKeepDiagnostics()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var txt42Dir = Path.Combine(tempDir, "mods", "SubMod", "42", "media", "lua", "shared", "translate", "en");
        var txt4213Dir = Path.Combine(tempDir, "mods", "SubMod", "42.13", "media", "lua", "shared", "translate", "en");
        var json4213Dir = Path.Combine(tempDir, "mods", "SubMod", "42.13", "media", "lua", "shared", "translate", "EN");
        var outDir = Path.Combine(tempDir, "out");
        var runDir = Path.Combine(tempDir, "run");
        Directory.CreateDirectory(txt42Dir);
        Directory.CreateDirectory(txt4213Dir);
        Directory.CreateDirectory(json4213Dir);
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(txt42Dir, "UI_CN.txt"), """
        UI_EN = {
            Dup_Key = "txt42",
            Relaxed_Key: "relaxed",
            Concat_Key = "hello " ..
                "world",
        }
        """);
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(txt4213Dir, "UI_EN.txt"), """
        UI_EN = {
            Dup_Key = "txt4213",
            Txt_Only = "txt only",
        }
        """);
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(json4213Dir, "UI.json"), """{"Dup_Key":"json wins","Json_Only":"json only"}""");

        var config = TestConfig.Create();
        config.extractedContentsTempDir = outDir;
        config.runTempDir = runDir;
        var service = new ContentExtractorService(config);
        var modInfoDict = new Dictionary<string, ModInfo>
        {
            ["99"] = new() { modId = "99", modName = "Test", localDownloadedPath = tempDir }
        };
        var entries = new Dictionary<string, TranslationEntry>();

        try
        {
            var result = await service.ExtractContentsAsync(modInfoDict, entries, "testbatch");

            Assert.True(result.isSuccess);
            Assert.Equal("json wins", entries["99::Dup_Key"].translationValues["en"].text);
            Assert.Equal("txt only", entries["99::Txt_Only"].translationValues["en"].text);
            Assert.Equal("json only", entries["99::Json_Only"].translationValues["en"].text);
            Assert.Equal("relaxed", entries["99::Relaxed_Key"].translationValues["en"].text);
            Assert.Equal("hello world", entries["99::Concat_Key"].translationValues["en"].text);
            Assert.Contains(entries["99::Dup_Key"].containingFileInfos, info => info.gameMajorVersion == 42 && info.gameMinorVersion == 0);
            Assert.Contains(entries["99::Dup_Key"].containingFileInfos, info => info.gameMajorVersion == 42 && info.gameMinorVersion == 13);
            Assert.Contains("Relaxed_Key", await Utf8NoBom.ReadAllTextAsync(Path.Combine(runDir, "txt", "fuck.txt")));
            Assert.Contains("Dup_Key::en = \"json wins\"", await Utf8NoBom.ReadAllTextAsync(Path.Combine(outDir, "en", "99.txt")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}

/// <summary>Tests LLM-based content safety review with mock LLM responses.</summary>
public class ContentCheckerTests
{
    [Fact]
    /// <summary>Content checking should review unknown mods and queue accepted entries.</summary>
    public async Task CheckContents_ShouldReviewUnknownModAndQueueAcceptedEntries()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.contentCheckEnabled = true;
            var handler = new StubHttpMessageHandler(ChatResponse(
                """{"is_harmful":false,"confidence":0.99,"need_human_review":false,"risk_level":"safe","reason":"clean","violated_rules":[]}"""));
            using var httpClient = new HttpClient(handler);
            var service = new ContentCheckerService(config, httpClient);
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Safe Mod", description = "Simple UI labels", contentCheckStatus = ContentCheckStatus.NEEDVERIFICATION }
            };
            var entries = TestTranslations.Entries("hello");
            var diff = new Dictionary<string, TranslationEntry>();

            using var consoleOut = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(consoleOut);
            TaskResult result;
            try
            {
                result = await service.CheckContentsAsync(modInfo, entries, diff);
            }
            finally
            {
                Console.SetOut(originalOut);
            }

            Assert.True(result.isSuccess);
            Assert.Single(diff);
            Assert.Equal(ContentCheckStatus.ACCEPTED, modInfo["1"].contentCheckStatus);
            Assert.True(modInfo["1"].timeNextContentCheck > DateTime.UtcNow);
            Assert.Equal(1, handler.RequestCount);
            Assert.Contains("\"model\":\"deepseek-v4-flash\"", handler.LastRequestBody);
            using (var request = JsonDocument.Parse(handler.LastRequestBody))
            {
                Assert.Equal("low", request.RootElement.GetProperty("reasoning_effort").GetString());
                Assert.Equal("enabled", request.RootElement.GetProperty("thinking").GetProperty("type").GetString());
            }
            Assert.Contains("Key_0", handler.LastRequestBody);
            Assert.Contains("hello", handler.LastRequestBody);
            Assert.True(File.Exists(Path.Combine(config.contentCheckingPromptsTempDir, "1", "content_review_prompt.json")));
            Assert.True(File.Exists(Path.Combine(config.contentCheckingPromptsTempDir, "1", "content_review_prompt.md")));
            Assert.True(File.Exists(Path.Combine(config.contentCheckingResultsTempDir, "1.json")));
            Assert.True(File.Exists(Path.Combine(config.contentCheckingResultsTempDir, "1.md")));
            Assert.Contains("[content:reviewed]", consoleOut.ToString());
            Assert.Contains("status=ACCEPTED", consoleOut.ToString());
            Assert.DoesNotContain("checkedCount", result.summaryJson);
            Assert.DoesNotContain("skippedCount", result.summaryJson);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    /// <summary>Available UNKNOWN mods (newly added, never reviewed) should be content-checked.</summary>
    public async Task CheckContents_ShouldReviewAvailableUnknownMod()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.contentCheckEnabled = true;
            var handler = new StubHttpMessageHandler(ChatResponse(
                """{"is_harmful":false,"confidence":0.99,"need_human_review":false,"risk_level":"safe","reason":"clean","violated_rules":[]}"""));
            using var httpClient = new HttpClient(handler);
            var service = new ContentCheckerService(config, httpClient);
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Fresh Mod", contentCheckStatus = ContentCheckStatus.UNKNOWN, isAvailable = true }
            };
            var entries = TestTranslations.Entries("hello");
            var diff = new Dictionary<string, TranslationEntry>();

            var result = await service.CheckContentsAsync(modInfo, entries, diff);

            Assert.True(result.isSuccess);
            Assert.Single(diff);
            Assert.Equal(1, handler.RequestCount);
            Assert.Equal(ContentCheckStatus.ACCEPTED, modInfo["1"].contentCheckStatus);
            Assert.True(modInfo["1"].timeNextContentCheck > DateTime.UtcNow);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    /// <summary>Unavailable UNKNOWN mods (delisted / removed from Steam) should stay frozen.</summary>
    public async Task CheckContents_ShouldFreezeUnavailableUnknownMod()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.contentCheckEnabled = true;
            var handler = new StubHttpMessageHandler(ChatResponse(
                """{"is_harmful":false,"confidence":0.99,"need_human_review":false,"risk_level":"safe","reason":"clean","violated_rules":[]}"""));
            using var httpClient = new HttpClient(handler);
            var service = new ContentCheckerService(config, httpClient);
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Delisted Mod", contentCheckStatus = ContentCheckStatus.UNKNOWN, isAvailable = false }
            };
            var entries = TestTranslations.Entries("hello");
            var diff = new Dictionary<string, TranslationEntry>();

            var result = await service.CheckContentsAsync(modInfo, entries, diff);

            Assert.True(result.isSuccess);
            Assert.Empty(diff);
            Assert.Equal(0, handler.RequestCount);
            Assert.Equal(ContentCheckStatus.UNKNOWN, modInfo["1"].contentCheckStatus);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckContents_ShouldNotRecheckAcceptedModBeforeNextReview()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.contentCheckEnabled = true;
            var handler = new StubHttpMessageHandler(ChatResponse(
                """{"is_harmful":true,"confidence":0.99,"need_human_review":false,"risk_level":"high","reason":"bad","violated_rules":["毒品"]}"""));
            using var httpClient = new HttpClient(handler);
            var service = new ContentCheckerService(config, httpClient);
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new()
                {
                    modId = "1",
                    modName = "Cached Mod",
                    contentCheckStatus = ContentCheckStatus.ACCEPTED,
                    needsUpdate = true,
                    timeNextContentCheck = DateTime.UtcNow.AddDays(10)
                }
            };
            var entries = TestTranslations.Entries("hello");
            var diff = new Dictionary<string, TranslationEntry>();

            var result = await service.CheckContentsAsync(modInfo, entries, diff);

            Assert.True(result.isSuccess);
            Assert.Single(diff);
            Assert.Equal(0, handler.RequestCount);
            Assert.Equal(ContentCheckStatus.ACCEPTED, modInfo["1"].contentCheckStatus);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckContents_ShouldReuseReviewedModAcrossDifferentLanguageQueuesBeforeNextReview()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.contentCheckEnabled = true;
            config.supportedLanguages.Add(new() { ingameCode = "FR", englishName = "French", isoCode = "fr" });

            var handler = new StubHttpMessageHandler(ChatResponse(
                """{"is_harmful":false,"confidence":0.99,"need_human_review":false,"risk_level":"safe","reason":"clean","violated_rules":[]}"""));
            using var httpClient = new HttpClient(handler);
            var service = new ContentCheckerService(config, httpClient);
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Shared Review Mod", contentCheckStatus = ContentCheckStatus.NEEDVERIFICATION }
            };

            var firstEntry = TestTranslations.Entry("Key_Zh_Target", "hello");
            firstEntry.translationValues["zh-hans"] = new() { text = "", processStatus = "unprocessed" };
            var firstQueue = new Dictionary<string, TranslationEntry>
            {
                ["1::Key_Zh_Target"] = firstEntry
            };
            var firstDiff = new Dictionary<string, TranslationEntry>();

            var firstResult = await service.CheckContentsAsync(modInfo, firstQueue, firstDiff);

            Assert.True(firstResult.isSuccess);
            Assert.Single(firstDiff);
            Assert.Equal(1, handler.RequestCount);
            Assert.Equal(ContentCheckStatus.ACCEPTED, modInfo["1"].contentCheckStatus);
            Assert.True(modInfo["1"].timeNextContentCheck > DateTime.UtcNow);

            var secondEntry = TestTranslations.Entry("Key_Fr_Target", "world");
            secondEntry.translationValues["fr"] = new() { text = "", processStatus = "unprocessed" };
            var secondQueue = new Dictionary<string, TranslationEntry>
            {
                ["1::Key_Fr_Target"] = secondEntry
            };
            var secondDiff = new Dictionary<string, TranslationEntry>();

            var secondResult = await service.CheckContentsAsync(modInfo, secondQueue, secondDiff);

            Assert.True(secondResult.isSuccess);
            Assert.Single(secondDiff);
            Assert.Equal(1, handler.RequestCount);
            Assert.Equal(ContentCheckStatus.ACCEPTED, modInfo["1"].contentCheckStatus);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void CollectTargetWorkQueue_ShouldSkipEntriesOutsideKnownModSubset()
    {
        var keptEntry = TestTranslations.Entry("Known_Key", "hello");
        keptEntry.modId = "1";
        keptEntry.translationValues["zh-hans"] = new() { text = "", processStatus = "unprocessed" };

        var skippedEntry = TestTranslations.Entry("Unknown_Key", "world");
        skippedEntry.modId = "2";
        skippedEntry.translationValues["zh-hans"] = new() { text = "", processStatus = "unprocessed" };

        var entries = new Dictionary<string, TranslationEntry>
        {
            ["1::Known_Key"] = keptEntry,
            ["2::Unknown_Key"] = skippedEntry
        };

        var queue = global::PipelineRunner.CollectTargetWorkQueue(entries, ["zh-hans"], new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1" } });

        Assert.Single(queue);
        Assert.Contains("1::Known_Key", queue.Keys);
        Assert.DoesNotContain("2::Unknown_Key", queue.Keys);
    }

    [Fact]
    public void CreateDownloadExtractionBatches_ShouldCapRunAtThirtyBatches()
    {
        var modIds = Enumerable.Range(0, 31 * 2)
            .Select(index => $"mod_{index}")
            .ToList();

        var batches = global::PipelineRunner.CreateDownloadExtractionBatches(modIds, batchSize: 2);

        Assert.Equal(30, batches.Count);
        Assert.Equal(["mod_0", "mod_1"], batches[0]);
        Assert.DoesNotContain("mod_60", batches.SelectMany(batch => batch));
        Assert.DoesNotContain("mod_61", batches.SelectMany(batch => batch));
    }

    [Fact]
    public void DownloadBatchConcurrency_ShouldBeEight()
    {
        Assert.Equal(8, global::PipelineRunner.DownloadBatchConcurrency);
    }

    [Fact]
    public void CreateDownloadExtractionBatches_ShouldAllowRuntimeLimitOverride()
    {
        var modIds = Enumerable.Range(0, 10)
            .Select(index => $"mod_{index}")
            .ToList();

        var batches = global::PipelineRunner.CreateDownloadExtractionBatches(modIds, batchSize: 2, maxBatches: 2);

        Assert.Equal(2, batches.Count);
        Assert.Equal(["mod_0", "mod_1"], batches[0]);
        Assert.Equal(["mod_2", "mod_3"], batches[1]);
        Assert.DoesNotContain("mod_4", batches.SelectMany(batch => batch));
    }

    [Fact]
    public void ParseMaxDownloadExtractionBatches_ShouldUseDefaultOrExplicitValue()
    {
        Assert.Equal(
            global::PipelineRunner.MaxDownloadExtractionBatchesPerRun,
            global::PipelineRunner.ParseMaxDownloadExtractionBatches([]));
        Assert.Equal(
            7,
            global::PipelineRunner.ParseMaxDownloadExtractionBatches(["--max-download-batches", "7"]));
        Assert.Equal(
            9,
            global::PipelineRunner.ParseMaxDownloadExtractionBatches(["--max-download-batches=9"]));
        Assert.Throws<ArgumentException>(() =>
            global::PipelineRunner.ParseMaxDownloadExtractionBatches(["--max-download-batches", "0"]));
    }

    [Fact]
    public void HasPendingTargetEntries_ShouldReturnFalseWhenTargetAlreadyProcessed()
    {
        var entry = TestTranslations.Entry("Done_Key", "hello");
        entry.translationValues["zh-hans"] = new() { text = "已有译文", processStatus = "processed" };

        var batches = new List<TranslationBatch>
        {
            new()
            {
                batchId = 1,
                modId = "1",
                translationEntries = [entry],
                baseLang = "en",
                targetLang = "zh-hans"
            }
        };

        var hasPending = global::PipelineRunner.HasPendingTargetEntries(batches, "zh-hans");

        Assert.False(hasPending);
    }

    [Fact]
    public void HasPendingTargetEntries_ShouldReturnTrueWhenTargetMissingOrUnprocessed()
    {
        var processed = TestTranslations.Entry("Done_Key", "hello");
        processed.translationValues["zh-hans"] = new() { text = "已有译文", processStatus = "processed" };

        var pending = TestTranslations.Entry("Pending_Key", "world");
        pending.translationValues["zh-hans"] = new() { text = "", processStatus = "unprocessed" };

        var batches = new List<TranslationBatch>
        {
            new()
            {
                batchId = 1,
                modId = "1",
                translationEntries = [processed, pending],
                baseLang = "en",
                targetLang = "zh-hans"
            }
        };

        var hasPending = global::PipelineRunner.HasPendingTargetEntries(batches, "zh-hans");

        Assert.True(hasPending);
    }

    [Fact]
    public void MergeUpdatedModInfos_ShouldPreserveNonSubsetCacheAndApplySubsetUpdates()
    {
        var persisted = new Dictionary<string, ModInfo>
        {
            ["1"] = new() { modId = "1", modName = "Old One", contentCheckStatus = ContentCheckStatus.ACCEPTED },
            ["2"] = new() { modId = "2", modName = "Keep Me", contentCheckStatus = ContentCheckStatus.ACCEPTED }
        };
        var updated = new Dictionary<string, ModInfo>
        {
            ["1"] = new() { modId = "1", modName = "New One", contentCheckStatus = ContentCheckStatus.REJECTED }
        };

        global::PipelineRunner.MergeUpdatedModInfos(updated, persisted);

        Assert.Equal(2, persisted.Count);
        Assert.Equal("New One", persisted["1"].modName);
        Assert.Equal(ContentCheckStatus.REJECTED, persisted["1"].contentCheckStatus);
        Assert.Equal("Keep Me", persisted["2"].modName);
        Assert.Equal(ContentCheckStatus.ACCEPTED, persisted["2"].contentCheckStatus);
    }

    [Fact]
    public async Task CheckContents_ShouldQueueEntriesIndependentOfTargetLanguage()
    {
        var config = TestConfig.Create();
        config.contentCheckEnabled = false;
        config.priorityLanguage = "zh-hans";
        var entry = TestTranslations.Entry("Key_With_Target_History", "hello");
        entry.translationValues["zh-hans"] = new() { text = "已有译文" };
        var modInfo = new Dictionary<string, ModInfo>
        {
            ["1"] = new() { modId = "1", modName = "Target History Mod" }
        };
        var entries = new Dictionary<string, TranslationEntry>
        {
            ["1::Key_With_Target_History"] = entry
        };
        var diff = new Dictionary<string, TranslationEntry>();

        var result = await new ContentCheckerService(config).CheckContentsAsync(modInfo, entries, diff);

        Assert.True(result.isSuccess);
        Assert.Single(diff);
        Assert.Contains("queuedCount", result.summaryJson);
    }

    [Fact]
    public async Task CheckContents_ShouldRejectHarmfulModAndExcludeEntries()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.contentCheckEnabled = true;
            var handler = new StubHttpMessageHandler(ChatResponse(
                """{"is_harmful":true,"confidence":0.99,"need_human_review":false,"risk_level":"high","reason":"drug crafting","violated_rules":["毒品"]}"""));
            using var httpClient = new HttpClient(handler);
            var service = new ContentCheckerService(config, httpClient);
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Unsafe Mod", contentCheckStatus = ContentCheckStatus.NEEDVERIFICATION }
            };
            var entries = TestTranslations.Entries("craft drugs");
            var diff = new Dictionary<string, TranslationEntry>();

            var result = await service.CheckContentsAsync(modInfo, entries, diff);

            Assert.True(result.isSuccess);
            Assert.Empty(diff);
            Assert.Equal(ContentCheckStatus.REJECTED, modInfo["1"].contentCheckStatus);
            Assert.Equal(1, handler.RequestCount);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    private static string ChatResponse(string content)
    {
        return Utf8NoBom.SerializeJson(new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content
                    }
                }
            }
        });
    }
}

/// <summary>Tests placeholder/canary methods across downstream pipeline services.</summary>
public class PlaceholderServiceTests
{
    [Fact]
    /// <summary>All downstream services should return successful TaskResults for valid inputs.</summary>
    public async Task DownstreamServices_ShouldReturnSuccessfulTaskResult()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            var modInfo = new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1", modName = "Test Mod" } };
            var entries = TestTranslations.Entries("hello");
            var refModInfo = new Dictionary<string, ModInfo>();
            var refEntries = new Dictionary<string, TranslationEntry>();
            var diff = new Dictionary<string, TranslationEntry>();
            var batches = new List<TranslationBatch>();
            var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);

            const string embeddingResponse = """{"data":[{"embedding":[1.0,0.0]}]}""";
            const string llmResponse = """
            {
              "choices": [
                {
                  "message": {
                    "content": "T1\t你好\t0.95"
                  }
                }
              ]
            }
            """;

            Assert.True((await new ContentCheckerService(config).CheckContentsAsync(modInfo, entries, diff)).isSuccess);
            Assert.Single(diff);
            Assert.True((await new EmbeddingFetcherService(config, new HttpClient(new StubHttpMessageHandler(embeddingResponse))).FetchEmbeddingsAsync(modInfo, diff, entries, refModInfo, refEntries)).isSuccess);
            Assert.Equal(new[] { 1.0f, 0.0f }, diff["1::Key_0"].embeddingVector);
            Assert.True((await new TranslationBatcherService(config).CreateBatchesAsync(modInfo, diff, batches)).isSuccess);
            Assert.Single(batches);
            Assert.True((await new RagContextRetrieverService(config).RetrieveContextsAsync(refEntries, diff, batches, ragContextByEntryKey)).isSuccess);
            Assert.True((await new LLMTranslatorService(config, new HttpClient(new StubHttpMessageHandler(llmResponse))).TranslateAsync(modInfo, batches, entries, ragContextByEntryKey)).isSuccess);
            Assert.Equal("你好", entries["1::Key_0"].translationValues["zh-hans"].text);
            Assert.Equal(0.95f, entries["1::Key_0"].translationValues["zh-hans"].confidence);
            Assert.True((await new ResultWriterService(config).WriteResultsAsync(modInfo, refEntries, entries)).isSuccess);
            Assert.True(Directory.Exists(Path.Combine(tempDir, "data", "translations")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task EmbeddingFetcher_ShouldStoreFullSourceHashWhenEmbeddingTruncatedText()
    {
        var config = TestConfig.Create();
        var entry = TestTranslations.Entry("Long_Key", new string('a', 600));
        var entries = new Dictionary<string, TranslationEntry> { ["1::Long_Key"] = entry };
        var firstHandler = new StubHttpMessageHandler("""{"data":[{"embedding":[1.0,0.0]}]}""");

        var firstResult = await new EmbeddingFetcherService(config, new HttpClient(firstHandler))
            .FetchEmbeddingsAsync([], entries, entries, [], []);

        Assert.True(firstResult.isSuccess);
        Assert.Equal(1, firstHandler.RequestCount);
        Assert.Equal(new[] { 1.0f, 0.0f }, entry.embeddingVector);
        Assert.Equal(64, entry.embeddingHash.Length);

        var secondHandler = new StubHttpMessageHandler("""{"data":[{"embedding":[0.0,1.0]}]}""");
        var secondResult = await new EmbeddingFetcherService(config, new HttpClient(secondHandler))
            .FetchEmbeddingsAsync([], entries, entries, [], []);

        Assert.True(secondResult.isSuccess);
        Assert.Equal(0, secondHandler.RequestCount);
        Assert.Equal(new[] { 1.0f, 0.0f }, entry.embeddingVector);
    }

    [Fact]
    public async Task EmbeddingFetcher_ShouldUseKeyOnlyWhenBaseTextMissing()
    {
        var config = TestConfig.Create();
        var entry = TestTranslations.Entry("Missing_Base_Key", "", "en");
        entry.translationValues["ar"] = new() { text = "ترجمة مولدة", processStatus = "processed", status = "unverified" };
        var entries = new Dictionary<string, TranslationEntry> { ["1::Missing_Base_Key"] = entry };
        var handler = new StubHttpMessageHandler("""{"data":[{"embedding":[1.0,0.0]}]}""");

        var result = await new EmbeddingFetcherService(config, new HttpClient(handler))
            .FetchEmbeddingsAsync([], entries, entries, [], []);

        Assert.True(result.isSuccess);
        Assert.Equal(1, handler.RequestCount);
        Assert.True(entry.embeddingValues.ContainsKey("normal_key_only"));
        Assert.False(entry.embeddingValues.ContainsKey("normal_fallback_text"));
    }

    [Fact]
    public async Task EmbeddingFetcher_ShouldReuseKeyOnlyWhenGeneratedTranslationsExist()
    {
        var config = TestConfig.Create();
        var entry = TestTranslations.Entry("Missing_Base_Key", "", "en");
        entry.translationValues["ar"] = new() { text = "ترجمة مولدة", processStatus = "processed", status = "unverified" };
        entry.embeddingValues["normal_key_only"] = new()
        {
            sourceKind = "normal_key_only",
            hash = ComputeSha256("normal_key_only::1::Missing_Base_Key = \"\""),
            vector = [1.0f, 0.0f]
        };
        var entries = new Dictionary<string, TranslationEntry> { ["1::Missing_Base_Key"] = entry };
        var handler = new StubHttpMessageHandler("""{"data":[{"embedding":[0.0,1.0]}]}""");

        var result = await new EmbeddingFetcherService(config, new HttpClient(handler))
            .FetchEmbeddingsAsync([], entries, entries, [], []);

        Assert.True(result.isSuccess);
        Assert.Equal(0, handler.RequestCount);
        Assert.False(entry.embeddingValues.ContainsKey("normal_fallback_text"));
    }

    [Fact]
    public async Task EmbeddingFetcher_ShouldStopAfterRepeatedFailedBatches()
    {
        var config = TestConfig.Create();
        var entries = Enumerable.Range(0, 97)
            .Select(i => TestTranslations.Entry($"Key_{i}", $"text {i}"))
            .ToDictionary(entry => $"1::{entry.translationKey}", entry => entry, StringComparer.Ordinal);
        var handler = new StubHttpMessageHandler("""{"data":[]}""");

        var result = await new EmbeddingFetcherService(config, new HttpClient(handler))
            .FetchEmbeddingsAsync([], entries, entries, [], []);

        Assert.True(result.isSuccess);
        Assert.Equal(3, handler.RequestCount);
        Assert.Equal(97, result.warningCount);
        Assert.All(entries.Values, entry => Assert.Empty(entry.embeddingValues));
    }

    [Fact]
    public async Task EmbeddingFetcher_ShouldRetryCanceledRequests()
    {
        var config = TestConfig.Create();
        var entries = TestTranslations.Entries("text");
        var handler = new TimeoutHttpMessageHandler();

        var result = await new EmbeddingFetcherService(config, new HttpClient(handler))
            .FetchEmbeddingsAsync([], entries, entries, [], []);

        Assert.True(result.isSuccess);
        Assert.Equal(3, handler.RequestCount);
        Assert.Equal(1, result.warningCount);
    }

    private static string ComputeSha256(string text)
    {
        var hash = SHA256.HashData(Utf8NoBom.Encoding.GetBytes(text));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    [Fact]
    public async Task LlmTranslator_ShouldNotSplitBatchAfterJsonParseFailure()
    {
        var config = TestConfig.Create();
        config.llmFixedConcurrency = 0;
        config.llmConcurrencyMaxRetries = 0;
        var entries = TestTranslations.Entries("hello", "world");
        var batch = new TranslationBatch
        {
            batchId = 1,
            modId = "1",
            translationEntries = entries.Values.ToList()
        };
        var response = Utf8NoBom.SerializeJson(new
        {
            choices = new[]
            {
                new { message = new { content = "T1\tbroken" } }
            }
        });
        var handler = new StubHttpMessageHandler(response);

        var result = await new LLMTranslatorService(config, new HttpClient(handler)).TranslateAsync(
            new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1", modName = "Test Mod" } },
            [batch],
            entries,
            new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal));

        Assert.True(result.isSuccess);
        Assert.Equal(0, result.errorCount);
        Assert.Equal(1, result.warningCount);
        Assert.Equal(1, handler.RequestCount);
        Assert.DoesNotContain("\"response_format\"", handler.LastRequestBody);
    }

    [Fact]
    public async Task LlmTranslator_ShouldParseTabSeparatedOutput()
    {
        var config = TestConfig.Create();
        config.llmModel = "deepseek-v4-flash";
        config.llmConcurrencyMaxRetries = 0;
        var entries = TestTranslations.Entries("hello");
        var batch = new TranslationBatch
        {
            batchId = 1,
            modId = "1",
            translationEntries = entries.Values.ToList()
        };
        var response = LlmChatResponse("T1\t你好\t0.9");
        var handler = new StubHttpMessageHandler(response);

        var result = await new LLMTranslatorService(config, new HttpClient(handler)).TranslateAsync(
            new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1", modName = "Tab Output Mod" } },
            [batch],
            entries,
            new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal));

        Assert.True(result.isSuccess);
        Assert.Equal(0, result.warningCount);
        Assert.Equal("你好", entries["1::Key_0"].translationValues["zh-hans"].text);
        Assert.DoesNotContain("\"response_format\"", handler.LastRequestBody);
        using var request = JsonDocument.Parse(handler.LastRequestBody);
        Assert.Equal("low", request.RootElement.GetProperty("reasoning_effort").GetString());
        Assert.Equal("enabled", request.RootElement.GetProperty("thinking").GetProperty("type").GetString());
    }

    [Fact]
    public async Task LlmTranslator_ShouldSanitizeDebugResponsePathSegments()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            Utf8NoBom.WriteAllText(
                Path.Combine(config.baseDir, "src", "prompt_templates", "system_prompt_translate_engine.txt"),
                "Translate to {{TARGET_LANG}}.");
            config.llmConcurrencyMaxRetries = 0;
            var modId = "1\0bad";
            var key = "Key_\0With:Invalid/Chars";
            var entry = TestTranslations.Entry(key, "hello");
            entry.modId = modId;
            var entries = new Dictionary<string, TranslationEntry> { [$"{modId}::{key}"] = entry };
            var batch = new TranslationBatch
            {
                batchId = 1,
                modId = modId,
                translationEntries = [entry]
            };
            var responseContent = "T1\t你好\t0.9";
            var handler = new StubHttpMessageHandler(LlmChatResponse(responseContent));

            var result = await new LLMTranslatorService(config, new HttpClient(handler)).TranslateAsync(
                new Dictionary<string, ModInfo> { [modId] = new() { modId = modId, modName = "Debug Path Mod" } },
                [batch],
                entries,
                new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal));

            Assert.True(result.isSuccess);
            Assert.Equal(0, result.warningCount);
            Assert.Equal("你好", entry.translationValues["zh-hans"].text);
            var responseFile = Assert.Single(Directory.GetFiles(Path.Combine(config.runTempDir, "llm_responses"), "*.json", SearchOption.AllDirectories));
            Assert.DoesNotContain('\0', Path.GetFileName(responseFile));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task LlmTranslator_ShouldBuildTextOutputPromptContract()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            Utf8NoBom.WriteAllText(
                Path.Combine(config.baseDir, "src", "prompt_templates", "system_prompt_translate_engine.txt"),
                "Translate to {{TARGET_LANG}}. Return plain text.");
            var entries = TestTranslations.Entries("hello");
            var batch = new TranslationBatch
            {
                batchId = 1,
                modId = "1",
                translationEntries = entries.Values.ToList()
            };

            await new LLMTranslatorService(config).PrepareTranslationPlanAsync(
                new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1", modName = "Prompt Contract Mod" } },
                [batch],
                new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal),
                "zh-hans");

            var prompt = await Utf8NoBom.ReadAllTextAsync(Path.Combine(config.runTempDir, "prompts", "1", "prompt_001.md"));
            Assert.Contains("# Output Rules", prompt);
            Assert.Contains("CRITICAL OUTPUT RULES", prompt);
            Assert.Contains("T1", prompt);
            Assert.Contains("Target translation 1", prompt);
            Assert.EndsWith("只返回符合上述输出规则的纯文本, 不输出任何额外字符。", prompt);
            Assert.True(prompt.IndexOf("# Output Rules", StringComparison.Ordinal) < prompt.IndexOf("# Translation Entry", StringComparison.Ordinal));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task LlmTranslator_ShouldUseSharedCacheablePrefixBeforeModInfo()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            var targetDir = Path.Combine(config.baseDir, "src", "prompt_templates", "zh-hans");
            Directory.CreateDirectory(targetDir);
            Utf8NoBom.WriteAllText(Path.Combine(targetDir, "translation_schema_zh-hans.md"), "Use concise target text.");
            Utf8NoBom.WriteAllText(Path.Combine(targetDir, "translation_dictionary_zh-hans.json"), """
            [
              { "en": "hello", "translated": "你好" }
            ]
            """);
            var entry0 = TestTranslations.Entry("Key_0", "hello");
            var entry1 = TestTranslations.Entry("Key_1", "world");
            var batches = new List<TranslationBatch>
            {
                new() { batchId = 1, modId = "1", translationEntries = [entry0] },
                new() { batchId = 2, modId = "1", translationEntries = [entry1] }
            };

            await new LLMTranslatorService(config).PrepareTranslationPlanAsync(
                new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1", modName = "Cache Prefix Mod" } },
                batches,
                new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal),
                "zh-hans");

            var prompt0 = await Utf8NoBom.ReadAllTextAsync(Path.Combine(config.runTempDir, "prompts", "1", "prompt_001.md"));
            var prompt1 = await Utf8NoBom.ReadAllTextAsync(Path.Combine(config.runTempDir, "prompts", "1", "prompt_002.md"));
            var prefix0 = prompt0[..prompt0.IndexOf("# Mod Info", StringComparison.Ordinal)];
            var prefix1 = prompt1[..prompt1.IndexOf("# Mod Info", StringComparison.Ordinal)];

            Assert.Equal(prefix0, prefix1);
            Assert.Contains("# Translation Rules", prefix0);
            Assert.Contains("# Terminology", prefix0);
            Assert.Contains("# Output Rules", prefix0);
            Assert.DoesNotContain("# Translation Entry", prefix0);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task LlmTranslator_ShouldRetryPromptAtMostFiveTimes()
    {
        var config = TestConfig.Create();
        config.llmFixedConcurrency = 0;
        config.llmConcurrencyInitial = 1;
        config.llmConcurrencyMaximum = 1;
        config.llmConcurrencyMaxRetries = 5;
        config.llmConcurrencyRetryBaseDelayMs = 1;
        config.llmConcurrencyRetryMaxDelayMs = 1;
        var entries = TestTranslations.Entries("hello");
        var batch = new TranslationBatch
        {
            batchId = 1,
            modId = "1",
            translationEntries = entries.Values.ToList()
        };
        var handler = new DynamicHttpMessageHandler((_, requestNumber) =>
        {
            var response = requestNumber <= 5
                ? """{"error":"rate limited"}"""
                : LlmChatResponse("T1\t你好\t0.9");
            var status = requestNumber <= 5 ? HttpStatusCode.TooManyRequests : HttpStatusCode.OK;
            return Task.FromResult((status, response));
        });

        var result = await new LLMTranslatorService(config, new HttpClient(handler)).TranslateAsync(
            new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1", modName = "Retry Mod" } },
            [batch],
            entries,
            new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal));

        Assert.True(result.isSuccess);
        Assert.Equal(6, handler.RequestCount);
        Assert.Equal("你好", entries["1::Key_0"].translationValues["zh-hans"].text);
        Assert.Contains("\"retriedAttemptCount\":5", result.summaryJson);
    }

    [Fact]
    public async Task LlmTranslator_ShouldWarmUpTargetsWithMoreThanFiveBatches()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.llmFixedConcurrency = 0;
            config.llmConcurrencyInitial = 4;
            config.llmConcurrencyMaximum = 4;
            config.llmConcurrencyMaxRetries = 0;
            var entries = Enumerable.Range(0, 6)
                .Select(index => TestTranslations.Entry($"Key_{index}", $"text {index}"))
                .ToDictionary(entry => $"1::{entry.translationKey}", entry => entry, StringComparer.Ordinal);
            var batches = entries.Values
                .Select((entry, index) => new TranslationBatch
                {
                    batchId = index + 1,
                    modId = "1",
                    translationEntries = [entry]
                })
                .ToList();
            var requestKinds = new List<string>();
            var keys = entries.Values.Select(entry => entry.translationKey).ToList();
            var handler = new DynamicHttpMessageHandler((body, _) =>
            {
                using var doc = JsonDocument.Parse(body);
                var prompt = doc.RootElement
                    .GetProperty("messages")[0]
                    .GetProperty("content")
                    .GetString() ?? "";
                if (!prompt.Contains("# Translation Entry", StringComparison.Ordinal))
                {
                    requestKinds.Add("warmup");
                    return Task.FromResult((HttpStatusCode.OK, LlmChatResponse("Warmup done")));
                }

                var key = keys.Single(candidate => prompt.Contains($"\t{candidate}\t", StringComparison.Ordinal));
                requestKinds.Add(key);
                var content = $"T1\tTranslated {key}\t0.9";
                return Task.FromResult((HttpStatusCode.OK, LlmChatResponse(content)));
            });
            var service = new LLMTranslatorService(config, new HttpClient(handler));

            var plan = await service.PrepareTranslationPlanAsync(
                new Dictionary<string, ModInfo> { ["1"] = new() { modId = "1", modName = "Warmup Mod" } },
                batches,
                new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal),
                "zh-hans");
            var result = await service.ExecuteTranslationPlansAsync([plan]);

            Assert.True(result.isSuccess);
            Assert.Equal(7, handler.RequestCount);
            Assert.Equal(1, handler.MaxInFlight);
            Assert.Equal("warmup", requestKinds[0]);
            Assert.Contains("\"warmupRequestCount\":1", result.summaryJson);
            Assert.Contains("\"failedWarmupCount\":0", result.summaryJson);
            Assert.True(File.Exists(Path.Combine(config.runTempDir, "prompts", "warmup.md")));
            foreach (var entry in entries.Values)
                Assert.Equal($"Translated {entry.translationKey}", entry.translationValues["zh-hans"].text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task LlmTranslator_TaskPoolShouldApplyMultipleTargetsSerially()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            Utf8NoBom.WriteAllText(Path.Combine(config.baseDir, "src", "prompt_templates", "system_prompt_translate_engine.txt"), "Translate to {{TARGET_LANG}}.");
            config.supportedLanguages.Add(new() { ingameCode = "AR", englishName = "Arabic", isoCode = "ar" });
            config.llmConcurrencyInitial = 2;
            config.llmConcurrencyMaximum = 2;
            config.llmConcurrencyMaxRetries = 0;
            var entry = TestTranslations.Entry("Key_0", "hello");
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };
            var batch = new TranslationBatch
            {
                batchId = 1,
                modId = "1",
                baseLang = "en",
                translationEntries = [entry]
            };
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Pool Mod" }
            };
            var rag = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);
            var requestTargets = new List<string>();
            var handler = new DynamicHttpMessageHandler(async (body, _) =>
            {
                await Task.Delay(50);
                var isArabic = body.Contains("Arabic (ar)", StringComparison.Ordinal);
                requestTargets.Add(isArabic ? "ar" : "zh-hans");
                var translation = isArabic ? "مرحبا" : "你好";
                return (HttpStatusCode.OK, LlmChatResponse($"T1\t{translation}\t0.9"));
            });
            var service = new LLMTranslatorService(config, new HttpClient(handler));

            var zhPlan = await service.PrepareTranslationPlanAsync(modInfo, [batch], rag, "zh-hans");
            var arPlan = await service.PrepareTranslationPlanAsync(modInfo, [batch], rag, "ar");
            var result = await service.ExecuteTranslationPlansAsync([zhPlan, arPlan]);

            Assert.True(result.isSuccess);
            Assert.Equal(2, handler.RequestCount);
            Assert.Equal(1, handler.MaxInFlight);
            Assert.Equal(["zh-hans", "ar"], requestTargets);
            Assert.Equal("你好", entry.translationValues["zh-hans"].text);
            Assert.Equal("مرحبا", entry.translationValues["ar"].text);
            Assert.True(File.Exists(Path.Combine(config.runTempDir, "prompts", "zh-hans", "1", "prompt_001.md")));
            Assert.True(File.Exists(Path.Combine(config.runTempDir, "prompts", "ar", "1", "prompt_001.md")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task LlmTranslator_ShouldUseOnlyCurrentTargetHistoryInPrompt()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.supportedLanguages.Add(new() { ingameCode = "AR", englishName = "Arabic", isoCode = "ar" });
            var entry = TestTranslations.Entry("Key_0", "hello");
            entry.translationValues["zh-hans"] = new() { text = "已有中文" };
            entry.translationValues["fr"] = new() { text = "francais" };
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };
            var batch = new TranslationBatch
            {
                batchId = 1,
                modId = "1",
                baseLang = "en",
                targetLang = "ar",
                translationEntries = [entry]
            };
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Prompt Context Mod" }
            };
            var rag = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);

            config.priorityLanguage = "ar";
            var arResponse = Utf8NoBom.SerializeJson(new
            {
                choices = new[] { new { message = new { content = "T1\tترجمة عربية\t0.9" } } }
            });
            Assert.True((await new LLMTranslatorService(config, new HttpClient(new StubHttpMessageHandler(arResponse)))
                .TranslateAsync(modInfo, [batch], entries, rag)).isSuccess);

            var arPrompt = await Utf8NoBom.ReadAllTextAsync(Path.Combine(config.runTempDir, "prompts", "ar", "1", "prompt_001.md"));
            Assert.Contains("T1\tKey_0\ten\thello\t\tfalse", arPrompt);
            Assert.DoesNotContain("已有中文", arPrompt);
            Assert.DoesNotContain("francais", arPrompt);

            config.priorityLanguage = "zh-hans";
            batch.targetLang = "zh-hans";
            var zhResponse = Utf8NoBom.SerializeJson(new
            {
                choices = new[] { new { message = new { content = "T1\t新的中文\t0.9" } } }
            });
            Assert.True((await new LLMTranslatorService(config, new HttpClient(new StubHttpMessageHandler(zhResponse)))
                .TranslateAsync(modInfo, [batch], entries, rag)).isSuccess);

            var zhPrompt = await Utf8NoBom.ReadAllTextAsync(Path.Combine(config.runTempDir, "prompts", "zh-hans", "1", "prompt_001.md"));
            Assert.Contains("已有中文", zhPrompt);
            Assert.DoesNotContain("francais", zhPrompt);
            Assert.DoesNotContain("ترجمة عربية", zhPrompt);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task MissingBaseSource_ShouldUseKeyOnlySource()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.contentCheckEnabled = false;
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Key Only Source Mod" }
            };
            var entry = TestTranslations.Entry("Recipe_Improvise_Antibiotics", "항생제 급조하기", "ko");
            var entries = new Dictionary<string, TranslationEntry>
            {
                ["1::Recipe_Improvise_Antibiotics"] = entry
            };
            var diff = new Dictionary<string, TranslationEntry>();
            var batches = new List<TranslationBatch>();
            var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);
            const string llmResponse = """
            {
              "choices": [
                {
                  "message": {
                    "content": "T1\t临时制作抗生素\t0.85"
                  }
                }
              ]
            }
            """;
            var handler = new StubHttpMessageHandler(llmResponse);

            Assert.True((await new ContentCheckerService(config).CheckContentsAsync(modInfo, entries, diff)).isSuccess);
            Assert.Single(diff);
            Assert.True((await new TranslationBatcherService(config).CreateBatchesAsync(modInfo, diff, batches)).isSuccess);
            Assert.True((await new LLMTranslatorService(config, new HttpClient(handler)).TranslateAsync(modInfo, batches, entries, ragContextByEntryKey)).isSuccess);
            Assert.True((await new ResultWriterService(config).WriteResultsAsync(modInfo, [], entries)).isSuccess);

            var batchJson = await Utf8NoBom.ReadAllTextAsync(Path.Combine(config.translationBatchesTempDir, "1", "batch_001.json"));
            Assert.Contains("\"en\": \"\"", batchJson);

            Assert.Equal("", entries["1::Recipe_Improvise_Antibiotics"].translationValues["zh-hans"].text);
            Assert.Equal(1.0f, entries["1::Recipe_Improvise_Antibiotics"].translationValues["zh-hans"].confidence);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    private static string LlmChatResponse(string content)
    {
        return Utf8NoBom.SerializeJson(new
        {
            choices = new[]
            {
                new { message = new { content } }
            }
        });
    }
}

/// <summary>End-to-end integration tests for the full pipeline flow.</summary>
public class IntegrationTests
{
    [Fact]
    /// <summary>Full pipeline from extraction through translation should produce data/translations output.</summary>
    public async Task SharedDictionaryFlow_ExtractToPlaceholders_ShouldWork()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var enDir = Path.Combine(tempDir, "media", "lua", "shared", "Translate", "EN");
        Directory.CreateDirectory(enDir);
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(enDir, "UI_EN.json"), """{"IGUI_Test":"Hello"}""");

        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.extractedContentsTempDir = Path.Combine(tempDir, "out");
            var modInfo = new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", modName = "Test Mod", localDownloadedPath = tempDir }
            };
            var entries = new Dictionary<string, TranslationEntry>();
            var refModInfo = new Dictionary<string, ModInfo>();
            var refEntries = new Dictionary<string, TranslationEntry>();
            var diff = new Dictionary<string, TranslationEntry>();
            var batches = new List<TranslationBatch>();
            var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);
            const string embeddingResponse = """{"data":[{"embedding":[1.0,0.0]}]}""";
            const string llmResponse = """
            {
              "choices": [
                {
                  "message": {
                    "content": "T1\t你好\t0.9"
                  }
                }
              ]
            }
            """;

            Assert.True((await new ContentExtractorService(config).ExtractContentsAsync(modInfo, entries)).isSuccess);
            Assert.True((await new ContentCheckerService(config).CheckContentsAsync(modInfo, entries, diff)).isSuccess);
            Assert.True((await new EmbeddingFetcherService(config, new HttpClient(new StubHttpMessageHandler(embeddingResponse))).FetchEmbeddingsAsync(modInfo, diff, entries, refModInfo, refEntries)).isSuccess);
            Assert.True((await new TranslationBatcherService(config).CreateBatchesAsync(modInfo, diff, batches)).isSuccess);
            Assert.True((await new RagContextRetrieverService(config).RetrieveContextsAsync(refEntries, diff, batches, ragContextByEntryKey)).isSuccess);
            Assert.True((await new LLMTranslatorService(config, new HttpClient(new StubHttpMessageHandler(llmResponse))).TranslateAsync(modInfo, batches, entries, ragContextByEntryKey)).isSuccess);
            Assert.True((await new ResultWriterService(config).WriteResultsAsync(modInfo, refEntries, entries)).isSuccess);
            Assert.Contains("1::IGUI_Test", entries.Keys);
            Assert.True(Directory.Exists(Path.Combine(tempDir, "data", "translations")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}

/// <summary>Tests RAG context retrieval from reference embeddings and exact key matches.</summary>
public class RagContextTests
{
    [Fact]
    /// <summary>RAG context retrieval should populate the runtime context map for matching entries.</summary>
    public async Task RetrieveContexts_ShouldPopulateRuntimeMap()
    {
        var config = TestConfig.Create();
        config.ragSimilarityThreshold = 0.1f;
        var entry = TestTranslations.Entry("Key_0", "hello");
        entry.embeddingVector = [1.0f, 0.0f];
        var refEntry = TestTranslations.Entry("Ref_Key", "hello ref");
        refEntry.modId = "ref";
        refEntry.embeddingVector = [1.0f, 0.0f];
        refEntry.translationValues["zh-hans"] = new() { text = "参考", isVerified = true, status = "verified" };
        var batches = new List<TranslationBatch>
        {
            new()
            {
                batchId = 1,
                modId = "1",
                translationEntries = [entry]
            }
        };
        var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);

        var result = await new RagContextRetrieverService(config).RetrieveContextsAsync(
            new Dictionary<string, TranslationEntry> { ["ref::Ref_Key"] = refEntry },
            new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry },
            batches,
            ragContextByEntryKey);

        Assert.True(result.isSuccess);
        var context = Assert.Single(ragContextByEntryKey["1::Key_0"]);
        Assert.Equal("参考", context["translation"]);
    }

    [Fact]
    /// <summary>Should use all available translation entries as reference pool, skipping dimension mismatches.</summary>
    public async Task RetrieveContexts_ShouldUseAllTranslationEntriesAndSkipDimensionMismatch()
    {
        var config = TestConfig.Create();
        config.ragSimilarityThreshold = 0.1f;
        var query = TestTranslations.Entry("Query_Key", "query");
        query.embeddingVector = [1.0f, 0.0f];

        var translated = TestTranslations.Entry("Translated_Key", "translated source");
        translated.modId = "2";
        translated.embeddingVector = [1.0f, 0.0f];
        translated.translationValues["zh-hans"] = new() { text = "全量上下文", isVerified = true, status = "verified" };

        var badDimension = TestTranslations.Entry("Bad_Dim", "bad dim");
        badDimension.modId = "3";
        badDimension.embeddingVector = [1.0f, 0.0f, 0.0f];
        badDimension.translationValues["zh-hans"] = new() { text = "维度错误", isVerified = true, status = "verified" };

        var batches = new List<TranslationBatch>
        {
            new()
            {
                batchId = 1,
                modId = "1",
                translationEntries = [query]
            }
        };
        var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);

        var result = await new RagContextRetrieverService(config).RetrieveContextsAsync(
            [],
            new Dictionary<string, TranslationEntry>
            {
                ["1::Query_Key"] = query,
                ["2::Translated_Key"] = translated,
                ["3::Bad_Dim"] = badDimension
            },
            new Dictionary<string, TranslationEntry> { ["1::Query_Key"] = query },
            batches,
            ragContextByEntryKey);

        Assert.True(result.isSuccess);
        var context = Assert.Single(ragContextByEntryKey["1::Query_Key"]);
        Assert.Equal("全量上下文", context["translation"]);
    }

    [Fact]
    /// <summary>Reference translations should switch by target language when embeddings match.</summary>
    public async Task RetrieveContexts_ShouldSwitchReferenceTranslationByTargetLanguage()
    {
        var config = TestConfig.Create();
        config.supportedLanguages.Add(new() { ingameCode = "AR", englishName = "Arabic", isoCode = "ar" });
        config.ragSimilarityThreshold = 0.1f;
        var query = TestTranslations.Entry("Query_Key", "query");
        query.embeddingVector = [1.0f, 0.0f];

        var reference = TestTranslations.Entry("Reference_Key", "reference");
        reference.modId = "ref";
        reference.embeddingVector = [1.0f, 0.0f];
        reference.translationValues["zh-hans"] = new() { text = "中文参考", isVerified = true, status = "verified" };
        reference.translationValues["ar"] = new() { text = "مرجع عربي", isVerified = true, status = "verified" };

        var batches = new List<TranslationBatch>
        {
            new()
            {
                batchId = 1,
                modId = "1",
                translationEntries = [query]
            }
        };
        var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);
        var refEntries = new Dictionary<string, TranslationEntry> { ["ref::Reference_Key"] = reference };
        var entries = new Dictionary<string, TranslationEntry> { ["1::Query_Key"] = query };

        config.priorityLanguage = "ar";
        Assert.True((await new RagContextRetrieverService(config).RetrieveContextsAsync(refEntries, entries, batches, ragContextByEntryKey)).isSuccess);
        Assert.Equal("مرجع عربي", Assert.Single(ragContextByEntryKey["1::Query_Key"])["translation"]);

        config.priorityLanguage = "zh-hans";
        Assert.True((await new RagContextRetrieverService(config).RetrieveContextsAsync(refEntries, entries, batches, ragContextByEntryKey)).isSuccess);
        Assert.Equal("中文参考", Assert.Single(ragContextByEntryKey["1::Query_Key"])["translation"]);
    }

    [Fact]
    /// <summary>Candidates should be scored and filtered per-target-language based on similarity threshold.</summary>
    public async Task RetrieveContexts_ShouldScoreCandidatesPerTargetLanguage()
    {
        var config = TestConfig.Create();
        config.supportedLanguages.Add(new() { ingameCode = "AR", englishName = "Arabic", isoCode = "ar" });
        config.ragSimilarityThreshold = 0.0f;
        config.ragTopK = 2;

        var query = TestTranslations.Entry("Query_Key", "query");
        query.embeddingVector = [1.0f, 0.0f];

        var referenceA = TestTranslations.Entry("Reference_A", "reference a");
        referenceA.modId = "ref-a";
        referenceA.embeddingVector = [1.0f, 0.0f];
        referenceA.translationValues["ar"] = new() { text = "A ar", isVerified = true, status = "verified" };
        referenceA.translationValues["zh-hans"] = new() { text = "A zh", isVerified = true, status = "verified" };

        var referenceB = TestTranslations.Entry("Reference_B", "reference b");
        referenceB.modId = "ref-b";
        referenceB.embeddingVector = [0.0f, 1.0f];
        referenceB.translationValues["ar"] = new() { text = "B ar", isVerified = true, status = "verified" };
        referenceB.translationValues["zh-hans"] = new() { text = "B zh", isVerified = true, status = "verified" };

        var batches = new List<TranslationBatch>
        {
            new()
            {
                batchId = 1,
                modId = "1",
                translationEntries = [query]
            }
        };
        var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);
        var refEntries = new Dictionary<string, TranslationEntry>
        {
            ["ref-a::Reference_A"] = referenceA,
            ["ref-b::Reference_B"] = referenceB
        };
        var entries = new Dictionary<string, TranslationEntry> { ["1::Query_Key"] = query };
        var service = new RagContextRetrieverService(config);

        config.priorityLanguage = "ar";
        Assert.True((await service.RetrieveContextsAsync(refEntries, entries, batches, ragContextByEntryKey)).isSuccess);
        Assert.Equal("A ar", ragContextByEntryKey["1::Query_Key"][0]["translation"]);

        referenceA.embeddingVector = [0.0f, 1.0f];
        referenceB.embeddingVector = [1.0f, 0.0f];

        config.priorityLanguage = "zh-hans";
        Assert.True((await service.RetrieveContextsAsync(refEntries, entries, batches, ragContextByEntryKey)).isSuccess);
        Assert.Equal("B zh", ragContextByEntryKey["1::Query_Key"][0]["translation"]);
    }

    [Fact]
    /// <summary>An entry should not return its own translation as a RAG context match.</summary>
    public async Task RetrieveContexts_ShouldNotReturnCurrentEntryAsRagContext()
    {
        var config = TestConfig.Create();
        config.ragSimilarityThreshold = 0.1f;
        var query = TestTranslations.Entry("Query_Key", "query");
        query.embeddingVector = [1.0f, 0.0f];
        query.translationValues["zh-hans"] = new() { text = "", isVerified = true, status = "verified" };
        var batches = new List<TranslationBatch>
        {
            new()
            {
                batchId = 1,
                modId = "1",
                translationEntries = [query]
            }
        };
        var ragContextByEntryKey = new Dictionary<string, List<Dictionary<string, object?>>>(StringComparer.Ordinal);

        var result = await new RagContextRetrieverService(config).RetrieveContextsAsync(
            [],
            new Dictionary<string, TranslationEntry> { ["1::Query_Key"] = query },
            batches,
            ragContextByEntryKey);

        Assert.True(result.isSuccess);
        Assert.False(ragContextByEntryKey.ContainsKey("1::Query_Key"));
    }
}
