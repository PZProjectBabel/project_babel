# 3rd_party

Structure: bundled third-party runtime assets.

Function: `steamcmd/` provides the Windows SteamCMD executable that `SteamCmdBootstrapper` runs in place to self-update, then `ModDownloader` copies it into each download batch temp folder.

Notes: Treat contents as external binaries/assets. Do not edit steamcmd files as source code. Its `.gitignore` whitelists platform binaries and excludes runtime cache/output files.
