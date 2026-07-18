using System.Text.Json;
using Common;
using WorkshopMonitor;

namespace TranslationPipeline.Tests;

public class WorkshopMonitorTests
{
    // ── ParseIds ──

    [Fact]
    public void ParseIds_EmptyHtml_ReturnsEmpty()
    {
        var ids = WorkshopMonitorService.ParseIds("<html></html>");
        Assert.Empty(ids);
    }

    [Fact]
    public void ParseIds_ValidModLinks_ExtractsIds()
    {
        var html = """
            <html><body>
            <div class="aspectratio_square"><a href="https://steamcommunity.com/sharedfiles/filedetails/?id=1234567890"></a></div>
            <div class="aspectratio_square"><a href="https://steamcommunity.com/sharedfiles/filedetails/?id=9876543210"></a></div>
            </body></html>
            """;
        var ids = WorkshopMonitorService.ParseIds(html);
        Assert.Equal(2, ids.Count);
        Assert.Contains("1234567890", ids);
        Assert.Contains("9876543210", ids);
    }

    [Fact]
    public void ParseIds_NoModDivs_ReturnsEmpty()
    {
        var html = "<html><body><div class='other'>no match</div></body></html>";
        var ids = WorkshopMonitorService.ParseIds(html);
        Assert.Empty(ids);
    }

    // ── Cache I/O ──

    [Fact]
    public void LoadCache_NoFile_ReturnsDefault()
    {
        var (last, cache) = WorkshopMonitorService.LoadCache("/nonexistent/path/cache.bin");
        Assert.Empty(cache);
        Assert.True((DateTime.UtcNow - last).TotalHours < 49);
    }

    [Fact]
    public void SaveAndLoadCache_RoundTrip()
    {
        var tmp = Path.GetTempFileName();
        try
        {
            var orig = new Dictionary<string, long> { ["123"] = 1700000000, ["456"] = 1700000100 };
            WorkshopMonitorService.SaveCache(tmp, new DateTime(2026, 7, 19, 0, 0, 0, DateTimeKind.Utc), orig);

            var (last, loaded) = WorkshopMonitorService.LoadCache(tmp);
            Assert.Equal(new DateTime(2026, 7, 19, 0, 0, 0, DateTimeKind.Utc), last);
            Assert.Equal(2, loaded.Count);
            Assert.Equal(1700000000, loaded["123"]);
            Assert.Equal(1700000100, loaded["456"]);
        }
        finally { File.Delete(tmp); }
    }

    [Fact]
    public void LoadCache_EmptyFile_ReturnsDefault()
    {
        var tmp = Path.GetTempFileName();
        try
        {
            File.WriteAllBytes(tmp, []);
            var (last, cache) = WorkshopMonitorService.LoadCache(tmp);
            Assert.Empty(cache);
            Assert.True((DateTime.UtcNow - last).TotalHours < 49);
        }
        finally { File.Delete(tmp); }
    }

    // ── MergeRequestFile ──

    [Fact]
    public void MergeRequestFile_NewFile_CreatesWithAllIds()
    {
        var tmp = Path.GetTempFileName();
        File.Delete(tmp);
        try
        {
            WorkshopMonitorService.MergeRequestFile(tmp, new HashSet<string> { "111", "222" });
            var lines = Utf8NoBom.ReadAllLines(tmp);
            Assert.Equal(2, lines.Length);
            Assert.Contains("111", lines);
            Assert.Contains("222", lines);
        }
        finally { if (File.Exists(tmp)) File.Delete(tmp); }
    }

    [Fact]
    public void MergeRequestFile_ExistingFile_Dedup()
    {
        var tmp = Path.GetTempFileName();
        try
        {
            Utf8NoBom.WriteAllText(tmp, "111\n222\n");
            WorkshopMonitorService.MergeRequestFile(tmp, new HashSet<string> { "222", "333" });
            var lines = Utf8NoBom.ReadAllLines(tmp);
            Assert.Equal(3, lines.Length);
            Assert.Contains("111", lines);
            Assert.Contains("222", lines);
            Assert.Contains("333", lines);
            Assert.Equal(1, lines.Count(l => l == "222"));
        }
        finally { File.Delete(tmp); }
    }

    // ── JSON helpers ──

    [Fact]
    public void Str_ExistingString_ReturnsValue()
    {
        using var doc = JsonDocument.Parse("""{"key":"value"}""");
        Assert.Equal("value", WorkshopMonitorService.Str(doc.RootElement, "key"));
    }

    [Fact]
    public void Str_Missing_ReturnsNull()
    {
        using var doc = JsonDocument.Parse("""{}""");
        Assert.Null(WorkshopMonitorService.Str(doc.RootElement, "key"));
    }

    [Fact]
    public void Int_Number_ReturnsValue()
    {
        using var doc = JsonDocument.Parse("""{"count":42}""");
        Assert.Equal(42, WorkshopMonitorService.Int(doc.RootElement, "count"));
    }

    [Fact]
    public void Int_StringNumber_ReturnsParsed()
    {
        using var doc = JsonDocument.Parse("""{"count":"42"}""");
        Assert.Equal(42, WorkshopMonitorService.Int(doc.RootElement, "count"));
    }

    [Fact]
    public void Long_Number_ReturnsValue()
    {
        using var doc = JsonDocument.Parse("""{"ts":1700000000}""");
        Assert.Equal(1700000000L, WorkshopMonitorService.Long(doc.RootElement, "ts"));
    }

    // ── HttpClient ──

    [Fact]
    public void NewClient_HasExpectedHeaders()
    {
        var client = WorkshopMonitorService.NewClient();
        Assert.True(client.DefaultRequestHeaders.Contains("User-Agent"));
        Assert.Equal(TimeSpan.FromSeconds(30), client.Timeout);
    }

    // ── ReadSteamApiKey ──

    [Fact]
    public void ReadSteamApiKey_SecretsFile_ReturnsKey()
    {
        var tmpDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tmpDir);
        try
        {
            Utf8NoBom.WriteAllText(Path.Combine(tmpDir, "secrets.json"),
                """{"STEAM_KEY":"test-key-123"}""");
            var key = Program.ReadSteamApiKey(tmpDir);
            Assert.Equal("test-key-123", key);
        }
        finally { Directory.Delete(tmpDir, true); }
    }

    [Fact]
    public void ReadSteamApiKey_EnvVar_ReturnsKey()
    {
        try
        {
            Environment.SetEnvironmentVariable("STEAM_KEY", "env-key-456");
            var tmpDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tmpDir);
            try
            {
                var key = Program.ReadSteamApiKey(tmpDir);
                Assert.Equal("env-key-456", key);
            }
            finally { Directory.Delete(tmpDir, true); }
        }
        finally { Environment.SetEnvironmentVariable("STEAM_KEY", null); }
    }

    [Fact]
    public void ReadSteamApiKey_NoSource_Throws()
    {
        var tmpDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tmpDir);
        try
        {
            Assert.Throws<InvalidOperationException>(() => Program.ReadSteamApiKey(tmpDir));
        }
        finally { Directory.Delete(tmpDir, true); }
    }
}
