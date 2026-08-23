using Common;
using System.Globalization;
using System.Text.Json;

namespace ModInfoFetcher;

/// <summary>
/// Mod info retriever service - fetches mod info from Steam Web API based on mod IDs collected by ModIdCollectorService.
/// </summary>
public class ModInfoFetcherService
{
    private readonly PipelineConfig _config;
    private readonly HttpClient? _httpClient;

    public ModInfoFetcherService(PipelineConfig config, HttpClient? httpClient = null)
    {
        _config = config;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Fills the input dictionary with the latest metadata available from Steam Web API.
    /// </summary>
    public async Task<TaskResult> FetchModInfosAsync(Dictionary<string, ModInfo> modInfoDict)
    {
        if (modInfoDict.Count == 0)
        {
            GitHubActions.Warning("No mod info to fetch; ModInfoFetcher will be skipped.", "No mod info");
            return new TaskResult { warningCount = 1 };
        }

        // Retrieve every requested ID in batches to avoid Steam API limits.
        var chunkSize = Math.Max(1, _config.steamApiChunkSize);
        var total = modInfoDict.Keys.Distinct().Count();
        var progress = 0;
        var pzCount = 0;
        var nonPzCount = 0;
        var unknownCount = 0;
        var consecutiveFailures = 0;
        var errorCount = 0;
        foreach (var ids in modInfoDict.Keys.Distinct().Chunk(chunkSize))
        {
            var fetched = await FetchModInfosBatchAsync(ids, modInfoDict);
            foreach (var (modId, info) in fetched.Infos)
            {
                modInfoDict[modId] = info;
            }

            progress += fetched.Counts.Total;
            pzCount += fetched.Counts.Pz;
            nonPzCount += fetched.Counts.NonPz;
            unknownCount += fetched.Counts.Unknown;
            WriteProgress("Retrieved", progress, total, pzCount, nonPzCount, unknownCount);

            if (fetched.Succeeded)
            {
                consecutiveFailures = 0;
                continue;
            }

            consecutiveFailures++;
            errorCount++;
            if (consecutiveFailures >= 5)
            {
                GitHubActions.Error(
                    "Steam metadata fetch failed for 5 consecutive batches; stopping metadata fetch and continuing with collected partial mod info.",
                    "Steam fetch aborted");
                break;
            }
        }

        Console.WriteLine($"  ------SUMMARY------");
        WriteProgress("Summary", total, total, pzCount, nonPzCount, unknownCount);
        Console.WriteLine($"  Total: {total} | PZ: {pzCount} | Non-PZ: {nonPzCount} | Unknown: {unknownCount}");
        return new TaskResult
        {
            isSuccess = errorCount == 0,
            errorCount = errorCount
        };
    }

    private async Task<BatchFetchResult> FetchModInfosBatchAsync(IEnumerable<string> ids, Dictionary<string, ModInfo> source)
    {
        var batchIds = ids.ToList();
        var result = batchIds.ToDictionary(
            id => id,
            id =>
            {
                var info = source[id];
                if (string.IsNullOrWhiteSpace(info.modId))
                    info.modId = id;
                info.timeLastChecked = DateTime.UtcNow;
                info.lastFetchStatus = "fetch_failed";
                return info;
            });

        try
        {
            // Fetch published file metadata for this batch of mod IDs.
            using var ownedClient = _httpClient == null ? new HttpClient() : null;
            var client = _httpClient ?? ownedClient!;
            if (ownedClient != null)
                client.Timeout = TimeSpan.FromSeconds(Math.Max(1, _config.steamRequestTimeoutSeconds));

            var payload = new Dictionary<string, string>
            {
                ["itemcount"] = batchIds.Count.ToString(CultureInfo.InvariantCulture),
                ["format"] = "json"
            };
            for (var idx = 0; idx < batchIds.Count; idx++)
                payload[$"publishedfileids[{idx}]"] = batchIds[idx];

            using var content = new FormUrlEncodedContent(payload);
            var url = $"https://api.steampowered.com/ISteamRemoteStorage/GetPublishedFileDetails/v1/?key={Uri.EscapeDataString(_config.steamApiKey)}";
            using var responseMessage = await client.PostAsync(url, content);
            responseMessage.EnsureSuccessStatusCode();
            string jsonResponse = await responseMessage.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonResponse);
            if (!TryGetProperty(doc.RootElement, "response", out var response)
                || !TryGetProperty(response, "publishedfiledetails", out var details)
                || details.ValueKind != JsonValueKind.Array)
            {
                GitHubActions.Warning("Steam response did not include publishedfiledetails; existing mod info was kept.", "Steam response missing details");
                return new BatchFetchResult(result, new ModInfoFetchCounts(0, 0, batchIds.Count), false);
            }

            var seen = new HashSet<string>();
            var pzCount = 0;
            var nonPzCount = 0;
            var unknownCount = 0;
            foreach (var detail in details.EnumerateArray())
            {
                if (!TryReadString(detail, "publishedfileid", out var modId) || string.IsNullOrWhiteSpace(modId))
                    continue;

                seen.Add(modId);
                if (TryReadInt(detail, "result", out var detailResult) && detailResult != 1)
                {
                    var unavailable = result.TryGetValue(modId, out var unavailableInfo) ? unavailableInfo : new ModInfo { modId = modId };
                    unavailable.modId = modId;
                    unavailable.timeLastChecked = DateTime.UtcNow;
                    unavailable.isAvailable = false;
                    unavailable.lastFetchStatus = "missing";
                    unavailable.needsUpdate = false;
                    result[modId] = unavailable;
                    unknownCount++;
                    continue;
                }

                var current = result.TryGetValue(modId, out var existing) ? existing : new ModInfo { modId = modId };
                var oldTimeUpdated = current.timeModUpdated;
                current.modId = modId;
                current.modName = ReadString(detail, "title", current.modName);
                current.creator = ReadString(detail, "creator", current.creator);
                current.timeModCreated = ReadUnixTime(detail, "time_created", current.timeModCreated);
                current.timeModUpdated = ReadUnixTime(detail, "time_updated", current.timeModUpdated);
                current.timeLastChecked = DateTime.UtcNow;
                current.subscription = ReadInt(detail, "subscriptions", current.subscription);
                current.favorite = ReadInt(detail, "favorited", current.favorite);
                current.description = DescriptionCleaner.Clean(ReadString(detail, "description", current.description));
                current.consumerAppId = ReadInt(detail, "consumer_app_id", current.consumerAppId);
                current.isAvailable = current.consumerAppId == 108600;
                current.lastFetchStatus = current.isAvailable ? "ok" : "not_pz";
                // Keep an already queued update until the download/extraction stage clears it.
                // Fetching metadata for a later batch must not dequeue that mod prematurely.
                current.needsUpdate = current.needsUpdate
                    || oldTimeUpdated == DateTime.MinValue
                    || current.timeModUpdated > oldTimeUpdated;
                result[modId] = current;

                if (current.consumerAppId == 108600)
                    pzCount++;
                else
                    nonPzCount++;
            }

            foreach (var missingId in batchIds.Where(id => !seen.Contains(id)))
            {
                GitHubActions.Warning($"Steam response did not contain mod {missingId}; existing mod info was kept.", "Steam mod detail missing");
                var missingInfo = result[missingId];
                missingInfo.lastFetchStatus = "fetch_failed";
                result[missingId] = missingInfo;
                unknownCount++;
            }

            return new BatchFetchResult(result, new ModInfoFetchCounts(pzCount, nonPzCount, unknownCount), true);
        }
        catch (Exception ex)
        {
            GitHubActions.Error($"Failed to fetch mod infos for batch [{string.Join(", ", batchIds)}]: {ex.Message}", "Steam fetch failed");
            return new BatchFetchResult(result, new ModInfoFetchCounts(0, 0, batchIds.Count), false);
        }
    }

    private static void WriteProgress(string label, int progress, int total, int pzCount, int nonPzCount, int unknownCount)
    {
        Console.WriteLine($"  {label} [{progress,6}/{total,6}] mod info from steam: {pzCount,3} PZ, {nonPzCount,3} Non-PZ, {unknownCount,3} unknown");
    }

    private sealed record ModInfoFetchCounts(int Pz, int NonPz, int Unknown)
    {
        public int Total => Pz + NonPz + Unknown;
    }

    private sealed record BatchFetchResult(Dictionary<string, ModInfo> Infos, ModInfoFetchCounts Counts, bool Succeeded);

    private static bool TryGetProperty(JsonElement item, string name, out JsonElement value)
    {
        if (item.ValueKind != JsonValueKind.Object)
        {
            value = default;
            return false;
        }

        foreach (var property in item.EnumerateObject())
        {
            if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private static string ReadString(JsonElement item, string name, string fallback) =>
        TryReadString(item, name, out var value) ? value : fallback;

    private static bool TryReadString(JsonElement item, string name, out string value)
    {
        if (TryGetProperty(item, name, out var element))
        {
            if (element.ValueKind == JsonValueKind.String)
            {
                value = element.GetString() ?? "";
                return true;
            }

            if (element.ValueKind == JsonValueKind.Number)
            {
                value = element.GetRawText();
                return true;
            }
        }

        value = "";
        return false;
    }

    private static int ReadInt(JsonElement item, string name, int fallback) =>
        TryReadInt(item, name, out var value) ? value : fallback;

    private static bool TryReadInt(JsonElement item, string name, out int value)
    {
        if (!TryGetProperty(item, name, out var element))
        {
            value = 0;
            return false;
        }

        if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out value))
            return true;

        if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
            return true;

        value = 0;
        return false;
    }

    private static DateTime ReadUnixTime(JsonElement item, string name, DateTime fallback)
    {
        if (!TryGetProperty(item, name, out var element))
            return fallback;

        long seconds;
        if (element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out seconds))
            return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;

        if (element.ValueKind == JsonValueKind.String
            && long.TryParse(element.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out seconds))
            return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;

        return fallback;
    }
}
