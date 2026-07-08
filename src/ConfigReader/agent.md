# ConfigReader

Structure: service project referencing `Common`.

Function: load and validate runtime configuration.

Inputs:
- `config/config.json`
- `config/secrets.json` and environment secrets
- `config/supported_languages.json`
- optional `config/request_for_translation.txt`
- optional `config/ref_translation_mods.json`

Outputs: populated `PipelineConfig` with absolute repo paths, temp run folders, supported languages, secrets, reference mod metadata, and LLM concurrency settings.

Notes: Fail fast on required config/secrets. Optional inputs warn. `InitializeFolders` owns runtime temp paths such as `runTempDir`, `downloadedModsTempDir`, `translationBatchesTempDir`, `translationResultsTempDir`, and `warningsTempDir`.
