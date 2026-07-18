# WorkshopMonitor

Structure: standalone console project referencing `Common`. Not part of `TranslationPipeline.slnx`; built and run independently with `dotnet run --project src/WorkshopMonitor/WorkshopMonitor.csproj`.

Function: scrape Steam Workshop "most recent" listing for Project Zomboid (app 108600, Build 42 tag, excluding Language/Translation), resolve publish timestamps via Steam API, filter by subscription count > threshold, merge new mod IDs into `config/request_for_translation.txt` (dedup).

Inputs:
- `config/secrets.json` → `STEAM_KEY` field, or env var `STEAM_KEY` / `STEAM_API_KEY` (secrets.json preferred, matches `ConfigReader` pattern).
- `data/monitor_cache.bin` — zstd-compressed binary cache. Format (little-endian int64 sequence): `[lastRunUnixSec][modId0][timeCreated0][modId1][timeCreated1]...`

Outputs: updated `data/monitor_cache.bin`; updated `config/request_for_translation.txt` (full rewrite with dedup + trailing newline).

Hardcoded params: AppId=108600, MinSubs=500, SafetyPages=5, PageSize=30, Lookback=48h, Build 42 tag, exclude Language/Translation.

GitHub Action: `.github/workflows/monitor-workshop.yml` — cron `0 16 * * *` (Beijing 00:00 daily), auto PR + squash merge.

Notes: uses `HtmlAgilityPack` for HTML parsing, `ZstdSharp` for binary compression (same pattern as `BinaryEmbeddingSerializer` in Common). HTTP client uses desktop User-Agent with gzip/deflate decompression, random 3-20s delays between pages, 300ms delay between subscription batches.
