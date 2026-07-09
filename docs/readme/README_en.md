# Project Babel — PZ Mod LLM Auto-Translation

> [简体中文](../../README.md)  <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Driven and maintained by the [Project Babel](https://github.com/PZProjectBabel/project_babel) toolset.*

---

## Table of Contents

- [Supported Target Languages](#supported-target-languages)
- [Install & Use](#install--use)
- [Translation Progress](#translation-progress)
- [Contributing](#contributing)
- [Directory Structure (Developers)](#directory-structure-developers)
- [Copyright & License](#copyright--license)
- [Acknowledgments](#acknowledgments)
- [Third-Party Software](#third-party-software)

---

## Supported Target Languages

| Language | Native Name | ISO Code | In-Game Code | Supported | Notes |
|------|------|------|------|------|------|
| Arabic | العربية | `ar` | `AR` | ❌ | Insufficient token credits |
| Catalan | català | `ca` | `CA` | ❌ | Insufficient token credits |
| Traditional Chinese | 繁體中文 | `zh-hant` | `CH` | ❌ | Insufficient token credits |
| Simplified Chinese | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Czech | čeština | `cs` | `CS` | ❌ | Insufficient token credits |
| Danish | dansk | `da` | `DA` | ❌ | Insufficient token credits |
| German | Deutsch | `de` | `DE` | ✅ | |
| English | English | `en` | `EN` | ✅ | |
| Spanish | español | `es` | `ES` | ❌ | Insufficient token credits |
| Finnish | suomi | `fi` | `FI` | ❌ | Insufficient token credits |
| French | français | `fr` | `FR` | ✅ | |
| Hungarian | magyar | `hu` | `HU` | ❌ | Insufficient token credits |
| Indonesian | Bahasa Indonesia | `id` | `ID` | ❌ | Insufficient token credits |
| Italian | italiano | `it` | `IT` | ❌ | Insufficient token credits |
| Japanese | 日本語 | `ja` | `JP` | ✅ | |
| Korean | 한국어 | `ko` | `KO` | ❌ | Insufficient token credits |
| Dutch | Nederlands | `nl` | `NL` | ❌ | Insufficient token credits |
| Norwegian | norsk | `no` | `NO` | ❌ | Insufficient token credits |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Insufficient token credits |
| Polish | polski | `pl` | `PL` | ❌ | Insufficient token credits |
| Portuguese (Portugal) | português | `pt` | `PT` | ❌ | Insufficient token credits |
| Portuguese (Brazil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Insufficient token credits |
| Romanian | română | `ro` | `RO` | ❌ | Insufficient token credits |
| Russian | русский | `ru` | `RU` | ❌ | Insufficient token credits |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Insufficient token credits |
| Turkish | Türkçe | `tr` | `TR` | ❌ | Insufficient token credits |
| Ukrainian | українська | `uk` | `UA` | ❌ | Insufficient token credits |

**Total**: 27 planned languages | **Supported**: 5 | **Pending**: 22

---

## Install & Use

For players who want to use the translation pack in-game:

1. Go to our Steam Workshop page: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Click "Subscribe".
3. Launch the game, enable this translation mod in the Mods menu.
4. Later-loaded mod translations override earlier ones, so this translation mod should be loaded after gameplay mods.
5. Enjoy!

---

## Translation Progress

[➡️ Translation Progress](../progress/progress_en.md)

---

## Contributing

We welcome contributions: translation fixes, new features, prompt templates, or reference translations!

LLM API calls cost tokens. Your support helps the project run sustainably!

Read the [Contributing Guide](../contributing/contributing_en.md) for details.

---

## Directory Structure (Developers)

This section is for developers who want to understand the project's automation internals.

### Project Directories

| Directory | Description |
|------|------|
| `src/` | .NET 10 translation pipeline source, 15 modules |
| `config/` | Pipeline config (LLM, Steam, RAG parameters, etc.) |
| `data/` | Runtime data: mod metadata, embeddings, translation cache |
| `translation_ref/` | Reference translations as LLM context |
| `base_game_keys/` | Base game translation keys for deduplication |
| `final_outputs/` | Final PZ mod-format translation output |
| `docs/` | Documentation: progress, contributing, pipeline specs |
| `temp/` | Per-run temporary pipeline files |
| `src/prompt_templates/` | LLM prompt templates |

### Pipeline Modules (execution order)

| Step | Module | Purpose |
|------|------|------|
| 1 | `ConfigReader` | Load config/secrets/languages |
| 2 | `RepoDataLoader` | Load reference & translation cache |
| 3 | `ModIdCollector` | Collect Workshop mod IDs |
| 4 | `ModInfoFetcher` | Fetch Steam metadata |
| 5 | `ModDownloader` | Download mods via steamcmd |
| 6 | `ContentExtractor` | Parse mod translation files → `TranslationEntry` |
| 7 | `ContentChecker` | Content safety review |
| 8 | `EmbeddingFetcher` | Compute text embedding vectors |
| 9 | `TranslationBatcher` | Create translation batches |
| 10 | `RagContextRetriever` | Retrieve RAG contexts |
| 11 | `LLMTranslator` | Execute LLM translation |
| 12 | `ResultWriter` | Write data/ & translation_ref/ |
| 13 | `FinalOutputWriter` | Generate final PZ mod output |
| 14 | `ProgressReporter` | Generate progress reports |

### Tech Stack

- **Language**: C# (.NET 10)
- **Target**: GitHub Actions Linux x64 runner
- **Tests**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurable)
- **Embedding**: Text vectorization for RAG similarity retrieval
- **Content Check**: LLM-driven multi-level safety review

Detailed technical docs: [TranslationEntry Pipeline](../pipeline/translation_entry_pipeline_en.md)

---

## Copyright & License

© 2025 Project Babel and authors. All rights reserved.

### Content (text, images)

Licensed under **CC BY-NC-SA 4.0**.

- **Attribution**: Credit "Project Babel" as the basis, include repo & Workshop links
- **Non-Commercial**: Commercial use prohibited
- **ShareAlike**: Modifications must be shared under the same license

### Code

Code under `src/` is licensed under **GPL-3.0**.

---

## Acknowledgments

| Reference Mod | Author | Page |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Deep gratitude to the authors above!**

---

## Third-Party Software

This project uses third-party programs and libraries; copyrights belong to their respective developers.
