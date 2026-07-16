# 3rd_party

Structure: bundled third-party runtime assets.

Function: `steamcmd/` is refreshed from Valve's official archive by `SteamCmdBootstrapper`, then copied by `ModDownloader` into each download batch temp folder.

Notes: Treat contents as external binaries/assets. Do not edit steamcmd files as source code. Its `.gitignore` whitelists platform binaries and excludes runtime cache/output files.
