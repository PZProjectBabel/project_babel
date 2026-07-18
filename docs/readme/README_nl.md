# Project Babel — LLM automatisch vertaalproject voor de mod 'Project Zomboid'

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Dit vertaalproject wordt aangedreven en onderhouden door de [Project Babel](https://github.com/PZProjectBabel/project_babel) toolset.*

---

## Inhoudsopgave

- [Ondersteunde doeltalen van het project](#ondersteunde-doeltalen-van-het-project)
- [Hoe te installeren en gebruiken](#hoe-te-installeren-en-gebruiken)
- [Vertaalvoortgang](#vertaalvoortgang)
- [Hoe bij te dragen](#hoe-bij-te-dragen)
- [Hulpmiddelen en mapstructuur (voor ontwikkelaars)](#hulpmiddelen-en-mapstructuur-voor-ontwikkelaars)
  - [Projectmappen](#projectmappen)
  - [Pijplijnmodules (in uitvoeringsvolgorde)](#pijplijnmodules-in-uitvoeringsvolgorde)
  - [Onafhankelijke modules](#onafhankelijke-modules)
  - [Technologiestack](#technologiestack)
- [Auteursrecht en Licentie](#auteursrecht-en-licentie)
  - [1. Tekst en afbeeldingen, etc.](#1-tekst-en-afbeeldingen-etc)
  - [2. Programma's, scripts en andere ontwikkelingsinhoud](#2-programmas-scripts-en-andere-ontwikkelingsinhoud)
- [Dankbetuigingen](#dankbetuigingen)
- [Programma's van derden](#programmas-van-derden)

---

## Ondersteunde doeltalen van het project

| Taal | Lokale naam | Internationale code | In-game code | Ondersteund? | Opmerkingen |
|------|------|------|------|------|------|
| Arabisch | العربية | `ar` | `AR` | ❌ | Onvoldoende tokenquotum |
| Catalaans | català | `ca` | `CA` | ❌ | Onvoldoende tokenquotum |
| Traditioneel Chinees | 繁體中文 | `zh-hant` | `CH` | ❌ | Onvoldoende tokenquotum |
| Vereenvoudigd Chinees | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tsjechisch | čeština | `cs` | `CS` | ❌ | Onvoldoende tokenquotum |
| Deens | dansk | `da` | `DA` | ❌ | Onvoldoende tokenquotum |
| Duits | Deutsch | `de` | `DE` | ✅ | |
| Engels | English | `en` | `EN` | ✅ | |
| Spaans | español | `es` | `ES` | ❌ | Onvoldoende tokenquotum |
| Fins | suomi | `fi` | `FI` | ❌ | Onvoldoende tokenquotum |
| Frans | français | `fr` | `FR` | ✅ | |
| Hongaars | magyar | `hu` | `HU` | ❌ | Onvoldoende tokenquotum |
| Indonesisch | Bahasa Indonesia | `id` | `ID` | ❌ | Onvoldoende tokenquotum |
| Italiaans | italiano | `it` | `IT` | ❌ | Onvoldoende tokenquotum |
| Japans | 日本語 | `ja` | `JP` | ✅ | |
| Koreaans | 한국어 | `ko` | `KO` | ❌ | Onvoldoende tokenquotum |
| Nederlands | Nederlands | `nl` | `NL` | ❌ | Onvoldoende tokenquotum |
| Noors | norsk | `no` | `NO` | ❌ | Onvoldoende tokenquotum |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Onvoldoende tokenquotum |
| Pools | polski | `pl` | `PL` | ❌ | Onvoldoende tokenquotum |
| Portugees (Portugal) | português | `pt` | `PT` | ❌ | Onvoldoende tokenquotum |
| Portugees (Brazilië) | português do Brasil | `pt-br` | `PTBR` | ❌ | Onvoldoende tokenquotum |
| Roemeens | română | `ro` | `RO` | ❌ | Onvoldoende tokenquotum |
| Russisch | русский | `ru` | `RU` | ❌ | Onvoldoende tokenquotum |
| Thais | ภาษาไทย | `th` | `TH` | ❌ | Onvoldoende tokenquotum |
| Turks | Türkçe | `tr` | `TR` | ❌ | Onvoldoende tokens |
| Oekraïens | українська | `uk` | `UA` | ❌ | Onvoldoende tokens |

**Totaal**: 27 geplande talen | **Ondersteund**: 5 | **Nog te ondersteunen**: 22

---

## Hoe te installeren en gebruiken

Dit is een gids voor spelers die dit vertaalproject direct in het spel willen gebruiken.

1.  Ga naar onze Steam Workshop-pagina: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Klik op de "Abonneren" knop.
3.  Start het spel en schakel deze vertaalmod in via het "Mods" beheer in het hoofdmenu.
4.  Later ingeschakelde mods overschrijven eerdere mods, dus deze vertaalmod moet na functionele mods worden ingeschakeld (zo laag mogelijk).
5.  Geniet van het spel!

---

## Vertaalvoortgang

**[➡️ Klik hier om de vertaalvoortgang te bekijken](./docs/progress/progress_nl.md)**

---

## Hoe bij te dragen

We verwelkomen iedereen om bij te dragen, of het nu gaat om het verbeteren van een fout, het toevoegen van een functie, het schrijven van prompt-sjablonen of het leveren van referentievertalingen!

Het aanroepen van de LLM API voor vertaling vereist betaling voor tokens. Om het project op lange termijn stabiel te laten draaien, hopen we op uw vrijgevigheid!

Lees de [Bijdragengids](./docs/contributing/contributing_nl.md) voor meer details.

---

## Hulpmiddelen en mapstructuur (voor ontwikkelaars)

Deze sectie is bedoeld voor ontwikkelaars die de automatisering van het project willen begrijpen.

### Projectmappen

| Map | Beschrijving |
|------|------|
| `src/` | .NET 10 vertaalpijplijn broncode, met 15 modules + 2 onafhankelijke modules |
| `config/` | Pijplijnconfiguratiebestanden (LLM, Steam, RAG parameters, enz.) |
| `data/` | Runtimegegevens: mod metadata, embeddings, vertaalcache |
| `translation_ref/` | Referentievertalingsgegevens (bijv. As1-gelicentieerde mods), voor LLM vertaalreferentie |
| `base_game_keys/` | Basis spel vertaalsleutels, voor deduplicatie om originele tekst te beschermen |
| `final_outputs/` | Definitieve uitvoer: `project_babel/` modpakket, `icons/` pictogrammen en `workshop_descriptions/` workshopbeschrijvingen |
| `docs/` | Projectdocumentatie: voortgangsrapport, bijdragengids, pijplijnuitleg |
| `temp/` | Tijdelijke bestanden van de pijplijn (aparte map per run) |
| `src/prompt_templates/` | LLM prompt-sjablonen (vertaling/inhoudscontrole) |

### Pijplijnmodules (in uitvoeringsvolgorde)

| Stap | Module | Functie |
|------|------|------|
| 1 | `ConfigReader` | Laad configuratie/sleutels/taallijst |
| 2 | `RepoDataLoader` | Laad referentievertalingen en vertaalcache |
| 3 | `ModIdCollector` | Verzamel Workshop-mod-ID's |
| 4 | `ModInfoFetcher` | Haal Steam-metadata op |
| 5 | `SteamCmdBootstrapper` | Bereid de steamcmd-runtime voor het huidige platform voor |
| 6 | `ModDownloader` | Download mods via steamcmd |
| 7 | `ContentExtractor` | Parseer mod-vertaalbestanden → `TranslationEntry` |
| 8 | `ContentChecker` | Inhoudsveiligheidscontrole (drugs/porno/geweld) |
| 9 | `EmbeddingFetcher` | Bereken tekst-embedding-vectoren |
| 10 | `TranslationBatcher` | Maak taal-onafhankelijke vertaalbatches |
| 11 | `RagContextRetriever` | Haal RAG-context op (exacte sleutels + embedding-overeenkomst) |
| 12 | `LLMTranslator` | Roep LLM aan om vertaling uit te voeren |
| 13 | `ResultWriter` | Schrijf naar data/ en translation_ref/ |
| 14 | `FinalOutputWriter` | Genereer definitieve PZ-mod-formaat uitvoer |
| 15 | `ProgressReporter` | Genereer voortgangsrapport |

### Onafhankelijke modules

| Module | Functie |
|------|------|
| `WorkshopMonitor` | Regelmatig nieuwe mods van Steam Workshop ophalen, filteren op abonnementen en integreren in `request_for_translation.txt` |
| `DocGenerator` | Meertalige documentgenerator aangedreven door LLM |

### Technologiestack

- **Taal**: C# (.NET 10)
- **Doelplatform**: GitHub Actions Linux x64 runner
- **Testen**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configureerbaar)
- **Embedding**: Tekstvectorisatie voor RAG-gelijkeniszoekopdracht
- **Inhoudscontrole**: Door LLM aangedreven meerlaagse veiligheidscontrole

Gedetailleerde [technische referentie](./docs/technical_reference/technical_reference_nl.md).

---

## Auteursrecht en Licentie

De vertaaltekstinhoud en gerelateerde afbeeldingen van dit vertaalproject zijn gemaakt of opnieuw gemaakt door **Project Babel** en deelnemers op basis van de originele spelmods.

© 2025 Project Babel en alle auteurs behouden alle rechten.

### 1. Tekst en afbeeldingen, etc.

Tenzij anders vermeld, in deze repository:

- In-game tekstvertaling, redactie en proefleesinhoud;
Projectdocumentatie, vertalingen van tekst binnen mods;
Speciaal voor dit project gemaakte afbeeldingen en kunstbronnen

vallen alle onder de **Naamsvermelding-NietCommercieel-GelijkDelen 4.0 Internationaal** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, afgekort **CC BY-NC-SA 4.0**) licentie.

Dit betekent dat u deze inhoud vrijelijk kunt delen en aanpassen, mits u zich aan de volgende voorwaarden houdt:

- **Naamsvermelding (BY)**: Vermeld op een duidelijke plek: “Dit vertaalproject is gebaseerd op het werk van 'Project Babel' en is aangepast”, en voeg de link naar deze repository en de Steam Workshop-link `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080` toe.
- **Niet-commercieel (NC)**: De inhoud van dit project of de bewerkingen ervan mogen niet worden gebruikt voor directe of indirecte commerciële doeleinden (waaronder, maar niet beperkt tot, betaalde bundels, betaalde downloads, advertentie-inkomsten, enz.);
- **GelijkDelen (SA)**: Als u wijzigingen of afgeleide werken maakt op basis van de inhoud van dit project, moet u uw gewijzigde versie openbaar maken onder **dezelfde CC BY-NC-SA 4.0-licentie**.

Zie voor meer informatie over deze licentie:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.nl>

*Speciale opmerkingen:*
- *De inhoud van de base_game_keys-map is afkomstig van de basisgame, het auteursrecht behoort toe aan de game-ontwikkelaar! De inhoud wordt gebruikt om te voorkomen dat vertaalsleutels gamesleutels overschrijven (deduplicatie).*
- *De inhoud van de translation_ref-map wordt gebruikt als vertaalreferentie voor de LLM, het auteursrecht behoort toe aan de respectievelijke mod-ontwikkelaars!*

### 2. Programma's, scripts en andere ontwikkelingsinhoud

Tenzij anders vermeld in het bronbestand of de directory, valt de programmacode in deze repository die wordt gebruikt voor het maken/pakken/verwerken van vertaalde inhoud (bijv. programmacode in de `src/` directory) onder de **GNU General Public License versie 3 (GPL-3.0)**.

Zie de volledige voorwaarden in het `LICENSE`-bestand in de root van deze repository (GPL-3.0), of bezoek de GNU-website: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Dankbetuigingen

Dit project gebruikt mods van derden als referentieteksten voor de doeltaalvertaling; de referentieteksten worden naar de LLM gestuurd als vertaalreferentie.

| Referentie mod naam | Auteur | Mod pagina |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop-pagina](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop-pagina](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop-pagina](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Hartelijk dank aan bovenstaande auteurs!**

---

## Programma's van derden

Dit project maakt gebruik van programma's en bibliotheken van derden; het auteursrecht van deze derden behoort toe aan de respectievelijke ontwikkelaars.

