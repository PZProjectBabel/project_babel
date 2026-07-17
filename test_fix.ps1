using System.Text.RegularExpressions;
string[] lines = { "- **Embedding**: Text vectorization", "**Content Review**: LLM review" };
var result = new List<string>();
for (int i = 0; i < lines.Length; i++) {
    var line = lines[i]; var trimmed = line.Trim();
    if (trimmed.StartsWith("- ") || trimmed.StartsWith("* ") || Regex.IsMatch(trimmed, @"^\d+\.\s")) { result.Add(line); continue; }
    var prevIsListItem = i > 0 && (lines[i-1].TrimStart().StartsWith("- ") || lines[i-1].TrimStart().StartsWith("* ") || Regex.IsMatch(lines[i-1].TrimStart(), @"^\d+\.\s"));
    if (!prevIsListItem) { result.Add(line); continue; }
    var looksLikeContent = Regex.IsMatch(trimmed, @"^\*\*?[^*]+\*\*?:\s");
    var hasMalformedMarker = Regex.IsMatch(trimmed, @"^[*-]-\s");
    if (looksLikeContent || hasMalformedMarker) {
        var cleanContent = Regex.Replace(trimmed, @"^[*-]-\s", "");
        cleanContent = cleanContent.Trim();
        var indent = line.Length - trimmed.Length > 0 ? line[..(line.Length - trimmed.Length)] : "";
        result.Add(indent + "- " + cleanContent);
        continue;
    }
    result.Add(line);
}
Console.WriteLine(result[0]);
Console.WriteLine(result[1]);
