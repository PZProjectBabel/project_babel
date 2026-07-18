# Project Babel — Project Zomboid -moduulin LLM-automaattikäännösprojekti

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Tämä käännösprojekti on [Project Babel](https://github.com/PZProjectBabel/project_babel) -työkalusarjan ohjaama ja ylläpitämä.*

---

## Sisällysluettelo

- [Projektin tukemat kohdekielet](#projektin-tukemat-kohdekielet)
- [Asentaminen ja käyttö](#asentaminen-ja-käyttö)
- [Käännöksen edistyminen](#käännöksen-edistyminen)
- [Kuinka osallistua](#kuinka-osallistua)
- [Työkalut ja hakemistorakenne (kehittäjille)](#työkalut-ja-hakemistorakenne-kehittäjille)
  - [Projektihakemisto](#projektihakemisto)
  - [Putkimoduulit (suoritusjärjestyksessä)](#putkimoduulit-suoritusjärjestyksessä)
  - [Itsenäiset moduulit](#itsenäiset-moduulit)
  - [Teknologiapino](#teknologiapino)
- [Tekijänoikeus ja lisenssi](#tekijänoikeus-ja-lisenssi)
  - [1. Tekstit ja kuvat jne.](#1-tekstit-ja-kuvat-jne)
  - [2. Ohjelmat, skriptit ja muut kehityssisällöt](#2-ohjelmat-skriptit-ja-muut-kehityssisällöt)
- [Kiitokset](#kiitokset)
- [Kolmannen osapuolen ohjelmat](#kolmannen-osapuolen-ohjelmat)

---

## Projektin tukemat kohdekielet

| Kieli | Paikallinen nimi | Kansainvälinen koodi | Pelin sisäinen koodi | Tuki | Huomautus |
|------|------|------|------|------|------|
| Arabia | العربية | `ar` | `AR` | ❌ | Token-raja ylitetty |
| Katalaani | català | `ca` | `CA` | ❌ | Token-raja ylitetty |
| Perinteinen kiina | 繁體中文 | `zh-hant` | `CH` | ❌ | Token-raja ylitetty |
| Yksinkertaistettu kiina | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tšekki | čeština | `cs` | `CS` | ❌ | Token-raja ylitetty |
| Tanska | dansk | `da` | `DA` | ❌ | Token-raja ylitetty |
| Saksa | Deutsch | `de` | `DE` | ✅ | |
| Englanti | English | `en` | `EN` | ✅ | |
| Espanja | español | `es` | `ES` | ❌ | Token-raja ylitetty |
| Suomi | suomi | `fi` | `FI` | ❌ | Token-raja ylitetty |
| Ranska | français | `fr` | `FR` | ✅ | |
| Unkari | magyar | `hu` | `HU` | ❌ | Token-raja ylitetty |
| Indonesia | Bahasa Indonesia | `id` | `ID` | ❌ | Token-raja ylitetty |
| Italia | italiano | `it` | `IT` | ❌ | Token-raja ylitetty |
| Japani | 日本語 | `ja` | `JP` | ✅ | |
| Korea | 한국어 | `ko` | `KO` | ❌ | Token-raja ylitetty |
| Hollanti | Nederlands | `nl` | `NL` | ❌ | Token-raja ylitetty |
| Norja | norsk | `no` | `NO` | ❌ | Token-raja ylitetty |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Token-raja ylitetty |
| Puola | polski | `pl` | `PL` | ❌ | Token-raja ylitetty |
| Portugali (Portugali) | português | `pt` | `PT` | ❌ | Token-raja ylitetty |
| Portugali (Brasilia) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token-raja ylitetty |
| Romania | română | `ro` | `RO` | ❌ | Token-raja ylitetty |
| Venäjä | русский | `ru` | `RU` | ❌ | Token-raja ylitetty |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Token-raja ylitetty |
| turkki | Türkçe | `tr` | `TR` | ❌ | Token-raja on liian matala |
| ukraina | українська | `uk` | `UA` | ❌ | Token-raja on liian matala |

**Yhteensä**: 27 suunniteltua kieltä | **Tuettu**: 5 kieltä | **Odottamassa**: 22 kieltä

---

## Asentaminen ja käyttö

Tämä on opas pelaajille, jotka haluavat käyttää tätä käännösprojektia suoraan pelissä.

1.  Mene Steam-työpajasivullemme: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Napsauta "Tilaa"-painiketta.
3.  Käynnistä peli ja ota tämä käännösmoduuli käyttöön pelin päävalikon "Modit"-hallinnassa.
4.  Myöhemmin käyttöön otettujen moduulien käännöstekstit korvaavat aiemmin käyttöön otetut, joten tämä käännösmoduuli tulee ottaa käyttöön toimintamoduulien jälkeen (mahdollisimman alhaalla).
5.  Nauti pelistä!

---

## Käännöksen edistyminen

**[➡️ Napsauta tästä nähdäksesi käännöksen edistymisen](./docs/progress/progress_fi.md)**

---

## Kuinka osallistua

Toivotamme kaikki tervetulleiksi osallistumaan, olipa kyseessä virheen korjaaminen, uuden ominaisuuden lisääminen, kehoitemallin kirjoittaminen tai referenssikäännöksen tarjoaminen!

LLM API:n käyttö käännöksiin maksaa tokeneista. Jotta projekti voi toimia pitkällä aikavälillä, toivomme teidän antavan anteliaasti apuanne!

Lisätietoja löydät [Osallistumisoppaasta](./docs/contributing/contributing_fi.md)

---

## Työkalut ja hakemistorakenne (kehittäjille)

Tämä osio on tarkoitettu kehittäjille, jotka haluavat ymmärtää projektin automaation periaatteet.

### Projektihakemisto

| Hakemisto | Kuvaus |
|------|------|
| `src/` | .NET 10 -käännösputkilähdekoodi, sisältää 15 moduulia + 2 itsenäistä moduulia |
| `config/` | Käännösputken asetustiedosto (LLM-, Steam-, RAG-parametrit jne.) |
| `data/` | Ajonaikaiset tiedot: moduulien metadata, upotukset, käännösvälimuisti |
| `translation_ref/` | Referenssikäännöstiedot (kuten As1:n valtuuttamat moduulit), tarjoaa LLM:lle käännösviitteitä |
| `base_game_keys/` | Pelin perusavaimet, joita käytetään päällekkäisyyksien poistamiseen ja alkuperäisen tekstin ylikirjoittamisen estämiseen |
| `final_outputs/` | Lopulliset tulosteet: `project_babel/`-moduulipaketti, `icons/`-kuvakkeet ja `workshop_descriptions/`-työpajakuvaukset |
| `docs/` | Projektin dokumentaatio: edistymisraportit, osallistumisopas, putken kuvaus |
| `temp/` | Käännösputken väliaikaistiedostot (jokainen ajo omassa hakemistossaan) |
| `src/prompt_templates/` | LLM-kehoitemallit (käännös/sisällöntarkastus) |

### Putkimoduulit (suoritusjärjestyksessä)

| Vaihe | Moduuli | Toiminto |
|------|------|------|
| 1 | `ConfigReader` | Lataa asetukset/avaimet/kieliluettelo |
| 2 | `RepoDataLoader` | Lataa viitekäännökset ja käännösvälimuisti |
| 3 | `ModIdCollector` | Kerää Workshop-modien ID:t |
| 4 | `ModInfoFetcher` | Hae Steam-metatiedot |
| 5 | `SteamCmdBootstrapper` | Valmistele nykyisen alustan steamcmd-ajoympäristö |
| 6 | `ModDownloader` | Lataa modit steamcmd:n kautta |
| 7 | `ContentExtractor` | Pura modien käännöstiedostot → `TranslationEntry` |
| 8 | `ContentChecker` | Sisällön turvallisuustarkistus (huumeet/porno/väkivalta) |
| 9 | `EmbeddingFetcher` | Laske tekstien embedding-vektorit |
| 10 | `TranslationBatcher` | Luo kieliriippumattomia käännöseriä |
| 11 | `RagContextRetriever` | Hae RAG-konteksti (tarkka avain + embedding-samankaltaisuus) |
| 12 | `LLMTranslator` | Kutsu LLM:ää suorittamaan käännös |
| 13 | `ResultWriter` | Kirjoita data/- ja translation_ref/-kansioihin |
| 14 | `FinalOutputWriter` | Luo lopullinen PZ-moduuliformaatin tuloste |
| 15 | `ProgressReporter` | Luo edistymisraportti |

### Itsenäiset moduulit

| Moduuli | Toiminto |
|------|------|
| `WorkshopMonitor` | Hakee säännöllisesti uusia Steam Workshop -modeja, suodattaa tilausmäärän perusteella ja lisää tiedostoon `request_for_translation.txt` |
| `DocGenerator` | LLM-ohjattu monikielinen dokumentaatiogeneraattori |

### Teknologiapino

- **Kieli**: C# (.NET 10)
- **Kohdealusta**: GitHub Actions Linux x64 runner
- **Testaus**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfiguroitava)
- **Embedding**: Tekstivektorointi RAG-samankaltaisuushakuun
- **Sisällöntarkistus**: LLM-pohjainen monitasoinen turvallisuustarkastus

Yksityiskohtainen [tekninen viite](./docs/technical_reference/technical_reference_fi.md).

---

## Tekijänoikeus ja lisenssi

Tämän käännösprojektin käännöstekstit ja liittyvät kuvat on luonut tai toissijaisesti luonut **Project Babel** ja osallistujat alkuperäisten pelimodien pohjalta.

© 2025 Project Babel ja kirjoittajat pidättävät kaikki oikeudet.

### 1. Tekstit ja kuvat jne.

Ellei toisin mainita, tässä arkistossa:

- Pelin sisäiset tekstit, käännökset, viimeistely ja oikoluku;
Projektin selitysdokumentit, modin sisäiset tekstikäännökset;
Tämän projektin erityisesti luodut kuvat ja taideresurssit

kaikki on lisensoitu **Nimeä-EiKaupallinen-JaaSamoin 4.0 Kansainvälinen** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, lyhenne **CC BY-NC-SA 4.0**) -lisenssillä.

Tämä tarkoittaa, että voit vapaasti jakaa ja muokata näitä sisältöjä seuraavien ehtojen mukaisesti:

- **Nimeä (BY)**: Ilmoita selkeästi "Tämä käännösprojekti perustuu Project Babelin työhön" ja liitä linkki tähän repoon ja Steam-työpajaan `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **EiKaupallinen (NC)**: Älä käytä tämän projektin sisältöjä tai niiden muunnoksia mihinkään suoraan tai epäsuoraan kaupalliseen tarkoitukseen (mukaan lukien mutta ei rajoittuen maksullisiin paketteihin, maksullisiin latauksiin, mainostuloihin jne.).
- **JaaSamoin (SA)**: Jos muokkaat tai luot uudelleen tämän projektin sisältöjä, sinun on julkaistava muutetut versiot **samalla CC BY-NC-SA 4.0 -lisenssillä**.

Lisätietoja tästä lisenssistä on saatavilla:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.fi>

*Erityisiä huomautuksia:*
- *base_game_keys-kansion sisältö on peräisin pelin perusversiosta, tekijänoikeus kuuluu pelin kehittäjälle! Sisältö estää käännösavainten päällekkäisyyden pelin avaimiin (deduplikointi)*
- *translation_ref-kansion sisältöä käytetään LLM:lle käännösviitteenä, tekijänoikeus kuuluu kunkin modin kehittäjälle!*

### 2. Ohjelmat, skriptit ja muut kehityssisällöt

Ellei lähdekooditiedostossa tai -hakemistossa ole toisin mainittu, tämän repon ohjelmakoodi, jota käytetään käännössisältöjen luomiseen/pakkaamiseen/käsittelyyn (esim. `src/`-hakemiston koodi), on lisensoitu **GNU General Public License Version 3 (GPL-3.0)** -lisenssillä.

Täydelliset ehdot löytyvät tämän repon juurihakemiston `LICENSE`-tiedostosta (GPL-3.0) tai GNU:n verkkosivuilta: <https://www.gnu.org/licenses/gpl-3.0.html>

---

## Kiitokset

Tämä projekti käyttää kolmannen osapuolen modeja kohdekielen käännösten viiteteksteinä. Viitetekstit lähetetään LLM:lle käännösviitteenä.

| Viitemodin nimi | Tekijä | Modin sivu |
|------|------|------|
| [B42] Yhdistetty·Kiinan käännös | Ruyi käännösryhmä (As1) | [Työpajan sivu](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Yhdistetty·Modin käännös | Ruyi käännösryhmä (As1) | [Työpajan sivu](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Yhdistetty·Arkin käännös | Ruyi käännösryhmä (As1) | [Työpajan sivu](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Esitämme sydämelliset kiitokset yllä mainituille tekijöille!**

---

## Kolmannen osapuolen ohjelmat

Tämä projekti käyttää kolmannen osapuolen ohjelmia ja kirjastoja, joiden tekijänoikeudet kuuluvat niiden kehittäjille.

