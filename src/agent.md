# src

Structure: .NET solution with one entry project plus module projects. `TranslationPipeline.slnx` includes `TranslationPipeline.csproj`, `Common`, all pipeline modules, and `test`.

Entry:
- `Program.cs`: pipeline runner, debug mod/language subset controls, known-mod filtered target work queue, persisted modinfo merge before write-back, stage numbering, per-target RAG/LLM short-circuit for languages with no pending entries, and per-language output loops.
- `TranslationPipeline.csproj`: entry project references all modules; module folders own their service classes.

Support folders:
- `prompt_templates/`: content-check and translation prompt templates/dictionaries.
- `3rd_party/`: bundled steamcmd template copied by `ModDownloader`.
- `test/`: xUnit regression tests.

Pipeline order:
1. `ConfigReader`: load config/secrets/supported languages and temp folders.
2. `RepoDataLoader`: load cached ref/translation data, diff entries, persist merge.
3. `ModIdCollector`: merge remote/local/cache mod IDs.
4. `ModInfoFetcher`: fetch Steam metadata.
5. `ModDownloader`: copy/run steamcmd and download Workshop content.
6. `ContentExtractor`: parse mod translation files into shared `TranslationEntry` dictionaries.
7. `ContentChecker`: review normal mods and filter queued entries.
8. `EmbeddingFetcher`: embed normal base/key-only and ref target text by source kind.
9. `TranslationBatcher`: create target-independent batches from checked target work queues; inactive work lowest priority.
10. `RagContextRetriever`: per-target exact key + embedding RAG contexts.
11. `LLMTranslator`: skip processed targets, build cacheable per-target prompts, warm up large target queues, execute LLM calls serially per language.
12. `ResultWriter`: write history stores to `data/`/`translation_ref/`.
13. `FinalOutputWriter`: generate PZ mod-format translation output.
14. `ProgressReporter`: generate progress reports in `docs/progress/`.

Shared data:
- `Dictionary<string, ModInfo>`: mod metadata and local downloaded paths.
- `Dictionary<string, TranslationEntry>`: all entries keyed by `modId::translationKey`.
- `TranslationEntry`: active/source hash metadata, per-target processed/verified state, source-kind embeddings.
- `List<TranslationBatch>`: source batches reused for every target; target language is passed explicitly during RAG/prompt/result stages.
- `ragContextByEntryKey`: runtime-only map keyed by `modId::translationKey`.

Notes: `data/` and `translation_ref/` are append/update history stores; do not delete historical keys. Keep comments/logs in English and annotations through `Common.GitHubActions`.
