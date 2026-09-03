using Common;
using ContentExtractor;
using EmbeddingFetcher;
using FinalOutputWriter;
using LLMTranslator;
using RepoDataLoader;
using ResultWriter;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using TranslationBatcher;

namespace TranslationPipeline.Tests;

/// <summary>Focused coverage for output-file routing metadata and its cache merge.</summary>
public sealed class OutputRoutingTests
{
    [Fact]
    public async Task ExtractContents_ShouldPersistWinningOutputFileStem()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var txt42Dir = Path.Combine(tempDir, "mods", "SubMod", "42", "media", "lua", "shared", "Translate", "EN");
        var txt4213Dir = Path.Combine(tempDir, "mods", "SubMod", "42.13", "media", "lua", "shared", "Translate", "EN");
        var outDir = Path.Combine(tempDir, "out");
        var runDir = Path.Combine(tempDir, "run");
        Directory.CreateDirectory(txt42Dir);
        Directory.CreateDirectory(txt4213Dir);

        await Utf8NoBom.WriteAllTextAsync(Path.Combine(txt42Dir, "Old_EN.txt"), """
        UI_EN = {
            Base.AKM = "txt 42",
            Base.MP5A2_10mm = "mp5 42",
        }
        """);
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(txt4213Dir, "New_EN.txt"), """
        UI_EN = {
            Base.MP5A2_10mm = "mp5 42.13",
        }
        """);
        await Utf8NoBom.WriteAllTextAsync(Path.Combine(txt4213Dir, "Tooltip_EN.txt"), """
        UI_EN = {
            Tooltip_Foo = "tooltip",
        }
        """);
        await Utf8NoBom.WriteAllTextAsync(
            Path.Combine(txt4213Dir, "Json_EN.json"),
            """{"Base.AKM":"json wins"}""");

        var config = TestConfig.Create();
        config.extractedContentsTempDir = outDir;
        config.runTempDir = runDir;
        var entries = new Dictionary<string, TranslationEntry>();
        var mods = new Dictionary<string, ModInfo>
        {
            ["fixture-route-001"] = new()
            {
                modId = "fixture-route-001",
                modName = "Output routing fixture",
                localDownloadedPath = tempDir
            }
        };

        try
        {
            var result = await new ContentExtractorService(config).ExtractContentsAsync(mods, entries, "routing");

            Assert.True(result.isSuccess);
            Assert.Equal("json wins", entries["fixture-route-001::Base.AKM"].translationValues["en"].text);
            Assert.Equal("Json", entries["fixture-route-001::Base.AKM"].outputFileStem);
            Assert.Equal("mp5 42.13", entries["fixture-route-001::Base.MP5A2_10mm"].translationValues["en"].text);
            Assert.Equal("New", entries["fixture-route-001::Base.MP5A2_10mm"].outputFileStem);
            Assert.Equal("Tooltip", entries["fixture-route-001::Tooltip_Foo"].outputFileStem);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task EntryMetadata_ShouldRoundTripOutputFileStem_AndAcceptLegacyRows()
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
            entry.sourceHash = "source-hash";
            entry.outputFileStem = "WinningFile";
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = entry };

            Assert.True((await new ResultWriterService(config).WriteDataAsync([], entries, [], [])).isSuccess);
            var metadataPath = Path.Combine(config.dataDir, "entry_metadata", "1.json");
            using (var doc = JsonDocument.Parse(Utf8NoBom.ReadAllText(metadataPath)))
            {
                var row = doc.RootElement.EnumerateArray().Single();
                Assert.Equal("WinningFile", row.GetProperty("output_file_stem").GetString());
            }

            var loaded = TestTranslations.Entry("Key_0", "Hello");
            var loadedDict = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = loaded };
            new RepoDataLoaderService(config).LoadEntryMetadataCache(loadedDict);
            Assert.Equal("WinningFile", loaded.outputFileStem);
            Assert.Equal("source-hash", loaded.sourceHash);

            // A pre-route metadata row must remain readable and leave the optional
            // route empty so fresh extraction can backfill it later.
            Utf8NoBom.WriteAllText(metadataPath, """
            [{"mod_id":"1","translation_key":"Key_0","source_hash":"legacy-hash"}]
            """);
            var legacy = TestTranslations.Entry("Key_0", "Hello");
            var legacyDict = new Dictionary<string, TranslationEntry> { ["1::Key_0"] = legacy };
            new RepoDataLoaderService(config).LoadEntryMetadataCache(legacyDict);
            Assert.Equal("", legacy.outputFileStem);
            Assert.Equal("legacy-hash", legacy.sourceHash);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task FinalOutput_ShouldPreferExplicitRouteAndRejectUnsafeRoute()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var baseKeysDir = Path.Combine(tempDir, "base_game_keys");
        Directory.CreateDirectory(baseKeysDir);
        Utf8NoBom.WriteAllText(Path.Combine(baseKeysDir, "Tooltip.json"), """{"Tooltip_Base":"base"}""");

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            var entries = new Dictionary<string, TranslationEntry>
            {
                ["1::Base.AKM"] = TargetEntry("Base.AKM", "AKM", "Weapons"),
                ["1::Base.Dot"] = TargetEntry("Base.Dot", "dot stem", "Base.AKM"),
                ["1::Tooltip_Fallback"] = TargetEntry("Tooltip_Fallback", "fallback", ""),
                ["1::Tooltip_Explicit"] = TargetEntry("Tooltip_Explicit", "explicit", "Explicit"),
                ["1::Base.MP5A2_10mm"] = TargetEntry("Base.MP5A2_10mm", "unsafe", "../escape"),
                ["1::Tooltip_Base"] = TargetEntry("Tooltip_Base", "excluded", "ShouldNotWrite"),
                ["1::NoRoute"] = TargetEntry("NoRoute", "missing", "")
            };

            var result = await new FinalOutputWriterService(config).WriteFinalOutputAsync(
                entries,
                new Dictionary<string, ModInfo>(),
                [new LangInfoData { ingameCode = "CN", isoCode = "zh-hans" }]);
            Assert.True(result.isSuccess);

            var outputDir = Path.Combine(
                tempDir, "final_outputs", "project_babel", "contents", "mods", "project_babel",
                "42.20", "media", "lua", "shared", "Translate", "CN");
            var weapons = JsonDocument.Parse(Utf8NoBom.ReadAllText(Path.Combine(outputDir, "Weapons.json"))).RootElement;
            Assert.Equal("AKM", weapons.GetProperty("Base.AKM").GetString());
            var dottedStem = JsonDocument.Parse(Utf8NoBom.ReadAllText(Path.Combine(outputDir, "Base.AKM.json"))).RootElement;
            Assert.Equal("dot stem", dottedStem.GetProperty("Base.Dot").GetString());

            var tooltip = JsonDocument.Parse(Utf8NoBom.ReadAllText(Path.Combine(outputDir, "Tooltip.json"))).RootElement;
            Assert.Equal("fallback", tooltip.GetProperty("Tooltip_Fallback").GetString());
            Assert.DoesNotContain("Tooltip_Explicit", tooltip.EnumerateObject().Select(p => p.Name));
            Assert.DoesNotContain("Tooltip_Base", tooltip.EnumerateObject().Select(p => p.Name));

            var explicitOutput = JsonDocument.Parse(Utf8NoBom.ReadAllText(Path.Combine(outputDir, "Explicit.json"))).RootElement;
            Assert.Equal("explicit", explicitOutput.GetProperty("Tooltip_Explicit").GetString());
            Assert.False(File.Exists(Path.Combine(outputDir, "escape.json")));
            Assert.False(File.Exists(Path.Combine(tempDir, "escape.json")));
            Assert.False(File.Exists(Path.Combine(outputDir, "NoRoute.json")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task FinalOutput_ShouldNeverWriteNameFieldToModJson()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(tempDir, "base_game_keys"));

        var entries = new Dictionary<string, TranslationEntry>
        {
            ["fixture::name"] = TargetEntry("name", "must not be emitted", "Mod"),
            ["fixture::Mod_Fixture_name"] = TargetEntry("Mod_Fixture_name", "must remain", "Mod")
        };

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            var result = await new FinalOutputWriterService(config).WriteFinalOutputAsync(
                entries,
                new Dictionary<string, ModInfo>(),
                [new LangInfoData { ingameCode = "CN", isoCode = "zh-hans" }]);
            Assert.True(result.isSuccess);

            foreach (var versionDir in new[] { "42.20", "42" })
            {
                var outputPath = Path.Combine(
                    tempDir, "final_outputs", "project_babel", "contents", "mods", "project_babel",
                    versionDir, "media", "lua", "shared", "Translate", "CN", "Mod.json");
                using var document = JsonDocument.Parse(Utf8NoBom.ReadAllText(outputPath));
                var root = document.RootElement;
                Assert.DoesNotContain(root.EnumerateObject(), property =>
                    string.Equals(property.Name, "name", StringComparison.OrdinalIgnoreCase));
                Assert.Equal("must remain", root.GetProperty("Mod_Fixture_name").GetString());
            }
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task OutputFileStemValidation_ShouldBeStableAcrossPlatforms()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(tempDir, "base_game_keys"));
        var invalidStems = new[]
        {
            "../escape",
            "..\\escape",
            "C:\\escape",
            "bad|name",
            "bad:name",
            "bad*name",
            "bad?name",
            "bad<name",
            "bad>name",
            "bad\0name",
            "route.json",
            ".",
            "..",
            "foo..bar",
            "CON",
            "路径",
            new string('a', 129)
        };

        var entries = new Dictionary<string, TranslationEntry>
        {
            ["fixture::Valid_Weapons"] = TargetEntry("Valid_Weapons", "weapons", "Weapons"),
            ["fixture::Valid_Dotted"] = TargetEntry("Valid_Dotted", "dotted", "Base.AKM"),
            ["fixture::Valid_Map"] = TargetEntry("Valid_Map", "map", "Brandenburg, KY")
        };
        for (var index = 0; index < invalidStems.Length; index++)
        {
            var key = $"Invalid_{index}";
            entries[$"fixture::{key}"] = TargetEntry(key, $"invalid-{index}", invalidStems[index]);
        }

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            var result = await new FinalOutputWriterService(config).WriteFinalOutputAsync(
                entries,
                new Dictionary<string, ModInfo>(),
                [new LangInfoData { ingameCode = "CN", isoCode = "zh-hans" }]);
            Assert.True(result.isSuccess);

            var outputDir = Path.Combine(
                tempDir, "final_outputs", "project_babel", "contents", "mods", "project_babel",
                "42.20", "media", "lua", "shared", "Translate", "CN");
            var outputFiles = Directory.GetFiles(outputDir, "*.json")
                .Select(path => Path.GetFileName(path)!)
                .ToHashSet(StringComparer.Ordinal);
            Assert.Equal(
                new HashSet<string>(["Weapons.json", "Base.AKM.json", "Brandenburg, KY.json"], StringComparer.Ordinal),
                outputFiles);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task FinalOutput_ShouldMergeCaseInsensitiveRoutesWithStableCanonicalNameAndDiagnostics()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var baseKeysDir = Path.Combine(tempDir, "base_game_keys");
        Directory.CreateDirectory(baseKeysDir);
        Utf8NoBom.WriteAllText(Path.Combine(baseKeysDir, "ItemName.json"), "{\"Base.Game\":\"game\"}");

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            var sharedWinner = TargetEntry("Base.Shared", "winner", "Itemname");
            sharedWinner.modId = "2";
            var sharedLoser = TargetEntry("Base.Shared", "loser", "ItemName");
            sharedLoser.modId = "3";
            var entries = new Dictionary<string, TranslationEntry>
            {
                // Deliberately use all three casing variants and a non-sorted
                // insertion order. ItemName.json is the known canonical name.
                ["3::Base.CaseThree"] = TargetEntry("Base.CaseThree", "three", "ITEMNAME"),
                ["2::Base.Shared"] = sharedWinner,
                ["1::Base.CaseOne"] = TargetEntry("Base.CaseOne", "one", "ItemName"),
                ["3::Base.Shared"] = sharedLoser
            };

            var outputDir = Path.Combine(
                tempDir, "final_outputs", "project_babel", "contents", "mods", "project_babel",
                "42.20", "media", "lua", "shared", "Translate", "CN");
            Directory.CreateDirectory(outputDir);
            // A previous run on Linux could have left the lower-case spelling;
            // the writer must remove it when emitting the canonical path.
            Utf8NoBom.WriteAllText(Path.Combine(outputDir, "Itemname.json"), "{\"stale\":\"value\"}");

            var originalError = Console.Error;
            using var errorCapture = new StringWriter();
            try
            {
                Console.SetError(errorCapture);
                var result = await new FinalOutputWriterService(config).WriteFinalOutputAsync(
                    entries,
                    new Dictionary<string, ModInfo>(),
                    [new LangInfoData { ingameCode = "CN", isoCode = "zh-hans" }]);
                Assert.True(result.isSuccess);
            }
            finally
            {
                Console.SetError(originalError);
            }

            var outputFiles = Directory.GetFiles(outputDir, "*.json")
                .Select(path => Path.GetFileName(path)!)
                .ToArray();
            Assert.Equal(["ItemName.json"], outputFiles);

            using var itemName = JsonDocument.Parse(Utf8NoBom.ReadAllText(Path.Combine(outputDir, "ItemName.json")));
            var root = itemName.RootElement;
            Assert.Equal("one", root.GetProperty("Base.CaseOne").GetString());
            Assert.Equal("three", root.GetProperty("Base.CaseThree").GetString());
            Assert.Equal("winner", root.GetProperty("Base.Shared").GetString());
            Assert.Equal(3, root.EnumerateObject().Count());

            var output42 = Path.Combine(
                tempDir, "final_outputs", "project_babel", "contents", "mods", "project_babel",
                "42", "media", "lua", "shared", "Translate", "CN", "ItemName.json");
            Assert.Equal(File.ReadAllBytes(Path.Combine(outputDir, "ItemName.json")), File.ReadAllBytes(output42));

            var warning = errorCapture.ToString();
            Assert.Contains("Output translation value conflicts detected", warning);
            Assert.Contains("Base.Shared", warning);
            Assert.Contains("2::Base.Shared", warning);
            Assert.Contains("3::Base.Shared", warning);
            Assert.Contains("selected=2::Base.Shared", warning);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task FinalOutput_ShouldSummarizeUnroutableEntriesWithBoundedExamples()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(tempDir, "base_game_keys"));

        var entries = Enumerable.Range(0, 12)
            .ToDictionary(
                index => $"fixture::NoRoute_{index}",
                index => TargetEntry($"NoRoute_{index}", $"translated-{index}", "bad|stem"));

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            var originalError = Console.Error;
            using var errorCapture = new StringWriter();
            try
            {
                Console.SetError(errorCapture);
                var result = await new FinalOutputWriterService(config).WriteFinalOutputAsync(
                    entries,
                    new Dictionary<string, ModInfo>(),
                    [new LangInfoData { ingameCode = "CN", isoCode = "zh-hans" }]);
                Assert.True(result.isSuccess);
            }
            finally
            {
                Console.SetError(originalError);
            }

            var warning = errorCapture.ToString();
            Assert.Contains("No valid output file route for 12 translated entries", warning);
            for (var index = 0; index < 5; index++)
                Assert.Contains($"1::NoRoute_{index}", warning);
            for (var index = 5; index < 12; index++)
                Assert.DoesNotContain($"1::NoRoute_{index}", warning);
            Assert.Contains("and 7 more", warning);
            Assert.Equal(1, warning.Split("No valid output file route", StringSplitOptions.None).Length - 1);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task RouteOnlyBackfill_ShouldNotAddTranslationOrEmbeddingWork()
    {
        var config = TestConfig.Create();
        config.baseLanguage = "en";
        config.priorityLanguage = "zh-hans";

        var cached = TestTranslations.Entry("Base.AKM", "Hello");
        cached.translationValues["zh-hans"] = new TranslationData
        {
            text = "你好",
            isVerified = true,
            status = "verified",
            processStatus = "processed"
        };
        cached.sourceHash = RepoDataLoaderService.ComputeSourceHash(cached, "en");
        var embeddingHash = ComputeNormalEmbeddingHash(cached);
        cached.embeddingHash = embeddingHash;
        cached.embeddingSourceKind = "normal_base_text";
        cached.embeddingVector = [1.0f, 2.0f];
        cached.embeddingValues["normal_base_text"] = new TranslationEmbedding
        {
            sourceKind = "normal_base_text",
            hash = embeddingHash,
            vector = [1.0f, 2.0f]
        };

        var fresh = TestTranslations.Entry("Base.AKM", "Hello");
        fresh.outputFileStem = "Weapons";
        var cachedEntries = new Dictionary<string, TranslationEntry> { ["1::Base.AKM"] = cached };
        var freshEntries = new Dictionary<string, TranslationEntry> { ["1::Base.AKM"] = fresh };
        var mods = new Dictionary<string, ModInfo>
        {
            ["1"] = new() { modId = "1", contentCheckStatus = ContentCheckStatus.ACCEPTED }
        };

        var beforeQueue = PipelineRunner.CollectTargetWorkQueue(cachedEntries, ["zh-hans"], mods);
        var beforeBatches = new List<TranslationBatch>();
        await new TranslationBatcherService(config).CreateBatchesAsync(mods, beforeQueue, beforeBatches);
        var beforeSourceHash = cached.sourceHash;
        var beforeEmbeddingHash = cached.embeddingHash;
        var beforeEmbedding = cached.embeddingVector.ToArray();

        var diff = RepoDataLoaderService.DiffTranslationEntries(
            freshEntries,
            cachedEntries,
            "en",
            mods,
            new HashSet<string>(["zh-hans"], StringComparer.OrdinalIgnoreCase));
        Assert.Empty(diff);
        PipelineRunner.BackfillOutputRoute(cached, fresh);

        var afterQueue = PipelineRunner.CollectTargetWorkQueue(cachedEntries, ["zh-hans"], mods);
        var afterBatches = new List<TranslationBatch>();
        await new TranslationBatcherService(config).CreateBatchesAsync(mods, afterQueue, afterBatches);
        Assert.Empty(beforeQueue);
        Assert.Empty(afterQueue);
        Assert.Empty(beforeBatches);
        Assert.Empty(afterBatches);
        Assert.Equal("Weapons", cached.outputFileStem);
        Assert.Equal(beforeSourceHash, cached.sourceHash);
        Assert.Equal(beforeEmbeddingHash, cached.embeddingHash);
        Assert.Equal(beforeEmbedding, cached.embeddingVector);

        // A fail-fast sentinel proves that route-only state does not reach either
        // external provider when the cache already satisfies the work predicates.
        var embeddingSentinel = new FailFastHttpHandler();
        using (var embeddingClient = new HttpClient(embeddingSentinel))
        {
            var result = await new EmbeddingFetcherService(config, embeddingClient).FetchEmbeddingsAsync(
                mods,
                diff,
                cachedEntries,
                new Dictionary<string, ModInfo>(),
                new Dictionary<string, TranslationEntry>());
            Assert.True(result.isSuccess);
        }
        Assert.Equal(0, embeddingSentinel.RequestCount);

        var translatorSentinel = new FailFastHttpHandler();
        using (var translatorClient = new HttpClient(translatorSentinel))
        {
            var translator = new LLMTranslatorService(config, translatorClient);
            var plan = await translator.PrepareTranslationPlanAsync(
                mods,
                afterBatches,
                new Dictionary<string, List<Dictionary<string, object?>>>(),
                "zh-hans");
            Assert.Equal(0, plan.RequestCount);
            await translator.ExecuteTranslationPlansAsync([plan]);
        }
        Assert.Equal(0, translatorSentinel.RequestCount);
    }

    [Fact]
    public async Task RouteOnlyBackfill_ShouldNotAddWorkRelativeToSameSnapshotBaseline()
    {
        var config = TestConfig.Create();
        config.baseLanguage = "en";
        config.priorityLanguage = "zh-hans";
        config.embeddingKey = ""; // keep capture request bodies deterministic

        // A and B use independently-created copies of the same cached/fresh
        // snapshot. B differs only by the new fresh output route. The natural
        // source update is intentionally retained so the assertion compares
        // work sets rather than assuming that every queue is empty.
        var baseline = await RunSnapshotAsync(BuildSnapshot(), config, applyRoute: false);
        var patched = await RunSnapshotAsync(BuildSnapshot(), config, applyRoute: true);
        var baselineWork = BuildWorkFingerprints(baseline);
        var patchedWork = BuildWorkFingerprints(patched);

        Assert.Empty(patchedWork.Except(baselineWork));
        Assert.Equal(baselineWork.OrderBy(item => item), patchedWork.OrderBy(item => item));
        Assert.Contains(baselineWork, item => item.Contains("Natural_Key", StringComparison.Ordinal));
        Assert.DoesNotContain(patchedWork, item => item.Contains("Base.AKM", StringComparison.Ordinal));
        Assert.Equal(
            baseline.Queue.Keys.OrderBy(key => key),
            patched.Queue.Keys.OrderBy(key => key));
        Assert.Equal(baseline.Batches.Count, patched.Batches.Count);

        // The natural update is allowed to produce one embedding request, but
        // route metadata must not create an additional provider call or change
        // the request body set.
        var baselineEmbeddingHandler = new CaptureEmbeddingHandler();
        var patchedEmbeddingHandler = new CaptureEmbeddingHandler();
        using (var baselineClient = new HttpClient(baselineEmbeddingHandler))
        using (var patchedClient = new HttpClient(patchedEmbeddingHandler))
        {
            await new EmbeddingFetcherService(config, baselineClient).FetchEmbeddingsAsync(
                baseline.Mods,
                baseline.Diff,
                baseline.Entries,
                new Dictionary<string, ModInfo>(),
                new Dictionary<string, TranslationEntry>());
            await new EmbeddingFetcherService(config, patchedClient).FetchEmbeddingsAsync(
                patched.Mods,
                patched.Diff,
                patched.Entries,
                new Dictionary<string, ModInfo>(),
                new Dictionary<string, TranslationEntry>());
        }
        Assert.Equal(baselineEmbeddingHandler.RequestCount, patchedEmbeddingHandler.RequestCount);
        Assert.Equal(baselineEmbeddingHandler.RequestBodies, patchedEmbeddingHandler.RequestBodies);
    }

    private static SnapshotFixture BuildSnapshot()
    {
        var cachedRoute = TestTranslations.Entry("Base.AKM", "Hello");
        cachedRoute.translationValues["zh-hans"] = new TranslationData
        {
            text = "你好",
            isVerified = true,
            status = "verified",
            processStatus = "processed"
        };
        cachedRoute.sourceHash = RepoDataLoaderService.ComputeSourceHash(cachedRoute, "en");
        var routeEmbeddingHash = ComputeNormalEmbeddingHash(cachedRoute);
        cachedRoute.embeddingHash = routeEmbeddingHash;
        cachedRoute.embeddingSourceKind = "normal_base_text";
        cachedRoute.embeddingVector = [1.0f, 2.0f];
        cachedRoute.embeddingValues["normal_base_text"] = new TranslationEmbedding
        {
            sourceKind = "normal_base_text",
            hash = routeEmbeddingHash,
            vector = [1.0f, 2.0f]
        };

        var cachedNatural = TestTranslations.Entry("Natural_Key", "Old");
        var freshRoute = TestTranslations.Entry("Base.AKM", "Hello");
        freshRoute.outputFileStem = "Weapons";
        var freshNatural = TestTranslations.Entry("Natural_Key", "New");
        freshNatural.outputFileStem = "Natural";

        return new SnapshotFixture(
            new Dictionary<string, TranslationEntry>
            {
                ["1::Base.AKM"] = cachedRoute,
                ["1::Natural_Key"] = cachedNatural
            },
            new Dictionary<string, TranslationEntry>
            {
                ["1::Base.AKM"] = freshRoute,
                ["1::Natural_Key"] = freshNatural
            },
            new Dictionary<string, ModInfo>
            {
                ["1"] = new() { modId = "1", contentCheckStatus = ContentCheckStatus.ACCEPTED }
            });
    }

    private static async Task<SnapshotResult> RunSnapshotAsync(
        SnapshotFixture snapshot,
        PipelineConfig config,
        bool applyRoute)
    {
        var diff = RepoDataLoaderService.DiffTranslationEntries(
            snapshot.Fresh,
            snapshot.Cached,
            "en",
            snapshot.Mods,
            new HashSet<string>(["zh-hans"], StringComparer.OrdinalIgnoreCase));
        var entries = new Dictionary<string, TranslationEntry>(snapshot.Cached, StringComparer.Ordinal);
        foreach (var (key, fresh) in snapshot.Fresh)
        {
            if (diff.TryGetValue(key, out var changed))
            {
                entries[key] = changed;
                continue;
            }

            var cached = snapshot.Cached[key];
            if (applyRoute)
                PipelineRunner.BackfillOutputRoute(cached, fresh);
            entries[key] = cached;
        }

        var queue = PipelineRunner.CollectTargetWorkQueue(entries, ["zh-hans"], snapshot.Mods);
        var batches = new List<TranslationBatch>();
        await new TranslationBatcherService(config).CreateBatchesAsync(snapshot.Mods, queue, batches);
        return new SnapshotResult(snapshot.Mods, entries, diff, queue, batches);
    }

    private static HashSet<string> BuildWorkFingerprints(SnapshotResult snapshot)
    {
        var work = new HashSet<string>(StringComparer.Ordinal);
        foreach (var entry in snapshot.Diff.Values)
        {
            work.Add($"{entry.modId}::{entry.translationKey}::{entry.sourceHash}::embedding");
        }

        foreach (var entry in snapshot.Queue.Values)
        {
            work.Add($"{entry.modId}::{entry.translationKey}::{entry.sourceHash}::translation:zh-hans");
        }

        return work;
    }

    private static TranslationEntry TargetEntry(string key, string text, string route)
    {
        var entry = TestTranslations.Entry(key, text, "zh-hans");
        entry.outputFileStem = route;
        return entry;
    }

    private static string ComputeNormalEmbeddingHash(TranslationEntry entry)
    {
        var input = $"{entry.modId}::{entry.translationKey} = \"{entry.GetBaseTextStrict("en").text}\"";
        var bytes = Utf8NoBom.Encoding.GetBytes($"normal_base_text::{input}");
        return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    }

    private sealed class FailFastHttpHandler : HttpMessageHandler
    {
        private int _requestCount;
        public int RequestCount => _requestCount;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _requestCount);
            throw new InvalidOperationException("External provider must not be called by a route-only change.");
        }
    }

    private sealed class CaptureEmbeddingHandler : HttpMessageHandler
    {
        private int _requestCount;
        public int RequestCount => _requestCount;
        public List<string> RequestBodies { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _requestCount);
            var body = request.Content == null
                ? ""
                : await request.Content.ReadAsStringAsync(cancellationToken);
            RequestBodies.Add(body);
            using var doc = JsonDocument.Parse(body);
            var count = doc.RootElement.GetProperty("input").GetArrayLength();
            var data = string.Join(",", Enumerable.Repeat("{\"embedding\":[1.0,0.0]}", count));
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    $"{{\"data\":[{data}]}}",
                    Utf8NoBom.Encoding,
                    "application/json")
            };
        }
    }

    private sealed record SnapshotFixture(
        Dictionary<string, TranslationEntry> Cached,
        Dictionary<string, TranslationEntry> Fresh,
        Dictionary<string, ModInfo> Mods);

    private sealed record SnapshotResult(
        Dictionary<string, ModInfo> Mods,
        Dictionary<string, TranslationEntry> Entries,
        Dictionary<string, TranslationEntry> Diff,
        Dictionary<string, TranslationEntry> Queue,
        List<TranslationBatch> Batches);
}
