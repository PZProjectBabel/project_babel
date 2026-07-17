using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common;

namespace DocGenerator;

/// <summary>
/// Category of a template line for translation decision.
/// </summary>
public enum TemplateLineCategory
{
    /// <summary>Empty or whitespace-only line — never translate.</summary>
    Blank,
    /// <summary>Pure markdown formatting (---, #, |---|, etc.) — never translate.</summary>
    MarkdownTag,
    /// <summary>Only contains {{...}} placeholders — never translate.</summary>
    Placeholder,
    /// <summary>Contains actual text that may need translation.</summary>
    Translatable
}

/// <summary>
/// Parsed line from a template document.
/// </summary>
public sealed class DocTemplateLine
{
    public int LineNumber { get; init; }
    public string Text { get; init; } = "";
    public string Sha256 { get; init; } = "";

    [JsonIgnore]
    public TemplateLineCategory Category { get; init; }

    /// <summary>Per-target-language: whether this line needs LLM translation.</summary>
    public Dictionary<string, bool> NeedsTranslation { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    public static DocTemplateLine FromLine(int lineNumber, string text)
    {
        var category = ClassifyLine(text);
        var sha256 = ComputeSha256(text);
        return new DocTemplateLine
        {
            LineNumber = lineNumber,
            Text = text,
            Sha256 = sha256,
            Category = category,
            // Initially mark all Translatable lines as needing translation;
            // LLM may override this later for false-positives (code, signatures etc.)
            NeedsTranslation = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        };
    }

    /// <summary>
    /// Quick heuristic classification. Translatable lines may be further refined by LLM.
    /// </summary>
    public static TemplateLineCategory ClassifyLine(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return TemplateLineCategory.Blank;

        var trimmed = text.Trim();

        // Pure markdown structural elements.
        if (IsPureMarkdownTag(trimmed))
            return TemplateLineCategory.MarkdownTag;

        // Only placeholders: line contains only {{...}} patterns and whitespace.
        if (IsOnlyPlaceholders(trimmed))
            return TemplateLineCategory.Placeholder;

        return TemplateLineCategory.Translatable;
    }

    private static bool IsPureMarkdownTag(string trimmed)
    {
        // Horizontal rules.
        if (trimmed is "---" or "***" or "___" or "===")
            return true;

        // Pure heading markers (e.g. "#", "##", "###" with no following text).
        if (trimmed.All(c => c == '#'))
            return true;

        // Table separator rows (e.g. "|------|------|").
        if (trimmed.StartsWith('|') && trimmed.EndsWith('|')
            && trimmed.Replace("|", "").Replace("-", "").Replace(":", "").Replace(" ", "").Length == 0)
            return true;

        // Pure divider comment lines like "<!-- ... -->"
        if (trimmed.StartsWith("<!--") && trimmed.EndsWith("-->"))
            return true;

        return false;
    }

    private static bool IsOnlyPlaceholders(string trimmed)
    {
        // Remove all {{...}} blocks and check if anything remains.
        var withoutPlaceholders = System.Text.RegularExpressions.Regex.Replace(
            trimmed, @"\{\{.*?\}\}", "");
        return string.IsNullOrWhiteSpace(withoutPlaceholders);
    }

    public static string ComputeSha256(string text)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        return Convert.ToHexStringLower(hash);
    }
}

/// <summary>
/// Cache entry for a single template line, persisted as JSON array per template file.
/// </summary>
public sealed class TemplateCacheEntry
{
    /// <summary>SHA-256 of the source (Chinese) text.</summary>
    public string Sha256 { get; set; } = "";

    /// <summary>The original Chinese source text.</summary>
    public string SourceText { get; set; } = "";

    /// <summary>Per-language translations. Missing key = not yet translated.</summary>
    public Dictionary<string, string?> Translations { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Per-language: whether this line needs translation. true = needs LLM, false = skip.</summary>
    public Dictionary<string, bool> NeedsTranslation { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Returns true if the given language has a usable cached translation.</summary>
    public bool HasTranslation(string lang)
    {
        return Translations.TryGetValue(lang, out var text) && !string.IsNullOrWhiteSpace(text);
    }

    /// <summary>Returns true if needs-translation flag is set to false for this language.</summary>
    public bool IsExplicitlySkipped(string lang)
    {
        return NeedsTranslation.TryGetValue(lang, out var needs) && !needs;
    }
}

/// <summary>
/// A batch of template lines to translate, for one target language.
/// </summary>
public sealed class DocTranslationBatch
{
    public string TargetLang { get; init; } = "";
    public int BatchIndex { get; init; }
    public int TotalBatches { get; init; }
    public List<DocTemplateLine> Lines { get; init; } = [];
}

/// <summary>
/// Parsed result from a single LLM translation response line.
/// </summary>
public sealed class LlmLineResult
{
    public int LineNumber { get; init; }
    public string TranslatedText { get; set; } = "";
    public float Confidence { get; set; }
    public string? Comment { get; set; }
}

/// <summary>
/// Deserialized from *_links_mapping.json. Contains all placeholder definitions
/// for a template: multi-lang file links, markdown links, raw URLs, and named links.
/// </summary>
public sealed class LinksMapping
{
    public MultiLangBlock? multi_lang_file_links_block { get; set; }

    /// <summary>
    /// md_file_links_blocks values can be either a string (plain URL, no link text)
    /// or an object {text, url}. Use JsonElement to handle both at parse time.
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, MdLinkDef> MdLinkDefs { get; set; } = [];

    public Dictionary<string, string>? url_blocks { get; set; }
    public Dictionary<string, string>? named_links { get; set; }

    // Raw deserialization target — processed into MdLinkDefs after deserialization.
    [JsonPropertyName("md_file_links_blocks")]
    public Dictionary<string, JsonElement>? md_file_links_blocks_raw
    {
        set
        {
            MdLinkDefs = [];
            if (value == null) return;
            foreach (var kv in value)
            {
                if (kv.Value.ValueKind == JsonValueKind.String)
                {
                    // Plain URL string — use the URL itself as link text.
                    var url = kv.Value.GetString() ?? "";
                    MdLinkDefs[kv.Key] = new MdLinkDef { text = url, url = url };
                }
                else if (kv.Value.ValueKind == JsonValueKind.Object)
                {
                    var def = JsonSerializer.Deserialize<MdLinkDef>(
                        kv.Value.GetRawText(), Utf8NoBom.JsonOptions);
                    if (def != null)
                        MdLinkDefs[kv.Key] = def;
                }
            }
        }
        get => null; // Never serialized back.
    }
}

public sealed class MultiLangBlock
{
    public Dictionary<string, LangLinkDef>? primary_links { get; set; }
    public List<LangLinkDef>? language_links { get; set; }
}

public sealed class LangLinkDef
{
    public string text { get; set; } = "";
    public string url { get; set; } = "";
}

public sealed class MdLinkDef
{
    public string text { get; set; } = "";
    public string url { get; set; } = "";
}
