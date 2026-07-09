# Project Babel — Automatisk LLM-oversættelse af PZ-mods

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Bemærk:** Denne oversættelse er endnu ikke understøttet. Det autoritative indhold er den [kinesiske version](../../README.md).

---

*Dette oversættelsesprojekt drives og vedligeholdes af [Project Babel](https://github.com/PZProjectBabel/project_babel)-værktøjet.*

---

## Indholdsfortegnelse

- [Understøttede målsprog](#understøttede-målsprog)
- [Sådan installeres og bruges](#sådan-installeres-og-bruges)
- [Oversættelsesfremskridt](#oversættelsesfremskridt)
- [Bidrag](#bidrag)
- [Værktøjer og mappestruktur (for udviklere)](#værktøjer-og-mappestruktur-(for-udviklere))
- [Ophavsret og licens](#ophavsret-og-licens)
- [Anerkendelser](#anerkendelser)
- [Tredjepartssoftware](#tredjepartssoftware)

---

## Understøttede målsprog

| Sprog | Lokalt navn | ISO-kode | Kode i spillet | Understøttet | Bemærkning |
|------|------|------|------|------|------|
| Arabisk | العربية | `ar` | `AR` | ❌ | Manglende token-kreditter |
| Catalansk | català | `ca` | `CA` | ❌ | Manglende token-kreditter |
| Traditionelt kinesisk | 繁體中文 | `zh-hant` | `CH` | ❌ | Manglende token-kreditter |
| Forenklet kinesisk | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tjekkisk | čeština | `cs` | `CS` | ❌ | Manglende token-kreditter |
| Dansk | dansk | `da` | `DA` | ❌ | Manglende token-kreditter |
| Tysk | Deutsch | `de` | `DE` | ✅ | |
| Engelsk | English | `en` | `EN` | ✅ | |
| Spansk | español | `es` | `ES` | ❌ | Manglende token-kreditter |
| Finsk | suomi | `fi` | `FI` | ❌ | Manglende token-kreditter |
| Fransk | français | `fr` | `FR` | ✅ | |
| Ungarsk | magyar | `hu` | `HU` | ❌ | Manglende token-kreditter |
| Indonesisk | Bahasa Indonesia | `id` | `ID` | ❌ | Manglende token-kreditter |
| Italiensk | italiano | `it` | `IT` | ❌ | Manglende token-kreditter |
| Japansk | 日本語 | `ja` | `JP` | ✅ | |
| Koreansk | 한국어 | `ko` | `KO` | ❌ | Manglende token-kreditter |
| Nederlandsk | Nederlands | `nl` | `NL` | ❌ | Manglende token-kreditter |
| Norsk | norsk | `no` | `NO` | ❌ | Manglende token-kreditter |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Manglende token-kreditter |
| Polsk | polski | `pl` | `PL` | ❌ | Manglende token-kreditter |
| Portugisisk (Portugal) | português | `pt` | `PT` | ❌ | Manglende token-kreditter |
| Portugisisk (Brasilien) | português do Brasil | `pt-br` | `PTBR` | ❌ | Manglende token-kreditter |
| Rumænsk | română | `ro` | `RO` | ❌ | Manglende token-kreditter |
| Russisk | русский | `ru` | `RU` | ❌ | Manglende token-kreditter |
| Thailandsk | ภาษาไทย | `th` | `TH` | ❌ | Manglende token-kreditter |
| Tyrkisk | Türkçe | `tr` | `TR` | ❌ | Manglende token-kreditter |
| Ukrainsk | українська | `uk` | `UA` | ❌ | Manglende token-kreditter |

**I alt**: 27 planlagte sprog | **Understøttet**: 5 | **Afventer**: 22

---

## Sådan installeres og bruges

En guide til spillere, der vil bruge oversættelsespakken i spillet.

1. Gå til Steam Workshop-siden: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Klik på "Abonner".
3. Start spillet, og aktiver dette oversættelsesmod i menuen Mods.
4. Oversættelsestekst fra senere indlæste mods overskriver tidligere, så dette oversættelsesmod skal indlæses efter spilmods.
5. God fornøjelse!

---

## Oversættelsesfremskridt

[➡️ Oversættelsesfremskridt](../progress/progress_da.md)

---

## Bidrag

Vi byder bidrag velkommen! Oversættelsesrettelser, nye funktioner, promptskabeloner eller referenceoversættelser.

LLM API-kald til oversættelse medfører token-omkostninger. Din støtte hjælper projektet med at køre bæredygtigt!

Read the [Contributing Guide](../contributing/contributing_da.md) for details.

---

## Værktøjer og mappestruktur (for udviklere)

Dette afsnit er rettet mod udviklere, der ønsker at forstå projektets automatisering internt.

### Projektmapper

| Mappe | Beskrivelse |
|------|------|
| `src/` | .NET 10 oversættelsespipeline-kildekode, 15 moduler |
| `config/` | Pipeline-konfiguration (LLM, Steam, RAG-parametre osv.) |
| `data/` | Kørselsdata: mod-metadata, embeddings, oversættelsescache |
| `translation_ref/` | Referenceoversættelser som LLM-kontekst |
| `base_game_keys/` | Grundspils oversættelsesnøgler til deduplikering |
| `final_outputs/` | Endeligt PZ mod-format output |
| `docs/` | Dokumentation: fremskridt, bidrag, pipeline-specifikationer |
| `temp/` | Midlertidige pipeline-filer |
| `src/prompt_templates/` | LLM prompt-skabeloner |

### Pipeline-moduler (udførelsesrækkefølge)

| Trin | Modul | Funktion |
|------|------|------|
| 1 | `ConfigReader` | Indlæs konfiguration/hemmeligheder/sprog |
| 2 | `RepoDataLoader` | Indlæs referencer og oversættelsescache |
| 3 | `ModIdCollector` | Indsaml Workshop mod-ID'er |
| 4 | `ModInfoFetcher` | Hent Steam metadata |
| 5 | `ModDownloader` | Download mods via steamcmd |
| 6 | `ContentExtractor` | Analysér mod-oversættelsesfiler → `TranslationEntry` |
| 7 | `ContentChecker` | Gennemgang af indholdssikkerhed |
| 8 | `EmbeddingFetcher` | Beregn tekst-embedding vektorer |
| 9 | `TranslationBatcher` | Opret oversættelsesbatches |
| 10 | `RagContextRetriever` | Hent RAG-kontekster |
| 11 | `LLMTranslator` | Udfør LLM-oversættelse |
| 12 | `ResultWriter` | Skriv til data/ og translation_ref/ |
| 13 | `FinalOutputWriter` | Generér endeligt PZ mod-output |
| 14 | `ProgressReporter` | Generér fremskridtsrapporter |

### Teknologistak

- **Sprog**: C# (.NET 10)
- **Målplatform**: GitHub Actions Linux x64 runner
- **Tests**: xUnit (Windows x64)
- **LLM**: DeepSeek API (kan konfigureres)
- **Embedding**: Tekstvektorisering til RAG-lighedssøgning
- **Indholdsgennemgang**: LLM-drevet flerniveaus sikkerhedsgennemgang

Detaljeret teknisk dokumentation: [TranslationEntry pipeline](../pipeline/translation_entry_pipeline_da.md)

---

## Ophavsret og licens

© 2025 Project Babel og alle forfattere. Alle rettigheder forbeholdes.

### Indhold (tekster, billeder)

Licenseret under **CC BY-NC-SA 4.0**.

- **Kreditering**: Angiv ændringer baseret på "Project Babel", med repo- og Workshop-links
- **Ikke-kommerciel**: Kommerciel brug forbudt
- **Del på samme vilkår**: Ændringer skal offentliggøres under samme licens

### Kode

Kode i `src/` er licenseret under **GPL-3.0**.

---

## Anerkendelser

| Referencemod | Forfatter | Side |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Dybtfølt tak til ovenstående forfattere!**

---

## Tredjepartssoftware

Dette projekt bruger tredjepartsprogrammer og -biblioteker, ophavsretten tilhører de respektive udviklere.
