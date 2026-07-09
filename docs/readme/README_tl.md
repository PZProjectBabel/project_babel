# Project Babel — Awtomatikong Pagsasalin ng PZ Mods gamit ang LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Tala:** Ang pagsasaling ito ay hindi pa suportado. Ang opisyal na nilalaman ay ang [bersyong Tsino](../../README.md).

---

*Ang proyektong pagsasalin na ito ay pinapatakbo at pinapanatili ng [Project Babel](https://github.com/PZProjectBabel/project_babel) toolset.*

---

## Talaan ng mga Nilalaman

- [Mga Suportadong Target na Wika](#mga-suportadong-target-na-wika)
- [Paano i-install at gamitin](#paano-i-install-at-gamitin)
- [Pag-unlad ng Pagsasalin](#pag-unlad-ng-pagsasalin)
- [Pag-ambag](#pag-ambag)
- [Mga Tool at Istraktura ng Direktoryo (para sa mga Developer)](#mga-tool-at-istraktura-ng-direktoryo-(para-sa-mga-developer))
- [Karapatang-ari at Lisensya](#karapatang-ari-at-lisensya)
- [Mga Pasasalamat](#mga-pasasalamat)
- [Software ng Third-Party](#software-ng-third-party)

---

## Mga Suportadong Target na Wika

| Wika | Lokal na Pangalan | ISO Code | In-Game Code | Suportado | Tala |
|------|------|------|------|------|------|
| Arabe | العربية | `ar` | `AR` | ❌ | Kulang sa token credits |
| Katalan | català | `ca` | `CA` | ❌ | Kulang sa token credits |
| Tradisyonal na Tsino | 繁體中文 | `zh-hant` | `CH` | ❌ | Kulang sa token credits |
| Pinasimpleng Tsino | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tseko | čeština | `cs` | `CS` | ❌ | Kulang sa token credits |
| Danes | dansk | `da` | `DA` | ❌ | Kulang sa token credits |
| Aleman | Deutsch | `de` | `DE` | ✅ | |
| Ingles | English | `en` | `EN` | ✅ | |
| Espanyol | español | `es` | `ES` | ❌ | Kulang sa token credits |
| Pinlandes | suomi | `fi` | `FI` | ❌ | Kulang sa token credits |
| Pranses | français | `fr` | `FR` | ✅ | |
| Unggaro | magyar | `hu` | `HU` | ❌ | Kulang sa token credits |
| Indones | Bahasa Indonesia | `id` | `ID` | ❌ | Kulang sa token credits |
| Italyano | italiano | `it` | `IT` | ❌ | Kulang sa token credits |
| Hapones | 日本語 | `ja` | `JP` | ✅ | |
| Koreano | 한국어 | `ko` | `KO` | ❌ | Kulang sa token credits |
| Olandes | Nederlands | `nl` | `NL` | ❌ | Kulang sa token credits |
| Norwego | norsk | `no` | `NO` | ❌ | Kulang sa token credits |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Kulang sa token credits |
| Polako | polski | `pl` | `PL` | ❌ | Kulang sa token credits |
| Portuges (Portugal) | português | `pt` | `PT` | ❌ | Kulang sa token credits |
| Portuges (Brazil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Kulang sa token credits |
| Rumano | română | `ro` | `RO` | ❌ | Kulang sa token credits |
| Ruso | русский | `ru` | `RU` | ❌ | Kulang sa token credits |
| Thai | ภาษาไทย | `th` | `TH` | ❌ | Kulang sa token credits |
| Turko | Türkçe | `tr` | `TR` | ❌ | Kulang sa token credits |
| Ukranyano | українська | `uk` | `UA` | ❌ | Kulang sa token credits |

**Kabuuan**: 27 nakaplanong wika | **Suportado**: 5 | **Nakabinbin**: 22

---

## Paano i-install at gamitin

Gabay para sa mga manlalaro na gustong gamitin ang translation pack sa laro.

1. Pumunta sa pahina ng Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. I-click ang "Subscribe".
3. Ilunsad ang laro, paganahin ang translation mod na ito sa menu ng Mods.
4. Ang teksto ng pagsasalin mula sa mga mod na huling na-load ay nangingibabaw sa mga nauna, kaya ang translation mod na ito ay dapat i-load pagkatapos ng mga gameplay mod.
5. Mag-enjoy!

---

## Pag-unlad ng Pagsasalin

[➡️ Pag-unlad ng Pagsasalin](../progress/progress_tl.md)

---

## Pag-ambag

Tinatanggap namin ang mga ambag! Pag-aayos ng pagsasalin, bagong feature, prompt template, o sangguniang pagsasalin.

Ang mga tawag sa LLM API para sa pagsasalin ay may gastos sa token. Ang iyong suporta ay tumutulong sa proyekto na tumakbo nang tuluy-tuloy!

Read the [Contributing Guide](../contributing/contributing_tl.md) for details.

---

## Mga Tool at Istraktura ng Direktoryo (para sa mga Developer)

Ang seksyong ito ay para sa mga developer na gustong maunawaan ang panloob na automation ng proyekto.

### Mga Direktoryo ng Proyekto

| Direktoryo | Paglalarawan |
|------|------|
| `src/` | .NET 10 pipeline source code, 15 na modyul |
| `config/` | Konpigurasyon ng pipeline (LLM, Steam, RAG parameters, atbp.) |
| `data/` | Runtime data: mod metadata, embeddings, translation cache |
| `translation_ref/` | Mga sangguniang pagsasalin bilang konteksto ng LLM |
| `base_game_keys/` | Mga translation key ng batayang laro para sa deduplikasyon |
| `final_outputs/` | Panghuling output sa format ng PZ mod |
| `docs/` | Dokumentasyon: progreso, kontribusyon, mga detalye ng pipeline |
| `temp/` | Pansamantalang mga file ng pipeline |
| `src/prompt_templates/` | Mga template ng prompt ng LLM |

### Mga Modyul ng Pipeline (pagkakasunod-sunod)

| Hakbang | Modyul | Tungkulin |
|------|------|------|
| 1 | `ConfigReader` | I-load ang konpigurasyon/mga sikreto/mga wika |
| 2 | `RepoDataLoader` | I-load ang mga sanggunian at translation cache |
| 3 | `ModIdCollector` | Kolektahin ang mga Workshop mod ID |
| 4 | `ModInfoFetcher` | Kunin ang Steam metadata |
| 5 | `ModDownloader` | I-download ang mga mod sa pamamagitan ng steamcmd |
| 6 | `ContentExtractor` | I-parse ang mod translation files → `TranslationEntry` |
| 7 | `ContentChecker` | Pagsusuri ng kaligtasan ng nilalaman |
| 8 | `EmbeddingFetcher` | Kalkulahin ang text embedding vectors |
| 9 | `TranslationBatcher` | Lumikha ng mga batch ng pagsasalin |
| 10 | `RagContextRetriever` | Kunin ang mga konteksto ng RAG |
| 11 | `LLMTranslator` | Isagawa ang pagsasalin ng LLM |
| 12 | `ResultWriter` | Isulat sa data/ at translation_ref/ |
| 13 | `FinalOutputWriter` | Bumuo ng panghuling output sa format ng PZ mod |
| 14 | `ProgressReporter` | Bumuo ng mga ulat ng progreso |

### Tech Stack

- **Wika**: C# (.NET 10)
- **Target na Platform**: GitHub Actions Linux x64 runner
- **Mga Pagsubok**: xUnit (Windows x64)
- **LLM**: DeepSeek API (naaayos)
- **Embedding**: Text vectorization para sa RAG similarity search
- **Pagsusuri ng Nilalaman**: Multi-level na safety audit na pinapatakbo ng LLM

Detalyadong teknikal na dokumentasyon: [Pipeline ng TranslationEntry](../pipeline/translation_entry_pipeline_tl.md)

---

## Karapatang-ari at Lisensya

© 2025 Project Babel at lahat ng mga may-akda. Lahat ng karapatan ay nakalaan.

### Nilalaman (mga teksto, larawan)

May lisensya sa ilalim ng **CC BY-NC-SA 4.0**.

- **Pagbanggit**: Ipahiwatig ang mga pagbabago batay sa "Project Babel", may mga link sa repo at Workshop
- **Di-komersyal**: Ipinagbabawal ang komersyal na paggamit
- **Magbahagi nang katulad**: Ang mga pagbabago ay dapat ilathala sa ilalim ng parehong lisensya

### Kodigo

Ang code sa ilalim ng `src/` ay lisensyado sa ilalim ng **GPL-3.0**.

---

## Mga Pasasalamat

| Sangguniang Mod | May-akda | Pahina |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Taos-pusong pasasalamat sa mga may-akda sa itaas!**

---

## Software ng Third-Party

Gumagamit ang proyektong ito ng mga programa at aklatan ng third-party, ang mga karapatang-ari ay pag-aari ng kani-kanilang mga developer.
