using System.Diagnostics;
using System.Runtime.InteropServices;
using Common;

namespace SteamCmdBootstrapper;

public class SteamCmdBootstrapperService
{
    private readonly PipelineConfig _config;
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(120) };

    private const string LinuxUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz";
    private const int MaxRetries = 3;

    public SteamCmdBootstrapperService(PipelineConfig config)
    {
        _config = config;
    }

    public async Task BootstrapAsync()
    {
        string destinationDirectory = Path.Combine(_config.baseDir, "src", "3rd_party", "steamcmd");
        string executableName = OperatingSystem.IsWindows() ? "steamcmd.exe" : "steamcmd.sh";
        string executablePath = Path.Combine(destinationDirectory, executableName);

        Console.WriteLine($"[steamcmd] Bootstrapping for {RuntimeInformation.RuntimeIdentifier}...");

        if (OperatingSystem.IsWindows())
        {
            await UpdateBundledWindowsSteamCmdAsync(destinationDirectory, executablePath);
            return;
        }

        RecreateDestinationDirectory(destinationDirectory);

        string temporaryFile = Path.GetTempFileName();
        try
        {
            await DownloadWithRetryAsync(LinuxUrl, temporaryFile);
            Console.WriteLine($"  [steamcmd] Downloaded {new FileInfo(temporaryFile).Length / 1024} KB");

            await ExtractLinuxArchiveAsync(temporaryFile, destinationDirectory);
            ChmodIfLinux(Path.Combine(destinationDirectory, "steamcmd.sh"));
            string linux32Steamcmd = Path.Combine(destinationDirectory, "linux32", "steamcmd");
            if (File.Exists(linux32Steamcmd))
                ChmodIfLinux(linux32Steamcmd);

            if (!File.Exists(executablePath))
                throw new FileNotFoundException($"steamcmd executable missing after extract: {executablePath}");

            Console.WriteLine($"  [OK] steamcmd ready: {executablePath}");
        }
        finally
        {
            if (File.Exists(temporaryFile))
                File.Delete(temporaryFile);
        }
    }

    private static async Task UpdateBundledWindowsSteamCmdAsync(string destinationDirectory, string executablePath)
    {
        // 更新前不删除/移动现有 exe：steamcmd 自更新只会在覆盖文件时才把旧版本备份为 .old。
        // 若上次更新中断导致 exe 缺失（仅剩 .old 备份），先从备份恢复再继续。
        TryRestoreFromBackup(executablePath);
        if (!File.Exists(executablePath))
            throw new FileNotFoundException($"Bundled Windows steamcmd executable not found: {executablePath}");

        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = executablePath,
            WorkingDirectory = destinationDirectory,
            UseShellExecute = false,
            CreateNoWindow = true,
            ArgumentList = { "+quit" }
        }) ?? throw new InvalidOperationException($"Failed to start bundled steamcmd: {executablePath}");

        await process.WaitForExitAsync();

        // 更新后：若 steamcmd 覆盖文件失败导致 exe 缺失，再次尝试从 .old 备份恢复。
        if (!File.Exists(executablePath))
        {
            TryRestoreFromBackup(executablePath);
            if (!File.Exists(executablePath))
                throw new InvalidOperationException($"Bundled steamcmd update failed: executable missing after update: {executablePath}");
        }
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Bundled steamcmd update failed with exit code {process.ExitCode}.");

        Console.WriteLine($"  [OK] bundled steamcmd updated: {executablePath}");
    }

    /// <summary>
    /// 若 exe 缺失但存在 steamcmd 自更新留下的 .old 备份，则复制恢复（保留 .old，便于再次恢复）。
    /// </summary>
    private static void TryRestoreFromBackup(string executablePath)
    {
        string backupPath = executablePath + ".old";
        if (File.Exists(executablePath) || !File.Exists(backupPath))
            return;

        File.Copy(backupPath, executablePath, overwrite: true);
        Console.WriteLine($"  [steamcmd] Restored {Path.GetFileName(executablePath)} from {Path.GetFileName(backupPath)}");
    }

    private static void RecreateDestinationDirectory(string destinationDirectory)
    {
        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
            return;
        }

        foreach (string entry in Directory.GetFileSystemEntries(destinationDirectory))
        {
            string fileName = Path.GetFileName(entry);
            // 保留 .gitignore 以及 Windows 平台二进制与其 .old 备份：
            // Windows 版本由 Windows 分支维护，Linux 更新不得删除。
            if (fileName == ".gitignore" ||
                fileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".old", StringComparison.OrdinalIgnoreCase))
                continue;

            if (Directory.Exists(entry))
                Directory.Delete(entry, true);
            else
                File.Delete(entry);
        }
    }

    private static async Task ExtractLinuxArchiveAsync(string archivePath, string destinationDirectory)
    {
        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = "tar",
            Arguments = $"-xzf \"{archivePath}\" -C \"{destinationDirectory}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }) ?? throw new InvalidOperationException("Failed to start tar for steamcmd extraction.");

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
        {
            string error = await process.StandardError.ReadToEndAsync();
            throw new InvalidOperationException($"tar extract failed (exit {process.ExitCode}): {error}");
        }
    }

    private static async Task DownloadWithRetryAsync(string url, string destinationPath)
    {
        Exception? lastException = null;
        for (int attempt = 0; attempt < MaxRetries; attempt++)
        {
            try
            {
                using var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                await using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream);
                return;
            }
            catch (Exception exception)
            {
                lastException = exception;
                if (attempt == MaxRetries - 1)
                    break;

                Console.WriteLine($"  [steamcmd] Download attempt {attempt + 1}/{MaxRetries} failed: {exception.Message}");
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
        }

        throw new HttpRequestException($"Failed to download steamcmd after {MaxRetries} attempts.", lastException);
    }

    private static void ChmodIfLinux(string path)
    {
        if (OperatingSystem.IsWindows())
            return;

        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "chmod",
                Arguments = $"+x \"{path}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            process?.WaitForExit();
        }
        catch (Exception exception)
        {
            GitHubActions.Warning($"chmod +x failed for {path}: {exception.Message}", "steamcmd");
        }
    }
}