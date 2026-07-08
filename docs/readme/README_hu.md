# Project Babel — PZ modok automatikus LLM fordítása

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Megjegyzés:** Ez a fordítás még nem támogatott. A mérvadó tartalom a [kínai verzió](../../README.md).

---

*Ezt a fordítási projektet a [Project Babel](https://github.com/PZProjectBabel/project_babel) eszköz működteti és tartja karban.*

---

## Tartalomjegyzék

- [Támogatott célnyelvek](#támogatott-célnyelvek)
- [Telepítés és használat](#telepítés-és-használat)
- [Fordítási állapot](#fordítási-állapot)
- [Közreműködés](#közreműködés)
- [Eszközök és könyvtárszerkezet (fejlesztőknek)](#eszközök-és-könyvtárszerkezet-(fejlesztőknek))
- [Szerzői jog és licenc](#szerzői-jog-és-licenc)
- [Köszönetnyilvánítás](#köszönetnyilvánítás)
- [Harmadik féltől származó szoftverek](#harmadik-féltől-származó-szoftverek)

---

## Támogatott célnyelvek

| Nyelv | Helyi név | ISO kód | Játékbeli kód | Támogatott | Megjegyzés |
|------|------|------|------|------|------|
| Arab | العربية | `ar` | `AR` | ❌ | Finanszírozás hiánya |
| Katalán | català | `ca` | `CA` | ❌ | Finanszírozás hiánya |
| Hagyományos kínai | 繁體中文 | `zh-hant` | `CH` | ❌ | Finanszírozás hiánya |
| Egyszerűsített kínai | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Cseh | čeština | `cs` | `CS` | ❌ | Finanszírozás hiánya |
| Dán | dansk | `da` | `DA` | ❌ | Finanszírozás hiánya |
| Német | Deutsch | `de` | `DE` | ✅ | |
| Angol | English | `en` | `EN` | ✅ | |
| Spanyol | español | `es` | `ES` | ❌ | Finanszírozás hiánya |
| Finn | suomi | `fi` | `FI` | ❌ | Finanszírozás hiánya |
| Francia | français | `fr` | `FR` | ✅ | |
| Magyar | magyar | `hu` | `HU` | ❌ | Finanszírozás hiánya |
| Indonéz | Bahasa Indonesia | `id` | `ID` | ❌ | Finanszírozás hiánya |
| Olasz | italiano | `it` | `IT` | ❌ | Finanszírozás hiánya |
| Japán | 日本語 | `ja` | `JP` | ✅ | |
| Koreai | 한국어 | `ko` | `KO` | ❌ | Finanszírozás hiánya |
| Holland | Nederlands | `nl` | `NL` | ❌ | Finanszírozás hiánya |
| Norvég | norsk | `no` | `NO` | ❌ | Finanszírozás hiánya |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Finanszírozás hiánya |
| Lengyel | polski | `pl` | `PL` | ❌ | Finanszírozás hiánya |
| Portugál (Portugália) | português | `pt` | `PT` | ❌ | Finanszírozás hiánya |
| Portugál (Brazília) | português do Brasil | `pt-br` | `PTBR` | ❌ | Finanszírozás hiánya |
| Román | română | `ro` | `RO` | ❌ | Finanszírozás hiánya |
| Orosz | русский | `ru` | `RU` | ❌ | Finanszírozás hiánya |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Finanszírozás hiánya |
| Török | Türkçe | `tr` | `TR` | ❌ | Finanszírozás hiánya |
| Ukrán | українська | `uk` | `UA` | ❌ | Finanszírozás hiánya |

**Összesen**: 27 tervezett nyelv | **Támogatott**: 5 | **Függőben**: 22

---

## Telepítés és használat

Útmutató azoknak a játékosoknak, akik a fordításcsomagot szeretnék használni a játékban.

1. Látogass el a Steam Workshop oldalra: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Kattints a "Feliratkozás" gombra.
3. Indítsd el a játékot, és engedélyezd ezt a fordítási modot a Modok menüben.
4. A később betöltött modok fordítási szövege felülírja a korábbiakat, ezért ennek a fordítási modnak a játékmodok után kell betöltődnie.
5. Jó szórakozást!

---

## Fordítási állapot

[➡️ Fordítási állapot](../progress/progress_hu.md)

---

## Közreműködés

Várjuk a hozzájárulásokat! Fordítási javítások, új funkciók, promptsablonok vagy referenciafordítások.

Az LLM API-hívások fordításra tokenköltséggel járnak. Támogatása segíti a projekt fenntartható működését!

---

## Eszközök és könyvtárszerkezet (fejlesztőknek)

Ez a szakasz azoknak a fejlesztőknek szól, akik meg szeretnék érteni a projekt automatizálásának belső működését.

### Projektkönyvtárak

| Könyvtár | Leírás |
|------|------|
| `src/` | .NET 10 fordítási folyamat forráskódja, 15 modul |
| `config/` | Folyamatkonfiguráció (LLM, Steam, RAG paraméterek stb.) |
| `data/` | Futásidejű adatok: mod metaadatok, beágyazások, fordítási gyorsítótár |
| `translation_ref/` | Referenciafordítások LLM kontextusként |
| `base_game_keys/` | Alapjáték fordítási kulcsok duplikációszűréshez |
| `final_outputs/` | Végső PZ mod formátumú fordítási kimenet |
| `docs/` | Dokumentáció: haladás, közreműködés, folyamat specifikációk |
| `temp/` | Folyamat ideiglenes fájljai |
| `src/prompt_templates/` | LLM prompt sablonok |

### Folyamatmodulok (végrehajtási sorrend)

| Lépés | Modul | Funkció |
|------|------|------|
| 1 | `ConfigReader` | Konfiguráció/titkok/nyelvek betöltése |
| 2 | `RepoDataLoader` | Referenciák és fordítási gyorsítótár betöltése |
| 3 | `ModIdCollector` | Workshop mod azonosítók gyűjtése |
| 4 | `ModInfoFetcher` | Steam metaadatok lekérése |
| 5 | `ModDownloader` | Modok letöltése steamcmd-n keresztül |
| 6 | `ContentExtractor` | Mod fordítási fájlok elemzése → `TranslationEntry` |
| 7 | `ContentChecker` | Tartalombiztonsági ellenőrzés |
| 8 | `EmbeddingFetcher` | Szöveg beágyazási vektorok számítása |
| 9 | `TranslationBatcher` | Fordítási kötegek létrehozása |
| 10 | `RagContextRetriever` | RAG kontextusok lekérése |
| 11 | `LLMTranslator` | LLM fordítás végrehajtása |
| 12 | `ResultWriter` | Írás a data/ és translation_ref/ könyvtárakba |
| 13 | `FinalOutputWriter` | Végső PZ mod kimenet generálása |
| 14 | `ProgressReporter` | Haladási jelentések generálása |

### Technológiai stack

- **Nyelv**: C# (.NET 10)
- **Célplatform**: GitHub Actions Linux x64 runner
- **Tesztelés**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurálható)
- **Embedding**: Szövegvektorizálás RAG hasonlósági kereséshez
- **Tartalomellenőrzés**: LLM-vezérelt többszintű biztonsági ellenőrzés

Részletes műszaki dokumentáció: [TranslationEntry folyamat](../pipeline/translation_entry_pipeline_hu.md)

---

## Szerzői jog és licenc

© 2025 Project Babel és minden szerző. Minden jog fenntartva.

### Tartalom (szövegek, képek)

Licencelve **CC BY-NC-SA 4.0** alatt.

- **Nevezd meg!**: Tüntesd fel a "Project Babel" alapú módosításokat, repó és Workshop linkekkel
- **Ne add el!**: Kereskedelmi felhasználás tilos
- **Így add tovább!**: A módosításokat ugyanazon licenc alatt kell közzétenni

### Kód

A `src/` alatti kód **GPL-3.0** licenc alatt áll.

---

## Köszönetnyilvánítás

| Referencia mod | Szerző | Oldal |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Őszinte köszönet a fenti szerzőknek!**

---

## Harmadik féltől származó szoftverek

Ez a projekt harmadik féltől származó programokat és könyvtárakat használ, a szerzői jogok a megfelelő fejlesztőket illetik.
