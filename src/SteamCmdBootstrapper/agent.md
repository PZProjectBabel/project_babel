# SteamCmdBootstrapper

Structure: service project referencing `Common`.

Function: refresh SteamCMD before mod downloads. Windows runs the repository-bundled SteamCMD so it self-updates; Linux downloads and extracts its platform archive.

Inputs: `PipelineConfig.baseDir`; Windows requires `src/3rd_party/steamcmd/steamcmd.exe`, while Linux uses Valve SteamCMD CDN.

Outputs: refreshed `src/3rd_party/steamcmd/` runtime files; preserves its `.gitignore`.

Notes: Windows must never download the SteamCMD bootstrap ZIP. Before updating it restores `steamcmd.exe` from its `.old` backup when the executable is missing (interrupted self-update), and never deletes/moves the exe beforehand; a missing executable with no backup fails immediately. Linux extracts tar.gz and marks executables runnable, but never deletes Windows binaries (`*.exe`/`*.dll`) or their `.old` backups. Failures throw to the pipeline's fatal error handler.