# docs/templates

Structure: Chinese doc templates consumed by `DocGenerator`. Each doc family has a `_template.md`, `_links_mapping.json`, and `_template_cache.json`.

Function: source-of-truth for multi-language doc generation. Templates are Chinese markdown with `{{PLACEHOLDER}}` slots. `DocGenerator` reads them, diffs against caches, translates new/changed lines via LLM, and writes final docs to `README.md` (root) and `docs/`.

Files:
- `readme_template.md` + `readme_links_mapping.json` + `readme_template_cache.json`
- `contributing_template.md` + `contributing_links_mapping.json` + `contributing_template_cache.json`
- `technical_reference_template.md` + `technical_reference_links_mapping.json` + `technical_reference_template_cache.json`
- `prompt_header.md` + `prompt_tail.md`: LLM prompt wrapper for doc translation.

Notes: templates are Chinese source (translatable data), not code. Cache files record SHA256 of each translated line; only changed lines trigger re-translation. Link mappings define multi-language switcher block and named links (`{{progress_link}}`, etc).
