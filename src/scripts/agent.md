# src/scripts

Python utility scripts for project-wide documentation quality assurance. Located at `src/scripts/`, run from the repository root.
Covers three doc families: **technical_reference**, **readme**, **contributing**.

## Scripts

### `_compare_docs.py` — Multi-family Multi-language LLM Comparison

Automatically splits Chinese base docs and all translated versions by heading, then uses an LLM to check each segment pair for semantic consistency, untranslated residue, and Markdown structure integrity.

**Usage:**
```bash
# Full comparison (all families, all languages, requires LLM API)
python src/scripts/_compare_docs.py

# Dry-run: only show segment splits without calling LLM
python src/scripts/_compare_docs.py --dry-run

# Compare specific languages across all families
python src/scripts/_compare_docs.py --lang hu,ja,ko

# Compare a specific doc family only
python src/scripts/_compare_docs.py --family readme
python src/scripts/_compare_docs.py --family technical_reference,contributing
```

**Output:**
- Console: per-family, per-language PASS/FAIL summary with segment-level failure details
- Report file: `temp/_compare_report.md` with full diff details (last family processed)

**Dependencies:** `requests` (for LLM API), reads `config/secrets.json` and `config/config.json`

**Verdict types per segment:**
- `line:MISMATCH` — line count differs (structural loss or surplus)
- `struct_diff` — blank lines / code fences / brace lines differ
- `semantic_diff` — LLM flag (may include false positives for low-resource languages)
- `LLM_parse_fail` — LLM response unparseable

### `_find_cjk.py` — CJK Character Residue Scanner (Multi-family)

Scans all non-Chinese translations across technical_reference, readme, and contributing for stray Chinese characters in the main text (outside code blocks and table separators).

**Usage:**
```bash
python src/scripts/_find_cjk.py
```

**Notes:** Expected CJK in code examples (e.g. `日本語`, `한국어`, `拾起`, `简体中文`) is not filtered — interpret results with context.

### `_list_segments.py` — Segment Index Dumper

Lists all heading-based segments of a markdown file with index, line range, line count, and heading text. Used to map `seg[N]` references from `_compare_docs.py` output back to source file line numbers.

**Usage:**
```bash
# Default: lists zh-hans base files for all 3 families
python src/scripts/_list_segments.py

# Specific file
python src/scripts/_list_segments.py docs/technical_reference/technical_reference_hu.md
```

### `_add_crosslinks.py` — Multi-language Cross-link Sync (Multi-family)

Ensures all technical_reference, readme, and contributing files have the cross-language link bar inserted after the `---` metadata separator.

**Usage:**
```bash
python src/scripts/_add_crosslinks.py
```

## QA Workflow

When `_compare_docs.py` reports failures:

1. Run `_find_cjk.py` first — Chinese character residue is the most common real issue.
2. For `line:MISMATCH` or `struct_diff` — use `_list_segments.py` on both `zh-hans` and the target file to identify missing/surplus lines, then fix.
3. For pure `semantic_diff` (no structural issues, no CJK residue) — likely an LLM false positive. Spot-check 1–2 segments manually; if correct, ignore.
4. Re-run `_compare_docs.py` on the fixed language(s) to verify.
