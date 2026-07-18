# Hozzájárulási Útmutató (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Tartalomjegyzék

- [1. Kezdés előtt](#1-kezdés-előtt)
- [2. Hogyan járulhatok hozzá?](#2-hogyan-járulhatok-hozzá)
- [3. Fordítási szabályok, szótár, rendszerprompt fejlesztése](#3-fordítási-szabályok-szótár-rendszerprompt-fejlesztése)
- [4. Kézzel ellenőrzött korpusz biztosítása](#4-kézzel-ellenőrzött-korpusz-biztosítása)
- [5. Csővezeték- és eszközfejlesztési hozzájárulás](#5-csővezeték--és-eszközfejlesztési-hozzájárulás)
- [6. Szerzői jogok és licenc megállapodás](#6-szerzői-jogok-és-licenc-megállapodás)
  - [6.1 Alapelv: Te megtartod a szerzői jogokat, és egyidejűleg felhatalmazod a projektet a felhasználásra](#61-alapelv-te-megtartod-a-szerzői-jogokat-és-egyidejűleg-felhatalmazod-a-projektet-a-felhasználásra)
  - [6.2 Szövegek és képek engedélyezése (CC BY-NC-SA 4.0)](#62-szövegek-és-képek-engedélyezése-cc-by-nc-sa-40)
  - [6.3 Szkriptek és eszközkódok engedélyezése (GPL-3.0)](#63-szkriptek-és-eszközkódok-engedélyezése-gpl-30)
  - [6.4 Felsőbb művek és az eredeti játék szerzői joga](#64-felsőbb-művek-és-az-eredeti-játék-szerzői-joga)
- [7. Kommunikáció és együttműködés](#7-kommunikáció-és-együttműködés)
- [8. Pénzügyi támogatás](#8-pénzügyi-támogatás)

---

Nagyon köszönjük, hogy készen állsz hozzájárulni a **Project Babel - Zomboid mod LLM automatikus fordító projekt**-hez! Legyen szó hibajavításról, új funkció hozzáadásáról, prompt sablon írásáról, vagy referenciául szolgáló fordításról!

A LLM API hívása tokenekért fizetős, hogy a projekt hosszú távon stabilan működhessen, reméljük, nagylelkűen támogatsz minket!

> ⚠️ **Fontos figyelmeztetés:**
> Mielőtt bármit beküldesz ebbe a tárolóba, feltétlenül olvasd el és értsd meg a "Szerzői jogok és licencszerződés" szakaszt.
> Ha beküldésre és beolvasztásra kerül, azzal elfogadod a vonatkozó licencfeltételeket.

---

## 1. Kezdés előtt

Először olvasd el a projekt `README.md` fájlját, hogy megismerd:
- A projekt átfogó célját és jelenlegi állapotát;
- Hogyan használhatják a hétköznapi játékosok a projektet (hogy magad is tesztelhesd);
- A projekt technikai részleteit.

---

## 2. Hogyan járulhatok hozzá?

Érdeklődésed és képességeid alapján választhatsz egy vagy több módot a részvételre:

- A célnyelv fordítási szabályainak biztosítása
- A célnyelv fordítói szótárának biztosítása
- A rendszer promptjainak fejlesztése
- Ember által ellenőrzött fordítási szövegek biztosítása
- A feldolgozó modul (.NET) és automatizálási szkriptek fejlesztése
- Problémák jelentése, javaslatok tétele (az Issues-ben kifejtve)
- Anyagi támogatás nyújtása a LLM hívásokhoz

Az alábbiakban röviden ismertetjük a főbb hozzájárulási területeket.

---

## 3. Fordítási szabályok, szótár, rendszerprompt fejlesztése

A feldolgozó prompt sablonjai a `src/prompt_templates/` könyvtárban találhatók, szerkezetük a következő:

- `system_prompt_translate_engine.txt`: Globális fordítórendszer rendszerprompt (minden nyelv közös);
- `<nyelvkód>/translation_dictionary_<nyelvkód>.json`: Az adott nyelv szótára;
- `<nyelvkód>/translation_schema_<nyelvkód>.md`: Az adott nyelv fordítási szabályai és stíluskorlátai.

A hozzájárulás lépései:

1. Hozz létre egy alkönyvtárat a nyelvek számára a `src/prompt_templates/` alatt, add hozzá a szótárt és a fordítási szabályfájlokat;
2. Ha módosítani szeretnéd a globális fordítási viselkedést, szerkeszd a `system_prompt_translate_engine.txt` fájlt (figyelem: ez minden nyelvre hatással van);
3. Helyi teszteléssel ellenőrizd a hatást;
4. Nyújts be egy PR-t.

---

## 4. Kézzel ellenőrzött korpusz biztosítása

Ha te egy fordítási mod készítője vagy, és hajlandó vagy biztosítani a fordítási korpuszodat LLM fordítási referenciaként, kérjük, indíts egy Issue-t. A következő információkat kell megadnod:

- A fordítómodulod Mod ID-ja és a fordítás célnyelve;
- A fordítómodulod adminisztrációs oldalának képernyőképe, hogy igazolja, te vagy a mod szerzője;
- Az Issue-ban egyértelműen jelezd, hogy hajlandó vagy biztosítani a fordítási korpuszt;
- Ha különleges körülmények vannak (különleges engedélyek stb.), kérjük, jelezd;
- Győződj meg róla, hogy a biztosított korpusz megfelelő minőségű.

Az engedélyeddel a projekt felveszi a mododat a `config/ref_translation_mods.json` referenciaként szolgáló fordítási modok listájába, és a csővezeték automatikusan szinkronizálja a fordítási szövegedet RAG referenciakorpuszként.

---

## 5. Csővezeték- és eszközfejlesztési hozzájárulás

A projekt automatizálása két részből áll:

**Csővezeték modulok (`src/`, C# / .NET 10)**: 15 szekvenciálisan végrehajtott modult, plusz 2 önálló modult (`WorkshopMonitor` modul felfedező, `DocGenerator` dokumentumgenerátor) tartalmaz, amelyek a SteamCMD inicializálástól, modul letöltéstől, szöveg kivonástól, tartalom ellenőrzéstől, Embedding számítástól, RAG kereséstől az LLM fordításig és a végső kimenetig a teljes folyamatot végzik. Részletek a [technikai referenciában](../technical_reference/technical_reference_hu.md).

**Segéd szkriptek (`.github/`)**: a GitHub automatizálásához.

Ha szeretnéd:

* Kijavítani a meglévő csővezeték-modulok vagy szkriptek hibáit;
* Új funkciókat vagy modulokat hozzáadni a csővezetékhez;
* Optimalizálni a teljesítményt vagy a kód szerkezetét;
* Javítani a prompt sablonokat vagy a RAG stratégiát;

A következő lépéseket követheted:

1. Fork-old ezt a tárolót és klónozd helyben;
2. Hozz létre egy új ágat a legfrissebb ág alapján;
3. Módosíts vagy adj hozzá fájlokat a megfelelő könyvtárakban:
- Csővezeték-modul módosítása → `src/<modulnév>/`;
- CI munkafolyamat módosítása → `.github/workflows/`；
- Prompt sablon módosítása → `src/prompt_templates/`;
4. Kérjük, a beküldés előtt lehetőség szerint:

* Tartsd meg az eredeti kódstílust;
* Adj hozzá szükséges megjegyzéseket;
* Ha lehetséges, mellékelj egyszerű tesztelési vagy használati útmutatót;
5. Nyújtsd be a módosításokat PR-en keresztül, és a leírásban jelezd:

* a változtatás célját;
* az érintett könyvtárakat/modulokat/scripteket;
* hogy tartalmaz-e megszakító változtatást.

---

## 6. Szerzői jogok és licenc megállapodás

> **Fontos megjegyzés:**
> A szerzői jogi és licenc megállapodás célja, hogy védje a projekt, a szerzők, a közreműködők és a játékosok jogos érdekeit, elkerülve a félreértéseket, amelyek a „hallgatólagos egyetértésből” vagy „alapértelmezésből” adódhatnak. Kérjük, figyelmesen olvasd el.
> A szerzői jogokra és licencekre vonatkozóan a README.md fájlban található tartalom az irányadó, ez a szakasz csak közérthetőbb leírást nyújt.

### 6.1 Alapelv: Te megtartod a szerzői jogokat, és egyidejűleg felhatalmazod a projektet a felhasználásra

* Saját alkotásaidra (fordítások, képek, szkriptek/programok stb.) továbbra is szerzői jogod van;
* Azonban ha ezeket a tartalmakat benyújtod a projekthez és elfogadják (egyesítik), akkor beleegyezel, hogy a projekt által használt nyílt forráskódú/megosztási licenc alapján mások számára engedélyezed a felhasználást.

Ez azt jelenti:

* Továbbra **is használhatod** és bemutathatod a munkáidat más helyeken;
* Azonban **nem követelheted** a projekt vagy más, jogszerűen hozzájutott felhasználók számára a „visszavonást” vagy a „történeti verziók törlését” a hozzájárulásod egyesítése után.

### 6.2 Szövegek és képek engedélyezése (CC BY-NC-SA 4.0)

A következő benyújtott tartalmakra vonatkozóan:

* Játék szövegfordítások, finomítások és lektorálások;
* Projekt dokumentáció, magyarázó szövegek;
* Kifejezetten a projekthez készített képek, művészeti erőforrások;

Miután a tárhely elfogadta és egyesítette őket, úgy tekintjük, hogy beleegyezel:

1. A tartalmak a **Nevezd meg! - Ne add el! - Így add tovább! 4.0 Nemzetközi** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, röviden **CC BY-NC-SA 4.0**) licenc alá esnek;
2. A Project Babel és minden felhasználó, aki hozzáfér ezekhez a tartalmakhoz, a **CC BY-NC-SA 4.0 feltételeinek betartásával**:
* megoszthatja, másolhatja, továbboszthatja ezeket a tartalmakat;
* nem kereskedelmi célból módosíthatja és újraalkothatja őket;
3. Beleegyezel, hogy a licenc az alkalmazandó jog által megengedett mértékben **nem kizárólagos, világméretű, jogdíjmentes és visszavonhatatlan**;
4. Még ha később kilépsz vagy beszünteted a projekthez való hozzájárulást, a projekt továbbra is használhatja és újra kiadhatja a CC BY-NC-SA 4.0 alapján azokat a tartalmakat, amelyeket benyújtottál és egyesítettek.

> Ha nem fogadod el a fenti licencelési módot, ne nyújts be szöveges vagy képi jellegű hozzájárulást a projekthez,
> vagy egyeztess előzetesen a projekt fenntartójával, hogy más módon lehetséges-e az együttműködés.

### 6.3 Szkriptek és eszközkódok engedélyezése (GPL-3.0)

Azokra a benyújtott és elfogadott tartalmakra vonatkozóan:

* Automatizációs szkriptek;
* Építési/exportálási eszközök;
* Egyéb programkódok, amelyek e lokalizációs projekt feldolgozására szolgálnak;

Külön nyilatkozat hiányában úgy tekintjük, hogy elfogadod:

1. A kód **GPL-3.0** (GNU Általános Nyilvános Licenc 3. verzió) alatt van licencelve;
2. A projekt fenntartói a GPL-3.0 által engedélyezett keretek között módosíthatják, egyesíthetik és terjeszthetik azt;
3. Te is folytathatsz más projekteket ugyanazon kód alapján, feltéve, hogy betartod a GPL-3.0 feltételeit.

A licencütközések elkerülése érdekében lehetőség szerint:

* Ne vezess be **GPL-3.0-val nem kompatibilis** harmadik féltől származó kódot megerősítés nélkül;
* Ha valóban szükséges egy harmadik fél könyvtárának használata, a PR-ban világosan tüntesd fel annak forrását és licencét, és erősítsd meg a kompatibilitást.

### 6.4 Felsőbb művek és az eredeti játék szerzői joga

Ez a projekt a Project Zomboid modok **nem hivatalos fordítási** projektje:

* Az eredeti játék és az egyes modok szerzői joga a megfelelő szerzőiket/kiadóikat illeti;
* Ez a projekt kizárólag a szöveg fordítására, finomítására és egyes kapcsolódó erőforrások létrehozására és rendezésére irányul;
* A közreműködőknek a tartalom beküldésekor biztosítaniuk kell, hogy:
* Ne másoljanak közvetlenül engedély nélküli harmadik féltől származó lokalizált szövegeket vagy grafikai erőforrásokat;
* Tartsák tiszteletben az eredeti szerzők és modkészítők jogait, és ne terjesszenek jogellenesen.

---

## 7. Kommunikáció és együttműködés

Ha bármilyen kérdésed van:

* Kérdésed van a licencfeltételekkel kapcsolatban;
* Nem vagy biztos abban, hogy egy adott tartalom hozzájárulható-e;
* Speciális módon szeretnéd licencelni a művedet (pl. csak nem kereskedelmi célú felhasználás engedélyezése, de nem engedélyezed a módosítást stb.);

Vedd fel a kapcsolatot a projekt fenntartóival a következő módokon:

* Nyújts be egy Issue-t a megbeszéléshez;
* A fenntartók által nyilvánosan elérhető kapcsolattartási módok.

Igyekszünk a felek jogainak tiszteletben tartása mellett olyan megoldást találni, amely a projekt egészséges fejlődését is szolgálja.

---

## 8. Pénzügyi támogatás

A projekt működése során, mivel új modok jelennek meg, régi modok szövegei frissülnek, folyamatosan szükséges az LLM API hívása a fordításhoz. A LLM viselkedésének szabályozásához az alapvető modszövegeken kívül nagy mennyiségű prompttartalomra is szükség van (beleértve az alap promptokat, fordítási szabályokat, szójegyzéket, bemeneti/kimeneti korlátozásokat, szemantikai lekérdezési eredményeket stb.), amelyek sokkal több tokent fogyasztanak, mint az eredeti szöveg. Ezért a projekt pénzügyi támogatást igényel.

Ha hajlandó vagy pénzügyi támogatást nyújtani, kérjük, vedd fel a kapcsolatot a projekt fenntartóival. Nagyon köszönjük!

---

Még egyszer köszönjük, hogy hajlandó vagy hozzájárulni ehhez a projekthez!
Minden hozzájárulásoddal több játékos profitál!
