# Projekt Babel — Projekt automatického překladu modů pro Project Zomboid pomocí LLM

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Tento překladový projekt je poháněn a udržován nástrojovou sadou [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Obsah

- [Podporované cílové překladové jazyky](#podporované-cílové-překladové-jazyky)
- [Jak nainstalovat a používat](#jak-nainstalovat-a-používat)
- [Pokrok překladu](#pokrok-překladu)
- [Jak přispět](#jak-přispět)
- [Nástroje a struktura adresářů (pro vývojáře)](#nástroje-a-struktura-adresářů-pro-vývojáře)
  - [Adresář projektu](#adresář-projektu)
  - [Moduly pipeline (v pořadí provádění)](#moduly-pipeline-v-pořadí-provádění)
  - [Technologický stack](#technologický-stack)
- [Autorská práva a licence](#autorská-práva-a-licence)
  - [1. Texty, obrázky a další obsah](#1-texty-obrázky-a-další-obsah)
  - [2. Programy, skripty a další vývojářský obsah](#2-programy-skripty-a-další-vývojářský-obsah)
- [Poděkování](#poděkování)
- [Programy třetích stran](#programy-třetích-stran)

---

## Podporované cílové překladové jazyky

| Jazyk | Místní název | Mezinárodní kód | Kód ve hře | Podpora | Poznámka |
|------|------|------|------|------|------|
| Arabština | العربية | `ar` | `AR` | ❌ | Nedostatek tokenového kreditu |
| Katalánština | català | `ca` | `CA` | ❌ | Nedostatek tokenového kreditu |
| Tradiční čínština | 繁體中文 | `zh-hant` | `CH` | ❌ | Nedostatek tokenového kreditu |
| Zjednodušená čínština | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Čeština | čeština | `cs` | `CS` | ❌ | Nedostatek tokenového kreditu |
| Dánština | dansk | `da` | `DA` | ❌ | Nedostatek tokenového kreditu |
| Němčina | Deutsch | `de` | `DE` | ✅ | |
| Angličtina | English | `en` | `EN` | ✅ | |
| Španělština | español | `es` | `ES` | ❌ | Nedostatek tokenového kreditu |
| Finština | suomi | `fi` | `FI` | ❌ | Nedostatek tokenového kreditu |
| Francouzština | français | `fr` | `FR` | ✅ | |
| Maďarština | magyar | `hu` | `HU` | ❌ | Nedostatek tokenového kreditu |
| Indonéština | Bahasa Indonesia | `id` | `ID` | ❌ | Nedostatek tokenového kreditu |
| Italština | italiano | `it` | `IT` | ❌ | Nedostatek tokenového kreditu |
| Japonština | 日本語 | `ja` | `JP` | ✅ | |
| Korejština | 한국어 | `ko` | `KO` | ❌ | Nedostatek tokenového kreditu |
| Nizozemština | Nederlands | `nl` | `NL` | ❌ | Nedostatek tokenového kreditu |
| Norština | norsk | `no` | `NO` | ❌ | Nedostatek tokenového kreditu |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Nedostatek tokenového kreditu |
| Polština | polski | `pl` | `PL` | ❌ | Nedostatek tokenového kreditu |
| Portugalština (Portugalsko) | português | `pt` | `PT` | ❌ | Nedostatek tokenového kreditu |
| Portugalština (Brazílie) | português do Brasil | `pt-br` | `PTBR` | ❌ | Nedostatek tokenového kreditu |
| Rumunština | română | `ro` | `RO` | ❌ | Nedostatek tokenového kreditu |
| Ruština | русский | `ru` | `RU` | ❌ | Nedostatek tokenového kreditu |
| Thajština | ภาษาไทย | `th` | `TH` | ❌ | Nedostatek tokenového kreditu |
| turečtina | Türkçe | `tr` | `TR` | ❌ | Nedostatek tokenů |
| ukrajinština | українська | `uk` | `UA` | ❌ | Nedostatek tokenů |

**Celkem**: 27 plánovaných jazyků | **Podporováno**: 5 | **Čeká na podporu**: 22

---

## Jak nainstalovat a používat

Toto je průvodce pro hráče, kteří chtějí tento překladový projekt přímo používat ve hře.

1.  Přejděte na náš Steam Workshop: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Klikněte na tlačítko „Odebírat".
3.  Spusťte hru a v hlavním menu v sekci „Módy" povolte tento překladový mód.
4.  Překladové texty později povolených módů přepisují dříve povolené, proto tento překladový mód musí být povolen až po funkčních módech (co nejníže).
5.  Užijte si hru!

---

## Pokrok překladu

**[➡️ Klikněte zde pro zobrazení pokroku překladu](./docs/progress/progress_cs.md)**

---

## Jak přispět

Vítáme každého, kdo chce přispět, ať už opravou chyby, přidáním nové funkce, napsáním šablony promptu nebo poskytnutím referenčního překladu!

Volání LLM API pro překlad vyžaduje platbu za tokeny. Aby projekt mohl dlouhodobě stabilně fungovat, doufáme, že budete štědří!

Podrobnosti najdete v [Průvodci přispíváním](./docs/contributing/contributing_cs.md)

---

## Nástroje a struktura adresářů (pro vývojáře)

Tato sekce je určena vývojářům, kteří chtějí porozumět principům automatizace projektu.

### Adresář projektu

| Adresář | Popis |
|------|------|
| `src/` | Zdrojový kód překladového pipeline .NET 10, obsahuje 15 modulů |
| `config/` | Konfigurační soubory pipeline (LLM, Steam, parametry RAG atd.) |
| `data/` | Runtime data: metadata módů, embedding, překladová cache |
| `translation_ref/` | Referenční překladová data (autorizované módy od 如一汉化组), poskytují překladové reference pro LLM |
| `base_game_keys/` | Klíče překladu základní hry, slouží k deduplikaci a zabránění přepisování původního textu |
| `final_outputs/` | Konečný výstup: balíček módu `project_babel/`, ikony `icons/` a popisy Workshopu `workshop_descriptions/` |
| `docs/` | Dokumentace projektu: zprávy o pokroku, průvodce přispíváním, popis pipeline |
| `temp/` | Dočasné soubory pipeline (nezávislý adresář pro každé spuštění) |
| `src/prompt_templates/` | Šablony promptů pro LLM (překlad/kontrola obsahu) |

### Moduly pipeline (v pořadí provádění)

| Krok | Modul | Funkce |
|------|------|------|
| 1 | `ConfigReader` | Načíst konfiguraci/klíče/seznam jazyků |
| 2 | `RepoDataLoader` | Načíst referenční překlady a překladovou mezipaměť |
| 3 | `ModIdCollector` | Shromáždit ID Workshop modů |
| 4 | `ModInfoFetcher` | Získat Steam metadata |
| 5 | `SteamCmdBootstrapper` | Připravit steamcmd runtime pro aktuální platformu |
| 6 | `ModDownloader` | Stáhnout mody přes steamcmd |
| 7 | `ContentExtractor` | Parsovat soubory s překlady modů → `TranslationEntry` |
| 8 | `ContentChecker` | Bezpečnostní kontrola obsahu (drogy/pornografie/násilí) |
| 9 | `EmbeddingFetcher` | Vypočítat embedding vektory textu |
| 10 | `TranslationBatcher` | Vytvořit dávky překladu nezávislé na cílovém jazyce |
| 11 | `RagContextRetriever` | Vyhledat RAG kontext (přesné klíče + podobnost embeddingů) |
| 12 | `LLMTranslator` | Zavolat LLM k provedení překladu |
| 13 | `ResultWriter` | Zapsat do data/ a translation_ref/ |
| 14 | `FinalOutputWriter` | Vygenerovat konečný výstup ve formátu PZ modu |
| 15 | `ProgressReporter` | Vygenerovat zprávu o pokroku |

### Technologický stack

- **Jazyk**: C# (.NET 10)
- **Cílová platforma**: GitHub Actions Linux x64 runner
- **Testování**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurovatelné)
- **Embedding**: Textová vektorizace pro RAG podobnostní vyhledávání
- **Kontrola obsahu**: Víceúrovňová bezpečnostní kontrola řízená LLM

Podrobné [technické reference](./docs/technical_reference/technical_reference_cs.md).

---

## Autorská práva a licence

Překladatelský text a související obrázky tohoto projektu byly vytvořeny nebo odvozeny **Project Babel** a jednotlivými přispěvateli na základě původních herních modů.

© 2025 Project Babel a jednotliví autoři. Všechna práva vyhrazena.

### 1. Texty, obrázky a další obsah

Pokud není uvedeno jinak, v tomto repozitáři:

- Překlady, úpravy a korektury textů ve hře;
Dokumentace projektu, překlad textů v modulech.
Obrázky a grafické zdroje vytvořené speciálně pro tento projekt.

Všechna jsou licencována pod **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, zkráceně **CC BY-NC-SA 4.0**).

To znamená, že za dodržení následujících podmínek můžete tyto materiály volně sdílet a upravovat:

- **Uveďte autora (BY)**: Na viditelném místě uveďte „Tento překlad vychází z práce projektu ‚Project Babel‘“ a připojte odkaz na toto úložiště a Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Nekomerční použití (NC)**: Tyto materiály ani jejich odvozená díla nesmíte používat k žádnému přímému či nepřímému komerčnímu účelu (včetně, nikoli však výhradně, placených balíčků, placeného stahování, podílu z reklamy atd.).
- **Zachovejte stejnou licenci (SA)**: Pokud toto dílo upravíte nebo z něj vytvoříte odvozené dílo, musíte svou upravenou verzi zveřejnit pod **stejnou licencí CC BY-NC-SA 4.0**.

Další informace o této licenci naleznete na:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.cs>

*Zvláštní poznámka:*
- *Obsah složky `base_game_keys` pochází ze samotné hry a autorská práva náleží vývojářům hry! Obsah slouží k zabránění přepisu herních klíčů překladovými klíči (deduplikace).*
- *Obsah složky `translation_ref` slouží jako referenční překlad pro LLM a autorská práva náleží příslušným vývojářům modů!*

### 2. Programy, skripty a další vývojářský obsah

Pokud není ve zdrojovém souboru nebo adresáři uvedeno jinak, je programový kód v tomto úložišti sloužící k vytváření, balení a zpracování lokalizačního obsahu (např. programový kód v adresáři `src/`) licencován pod **GNU General Public License verze 3 (GPL-3.0)**.

Plné znění naleznete v souboru `LICENSE` v kořenovém adresáři tohoto úložiště nebo na webu GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Poděkování

Tento projekt využívá mody třetích stran jako referenční texty pro překlad cílového jazyka. Referenční texty jsou odesílány LLM pro překlad.

| Název referenčního modu | Autor | Stránka modu |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Stránka Workshopu](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Stránka Workshopu](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Stránka Workshopu](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Všem výše uvedeným autorům srdečně děkujeme!**

---

## Programy třetích stran

Tento projekt využívá programy a knihovny třetích stran, jejichž autorská práva náleží příslušným vývojářům.

