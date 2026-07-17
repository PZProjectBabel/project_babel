# ProgressReporter

Structure: service project referencing `Common`.

Function: render progress markdown from localized templates after pipeline completion.

Inputs: `ModInfo`, `TranslationEntry`, ref `ModInfo`, and `src/prompt_templates/progress/` templates.

Outputs: `docs/progress/progress_{iso}.md` with entry totals, translated/pending stats, mod review counts, rejected tables, and overview tables.

Notes: only supported-language stats are emitted. Runs last.
