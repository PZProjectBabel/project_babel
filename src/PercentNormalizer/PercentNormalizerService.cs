using System.Text;
using System.Text.RegularExpressions;

namespace PercentNormalizer;

/// <summary>
/// Normalizes percent signs to the Project Zomboid Build 42.20.1+ canonical format.
/// This is the permanent pipeline guard: every text entering the translation database
/// (extracted mod text or LLM translation output) must pass through
/// <see cref="PercentNormalizerService.Normalize"/>.
/// <para>
/// Kept as-is (never modified):
///  - "%%" — an already-escaped literal percent;
///  - "%1".."%9" — PZ positional placeholders;
///  - "%N$c" — Java indexed placeholders (e.g. "%1$s");
///  - Java formatter conversions ("%d", "%s", "%10s", "%.2f", ...).
/// </para>
/// <para>
/// Modified (old-format literal percents):
///  - a lone "%" becomes "%%" ("100%" → "100%%");
///  - a lone "%" right after a placeholder also becomes "%%" ("%1%" → "%1%%").
/// </para>
/// Explicitly NOT a naive replace("%","%%") and never collapses "%%%%" to "%%",
/// so already-canonical new-format text is never damaged.
/// The one-time migration tool (temp/MigratePercentFormat) mirrors this logic.
/// </summary>
public static class PercentNormalizerService
{
    // Ordered alternatives:
    //  1. "%%" escaped pair
    //  2. "%1$s" indexed Java placeholder (also "%12$s")
    //  3. "%1".."%9" PZ positional placeholder
    //  4. Java conversion with optional flags/width/precision, e.g. "%d", "%s", "%10s", "%.2f".
    //     Note: the space flag is intentionally excluded so plain text like "% chance"
    //     is treated as a literal percent, not a Java format specifier; the conversion
    //     letter set is restricted to real Java conversions so "%z" stays a literal.
    private const string JavaConversions = "aAbBcCdDeEfgGhHnsoSxX";
    private static readonly Regex KeepToken = new(
        $@"\G(?:%%|%[1-9]\d*\$[{JavaConversions}]|%[1-9]|%[-#+0,(]*\d*(?:\.\d+)?[{JavaConversions}])",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>Normalizes a text to the canonical PZ 42.20.1+ percent format.</summary>
    public static string Normalize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text ?? "";

        var sb = new StringBuilder(text.Length + 8);
        var i = 0;
        while (i < text.Length)
        {
            if (text[i] != '%')
            {
                sb.Append(text[i]);
                i++;
                continue;
            }

            var match = KeepToken.Match(text, i);
            if (match.Success)
            {
                sb.Append(match.Value);
                i += match.Length;
                continue;
            }

            // Lone literal percent in old format → escape it.
            sb.Append("%%");
            i++;
        }

        return sb.ToString();
    }
}
