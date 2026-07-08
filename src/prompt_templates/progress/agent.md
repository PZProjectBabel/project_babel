# progress

Structure: progress report templates, one file per target language.

Function: localized markdown shells consumed by `ProgressReporter`.

Naming: `progress_template_{iso}.md`.

Placeholders:
- `{{LANGUAGE_LINKS}}`, `{{DATE}}`
- `{{TOTAL_ENTRIES}}`, `{{UNTRANSLATABLE_ENTRIES}}`, `{{TRANSLATED_ENTRIES}}`, `{{PENDING_ENTRIES}}`, `{{PROGRESS_PCT}}`
- `{{TOTAL_MODS}}`, `{{ACCEPTED_MODS}}`, `{{REJECTED_MODS}}`, `{{UNKNOWN_MODS}}`
- `{{REJECTED_TABLE}}`, `{{ALL_REVIEW_TABLE}}`, `{{OVERVIEW_TABLE}}`, `{{PER_LANG_SECTIONS}}`

Notes: fixed prose stays in target language; runtime only fills supported-language stats.
