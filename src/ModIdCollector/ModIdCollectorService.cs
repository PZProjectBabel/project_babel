using Common;
using System.Globalization;
using System.Text.Json;

namespace ModIdCollector;

/// <summary>
/// Collects mod IDs and seed metadata from remote and local sources.
/// ConfigReader must validate the environment before this service runs.
/// </summary>
public class ModIdCollectorService
{
    private readonly PipelineConfig _config;
    private readonly HttpClient? _httpClient;
    private const int AsOneTimeoutLimit = 3;

    public int errorCount = 0;
    public int warningCount = 0;

    public ModIdCollectorService(PipelineConfig config, HttpClient? httpClient = null)
    {
        _config = config;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Collects every configured mod ID source. The base directory is taken from PipelineConfig.
    /// </summary>
    public async Task<TaskResult> CollectModIdsAsync(
        Dictionary<string, ModInfo> modInfoDict,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("CollectModIdsAsync - base dir: " + _config.baseDir);
        errorCount = 0;
        warningCount = 0;

        string requestFile = Path.Combine(_config.baseDir, "config", "request_for_translation.txt");
        // Remote AsOne list.
        if (_config.asOneEnabled)
        {
            var asOneModInfos = await GetModInfosFromRemoteServerAsync(
                _config.asOneBaseUrl,
                _config.asOnePublicModListPath,
                cancellationToken);
            if (asOneModInfos != null)
            {
                Console.WriteLine($"  Collected {asOneModInfos.Count} mod IDs from AsOne.");
                foreach (var (modId, info) in asOneModInfos)
                {
                    if (PipelineExclusions.IsExcluded(modId))
                        continue;
                    if (modInfoDict.ContainsKey(modId))
                        continue;
                    modInfoDict[modId] = info;
                }
            }
        }
        else
        {
            GitHubActions.Warning("AsOne is disabled; remote mod ID collection will be skipped.", "Remote list skipped");
            warningCount++;
        }

        // Local request_for_translation.txt list.
        if (File.Exists(requestFile))
        {
            var localIds = (await Utf8NoBom.ReadAllLinesAsync(requestFile, cancellationToken))
                .Select(ParseLocalModId)
                .Where(id => id != null)
                .Select(id => id!)
                .Where(id => !PipelineExclusions.IsExcluded(id))
                .Distinct()
                .ToList();
            Console.WriteLine($"  Collected {localIds.Count} mod IDs from request_for_translation.txt.");
            foreach (var modId in localIds)
            {
                if (modInfoDict.ContainsKey(modId))
                    continue;

                modInfoDict[modId] = new ModInfo
                {
                    modId = modId
                };
            }
        }
        else
        {
            GitHubActions.Warning($"request_for_translation.txt not found at {requestFile}; local mod ID list will be skipped.", "Local list missing");
            warningCount++;
        }

        // Summary.
        int total = modInfoDict.Count;
        int needCheck = modInfoDict.Values.Count(i => i.contentCheckStatus == ContentCheckStatus.NEEDVERIFICATION);
        int accepted = modInfoDict.Values.Count(i => i.contentCheckStatus == ContentCheckStatus.ACCEPTED);
        int rejected = modInfoDict.Values.Count(i => i.contentCheckStatus == ContentCheckStatus.REJECTED);
        Console.WriteLine($"  ------SUMMARY------");
        Console.WriteLine($"  Total: {total} | Need verification: {needCheck} | Accepted: {accepted} | Rejected: {rejected}");

        return new TaskResult
        {
            isSuccess = errorCount == 0,
            errorCount = errorCount,
            warningCount = warningCount
        };
    }

    public async Task<IDictionary<string, ModInfo>?> GetModInfosFromRemoteServerAsync(
        string baseUrl,
        string modListPath,
        CancellationToken cancellationToken = default)
    {
        // 1) Try DownloadCenter file download first.
        var downloadResult = await TryGetModInfosFromDownloadCenterAsync(baseUrl, cancellationToken);
        if (downloadResult != null)
            return downloadResult;

        // 2) Fallback to current GetAllModinfo endpoint.
        var url = $"{baseUrl.TrimEnd('/')}/{modListPath.TrimStart('/')}";
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            GitHubActions.Error($"Invalid remote mod list URL: {baseUrl}/{modListPath}", "Remote list failed");
            errorCount++;
            return null;
        }

        using var ownedClient = _httpClient == null ? new HttpClient() : null;
        var httpClient = _httpClient ?? ownedClient!;
        if (ownedClient != null)
            httpClient.Timeout = Timeout.InfiniteTimeSpan;

        var timeout = TimeSpan.FromSeconds(Math.Max(1, _config.steamRequestTimeoutSeconds));
        for (var consecutiveTimeouts = 0; consecutiveTimeouts < AsOneTimeoutLimit;)
        {
            try
            {
                Console.WriteLine($"  Fetching mod IDs from: {uri}");
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(timeout);
                using var response = await httpClient.GetAsync(uri, timeoutCts.Token);
                if (!response.IsSuccessStatusCode)
                {
                    GitHubActions.Error($"HTTP {(int)response.StatusCode} from remote mod list server: {uri}", "Remote list failed");
                    errorCount++;
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync(timeoutCts.Token);
                return ParseRemoteModList(content);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex) when (IsTimeout(ex, cancellationToken))
            {
                consecutiveTimeouts++;
                GitHubActions.Warning(
                    $"Timed out fetching remote mod list from {uri} ({consecutiveTimeouts}/{AsOneTimeoutLimit}).",
                    "Remote list timeout");
                warningCount++;

                if (consecutiveTimeouts >= AsOneTimeoutLimit)
                {
                    GitHubActions.Warning("AsOne remote mod list timed out 3 consecutive times; skipping remote list.", "Remote list skipped");
                    warningCount++;
                    return null;
                }
            }
            catch (Exception ex)
            {
                GitHubActions.Error($"Failed to fetch remote mod list from {uri}: {ex.Message}", "Remote list failed");
                errorCount++;
                return null;
            }
        }

        return null;
    }

    private async Task<IDictionary<string, ModInfo>?> TryGetModInfosFromDownloadCenterAsync(
        string baseUrl,
        CancellationToken cancellationToken = default)
    {
        var fileName = _config.asOneModInfoFileName;
        if (string.IsNullOrWhiteSpace(fileName))
            return null;

        var encodedFileName = Uri.EscapeDataString(fileName);
        var url = $"{baseUrl.TrimEnd('/')}/api/DownloadCenter/GetModInfoFile?fileName={encodedFileName}";

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return null;

        using var ownedClient = _httpClient == null ? new HttpClient() : null;
        var httpClient = _httpClient ?? ownedClient!;
        if (ownedClient != null)
            httpClient.Timeout = TimeSpan.FromSeconds(Math.Max(1, _config.steamRequestTimeoutSeconds));

        try
        {
            Console.WriteLine($"  Trying DownloadCenter: {uri}");
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Accept", "application/json, application/octet-stream, */*");

            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, _config.steamRequestTimeoutSeconds)));

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeoutCts.Token);
            if (!response.IsSuccessStatusCode)
            {
                GitHubActions.Warning(
                    $"DownloadCenter returned {(int)response.StatusCode}, falling back to GetAllModinfo.",
                    "DownloadCenter failed");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync(timeoutCts.Token);
            if (string.IsNullOrWhiteSpace(content))
            {
                GitHubActions.Warning("DownloadCenter returned empty content, falling back to GetAllModinfo.", "DownloadCenter empty");
                return null;
            }

            var result = ParseRemoteModList(content);
            if (result.Count == 0)
            {
                GitHubActions.Warning("DownloadCenter parsed 0 mods, falling back to GetAllModinfo.", "DownloadCenter empty parse");
                return null;
            }

            Console.WriteLine($"  [OK] DownloadCenter: {result.Count} mod(s)");
            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            GitHubActions.Warning(
                $"DownloadCenter failed: {ex.Message}, falling back to GetAllModinfo.",
                "DownloadCenter exception");
            return null;
        }
    }

    private static bool IsTimeout(Exception ex, CancellationToken callerToken)
    {
        if (callerToken.IsCancellationRequested)
            return false;

        return ex is TimeoutException
            || ex.InnerException is TimeoutException
            || ex is TaskCanceledException
            || ex is OperationCanceledException;
    }

    private static Dictionary<string, ModInfo> ParseRemoteModList(string content)
    {
        if (TryParseJsonRemoteList(content, out var result))
            return result;

        return content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(ParseLocalModId)
            .Where(id => id != null)
            .Select(id => id!)
            .Distinct()
            .ToDictionary(
                id => id,
                id => new ModInfo
                {
                    modId = id
                });
    }

    private static bool TryParseJsonRemoteList(string content, out Dictionary<string, ModInfo> result)
    {
        result = new Dictionary<string, ModInfo>();
        try
        {
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            var list = root.ValueKind == JsonValueKind.Array
                ? root
                : TryGetProperty(root, "data", out var data) && data.ValueKind == JsonValueKind.Array
                    ? data
                    : default;

            if (list.ValueKind != JsonValueKind.Array)
                return false;

            foreach (var item in list.EnumerateArray())
            {
                if (!TryReadModId(item, out var modId))
                    continue;

                result[modId] = new ModInfo
                {
                    modId = modId
                };
            }

            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static bool TryReadModId(JsonElement item, out string modId)
    {
        modId = "";
        if (!TryGetProperty(item, "ModId", out var value))
            return false;

        modId = value.ValueKind == JsonValueKind.Number
            ? value.GetInt64().ToString(CultureInfo.InvariantCulture)
            : value.GetString()?.Trim() ?? "";

        return modId.Length > 0;
    }

    private static string? ReadString(JsonElement item, string name)
    {
        return TryGetProperty(item, name, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;
    }

    private static string? ReadContributors(JsonElement item)
    {
        if (!TryGetProperty(item, "Contributors", out var contributors) || contributors.ValueKind != JsonValueKind.Array)
            return null;

        var names = contributors.EnumerateArray()
            .Where(value => value.ValueKind == JsonValueKind.String)
            .Select(value => value.GetString())
            .Where(value => !string.IsNullOrWhiteSpace(value));

        var text = string.Join(", ", names);
        return string.IsNullOrWhiteSpace(text) ? null : text;
    }

    private static DateTime ReadDateTime(JsonElement item, string name)
    {
        var text = ReadString(item, name);
        if (text == null)
            return DateTime.MinValue;

        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed)
            || DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out parsed)
            ? parsed
            : DateTime.MinValue;
    }

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

    private static string? ParseLocalModId(string line)
    {
        var trimmed = line.Trim();
        if (trimmed.Length == 0 || trimmed.StartsWith('#'))
            return null;

        var id = trimmed.Split([' ', '\t', ',', ';'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        return string.IsNullOrWhiteSpace(id) ? null : id;
    }
}
