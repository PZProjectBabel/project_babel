# ModDownloader

Structure: service project referencing `Common`, `ModIdCollector`, and `ModInfoFetcher`.

Function: download Steam Workshop content through an independent copied steamcmd instance per batch. The pipeline runs at most eight batch downloads concurrently.

Flow:
1. Copy `src/3rd_party/steamcmd` to the batch temp folder.
2. Run `+login anonymous +workshop_download_item 108600 <id> ... +quit`.
3. Retry only pending IDs up to `steamMaxRetries`.
4. Parse process output and `logs/*.txt|*.log` for per-item start/size/rate/commit/success.
5. Condense steamcmd self-update lines as transient progress.
6. Move `steamapps/workshop/content/108600/<id>` to `downloadedModsTempDir/<id>`.

Outputs: sets `ModInfo.localDownloadedPath`; returns failed count for missing downloads. The pipeline runner deletes the downloaded mod directory and per-batch SteamCMD workspace immediately after extraction.

Notes: Ctrl+C/process-exit handlers synchronously kill running steamcmd process trees. Killing steamcmd directly is intentional.
