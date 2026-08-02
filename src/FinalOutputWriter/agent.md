# FinalOutputWriter

Structure: service project referencing `Common`.

Function: write final mod translation files for PZ mod distribution.

Inputs: translation entries, ref mod IDs, target languages, base_game_keys dir.

Outputs:
- `final_outputs/project_babel/contents/mods/project_babel/42.20/media/lua/shared/Translate/<gamecode>/*.json`
- `final_outputs/project_babel/contents/mods/project_babel/42/media/lua/shared/Translate/<gamecode>/*.json`
- Empty `media/AnimSets` and `media/actiongroups` directories under `common`, `42`, and `42.20` for the B42 animation loader.
- Both dirs identical; write 42.20 first, copy to 42.

Rules:
- Group by key root (prefix before first `_`), map to file via base_game_keys prefix→file mapping.
- Exclude keys present in base_game_keys (no override).
- Exclude entries from reference translation mods.
- Only non-empty translated text.
- Output ALL translated entries, not incremental.
- JSON format: `{"key": "translated_text"}` flat dict.
