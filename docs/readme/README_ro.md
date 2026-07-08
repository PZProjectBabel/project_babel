# Project Babel — Traducere automată a modurilor PZ cu LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Notă:** Această traducere nu este încă acceptată. Conținutul autoritar este [versiunea chineză](../../README.md).

---

*Acest proiect de traducere este gestionat de instrumentul [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Cuprins

- [Limbi țintă acceptate](#limbi-țintă-acceptate)
- [Instalare și utilizare](#instalare-și-utilizare)
- [Progresul traducerii](#progresul-traducerii)
- [Contribuții](#contribuții)
- [Unelte și structura directoarelor (pentru dezvoltatori)](#unelte-și-structura-directoarelor-(pentru-dezvoltatori))
- [Drepturi de autor și licență](#drepturi-de-autor-și-licență)
- [Mulțumiri](#mulțumiri)
- [Software terță parte](#software-terță-parte)

---

## Limbi țintă acceptate

| Limbă | Nume local | Cod ISO | Cod în joc | Acceptată | Note |
|------|------|------|------|------|------|
| Arabă | العربية | `ar` | `AR` | ❌ | Lipsă de finanțare |
| Catalană | català | `ca` | `CA` | ❌ | Lipsă de finanțare |
| Chineză tradițională | 繁體中文 | `zh-hant` | `CH` | ❌ | Lipsă de finanțare |
| Chineză simplificată | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Cehă | čeština | `cs` | `CS` | ❌ | Lipsă de finanțare |
| Daneză | dansk | `da` | `DA` | ❌ | Lipsă de finanțare |
| Germană | Deutsch | `de` | `DE` | ✅ | |
| Engleză | English | `en` | `EN` | ✅ | |
| Spaniolă | español | `es` | `ES` | ❌ | Lipsă de finanțare |
| Finlandeză | suomi | `fi` | `FI` | ❌ | Lipsă de finanțare |
| Franceză | français | `fr` | `FR` | ✅ | |
| Maghiară | magyar | `hu` | `HU` | ❌ | Lipsă de finanțare |
| Indoneziană | Bahasa Indonesia | `id` | `ID` | ❌ | Lipsă de finanțare |
| Italiană | italiano | `it` | `IT` | ❌ | Lipsă de finanțare |
| Japoneză | 日本語 | `ja` | `JP` | ✅ | |
| Coreeană | 한국어 | `ko` | `KO` | ❌ | Lipsă de finanțare |
| Olandeză | Nederlands | `nl` | `NL` | ❌ | Lipsă de finanțare |
| Norvegiană | norsk | `no` | `NO` | ❌ | Lipsă de finanțare |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Lipsă de finanțare |
| Poloneză | polski | `pl` | `PL` | ❌ | Lipsă de finanțare |
| Portugheză (Portugalia) | português | `pt` | `PT` | ❌ | Lipsă de finanțare |
| Portugheză (Brazilia) | português do Brasil | `pt-br` | `PTBR` | ❌ | Lipsă de finanțare |
| Română | română | `ro` | `RO` | ❌ | Lipsă de finanțare |
| Rusă | русский | `ru` | `RU` | ❌ | Lipsă de finanțare |
| Thailandeză | ภาษาไทย | `th` | `TH` | ❌ | Lipsă de finanțare |
| Turcă | Türkçe | `tr` | `TR` | ❌ | Lipsă de finanțare |
| Ucraineană | українська | `uk` | `UA` | ❌ | Lipsă de finanțare |

**Total**: 27 de limbi planificate | **Acceptate**: 5 | **În așteptare**: 22

---

## Instalare și utilizare

Ghid pentru jucătorii care doresc să folosească pachetul de traducere în joc.

1. Mergi pe pagina Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Dă clic pe "Abonare".
3. Lansează jocul, activează acest mod de traducere în meniul Mods.
4. Textul de traducere din modurile încărcate ulterior le suprascrie pe cele anterioare, așa că acest mod de traducere trebuie încărcat după modurile de joc.
5. Bucură-te!

---

## Progresul traducerii

[➡️ Progresul traducerii](../progress/progress_ro.md)

---

## Contribuții

Acceptăm contribuții! Corecturi de traducere, funcții noi, șabloane de prompt sau traduceri de referință.

Apelurile API LLM pentru traducere implică costuri de tokeni. Sprijinul dumneavoastră ajută proiectul să funcționeze sustenabil!

---

## Unelte și structura directoarelor (pentru dezvoltatori)

Această secțiune se adresează dezvoltatorilor care doresc să înțeleagă funcționarea internă a automatizării proiectului.

### Directoarele proiectului

| Director | Descriere |
|------|------|
| `src/` | Cod sursă pipeline traducere .NET 10, 15 module |
| `config/` | Configurare pipeline (LLM, Steam, parametri RAG etc.) |
| `data/` | Date runtime: metadate moduri, embeddings, cache traduceri |
| `translation_ref/` | Traduceri de referință ca context LLM |
| `base_game_keys/` | Chei de traducere ale jocului de bază pentru deduplicare |
| `final_outputs/` | Ieșire finală în format mod PZ |
| `docs/` | Documentație: progres, contribuții, specificații pipeline |
| `temp/` | Fișiere temporare pipeline |
| `src/prompt_templates/` | Șabloane de prompt LLM |

### Modulele pipeline (ordine de execuție)

| Pas | Modul | Funcție |
|------|------|------|
| 1 | `ConfigReader` | Încărcare configurare/secrete/limbi |
| 2 | `RepoDataLoader` | Încărcare referințe și cache traduceri |
| 3 | `ModIdCollector` | Colectare ID-uri moduri Workshop |
| 4 | `ModInfoFetcher` | Obținere metadate Steam |
| 5 | `ModDownloader` | Descărcare moduri prin steamcmd |
| 6 | `ContentExtractor` | Analizare fișiere traducere → `TranslationEntry` |
| 7 | `ContentChecker` | Verificare securitate conținut |
| 8 | `EmbeddingFetcher` | Calculare vectori embedding text |
| 9 | `TranslationBatcher` | Creare loturi de traducere |
| 10 | `RagContextRetriever` | Recuperare contexte RAG |
| 11 | `LLMTranslator` | Executare traducere LLM |
| 12 | `ResultWriter` | Scriere în data/ și translation_ref/ |
| 13 | `FinalOutputWriter` | Generare ieșire finală format mod PZ |
| 14 | `ProgressReporter` | Generare rapoarte de progres |

### Stiva tehnologică

- **Limbaj**: C# (.NET 10)
- **Platformă țintă**: GitHub Actions Linux x64 runner
- **Teste**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurabil)
- **Embedding**: Vectorizare text pentru căutare de similaritate RAG
- **Verificare conținut**: Audit de securitate multi-nivel bazat pe LLM

Documentație tehnică detaliată: [Pipeline TranslationEntry](../pipeline/translation_entry_pipeline_ro.md)

---

## Drepturi de autor și licență

© 2025 Project Babel și toți autorii. Toate drepturile rezervate.

### Conținut (texte, imagini)

Licențiat sub **CC BY-NC-SA 4.0**.

- **Atribuire**: Menționați modificările bazate pe „Project Babel", cu link-uri către repo și Workshop
- **Necomercial**: Utilizarea comercială interzisă
- **Distribuire în condiții identice**: Modificările trebuie publicate sub aceeași licență

### Cod

Codul din `src/` este licențiat sub **GPL-3.0**.

---

## Mulțumiri

| Mod de referință | Autor | Pagină |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Mulțumiri profunde autorilor de mai sus!**

---

## Software terță parte

Acest proiect utilizează programe și biblioteci terțe, drepturile de autor aparținând dezvoltatorilor respectivi.
