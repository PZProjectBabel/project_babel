using Common;
using DocGenerator;

Console.OutputEncoding = Utf8NoBom.Encoding;
Console.InputEncoding = Utf8NoBom.Encoding;

try
{
    var repoRoot = FindRepositoryRoot();
    Console.WriteLine($"[DocGen] Repository root: {repoRoot}");

    var service = new DocGeneratorService(repoRoot);
    await service.RunAsync();

    Console.WriteLine("\n[DocGen] Done.");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"\n[DocGen] FATAL: {ex.Message}");
    if (!string.IsNullOrWhiteSpace(ex.StackTrace))
        Console.Error.WriteLine(ex.StackTrace);
    Environment.Exit(1);
}

static string FindRepositoryRoot()
{
    var dir = AppDomain.CurrentDomain.BaseDirectory;
    while (!string.IsNullOrEmpty(dir))
    {
        if (File.Exists(Path.Combine(dir, "README.md"))
            && Directory.Exists(Path.Combine(dir, "src"))
            && Directory.Exists(Path.Combine(dir, "config")))
        {
            return dir;
        }
        var parent = Path.GetDirectoryName(dir);
        if (parent == dir) break;
        dir = parent!;
    }
    // Fallback: use relative path from project location.
    return Path.GetFullPath(Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", ".."));
}
