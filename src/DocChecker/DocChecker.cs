using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using Common;

namespace DocChecker;

public static partial class DocChecker
{
    private static readonly string BaseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static readonly HttpClient Http = new();

    // ── Doc families ──────────────────────────────────────────
    private static readonly List<DocFamily> Families =
    [
        new("technical_reference", "Technical Reference",
            Path.Combine(BaseDir, "docs", "technical_reference"),
            "technical_reference_zh-hans.md",
            "technical_reference_*.md",
            ["technical_reference_zh-hans.md", "technical_reference_zh-hant.md"],
            "technical_reference_"),
        new("readme", "README",
            Path.Combine(BaseDir, "docs", "readme"),
            "README_zh-hans.md",
            "README_*.md",
            ["README_zh-hans.md", "README_zh-hant.md"],
            "README_", Path.Combine(BaseDir, "README.md")),
        new("contributing", "Contributing",
            Path.Combine(BaseDir, "docs", "contributing"),
            "contributing_zh-hans.md",
            "contributing_*.md",
            ["contributing_zh-hans.md", "contributing_zh-hant.md"],
            "contributing_"),
    ];

    // ── Config (lazy async) ───────────────────────────────────
    private static string LmKey => _lmKey ??= LoadJsonProp("secrets.json", "LLM_KEY");
    private static string? _lmKey;
    private static string LmEndpoint => _lmEndpoint ??= LoadJsonProp("config.json", e => e.GetProperty("LLM").GetProperty("api_endpoint").GetString()!);
    private static string? _lmEndpoint;

    private static string LoadJsonProp(string file, Func<JsonElement, string> extract)
    {
        using var fs = File.OpenRead(Path.Combine(BaseDir, "config", file));
        return extract(JsonDocument.Parse(fs).RootElement);
    }
    private static string LoadJsonProp(string file, string prop) => LoadJsonProp(file, e => e.GetProperty(prop).GetString()!);
    private const string LmModel = "deepseek-v4-flash";
    private const int LmTimeout = 300;
    private const int MaxConcur = 256;
    private const int MaxRetries = 3;

    // ── Lang names ────────────────────────────────────────────
    private static readonly Dictionary<string, string> LangNames = new()
    {
        ["ar"] = "العربية", ["ca"] = "català", ["cs"] = "čeština", ["da"] = "dansk",
        ["de"] = "Deutsch", ["es"] = "español", ["fi"] = "suomi", ["fr"] = "français",
        ["hu"] = "magyar", ["id"] = "Bahasa Indonesia", ["it"] = "italiano",
        ["ja"] = "日本語", ["ko"] = "한국어", ["nl"] = "Nederlands", ["no"] = "norsk",
        ["pl"] = "polski", ["pt"] = "português", ["pt-br"] = "português (Brasil)",
        ["ro"] = "română", ["ru"] = "русский", ["th"] = "ไทย", ["tl"] = "Tagalog",
        ["tr"] = "Türkçe", ["uk"] = "українська", ["zh-hant"] = "繁體中文",
    };

    private static readonly Dictionary<string, string> CrosslinkLangNames = new()
    {
        ["ar"] = "العربية", ["ca"] = "català", ["cs"] = "čeština", ["da"] = "dansk",
        ["de"] = "Deutsch", ["en"] = "English", ["es"] = "español", ["fi"] = "suomi",
        ["fr"] = "français", ["hu"] = "magyar", ["id"] = "Bahasa Indonesia",
        ["it"] = "italiano", ["ja"] = "日本語", ["ko"] = "한국어",
        ["nl"] = "Nederlands", ["no"] = "norsk", ["pl"] = "polski",
        ["pt"] = "português", ["pt-br"] = "português do Brasil", ["ro"] = "română",
        ["ru"] = "русский", ["th"] = "ไทย", ["tl"] = "Tagalog", ["tr"] = "Türkçe",
        ["uk"] = "українська", ["zh-hans"] = "简体中文", ["zh-hant"] = "繁體中文",
    };

    private static readonly string[] LinkOrder =
    [
        "ar", "ca", "zh-hant", "cs", "da", "de", "en", "es", "fi", "fr",
        "hu", "id", "it", "ja", "ko", "nl", "no", "tl", "pl", "pt",
        "pt-br", "ro", "ru", "th", "tr", "uk", "zh-hans",
    ];

    // ── Heading regex ─────────────────────────────────────────
    [GeneratedRegex(@"^(#{1,6}\s)", RegexOptions.Multiline)]
    private static partial Regex HeadingRe();
    [GeneratedRegex(@"[\u4e00-\u9fff\u3400-\u4dbf\uf900-\ufaff]")]
    private static partial Regex CjkRe();
    [GeneratedRegex(@"^```", RegexOptions.Multiline)]
    private static partial Regex CodeFenceRe();
    [GeneratedRegex(@"^[{}]\s*$", RegexOptions.Multiline)]
    private static partial Regex BraceLineRe();

    // ══════════════════════════════════════════════════════════
    //  Entry Point
    // ══════════════════════════════════════════════════════════
    public static async Task<int> RunAsync(string[] args)
    {
        Console.OutputEncoding = Utf8NoBom.Encoding;
        var full = args.Contains("--full");
        var family = ParseArg(args, "--family");

        Console.WriteLine("=" .PadRight(50, '='));
        Console.WriteLine("Phase 1: Structure Checks");
        Console.WriteLine("=" .PadRight(50, '='));
        Console.WriteLine("Coverage: technical_reference / readme / contributing");
        Console.WriteLine(full ? "Mode: full (Phase 2 will run if Phase 1 passes)" : "Mode: dry-run (use --full for LLM comparison)");

        var familyList = family is not null ? family.Split(',') : Families.Select(f => f.Name).ToArray();
        var phase1Fail = 0;
        var softWarnings = 0;

        // 1a — segment structure
        var (segPassed, segIssues) = RunListSegments(familyList);
        if (!segPassed)
            phase1Fail++;

        // 1b — CJK residue
        var (cjkPassed, cjkIssues) = await RunFindCjk();
        if (!cjkPassed)
            softWarnings++;

        // 1c — crosslinks
        var (xlPassed, xlIssues) = await RunCrosslinks(familyList);
        if (!xlPassed)
            softWarnings++;

        // Always write Phase 1 report (regardless of pass/fail or --full flag)
        WritePhase1Report(segIssues, cjkIssues, xlIssues, phase1Fail > 0, softWarnings > 0);

        Console.WriteLine();
        if (phase1Fail > 0)
        {
            Console.WriteLine($"Phase 1 FAILED — {phase1Fail} structural check(s) had issues.");
            Console.WriteLine("Fix the above before re-running with --full.");
            return 1;
        }

        if (softWarnings > 0)
            Console.WriteLine($"Phase 1 PASSED — {softWarnings} non-blocking warning(s) above (CJK / crosslinks).");
        else
            Console.WriteLine("Phase 1: ALL PASSED");

        if (!full)
        {
            Console.WriteLine("Dry-run complete. Use --full to enable Phase 2 (LLM semantic comparison).");

            // Also write a Phase 2 empty report to record that Phase 2 was skipped
            WritePhase2SkippedReport();

            return 0;
        }

        // ── Phase 2 ──
        Console.WriteLine();
        Console.WriteLine("=" .PadRight(50, '='));
        Console.WriteLine("Phase 2: LLM Semantic Comparison");
        Console.WriteLine("=" .PadRight(50, '='));
        Console.WriteLine("Structures verified — starting LLM comparison...");

        var dryRun = args.Contains("--dry-run");
        var lang = ParseArg(args, "--lang");
        var fromIso = ParseArg(args, "--from");
        var toIso = ParseArg(args, "--to");

        var ec = await RunCompareDocs(familyList, dryRun, lang, fromIso, toIso);
        if (ec != 0)
            Console.WriteLine("\nPhase 2 found issues — see output above and report file.");
        else
            Console.WriteLine("\nPhase 2: ALL PASSED");
        return ec;
    }

    private static string? ParseArg(string[] args, string key)
    {
        var i = Array.IndexOf(args, key);
        return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
    }

    // ══════════════════════════════════════════════════════════
    //  Helpers
    // ══════════════════════════════════════════════════════════
    private static string IsoFromFilename(string fname, string prefix) => fname[prefix.Length..].Replace(".md", "");
    private static string LangName(string iso) => LangNames.GetValueOrDefault(iso, iso);

    private static DocFamily? GetFamily(string name) => Families.Find(f => f.Name == name);

    // ══════════════════════════════════════════════════════════
    //  1a — _list_segments.cs
    // ══════════════════════════════════════════════════════════
    private static (bool Passed, List<string> Issues) RunListSegments(string[] familyList)
    {
        Console.WriteLine("\n--- 1/3 Segment Structure ---");
        var allIssues = new List<string>();
        foreach (var famName in familyList)
        {
            var fam = GetFamily(famName);
            if (fam is null) { Console.WriteLine($"[WARN] Unknown family: {famName}"); continue; }
            allIssues.AddRange(CheckFamilySegments(fam));
        }
        if (allIssues.Count > 0)
        {
            Console.WriteLine($"\n=== Structure mismatch ({allIssues.Count} item(s)) ===");
            foreach (var i in allIssues) Console.WriteLine($"  {i}");
            Console.WriteLine("--- 1/3 Segment Structure: FAILED (exit=1) ---");
            return (false, allIssues);
        }
        Console.WriteLine("--- 1/3 Segment Structure: PASSED ---");
        return (true, allIssues);
    }

    private static List<string> CheckFamilySegments(DocFamily fam)
    {
        var issues = new List<string>();
        var baseFile = fam.BasePath ?? Path.Combine(fam.Dir, fam.Base);
        if (!File.Exists(baseFile))
        {
            issues.Add($"[{fam.Label}] Base file missing: {baseFile}");
            return issues;
        }
        var zhText = File.ReadAllText(baseFile, Utf8NoBom.Encoding);
        var zhSegs = SplitByHeadings(zhText);
        var targets = Directory.GetFiles(fam.Dir, fam.Glob)
            .Select(f => (File: f, Name: Path.GetFileName(f)))
            .Where(x => !fam.Skip.Contains(x.Name))
            .OrderBy(x => x.Name).ToList();

        foreach (var (tf, name) in targets)
        {
            var iso = name.Replace(fam.Prefix, "").Replace(".md", "");
            var tgtText = File.ReadAllText(tf, Utf8NoBom.Encoding);
            var tgtSegs = SplitByHeadings(tgtText);

            if (zhSegs.Count != tgtSegs.Count)
            {
                issues.Add($"[{fam.Label}] [{iso}] Segment count mismatch: zh={zhSegs.Count} tgt={tgtSegs.Count}");
                continue;
            }

            var anyHeadingMismatch = false;
            for (int i = 0; i < zhSegs.Count; i++)
            {
                var zhH = zhSegs[i].Text.Split('\n')[0].Trim();
                var tgtH = tgtSegs[i].Text.Split('\n')[0].Trim();
                var zhLvl = zhH.Length - zhH.TrimStart('#').Length;
                var tgtLvl = tgtH.Length - tgtH.TrimStart('#').Length;
                if (zhLvl != tgtLvl)
                {
                    issues.Add($"[{fam.Label}] [{iso}] seg[{i:D3}] Heading level mismatch | zh(L{zhLvl}): {zhH[..Math.Min(60, zhH.Length)]} | tgt(L{tgtLvl}): {tgtH[..Math.Min(60, tgtH.Length)]}");
                    anyHeadingMismatch = true;
                }
            }
            if (anyHeadingMismatch) continue;

            // Per-segment structure checks (line count, blank lines) — consistent with VerifySegment in Phase 2
            for (int i = 0; i < zhSegs.Count; i++)
            {
                var (_, lineVerdict, _, structDiffs) = VerifySegment(zhSegs[i].Text, tgtSegs[i].Text);
                if (lineVerdict != "OK" || structDiffs.Count > 0)
                {
                    var zhH = zhSegs[i].Text.Split('\n')[0].Trim();
                    var segIssues = new List<string> { $"[{fam.Label}] [{iso}] seg[{i:D3}] {zhH[..Math.Min(60, zhH.Length)]}" };
                    if (lineVerdict != "OK") segIssues.Add($"    line: {lineVerdict}");
                    foreach (var d in structDiffs) segIssues.Add($"    {d}");
                    issues.AddRange(segIssues);
                }
            }
        }
        return issues;
    }

    public static List<SegmentInfo> GetSegments(string filepath)
    {
        var text = File.ReadAllText(filepath, Utf8NoBom.Encoding);
        var lines = text.Split('\n');
        var headingIndices = new List<int>();
        var inFence = false;
        for (int i = 0; i < lines.Length; i++)
        {
            var s = lines[i].Trim();
            if (s.StartsWith("```")) { inFence = !inFence; continue; }
            if (!inFence && HeadingRe().IsMatch(lines[i])) headingIndices.Add(i);
        }
        var segs = new List<SegmentInfo>();
        foreach (var hIdx in headingIndices)
        {
            var heading = lines[hIdx].Trim();
            var level = heading.Length - heading.TrimStart('#').Length;
            segs.Add(new SegmentInfo(hIdx + 1, level, heading));
        }
        return segs;
    }

    public static List<SegContent> SplitByHeadings(string text)
    {
        var lines = text.Split('\n');
        var headingIndices = new List<int>();
        var inFence = false;
        for (int i = 0; i < lines.Length; i++)
        {
            var s = lines[i].Trim();
            if (s.StartsWith("```")) { inFence = !inFence; continue; }
            if (!inFence && HeadingRe().IsMatch(lines[i])) headingIndices.Add(i);
        }
        if (headingIndices.Count == 0) return [new(1, lines.Length, text)];

        var segs = new List<SegContent>();
        for (int j = 0; j < headingIndices.Count; j++)
        {
            var start = headingIndices[j];
            var end = j + 1 < headingIndices.Count ? headingIndices[j + 1] - 1 : lines.Length - 1;
            while (end > start && lines[end].Trim() == "") end--;
            segs.Add(new(start + 1, end + 1, string.Join("\n", lines[start..(end + 1)])));
        }
        return segs;
    }

    // ══════════════════════════════════════════════════════════
    //  1b — _find_cjk.cs
    // ══════════════════════════════════════════════════════════
    private static async Task<(bool Passed, List<string> Issues)> RunFindCjk()
    {
        Console.WriteLine("\n--- 2/3 CJK Residue Scan (non-blocking) ---");
        var allIssues = new List<string>();
        var total = 0;
        foreach (var fam in Families)
        {
            var files = Directory.EnumerateFiles(fam.Dir, fam.Glob).OrderBy(x => x).ToList();
            foreach (var f in files)
            {
                var name = Path.GetFileName(f);
                if (fam.Skip.Contains(name)) continue;
                var text = await File.ReadAllTextAsync(f, Utf8NoBom.Encoding);
                var lines = text.Split('\n');
                var issues = new List<(int Line, string Chars, string Context)>();
                var inCrosslinkBlock = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    var s = lines[i].Trim();
                    if (s.StartsWith('>') && s.Contains("<details><summary>")) continue;
                    if (s.Contains("<details><summary>")) { inCrosslinkBlock = true; continue; }
                    if (inCrosslinkBlock && s.Contains("</details>")) { inCrosslinkBlock = false; continue; }
                    if (inCrosslinkBlock) continue;
                    if (s.StartsWith("```") || s.StartsWith("|---") || s.StartsWith("|--")) continue;
                    var m = CjkRe().Matches(lines[i]);
                    if (m.Count > 0) issues.Add((i + 1, string.Concat(m.Select(x => x.Value)), lines[i].Trim()[..Math.Min(120, lines[i].Trim().Length)]));
                }
                if (issues.Count > 0)
                {
                    var stem = Path.GetFileNameWithoutExtension(f);
                    foreach (var p in new[] { "technical_reference_", "README_", "contributing_" })
                        if (stem.StartsWith(p)) { stem = stem[p.Length..]; break; }
                    Console.WriteLine($"\n[{fam.Label}] [{stem}] {issues.Count} line(s) with CJK:");
                    foreach (var (ln, chars, ctx) in issues.Take(20))
                    {
                        Console.WriteLine($"  L{ln}: [{chars}] {ctx}");
                        allIssues.Add($"[{fam.Label}] [{stem}] L{ln}: [{chars}] {ctx}");
                    }
                    total += issues.Count;
                }
            }
        }
        if (total > 0) Console.WriteLine($"\n=== CJK residue: {total} instance(s) ===");
        Console.WriteLine("--- 2/3 CJK Residue Scan: PASSED (non-blocking, warnings above) ---");
        return (true, allIssues); // non-blocking
    }

    // ══════════════════════════════════════════════════════════
    //  1c — _add_crosslinks.cs
    // ══════════════════════════════════════════════════════════
    private static async Task<(bool Passed, List<string> Issues)> RunCrosslinks(string[] familyList)
    {
        Console.WriteLine("\n--- 3/3 Crosslink Check (non-blocking) ---");
        var allIssues = new List<string>();
        foreach (var famName in familyList)
        {
            var fam = GetFamily(famName);
            if (fam is null) continue;
            var files = Directory.EnumerateFiles(fam.Dir, fam.Glob).OrderBy(x => x).ToList();
            foreach (var f in files)
            {
                var text = await File.ReadAllTextAsync(f, Utf8NoBom.Encoding);
                if (text.Contains("<details><summary>")) continue;
                var iso = Path.GetFileNameWithoutExtension(f).Replace(fam.Prefix, "");
                allIssues.Add($"[{fam.Label}] [{iso}] Missing crosslinks");
            }
        }
        if (allIssues.Count > 0)
        {
            Console.WriteLine($"\n=== Crosslinks missing ({allIssues.Count} item(s)) ===");
            foreach (var i in allIssues) Console.WriteLine($"  {i}");
            Console.WriteLine("--- 3/3 Crosslink Check: WARNINGS (non-blocking) ---");
            return (false, allIssues);
        }
        Console.WriteLine("--- 3/3 Crosslink Check: PASSED ---");
        return (true, allIssues);
    }

    // ══════════════════════════════════════════════════════════
    //  Phase 2 — _compare_docs.cs
    // ══════════════════════════════════════════════════════════
    private static async Task<int> RunCompareDocs(string[] familyList, bool dryRun, string? lang, string? fromIso, string? toIso)
    {
        var exitCode = 0;
        foreach (var famName in familyList)
        {
            var fam = GetFamily(famName);
            if (fam is null) { Console.WriteLine($"[WARN] Unknown doc family: {famName}, skipping"); continue; }

            Console.WriteLine($"\n{"=".PadRight(70, '=')}");
            Console.WriteLine($"  Doc family: {fam.Label} ({fam.Name})");
            Console.WriteLine($"{"=".PadRight(70, '=')}");

            var allFiles = Directory.GetFiles(fam.Dir, fam.Glob).OrderBy(x => x).ToList();
            var targets = allFiles.Where(f => !fam.Skip.Contains(Path.GetFileName(f))).ToList();
            if (lang is not null)
            {
                var wanted = lang.Split(',').ToHashSet();
                targets = targets.Where(f => wanted.Contains(IsoFromFilename(Path.GetFileName(f), fam.Prefix))).ToList();
            }
            if (fromIso is not null || toIso is not null)
            {
                var names = targets.Select(f => IsoFromFilename(Path.GetFileName(f), fam.Prefix)).ToList();
                var fi = fromIso is not null ? names.IndexOf(fromIso) : 0;
                var ti = toIso is not null ? names.IndexOf(toIso) : names.Count - 1;
                if (fi < 0) fi = 0;
                if (ti < 0) ti = names.Count - 1;
                targets = targets.GetRange(fi, ti - fi + 1);
            }

            var baseFile = fam.BasePath ?? Path.Combine(fam.Dir, fam.Base);
            Console.WriteLine($"Base: {Path.GetFileName(baseFile)}");
            Console.WriteLine($"Targets: {targets.Count} language(s)");

            if (dryRun)
            {
                Console.WriteLine("\n[Dry-run] Segment split preview\n");
                var zhText = await File.ReadAllTextAsync(baseFile, Utf8NoBom.Encoding);
                var zhSegs = SplitByHeadings(zhText);
                Console.WriteLine($"{Path.GetFileName(baseFile)} → {zhSegs.Count} seg(s):");
                foreach (var (i, s, e, txt) in zhSegs.Select((v, i) => (i, v.Start, v.End, v.Text)))
                    Console.WriteLine($"  [{i:D3}] L{s}-L{e} | {txt.Split('\n')[0][..Math.Min(70, txt.Split('\n')[0].Length)]}");
                foreach (var tf in targets)
                {
                    var iso = IsoFromFilename(Path.GetFileName(tf), fam.Prefix);
                    var txt = await File.ReadAllTextAsync(tf, Utf8NoBom.Encoding);
                    var segs = SplitByHeadings(txt);
                    Console.WriteLine($"\n{iso} → {segs.Count} seg(s):");
                    foreach (var (i, s, e, t) in segs.Select((v, i) => (i, v.Start, v.End, v.Text)))
                        Console.WriteLine($"  [{i:D3}] L{s}-L{e} | {t.Split('\n')[0][..Math.Min(70, t.Split('\n')[0].Length)]}");
                }
                continue;
            }

            // Phase 1: file-level structure pre-check
            var (structOk, fileStructIssues) = await CheckFileStructuresAsync(fam, targets);
            if (fileStructIssues.Count == 0)
                Console.WriteLine("  File structure: all consistent ✓");
            else
            {
                Console.WriteLine($"\n=== File structure mismatch ({fileStructIssues.Count} item(s)) — skipping LLM comparison ===");
                foreach (var i in fileStructIssues) Console.WriteLine($"  {i}");
                exitCode = 1;
                continue;
            }
            if (!structOk) continue;

            // Phase 2: prepare all segments
            var allSegments = new List<SegTask>();
            foreach (var tf in targets)
            {
                var (iso, name, segs) = await PrepareLangSegments(tf, fam, baseFile);
                allSegments.AddRange(segs);
            }
            Console.WriteLine($"  Total tasks: {allSegments.Count} LLM calls (concurrency={MaxConcur}, retries={MaxRetries})");

            // Phase 3: parallel LLM calls
            var rawResults = new ConcurrentBag<SegTask>();
            await Parallel.ForEachAsync(allSegments, new ParallelOptions { MaxDegreeOfParallelism = MaxConcur }, async (seg, ct) =>
            {
                var r = await ProcessOneTask(seg);
                rawResults.Add(r);
            });

            // Group by iso
            var byIso = rawResults.GroupBy(r => r.Iso).ToDictionary(g => g.Key, g => g.OrderBy(x => x.SegIdx).ToList());

            // Phase 4: classify output
            var allResults = new List<(string Iso, string Name, List<SegTask> StructIssues, List<SegTask> SemanticIssues)>();
            foreach (var tf in targets)
            {
                var iso = IsoFromFilename(Path.GetFileName(tf), fam.Prefix);
                var name = LangName(iso);
                byIso.TryGetValue(iso, out var segResults);
                segResults ??= [];

                var structIssues = new List<SegTask>();
                var semanticIssues = new List<SegTask>();
                foreach (var r in segResults)
                {
                    var isStruct = r.LineVerdict != "OK" || !r.StructMatch;
                    var isSemantic = r.LlmSemantic is false || r.LlmSemantic is null;
                    if (isStruct) structIssues.Add(r);
                    else if (isSemantic) semanticIssues.Add(r);
                }
                allResults.Add((iso, name, structIssues, semanticIssues));

                if (structIssues.Count > 0)
                {
                    Console.WriteLine($"\n--- [{iso}] {name} Segment structure issues ({structIssues.Count} seg(s)) ---");
                    foreach (var r in structIssues)
                    {
                        var tags = new List<string>();
                        if (r.LineVerdict != "OK") tags.Add($"line:{r.LineVerdict}");
                        if (!r.StructMatch) tags.Add("struct_diff");
                        Console.WriteLine($"  seg[{r.SegIdx:D3}] {r.ZhHeading[..Math.Min(60, r.ZhHeading.Length)]} | {string.Join(" ", tags)}");
                    }
                    exitCode = 1;
                }
                if (semanticIssues.Count > 0)
                {
                    Console.WriteLine($"\n--- [{iso}] {name} Semantic issues ({semanticIssues.Count} seg(s)) ---");
                    foreach (var r in semanticIssues)
                    {
                        var tag = r.LlmSemantic is null ? "LLM_parse_fail" : "semantic_diff";
                        Console.WriteLine($"  seg[{r.SegIdx:D3}] {r.ZhHeading[..Math.Min(60, r.ZhHeading.Length)]} | {tag} | {(r.LlmRaw ?? "")[..Math.Min(120, (r.LlmRaw ?? "").Length)]}");
                    }
                    exitCode = 1;
                }
            }
            WriteReport(allResults, fam.Label, Path.GetFileName(baseFile));
        }
        return exitCode;
    }

    private static async Task<(bool Ok, List<string> Issues)> CheckFileStructuresAsync(DocFamily fam, List<string> targets)
    {
        var baseFile = fam.BasePath ?? Path.Combine(fam.Dir, fam.Base);
        var zhText = await File.ReadAllTextAsync(baseFile, Utf8NoBom.Encoding);
        var zhSegs = SplitByHeadings(zhText);
        var issues = new List<string>();
        foreach (var tf in targets)
        {
            var iso = IsoFromFilename(Path.GetFileName(tf), fam.Prefix);
            var tgtText = await File.ReadAllTextAsync(tf, Utf8NoBom.Encoding);
            var tgtSegs = SplitByHeadings(tgtText);
            if (zhSegs.Count != tgtSegs.Count)
                issues.Add($"[{iso}] Segment count mismatch: zh={zhSegs.Count} tgt={tgtSegs.Count}");

            var n = Math.Min(zhSegs.Count, tgtSegs.Count);
            for (int i = 0; i < n; i++)
            {
                var zhH = zhSegs[i].Text.Split('\n')[0].Trim();
                var tgtH = tgtSegs[i].Text.Split('\n')[0].Trim();
                var zhLvl = zhH.Length - zhH.TrimStart('#').Length;
                var tgtLvl = tgtH.Length - tgtH.TrimStart('#').Length;
                if (zhLvl != tgtLvl)
                    issues.Add($"[{iso}] seg[{i:D3}] Heading level mismatch | zh(L{zhLvl}): {zhH[..Math.Min(60, zhH.Length)]} | tgt(L{tgtLvl}): {tgtH[..Math.Min(60, tgtH.Length)]}");
            }
        }
        return (issues.Count == 0, issues);
    }

    private static async Task<(string Iso, string Name, List<SegTask> Segs)> PrepareLangSegments(string targetFile, DocFamily fam, string baseFile)
    {
        var iso = IsoFromFilename(Path.GetFileName(targetFile), fam.Prefix);
        var name = LangName(iso);
        var zhText = await File.ReadAllTextAsync(baseFile, Utf8NoBom.Encoding);
        var tgtText = await File.ReadAllTextAsync(targetFile, Utf8NoBom.Encoding);
        var zhSegs = SplitByHeadings(zhText);
        var tgtSegs = SplitByHeadings(tgtText);
        var n = Math.Max(zhSegs.Count, tgtSegs.Count);
        var segs = new List<SegTask>();
        for (int i = 0; i < n; i++)
        {
            var zhS = i < zhSegs.Count ? zhSegs[i] : new SegContent(0, 0, "");
            var tgtS = i < tgtSegs.Count ? tgtSegs[i] : new SegContent(0, 0, "");
            var zhHeading = !string.IsNullOrEmpty(zhS.Text) ? zhS.Text.Split('\n')[0].Trim() : "N/A";
            segs.Add(new SegTask
            {
                SegIdx = i, Iso = iso, Name = name,
                ZhRange = $"L{zhS.Start}-L{zhS.End}", TgtRange = $"L{tgtS.Start}-L{tgtS.End}",
                ZhHeading = zhHeading, ZhContent = zhS.Text, TgtContent = tgtS.Text,
            });
        }
        return (iso, name, segs);
    }

    private static async Task<SegTask> ProcessOneTask(SegTask seg)
    {
        var raw = await CallLlmWithRetry(seg.ZhContent, seg.TgtContent, seg.Iso);
        var (llmOk, llmRaw) = ParseLlm(raw, $"{seg.Iso}[{seg.SegIdx}]");
        var (lineMatch, lineVerdict, structMatch, structDiffs) = VerifySegment(seg.ZhContent, seg.TgtContent);
        seg.LlmSemantic = llmOk;
        seg.LlmRaw = llmRaw;
        seg.LineMatch = lineMatch;
        seg.LineVerdict = lineVerdict;
        seg.StructMatch = structMatch;
        seg.StructDiffs = structDiffs;
        return seg;
    }

    private static async Task<string> CallLlmWithRetry(string zhText, string tgtText, string targetIso)
    {
        string? lastError = null;
        for (int attempt = 0; attempt < MaxRetries; attempt++)
        {
            var raw = await CallLlm(zhText, tgtText, targetIso);
            if (!raw.StartsWith("ERROR:")) return raw;
            lastError = raw;
        }
        return lastError!;
    }

    private static async Task<string> CallLlm(string zhText, string tgtText, string targetIso)
    {
        var prompt = $@"你是语义对比机。只输出两行：
第一行: true 或 false (全部通过=true, 否则=false)
第二行: 简短原因(一行, 多问题用;分隔, 指出具体位置)

检查项(任一不通过则false):
A. 整体语义是否与中文原文一致
B. 目标语言段落中是否有未翻译的其它语言残留 (注意区分: 代码块内容/变量名/函数名/类名/文件名/路径/URL/Steam ID/专有名词/API字段名 不算残留)

对比: 中文(原文) vs {targetIso}(目标语言)

<中文片段起始>
{zhText}
</中文片段结束>
<目标语言片段起始>
{tgtText}
</目标语言片段结束>";

        var payload = new { model = LmModel, messages = new[] { new { role = "user", content = prompt } }, temperature = 0.0, max_tokens = 4096 };
        try
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Utf8NoBom.Encoding, "application/json");
            using var req = new HttpRequestMessage(HttpMethod.Post, LmEndpoint) { Content = jsonContent };
            req.Headers.Add("Authorization", $"Bearer {LmKey}");
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(LmTimeout));
            using var r = await Http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cts.Token);
            r.EnsureSuccessStatusCode();
            var body = await r.Content.ReadFromJsonAsync<LlmResponse>(cancellationToken: cts.Token);
            var msg = body?.choices?[0]?.message;
            return (msg?.content ?? msg?.reasoning_content ?? "").Trim().ToLower();
        }
        catch (Exception e) { return $"ERROR:{e.Message}"; }
    }

    private static (bool? Ok, string Raw) ParseLlm(string raw, string debugLabel = "")
    {
        if (raw.StartsWith("ERROR:")) return (null, raw);
        var lines = raw.Split('\n').Select(l => l.Trim().ToLower()).Where(l => l.Length > 0).ToList();
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            var ln = lines[i];
            if (ln is "true") return (true, raw);
            if (ln is "false") return (false, raw);
            if (ln.StartsWith("true")) return (true, raw);
            if (ln.StartsWith("false")) return (false, raw);
        }
        if (raw.Contains("true")) return (true, raw);
        if (raw.Contains("false")) return (false, raw);
        return (null, raw);
    }

    private static (bool LineMatch, string LineVerdict, bool StructMatch, List<string> StructDiffs) VerifySegment(string zhText, string tgtText)
    {
        var zhLines = zhText.Split('\n').Length;
        var tgtLines = tgtText.Split('\n').Length;
        var lineMatch = zhLines == tgtLines;
        var lineVerdict = lineMatch ? "OK" : $"MISMATCH: zh={zhLines} tgt={tgtLines}";

        var zhS = StructuralLandmarks(zhText);
        var tgtS = StructuralLandmarks(tgtText);
        var structDiffs = new List<string>();
        foreach (var k in new[] { "blank", "code_fence", "brace_line" })
            if (!zhS[k].SequenceEqual(tgtS[k]))
                structDiffs.Add($"{k}: zh=[{string.Join(",", zhS[k])}] tgt=[{string.Join(",", tgtS[k])}]");
        return (lineMatch, lineVerdict, structDiffs.Count == 0, structDiffs);
    }

    private static Dictionary<string, List<int>> StructuralLandmarks(string text)
    {
        var lines = text.Split('\n');
        var blank = new List<int>();
        var codeFence = new List<int>();
        var braceLine = new List<int>();
        for (int i = 0; i < lines.Length; i++)
        {
            var ln1 = i + 1;
            var s = lines[i].Trim();
            if (s == "") blank.Add(ln1);
            if (CodeFenceRe().IsMatch(s)) codeFence.Add(ln1);
            if (BraceLineRe().IsMatch(s)) braceLine.Add(ln1);
        }
        return new() { ["blank"] = blank, ["code_fence"] = codeFence, ["brace_line"] = braceLine };
    }

    private static string TimestampedPath(string dir, string prefix, string suffix)
    {
        var ts = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return Path.Combine(dir, $"{prefix}_{ts}{suffix}");
    }

    private static void WritePhase1Report(List<string> segIssues, List<string> cjkIssues, List<string> xlIssues, bool hasHardErrors, bool hasSoftWarnings)
    {
        var now = DateTime.Now;
        var ts = now.ToString("yyyyMMdd_HHmmss");
        var l = new List<string>
        {
            "# Phase 1 Structure Check Report",
            $"Generated: {now:yyyy-MM-dd HH:mm:ss}",
            $"Status: {(hasHardErrors ? "FAILED (blocking)" : hasSoftWarnings ? "PASSED (with non-blocking warnings)" : "ALL PASSED")}",
            "",
            "---",
            "",
            "## 1/3 Segment Structure",
            segIssues.Count > 0
                ? $"Found {segIssues.Count} issue(s)"
                : "All consistent ✓",
        };
        if (segIssues.Count > 0)
        {
            l.Add("");
            foreach (var i in segIssues) l.Add($"- {i}");
        }

        l.Add("");
        l.Add("---");
        l.Add("");
        l.Add("## 2/3 CJK Residue Scan");
        l.Add(cjkIssues.Count > 0
            ? $"Found {cjkIssues.Count} instance(s)"
            : "None found ✓");

        if (cjkIssues.Count > 0)
        {
            l.Add("");
            foreach (var i in cjkIssues) l.Add($"- {i}");
        }

        l.Add("");
        l.Add("---");
        l.Add("");
        l.Add("## 3/3 Crosslink Check");
        l.Add(xlIssues.Count > 0
            ? $"Found {xlIssues.Count} missing"
            : "All passed ✓");

        if (xlIssues.Count > 0)
        {
            l.Add("");
            foreach (var i in xlIssues) l.Add($"- {i}");
        }

        var content = string.Join("\n", l);
        var tempPath = TimestampedPath(Path.Combine(BaseDir, "temp"), "_phase1_report", ".md");
        var logPath = TimestampedPath(Path.Combine(BaseDir, "log"), "_phase1_report", ".md");
        File.WriteAllText(tempPath, content, Utf8NoBom.Encoding);
        File.WriteAllText(logPath, content, Utf8NoBom.Encoding);
        Console.WriteLine($"\nPhase 1 report: {tempPath}");
        Console.WriteLine($"Log copy: {logPath}");
    }

    private static void WritePhase2SkippedReport()
    {
        var now = DateTime.Now;
        var l = new List<string>
        {
            "# Phase 2 LLM Semantic Comparison Report",
            $"Generated: {now:yyyy-MM-dd HH:mm:ss}",
            $"Status: Not executed (only Phase 1 structure check was run)",
            "",
            "Use `--full` to enable Phase 2.",
        };
        var content = string.Join("\n", l);
        var tempPath = TimestampedPath(Path.Combine(BaseDir, "temp"), "_phase2_report_skipped", ".md");
        var logPath = TimestampedPath(Path.Combine(BaseDir, "log"), "_phase2_report_skipped", ".md");
        File.WriteAllText(tempPath, content, Utf8NoBom.Encoding);
        File.WriteAllText(logPath, content, Utf8NoBom.Encoding);
        Console.WriteLine($"\nPhase 2 skipped log: {tempPath}");
        Console.WriteLine($"Log copy: {logPath}");
    }

    private static void WriteReport(List<(string Iso, string Name, List<SegTask> StructIssues, List<SegTask> SemanticIssues)> allResults, string familyLabel, string baseFile)
    {
        var ts = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var outPath = TimestampedPath(Path.Combine(BaseDir, "temp"), "_compare_report", ".md");
        var l = new List<string>
        {
            "# Multi-language Doc Comparison Report",
            $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            $"Doc family: {familyLabel}",
            $"Base: {baseFile}",
            "",
        };
        var totalStruct = allResults.Sum(r => r.StructIssues.Count);
        var totalSemantic = allResults.Sum(r => r.SemanticIssues.Count);
        l.Insert(4, $"**Structure issues: {totalStruct} / Semantic issues: {totalSemantic}**");
        l.Insert(5, "");

        l.Add("## Structure Mismatches\n");
        foreach (var (iso, name, structIssues, _) in allResults.Where(r => r.StructIssues.Count > 0))
        {
            l.Add($"### {iso} — {name} ({structIssues.Count} seg(s))\n");
            foreach (var s in structIssues)
            {
                l.Add($"- seg[{s.SegIdx:D3}] `{s.ZhHeading[..Math.Min(60, s.ZhHeading.Length)]}`");
                l.Add($"  - Lines: zh={s.ZhContent.Split('\n').Length} tgt={s.TgtContent.Split('\n').Length} match={s.LineMatch}");
                if (s.StructDiffs?.Count > 0)
                    foreach (var d in s.StructDiffs) l.Add($"  - {d}");
                l.Add("");
            }
        }
        l.Add("## Semantic Mismatches\n");
        foreach (var (iso, name, _, semanticIssues) in allResults.Where(r => r.SemanticIssues.Count > 0))
        {
            l.Add($"### {iso} — {name} ({semanticIssues.Count} seg(s))\n");
            foreach (var s in semanticIssues)
            {
                l.Add($"- seg[{s.SegIdx:D3}] `{s.ZhHeading[..Math.Min(60, s.ZhHeading.Length)]}`");
                l.Add($"  - LLM: `{s.LlmSemantic}` reason=`{(s.LlmRaw ?? "")[..Math.Min(200, (s.LlmRaw ?? "").Length)]}`");
                l.Add("");
            }
        }
        var content = string.Join("\n", l);
        File.WriteAllText(outPath, content, Utf8NoBom.Encoding);
        var logPath = TimestampedPath(Path.Combine(BaseDir, "log"), "_compare_report", ".md");
        File.WriteAllText(logPath, content, Utf8NoBom.Encoding);
        Console.WriteLine($"\nReport: {outPath}");
        Console.WriteLine($"Log copy: {logPath}");
    }
}

// ══════════════════════════════════════════════════════════
//  Data Types
// ══════════════════════════════════════════════════════════

public record DocFamily(string Name, string Label, string Dir, string Base, string Glob, string[] Skip, string Prefix, string? BasePath = null);
public record SegmentInfo(int Line, int Level, string Heading);
public record SegContent(int Start, int End, string Text);

public class SegTask
{
    public int SegIdx { get; set; }
    public string Iso { get; set; } = "";
    public string Name { get; set; } = "";
    public string ZhRange { get; set; } = "";
    public string TgtRange { get; set; } = "";
    public string ZhHeading { get; set; } = "";
    public string ZhContent { get; set; } = "";
    public string TgtContent { get; set; } = "";
    public bool? LlmSemantic { get; set; }
    public string? LlmRaw { get; set; }
    public bool LineMatch { get; set; }
    public string LineVerdict { get; set; } = "";
    public bool StructMatch { get; set; }
    public List<string>? StructDiffs { get; set; }
}

public class LlmResponse
{
    public Choice[]? choices { get; set; }
}

public class Choice
{
    public Message? message { get; set; }
}

public class Message
{
    public string? content { get; set; }
    public string? reasoning_content { get; set; }
}
