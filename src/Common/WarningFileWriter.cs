using System.Text.RegularExpressions;

namespace Common;

/// <summary>
    /// Writes structured warning files for later pipeline-level aggregation.
    /// </summary>
    public static class WarningFileWriter
{
    /// <summary>
    /// Writes a pipeline warning to a timestamped JSON file under the warnings temp directory.
    /// Returns the written file path, or null if the directory is not configured.
    /// </summary>
    public static string? Write(
        PipelineConfig config,
        string moduleName,
        string? batchId,
        PipelineWarning warning)
    {
        if (string.IsNullOrWhiteSpace(config.warningsTempDir))
            return null;

        Directory.CreateDirectory(config.warningsTempDir);
        var safeModule = SafeName(moduleName);
        var safeBatch = string.IsNullOrWhiteSpace(batchId) ? "" : "_" + SafeName(batchId);
        var random = Guid.NewGuid().ToString("N")[..12];
        var path = Path.Combine(config.warningsTempDir, $"{safeModule}{safeBatch}_{random}.json");
        Utf8NoBom.WriteAllText(path, Utf8NoBom.SerializeIndentedJson(warning));
        return path;
    }

    private static string SafeName(string value)
    {
        var safe = Regex.Replace(value, @"[^A-Za-z0-9_-]+", "_").Trim('_');
        return string.IsNullOrWhiteSpace(safe) ? "unknown" : safe;
    }
}

/// <summary>Structured warning record written for diagnostics and aggregation.</summary>
public sealed class PipelineWarning
{
    /// <summary>UTC timestamp when the warning was recorded.</summary>
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    /// <summary>Name of the module that produced this warning.</summary>
    public string ModuleName { get; set; } = "";
    /// <summary>Optional batch identifier for context.</summary>
    public string? BatchId { get; set; }
    /// <summary>Mod ID related to the warning, if applicable.</summary>
    public string? ModId { get; set; }
    /// <summary>Mod name for human-readable context.</summary>
    public string? ModName { get; set; }
    /// <summary>File path where the issue occurred, if applicable.</summary>
    public string? FilePath { get; set; }
    /// <summary>Line number (1-based) where the issue occurred.</summary>
    public long? LineNumber { get; set; }
    /// <summary>Byte position within the line, if relevant.</summary>
    public long? BytePositionInLine { get; set; }
    /// <summary>Target language ISO code related to the warning.</summary>
    public string? TargetLang { get; set; }
    /// <summary>Number of retry attempts before the warning was raised.</summary>
    public int? AttemptCount { get; set; }
    /// <summary>Short classification of the error type.</summary>
    public string ErrorType { get; set; } = "";
    /// <summary>Human-readable warning message.</summary>
    public string Message { get; set; } = "";
}
