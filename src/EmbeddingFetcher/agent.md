# EmbeddingFetcher

Structure: service project referencing `Common` and `ContentExtractor`.

Function: fetch vector embeddings for RAG with explicit source kinds.

Inputs: diff entries, reference entries, embedding host/port/key, in-memory existing vectors/hashes.

Outputs:
- writes `TranslationEntry.embeddingValues` plus legacy `embeddingVector/hash`.
- normal: `normal_base_text` or `normal_key_only`; generated target translations are never embedded as source text.
- reference: `ref_target_text` per target language.
- writes temp diagnostics to `embeddingsTempDir/embedding_summary.json`.

Notes: Normal entries never fallback to non-base text. Empty base embeds `<modid>::<key> = ""`. Hash includes source kind and full input.
Progress logs are throttled for large queues. Real endpoint runs probe host/port first and stops after repeated failed batches to avoid silent long stalls.
