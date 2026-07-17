# DocGenerator

Structure: service project referencing `Common`.

Function: translate Chinese template docs into multi-language via LLM, output to final doc locations.

Inputs: `docs/templates/*_template.md`, `*_links_mapping.json`, `*_template_cache.json`.

Outputs:
- `temp/docgen/{name}_{lang}.md` (intermediate)
- `README.md` (root, zh-hans only)
- `docs/readme/README_{lang}.md`
- `docs/{name}/{name}_{lang}.md`

Pipeline:
1. Parse template lines & classify (translatable/markdown/placeholder).
2. Load links mapping & translation cache.
3. Diff cache → collect lines needing LLM translation.
4. Translate per lang via LLM (batched, concurrent, retry).
5. Assemble final doc with post-processing (TOC anchors, table rows, list items, placeholder resolution).
6. Copy outputs to final locations (readme zh-hans→root README.md; rest→docs/).

Rules:
- MaxLinesPerBatch=30, MaxConcurrency=128, MaxRetries=3.
- Cache by SHA256 of source text.
- LLM: env LLM_KEY|DEEPSEEK_API_KEY, LLM_ENDPOINT, LLM_MODEL.
