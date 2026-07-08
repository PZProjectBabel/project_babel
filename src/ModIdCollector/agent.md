# ModIdCollector

Structure: service project referencing `Common`.

Function: merge requested mod IDs into caller-owned `Dictionary<string, ModInfo>` without overwriting loaded cache state.

Inputs:
- AsOne remote list when enabled.
- `config/request_for_translation.txt`.

Outputs: new `ModInfo` seeds only for unseen IDs.

API: `CollectModIdsAsync(Dictionary<string, ModInfo>, CancellationToken)` returns `TaskResult`.

Notes: Missing optional inputs warn, not fail. Support both JSON and plain-text remote responses. Keep HTTP injectable/testable.
