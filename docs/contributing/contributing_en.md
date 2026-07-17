# Contributing Guide (CONTRIBUTING)

> [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Table of Contents

- [1. Before You Start](#1-before-you-start)
- [2. How Can I Contribute?](#2-how-can-i-contribute)
- [3. Provide Translation Rules, Terminology Dictionary, Improve System Prompts](#3-provide-translation-rules-terminology-dictionary-improve-system-prompts)
- [4. Provide manually proofread corpus](#4-provide-manually-proofread-corpus)
- [5. Pipeline and tool development contributions](#5-pipeline-and-tool-development-contributions)
- [6. Copyright and License Agreement](#6-copyright-and-license-agreement)
  - [6.1 Basic Principle: You retain copyright while granting the project the right to use](#61-basic-principle-you-retain-copyright-while-granting-the-project-the-right-to-use)
  - [6.2 License for Text and Images, etc. (CC BY-NC-SA 4.0)](#62-license-for-text-and-images-etc-cc-by-nc-sa-40)
  - [6.3 Licensing of Scripts and Tool Code (GPL-3.0)](#63-licensing-of-scripts-and-tool-code-gpl-30)
  - [6.4 Upstream Works and Original Game Copyright](#64-upstream-works-and-original-game-copyright)
- [7. Communication and Collaboration](#7-communication-and-collaboration)
- [8. Financial Support](#8-financial-support)

---

Thank you very much for being willing to contribute to **Project Babel - Project Zomboid Mod LLM Auto Translation Project**! Whether it's fixing a bug, adding a feature, writing prompt templates, or providing reference translations!

Calling the LLM API for translation requires paying for tokens. To ensure the long-term stable operation of the project, we hope you can generously help!

> ⚠️ **Important Reminder:**
> Before submitting any content to this repository, please be sure to read and understand the "Copyright and License Agreement" section.
> Once submitted and merged, you are deemed to have agreed to the corresponding license terms.

---

## 1. Before You Start

Please read the project `README.md` first to understand:
- The overall goal and current status of this project;
- How ordinary players can use this project (for self-testing);
- Technical details of the project.

---

## 2. How Can I Contribute?

You can choose one or more ways to participate based on your interests and skills:

- Provide translation rules for the target language
- Provide a translation terminology dictionary for the target language
- Improve system prompts
- Provide human-proofread translated text corpora
- Improve pipeline modules (.NET) and automation scripts
- Report issues, suggest improvements (describe in Issues)
- Provide financial support for LLM calls

Below are some explanations for the main contribution scenarios.

---

## 3. Provide Translation Rules, Terminology Dictionary, Improve System Prompts

The pipeline's prompt templates are located in `src/prompt_templates/`, with the following structure:

- `system_prompt_translate_engine.txt`: Global translation engine system prompt (shared by all languages);
- `<language_code>/translation_dictionary_<language_code>.json`: Terminology dictionary for that language;
- `<language_code>/translation_schema_<language_code>.md`: Translation rules and style constraints for that language.

Contribution steps:

1. Create a subdirectory under `src/prompt_templates/` for your language, add the terminology dictionary and translation rules file;
2. If you need to adjust the global translation behavior, modify `system_prompt_translate_engine.txt` (note that this affects all languages);
3. Test locally to confirm the effect;
4. Submit a PR.

---

## 4. Provide manually proofread corpus

If you are a translation mod maker and are willing to provide your translation corpus as a reference for LLM translation, please initiate an application in an Issue. You need to provide the following materials:

- Your translation mod's Mod ID and the target language;
- A screenshot of your translation mod's backend page to prove you are the mod author;
- Explicitly state in the Issue that you are willing to provide the translation corpus;
- If there are special circumstances (special authorization, etc.), please explain as well;
- Please ensure that the corpus you provide is of high quality.

Under your authorization, the project will include your mod in the `config/ref_translation_mods.json` reference translation mod list, and the pipeline will automatically sync your translated text as RAG reference corpus.

---

## 5. Pipeline and tool development contributions

The automation of this project is divided into two parts:

**Pipeline module (`src/`, C# / .NET 10)**: Contains 15 sequentially executed modules responsible for the complete process from SteamCMD initialization, mod download, text extraction, content review, embedding calculation, RAG retrieval to LLM translation and final output. See [Technical Reference](../technical_reference/technical_reference_en.md).

**Helper scripts (`.github/`)**: Used for GitHub automation.

If you wish:

* Fix bugs in existing pipeline modules or scripts;
* Add new features or new modules to the pipeline;
* Optimize performance or code structure;
* Improve prompt templates or RAG strategies;

You can follow these steps:

1. Fork this repository and clone it locally;
2. Create a new branch based on the latest branch;
3. Modify or add files in the corresponding directories:
- Pipeline module modifications → `src/<module_name>/`;
- Script modifications → `scripts/`;
- Prompt template modifications → `src/prompt_templates/`;
4. Before submitting, please try to:

* Maintain the original code style;
* Add necessary comments;
* If possible, include simple test or usage instructions;
5. Submit changes via PR, and describe in the description:

* Purpose of the change;
* Directories/modules/scripts potentially affected;
* Whether it involves breaking changes.

---

## 6. Copyright and License Agreement

> **Important Note:**
> The copyright and license agreement is intended to protect the legitimate rights and interests of the project, authors, contributors, and players, and to avoid misunderstandings caused by "tacit understanding" or "default". Please read it carefully.
> The copyright and license are based on the content in the README.md file. This section only provides a more accessible description.

### 6.1 Basic Principle: You retain copyright while granting the project the right to use

* You still hold the copyright to the content you create (translations, images, scripts/programs, etc.);
* However, after submitting these contents to this project and having them accepted (merged), you agree to authorize others to use these contents externally in accordance with the open-source/shared license adopted by this project.

This means:

* You **can still** continue to use and display your own work elsewhere;
* But you **cannot** require this project or other users who have legally obtained the work to "revoke the license" or "delete historical versions" after your contribution has been merged.

### 6.2 License for Text and Images, etc. (CC BY-NC-SA 4.0)

For the following content you submit:

* Game text translations, polish, and proofreading;
* Project documentation, explanatory text;
* Images and art resources created specifically for this project;

Once adopted and merged into this repository, you are deemed to have agreed:

1. These contents are licensed under the **Attribution-NonCommercial-ShareAlike 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abbreviated as **CC BY-NC-SA 4.0**) license;
2. Project Babel and all users who obtain this content may, under the premise of **complying with the CC BY-NC-SA 4.0 terms**:
* Share, copy, and redistribute this content;
* Modify and create derivative works for non-commercial purposes;
3. You agree that, to the extent permitted by applicable law, this license is **non-exclusive, worldwide, royalty-free, and irrevocable**;
4. Even if you withdraw or stop participating in this project in the future, this project may continue to use and republish the relevant content you have submitted and merged in accordance with CC BY-NC-SA 4.0.

> If you do not accept the above licensing method, please do not submit text or image contributions to this project,
> or communicate with the project maintainer in advance to confirm whether other methods of collaboration are possible.

### 6.3 Licensing of Scripts and Tool Code (GPL-3.0)

For contributions you submit and are accepted:

* Automated scripts;
* Build/export tools;
* Other program code for processing this localization project;

Unless otherwise stated, you agree to:

1. The code is licensed under **GPL-3.0** (GNU General Public License, Version 3);
2. Project maintainers may modify, merge, and distribute it within the scope permitted by GPL-3.0;
3. You may also carry out other projects based on the same code, as long as you comply with the terms of GPL-3.0.

To avoid introducing license conflicts, please try to:

* Do not introduce third-party code that is **incompatible with GPL-3.0** without confirmation;
* If you really need to reference a third-party library, clearly state its source and license in the PR and confirm its compatibility.

### 6.4 Upstream Works and Original Game Copyright

This project is an **unofficial translation** project for mods related to *Project Zomboid*:

* The copyright of the original game and each mod belongs to their respective authors/publishers;
* This project only creates and organizes text translations, polishing adjustments, and some accompanying resources;
* Contributors should ensure when submitting content:
* Do not directly copy unauthorized third-party localization text or art resources;
* Respect the rights of original authors and mod authors, and do not engage in infringing reproduction.

---

## 7. Communication and Collaboration

If you:

* Have questions about the license terms;
* Are unsure whether a certain piece of content can be contributed;
* Wish to license your work in a special way (e.g., non-commercial only but no adaptation allowed, etc.);

Welcome to contact the project maintainers through the following methods:

* Submit an Issue for discussion;
* Other contact methods publicly provided by maintainers.

We will try to find a solution that balances the healthy development of the project while respecting the rights of all parties.

---

## 8. Financial Support

During project operation, due to adding new mods, updating text content of old mods, etc., there is a continuous need to call the LLM API for translation. In order to constrain the LLM behavior, in addition to the basic mod text, a large amount of prompt content (including base prompts, translation rules, terminology tables, input/output constraints, semantic query results, etc.) needs to be provided. These contents consume far more tokens than the original text. Therefore, the project requires financial support.

If you are willing to provide financial support, please contact the project maintainers. Thank you very much!

---

Thank you again for your willingness to contribute to this project!
Every contribution you make will benefit more players!
