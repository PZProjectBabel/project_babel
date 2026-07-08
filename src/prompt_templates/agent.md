# prompt_templates

Structure: prompt files used by `ContentChecker` and `LLMTranslator`.

Function:
- `content_verification.txt`: content safety system prompt.
- `system_prompt_translate_engine.txt`: common translation system prompt; contains `{{TARGET_LANG}}`.
- `<Target_Name>/translation_schema_<Target_Name>.md`: target-specific schema/style rules.
- `<Target_Name>/translation_dictionary_<Target_Name>.json`: optional terminology dictionary.
- `translation_output.md`: common strict output rules shared by every target language.

Notes: Target folder names come from language English names with spaces/hyphens replaced by `_`. Missing target-specific rules/dictionaries are allowed; the common system prompt still supplies target language.
