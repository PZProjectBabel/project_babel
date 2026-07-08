# Contributing Guide (CONTRIBUTING)

> [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Thank you for your willingness to contribute to **Project Babel — the LLM-powered automatic translation project for Project Zomboid mods**! Whether it's fixing a bug, adding a feature, writing prompt templates, or providing reference translations — every contribution matters!

Calling the LLM API for translation costs tokens. To keep the project running sustainably in the long term, your generous support is greatly appreciated!

> ⚠️ **Important Notice:**  
> Before submitting anything to this repository, please be sure to read and understand the "Copyright & Licensing" section.  
> Once submitted and merged, you are deemed to have agreed to the corresponding licensing terms.

---

## Before You Start

Please read the project `README.md` to understand:

- The overall goals and current status of this project;
- How regular players use this project (for your own testing);
- Technical details of the project.

---

## How Can I Contribute?

You can choose one or more ways to participate based on your interests and skills:

- Provide translation rules for a target language
- Provide a term dictionary for a target language
- Improve the system prompt templates
- Provide manually proofread translation corpora
- Improve pipeline modules (.NET) and automation scripts
- Report issues and suggest improvements (via Issues)
- Provide financial support for LLM API calls

Below are explanations for the main contribution scenarios.

---

## Providing Translation Rules, Term Dictionaries, and Improving System Prompts

The pipeline's prompt templates are located in `src/prompt_templates/`, with the following structure:

- `system_prompt_translate_engine.txt`: the global translation engine system prompt (shared by all languages);
- `<language_code>/translation_dictionary_<language_code>.json`: the term dictionary for that language;
- `<language_code>/translation_schema_<language_code>.md`: the translation rules and style constraints for that language.

Contribution steps:

1. Create a subdirectory under `src/prompt_templates/` for your language, and add the term dictionary and translation rule files;
2. If you need to adjust global translation behavior, modify `system_prompt_translate_engine.txt` (note: this affects all languages);
3. Test locally to confirm the results;
4. Submit a PR.

---

## Providing Manually Proofread Corpora

If you are a translation mod author and are willing to provide your translation corpus as LLM translation reference, please submit a request via an Issue. You need to provide the following information:

- The Mod ID of your translation mod and the target language;
- A screenshot of your translation mod's backend page to prove you are the mod author;
- A clear statement in the Issue that you are willing to provide the translation corpus;
- If there are special circumstances (special licensing, etc.), please explain;
- Please ensure the corpus you provide is of high quality.

With your authorization, the project will add your mod to the `config/ref_translation_mods.json` reference translation mod list, and the pipeline will automatically sync your translation texts as RAG reference corpora.

---

## Pipeline & Tool Development Contributions

The automation in this project is divided into two parts:

**Pipeline modules (`src/`, C# / .NET 10)**: Contains 15 sequentially executed modules, responsible for the complete workflow from mod downloading, text extraction, content review, embedding computation, RAG retrieval, to LLM translation and final output. See the [technical documentation](../translation_entry_pipeline_zh-hans.md) for details.

**Auxiliary scripts (`.github/`)**: Used for GitHub automation.

If you wish to:

* Fix bugs in existing pipeline modules or scripts;
* Add new features or modules to the pipeline;
* Optimize performance or code structure;
* Improve prompt templates or RAG strategies;

You can follow these steps:

1. Fork this repository and clone it locally;
2. Create a new branch from the latest branch;
3. Modify or add files in the corresponding directories:
   - Pipeline module changes → `src/<module_name>/`;
   - Script changes → `scripts/`;
   - Prompt template changes → `src/prompt_templates/`;
4. Before submitting, please try to:

   * Maintain the existing code style;
   * Add necessary comments;
   * If possible, include simple tests or usage instructions;
5. Submit changes via PR, and explain in the description:

   * The purpose of the changes;
   * The directories / modules / scripts that may be affected;
   * Whether it involves breaking changes.

---

## Copyright & Licensing

> **Friendly Reminder:**
> The copyright and licensing terms are designed to protect the legitimate rights and interests of the project, authors, contributors, and players, and to avoid misunderstandings arising from "tacit agreement" or "default assumptions." Please read them carefully.
> The copyright and licensing terms are governed by the content in the README.md file; this section only provides a more accessible description.

### 1. Basic Principle: You retain copyright, while licensing the project to use your work

* You still hold the copyright to the content you create (translations, images, scripts/programs, etc.);
* However, once these are submitted to this project and accepted (merged),
  you agree to license others to use this content under the open-source/shared license adopted by this project.

This means:

* You **may still** continue to use and display your work elsewhere;
* But you **cannot**, after your contribution is merged, demand that this project or other users who have legally obtained the work "revoke the license" or "delete historical versions."

### 2. Licensing of Text, Images, and Similar Content (CC BY-NC-SA 4.0)

For the following content you submit:

* Game text translations, polishing, and proofreading;
* Project documentation and explanatory text;
* Images and art assets created specifically for this project;

Once accepted and merged into this repository, you are deemed to agree that:

1. These contents are licensed under **Attribution-NonCommercial-ShareAlike 4.0 International**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abbreviated as **CC BY-NC-SA 4.0**);
2. Project Babel and all users who receive this content may, **in compliance with the CC BY-NC-SA 4.0 terms**:

   * Share, copy, and redistribute this content;
   * Modify and create derivative works from it for non-commercial purposes;
3. You agree that this license is **non-exclusive, worldwide, royalty-free, and irrevocable** to the extent permitted by applicable law;
4. Even if you later withdraw or stop participating in this project, the project may continue to use and redistribute the relevant content you have submitted and that has been merged, under CC BY-NC-SA 4.0.

> If you do not accept the above licensing terms, please do not submit text or image contributions to this project,
> or communicate with the project maintainers in advance to confirm whether collaboration is possible under other arrangements.

### 3. Licensing of Scripts and Tool Code (GPL-3.0)

For the following you submit and have accepted:

* Automation scripts;
* Build/export tools;
* Other program code used for processing this translation project;

In the absence of special declarations, you are deemed to agree that:

1. The code is licensed under **GPL-3.0** (GNU General Public License version 3);
2. Project maintainers may modify, merge, and distribute it within the scope permitted by GPL-3.0;
3. You may also continue other projects based on the same code, as long as you comply with the GPL-3.0 terms.

To avoid introducing licensing conflicts, please try to:

* Not introduce third-party code that is **incompatible with GPL-3.0** without confirmation;
* If you do need to reference third-party libraries, clearly state their source and license in the PR, and confirm their compatibility.

### 4. Upstream Works and Original Game Copyright

This project is an **unofficial translation** project for mods related to *Project Zomboid*:

* The copyright of the original game and each mod belongs to their respective authors/publishers;
* This project only involves the creation and organization of text translations, polish adjustments, and some supporting resources;
* When submitting content, contributors should ensure:

  * Not to directly copy unauthorized third-party translation texts or art assets;
  * To respect the rights of original authors and mod authors, and not to engage in infringing re-distribution.

---

## Communication & Collaboration

If you have:

* Questions about the licensing terms;
* Uncertainty about whether certain content can be contributed;
* A desire to license your work in a special way (e.g., non-commercial only but no adaptation allowed, etc.);

You are welcome to contact the project maintainers through:

* Submitting an Issue for discussion;
* Other publicly available contact methods of the maintainers.

We will do our best to find a solution that balances the healthy development of the project while respecting the rights and interests of all parties.

---

## Financial Support

During the project's operation, due to new mod additions and text updates to existing mods, the LLM API needs to be called continuously for translation. To constrain the LLM's behavior, in addition to the base mod texts, a large amount of prompt content is required (including base prompts, translation rules, term tables, input/output constraints, semantic query results, etc.), which consumes far more tokens than the original texts. Therefore, the project needs financial support.

If you are willing to provide financial support, please contact the project maintainers. Thank you very much!

---

Thank you again for your willingness to contribute to this project!
Every contribution you make benefits more players!
