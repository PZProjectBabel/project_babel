using Common;
using ContentExtractor;
using PercentNormalizer;
using RepoDataLoader;
using ResultWriter;

namespace TranslationPipeline.Tests;

/// <summary>
/// Tests for the permanent pipeline PercentNormalizer (PZ Build 42.20.1+ canonical format).
/// </summary>
public class PercentNormalizerPipelineTests
{
    [Theory]
    [InlineData("100%", "100%%")]
    [InlineData("100%%", "100%%")]
    [InlineData("50% chance", "50%% chance")]
    [InlineData("50%% chance", "50%% chance")]
    [InlineData("%1", "%1")]
    [InlineData("%1%", "%1%%")]
    [InlineData("%1%%", "%1%%")]
    [InlineData("%2%", "%2%%")]
    [InlineData("%1 killed %2%", "%1 killed %2%%")]
    [InlineData("%d", "%d")]
    [InlineData("%s", "%s")]
    [InlineData("%1$s", "%1$s")]
    [InlineData("%d%", "%d%%")]
    [InlineData("%10s", "%10s")]
    [InlineData("%.2f", "%.2f")]
    [InlineData("%%", "%%")]
    [InlineData("%%%", "%%%%")]
    [InlineData("%%%%", "%%%%")]
    [InlineData("100%%%%", "100%%%%")]
    public void Normalize_SpecTable(string input, string expected)
    {
        Assert.Equal(expected, PercentNormalizerService.Normalize(input));
    }

    [Theory]
    [InlineData("100%")]
    [InlineData("100%%")]
    [InlineData("%1%")]
    [InlineData("%1 killed %2%")]
    [InlineData("%d%")]
    [InlineData("%%%")]
    [InlineData("x%%y%z")]
    public void Normalize_IsIdempotent(string input)
    {
        var once = PercentNormalizerService.Normalize(input);
        Assert.Equal(once, PercentNormalizerService.Normalize(once));
    }

    [Fact]
    public void Normalize_DoesNotUseNaiveGlobalReplace()
    {
        Assert.Equal("%%%%", PercentNormalizerService.Normalize("%%%%"));
        Assert.Equal("100%%", PercentNormalizerService.Normalize("100%%"));
        Assert.Equal("%1%%", PercentNormalizerService.Normalize("%1%"));
    }
}

/// <summary>
/// Pipeline integration: text extracted by ContentExtractor must already be canonical
/// (Extract → PercentNormalizer → TranslationEntry).
/// </summary>
public class ContentExtractorPercentNormalizerTests
{
    [Fact]
    public async Task ExtractContents_CanonicalizesPercentTextBeforeEntry()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var enDir = Path.Combine(tempDir, "media", "lua", "shared", "Translate", "EN");
        var outDir = Path.Combine(tempDir, "out");
        var runDir = Path.Combine(tempDir, "run");
        Directory.CreateDirectory(enDir);

        await Utf8NoBom.WriteAllTextAsync(Path.Combine(enDir, "IG_UI_EN.json"), """
        {
          "UI_Percent_Old": "Progress 100%",
          "UI_Percent_New": "Progress 100%%",
          "UI_Percent_Placeholder": "%1%",
          "UI_Percent_Format": "%d%",
          "UI_Percent_Chance": "50% chance"
        }
        """);

        var config = TestConfig.Create();
        config.extractedContentsTempDir = outDir;
        config.runTempDir = runDir;
        var service = new ContentExtractorService(config);
        var modInfoDict = new Dictionary<string, ModInfo>
        {
            ["1"] = new() { modId = "1", modName = "Test", localDownloadedPath = tempDir }
        };
        var entries = new Dictionary<string, TranslationEntry>();

        try
        {
            var result = await service.ExtractContentsAsync(modInfoDict, entries, "testbatch");
            Assert.True(result.isSuccess);

            // "100%" must become canonical "100%%"; already-canonical "100%%" must not change.
            Assert.Equal("Progress 100%%", entries["1::UI_Percent_Old"].translationValues["en"].text);
            Assert.Equal("Progress 100%%", entries["1::UI_Percent_New"].translationValues["en"].text);
            Assert.Equal("%1%%", entries["1::UI_Percent_Placeholder"].translationValues["en"].text);
            Assert.Equal("%d%%", entries["1::UI_Percent_Format"].translationValues["en"].text);
            Assert.Equal("50%% chance", entries["1::UI_Percent_Chance"].translationValues["en"].text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}

/// <summary>
/// The translation file codec must escape/unescape CR/LF/TAB so multi-line values
/// survive the txt round-trip as single lines (regression for the raw-newline bug).
/// </summary>
public class NewlineEscapingRoundTripTests
{
    [Fact]
    public async Task WriteResults_ShouldEscapeNewlinesAndRoundTrip()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(config.dataDir);

            var original = "Line one\nLine two\tTabbed\rCR\n\nTrailing paragraph";
            var entry = TestTranslations.Entry("Key_Multi", original);
            var entries = new Dictionary<string, TranslationEntry> { ["1::Key_Multi"] = entry };

            Assert.True((await new ResultWriterService(config).WriteResultsAsync([], [], entries, "en")).isSuccess);

            var filePath = Path.Combine(config.dataDir, "translations", "en", "1.txt");
            var text = Utf8NoBom.ReadAllText(filePath);
            // Must be a single logical line with \n escaped, never a raw multi-line value.
            Assert.Single(text.TrimEnd().Split('\n'));
            Assert.Contains("Key_Multi::en = \"Line one\\nLine two\\tTabbed\\rCR\\n\\nTrailing paragraph\",", text);

            // The pipeline loader must read the exact original text back.
            var loader = new RepoDataLoaderService(config);
            var loaded = new Dictionary<string, TranslationEntry>();
            loader.LoadTranslationCache(loaded);
            Assert.True(loaded.TryGetValue("1::Key_Multi", out var loadedEntry));
            Assert.Equal(original, loadedEntry.translationValues["en"].text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ParseTranslationLine_ShouldUnescapeControlSequences()
    {
        // Direct codec check through the loader path (lines written by the fixed writer).
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(Path.Combine(config.dataDir, "translations", "en"));
            Utf8NoBom.WriteAllText(
                Path.Combine(config.dataDir, "translations", "en", "7.txt"),
                "UI_A::en = \"a\\nb\\tc\\rd\\\\e\\\"f\",\n");

            var loader = new RepoDataLoaderService(config);
            var loaded = new Dictionary<string, TranslationEntry>();
            loader.LoadTranslationCache(loaded);
            Assert.Equal("a\nb\tc\rd\\e\"f", loaded["7::UI_A"].translationValues["en"].text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ParseTranslationLine_ShouldHandleValueContainingEqualsQuote()
    {
        // A value may itself contain ' = "'; the first ' = "' is still the separator.
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var config = TestConfig.Create();
            TestConfig.ConfigureTempFolders(config, tempDir);
            config.dataDir = Path.Combine(tempDir, "data");
            Directory.CreateDirectory(Path.Combine(config.dataDir, "translations", "en"));
            Utf8NoBom.WriteAllText(
                Path.Combine(config.dataDir, "translations", "en", "7.txt"),
                "UI_State::en = \"腐坏血反应: 状态 = \",\n");

            var loader = new RepoDataLoaderService(config);
            var loaded = new Dictionary<string, TranslationEntry>();
            loader.LoadTranslationCache(loaded);
            Assert.True(loaded.ContainsKey("7::UI_State"));
            Assert.Equal("腐坏血反应: 状态 = ", loaded["7::UI_State"].translationValues["en"].text);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
