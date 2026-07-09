# Project Babel — PZ-modien automaattinen LLM-käännös

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Huom:** Tätä käännöstä ei vielä tueta. Virallinen sisältö on [kiinankielinen versio](../../README.md).

---

*Tätä käännösprojektia ylläpitää [Project Babel](https://github.com/PZProjectBabel/project_babel) -työkalu.*

---

## Sisällysluettelo

- [Tuetut kohdekielet](#tuetut-kohdekielet)
- [Asennus ja käyttö](#asennus-ja-käyttö)
- [Käännöksen edistyminen](#käännöksen-edistyminen)
- [Osallistuminen](#osallistuminen)
- [Työkalut ja hakemistorakenne (kehittäjille)](#työkalut-ja-hakemistorakenne-(kehittäjille))
- [Tekijänoikeus ja lisenssi](#tekijänoikeus-ja-lisenssi)
- [Kiitokset](#kiitokset)
- [Kolmannen osapuolen ohjelmistot](#kolmannen-osapuolen-ohjelmistot)

---

## Tuetut kohdekielet

| Kieli | Paikallinen nimi | ISO-koodi | Pelin koodi | Tuettu | Huomautus |
|------|------|------|------|------|------|
| Arabia | العربية | `ar` | `AR` | ❌ | Token-kiintiö riittämätön |
| Katalaani | català | `ca` | `CA` | ❌ | Token-kiintiö riittämätön |
| Perinteinen kiina | 繁體中文 | `zh-hant` | `CH` | ❌ | Token-kiintiö riittämätön |
| Yksinkertaistettu kiina | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tšekki | čeština | `cs` | `CS` | ❌ | Token-kiintiö riittämätön |
| Tanska | dansk | `da` | `DA` | ❌ | Token-kiintiö riittämätön |
| Saksa | Deutsch | `de` | `DE` | ✅ | |
| Englanti | English | `en` | `EN` | ✅ | |
| Espanja | español | `es` | `ES` | ❌ | Token-kiintiö riittämätön |
| Suomi | suomi | `fi` | `FI` | ❌ | Token-kiintiö riittämätön |
| Ranska | français | `fr` | `FR` | ✅ | |
| Unkari | magyar | `hu` | `HU` | ❌ | Token-kiintiö riittämätön |
| Indonesia | Bahasa Indonesia | `id` | `ID` | ❌ | Token-kiintiö riittämätön |
| Italia | italiano | `it` | `IT` | ❌ | Token-kiintiö riittämätön |
| Japani | 日本語 | `ja` | `JP` | ✅ | |
| Korea | 한국어 | `ko` | `KO` | ❌ | Token-kiintiö riittämätön |
| Hollanti | Nederlands | `nl` | `NL` | ❌ | Token-kiintiö riittämätön |
| Norja | norsk | `no` | `NO` | ❌ | Token-kiintiö riittämätön |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Token-kiintiö riittämätön |
| Puola | polski | `pl` | `PL` | ❌ | Token-kiintiö riittämätön |
| Portugali (Portugali) | português | `pt` | `PT` | ❌ | Token-kiintiö riittämätön |
| Portugali (Brasilia) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token-kiintiö riittämätön |
| Romania | română | `ro` | `RO` | ❌ | Token-kiintiö riittämätön |
| Venäjä | русский | `ru` | `RU` | ❌ | Token-kiintiö riittämätön |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Token-kiintiö riittämätön |
| Turkki | Türkçe | `tr` | `TR` | ❌ | Token-kiintiö riittämätön |
| Ukraina | українська | `uk` | `UA` | ❌ | Token-kiintiö riittämätön |

**Yhteensä**: 27 suunniteltua kieltä | **Tuettu**: 5 | **Odottaa**: 22

---

## Asennus ja käyttö

Opas pelaajille, jotka haluavat käyttää käännöspakettia pelissä.

1. Mene Steam Workshop -sivulle: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Napsauta "Tilaa".
3. Käynnistä peli ja ota tämä käännösmodi käyttöön Modit-valikossa.
4. Myöhemmin ladattujen modien käännösteksti ohittaa aiemmat, joten tämän käännösmodin on latauduttava pelimodien jälkeen.
5. Nauti!

---

## Käännöksen edistyminen

[➡️ Käännöksen edistyminen](../progress/progress_fi.md)

---

## Osallistuminen

Otamme vastaan avustuksia! Käännöskorjauksia, uusia ominaisuuksia, kehotemalleja tai viitekäännöksiä.

LLM API -kutsut käännöksiin aiheuttavat token-kustannuksia. Tukesi auttaa projektia toimimaan kestävästi!

Read the [Contributing Guide](../contributing/contributing_fi.md) for details.

---

## Työkalut ja hakemistorakenne (kehittäjille)

Tämä osio on suunnattu kehittäjille, jotka haluavat ymmärtää projektin automaation sisäisen toiminnan.

### Projektihakemistot

| Hakemisto | Kuvaus |
|------|------|
| `src/` | .NET 10 käännösputken lähdekoodi, 15 moduulia |
| `config/` | Putken konfiguraatio (LLM, Steam, RAG-parametrit jne.) |
| `data/` | Ajonaikaiset tiedot: mod-metatiedot, upotukset, käännösvälimuisti |
| `translation_ref/` | Viitekäännökset LLM-kontekstina |
| `base_game_keys/` | Peruspelin käännösavaimet deduplikointiin |
| `final_outputs/` | Lopullinen PZ-mod-muotoinen käännöstuloste |
| `docs/` | Dokumentaatio: edistyminen, osallistuminen, putken tekniset tiedot |
| `temp/` | Putken väliaikaiset tiedostot |
| `src/prompt_templates/` | LLM-kehotesabluunat |

### Putken moduulit (suoritusjärjestys)

| Vaihe | Moduuli | Toiminto |
|------|------|------|
| 1 | `ConfigReader` | Lataa konfiguraatio/salaisuudet/kielet |
| 2 | `RepoDataLoader` | Lataa viitteet ja käännösvälimuisti |
| 3 | `ModIdCollector` | Kerää Workshop-modien ID:t |
| 4 | `ModInfoFetcher` | Hae Steam-metatiedot |
| 5 | `ModDownloader` | Lataa modit steamcmd:n kautta |
| 6 | `ContentExtractor` | Jäsennä modien käännöstiedostot → `TranslationEntry` |
| 7 | `ContentChecker` | Sisällön turvallisuustarkastus |
| 8 | `EmbeddingFetcher` | Laske tekstin upotusvektorit |
| 9 | `TranslationBatcher` | Luo käännöserät |
| 10 | `RagContextRetriever` | Hae RAG-kontekstit |
| 11 | `LLMTranslator` | Suorita LLM-käännös |
| 12 | `ResultWriter` | Kirjoita data/- ja translation_ref/-kansioihin |
| 13 | `FinalOutputWriter` | Generoi lopullinen PZ-mod-tuloste |
| 14 | `ProgressReporter` | Generoi edistymisraportit |

### Teknologiapino

- **Kieli**: C# (.NET 10)
- **Kohdealusta**: GitHub Actions Linux x64 runner
- **Testit**: xUnit (Windows x64)
- **LLM**: DeepSeek API (muokattavissa)
- **Embedding**: Tekstivektorointi RAG-samankaltaisuushakuun
- **Sisällön tarkistus**: LLM-ohjattu monitasoinen turvallisuustarkistus

Yksityiskohtainen tekninen dokumentaatio: [TranslationEntry-putki](../pipeline/translation_entry_pipeline_fi.md)

---

## Tekijänoikeus ja lisenssi

© 2025 Project Babel ja kaikki tekijät. Kaikki oikeudet pidätetään.

### Sisältö (tekstit, kuvat)

Lisensoitu **CC BY-NC-SA 4.0** -lisenssillä.

- **Nimeä**: Mainitse "Project Babel" -pohjaiset muutokset, liitä repo- ja Workshop-linkit
- **Ei-kaupallinen**: Kaupallinen käyttö kielletty
- **JaaSamoin**: Muutokset on julkaistava samalla lisenssillä

### Koodi

`src/`-kansion koodi on lisensoitu **GPL-3.0**-lisenssillä.

---

## Kiitokset

| Viitemodi | Tekijä | Sivu |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Syvä kiitollisuus yllä oleville tekijöille!**

---

## Kolmannen osapuolen ohjelmistot

Tämä projekti käyttää kolmannen osapuolen ohjelmia ja kirjastoja, joiden tekijänoikeudet kuuluvat niiden kehittäjille.
