using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Common;

/// <summary>
/// Centralized UTF-8 without BOM encoding and JSON serialization helpers.
/// Every file read/write and JSON operation in the pipeline should flow through these.
/// </summary>
public static class Utf8NoBom
{
    /// <summary>UTF-8 encoding that never emits a byte-order mark.</summary>
    public static readonly UTF8Encoding Encoding = new(encoderShouldEmitUTF8Identifier: false);

    /// <summary>JSON serializer options with relaxed Unicode escaping (preserves CJK, etc.).</summary>
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>JSON serializer options with relaxed escaping and indented formatting.</summary>
    public static readonly JsonSerializerOptions IndentedJsonOptions = new(JsonOptions)
    {
        WriteIndented = true
    };

    /// <summary>Serialize value to compact JSON string.</summary>
    public static string SerializeJson<TValue>(TValue value) =>
        JsonSerializer.Serialize(value, JsonOptions);

    /// <summary>Serialize value to indented JSON string.</summary>
    public static string SerializeIndentedJson<TValue>(TValue value) =>
        JsonSerializer.Serialize(value, IndentedJsonOptions);

    /// <summary>Read entire file as UTF-8 (no BOM).</summary>
    public static string ReadAllText(string path) =>
        File.ReadAllText(path, Encoding);

    /// <summary>Asynchronously read entire file as UTF-8 (no BOM).</summary>
    public static Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default) =>
        File.ReadAllTextAsync(path, Encoding, cancellationToken);

    /// <summary>Read all lines from a file as UTF-8 (no BOM).</summary>
    public static string[] ReadAllLines(string path) =>
        File.ReadAllLines(path, Encoding);

    /// <summary>Asynchronously read all lines from a file as UTF-8 (no BOM).</summary>
    public static Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default) =>
        File.ReadAllLinesAsync(path, Encoding, cancellationToken);

    /// <summary>Write text to file as UTF-8 (no BOM).</summary>
    public static void WriteAllText(string path, string contents) =>
        File.WriteAllText(path, contents, Encoding);

    /// <summary>Asynchronously write text to file as UTF-8 (no BOM).</summary>
    public static Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default) =>
        File.WriteAllTextAsync(path, contents, Encoding, cancellationToken);

    /// <summary>Append lines to file as UTF-8 (no BOM).</summary>
    public static void AppendAllLines(string path, IEnumerable<string> contents) =>
        File.AppendAllLines(path, contents, Encoding);

    /// <summary>Create a StreamWriter with UTF-8 (no BOM) encoding.</summary>
    public static StreamWriter CreateStreamWriter(string path, bool append = false) =>
        new(path, append, Encoding);

    /// <summary>Create a StreamReader that auto-detects encoding but defaults to UTF-8 (no BOM).</summary>
    public static StreamReader CreateStreamReader(Stream stream) =>
        new(stream, Encoding, detectEncodingFromByteOrderMarks: true);
}
