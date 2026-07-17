# Project Babel — LLM-automatisk oversettelsesprosjekt for mod til Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Dette oversettelsesprosjektet drives og vedlikeholdes av [Project Babel](https://github.com/PZProjectBabel/project_babel) verktøysettet.*

---

## Innholdsfortegnelse

- [Støttede oversettelsesspråk](#støttede-oversettelsesspråk)
- [Hvordan installere og bruke](#hvordan-installere-og-bruke)
- [Oversettelsesfremdrift](#oversettelsesfremdrift)
- [Hvordan bidra](#hvordan-bidra)
- [Verktøy og katalogstruktur (for utviklere)](#verktøy-og-katalogstruktur-for-utviklere)
  - [Prosjektkatalog](#prosjektkatalog)
  - [Pipelinemoduler (etter utførelsesrekkefølge)](#pipelinemoduler-etter-utførelsesrekkefølge)
  - [技术栈](#技术栈)
- [Opphavsrett og lisensiering](#opphavsrett-og-lisensiering)
  - [1. Tekst, bilder og annet innhold](#1-tekst-bilder-og-annet-innhold)
  - [2. Programmer, skript og annet utviklingsinnhold](#2-programmer-skript-og-annet-utviklingsinnhold)
- [Takk til](#takk-til)
- [Tredjepartsprogrammer](#tredjepartsprogrammer)

---

## Støttede oversettelsesspråk

| Språk | Lokalt navn | Internasjonal kode | Spillkode | Støttet | Notater |
|------|------|------|------|------|------|
| Arabisk | العربية | `ar` | `AR` | ❌ | Ikke nok tokenkvote |
| Katalansk | català | `ca` | `CA` | ❌ | Ikke nok tokenkvote |
| Tradisjonell kinesisk | 繁體中文 | `zh-hant` | `CH` | ❌ | Ikke nok tokenkvote |
| Forenklet kinesisk | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tsjekkisk | čeština | `cs` | `CS` | ❌ | Ikke nok tokenkvote |
| Dansk | dansk | `da` | `DA` | ❌ | Ikke nok tokenkvote |
| Tysk | Deutsch | `de` | `DE` | ✅ | |
| Engelsk | English | `en` | `EN` | ✅ | |
| Spansk | español | `es` | `ES` | ❌ | Ikke nok tokenkvote |
| Finsk | suomi | `fi` | `FI` | ❌ | Ikke nok tokenkvote |
| Fransk | français | `fr` | `FR` | ✅ | |
| Ungarsk | magyar | `hu` | `HU` | ❌ | Ikke nok tokenkvote |
| Indonesisk | Bahasa Indonesia | `id` | `ID` | ❌ | Ikke nok tokenkvote |
| Italiensk | italiano | `it` | `IT` | ❌ | Ikke nok tokenkvote |
| Japansk | 日本語 | `ja` | `JP` | ✅ | |
| Koreansk | 한국어 | `ko` | `KO` | ❌ | Ikke nok tokenkvote |
| Nederlandsk | Nederlands | `nl` | `NL` | ❌ | Ikke nok tokenkvote |
| Norsk | norsk | `no` | `NO` | ❌ | Ikke nok tokenkvote |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Ikke nok tokenkvote |
| Polsk | polski | `pl` | `PL` | ❌ | Ikke nok tokenkvote |
| Portugisisk (Portugal) | português | `pt` | `PT` | ❌ | Ikke nok tokenkvote |
| Portugisisk (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Ikke nok tokenkvote |
| Rumensk | română | `ro` | `RO` | ❌ | Ikke nok tokenkvote |
| Russisk | русский | `ru` | `RU` | ❌ | Ikke nok tokenkvote |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Ikke nok tokenkvote |
| Tyrkisk | Türkçe | `tr` | `TR` | ❌ | Utilstrekkelig token-kvote |
| Ukrainsk | українська | `uk` | `UA` | ❌ | Utilstrekkelig token-kvote |

**Totalt**: 27 planlagte språk | **Støttet**: 5 | **Venter**: 22

---

## Hvordan installere og bruke

Dette er en veiledning for spillere som ønsker å bruke dette oversettelsesprosjektet direkte i spillet.

1.  Gå til Steam-verkstedssiden vår: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Klikk på «Abonner»-knappen.
3.  Start spillet, og aktiver denne oversettelsesmodulen i «Mods»-menyen i hovedmenyen.
4.  Oversettelsestekster fra moduler som aktiveres senere, overstyrer de som aktiveres tidligere. Derfor bør denne oversettelsesmodulen aktiveres etter funksjonsmodulene (plasser den så lavt som mulig).
5.  Nyt spillet!

---

## Oversettelsesfremdrift

**[➡️ Klikk her for å se oversettelsesfremdrift](./docs/progress/progress_no.md)**

---

## Hvordan bidra

Vi ønsker alle velkommen til å bidra, enten det er å rette en feil, legge til en funksjon, skrive en promptmal eller gi referanseoversettelser!

Å kalle LLM-API for oversettelse krever betaling for tokens. For at prosjektet skal kunne kjøre stabilt over tid, håper vi du kan være sjenerøs og hjelpe!

Les [Bidragsguide](./docs/contributing/contributing_no.md) for detaljer.

---

## Verktøy og katalogstruktur (for utviklere)

Denne delen er for utviklere som ønsker å forstå prosjektets automatiseringsprinsipper.

### Prosjektkatalog

| Katalog | Beskrivelse |
|------|------|
| `src/` | .NET 10 oversettelsespipelinekildekode, inkludert 15 moduler |
| `config/` | Pipelinekonfigurasjonsfiler (LLM-, Steam-, RAG-parametere osv.) |
| `data/` | Kjøretidsdata: modulmetadata, embedding, oversettelsesbuffer |
| `translation_ref/` | Referanseoversettelsesdata (f.eks. autoriserte moduler fra As1), gir oversettelsesreferanse for LLM |
| `base_game_keys/` | Oversettelsesnøkler for spillet, brukes for å unngå duplikater og forhindre overskriving av originaltekst |
| `final_outputs/` | Endelig utdata: `project_babel/` modulpakke, `icons/` ikoner og `workshop_descriptions/` verkstedbeskrivelser |
| `docs/` | Prosjektdokumentasjon: fremdriftsrapport, bidragsguide, pipelinebeskrivelse |
| `temp/` | Pipeline midlertidige filer (uavhengig katalog for hver kjøring) |
| `src/prompt_templates/` | LLM promptmaler (oversettelse/innholdsmoderasjon) |

### Pipelinemoduler (etter utførelsesrekkefølge)

| Trinn | Modul | Funksjon |
|------|------|------|
| 1 | `ConfigReader` | Laster inn konfigurasjon/nøkler/språkliste |
| 2 | `RepoDataLoader` | Laster inn referanseoversettelser og oversettelsesbuffer |
| 3 | `ModIdCollector` | Samler inn Workshop-mod-ID-er |
| 4 | `ModInfoFetcher` | Henter Steam-metadata |
| 5 | `SteamCmdBootstrapper` | Forbereder steamcmd-kjøring for gjeldende plattform |
| 6 | `ModDownloader` | Laster ned moduler via steamcmd |
| 7 | `ContentExtractor` | Tolker moduloversettelsesfiler → `TranslationEntry` |
| 8 | `ContentChecker` | Innholdssikkerhetskontroll (narkotika/pornografi/vold) |
| 9 | `EmbeddingFetcher` | Beregner tekst-embedding-vektorer |
| 10 | `TranslationBatcher` | Oppretter språkuavhengige oversettelsespartier |
| 11 | `RagContextRetriever` | Henter RAG-kontekst (nøyaktig nøkkel + embedding-likhet) |
| 12 | `LLMTranslator` | Kaller LLM for å utføre oversettelse |
| 13 | `ResultWriter` | Skriver til data/ og translation_ref/ |
| 14 | `FinalOutputWriter` | Genererer endelig PZ-modulformatutdata |
| 15 | `ProgressReporter` | Genererer fremdriftsrapport |

### 技术栈

- **Språk**: C# (.NET 10)
- **Målplattform**: GitHub Actions Linux x64 runner
- **Testing**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurerbar)
- **Embedding**: Tekstvektorisering for RAG-likhetssøk
- **Innholdskontroll**: LLM-drevet flernivå sikkerhetsrevisjon

Detaljert [Teknisk referanse](./docs/technical_reference/technical_reference_no.md).

---

## Opphavsrett og lisensiering

Oversettelsestekstinnholdet og relaterte bilder i dette oversettelsesprosjektet er skapt eller videreutviklet av **Project Babel** og deltakerne basert på originale spillmoduler.

© 2025 Project Babel og respektive forfattere forbeholder seg rettigheter.

### 1. Tekst, bilder og annet innhold

Med mindre annet er spesifikt angitt, i dette repositoriet:

- Oversettelse av tekst i spillet, polering og korrekturlesing;
Prosjektdokumentasjon, modulinterne tekstoversettelser;
Bilder og kunstressurser spesielt laget for dette prosjektet

er alle lisensiert under **Navngivelse-IkkeKommersiell-DelPåSammeVilkår 4.0 Internasjonal** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, forkortet **CC BY-NC-SA 4.0**) lisensen.

Dette betyr at du kan fritt dele og bearbeide disse innholdene, under forutsetning av at følgende vilkår overholdes:

- **Navngivelse (BY)**: Angi på et tydelig sted at "dette oversettelsesprosjektet er basert på arbeidet til 'Project Babel' og er modifisert", og legg ved en lenke til dette repositoriet og Steam Workshop-lenken   `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Ikke-kommersiell bruk (NC)**: Du kan ikke bruke prosjektets innhold eller bearbeidede verk til noen direkte eller indirekte kommersielle formål (inkludert men ikke begrenset til betalte pakker, betalte nedlastinger, annonseinntekter, etc.);
- **Del på samme vilkår (SA)**: Hvis du modifiserer eller gjenbruker innholdet fra dette prosjektet, må du offentliggjøre dine endringer under **samme CC BY-NC-SA 4.0-lisens**.

For mer informasjon om denne lisensen, se:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.no>

*Spesielle merknader:*
- *base_game_keys-mappen inneholder innhold fra selve spillet, opphavsrett tilhører spillutvikleren! Innholdet brukes for å hindre at oversettelsesnøkler overskriver spillnøkler (deduplisering)*
- *translation_ref-mappen inneholder referanseoversettelser for LLM, opphavsrett tilhører de respektive modutviklerne!*

### 2. Programmer, skript og annet utviklingsinnhold

Med mindre annet er spesifikt erklært i kildekodefiler eller mapper, er programkoden i dette repositoriet som brukes til å lage/pakke/behandle oversettelsesinnhold (f.eks. programkoden i `src/` mappen) lisensiert under **GNU General Public License versjon 3 (GPL-3.0)**.

Fullstendige vilkår finner du i `LICENSE`-filen i rotkatalogen til dette repositoriet (GPL-3.0), eller besøk GNU-nettsiden: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Takk til

Dette prosjektet bruker tredjeparts mods som referansetekster for målspråkoversettelse. Referansetekstene sendes til LLM for oversettelsesreferanse.

| Referansemodnavn | Forfatter | Mod-side |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Vi retter en oppriktig takk til forfatterne ovenfor!**

---

## Tredjepartsprogrammer

Dette prosjektet bruker tredjeparts programmer og biblioteker. Opphavsretten til disse tredjepartsprogrammene tilhører de respektive utviklerne.

