using System.Text.Json;
using Common;

namespace WorkshopMonitor;

/// <summary>
/// Entry point: reads Steam API key from config, runs the monitor, and exits.
/// </summary>
public class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Utf8NoBom.Encoding;

        try
        {
            var configDir = ResolveConfigDir();
            var dataDir = ResolveDataDir(configDir);
            var apiKey = ReadSteamApiKey(configDir);

            var monitor = new WorkshopMonitorService(apiKey, configDir, dataDir);
            await monitor.RunAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal: {ex.Message}");
            Environment.Exit(1);
        }
    }

    private static string ResolveConfigDir()
    {
        // Try relative from cwd first, then relative to assembly location.
        var cwd = Path.Combine(Directory.GetCurrentDirectory(), "config");
        if (Directory.Exists(cwd)) return cwd;

        var asm = Path.Combine(AppContext.BaseDirectory, "config");
        if (Directory.Exists(asm)) return asm;

        // Fallback: two levels up from assembly (for src/WorkshopMonitor/bin/...)
        var up = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "config"));
        if (Directory.Exists(up)) return up;

        throw new DirectoryNotFoundException($"config directory not found. Searched: {cwd}, {asm}, {up}");
    }

    private static string ResolveDataDir(string configDir)
    {
        var dataDir = Path.GetFullPath(Path.Combine(configDir, "..", "data"));
        if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
        return dataDir;
    }

    public static string ReadSteamApiKey(string configDir)
    {
        // 1) secrets.json (same pattern as ConfigReader)
        var secretsFile = Path.Combine(configDir, "secrets.json");
        if (File.Exists(secretsFile))
        {
            try
            {
                var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(Utf8NoBom.ReadAllText(secretsFile));
                if (secrets != null && secrets.TryGetValue("STEAM_KEY", out var sk) && !string.IsNullOrWhiteSpace(sk))
                    return sk;
            }
            catch { /* fall through to env var */ }
        }

        // 2) Environment variable
        var env = Environment.GetEnvironmentVariable("STEAM_KEY")
               ?? Environment.GetEnvironmentVariable("STEAM_API_KEY");
        if (!string.IsNullOrWhiteSpace(env))
            return env;

        throw new InvalidOperationException("STEAM_KEY not found in secrets.json or environment variables.");
    }
}
