# RagContextRetriever

Structure: service project referencing `Common` and `EmbeddingFetcher`.

Function: build per-target runtime RAG context for each batch entry.

Inputs: reference entries, all known translation entries, diff entries, reusable batches, explicit target language or current `priorityLanguage`, embedding vectors.

Outputs: fills `ragContextByEntryKey` keyed by `modId::translationKey`; writes debug JSON under `runTempDir/rag_contexts/<target_iso>/rag_contexts.json` for multi-target runs.

Algorithm:
1. Add exact `translationKey` reference matches first.
2. Query normal embeddings (`normal_base_text`, else `normal_key_only`).
3. Score target-language reference embeddings plus normal historical embeddings.
4. Exclude current entry; skip dimension mismatch; keep above threshold/topK.

Notes: Reference embeddings are target-language dependent. Do not read temp debug files as input.
