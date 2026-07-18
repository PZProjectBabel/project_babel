# Project Babel — Proiect de traducere automată LLM pentru modul《Project Zomboid》

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Acest proiect de traducere este condus și întreținut de setul de instrumente [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Cuprins

- [Limbi țintă suportate de proiect](#limbi-țintă-suportate-de-proiect)
- [Cum se instalează și se utilizează](#cum-se-instalează-și-se-utilizează)
- [Progresul traducerii](#progresul-traducerii)
- [Cum să contribuiți](#cum-să-contribuiți)
- [Instrumente și structura directorului (pentru dezvoltatori)](#instrumente-și-structura-directorului-pentru-dezvoltatori)
  - [Directorul proiectului](#directorul-proiectului)
  - [Module pipeline (în ordinea execuției)](#module-pipeline-în-ordinea-execuției)
  - [Module independente](#module-independente)
  - [Stiva tehnologică](#stiva-tehnologică)
- [Drepturi de autor și licență](#drepturi-de-autor-și-licență)
  - [1. Text, imagini și alt conținut](#1-text-imagini-și-alt-conținut)
  - [2. Programe, scripturi și alte conținuturi de dezvoltare](#2-programe-scripturi-și-alte-conținuturi-de-dezvoltare)
- [Mulțumiri](#mulțumiri)
- [Programe terțe](#programe-terțe)

---

## Limbi țintă suportate de proiect

| Limbă | Nume local | Cod internațional | Cod în joc | Suportat | Note |
|------|------|------|------|------|------|
| Arabă | العربية | `ar` | `AR` | ❌ | Token insuficient |
| Catalană | català | `ca` | `CA` | ❌ | Token insuficient |
| Chineză tradițională | 繁體中文 | `zh-hant` | `CH` | ❌ | Token insuficient |
| Chineză simplificată | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Cehă | čeština | `cs` | `CS` | ❌ | Token insuficient |
| Daneză | dansk | `da` | `DA` | ❌ | Token insuficient |
| Germană | Deutsch | `de` | `DE` | ✅ | |
| Engleză | English | `en` | `EN` | ✅ | |
| Spaniolă | español | `es` | `ES` | ❌ | Token insuficient |
| Finlandeză | suomi | `fi` | `FI` | ❌ | Token insuficient |
| Franceză | français | `fr` | `FR` | ✅ | |
| Maghiară | magyar | `hu` | `HU` | ❌ | Token insuficient |
| Indoneziană | Bahasa Indonesia | `id` | `ID` | ❌ | Token insuficient |
| Italiană | italiano | `it` | `IT` | ❌ | Token insuficient |
| Japoneză | 日本語 | `ja` | `JP` | ✅ | |
| Coreeană | 한국어 | `ko` | `KO` | ❌ | Token insuficient |
| Olandeză | Nederlands | `nl` | `NL` | ❌ | Token insuficient |
| Norvegiană | norsk | `no` | `NO` | ❌ | Token insuficient |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Token insuficient |
| Poloneză | polski | `pl` | `PL` | ❌ | Token insuficient |
| Portugheză (Portugalia) | português | `pt` | `PT` | ❌ | Token insuficient |
| Portugheză (Brazilia) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token insuficient |
| Română | română | `ro` | `RO` | ❌ | Token insuficient |
| Rusă | русский | `ru` | `RU` | ❌ | Token insuficient |
| Thailandeză | ภาษาไทย | `th` | `TH` | ❌ | Token insuficient |
| Turcă | Türkçe | `tr` | `TR` | ❌ | Fond de token insuficient |
| Ucraineană | українська | `uk` | `UA` | ❌ | Fond de token insuficient |

**Total**: 27 limbi planificate | **Suportate**: 5 | **În așteptare**: 22

---

## Cum se instalează și se utilizează

Acesta este un ghid pentru jucătorii care doresc să utilizeze direct acest proiect de traducere în joc.

1.  Accesați pagina noastră Steam Workshop: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Faceți clic pe butonul „Abonare”.
3.  Porniți jocul, activați modul de traducere în meniul principal „Moduri”.
4.  Textele de traducere ale modulelor activate ulterior au prioritate față de cele activate anterior, deci acest mod de traducere trebuie activat după modulele funcționale (cât mai jos posibil).
5.  Bucurați-vă de joc!

---

## Progresul traducerii

**[➡️ Faceți clic aici pentru a vedea progresul traducerii](./docs/progress/progress_ro.md)**

---

## Cum să contribuiți

Întâmpinăm pe oricine să contribuie, fie pentru a corecta o eroare, a adăuga o funcționalitate, a scrie șabloane de prompturi, sau a oferi traduceri de referință!

Apelarea API-ului LLM pentru traducere necesită plata pentru tokeni. Pentru a asigura funcționarea stabilă pe termen lung a proiectului, sperăm că veți fi generoși!

Pentru detalii, citiți [Ghidul de contribuție](./docs/contributing/contributing_ro.md)

---

## Instrumente și structura directorului (pentru dezvoltatori)

Această secțiune se adresează dezvoltatorilor care doresc să înțeleagă principiile de automatizare ale proiectului.

### Directorul proiectului

| Director | Descriere |
|------|------|
| `src/` | Codul sursă al pipeline-ului de traducere .NET 10, conține 15 module + 2 module independente |
| `config/` | Fișiere de configurare pipeline (parametri LLM, Steam, RAG etc.) |
| `data/` | Date de execuție: metadate mod, embedding, cache de traducere |
| `translation_ref/` | Date de traducere de referință (de exemplu, modurile autorizate de As1), oferă referință de traducere LLM |
| `base_game_keys/` | Chei de traducere ale jocului de bază, utilizate pentru deduplicare și prevenirea suprascrierii textului nativ |
| `final_outputs/` | Ieșire finală: pachet mod `project_babel/`, pictograme `icons/` și descrieri workshop `workshop_descriptions/` |
| `docs/` | Documentație proiect: raport progres, ghid contribuție, explicații pipeline |
| `temp/` | Fișiere temporare pipeline (director independent pentru fiecare execuție) |
| `src/prompt_templates/` | Șabloane prompt LLM (traducere/revizuire conținut) |

### Module pipeline (în ordinea execuției)

| Pas | Modul | Funcție |
|------|------|------|
| 1 | `ConfigReader` | Încărcare configurație/chei/listă de limbi |
| 2 | `RepoDataLoader` | Încărcare traduceri de referință și cache de traducere |
| 3 | `ModIdCollector` | Colectare ID-uri de moduri Workshop |
| 4 | `ModInfoFetcher` | Obținere metadate Steam |
| 5 | `SteamCmdBootstrapper` | Pregătire runtime steamcmd pentru platforma curentă |
| 6 | `ModDownloader` | Descărcare moduri prin steamcmd |
| 7 | `ContentExtractor` | Parsare fișiere de traducere ale modului → `TranslationEntry` |
| 8 | `ContentChecker` | Revizuire securitate conținut (droguri/pornografie/violență) |
| 9 | `EmbeddingFetcher` | Calcul vectori embedding pentru text |
| 10 | `TranslationBatcher` | Creare loturi de traducere independente de limba țintă |
| 11 | `RagContextRetriever` | Recuperare context RAG (chei exacte + similaritate embedding) |
| 12 | `LLMTranslator` | Apelare LLM pentru executarea traducerii |
| 13 | `ResultWriter` | Scriere în data/ și translation_ref/ |
| 14 | `FinalOutputWriter` | Generare ieșire finală în format mod PZ |
| 15 | `ProgressReporter` | Generare raport de progres |

### Module independente

| Modul | Funcție |
|------|------|
| `WorkshopMonitor` | Preia periodic noi moduri din Steam Workshop, le filtrează după numărul de abonamente și le adaugă în `request_for_translation.txt` |
| `DocGenerator` | Generator de documentație multilingvă bazat pe LLM |

### Stiva tehnologică

- **Limbaj**: C# (.NET 10)
- **Platformă țintă**: GitHub Actions Linux x64 runner
- **Testare**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurabil)
- **Embedding**: Vectorizare text pentru căutare similaritate RAG
- **Revizuire conținut**: Audit multi-nivel securizat condus de LLM

Detalii [referință tehnică](./docs/technical_reference/technical_reference_ro.md).

---

## Drepturi de autor și licență

Conținutul textelor traduse și imaginile aferente din acest proiect de traducere sunt create sau derivate de **Project Babel** și de participanți pe baza modurilor originale de joc.

© 2025 Project Babel și toți autorii își rezervă drepturile.

### 1. Text, imagini și alt conținut

Cu excepția cazurilor specificate altfel, în acest depozit:

- Traducerea, rafinarea și corectura textelor din joc;
Documentația proiectului, traducerea textelor din moduri;
Imagini și resurse artistice create special pentru acest proiect

sunt licențiate sub **Atribuire-Necomercial-Partajare în condiții identice 4.0 Internațional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, prescurtat **CC BY-NC-SA 4.0**).

Aceasta înseamnă că, sub rezerva respectării următoarelor condiții, puteți partaja și adapta liber aceste conținuturi:

- **Atribuire (BY)**: Indicați în mod vizibil „Acest proiect de traducere se bazează pe munca proiectului „Project Babel” și a fost modificat”, și includeți linkul către acest depozit și către Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Necomercial (NC)**: Nu utilizați conținutul acestui proiect sau operele derivate în scopuri comerciale directe sau indirecte (inclusiv, dar fără a se limita la, pachete plătite, descărcări plătite, partajare de publicitate etc.);
- **Partajare în condiții identice (SA)**: Dacă modificați sau re-creați pe baza acestui conținut, trebuie să publicați versiunea modificată sub **aceeași licență CC BY-NC-SA 4.0**.

Pentru mai multe informații despre această licență, consultați:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.ro>

*Note speciale:*
- *Conținutul folderului base_game_keys provine din jocul de bază, drepturile de autor aparțin dezvoltatorului jocului! Conținutul este folosit pentru a preveni suprascrierea cheilor de traducere peste cele ale jocului (deduplicare)*
- *Conținutul folderului translation_ref este folosit pentru a oferi referințe de traducere LLM-ului, drepturile de autor aparțin dezvoltatorilor modurilor respective!*

### 2. Programe, scripturi și alte conținuturi de dezvoltare

Cu excepția cazului în care fișierele sursă sau directoarele declară altfel, codul programelor din acest depozit folosit pentru a produce/ambala/procesa conținutul de traducere (de exemplu, codul din directorul `src/`) este licențiat sub **Licența Publică Generală GNU versiunea 3 (GPL-3.0)**.

Termenii completi se găsesc în fișierul `LICENSE` din rădăcina acestui depozit (GPL-3.0) sau pe site-ul GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Mulțumiri

Acest proiect folosește moduri terțe ca texte de referință pentru traducerea în limba țintă, textele de referință fiind trimise LLM-ului pentru referință de traducere.

| Nume mod de referință | Autor | Pagina modului |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Pagina Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Pagina Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Pagina Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Le mulțumim din suflet autorilor de mai sus!**

---

## Programe terțe

Acest proiect folosește programe și biblioteci terțe, drepturile de autor ale acestor programe terțe aparțin dezvoltatorilor respectivi.

