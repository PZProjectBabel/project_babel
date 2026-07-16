using System.Diagnostics;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.RegularExpressions;
using Common;

namespace ModDownloader;

/// <summary>
/// Downloads Steam Workshop mod files through steamcmd.
/// </summary>
public class ModDownloaderService
{
    private readonly PipelineConfig _config;
    private const int AppId = 108600; // Project Zomboid
    private static readonly ConcurrentDictionary<int, Process> RunningSteamCmdProcesses = new();
    private static int _shutdownHandlersRegistered;

    public ModDownloaderService(PipelineConfig config)
    {
        _config = config;
        RegisterShutdownHandlers();
    }

    /// <summary>
    /// Downloads mods using a copied steamcmd instance. Failed mod IDs are retried only.
    /// </summary>
    public async Task<TaskResult> DownloadModsAsync(
        List<string> modIds,
        Dictionary<string, ModInfo> modInfoDict,
        string batchTempFolder)
    {
        var requestedIds = modIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct()
            .ToList();
        if (requestedIds.Count == 0) return new TaskResult();

        // 1. Copy steamcmd to batch folder
        string steamcmdSrc = Path.Combine(_config.baseDir, "src", "3rd_party", "steamcmd");
        string steamcmdDst = Path.Combine(batchTempFolder, "steamcmd");
        if (!Directory.Exists(steamcmdSrc))
        {
            GitHubActions.Warning($"steamcmd source folder not found: {steamcmdSrc}", "ModDownloader");
            CreateDownloadedPathRecords(requestedIds, modInfoDict);
            return new TaskResult
            {
                isSuccess = false,
                warningCount = 1
            };
        }

        CopyDirectory(steamcmdSrc, steamcmdDst);
        string steamcmdExe = Path.Combine(steamcmdDst, OperatingSystem.IsWindows() ? "steamcmd.exe" : "steamcmd.sh");
        if (!File.Exists(steamcmdExe))
        {
            GitHubActions.Warning($"steamcmd executable not found: {steamcmdExe}", "ModDownloader");
            CreateDownloadedPathRecords(requestedIds, modInfoDict);
            return new TaskResult
            {
                isSuccess = false,
                warningCount = 1
            };
        }

        Console.WriteLine($"  steamcmd copied to {steamcmdDst}");

        var succeeded = new HashSet<string>(StringComparer.Ordinal);
        string workshopContent = Path.Combine(steamcmdDst, "steamapps", "workshop", "content", AppId.ToString());
        var downloadedPaths = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var modId in requestedIds)
        {
            string dstModDir = Path.Combine(_config.downloadedModsTempDir, modId);
            if (Directory.Exists(dstModDir) && Directory.EnumerateFileSystemEntries(dstModDir).Any())
                succeeded.Add(modId);
        }

        // 2. Retry only failed/pending IDs.
        int maxAttempts = Math.Max(1, _config.steamMaxRetries + 1);
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var pending = requestedIds.Where(id => !succeeded.Contains(id)).ToList();
            if (pending.Count == 0)
                break;

            Console.WriteLine($"  download attempt [{attempt}/{maxAttempts}] pending {pending.Count}/{requestedIds.Count} mod(s)...");
            var dlCmds = string.Join(" ", pending.Select(id => $"+workshop_download_item {AppId} {id}"));
            string args = $"+force_install_dir \"{steamcmdDst}\" +login anonymous {dlCmds} +quit";
            string output = await RunSteamCmdAsync(steamcmdExe, args, steamcmdDst, pending, requestedIds.Count);

            foreach (var (modId, path) in ParseDownloadPaths(output, pending))
                downloadedPaths[modId] = path;

            // Newer steamcmd versions write completion paths to logs rather than stdout.
            foreach (var (modId, path) in ParseDownloadPathsFromLogs(steamcmdDst, pending))
                downloadedPaths[modId] = path;

            foreach (var modId in pending)
            {
                string srcModDir = GetDownloadedModPath(modId, workshopContent, downloadedPaths);
                if (Directory.Exists(srcModDir) && Directory.EnumerateFileSystemEntries(srcModDir).Any())
                    succeeded.Add(modId);
            }

            Console.WriteLine($"  download progress [{succeeded.Count}/{requestedIds.Count}] succeeded, {requestedIds.Count - succeeded.Count} pending");
            if (attempt < maxAttempts && succeeded.Count < requestedIds.Count)
                await Task.Delay(TimeSpan.FromSeconds(Math.Min(30, attempt * 2)));
        }

        // 3. Move downloaded mods from steamcmd output dir to final dir.
        foreach (var modId in requestedIds)
        {
            string srcModDir = GetDownloadedModPath(modId, workshopContent, downloadedPaths);
            string dstModDir = Path.Combine(_config.downloadedModsTempDir, modId);

            if (Directory.Exists(srcModDir))
            {
                if (Directory.Exists(dstModDir))
                    Directory.Delete(dstModDir, true);
                Directory.CreateDirectory(Path.GetDirectoryName(dstModDir)!);
                Directory.Move(srcModDir, dstModDir);
                Console.WriteLine($"  [OK] {modId} -> {dstModDir}");
            }
            else if (!succeeded.Contains(modId))
            {
                GitHubActions.Warning($"Mod {modId} download may have failed; no output directory found.", "ModDownloader");
            }

            SetDownloadedPath(modInfoDict, modId, dstModDir);
        }

        var errorCount = requestedIds.Count - succeeded.Count;
        return new TaskResult
        {
            isSuccess = errorCount == 0,
            errorCount = errorCount
        };
    }

    private static async Task<string> RunSteamCmdAsync(
        string exePath,
        string args,
        string workingDir,
        IReadOnlyList<string> pendingIds,
        int totalCount)
    {
        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Utf8NoBom.Encoding,
            StandardErrorEncoding = Utf8NoBom.Encoding,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDir
        };

        using var proc = new Process { StartInfo = psi };

        var outputLines = new List<string>();
        var outputLock = new object();
        var progress = new SteamCmdProgress(pendingIds, totalCount);
        using var progressCts = new CancellationTokenSource();
        var logMonitor = new SteamCmdLogMonitor(workingDir, progress);

        proc.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                HandleSteamCmdOutputLine(e.Data, outputLines, outputLock, progress);
        };

        proc.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
                HandleSteamCmdOutputLine(e.Data, outputLines, outputLock, progress);
        };

        Task monitorTask = Task.CompletedTask;
        var pid = 0;
        try
        {
            proc.Start();
            pid = proc.Id;
            RunningSteamCmdProcesses[pid] = proc;
            monitorTask = logMonitor.MonitorAsync(progressCts.Token);
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            await proc.WaitForExitAsync();
        }
        finally
        {
            progressCts.Cancel();
            logMonitor.ReadAndEmit();
            try { await monitorTask.WaitAsync(TimeSpan.FromSeconds(3)); } catch { }
            if (pid > 0)
                RunningSteamCmdProcesses.TryRemove(pid, out _);
        }

        if (proc.ExitCode != 0)
        {
            List<string> diagnostics;
            lock (outputLock)
                diagnostics = outputLines.TakeLast(20).ToList();

            if (progress.LastSelfUpdateSummary is { } summary
                && !diagnostics.Contains(summary, StringComparer.Ordinal))
            {
                diagnostics.Add(summary);
            }

            string lastLines = string.Join("\n", diagnostics.TakeLast(20));
            GitHubActions.Error($"steamcmd exit code {proc.ExitCode}\n{lastLines}", "steamcmd");
        }

        lock (outputLock)
            return string.Join("\n", outputLines);
    }

    private static void HandleSteamCmdOutputLine(
        string line,
        List<string> outputLines,
        object outputLock,
        SteamCmdProgress progress)
    {
        var lineKind = ParseProgressLine(line, progress);
        if (lineKind == SteamCmdOutputLineKind.TransientProgress)
            return;

        lock (outputLock)
            outputLines.Add(line);
    }

    private static SteamCmdOutputLineKind ParseProgressLine(string line, SteamCmdProgress progress)
    {
        foreach (var progressEvent in ParseDownloadProgressEvents(line))
        {
            if (progressEvent.Completed)
                progress.MarkCompleted(progressEvent.ModId, progressEvent.Bytes);
            else
                progress.MarkDownloading(progressEvent.ModId);
            return SteamCmdOutputLineKind.DownloadProgress;
        }

        var trimmed = line.Trim();
        if (TryParseSteamCmdUpdateProgress(trimmed, out var updateProgress))
        {
            progress.MarkSelfUpdateProgress(updateProgress);
            return SteamCmdOutputLineKind.TransientProgress;
        }

        var updateStarted = Regex.Match(
            trimmed,
            @"AppID\s+108600\s+update started\s*:\s*download\s+\d+/(\d+).*stage\s+\d+/(\d+)",
            RegexOptions.IgnoreCase);
        if (updateStarted.Success)
        {
            long.TryParse(updateStarted.Groups[1].Value, out var downloadBytes);
            long.TryParse(updateStarted.Groups[2].Value, out var stageBytes);
            progress.MarkDownloadPlan(downloadBytes, stageBytes);
            return SteamCmdOutputLineKind.DownloadProgress;
        }

        var rate = Regex.Match(trimmed, @"Current download rate:\s*(.+)", RegexOptions.IgnoreCase);
        if (rate.Success)
        {
            progress.MarkDownloadRate(rate.Groups[1].Value.Trim());
            return SteamCmdOutputLineKind.DownloadProgress;
        }

        if (trimmed.Contains("starting commit", StringComparison.OrdinalIgnoreCase))
        {
            progress.MarkCommitting();
            return SteamCmdOutputLineKind.DownloadProgress;
        }

        if (trimmed.StartsWith("Success.", StringComparison.OrdinalIgnoreCase))
            Console.WriteLine($"  {trimmed}");
        else if (line.StartsWith("ERROR") || line.StartsWith("Error"))
            GitHubActions.Error(trimmed, "steamcmd");

        return SteamCmdOutputLineKind.Other;
    }

    public static bool TryParseSteamCmdUpdateProgress(string line, out SteamCmdUpdateProgress progress)
    {
        progress = default;
        var trimmed = line.Trim();
        var match = Regex.Match(
            trimmed,
            @"^\[\s*(?:(?<percent>\d{1,3})%|-+)\]\s*(?<message>.+?)\s*(?:\.\.\.)?\s*$",
            RegexOptions.IgnoreCase);
        if (!match.Success)
            return false;

        var message = match.Groups["message"].Value.Trim();
        if (!LooksLikeSteamCmdUpdateProgressMessage(message))
            return false;

        int? percent = null;
        if (match.Groups["percent"].Success
            && int.TryParse(match.Groups["percent"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedPercent))
        {
            percent = parsedPercent;
        }

        long? downloadedKilobytes = null;
        long? totalKilobytes = null;
        var sizeMatch = Regex.Match(
            message,
            @"已下载\s*(?<downloaded>[\d, ]+)\s*[,，]\s*共\s*(?<total>[\d, ]+)\s*KB",
            RegexOptions.IgnoreCase);
        if (!sizeMatch.Success)
        {
            sizeMatch = Regex.Match(
                message,
                @"(?<downloaded>[\d, ]+)\s*(?:of|/)\s*(?<total>[\d, ]+)\s*KB",
                RegexOptions.IgnoreCase);
        }

        if (sizeMatch.Success)
        {
            if (TryParseKilobytes(sizeMatch.Groups["downloaded"].Value, out var downloaded))
                downloadedKilobytes = downloaded;
            if (TryParseKilobytes(sizeMatch.Groups["total"].Value, out var total))
                totalKilobytes = total;
        }

        progress = new SteamCmdUpdateProgress(
            percent,
            NormalizeSteamCmdUpdatePhase(message),
            downloadedKilobytes,
            totalKilobytes);
        return true;
    }

    private static bool LooksLikeSteamCmdUpdateProgressMessage(string message)
    {
        if (message.Contains("fatal", StringComparison.OrdinalIgnoreCase)
            || message.Contains("error", StringComparison.OrdinalIgnoreCase)
            || message.Contains("错误", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return message.Contains("update", StringComparison.OrdinalIgnoreCase)
            || message.Contains("installation", StringComparison.OrdinalIgnoreCase)
            || message.Contains("installing", StringComparison.OrdinalIgnoreCase)
            || message.Contains("extracting", StringComparison.OrdinalIgnoreCase)
            || message.Contains("更新", StringComparison.OrdinalIgnoreCase)
            || message.Contains("安装", StringComparison.OrdinalIgnoreCase)
            || message.Contains("验证", StringComparison.OrdinalIgnoreCase)
            || message.Contains("校验", StringComparison.OrdinalIgnoreCase)
            || message.Contains("解压", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeSteamCmdUpdatePhase(string message)
    {
        if (message.Contains("download", StringComparison.OrdinalIgnoreCase)
            || message.Contains("下载", StringComparison.OrdinalIgnoreCase))
            return "downloading update";
        if (message.Contains("check", StringComparison.OrdinalIgnoreCase)
            || message.Contains("检查", StringComparison.OrdinalIgnoreCase))
            return "checking update";
        if (message.Contains("verifying", StringComparison.OrdinalIgnoreCase)
            || message.Contains("验证", StringComparison.OrdinalIgnoreCase)
            || message.Contains("校验", StringComparison.OrdinalIgnoreCase))
            return "verifying installation";
        if (message.Contains("extract", StringComparison.OrdinalIgnoreCase)
            || message.Contains("解压", StringComparison.OrdinalIgnoreCase))
            return "extracting update";
        if (message.Contains("install", StringComparison.OrdinalIgnoreCase)
            || message.Contains("安装", StringComparison.OrdinalIgnoreCase))
            return "installing update";
        if (message.Contains("complete", StringComparison.OrdinalIgnoreCase)
            || message.Contains("完成", StringComparison.OrdinalIgnoreCase))
            return "update complete";

        return "updating steamcmd";
    }

    private static bool TryParseKilobytes(string value, out long kilobytes)
    {
        var normalized = value.Replace(",", "", StringComparison.Ordinal).Replace(" ", "", StringComparison.Ordinal);
        return long.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out kilobytes);
    }

    public static List<SteamCmdDownloadProgress> ParseDownloadProgressEvents(string text)
    {
        var result = new List<SteamCmdDownloadProgress>();
        foreach (var line in text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var success = Regex.Match(
                line,
                @"Success\.\s+Downloaded item\s+(\d+).*?\((\d+)\s+bytes\)",
                RegexOptions.IgnoreCase);
            if (success.Success)
            {
                long.TryParse(success.Groups[2].Value, out var bytes);
                result.Add(new SteamCmdDownloadProgress(success.Groups[1].Value, true, bytes));
                continue;
            }

            var downloading = Regex.Match(line, @"Downloading item\s+(\d+)", RegexOptions.IgnoreCase);
            if (downloading.Success)
            {
                result.Add(new SteamCmdDownloadProgress(downloading.Groups[1].Value, false, 0));
                continue;
            }

            var workshopStart = Regex.Match(
                line,
                @"Starting Workshop download job\s+\(requested item\s+(\d+)",
                RegexOptions.IgnoreCase);
            if (workshopStart.Success)
                result.Add(new SteamCmdDownloadProgress(workshopStart.Groups[1].Value, false, 0));
        }

        return result;
    }

    public static HashSet<string> ParseDownloadResults(string output, IReadOnlyList<string> expectedIds)
    {
        var expected = expectedIds.ToHashSet(StringComparer.Ordinal);
        var succeeded = new HashSet<string>(StringComparer.Ordinal);
        foreach (var line in output.Split('\n'))
        {
            // "Success. Downloaded item 1234567890 ..."
            if (line.TrimStart().StartsWith("Success.", StringComparison.OrdinalIgnoreCase)
                && line.Contains("Downloaded item", StringComparison.OrdinalIgnoreCase))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4 && expected.Contains(parts[3]) && long.TryParse(parts[3], out _))
                    succeeded.Add(parts[3]);
            }
        }
        return succeeded;
    }

    public static Dictionary<string, string> ParseDownloadPaths(string output, IReadOnlyList<string> expectedIds)
    {
        var expected = expectedIds.ToHashSet(StringComparer.Ordinal);
        var paths = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var line in output.Split('\n'))
        {
            var match = Regex.Match(
                line,
                @"Success\.\s+Downloaded item\s+(?<id>\d+)\s+to\s+""(?<path>.+?)""",
                RegexOptions.IgnoreCase);
            if (match.Success && expected.Contains(match.Groups["id"].Value))
                paths[match.Groups["id"].Value] = match.Groups["path"].Value;
        }
        return paths;
    }

    /// <summary>
    /// Reads steamcmd log files for SteamCMD completion paths.
    /// Newer steamcmd versions write download results to log files rather than stdout.
    /// </summary>
    private static Dictionary<string, string> ParseDownloadPathsFromLogs(string steamcmdDir, IReadOnlyList<string> expectedIds)
    {
        var logsDir = Path.Combine(steamcmdDir, "logs");
        if (!Directory.Exists(logsDir))
            return new Dictionary<string, string>(StringComparer.Ordinal);

        var paths = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var logFile in Directory.EnumerateFiles(logsDir, "*.*", SearchOption.AllDirectories))
        {
            var ext = Path.GetExtension(logFile);
            if (!string.Equals(ext, ".txt", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(ext, ".log", StringComparison.OrdinalIgnoreCase))
                continue;

            try
            {
                var content = File.ReadAllText(logFile);
                foreach (var (modId, path) in ParseDownloadPaths(content, expectedIds))
                    paths[modId] = path;
            }
            catch
            {
                // Log file may be locked or deleted; ignore.
            }
        }

        return paths;
    }

    private static string GetDownloadedModPath(
        string modId,
        string workshopContent,
        IReadOnlyDictionary<string, string> downloadedPaths)
    {
        string defaultPath = Path.Combine(workshopContent, modId);
        return downloadedPaths.TryGetValue(modId, out var reportedPath) && Directory.Exists(reportedPath)
            ? reportedPath
            : defaultPath;
    }

    private void CreateDownloadedPathRecords(
        IReadOnlyList<string> modIds,
        Dictionary<string, ModInfo> modInfoDict)
    {
        foreach (var modId in modIds)
            SetDownloadedPath(modInfoDict, modId, Path.Combine(_config.downloadedModsTempDir, modId));
    }

    private static void SetDownloadedPath(Dictionary<string, ModInfo> modInfoDict, string modId, string localPath)
    {
        if (!modInfoDict.TryGetValue(modId, out var info))
            info = new ModInfo { modId = modId };

        if (string.IsNullOrWhiteSpace(info.modName))
            info.modName = modId;
        info.localDownloadedPath = localPath;
        modInfoDict[modId] = info;
    }

    private static void CopyDirectory(string src, string dst)
    {
        if (Directory.Exists(dst))
            Directory.Delete(dst, true);

        Directory.CreateDirectory(dst);

        foreach (var dir in Directory.GetDirectories(src, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(dir.Replace(src, dst));

        foreach (var file in Directory.GetFiles(src, "*", SearchOption.AllDirectories))
        {
            string dstFile = file.Replace(src, dst);
            File.Copy(file, dstFile, true);
        }
    }

    private static void RegisterShutdownHandlers()
    {
        if (Interlocked.Exchange(ref _shutdownHandlersRegistered, 1) == 1)
            return;

        Console.CancelKeyPress += (_, _) => KillRunningSteamCmdProcesses("Ctrl+C");
        AppDomain.CurrentDomain.ProcessExit += (_, _) => KillRunningSteamCmdProcesses("process exit");
    }

    private static void KillRunningSteamCmdProcesses(string reason)
    {
        foreach (var (pid, process) in RunningSteamCmdProcesses.ToArray())
        {
            try
            {
                if (process.HasExited)
                    continue;

                Console.Error.WriteLine($"  Killing steamcmd process tree {pid} after {reason}...");
                process.Kill(entireProcessTree: true);
                process.WaitForExit(5000);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"  Failed to kill steamcmd process {pid}: {ex.Message}");
            }
            finally
            {
                RunningSteamCmdProcesses.TryRemove(pid, out _);
            }
        }
    }

    private sealed class SteamCmdProgress
    {
        private readonly Dictionary<string, int> _order;
        private readonly int _totalCount;
        private readonly HashSet<string> _downloading = new(StringComparer.Ordinal);
        private readonly HashSet<string> _completed = new(StringComparer.Ordinal);
        private readonly HashSet<string> _planned = new(StringComparer.Ordinal);
        private readonly HashSet<string> _committing = new(StringComparer.Ordinal);
        private string? _currentModId;
        private string? _lastRateText;
        private string? _lastSelfUpdateProgressKey;
        public string? LastSelfUpdateSummary { get; private set; }

        public SteamCmdProgress(IReadOnlyList<string> pendingIds, int totalCount)
        {
            _order = pendingIds
                .Select((id, index) => new { id, index })
                .ToDictionary(x => x.id, x => x.index, StringComparer.Ordinal);
            _totalCount = totalCount;
        }

        public void MarkDownloading(string modId)
        {
            if (!_order.TryGetValue(modId, out var index) || !_downloading.Add(modId))
                return;

            _currentModId = modId;
            Console.WriteLine($"  [steamcmd] [{index + 1}/{_order.Count}] {modId}: downloading");
        }

        public void MarkCompleted(string modId, long bytes)
        {
            if (!_order.TryGetValue(modId, out var index) || !_completed.Add(modId))
                return;

            if (string.Equals(_currentModId, modId, StringComparison.Ordinal))
                _currentModId = null;
            Console.WriteLine($"  [steamcmd] [{index + 1}/{_order.Count}] {modId}: downloaded {FormatBytes(bytes)} ({_completed.Count}/{_totalCount} total)");
        }

        public void MarkDownloadPlan(long downloadBytes, long stageBytes)
        {
            if (_currentModId == null || !_order.TryGetValue(_currentModId, out var index) || !_planned.Add(_currentModId))
                return;

            var total = stageBytes > 0 ? stageBytes : downloadBytes;
            Console.WriteLine($"  [steamcmd] [{index + 1}/{_order.Count}] {_currentModId}: planned {FormatBytes(total)}");
        }

        public void MarkDownloadRate(string rateText)
        {
            if (_currentModId == null || !_order.TryGetValue(_currentModId, out var index))
                return;
            if (string.Equals(_lastRateText, rateText, StringComparison.OrdinalIgnoreCase))
                return;

            _lastRateText = rateText;
            Console.WriteLine($"  [steamcmd] [{index + 1}/{_order.Count}] {_currentModId}: rate {rateText}");
        }

        public void MarkCommitting()
        {
            if (_currentModId == null || !_order.TryGetValue(_currentModId, out var index) || !_committing.Add(_currentModId))
                return;

            Console.WriteLine($"  [steamcmd] [{index + 1}/{_order.Count}] {_currentModId}: committing");
        }

        public void MarkSelfUpdateProgress(SteamCmdUpdateProgress updateProgress)
        {
            var progressKey = $"{updateProgress.Phase}|{updateProgress.Percent?.ToString(CultureInfo.InvariantCulture) ?? "-"}";
            LastSelfUpdateSummary = FormatSelfUpdateSummary(updateProgress);
            if (string.Equals(_lastSelfUpdateProgressKey, progressKey, StringComparison.Ordinal))
                return;

            _lastSelfUpdateProgressKey = progressKey;
            Console.WriteLine($"  [steamcmd] {LastSelfUpdateSummary}");
        }

        private static string FormatBytes(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            var kb = bytes / 1024d;
            if (kb < 1024) return $"{kb:0.##} KB";
            var mb = kb / 1024d;
            if (mb < 1024) return $"{mb:0.##} MB";
            return $"{mb / 1024d:0.##} GB";
        }

        private static string FormatSelfUpdateSummary(SteamCmdUpdateProgress updateProgress)
        {
            var percentText = updateProgress.Percent.HasValue
                ? $" {updateProgress.Percent.Value.ToString(CultureInfo.InvariantCulture)}%"
                : "";
            var sizeText = updateProgress.DownloadedKilobytes.HasValue && updateProgress.TotalKilobytes.HasValue
                ? $" ({FormatBytes(updateProgress.DownloadedKilobytes.Value * 1024)}/{FormatBytes(updateProgress.TotalKilobytes.Value * 1024)})"
                : "";
            return $"self-update: {updateProgress.Phase}{percentText}{sizeText}";
        }
    }

    private sealed class SteamCmdLogMonitor
    {
        private readonly string _logsDir;
        private readonly SteamCmdProgress _progress;
        private readonly Dictionary<string, long> _offsets = new(StringComparer.OrdinalIgnoreCase);
        private string? _activeLog;

        public SteamCmdLogMonitor(string steamcmdWorkingDir, SteamCmdProgress progress)
        {
            _logsDir = Path.Combine(steamcmdWorkingDir, "logs");
            _progress = progress;
            foreach (var file in GetLogFiles())
                _offsets[file] = GetFileLength(file);
        }

        public async Task MonitorAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ReadAndEmit();
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public void ReadAndEmit()
        {
            foreach (var file in GetLogFiles())
            {
                if (!_offsets.ContainsKey(file))
                    _offsets[file] = 0;

                var offset = _offsets[file];
                var text = ReadNewText(file, ref offset);
                _offsets[file] = offset;
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                if (_activeLog == null && ParseDownloadProgressEvents(text).Count > 0)
                {
                    _activeLog = file;
                    Console.WriteLine($"  [steamcmd] progress log: {file}");
                }

                foreach (var line in text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
                    ParseProgressLine(line, _progress);
            }
        }

        private IEnumerable<string> GetLogFiles()
        {
            if (!Directory.Exists(_logsDir))
                return [];

            var files = Directory
                .EnumerateFiles(_logsDir, "*.*", SearchOption.AllDirectories)
                .Where(file =>
                    Path.GetExtension(file).Equals(".txt", StringComparison.OrdinalIgnoreCase) ||
                    Path.GetExtension(file).Equals(".log", StringComparison.OrdinalIgnoreCase))
                .OrderBy(file => Path.GetFileName(file).Equals("console_log.txt", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenByDescending(file => SafeLastWriteTimeUtc(file))
                .ToList();
            return files;
        }

        private static string ReadNewText(string path, ref long offset)
        {
            try
            {
                if (!File.Exists(path))
                    return "";

                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                if (fs.Length < offset)
                    offset = 0;

                fs.Seek(offset, SeekOrigin.Begin);
                using var reader = Utf8NoBom.CreateStreamReader(fs);
                var text = reader.ReadToEnd();
                offset = fs.Position;
                return text;
            }
            catch
            {
                return "";
            }
        }

        private static long GetFileLength(string path)
        {
            try { return File.Exists(path) ? new FileInfo(path).Length : 0; }
            catch { return 0; }
        }

        private static DateTime SafeLastWriteTimeUtc(string path)
        {
            try { return File.GetLastWriteTimeUtc(path); }
            catch { return DateTime.MinValue; }
        }
    }
}

public readonly record struct SteamCmdDownloadProgress(string ModId, bool Completed, long Bytes);
public readonly record struct SteamCmdUpdateProgress(
    int? Percent,
    string Phase,
    long? DownloadedKilobytes,
    long? TotalKilobytes);

internal enum SteamCmdOutputLineKind
{
    Other,
    DownloadProgress,
    TransientProgress
}
