using Common;
using SteamCmdBootstrapper;

namespace TranslationPipeline.Tests;

public class SteamCmdBootstrapperTests
{
    [Fact]
    public async Task BootstrapAsync_WindowsRequiresBundledExecutable()
    {
        if (!OperatingSystem.IsWindows())
            return;

        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = TestConfig.Create();
            config.baseDir = tempDir;
            var service = new SteamCmdBootstrapperService(config);

            var exception = await Assert.ThrowsAsync<FileNotFoundException>(() => service.BootstrapAsync());

            Assert.Contains(Path.Combine("src", "3rd_party", "steamcmd", "steamcmd.exe"), exception.Message);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}