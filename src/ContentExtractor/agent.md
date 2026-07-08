# ContentExtractor

Structure: service project referencing `Common` and `ModDownloader`.

Function: extract Project Zomboid translation content from downloaded mods into shared `TranslationEntry` dictionaries.

Inputs: `ModInfo.localDownloadedPath`, supported languages, base language, downloaded mod folder tree.

Outputs:
- fills `Dictionary<string, TranslationEntry>` keyed by `modId::translationKey`.
- writes debug text files to `extractedContentsTempDir/<iso>/<modid>.txt`.
- writes key/file mapping under `extracted_contents/translation_key_to_file_mapping/`.

Formats:
- base: `<key>::en = "<value>"`.
- non-base: `<key>::en = "<source>"` plus `<key>::<iso>::unverified = "<value>"`.

Notes: Parser order matters. Keep txt/json merge behavior: txt first, JSON wins, higher game version wins within same source type. Relaxed parse diagnostics go to run temp `txt/fuck.txt`.
