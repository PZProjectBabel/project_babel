# Project Babel — LLM Auto-Translation Mod for Project Zomboid

> [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*This translation project is powered and maintained by the [Project Babel](https://github.com/PZProjectBabel/project_babel) tool suite.*

---

## Table of Contents

- [Supported Target Translation Languages](#supported-target-translation-languages)
- [How to Install and Use](#how-to-install-and-use)
- [Translation Progress](#translation-progress)
- [How to Contribute](#how-to-contribute)
- [Tools and Directory Structure (For Developers)](#tools-and-directory-structure-for-developers)
  - [Project Directory](#project-directory)
  - [Pipeline Modules (In Execution Order)](#pipeline-modules-in-execution-order)
  - [Independent Modules](#independent-modules)
  - [Tech Stack](#tech-stack)
- [Copyright and License](#copyright-and-license)
  - [1. Text and Images etc.](#1-text-and-images-etc)
  - [2. Program, scripts and other development content](#2-program-scripts-and-other-development-content)
- [Acknowledgments](#acknowledgments)
- [Third-Party Programs](#third-party-programs)

---

## Supported Target Translation Languages

| Language | Native Name | ISO Code | In-game Code | Supported | Notes |
|------|------|------|------|------|------|
| Arabic | العربية | `ar` | `AR` | ❌ | Insufficient token quota |
| Catalan | català | `ca` | `CA` | ❌ | Insufficient token quota |
| Traditional Chinese | 繁體中文 | `zh-hant` | `CH` | ❌ | Insufficient token quota |
| Simplified Chinese | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Czech | čeština | `cs` | `CS` | ❌ | Insufficient token quota |
| Danish | dansk | `da` | `DA` | ❌ | Insufficient token quota |
| German | Deutsch | `de` | `DE` | ✅ | |
| English | English | `en` | `EN` | ✅ | |
| Spanish | español | `es` | `ES` | ❌ | Insufficient token quota |
| Finnish | suomi | `fi` | `FI` | ❌ | Insufficient token quota |
| French | français | `fr` | `FR` | ✅ | |
| Hungarian | magyar | `hu` | `HU` | ❌ | Insufficient token quota |
| Indonesian | Bahasa Indonesia | `id` | `ID` | ❌ | Insufficient token quota |
| Italian | italiano | `it` | `IT` | ❌ | Insufficient token quota |
| Japanese | 日本語 | `ja` | `JP` | ✅ | |
| Korean | 한국어 | `ko` | `KO` | ❌ | Insufficient token quota |
| Dutch | Nederlands | `nl` | `NL` | ❌ | Insufficient token quota |
| Norwegian | norsk | `no` | `NO` | ❌ | Insufficient token quota |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Insufficient token quota |
| Polish | polski | `pl` | `PL` | ❌ | Insufficient token quota |
| Portuguese (Portugal) | português | `pt` | `PT` | ❌ | Insufficient token quota |
| Portuguese (Brazil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Insufficient token quota |
| Romanian | română | `ro` | `RO` | ❌ | Insufficient token quota |
| Russian | русский | `ru` | `RU` | ❌ | Insufficient token quota |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Insufficient token quota |
| Turkish | Türkçe | `tr` | `TR` | ❌ | Insufficient token quota |
| Ukrainian | українська | `uk` | `UA` | ❌ | Insufficient token quota |

**Total**: 27 planned languages | **Supported**: 5 | **Pending**: 22

---

## How to Install and Use

This guide is for players who want to use this translation project directly in the game.

1.  Go to our Steam Workshop page: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Click the "Subscribe" button.
3.  Launch the game, and enable this translation mod in the "Mods" management on the main menu.
4.  Translation texts from mods loaded later take priority over those loaded earlier, so this translation mod should be enabled after the functional mods (try to place it at the bottom).
5.  Enjoy the game!

---

## Translation Progress

**[➡️ Click here to view translation progress](./docs/progress/progress_en.md)**

---

## How to Contribute

We welcome anyone to contribute, whether it's fixing a bug, adding a feature, writing prompt templates, or providing reference translations!

Calling the LLM API for translation requires paying for tokens. To ensure the long-term stable operation of the project, we hope you can generously support us!

For details, please read the [Contribution Guide](./docs/contributing/contributing_en.md)

---

## Tools and Directory Structure (For Developers)

This section is for developers who wish to understand the automation principles of the project.

### Project Directory

| Directory | Description |
|------|------|
| `src/` | .NET 10 translation pipeline source code, with 15 modules + 2 standalone modules |
| `config/` | Pipeline configuration files (LLM, Steam, RAG parameters, etc.) |
| `data/` | Runtime data: mod metadata, embeddings, translation cache |
| `translation_ref/` | Reference translation data (e.g., authorized mods from As1 Chinese Translation Group), providing translation references for LLM |
| `base_game_keys/` | Base game translation keys, used for deduplication to prevent overwriting native text |
| `final_outputs/` | Final output: `project_babel/` mod package, `icons/` icons, and `workshop_descriptions/` workshop descriptions |
| `docs/` | Project documentation: progress reports, contribution guide, pipeline description |
| `temp/` | Pipeline temporary files (separate directory per run) |
| `src/prompt_templates/` | LLM prompt templates (translation/content review) |

### Pipeline Modules (In Execution Order)

| Step | Module | Function |
|------|------|------|
| 1 | `ConfigReader` | Load configuration/keys/language list |
| 2 | `RepoDataLoader` | Load reference translations and translation cache |
| 3 | `ModIdCollector` | Collect Workshop mod IDs |
| 4 | `ModInfoFetcher` | Fetch Steam metadata |
| 5 | `SteamCmdBootstrapper` | Prepare steamcmd runtime for current platform |
| 6 | `ModDownloader` | Download mods via steamcmd |
| 7 | `ContentExtractor` | Parse mod translation files → `TranslationEntry` |
| 8 | `ContentChecker` | Content safety review (drugs/porn/violence) |
| 9 | `EmbeddingFetcher` | Compute text embedding vectors |
| 10 | `TranslationBatcher` | Create translation batches independent of target language |
| 11 | `RagContextRetriever` | Retrieve RAG context (exact key + embedding similarity) |
| 12 | `LLMTranslator` | Call LLM to perform translation |
| 13 | `ResultWriter` | Write to data/ and translation_ref/ |
| 14 | `FinalOutputWriter` | Generate final PZ mod format output |
| 15 | `ProgressReporter` | Generate progress report |

### Independent Modules

| Module | Function |
|------|------|
| `WorkshopMonitor` | Regularly fetch new mods from Steam Workshop, filter by subscription count and include into `request_for_translation.txt` |
| `DocGenerator` | LLM-driven multilingual document generator |

### Tech Stack

- **Language**: C# (.NET 10)
- **Target Platform**: GitHub Actions Linux x64 runner
- **Testing**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurable)
- **Embedding**: Text vectorization for RAG similarity retrieval
- **Content Review**: LLM-driven multi-level safety audit

Detailed [technical reference](./docs/technical_reference/technical_reference_en.md).

---

## Copyright and License

The translation text content and related images of this translation project are created or adapted by **Project Babel** and contributors based on original game mods.

© 2025 Project Babel and respective authors. All rights reserved.

### 1. Text and Images etc.

Unless otherwise stated, in this repository:

- In-game text translations, refinements and proofreading content;
Project documentation, in-mod text translations;
Images and art resources specifically created for this project

All are licensed under the **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International** (abbreviated as **CC BY-NC-SA 4.0**) license.

This means you are free to share and adapt these content, provided you comply with the following conditions:

- **Attribution (BY)**: Indicate in a prominent location that "This translation project is based on the work of 'Project Babel'" and include a link to this repository and Steam Workshop link   `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **NonCommercial (NC)**: You may not use the content of this project or its derivative works for any direct or indirect commercial purposes  (including but not limited to paid integration packs, paid downloads, ad revenue sharing, etc.);
- **ShareAlike (SA)**: If you modify or create derivative works based on this project, you must publish your changes under **the same CC BY-NC-SA 4.0 license**.

For more information about this license, see:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.en>

*Special notes:*
- *The content of the base_game_keys folder comes from the base game, copyright belongs to the game developer! It is used to prevent translation keys from overwriting game keys (deduplication)*
- *The content of the translation_ref folder provides translation references for the LLM, copyright belongs to the respective mod developers!*

### 2. Program, scripts and other development content

Unless otherwise stated in the source files or directories, the program code used for creating/packaging/handling localization content in this repository (e.g., the code in the `src/` directory) is licensed under the **GNU General Public License version 3 (GPL-3.0)**.

For the full terms, please see the `LICENSE` file at the root of this repository (GPL-3.0), or visit the GNU official website: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Acknowledgments

This project uses third-party mods as reference texts for target language translation. The reference texts are sent to the LLM for translation reference.

| Reference Mod Name | Author | Mod Page |
|------|------|------|
| [B42] Unified Chinese Localization | As1 Localization Group (As1) | [Workshop Page](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Unified Mod Localization | As1 Localization Group (As1) | [Workshop Page](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Unified Ark Localization | As1 Localization Group (As1) | [Workshop Page](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Sincere thanks to the above authors!**

---

## Third-Party Programs

This project uses third-party programs and libraries, whose copyrights belong to their respective developers.

