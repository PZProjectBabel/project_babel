# ModInfoFetcher

Structure: service project referencing `Common`.

Function: fetch Steam Web API metadata for collected Workshop IDs and update caller-owned `Dictionary<string, ModInfo>`.

Inputs: mod IDs from `ModIdCollector`, `steamApiKey`, `steamApiChunkSize`, request timeout settings.

Outputs: `ModInfo` metadata, update flags, `isAvailable`, and `lastFetchStatus`.

API: `FetchModInfosAsync(Dictionary<string, ModInfo>)` returns `TaskResult`.

Notes: Batch by `steamApiChunkSize`. `fetch_failed` preserves availability. An existing `needsUpdate` flag is preserved until download and extraction clear it; metadata fetches for later download batches must not dequeue pending mods. Explicit missing/private/non-PZ marks unavailable. Log PZ/non-PZ/unknown summary.
