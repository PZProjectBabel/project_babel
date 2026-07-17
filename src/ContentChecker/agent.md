# ContentChecker

Structure: service project referencing `Common` and `TranslationBatcher`.

Function: mod-level content safety review and queue filtering.

Inputs: `ModInfo` dictionary, all extracted `TranslationEntry` objects, `content_verification.txt`, LLM settings.

Outputs:
- updates `ModInfo.contentCheckStatus`, `needsContentCheck`, and `timeNextContentCheck`.
- fills diff dictionary with accepted queued entries; key-only entries are allowed.
- writes content-review prompts/results under run temp folders.

Notes: Runs for normal mods only. Content review cache is mod-scoped, not target-language-scoped; if a mod was reviewed and has not reached `timeNextContentCheck`, later language queues must reuse that result. Do not filter by target history; `Program` builds target-aware queue and `LLMTranslator` skips processed targets.
