using System.Text.RegularExpressions;

namespace Common;

/// <summary>Clean Steam BBCode mod descriptions: strip tags, keep text, compress whitespace.</summary>
public static partial class DescriptionCleaner
{
    // Known Steam BBCode tag names. Ordered: longer first to avoid partial match.
    private const string BbTags =
        @"noparse|spoiler|strike|olist|quote|list|code" +
        @"|h[123]|img|url|hr" +
        @"|[buin]" +
        @"|\*";

    [GeneratedRegex(@"\[(?:" + BbTags + @")(?:=[^\]]*)?\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex BbOpen();

    [GeneratedRegex(@"\[/(?:" + BbTags + @")\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex BbClose();

    [GeneratedRegex(@"\[img(?:=[^\]]*)?\].*?\[/img\]", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled, "en-US")]
    private static partial Regex ImgBlock();

    [GeneratedRegex(@"\[url=[^\]]*\]\s*\[/url\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex EmptyUrl();

    [GeneratedRegex(@"\[quote=([^\]]*)\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex QuoteOpen();

    [GeneratedRegex(@"\[/quote\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex QuoteClose();

    [GeneratedRegex(@"\[url=[^\]]*\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex UrlOpen();

    [GeneratedRegex(@"\[/url\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex UrlClose();

    [GeneratedRegex(@"\[table(?:=[^\]]*)?\].*?\[/table\]", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled, "en-US")]
    private static partial Regex TableBlock();

    [GeneratedRegex(@"\[tr(?:=[^\]]*)?\].*?\[/tr\]", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled, "en-US")]
    private static partial Regex TrBlock();

    [GeneratedRegex(@"\[th(?:=[^\]]*)?\].*?\[/th\]", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled, "en-US")]
    private static partial Regex ThBlock();

    [GeneratedRegex(@"\[/?(?:table|tr|td|th|tbody|thead|center|left|right)(?:=[^\]]*)?\]", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex LayoutTags();

    [GeneratedRegex(@"\n{2,}", RegexOptions.Compiled, "en-US")]
    private static partial Regex MultiNewline();

    [GeneratedRegex(@"\s+", RegexOptions.Compiled, "en-US")]
    private static partial Regex MultiSpace();

    /// <summary>Strip Steam BBCode, keep text, compress whitespace. Preserves [strike] semantic.</summary>
    public static string Clean(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "";

        var text = raw;

        // 1. Protect [strike]
        text = text.Replace("[strike]", "__STRIKE_OPEN__");
        text = text.Replace("[/strike]", "__STRIKE_CLOSE__");

        // 2. Normalize newlines
        text = text.Replace("\r\n", "\n").Replace('\r', '\n');

        // 3. Remove [img]...[/img] entirely (including content)
        text = ImgBlock().Replace(text, "");

        // 4. Remove empty [url=...] tags left after img removal: [url=xxx][/url]
        text = EmptyUrl().Replace(text, "");

        // 5. Convert [quote=author]...[/quote]
        text = QuoteOpen().Replace(text, "Quoted from $1:\n");
        text = QuoteClose().Replace(text, "");

        // 6. Remove [url=...] and [/url], keep display text
        text = UrlOpen().Replace(text, "");
        text = UrlClose().Replace(text, "");

        // 6a. Drop Steam layout/table blocks
        text = TableBlock().Replace(text, "");
        text = TrBlock().Replace(text, "");
        text = ThBlock().Replace(text, "");

        // 7. Remove all other BBCode tags
        text = BbOpen().Replace(text, "");
        text = BbClose().Replace(text, "");

        // 7a. Remove leftover exact Steam formatting tags
        text = LayoutTags().Replace(text, "");

        // 8. Restore [strike]
        text = text.Replace("__STRIKE_OPEN__", "[strike]");
        text = text.Replace("__STRIKE_CLOSE__", "[/strike]");

        // 9. Collapse consecutive newlines (2+ -> 1), then whitespace -> single space
        text = MultiNewline().Replace(text, "\n");
        text = MultiSpace().Replace(text, " ").Trim();

        return text;
    }
}
