# Project Babel — 《僵尸毁灭工程》 mod LLM automatikus fordítási projekt

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Ezt a fordító projektet a [Project Babel](https://github.com/PZProjectBabel/project_babel) eszközkészlet hajtja és tartja karban.*

---

## Tartalomjegyzék

- [Támogatott célfordítási nyelvek](#támogatott-célfordítási-nyelvek)
- [Telepítés és használat](#telepítés-és-használat)
- [Fordítási haladás](#fordítási-haladás)
- [Hogyan járulj hozzá](#hogyan-járulj-hozzá)
- [Eszközök és könyvtárszerkezet (Fejlesztőknek)](#eszközök-és-könyvtárszerkezet-fejlesztőknek)
  - [Projektkönyvtárak](#projektkönyvtárak)
  - [Folyamatmodulok (végrehajtási sorrendben)](#folyamatmodulok-végrehajtási-sorrendben)
  - [Független modulok](#független-modulok)
  - [Technológiai stack](#technológiai-stack)
- [Szerzői jog és licenc](#szerzői-jog-és-licenc)
  - [1. Szövegek, képek és egyéb tartalmak](#1-szövegek-képek-és-egyéb-tartalmak)
  - [2. Programok, szkriptek és egyéb fejlesztési tartalmak](#2-programok-szkriptek-és-egyéb-fejlesztési-tartalmak)
- [Köszönetnyilvánítás](#köszönetnyilvánítás)
- [Harmadik féltől származó programok](#harmadik-féltől-származó-programok)

---

## Támogatott célfordítási nyelvek

| Nyelv | Helyi név | Nemzetközi kód | Játékbeli kód | Támogatott | Megjegyzés |
|------|------|------|------|------|------|
| Arab | العربية | `ar` | `AR` | ❌ | Token keret elégtelen |
| Katalán | català | `ca` | `CA` | ❌ | Token keret elégtelen |
| Hagyományos kínai | 繁體中文 | `zh-hant` | `CH` | ❌ | Token keret elégtelen |
| Egyszerűsített kínai | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Cseh | čeština | `cs` | `CS` | ❌ | Token keret elégtelen |
| Dán | dansk | `da` | `DA` | ❌ | Token keret elégtelen |
| Német | Deutsch | `de` | `DE` | ✅ | |
| Angol | English | `en` | `EN` | ✅ | |
| Spanyol | español | `es` | `ES` | ❌ | Token keret elégtelen |
| Finn | suomi | `fi` | `FI` | ❌ | Token keret elégtelen |
| Francia | français | `fr` | `FR` | ✅ | |
| Magyar | magyar | `hu` | `HU` | ❌ | Token keret elégtelen |
| Indonéz | Bahasa Indonesia | `id` | `ID` | ❌ | Token keret elégtelen |
| Olasz | italiano | `it` | `IT` | ❌ | Token keret elégtelen |
| Japán | 日本語 | `ja` | `JP` | ✅ | |
| Koreai | 한국어 | `ko` | `KO` | ❌ | Token keret elégtelen |
| Holland | Nederlands | `nl` | `NL` | ❌ | Token keret elégtelen |
| Norvég | norsk | `no` | `NO` | ❌ | Token keret elégtelen |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Token keret elégtelen |
| Lengyel | polski | `pl` | `PL` | ❌ | Token keret elégtelen |
| Portugál (Portugália) | português | `pt` | `PT` | ❌ | Token keret elégtelen |
| Portugál (Brazília) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token keret elégtelen |
| Román | română | `ro` | `RO` | ❌ | Token keret elégtelen |
| Orosz | русский | `ru` | `RU` | ❌ | Token keret elégtelen |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Token keret elégtelen |
| török | Türkçe | `tr` | `TR` | ❌ | A tokenkeret nem elég |
| ukrán | українська | `uk` | `UA` | ❌ | A tokenkeret nem elég |

**Összesen**：27 tervezett nyelv | **Támogatott**：5 nyelv | **Várólistán**：22 nyelv

---

## Telepítés és használat

Ez az útmutató azoknak a játékosoknak szól, akik közvetlenül a játékban szeretnék használni ezt a fordítási projektet.

1.  Látogass el a Steam Műhely oldalunkra：[[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Kattints a „Feliratkozás” gombra.
3.  Indítsd el a játékot, majd a főmenü „Modok” kezelésében engedélyezd ezt a fordítási modot.
4.  A később engedélyezett modok fordítási szövege elsőbbséget élvez a korábban engedélyezettekkel szemben, ezért ezt a fordítási modot a funkcionális modok után kell engedélyezni (lehetőleg legalul).
5.  Élvezd a játékot!

---

## Fordítási haladás

**[➡️ Kattints ide a fordítás haladásának megtekintéséhez](./docs/progress/progress_hu.md)**

---

## Hogyan járulj hozzá

Üdvözöljük bárki részvételét, legyen szó hibajavításról, új funkció hozzáadásáról, prompt sablon megírásáról, vagy referenciául szolgáló fordítások biztosításáról!

Az LLM API használata tokenenként fizetős; a projekt hosszú távú stabil működése érdekében köszönjük nagylelkű támogatását!

Részletekért olvasd el a [Hozzájárulási útmutatót](./docs/contributing/contributing_hu.md)

---

## Eszközök és könyvtárszerkezet (Fejlesztőknek)

Ez a szakasz azoknak a fejlesztőknek szól, akik szeretnék megérteni a projekt automatizálási elveit.

### Projektkönyvtárak

| Könyvtár | Leírás |
|------|------|
| `src/` | .NET 10 fordítási csővezeték forráskódja, 15 modul + 2 független modul |
| `config/` | Folyamat konfigurációs fájlok (LLM, Steam, RAG paraméterek stb.) |
| `data/` | Futásidejű adatok: mod metaadatok, beágyazások, fordítási gyorsítótár |
| `translation_ref/` | Referencia fordítási adatok (如一汉化组 által engedélyezett modok), amelyek fordítási referenciát biztosítanak az LLM számára |
| `base_game_keys/` | Játékkulcsok az eredeti játékszövegből, ismétlődés elkerülésére és a natív szöveg felülírásának megakadályozására |
| `final_outputs/` | Végső kimenet: `project_babel/` mod csomag, `icons/` ikonok és `workshop_descriptions/` Műhely leírások |
| `docs/` | Projekt dokumentáció: haladási jelentés, hozzájárulási útmutató, folyamat leírás |
| `temp/` | Folyamat ideiglenes fájljai (minden futtatáskor külön könyvtár) |
| `src/prompt_templates/` | LLM prompt sablonok (fordítás/tartalomellenőrzés) |

### Folyamatmodulok (végrehajtási sorrendben)

| Lépés | Modul | Funkció |
|------|------|------|
| 1 | `ConfigReader` | Konfiguráció/kulcsok/nyelvlista betöltése |
| 2 | `RepoDataLoader` | Referenciafordítás és fordítói gyorsítótár betöltése |
| 3 | `ModIdCollector` | Workshop modul azonosítók gyűjtése |
| 4 | `ModInfoFetcher` | Steam metaadatok lekérése |
| 5 | `SteamCmdBootstrapper` | Az aktuális platform steamcmd futási környezetének előkészítése |
| 6 | `ModDownloader` | Modulok letöltése steamcmd-n keresztül |
| 7 | `ContentExtractor` | Modul fordítási fájljainak elemzése → `TranslationEntry` |
| 8 | `ContentChecker` | Tartalombiztonsági ellenőrzés (kábítószer/pornográfia/erőszak) |
| 9 | `EmbeddingFetcher` | Szöveges beágyazási vektorok számítása |
| 10 | `TranslationBatcher` | Nyelvfüggetlen fordítási kötegek létrehozása |
| 11 | `RagContextRetriever` | RAG kontextus lekérése (pontos kulcs + beágyazási hasonlóság) |
| 12 | `LLMTranslator` | LLM meghívása fordítás végrehajtásához |
| 13 | `ResultWriter` | Írás a data/ és translation_ref/ könyvtárakba |
| 14 | `FinalOutputWriter` | Végső PZ modul formátumú kimenet előállítása |
| 15 | `ProgressReporter` | Haladásjelentés generálása |

### Független modulok

| Modul | Funkció |
|------|------|
| `WorkshopMonitor` | Rendszeresen letölti az új Steam Workshop modulokat, szűrés a feliratkozások száma alapján, és hozzáadja a `request_for_translation.txt` fájlhoz |
| `DocGenerator` | LLM által vezérelt többnyelvű dokumentumgenerátor |

### Technológiai stack

- **Nyelv**: C# (.NET 10)
- **Célplatform**: GitHub Actions Linux x64 runner
- **Tesztelés**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurálható)
- **Embedding**: Szöveg vektorizálása RAG hasonlósági kereséshez
- **Tartalomellenőrzés**: LLM által vezérelt többszintű biztonsági audit

Részletes [technikai referenciák](./docs/technical_reference/technical_reference_hu.md).

---

## Szerzői jog és licenc

A fordítási projekt fordítási szövegei és kapcsolódó képei a **Project Babel** és a résztvevők által, az eredeti játékmódosítások alapján készültek vagy másodlagosan feldolgozottak.

© 2025 Project Babel és a szerzők – minden jog fenntartva.

### 1. Szövegek, képek és egyéb tartalmak

Hacsak külön nem jelezzük, a jelen tárolóban található:

- Játékon belüli szövegek fordítása, stilizálása és lektorálása;
Projektdokumentációk, mod belső szövegfordításai;
A projekt által készített képek és művészeti források.

Mindegyik a **Nevezd meg! - Ne add el! - Így add tovább! 4.0 Nemzetközi** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, röviden **CC BY-NC-SA 4.0**) licenc alatt áll.

Ez azt jelenti, hogy a következő feltételek betartásával szabadon megoszthatja és átdolgozhatja ezeket a tartalmakat:

- **Nevezd meg (BY)**: Tüntesse fel jól látható helyen, hogy "Ez a fordítási projekt a 'Project Babel' munkáján alapul", és csatolja a jelen tároló és a Steam Workshop linkjét: `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Ne add el (NC)**: Ne használja a projekt tartalmát vagy annak átdolgozott változatait semmilyen közvetlen vagy közvetett kereskedelmi célra (idesorolva a fizetős csomagokat, fizetős letöltéseket, hirdetésmegosztást stb.);
- **Így add tovább (SA)**: Ha a projekt tartalmát módosítja vagy átdolgozza, a változtatásokat **ugyanazon CC BY-NC-SA 4.0 licenc** alatt kell nyilvánosan közzétennie.

A licencről további információért látogassa meg:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.hu>

*Különleges megjegyzés:*
- *A base_game_keys mappa tartalma a játékból származik, a szerzői jog a játék fejlesztőjéé! A tartalom a fordítási kulcsok játékkulcsokkal való felülírásának megakadályozására szolgál (deduplikáció)*
- *A translation_ref mappa tartalma az LLM számára nyújt fordítási referenciát, a szerzői jog az egyes modfejlesztőké!*

### 2. Programok, szkriptek és egyéb fejlesztési tartalmak

Kivéve, ha a forrásfájl vagy mappa másként rendelkezik, a jelen tárolóban található, a lokalizációs tartalmak előállítására/csomagolására/feldolgozására szolgáló programkód (pl. a `src/` mappában található kód) a **GNU General Public License 3. verziója (GPL-3.0)** alatt licencelt.

A teljes licencfeltételek a tároló gyökérkönyvtárában található `LICENSE` fájlban (GPL-3.0) olvashatók, vagy látogassa meg a GNU weboldalát: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Köszönetnyilvánítás

Ez a projekt harmadik féltől származó modokat használ a célnyelvi fordítás referenciaszövegeként. A referenciaszövegeket elküldjük az LLM-nek fordítási referenciaként.

| Referencia mod neve | Szerző | Mod oldal |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Steam Workshop oldal](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Steam Workshop oldal](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Steam Workshop oldal](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Hálás köszönet a fent említett szerzőknek!**

---

## Harmadik féltől származó programok

Ez a projekt harmadik féltől származó programokat és könyvtárakat használ, ezek szerzői joga a megfelelő fejlesztőket illeti.

