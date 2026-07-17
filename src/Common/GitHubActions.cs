namespace Common;

/// <summary>
/// Writes GitHub Actions workflow annotations with the required escaping.
/// </summary>
public static class GitHubActions
{
    /// <summary>Writes an error-level workflow annotation to stderr.</summary>
    public static void Error(string message, string? title = null, string? file = null, int? line = null)
    {
        Console.Error.WriteLine(FormatAnnotation("error", message, title, file, line));
    }

    /// <summary>Writes a warning-level workflow annotation to stderr.</summary>
    public static void Warning(string message, string? title = null, string? file = null, int? line = null)
    {
        Console.Error.WriteLine(FormatAnnotation("warning", message, title, file, line));
    }

    /// <summary>Formats a GitHub Actions workflow command string with property escaping.</summary>
    private static string FormatAnnotation(string level, string message, string? title, string? file, int? line)
    {
        var properties = new List<string>();
        if (!string.IsNullOrWhiteSpace(title))
            properties.Add($"title={EscapeProperty(title)}");
        if (!string.IsNullOrWhiteSpace(file))
            properties.Add($"file={EscapeProperty(file)}");
        if (line.HasValue)
            properties.Add($"line={line.Value}");

        var propertyText = properties.Count == 0 ? "" : " " + string.Join(",", properties);
        return $"::{level}{propertyText}::{EscapeData(message)}";
    }

    /// <summary>Percent-encodes CR, LF, and % characters in annotation data.</summary>
    private static string EscapeData(string value) =>
        value
            .Replace("%", "%25")
            .Replace("\r", "%0D")
            .Replace("\n", "%0A");

    /// <summary>Percent-encodes property values, additionally escaping colon and comma.</summary>
    private static string EscapeProperty(string value) =>
        EscapeData(value)
            .Replace(":", "%3A")
            .Replace(",", "%2C");
}
