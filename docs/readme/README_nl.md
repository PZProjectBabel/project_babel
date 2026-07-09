# Project Babel — Automatische LLM-vertaling van PZ-mods

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Opmerking:** Deze vertaling wordt nog niet ondersteund. De gezaghebbende inhoud is de [Chinese versie](../../README.md).

---

*Dit vertaalproject wordt aangedreven en onderhouden door de [Project Babel](https://github.com/PZProjectBabel/project_babel) toolset.*

---

## Inhoudsopgave

- [Ondersteunde doeltalen](#ondersteunde-doeltalen)
- [Installeren & gebruiken](#installeren--gebruiken)
- [Vertaalvoortgang](#vertaalvoortgang)
- [Bijdragen](#bijdragen)
- [Tools & mappenstructuur (voor ontwikkelaars)](#tools--mappenstructuur-(voor-ontwikkelaars))
- [Auteursrecht & licentie](#auteursrecht--licentie)
- [Dankbetuigingen](#dankbetuigingen)
- [Software van derden](#software-van-derden)

---

## Ondersteunde doeltalen

| Taal | Lokale naam | ISO-code | In-game code | Ondersteund | Opmerking |
|------|------|------|------|------|------|
| Arabisch | العربية | `ar` | `AR` | ❌ | Onvoldoende token-tegoed |
| Catalaans | català | `ca` | `CA` | ❌ | Onvoldoende token-tegoed |
| Traditioneel Chinees | 繁體中文 | `zh-hant` | `CH` | ❌ | Onvoldoende token-tegoed |
| Vereenvoudigd Chinees | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tsjechisch | čeština | `cs` | `CS` | ❌ | Onvoldoende token-tegoed |
| Deens | dansk | `da` | `DA` | ❌ | Onvoldoende token-tegoed |
| Duits | Deutsch | `de` | `DE` | ✅ | |
| Engels | English | `en` | `EN` | ✅ | |
| Spaans | español | `es` | `ES` | ❌ | Onvoldoende token-tegoed |
| Fins | suomi | `fi` | `FI` | ❌ | Onvoldoende token-tegoed |
| Frans | français | `fr` | `FR` | ✅ | |
| Hongaars | magyar | `hu` | `HU` | ❌ | Onvoldoende token-tegoed |
| Indonesisch | Bahasa Indonesia | `id` | `ID` | ❌ | Onvoldoende token-tegoed |
| Italiaans | italiano | `it` | `IT` | ❌ | Onvoldoende token-tegoed |
| Japans | 日本語 | `ja` | `JP` | ✅ | |
| Koreaans | 한국어 | `ko` | `KO` | ❌ | Onvoldoende token-tegoed |
| Nederlands | Nederlands | `nl` | `NL` | ❌ | Onvoldoende token-tegoed |
| Noors | norsk | `no` | `NO` | ❌ | Onvoldoende token-tegoed |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Onvoldoende token-tegoed |
| Pools | polski | `pl` | `PL` | ❌ | Onvoldoende token-tegoed |
| Portugees (Portugal) | português | `pt` | `PT` | ❌ | Onvoldoende token-tegoed |
| Portugees (Brazilië) | português do Brasil | `pt-br` | `PTBR` | ❌ | Onvoldoende token-tegoed |
| Roemeens | română | `ro` | `RO` | ❌ | Onvoldoende token-tegoed |
| Russisch | русский | `ru` | `RU` | ❌ | Onvoldoende token-tegoed |
| Thais | ภาษาไทย | `th` | `TH` | ❌ | Onvoldoende token-tegoed |
| Turks | Türkçe | `tr` | `TR` | ❌ | Onvoldoende token-tegoed |
| Oekraïens | українська | `uk` | `UA` | ❌ | Onvoldoende token-tegoed |

**Totaal**: 27 geplande talen | **Ondersteund**: 5 | **In afwachting**: 22

---

## Installeren & gebruiken

Een handleiding voor spelers die het vertaalpakket in de game willen gebruiken.

1. Ga naar de Steam Workshop-pagina: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Klik op "Abonneren".
3. Start het spel, schakel deze vertaalmod in het Mods-menu in.
4. Vertaaltekst van later geladen mods overschrijft eerdere, dus deze vertaalmod moet na gameplay-mods worden geladen.
5. Veel plezier!

---

## Vertaalvoortgang

[➡️ Vertaalvoortgang](../progress/progress_nl.md)

---

## Bijdragen

We verwelkomen bijdragen! Vertaalcorrecties, nieuwe functies, promptsjablonen of referentievertalingen.

LLM API-aanroepen voor vertaling brengen tokenkosten met zich mee. Uw steun helpt het project duurzaam te draaien!

Read the [Contributing Guide](../contributing/contributing_nl.md) for details.

---

## Tools & mappenstructuur (voor ontwikkelaars)

Deze sectie is bedoeld voor ontwikkelaars die de interne automatisering van het project willen begrijpen.

### Projectmappen

| Map | Beschrijving |
|------|------|
| `src/` | .NET 10 vertaalpipeline broncode, 15 modules |
| `config/` | Pipelineconfiguratie (LLM, Steam, RAG-parameters, enz.) |
| `data/` | Runtimegegevens: mod-metadata, embeddings, vertaalcache |
| `translation_ref/` | Referentievertalingen als LLM-context |
| `base_game_keys/` | Basisspel vertaalsleutels voor deduplicatie |
| `final_outputs/` | Definitieve PZ mod-formaat vertaaluitvoer |
| `docs/` | Documentatie: voortgang, bijdragen, pipelinespecificaties |
| `temp/` | Tijdelijke pipelinebestanden |
| `src/prompt_templates/` | LLM promptsjablonen |

### Pipelinemodules (uitvoeringsvolgorde)

| Stap | Module | Functie |
|------|------|------|
| 1 | `ConfigReader` | Configuratie/geheimen/talen laden |
| 2 | `RepoDataLoader` | Referenties en vertaalcache laden |
| 3 | `ModIdCollector` | Workshop mod-ID's verzamelen |
| 4 | `ModInfoFetcher` | Steam-metadata ophalen |
| 5 | `ModDownloader` | Mods downloaden via steamcmd |
| 6 | `ContentExtractor` | Modvertaalbestanden parsen → `TranslationEntry` |
| 7 | `ContentChecker` | Inhoudsveiligheidscontrole |
| 8 | `EmbeddingFetcher` | Tekstembeddingvectoren berekenen |
| 9 | `TranslationBatcher` | Vertaalbatches maken |
| 10 | `RagContextRetriever` | RAG-contexten ophalen |
| 11 | `LLMTranslator` | LLM-vertaling uitvoeren |
| 12 | `ResultWriter` | Schrijven naar data/ en translation_ref/ |
| 13 | `FinalOutputWriter` | Definitieve PZ mod-uitvoer genereren |
| 14 | `ProgressReporter` | Voortgangsrapporten genereren |

### Technologiestack

- **Taal**: C# (.NET 10)
- **Doelplatform**: GitHub Actions Linux x64 runner
- **Tests**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configureerbaar)
- **Embedding**: Tekstvectorisatie voor RAG-gelijkeniszoekopdracht
- **Inhoudscontrole**: LLM-gestuurde meerlaagse veiligheidscontrole

Gedetailleerde technische documentatie: [TranslationEntry-pijplijn](../pipeline/translation_entry_pipeline_nl.md)

---

## Auteursrecht & licentie

© 2025 Project Babel en alle auteurs. Alle rechten voorbehouden.

### Inhoud (teksten, afbeeldingen)

Gelicentieerd onder **CC BY-NC-SA 4.0**.

- **Naamsvermelding**: Vermeld wijzigingen gebaseerd op "Project Babel", met repo- & Workshop-links
- **Niet-commercieel**: Commercieel gebruik verboden
- **Gelijk delen**: Wijzigingen moeten onder dezelfde licentie worden gepubliceerd

### Code

Code onder `src/` is gelicentieerd onder **GPL-3.0**.

---

## Dankbetuigingen

| Referentiemod | Auteur | Pagina |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Opvallend dank aan bovenstaande auteurs!**

---

## Software van derden

Dit project gebruikt programma's en bibliotheken van derden, waarvan de auteursrechten toebehoren aan de respectievelijke ontwikkelaars.
