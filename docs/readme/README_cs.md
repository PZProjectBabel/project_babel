# Project Babel — Automatický překlad PZ modů pomocí LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Poznámka:** Tento překlad zatím není podporován. Oficiální obsah je v [čínské verzi](../../README.md).

---

*Tento překladatelský projekt je poháněn a udržován nástrojem [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Obsah

- [Podporované cílové jazyky](#podporované-cílové-jazyky)
- [Jak nainstalovat a používat](#jak-nainstalovat-a-používat)
- [Průběh překladu](#průběh-překladu)
- [Jak přispět](#jak-přispět)
- [Nástroje a struktura adresářů (pro vývojáře)](#nástroje-a-struktura-adresářů-(pro-vývojáře))
- [Autorská práva a licence](#autorská-práva-a-licence)
- [Poděkování](#poděkování)
- [Software třetích stran](#software-třetích-stran)

---

## Podporované cílové jazyky

| Jazyk | Místní název | ISO kód | Kód ve hře | Podporováno | Poznámka |
|------|------|------|------|------|------|
| Arabština | العربية | `ar` | `AR` | ❌ | Nedostatek financí |
| Katalánština | català | `ca` | `CA` | ❌ | Nedostatek financí |
| Tradiční čínština | 繁體中文 | `zh-hant` | `CH` | ❌ | Nedostatek financí |
| Zjednodušená čínština | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Čeština | čeština | `cs` | `CS` | ❌ | Nedostatek financí |
| Dánština | dansk | `da` | `DA` | ❌ | Nedostatek financí |
| Němčina | Deutsch | `de` | `DE` | ✅ | |
| Angličtina | English | `en` | `EN` | ✅ | |
| Španělština | español | `es` | `ES` | ❌ | Nedostatek financí |
| Finština | suomi | `fi` | `FI` | ❌ | Nedostatek financí |
| Francouzština | français | `fr` | `FR` | ✅ | |
| Maďarština | magyar | `hu` | `HU` | ❌ | Nedostatek financí |
| Indonéština | Bahasa Indonesia | `id` | `ID` | ❌ | Nedostatek financí |
| Italština | italiano | `it` | `IT` | ❌ | Nedostatek financí |
| Japonština | 日本語 | `ja` | `JP` | ✅ | |
| Korejština | 한국어 | `ko` | `KO` | ❌ | Nedostatek financí |
| Nizozemština | Nederlands | `nl` | `NL` | ❌ | Nedostatek financí |
| Norština | norsk | `no` | `NO` | ❌ | Nedostatek financí |
| Tagalština | Tagalog | `tl` | `PH` | ❌ | Nedostatek financí |
| Polština | polski | `pl` | `PL` | ❌ | Nedostatek financí |
| Portugalština (Portugalsko) | português | `pt` | `PT` | ❌ | Nedostatek financí |
| Portugalština (Brazílie) | português do Brasil | `pt-br` | `PTBR` | ❌ | Nedostatek financí |
| Rumunština | română | `ro` | `RO` | ❌ | Nedostatek financí |
| Ruština | русский | `ru` | `RU` | ❌ | Nedostatek financí |
| Thajština | ภาษาไทย | `th` | `TH` | ❌ | Nedostatek financí |
| Turečtina | Türkçe | `tr` | `TR` | ❌ | Nedostatek financí |
| Ukrajinština | українська | `uk` | `UA` | ❌ | Nedostatek financí |

**Celkem**: 27 plánovaných jazyků | **Podporováno**: 5 | **Čeká**: 22

---

## Jak nainstalovat a používat

Průvodce pro hráče, kteří chtějí používat překladový balíček ve hře.

1. Přejděte na stránku Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Klikněte na "Odebírat".
3. Spusťte hru a povolte tento překladový mod v nabídce Mody.
4. Překladový text z později načtených modů přepisuje dřívější, takže tento překladový mod musí být načten až po herních modech.
5. Užijte si to!

---

## Průběh překladu

[➡️ Průběh překladu](../progress/progress_cs.md)

---

## Jak přispět

Vítáme příspěvky! Opravy překladů, nové funkce, šablony promptů nebo referenční překlady.

Volání LLM API pro překlad vyžaduje placení tokenů. Vaše podpora pomáhá projektu dlouhodobě fungovat!

---

## Nástroje a struktura adresářů (pro vývojáře)

Tato sekce je určena vývojářům, kteří chtějí porozumět vnitřní automatizaci projektu.

### Adresáře projektu

| Adresář | Popis |
|------|------|
| `src/` | Zdrojový kód překladového pipeline .NET 10, 15 modulů |
| `config/` | Konfigurace pipeline (LLM, Steam, parametry RAG atd.) |
| `data/` | Běhová data: metadata modů, embeddingy, překladová cache |
| `translation_ref/` | Referenční překlady jako kontext LLM |
| `base_game_keys/` | Překladové klíče základní hry pro deduplikaci |
| `final_outputs/` | Finální výstup ve formátu PZ modu |
| `docs/` | Dokumentace: postup, přispívání, specifikace pipeline |
| `temp/` | Dočasné soubory pipeline |
| `src/prompt_templates/` | Šablony promptů LLM |

### Moduly pipeline (pořadí provádění)

| Krok | Modul | Funkce |
|------|------|------|
| 1 | `ConfigReader` | Načíst konfiguraci/tajemství/jazyky |
| 2 | `RepoDataLoader` | Načíst reference a překladovou cache |
| 3 | `ModIdCollector` | Shromáždit ID modů Workshop |
| 4 | `ModInfoFetcher` | Získat metadata Steam |
| 5 | `ModDownloader` | Stáhnout mody přes steamcmd |
| 6 | `ContentExtractor` | Analyzovat překladové soubory → `TranslationEntry` |
| 7 | `ContentChecker` | Kontrola bezpečnosti obsahu |
| 8 | `EmbeddingFetcher` | Vypočítat embedding vektory textu |
| 9 | `TranslationBatcher` | Vytvořit dávky překladu |
| 10 | `RagContextRetriever` | Získat kontexty RAG |
| 11 | `LLMTranslator` | Provést překlad LLM |
| 12 | `ResultWriter` | Zapsat do data/ a translation_ref/ |
| 13 | `FinalOutputWriter` | Generovat finální výstup ve formátu PZ modu |
| 14 | `ProgressReporter` | Generovat zprávy o postupu |

### Technologický stack

- **Jazyk**: C# (.NET 10)
- **Cílová platforma**: GitHub Actions Linux x64 runner
- **Testy**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurovatelné)
- **Embedding**: Textová vektorizace pro RAG vyhledávání podobnosti
- **Kontrola obsahu**: Víceúrovňová bezpečnostní kontrola řízená LLM

Podrobná technická dokumentace: [TranslationEntry pipeline](../pipeline/translation_entry_pipeline_cs.md)

---

## Autorská práva a licence

© 2025 Project Babel a všichni autoři. Všechna práva vyhrazena.

### Obsah (texty, obrázky)

Licencováno pod **CC BY-NC-SA 4.0**.

- **Uvedení autora**: Uvést úpravy založené na „Project Babel", s odkazy na repozitář a Workshop
- **Nekomerční**: Komerční využití zakázáno
- **Zachovejte licenci**: Úpravy musí být zveřejněny pod stejnou licencí

### Kód

Kód v `src/` je licencován pod **GPL-3.0**.

---

## Poděkování

| Referenční mod | Autor | Stránka |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Srdečné díky výše uvedeným autorům!**

---

## Software třetích stran

Tento projekt používá programy a knihovny třetích stran, autorská práva náleží příslušným vývojářům.
