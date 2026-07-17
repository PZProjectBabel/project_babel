# TranslationBatcher

Structure: service project referencing `Common` and `ContentExtractor`.

Function: split accepted source entries into reusable LLM batches.

Inputs: `ModInfo` priority metadata, diff `TranslationEntry` dictionary, `llmBatchSize`, `llmBatchTokenBudget`.

Outputs: fills caller-owned `List<TranslationBatch>` and writes debug batches under `translationBatchesTempDir/<modid>/batch_###.json`.

Notes: Runs once before target-language loop. Token sizing uses base text only; missing base text remains key-only downstream. Batches are target-independent; inactive entries get lowest priority.
