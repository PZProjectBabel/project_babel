# WorkshopMonitor

Structure: standalone console project referencing `Common`.

Function: scrape Steam Workshop "most recent" listing for Project Zomboid (app 108600, Build 42 tag, excluding Language/Translation), resolve publish timestamps via Steam API, filter by subscription count > threshold, merge new mod IDs into `config/request_for_translation.txt` (dedup).

Inputs: `config/secrets.json` or env `STEAM_KEY`; `data/monitor_cache.bin` for last-run timestamp and known-mod cache (zstd-compressed binary, int64 LE sequence: lastRunSec then (modId, timeCreated) pairs).

Outputs: updated `data/monitor_cache.bin`; appended `config/request_for_translation.txt`.

Hardcoded params: AppId=108600, MinSubs=500, SafetyPages=5, PageSize=30, Lookback=48h, Build 42 tag, exclude Language/Translation.
