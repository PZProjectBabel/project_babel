using System.Buffers.Binary;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Common;
using HtmlAgilityPack;
using ZstdSharp;

namespace WorkshopMonitor;

/// <summary>
/// Steam Workshop monitor: scrapes recent Build 42 mods, resolves metadata via Steam API,
/// filters by subscription threshold, and merges new IDs into the translation request list.
/// </summary>
public class WorkshopMonitorService
{
    private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36 Edg/131.0.0.0";
    private const int MinSubs = 10000, SafetyPages = 5, AppId = 108600, PageSize = 30;
    private static readonly TimeSpan Lookback = TimeSpan.FromHours(48);
    private static readonly Regex ModIdRe = new(@"sharedfiles/filedetails/\?id=(\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private readonly string _apiKey;
    private readonly string _configDir;
    private readonly string _dataDir;

    public WorkshopMonitorService(string apiKey, string configDir, string dataDir)
    {
        _apiKey = apiKey;
        _configDir = configDir;
        _dataDir = dataDir;
    }

    public async Task RunAsync()
    {
        Console.OutputEncoding = Utf8NoBom.Encoding;
        Console.WriteLine($"=== Steam Workshop Monitor (>{MinSubs} subs) ===\n");

        var cacheFile = Path.Combine(_dataDir, "monitor_cache.bin");
        var requestFile = Path.Combine(_configDir, "request_for_translation.txt");

        var (last, cache) = LoadCache(cacheFile);
        var cutoff = ((DateTimeOffset)last).ToUnixTimeSeconds();
        Console.WriteLine($"Last: {last:yyyy-MM-dd HH:mm:ss} UTC | Cached: {cache.Count}\n");

        using var http = NewClient();
        int page = 0, safety = 0;
        bool reached = false;

        // ── scrape workshop listing ──
        while (true)
        {
            page++;
            var url = $"https://steamcommunity.com/workshop/browse/?appid={AppId}" +
                      "&browsesort=mostrecent&section=readytouseitems" +
                      $"&p={page}&num_per_page={PageSize}&days=365" +
                      "&requiredtags%5B%5D=Build+42&excludedtags%5B%5D=Language%2FTranslation";

            HashSet<string> ids;
            try { ids = ParseIds(await http.GetStringAsync(url)); }
            catch (Exception ex) { Console.WriteLine($"P{page}: {ex.Message}"); break; }
            if (ids.Count == 0) break;

            int added = 0;
            foreach (var id in ids) if (cache.TryAdd(id, 0)) added++;

            long oldest = 0;
            await EachDetail(http, ids.ToList(), d =>
            {
                var id = Str(d, "publishedfileid");
                if (string.IsNullOrWhiteSpace(id) || !cache.ContainsKey(id)) return;
                var t = Long(d, "time_created");
                cache[id] = t;
                if (t > 0 && (oldest == 0 || t < oldest)) oldest = t;
            });

            Console.Write($"P{page}: {ids.Count} (+{added})");

            if (!reached && oldest > 0 && oldest < cutoff)
            { reached = true; safety = SafetyPages; Console.Write(" ⏎"); }
            if (reached && --safety <= 0) { Console.WriteLine(" ✓\n"); break; }
            if (reached) Console.Write($" [{safety}]");
            Console.WriteLine();

            if (ids.Count < PageSize) break;
            await Task.Delay(Random.Shared.Next(3000, 20001));
        }

        Console.WriteLine($"Scraped {page}p, {cache.Count} mods.\n");

        // ── weekly distribution ──
        var times = cache.Values.Where(t => t > 0).ToArray();
        if (times.Length > 0)
        {
            foreach (var w in times.GroupBy(t =>
            {
                var d = DateTimeOffset.FromUnixTimeSeconds(t).UtcDateTime.Date;
                return d.AddDays(-((7 + (int)d.DayOfWeek - 1) % 7));
            }).OrderBy(g => g.Key))
                Console.WriteLine($"  {w.Key:MM-dd}~{w.Key.AddDays(6):MM-dd}: {w.Count(),4}");
            Console.WriteLine();
        }

        // ── subscription counts ──
        var subs = new Dictionary<string, int>();
        var ids2 = cache.Keys.ToList();
        int batches = (int)Math.Ceiling(ids2.Count / 100.0);
        Console.Write($"Fetching subs ({batches} batches)");

        for (int i = 0; i < ids2.Count; i += 100)
        {
            var chunk = ids2.Skip(i).Take(100).ToList();
            Console.Write($" [{i / 100 + 1}/{batches}]");
            await EachDetail(http, chunk, d =>
            {
                var id = Str(d, "publishedfileid");
                if (!string.IsNullOrWhiteSpace(id)) subs[id] = Int(d, "subscriptions");
            });
            if (i + 100 < ids2.Count) await Task.Delay(300);
        }
        Console.WriteLine($" → {subs.Count}");

        var popular = subs.Where(kv => kv.Value > MinSubs).OrderBy(kv => kv.Key).ToList();
        Console.WriteLine($"\n{new string('=', 50)}\n{popular.Count} mod(s) > {MinSubs} subs:\n{new string('=', 50)}");
        foreach (var (id, n) in popular) Console.WriteLine($"  {id}  {n,8:N0}");
        Console.WriteLine();

        // ── persist ──
        SaveCache(cacheFile, DateTime.UtcNow, cache);

        if (popular.Count > 0)
        {
            var popularIds = popular.Select(p => p.Key).ToHashSet();
            MergeRequestFile(requestFile, popularIds);
            Console.WriteLine($"Merged {popularIds.Count} into {Path.GetFileName(requestFile)}");
        }
    }

    // ── Cache I/O (zstd-compressed binary, little-endian int64 sequence) ──
    // Format: lastRunUnixSec(int64) then pairs of (modId:int64, timeCreatedUnixSec:int64)

    public static (DateTime, Dictionary<string, long>) LoadCache(string path)
    {
        var d = new Dictionary<string, long>();
        if (!File.Exists(path)) return (DateTime.UtcNow - Lookback, d);
        try
        {
            var compressed = File.ReadAllBytes(path);
            var raw = DecompressZstd(compressed);
            if (raw.Length < 8) return (DateTime.UtcNow - Lookback, d);

            var last = DateTimeOffset.FromUnixTimeSeconds(BinaryPrimitives.ReadInt64LittleEndian(raw)).UtcDateTime;
            var span = raw.AsSpan(8);
            for (int i = 0; i + 16 <= span.Length; i += 16)
            {
                var modId = BinaryPrimitives.ReadInt64LittleEndian(span.Slice(i, 8));
                var ts = BinaryPrimitives.ReadInt64LittleEndian(span.Slice(i + 8, 8));
                d[modId.ToString()] = ts;
            }
            return (last, d);
        }
        catch { return (DateTime.UtcNow - Lookback, new()); }
    }

    public static void SaveCache(string path, DateTime last, Dictionary<string, long> cache)
    {
        var count = 1 + cache.Count * 2; // 1 header + pairs
        var raw = new byte[count * 8];
        BinaryPrimitives.WriteInt64LittleEndian(raw, ((DateTimeOffset)last).ToUnixTimeSeconds());
        int pos = 8;
        foreach (var kv in cache.OrderBy(kv => kv.Key))
        {
            if (long.TryParse(kv.Key, NumberStyles.Integer, CultureInfo.InvariantCulture, out var modId))
            {
                BinaryPrimitives.WriteInt64LittleEndian(raw.AsSpan(pos, 8), modId);
                BinaryPrimitives.WriteInt64LittleEndian(raw.AsSpan(pos + 8, 8), kv.Value);
                pos += 16;
            }
        }
        var compressed = CompressZstd(raw.AsSpan(0, pos));
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, compressed);
    }

    private static byte[] CompressZstd(ReadOnlySpan<byte> data)
    {
        if (data.Length == 0) return [];
        using var outMs = new MemoryStream();
        using (var zs = new ZstdStream(outMs, ZstdStreamMode.Compress))
        {
            zs.Write(data);
            zs.Flush();
        }
        return outMs.ToArray();
    }

    private static byte[] DecompressZstd(byte[] compressed)
    {
        if (compressed.Length == 0) return [];
        using var inMs = new MemoryStream(compressed);
        using var zs = new ZstdStream(inMs, ZstdStreamMode.Decompress);
        using var outMs = new MemoryStream();
        zs.CopyTo(outMs);
        return outMs.ToArray();
    }

    // ── request_for_translation merge ──

    public static void MergeRequestFile(string path, HashSet<string> newIds)
    {
        var ids = new List<string>();
        if (File.Exists(path))
        {
            foreach (var line in Utf8NoBom.ReadAllLines(path))
            {
                var trimmed = line.Trim();
                if (trimmed.Length > 0) ids.Add(trimmed);
            }
        }
        var existing = new HashSet<string>(ids);
        foreach (var id in newIds)
        {
            if (existing.Add(id)) ids.Add(id);
        }
        Utf8NoBom.WriteAllText(path, string.Join("\n", ids) + "\n");
    }

    // ── Steam API ──

    private string ApiUrl() =>
        $"https://api.steampowered.com/ISteamRemoteStorage/GetPublishedFileDetails/v1/?key={Uri.EscapeDataString(_apiKey)}";

    private static FormUrlEncodedContent BuildPayload(List<string> ids)
    {
        var d = new Dictionary<string, string> { ["itemcount"] = ids.Count.ToString(), ["format"] = "json" };
        for (int i = 0; i < ids.Count; i++) d[$"publishedfileids[{i}]"] = ids[i];
        return new FormUrlEncodedContent(d);
    }

    private async Task EachDetail(HttpClient http, List<string> ids, Action<JsonElement> process)
    {
        if (ids.Count == 0) return;
        try
        {
            using var resp = await http.PostAsync(ApiUrl(), BuildPayload(ids));
            resp.EnsureSuccessStatusCode();
            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            if (doc.RootElement.TryGetProperty("response", out var r) &&
                r.TryGetProperty("publishedfiledetails", out var details) &&
                details.ValueKind == JsonValueKind.Array)
                foreach (var d in details.EnumerateArray()) process(d);
        }
        catch (Exception ex) { Console.Write($" [API:{ex.Message}]"); }
    }

    // ── HTML parsing ──

    public static HashSet<string> ParseIds(string html)
    {
        var ids = new HashSet<string>();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'aspectratio_square')]");
        if (nodes == null) return ids;
        foreach (var n in nodes)
        {
            var a = n.SelectSingleNode(".//a[contains(@href, 'sharedfiles/filedetails/?id=')]");
            if (a == null) continue;
            var m = ModIdRe.Match(a.GetAttributeValue("href", ""));
            if (m.Success) ids.Add(m.Groups[1].Value);
        }
        return ids;
    }

    // ── HTTP ──

    public static HttpClient NewClient()
    {
        var c = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All })
        { Timeout = TimeSpan.FromSeconds(30) };
        c.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        c.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        c.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
        return c;
    }

    // ── JSON helpers ──

    public static string? Str(JsonElement e, string n) =>
        e.TryGetProperty(n, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;

    public static int Int(JsonElement e, string n)
    {
        if (!e.TryGetProperty(n, out var v)) return 0;
        if (v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out int x)) return x;
        if (v.ValueKind == JsonValueKind.String && int.TryParse(v.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out x)) return x;
        return 0;
    }

    public static long Long(JsonElement e, string n)
    {
        if (!e.TryGetProperty(n, out var v)) return 0;
        if (v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out long x)) return x;
        if (v.ValueKind == JsonValueKind.String && long.TryParse(v.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out x)) return x;
        return 0;
    }
}
