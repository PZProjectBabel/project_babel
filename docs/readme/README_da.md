# Project Babel — 《僵尸毁灭工程》 mod LLM automatisk oversættelsesprojekt

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Dette oversættelsesprojekt er drevet og vedligeholdt af [Project Babel](https://github.com/PZProjectBabel/project_babel) værktøjssættet.*

---

## Indholdsfortegnelse

- [Understøttede målsprog for projektet](#understøttede-målsprog-for-projektet)
- [Sådan installeres og bruges](#sådan-installeres-og-bruges)
- [Oversættelsesfremskridt](#oversættelsesfremskridt)
- [Sådan bidrager du](#sådan-bidrager-du)
- [Værktøjer og mappestruktur (til udviklere)](#værktøjer-og-mappestruktur-til-udviklere)
  - [Projektmapper](#projektmapper)
  - [Pipeline-moduler (i udførelsesrækkefølge)](#pipeline-moduler-i-udførelsesrækkefølge)
  - [Teknologistak](#teknologistak)
- [Ophavsret og licens](#ophavsret-og-licens)
  - [1. Tekst og billeder mv.](#1-tekst-og-billeder-mv)
  - [2. Programmer, scripts og andet udviklingsindhold](#2-programmer-scripts-og-andet-udviklingsindhold)
- [Tak](#tak)
- [Tredjepartsprogrammer](#tredjepartsprogrammer)

---

## Understøttede målsprog for projektet

| Sprog | Lokalt navn | International kode | In-game kode | Understøttet | Bemærkninger |
|------|------|------|------|------|------|
| Arabisk | العربية | `ar` | `AR` | ❌ | Ikke nok token-kvote |
| Katalansk | català | `ca` | `CA` | ❌ | Ikke nok token-kvote |
| Traditionelt kinesisk | 繁體中文 | `zh-hant` | `CH` | ❌ | Ikke nok token-kvote |
| Simplificeret kinesisk | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tjekkisk | čeština | `cs` | `CS` | ❌ | Ikke nok token-kvote |
| Dansk | dansk | `da` | `DA` | ❌ | Ikke nok token-kvote |
| Tysk | Deutsch | `de` | `DE` | ✅ | |
| Engelsk | English | `en` | `EN` | ✅ | |
| Spansk | español | `es` | `ES` | ❌ | Ikke nok token-kvote |
| Finsk | suomi | `fi` | `FI` | ❌ | Ikke nok token-kvote |
| Fransk | français | `fr` | `FR` | ✅ | |
| Ungarsk | magyar | `hu` | `HU` | ❌ | Ikke nok token-kvote |
| Indonesisk | Bahasa Indonesia | `id` | `ID` | ❌ | Ikke nok token-kvote |
| Italiensk | italiano | `it` | `IT` | ❌ | Ikke nok token-kvote |
| Japansk | 日本語 | `ja` | `JP` | ✅ | |
| Koreansk | 한국어 | `ko` | `KO` | ❌ | Ikke nok token-kvote |
| Hollandsk | Nederlands | `nl` | `NL` | ❌ | Ikke nok token-kvote |
| Norsk | norsk | `no` | `NO` | ❌ | Ikke nok token-kvote |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Ikke nok token-kvote |
| Polsk | polski | `pl` | `PL` | ❌ | Ikke nok token-kvote |
| Portugisisk (Portugal) | português | `pt` | `PT` | ❌ | Ikke nok token-kvote |
| Portugisisk (Brasilien) | português do Brasil | `pt-br` | `PTBR` | ❌ | Ikke nok token-kvote |
| Rumænsk | română | `ro` | `RO` | ❌ | Ikke nok token-kvote |
| Russisk | русский | `ru` | `RU` | ❌ | Ikke nok token-kvote |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Ikke nok token-kvote |
| Tyrkisk | Tyrkisk | `tr` | `TR` | ❌ | Utilstrækkelig token-kvote |
| Ukrainsk | українська | `uk` | `UA` | ❌ | Utilstrækkelig token-kvote |

**I alt**: 27 planlagte sprog | **Understøttet**: 5 | **Afventer**: 22

---

## Sådan installeres og bruges

Dette er en guide til spillere, der ønsker at bruge dette oversættelsesprojekt direkte i spillet.

1. Gå til vores Steam Workshop-side: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Klik på "Abonner"-knappen.
3. Start spillet, og aktivér dette oversættelsesmodul i spillets hovedmenu under "Mods"-administration.
4. Oversættelsesteksten fra senere aktiverede moduler overskriver tidligere aktiverede moduler, så dette oversættelsesmodul skal aktiveres efter funktionsmodulerne (placer det så langt nede som muligt).
5. Nyd spillet!

---

## Oversættelsesfremskridt

**[➡️ Klik her for at se oversættelsesfremskridt](./docs/progress/progress_da.md)**

---

## Sådan bidrager du

Vi byder alle velkommen til at bidrage, uanset om det er at rette en fejl, tilføje en funktion, skrive prompt-skabeloner eller give referenceoversættelser!

At kalde LLM API for oversættelse kræver betaling for tokens. For at projektet kan fungere stabilt på lang sigt, håber vi på din generøse støtte!

Læs venligst [Bidragsguide](./docs/contributing/contributing_da.md) for detaljer.

---

## Værktøjer og mappestruktur (til udviklere)

Dette afsnit er for udviklere, der ønsker at forstå projektets automatiseringsprincipper.

### Projektmapper

| Mappe | Beskrivelse |
|------|------|
| `src/` | .NET 10 oversættelsespipeline kildekode med 15 moduler |
| `config/` | Pipeline-konfigurationsfiler (LLM-, Steam-, RAG-parametre osv.) |
| `data/` | Runtime-data: Modulmetadata, embedding, oversættelsescache |
| `translation_ref/` | Referenceoversættelsesdata (f.eks. godkendte moduler fra en oversættelsesgruppe), giver LLM oversættelsesreference |
| `base_game_keys/` | Spillets oversættelsesnøgler, bruges til deduplikering for at forhindre overskrivning af original tekst |
| `final_outputs/` | Endelig output: `project_babel/` modulpakke, `icons/` ikoner og `workshop_descriptions/` Workshop-beskrivelser |
| `docs/` | Projekt dokumentation: Fremskridtsrapport, bidragsguide, pipeline-beskrivelse |
| `temp/` | Pipeline midlertidige filer (separat mappe for hver kørsel) |
| `src/prompt_templates/` | LLM prompt-skabeloner (oversættelse/indholdsgennemgang) |

### Pipeline-moduler (i udførelsesrækkefølge)

| Trin | Modul | Funktion |
|------|------|------|
| 1 | `ConfigReader` | Indlæser konfiguration/nøgler/sprogliste |
| 2 | `RepoDataLoader` | Indlæser referenceoversættelser og oversættelsescache |
| 3 | `ModIdCollector` | Indsamler Workshop mod-ID'er |
| 4 | `ModInfoFetcher` | Henter Steam metadata |
| 5 | `SteamCmdBootstrapper` | Forbereder steamcmd runtime til den nuværende platform |
| 6 | `ModDownloader` | Downloader mods via steamcmd |
| 7 | `ContentExtractor` | Fortolker mod-oversættelsesfiler → `TranslationEntry` |
| 8 | `ContentChecker` | Sikkerhedsgennemgang af indhold (narkotika/porno/vold) |
| 9 | `EmbeddingFetcher` | Beregner tekst-embedding-vektorer |
| 10 | `TranslationBatcher` | Opretter oversættelsesbatcher uafhængigt af målsprog |
| 11 | `RagContextRetriever` | Henter RAG-kontekst (præcis nøgle + embedding-lighed) |
| 12 | `LLMTranslator` | Kalder LLM for at udføre oversættelse |
| 13 | `ResultWriter` | Skriver til data/ og translation_ref/ |
| 14 | `FinalOutputWriter` | Genererer endelig PZ-modformatoutput |
| 15 | `ProgressReporter` | Genererer fremskridtsrapport |

### Teknologistak

- **Sprog**: C# (.NET 10)
- **Målplatform**: GitHub Actions Linux x64 runner
- **Test**: xUnit (Windows x64)
- **LLM**: DeepSeek API (kan konfigureres)
- **Embedding**: Tekstvektorisering til RAG-lighedssøgning
- **Indholdsgennemgang**: LLM-drevet flerniveau sikkerhedsrevision

Detaljeret [teknisk reference](./docs/technical_reference/technical_reference_da.md).

---

## Ophavsret og licens

Oversættelsestekstindholdet og relaterede billeder i dette oversættelsesprojekt er skabt eller sekundært skabt af **Project Babel** og deltagerne baseret på de originale spilmods.

© 2025 Project Babel og forfattere. Alle rettigheder forbeholdes.

### 1. Tekst og billeder mv.

Medmindre andet er angivet, i dette depot:

- Indhold af oversættelse, sproglig forbedring og korrekturlæsning af spiltekst;
- Projektets dokumentation, tekstoversættelser i moduler;
- Billeder og kunstressourcer specifikt lavet til dette projekt

alle er licenseret under **Navngivelse-IkkeKommerciel-DelPåSammeVilkår 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, forkortet **CC BY-NC-SA 4.0**) licensen.

Det betyder, at under følgende betingelser kan du frit dele og tilpasse dette indhold:

- **Navngivelse (BY)**: Angiv på en synlig plads, at "dette oversættelsesprojekt er baseret på arbejdet fra 'Project Babel' og er blevet ændret", og vedlæg link til dette lager og Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Ikke-kommerciel brug (NC)**: Må ikke bruge dette projekts indhold eller afledte værker til nogen direkte eller indirekte kommerciel anvendelse (herunder, men ikke begrænset til, betalte pakker, betalt download, reklameindtægter osv.);
- **Del på samme vilkår (SA)**: Hvis du ændrer eller bearbejder dette projekts indhold, skal du offentliggøre din ændrede version under **samme CC BY-NC-SA 4.0-licens**.

For mere information om denne licens, se:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.da>

*Særlig bemærkning:*
- *Indholdet i `base_game_keys`-mappen stammer fra spillet, ophavsret tilhører spiludvikleren! Indholdet bruges til at forhindre, at oversættelsesnøgler overskriver spillets nøgler (deduplikering)*
- *Indholdet i `translation_ref`-mappen bruges til at give LLM'en oversættelsesreference, ophavsret tilhører de respektive mod-udviklere!*

### 2. Programmer, scripts og andet udviklingsindhold

Medmindre andet er angivet i kildefilerne eller mapperne, er programkoden (f.eks. i `src/`-mappen) i dette lager, som bruges til at lave/pakke/behandle lokaliseringsindhold, licenseret under **GNU General Public License version 3 (GPL-3.0)**.

Se den fulde licens i `LICENSE`-filen i roden af dette lager (GPL-3.0), eller besøg GNU's hjemmeside: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Tak

Dette projekt bruger tredjepartsmods som referencetekster til oversættelse til målsproget. Referenceteksterne sendes til LLM'en til oversættelsesreference.

| Referencemodnavn | Forfatter | Modside |
|------|------|------|
| [B42] Samlet kinesisk oversættelse | Ruyi Oversættelsesgruppe (As1) | [Steam Workshop-side](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Samlet mod-oversættelse | Ruyi Oversættelsesgruppe (As1) | [Steam Workshop-side](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Samlet Ark-oversættelse | Ruyi Oversættelsesgruppe (As1) | [Steam Workshop-side](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**En oprigtig tak til ovenstående forfattere!**

---

## Tredjepartsprogrammer

Dette projekt bruger tredjepartsprogrammer og -biblioteker, hvis ophavsret tilhører de respektive udviklere.

