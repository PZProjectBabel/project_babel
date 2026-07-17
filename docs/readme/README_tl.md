# Project Babel — Proyektong Awtomatikong Pagsasalin ng LLM para sa Mod ng Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Ang proyektong pagsasaling ito ay pinapatakbo at pinapanatili ng toolset [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Talaan ng Nilalaman

- [Mga Target na Wika ng Pagsasalin na Suportado ng Proyekto](#mga-target-na-wika-ng-pagsasalin-na-suportado-ng-proyekto)
- [Paano I-install at Gamitin](#paano-i-install-at-gamitin)
- [Pag-unlad ng Pagsasalin](#pag-unlad-ng-pagsasalin)
- [Paano Mag-ambag](#paano-mag-ambag)
- [Mga Tool at Istruktura ng Direktoryo (Para sa mga Developer)](#mga-tool-at-istruktura-ng-direktoryo-para-sa-mga-developer)
  - [Direktoryo ng Proyekto](#direktoryo-ng-proyekto)
  - [Mga Module ng Pipeline (Ayon sa pagkakasunud-sunod ng pagpapatakbo)](#mga-module-ng-pipeline-ayon-sa-pagkakasunud-sunod-ng-pagpapatakbo)
  - [Teknolohiyang Stack](#teknolohiyang-stack)
- [Copyright at Lisensya](#copyright-at-lisensya)
  - [1. Mga tekstong, larawan, at iba pa](#1-mga-tekstong-larawan-at-iba-pa)
  - [2. Programa, Script, at Iba Pang Nilalaman ng Pag-unlad](#2-programa-script-at-iba-pang-nilalaman-ng-pag-unlad)
- [Pasasalamat](#pasasalamat)
- [Third-Party na Programa](#third-party-na-programa)

---

## Mga Target na Wika ng Pagsasalin na Suportado ng Proyekto

| Wika | Pangalan sa Lokal | Internasyonal na Kodigo | Kodigo sa Laro | Suportado | Talaan |
|------|------|------|------|------|------|
| Arabic | العربية | `ar` | `AR` | ❌ | Hindi sapat ang token |
| Catalan | català | `ca` | `CA` | ❌ | Hindi sapat ang token |
| Traditional Chinese | 繁體中文 | `zh-hant` | `CH` | ❌ | Hindi sapat ang token |
| Simplified Chinese | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Czech | čeština | `cs` | `CS` | ❌ | Hindi sapat ang token |
| Danish | dansk | `da` | `DA` | ❌ | Hindi sapat ang token |
| German | Deutsch | `de` | `DE` | ✅ | |
| English | English | `en` | `EN` | ✅ | |
| Spanish | español | `es` | `ES` | ❌ | Hindi sapat ang token |
| Finnish | suomi | `fi` | `FI` | ❌ | Hindi sapat ang token |
| French | français | `fr` | `FR` | ✅ | |
| Hungarian | magyar | `hu` | `HU` | ❌ | Hindi sapat ang token |
| Indonesian | Bahasa Indonesia | `id` | `ID` | ❌ | Hindi sapat ang token |
| Italian | italiano | `it` | `IT` | ❌ | Hindi sapat ang token |
| Japanese | 日本語 | `ja` | `JP` | ✅ | |
| Korean | 한국어 | `ko` | `KO` | ❌ | Hindi sapat ang token |
| Dutch | Nederlands | `nl` | `NL` | ❌ | Hindi sapat ang token |
| Norwegian | norsk | `no` | `NO` | ❌ | Hindi sapat ang token |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Hindi sapat ang token |
| Polish | polski | `pl` | `PL` | ❌ | Hindi sapat ang token |
| Portuguese (Portugal) | português | `pt` | `PT` | ❌ | Hindi sapat ang token |
| Portuguese (Brazil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Hindi sapat ang token |
| Romanian | română | `ro` | `RO` | ❌ | Hindi sapat ang token |
| Russian | русский | `ru` | `RU` | ❌ | Hindi sapat ang token |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Hindi sapat ang token |
| Turko | Türkçe | `tr` | `TR` | ❌ | Kulang ang token allowance |
| Ukranyo | українська | `uk` | `UA` | ❌ | Kulang ang token allowance |

**Kabuuan**: 27 planong wika | **Nasuportahan**: 5 | **Susuportahan**: 22

---

## Paano I-install at Gamitin

Ito ay gabay para sa mga manlalaro na nais direktang gamitin ang proyektong pagsasalin na ito sa laro.

1.  Pumunta sa aming Steam Workshop page: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  I-click ang pindutang "Subscribe".
3.  Ilunsad ang laro, at sa pangunahing menu, sa pamamahala ng "Mods", paganahin ang mod na ito ng pagsasalin.
4.  Ang mga pagsasalin ng mod na na-activate mamaya ay uunahin kaysa sa naunang na-activate, kaya ang mod na ito ng pagsasalin ay dapat i-activate pagkatapos ng mga functional mod (ilagay sa ilalim hangga't maaari).
5.  Masiyahan sa laro!

---

## Pag-unlad ng Pagsasalin

**[➡️ I-click dito upang tingnan ang pag-unlad ng pagsasalin](./docs/progress/progress_tl.md)**

---

## Paano Mag-ambag

Tinatanggap namin ang sinuman na gustong mag-ambag, maging ito ay pagwawasto ng pagkakamali, pagdagdag ng bagong feature, pagsulat ng prompt template, o pagbibigay ng reference na pagsasalin!

Ang pagtawag sa LLM API para sa pagsasalin ay nangangailangan ng bayad para sa mga token. Upang mapanatili ang proyekto na tumakbo nang matagal at matatag, umaasa kaming makakapagbigay kayo ng bukas-palad na tulong!

Para sa mga detalye, basahin ang [Gabay sa Pag-ambag](./docs/contributing/contributing_tl.md)

---

## Mga Tool at Istruktura ng Direktoryo (Para sa mga Developer)

Ang seksyong ito ay para sa mga developer na nais maunawaan ang prinsipyo ng automation ng proyekto.

### Direktoryo ng Proyekto

| Direktoryo | Paglalarawan |
|------|------|
| `src/` | .NET 10 source code ng pipeline ng pagsasalin, may 15 modules |
| `config/` | Configuration files ng pipeline (LLM, Steam, RAG parameters atbp.) |
| `data/` | Data sa runtime: metadata ng mod, embedding, cache ng pagsasalin |
| `translation_ref/` | Data ng reference na pagsasalin (tulad ng mod na pinahintulutan ng As1), nagbibigay ng reference para sa LLM |
| `base_game_keys/` | Mga key ng pagsasalin ng base game, ginagamit para sa deduplication upang maiwasang masakop ang orihinal na teksto |
| `final_outputs/` | Huling output: `project_babel/` mod package, `icons/` icon at `workshop_descriptions/` paglalarawan ng workshop |
| `docs/` | Dokumentasyon ng proyekto: ulat ng pag-unlad, gabay sa pag-ambag, paliwanag ng pipeline |
| `temp/` | Temporaryong file ng pipeline (hiwalay na direktoryo bawat pagtakbo) |
| `src/prompt_templates/` | Templates ng prompt para sa LLM (pagsasalin/pagsusuri ng nilalaman) |

### Mga Module ng Pipeline (Ayon sa pagkakasunud-sunod ng pagpapatakbo)

| Hakbang | Module | Function |
|------|------|------|
| 1 | `ConfigReader` | I-load ang config/secret/wikang listahan |
| 2 | `RepoDataLoader` | I-load ang reference translation at translation cache |
| 3 | `ModIdCollector` | Kolektahin ang Workshop mod ID |
| 4 | `ModInfoFetcher` | Kunin ang Steam metadata |
| 5 | `SteamCmdBootstrapper` | Ihanda ang steamcmd runtime para sa kasalukuyang platform |
| 6 | `ModDownloader` | I-download ang mod sa pamamagitan ng steamcmd |
| 7 | `ContentExtractor` | I-parse ang mod translation file → `TranslationEntry` |
| 8 | `ContentChecker` | Pagsusuri ng kaligtasan ng nilalaman (droga/pornograpiya/karahasan) |
| 9 | `EmbeddingFetcher` | Kalkulahin ang tekstong embedding vector |
| 10 | `TranslationBatcher` | Lumikha ng translation batch na walang kinalaman sa target na wika |
| 11 | `RagContextRetriever` | Kunin ang RAG context (exact key + embedding similarity) |
| 12 | `LLMTranslator` | Tawagan ang LLM upang isagawa ang pagsasalin |
| 13 | `ResultWriter` | Isulat sa data/ at translation_ref/ |
| 14 | `FinalOutputWriter` | Gumawa ng final PZ mod format output |
| 15 | `ProgressReporter` | Gumawa ng progress report |

### Teknolohiyang Stack

- **Wika**: C# (.NET 10)
- **Target na platform**: GitHub Actions Linux x64 runner
- **Pagsubok**: xUnit (Windows x64)
- **LLM**: DeepSeek API (Maaaring i-configure)
- **Embedding**: Text vectorization para sa RAG similarity retrieval
- **Pagsusuri ng nilalaman**: Multi-level security audit na hinimok ng LLM

Detalyadong [teknikal na sanggunian](./docs/technical_reference/technical_reference_tl.md).

---

## Copyright at Lisensya

Ang mga nilalaman ng tekstong pagsasalin at kaugnay na mga larawan ng proyektong ito ay nilikha o muling nilikha ng **Project Babel** at ng mga kalahok batay sa orihinal na mga mod ng laro.

© 2025 Project Babel at lahat ng may-akda na nagtataglay ng mga karapatan.

### 1. Mga tekstong, larawan, at iba pa

Maliban kung iba ang sinabi, ang nasa repositoryong ito:

- Pagsasalin, pagpapakinis at pag-proofread ng in-game text;
Dokumentasyon ng proyekto, pagsasalin ng teksto sa loob ng mod;
Mga imahe at artistikong mapagkukunan na espesyal na ginawa para sa proyektong ito

Lahat ay lisensyado sa ilalim ng **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, dinaglat bilang **CC BY-NC-SA 4.0**).

Nangangahulugan ito na, sa ilalim ng mga sumusunod na kundisyon, maaari mong malayang ibahagi at baguhin ang mga nilalamang ito:

- **Pagkilala (BY)** : Maglagay ng malinaw na pahayag na "Ang proyektong ito ng pagsasalin ay binago batay sa gawa ng 'Project Babel'", at isama ang link ng repositoryo na ito at Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Hindi Pangkomersyal na Paggamit (NC)** : Huwag gamitin ang nilalaman ng proyektong ito o ang mga binagong bersyon nito para sa anumang direkta o hindi direktang layuning pangkomersyo (kabilang ngunit hindi limitado sa bayad na integrated pack, bayad na pag-download, pagbabahagi ng kita sa ad, atbp.);
- **Parehong Paraan ng Pagbabahagi (SA)** : Kung magbabago o muling lilikha ka batay sa nilalaman ng proyektong ito, dapat mong ilathala ang iyong binagong bersyon sa ilalim ng **parehong CC BY-NC-SA 4.0 na lisensya**.

Para sa karagdagang impormasyon tungkol sa lisensyang ito, tingnan ang:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.tl>

*Espesyal na tala:*
- *Ang nilalaman ng folder ng base_game_keys ay mula sa orihinal na laro, ang copyright ay pag-aari ng developer ng laro! Ginagamit ang mga ito upang maiwasan ang pag-overwrite ng mga translation key sa mga game key (de-duplication)*
- *Ang nilalaman ng folder ng translation_ref ay ginagamit upang magbigay ng sanggunian sa pagsasalin para sa LLM, ang copyright ay pag-aari ng kani-kanilang mod developer!*

### 2. Programa, Script, at Iba Pang Nilalaman ng Pag-unlad

Maliban kung may ibang partikular na pahayag sa source code file o direktoryo, ang program code na ginamit para sa paggawa/pag-pack/pagproseso ng nilalaman ng pagsasalin (halimbawa, program code sa ilalim ng direktoryong `src/`) ay lisensyado sa ilalim ng **GNU General Public License na Bersyon 3 (GPL-3.0)**.

Ang buong mga tuntunin ay matatagpuan sa file na `LICENSE` sa root directory ng repositoryong ito (GPL-3.0), o bisitahin ang opisyal na website ng GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Pasasalamat

Gumagamit ang proyektong ito ng mga third-party na mod bilang reference text para sa target na wika ng pagsasalin. Ang reference text ay ipinapadala sa LLM para sa sanggunian sa pagsasalin.

| Pangalan ng Reference na Mod | May-akda | Pahina ng Mod |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Pahina ng Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Pahina ng Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Pahina ng Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Taos-pusong pasasalamat sa mga may-akda sa itaas!**

---

## Third-Party na Programa

Gumagamit ang proyektong ito ng mga third-party na programa at library. Ang copyright ng mga third-party na programang ito ay pag-aari ng kani-kanilang developer.

