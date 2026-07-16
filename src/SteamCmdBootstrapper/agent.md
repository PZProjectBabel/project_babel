# SteamCmdBootstrapper

Structure: service project referencing `Common`.

Function: download and extract the official SteamCMD archive for the current platform before mod downloads.

Inputs: `PipelineConfig.baseDir`, Valve SteamCMD CDN.

Outputs: refreshed `src/3rd_party/steamcmd/` runtime files; preserves its `.gitignore`.

Notes: Windows extracts ZIP; Linux extracts tar.gz and marks executables runnable. Failures throw to the pipeline's fatal error handler.