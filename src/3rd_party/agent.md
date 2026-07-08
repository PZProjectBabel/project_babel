# 3rd_party

Structure: bundled third-party runtime assets.

Function: `steamcmd/` is the template copied by `ModDownloader` into each download batch temp folder.

Notes: Treat contents as external binaries/assets. Do not edit steamcmd files as source code. `ModDownloader` may copy this folder and then steamcmd may self-update inside the temp copy.
