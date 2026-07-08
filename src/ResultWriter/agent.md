# ResultWriter

Structure: service project referencing `Common` and `LLMTranslator`.

Function: write translations/embeddings/modinfo to `data/` + `translation_ref/`.

Inputs: `ModInfo` dict, ref modinfo/entries, translation entries, target lang iso.

Outputs:
- `data/translations/<target_iso>/<modid>.txt` : `key::base = "text",` + `key::target::processed|unprocessed::verified|unverified = "text",` including empty missing targets
- `data/embeddings/<modid>.bin` : zstd-compressed binary; per record: int32 keyLen + UTF8 `{key}|{sourceKind}|{targetLang}` + 16B raw hash + Half[384] fp16 vector
- `data/modinfos.json` : all ModInfo payload array
- `data/entry_metadata/<modid>.json` : active/source hash state
- `translation_ref/` : ref-only modinfos, per-mod entry metadata, translations, embeddings (same binary format as data/)

Methods:
- `WriteDataAsync` : normal data plus optional ref data
- `WriteRefDataAsync` : ref-only write
- `WriteResultsAsync` : translations .txt (per target lang)

Notes: Translation writes keep entries with source/known language state even when target text is empty. Non-base targets write the configured base source line only; missing base text stays empty. Data/ref are history stores.
