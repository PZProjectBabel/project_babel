# src/DocChecker

.NET console app: two-phase doc consistency checker (replaces `src/scripts/_doc_checks.py` + sub-scripts).
Run from repo root via `dotnet run --project src/DocChecker` or `run_doc_checks.bat`.

## Structure

- `Program.cs` — entry, passes args to `DocChecker.RunAsync`
- `DocChecker.cs` — all logic (static class + data types)

## Phases

Phase 1 (structure, no API): segment count/level, CJK residue, crosslinks.
Phase 2 (LLM, `--full`): heading-based split + parallel LLM semantic comparison.

## Args

- `--full` — run Phase 2 after Phase 1
- `--family readme,contributing` — limit families
- `--dry-run` — show segment splits only
- `--lang hu,ja,ko` — limit languages
- `--from iso --to iso` — range

## Config

Reads `config/secrets.json` (LLM_KEY) and `config/config.json` (LLM endpoint).
Output report to `temp/_compare_report.md`.

## Build

```bash
dotnet build src/DocChecker
```
