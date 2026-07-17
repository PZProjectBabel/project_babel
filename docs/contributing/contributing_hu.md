# Közreműködési útmutató (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Köszönjük, hogy hajlandó hozzájárulni a **Project Babel — a Project Zomboid modok LLM-alapú automatikus fordítási projektjéhez**! Legyen szó hibajavításról, új funkció hozzáadásáról, prompt sablonok írásáról vagy referenciafordítások biztosításáról — minden hozzájárulás számít!

Az LLM API fordításhoz való hívása tokenekbe kerül. Annak érdekében, hogy a projekt hosszú távon fenntarthatóan működjön, nagylelkű támogatását nagyra értékeljük!

> ⚠️ **Fontos figyelmeztetés:**
> Mielőtt bármit beküldene ebbe a tárolóba, feltétlenül olvassa el és értse meg a „Szerzői jog és licencelés" részt.
> A beküldéssel és egyesítéssel Ön elfogadja a megfelelő licencfeltételeket.

---

## Mielőtt elkezdené

Kérjük, olvassa el a projekt `README.md` fájlját, hogy megértse:

- A projekt átfogó céljait és jelenlegi állapotát;
- Hogyan használják a hétköznapi játékosok ezt a projektet (saját tesztjeihez);
- A projekt technikai részleteit.

---

## Hogyan járulhatok hozzá?

Érdeklődése és készségei alapján egy vagy több módon is részt vehet:

- Fordítási szabályok biztosítása egy célnyelvhez
- Terminológiai szótár biztosítása egy célnyelvhez
- A rendszerpromptok javítása
- Manuálisan javított fordítási korpuszok biztosítása
- A pipeline modulok (.NET) és automatizálási szkriptek javítása
- Problémák jelentése és fejlesztési javaslatok (Issue-kon keresztül)
- Pénzügyi támogatás nyújtása az LLM API hívásokhoz

Az alábbiakban a fő hozzájárulási forgatókönyvek magyarázata található.

---

## Fordítási szabályok, terminológiai szótárak biztosítása és a rendszerpromptok javítása

A pipeline prompt sablonjai a `src/prompt_templates/` mappában találhatók, a következő struktúrával:

- `system_prompt_translate_engine.txt`: a globális fordítómotor rendszerpromptja (minden nyelv közös);
- `<nyelvkód>/translation_dictionary_<nyelvkód>.json`: az adott nyelv terminológiai szótára;
- `<nyelvkód>/translation_schema_<nyelvkód>.md`: az adott nyelv fordítási szabályai és stíluskorlátai.

Hozzájárulási lépések:

1. Hozzon létre egy alkönyvtárat a `src/prompt_templates/` alatt a nyelvéhez, és adja hozzá a szótár- és szabályfájlokat;
2. Ha módosítania kell a globális fordítási viselkedést, módosítsa a `system_prompt_translate_engine.txt` fájlt (figyelem: ez minden nyelvet érint);
3. Tesztelje helyben az eredmények megerősítéséhez;
4. Küldjön be egy PR-t.

---

## Manuálisan javított korpuszok biztosítása

Ha Ön egy fordítási mod szerzője, és hajlandó biztosítani fordítási korpuszát LLM fordítási referenciaként, kérjük, nyújtson be kérelmet Issue-n keresztül. A következő információkat kell megadnia:

- A fordítási mod Mod ID-ja és a célnyelv;
- Képernyőkép a fordítási mod adminisztrációs oldaláról a szerzőség igazolására;
- Egyértelmű nyilatkozat az Issue-ban, hogy hajlandó biztosítani a fordítási korpuszt;
- Különleges körülmények esetén (különleges licenc stb.), kérjük, magyarázza el;
- Kérjük, győződjön meg arról, hogy a biztosított korpusz magas színvonalú.

Az Ön engedélyével a projekt hozzáadja a modját a referenciafordítási modok listájához (`config/ref_translation_mods.json`), és a pipeline automatikusan szinkronizálja a lefordított szövegeit RAG referencia korpuszként.

---

## Pipeline és eszközfejlesztési hozzájárulások

A projekt automatizálása két részre oszlik:

**Pipeline modulok (`src/`, C# / .NET 10)**: 15 szekvenciálisan végrehajtott modult tartalmaz, amelyek a modok letöltésétől, a szöveg kinyerésétől, a tartalom ellenőrzésétől, az embedding számítástól, a RAG visszakereséstől az LLM fordításig és a végső kimenetig terjedő teljes munkafolyamatért felelnek. A részletekért lásd a [műszaki referenciát](../technical_reference/technical_reference_hu.md).

**Segéd szkriptek (`.github/`)**: A GitHub automatizáláshoz használatosak.

Ha az alábbiakat szeretné:

* Hibák javítása a meglévő pipeline modulokban vagy szkriptekben;
* Új funkciók vagy modulok hozzáadása a pipeline-hoz;
* A teljesítmény vagy a kódstruktúra optimalizálása;
* A prompt sablonok vagy a RAG stratégiák javítása;

A következő lépéseket követheti:

1. Forkolja ezt a tárolót és klónozza helyben;
2. Hozzon létre egy új ágat a legfrissebb ágból;
3. Módosítsa vagy adja hozzá a fájlokat a megfelelő könyvtárakban:
   - Pipeline modul változtatások → `src/<modul_név>/`;
   - Szkript változtatások → `scripts/`;
   - Prompt sablon változtatások → `src/prompt_templates/`;
4. Beküldés előtt lehetőleg:

   * Tartsa meg a meglévő kódstílust;
   * Adja hozzá a szükséges megjegyzéseket;
   * Ha lehetséges, csatoljon egyszerű teszteket vagy használati utasításokat;
5. Küldje be a változtatásokat PR-en keresztül, és magyarázza el a leírásban:

   * A változtatások célját;
   * Az érintett könyvtárakat / modulokat / szkripteket;
   * Hogy tartalmaz-e kompatibilitást törő változtatásokat.

---

## Szerzői jog és licencelés

> **Baráti emlékeztető:**
> A szerzői jogi és licencfeltételek a projekt, a szerzők, a közreműködők és a játékosok jogos jogainak és érdekeinek védelmét szolgálják, valamint a „hallgatólagos megállapodásokból" vagy „alapértelmezett feltételezésekből" eredő félreértések elkerülését. Kérjük, figyelmesen olvassa el őket.
> A szerzői jog és a licencelés a README.md fájl tartalma szerint szabályozott; ez a szakasz csak egy közérthetőbb leírást nyújt.

### 1. Alapelv: Ön megtartja a szerzői jogot, miközben licenceli a projektet a műve használatára

* Ön továbbra is birtokolja a szerzői jogot az Ön által létrehozott tartalom felett (fordítások, képek, szkriptek/programok stb.);
* Azonban, miután ezt a tartalmat beküldte ebbe a projektbe és elfogadták (egyesítették),
  Ön elfogadja, hogy másoknak licenceli e tartalom használatát a projekt által elfogadott nyílt forráskódú/megosztott licenc alapján.

Ez azt jelenti:

* Ön **továbbra is** használhatja és megjelenítheti művét máshol;
* De **nem követelheti** a hozzájárulás egyesítése után, hogy ez a projekt vagy más felhasználók, akik jogszerűen megszerezték a művet, „vonják vissza a licencet" vagy „töröljék a korábbi verziókat".

### 2. Szövegek, képek és hasonló tartalom licencelése (CC BY-NC-SA 4.0)

Az Ön által beküldött következő tartalomra:

* Játékszövegek fordításai, stiláris javításai és korrektúrája;
* Projektdokumentáció és magyarázó szövegek;
* Kifejezetten ehhez a projekthez készített képek és művészeti erőforrások;

Amint elfogadták és egyesítették ebben a tárolóban, úgy tekintjük, hogy Ön elfogadja:

1. Ezek a tartalmak a **Nevezd meg! - Ne add el! - Így add tovább! 4.0 Nemzetközi**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, röviden **CC BY-NC-SA 4.0**) licenc alatt állnak;
2. A Project Babel és minden felhasználó, aki megkapja ezt a tartalmat, a **CC BY-NC-SA 4.0 feltételeinek betartásával**:

   * Megoszthatja, másolhatja és továbbterjesztheti ezt a tartalmat;
   * Módosíthatja és származékos műveket hozhat létre nem kereskedelmi célokra;
3. Ön elfogadja, hogy a vonatkozó jog által megengedett mértékben ez a licenc **nem kizárólagos, világméretű, jogdíjmentes és visszavonhatatlan**;
4. Még akkor is, ha később kilép vagy abbahagyja a részvételt ebben a projektben, a projekt továbbra is használhatja és továbbterjesztheti az Ön által beküldött és egyesített vonatkozó tartalmat a CC BY-NC-SA 4.0 alapján.

> Ha nem fogadja el a fenti licencfeltételeket, kérjük, ne küldjön szöveges vagy képi hozzájárulást ehhez a projekthez,
> vagy előzetesen egyeztessen a projekt karbantartóival, hogy más módon lehetséges-e az együttműködés.

### 3. Szkriptek és eszközkód licencelése (GPL-3.0)

Az Ön által beküldött és elfogadott következőkre:

* Automatizálási szkriptek;
* Build/export eszközök;
* A fordítási projekt feldolgozásához használt egyéb programkód;

Külön nyilatkozat hiányában úgy tekintjük, hogy Ön elfogadja:

1. A kód **GPL-3.0** (GNU General Public License 3. verzió) licenc alatt áll;
2. A projekt karbantartói módosíthatják, egyesíthetik és terjeszthetik a GPL-3.0 által megengedett kereteken belül;
3. Ön is folytathat más projekteket ugyanazon kód alapján, amennyiben betartja a GPL-3.0 feltételeit.

A licencütközések elkerülése érdekében lehetőleg:

* Ne vezessen be **GPL-3.0-val nem kompatibilis** harmadik féltől származó kódot előzetes megerősítés nélkül;
* Ha harmadik féltől származó könyvtárakra kell hivatkoznia, egyértelműen tüntesse fel azok forrását és licencét a PR-ben, és erősítse meg a kompatibilitást.

### 4. Eredeti művek és az eredeti játék szerzői joga

Ez a projekt a *Project Zomboid*hoz kapcsolódó modok **nem hivatalos fordítási** projektje:

* Az eredeti játék és az egyes modok szerzői joga a megfelelő szerzőket/kiadókat illeti;
* Ez a projekt csak a szövegfordítások, stiláris módosítások és néhány kísérő erőforrás létrehozását és rendszerezését foglalja magában;
* A közreműködőknek a tartalom beküldésekor biztosítaniuk kell:

  * Hogy ne másolják közvetlenül a jogosulatlan harmadik féltől származó fordítási szövegeket vagy művészeti erőforrásokat;
  * Hogy tiszteletben tartsák az eredeti szerzők és a modszerzők jogait, és ne végezzenek jogsértő továbbterjesztést.

---

## Kommunikáció és együttműködés

Ha:

* Kérdései vannak a licencfeltételekkel kapcsolatban;
* Bizonytalan abban, hogy egy bizonyos tartalom hozzájárulható-e;
* Különleges módon szeretné licencelni a művét (pl. csak nem kereskedelmi használat, adaptáció nem engedélyezett);

Forduljon bizalommal a projekt karbantartóihoz:

* Issue beküldése megbeszéléshez;
* A karbantartók egyéb nyilvánosan elérhető kapcsolattartási módjai.

Mindent megteszünk annak érdekében, hogy olyan megoldást találjunk, amely egyensúlyt teremt a projekt egészséges fejlődése és valamennyi fél jogainak és érdekeinek tiszteletben tartása között.

---

## Pénzügyi támogatás

A projekt működése során az új modok hozzáadása és a meglévő modok szövegfrissítései miatt folyamatosan hívni kell az LLM API-t a fordításhoz. Az LLM viselkedésének korlátozásához a modok alapszövegein kívül nagy mennyiségű prompt tartalomra van szükség (beleértve az alap promptokat, fordítási szabályokat, terminológiai táblákat, bemeneti/kimeneti korlátozásokat, szemantikus keresési eredményeket stb.), ami jóval több tokent fogyaszt, mint az eredeti szövegek. Ezért a projektnek pénzügyi támogatásra van szüksége.

Ha pénzügyi támogatást szeretne nyújtani, kérjük, forduljon a projekt karbantartóihoz. Nagyon köszönjük!

---

Még egyszer köszönjük, hogy hajlandó hozzájárulni ehhez a projekthez!
Minden hozzájárulása több játékos javát szolgálja!
