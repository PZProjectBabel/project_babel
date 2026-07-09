# Project Babel — Automatisk LLM-oversettelse av PZ-mods

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Merk:** Denne oversettelsen støttes ennå ikke. Det autoritative innholdet er den [kinesiske versjonen](../../README.md).

---

*Dette oversettelsesprosjektet drives og vedlikeholdes av [Project Babel](https://github.com/PZProjectBabel/project_babel)-verktøyet.*

---

## Innholdsfortegnelse

- [Støttede målspråk](#støttede-målspråk)
- [Installasjon og bruk](#installasjon-og-bruk)
- [Oversettelsesfremgang](#oversettelsesfremgang)
- [Bidra](#bidra)
- [Verktøy og mappestruktur (for utviklere)](#verktøy-og-mappestruktur-(for-utviklere))
- [Opphavsrett og lisens](#opphavsrett-og-lisens)
- [Anerkjennelser](#anerkjennelser)
- [Tredjepartsprogramvare](#tredjepartsprogramvare)

---

## Støttede målspråk

| Språk | Lokalt navn | ISO-kode | Spillkode | Støttet | Merknad |
|------|------|------|------|------|------|
| Arabisk | العربية | `ar` | `AR` | ❌ | Manglende token-kreditter |
| Katalansk | català | `ca` | `CA` | ❌ | Manglende token-kreditter |
| Tradisjonell kinesisk | 繁體中文 | `zh-hant` | `CH` | ❌ | Manglende token-kreditter |
| Forenklet kinesisk | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tsjekkisk | čeština | `cs` | `CS` | ❌ | Manglende token-kreditter |
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
| Portugisisk (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Manglende token-kreditter |
| Rumensk | română | `ro` | `RO` | ❌ | Manglende token-kreditter |
| Russisk | русский | `ru` | `RU` | ❌ | Manglende token-kreditter |
| Thailandsk | ภาษาไทย | `th` | `TH` | ❌ | Manglende token-kreditter |
| Tyrkisk | Türkçe | `tr` | `TR` | ❌ | Manglende token-kreditter |
| Ukrainsk | українська | `uk` | `UA` | ❌ | Manglende token-kreditter |

**Totalt**: 27 planlagte språk | **Støttet**: 5 | **Venter**: 22

---

## Installasjon og bruk

En guide for spillere som vil bruke oversettelsespakken i spillet.

1. Gå til Steam Workshop-siden: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Klikk på "Abonner".
3. Start spillet, og aktiver denne oversettelsesmodden i Mods-menyen.
4. Oversettelsestekst fra senere innlastede modder overstyrer tidligere, så denne oversettelsesmodden må lastes etter spillmodder.
5. Nyt!

---

## Oversettelsesfremgang

[➡️ Oversettelsesfremgang](../progress/progress_no.md)

---

## Bidra

Vi tar gjerne imot bidrag! Oversettelsesrettelser, nye funksjoner, promptmaler eller referanseoversettelser.

LLM API-kall for oversettelse medfører tokenkostnader. Din støtte hjelper prosjektet med å kjøre bærekraftig!

Read the [Contributing Guide](../contributing/contributing_no.md) for details.

---

## Verktøy og mappestruktur (for utviklere)

Denne delen er rettet mot utviklere som ønsker å forstå prosjektets interne automatisering.

### Prosjektmapper

| Mappe | Beskrivelse |
|------|------|
| `src/` | .NET 10 oversettelsespipeline-kildekode, 15 moduler |
| `config/` | Pipeline-konfigurasjon (LLM, Steam, RAG-parametere osv.) |
| `data/` | Kjøretidsdata: mod-metadata, embeddings, oversettelsescache |
| `translation_ref/` | Referanseoversettelser som LLM-kontekst |
| `base_game_keys/` | Grunnspillets oversettelsesnøkler for deduplisering |
| `final_outputs/` | Endelig PZ mod-format utdata |
| `docs/` | Dokumentasjon: fremgang, bidrag, pipeline-spesifikasjoner |
| `temp/` | Midlertidige pipeline-filer |
| `src/prompt_templates/` | LLM prompt-maler |

### Pipelinemoduler (utførelsesrekkefølge)

| Trinn | Modul | Funksjon |
|------|------|------|
| 1 | `ConfigReader` | Last konfigurasjon/hemmeligheter/språk |
| 2 | `RepoDataLoader` | Last referanser og oversettelsescache |
| 3 | `ModIdCollector` | Samle Workshop mod-IDer |
| 4 | `ModInfoFetcher` | Hent Steam metadata |
| 5 | `ModDownloader` | Last ned mods via steamcmd |
| 6 | `ContentExtractor` | Analyser mod-oversettelsesfiler → `TranslationEntry` |
| 7 | `ContentChecker` | Gjennomgang av innholdssikkerhet |
| 8 | `EmbeddingFetcher` | Beregn tekst-embedding vektorer |
| 9 | `TranslationBatcher` | Opprett oversettelsesbatcher |
| 10 | `RagContextRetriever` | Hent RAG-kontekster |
| 11 | `LLMTranslator` | Utfør LLM-oversettelse |
| 12 | `ResultWriter` | Skriv til data/ og translation_ref/ |
| 13 | `FinalOutputWriter` | Generer endelig PZ mod-utdata |
| 14 | `ProgressReporter` | Generer fremdriftsrapporter |

### Teknologistakk

- **Språk**: C# (.NET 10)
- **Målplattform**: GitHub Actions Linux x64 runner
- **Tester**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurerbar)
- **Embedding**: Tekstvektorisering for RAG-likhetssøk
- **Innholdsgjennomgang**: LLM-drevet flernivå sikkerhetsgjennomgang

Detaljert teknisk dokumentasjon: [TranslationEntry-pipeline](../pipeline/translation_entry_pipeline_no.md)

---

## Opphavsrett og lisens

© 2025 Project Babel og alle forfattere. Alle rettigheter forbeholdt.

### Innhold (tekster, bilder)

Lisensiert under **CC BY-NC-SA 4.0**.

- **Navngivelse**: Oppgi endringer basert på "Project Babel", med repo- og Workshop-lenker
- **Ikke-kommersiell**: Kommersiell bruk forbudt
- **Del på samme vilkår**: Endringer må publiseres under samme lisens

### Kode

Kode under `src/` er lisensiert under **GPL-3.0**.

---

## Anerkjennelser

| Referansemod | Forfatter | Side |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Dyp takknemlighet til forfatterne ovenfor!**

---

## Tredjepartsprogramvare

Dette prosjektet bruker tredjepartsprogrammer og -biblioteker, opphavsretten tilhører de respektive utviklerne.
