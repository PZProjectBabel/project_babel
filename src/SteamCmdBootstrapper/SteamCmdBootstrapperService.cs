using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Common;

namespace SteamCmdBootstrapper;

public class SteamCmdBootstrapperService
{
    private readonly PipelineConfig _config;
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(120) };

    private const string WindowsUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
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
        RecreateDestinationDirectory(destinationDirectory);

        string url = OperatingSystem.IsWindows() ? WindowsUrl : LinuxUrl;
        string temporaryFile = Path.GetTempFileName();
        try
        {
            await DownloadWithRetryAsync(url, temporaryFile);
            Console.WriteLine($"  [steamcmd] Downloaded {new FileInfo(temporaryFile).Length / 1024} KB");

            if (OperatingSystem.IsWindows())
            {
                ZipFile.ExtractToDirectory(temporaryFile, destinationDirectory, overwriteFiles: true);
            }
            else
            {
                await ExtractLinuxArchiveAsync(temporaryFile, destinationDirectory);
                ChmodIfLinux(Path.Combine(destinationDirectory, "steamcmd.sh"));
                string linux32Steamcmd = Path.Combine(destinationDirectory, "linux32", "steamcmd");
                if (File.Exists(linux32Steamcmd))
                    ChmodIfLinux(linux32Steamcmd);
            }

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

    private static void RecreateDestinationDirectory(string destinationDirectory)
    {
        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
            return;
        }

        foreach (string entry in Directory.GetFileSystemEntries(destinationDirectory))
        {
            if (Path.GetFileName(entry) == ".gitignore")
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