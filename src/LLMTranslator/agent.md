# LLMTranslator

Structure: service project referencing `Common`, `TranslationBatcher`, `RagContextRetriever`, and `ContentChecker`.

Function: build per-target prompt plans, run LLM translation batches, parse responses, and write target results back to `translationValues`.

Inputs:
- reusable `TranslationBatch` list
- `ModInfo` / `TranslationEntry` state
- explicit target language
- target-specific RAG context
- prompt templates and optional target dictionary

Outputs:
- fills `entry.translationValues[targetLang]` with text/confidence/process/verify state
- writes prompts to `runTempDir/prompts/<target_iso>/...`
- writes raw responses to `runTempDir/llm_responses/<target_iso>/...`
- writes warning files for failed or empty batches

Notes: skip already processed target entries. Warmup is optional and only validates target/model path. Detailed prompt/response wire format lives in code comments.
