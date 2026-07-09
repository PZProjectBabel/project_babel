# Project Babel — Traducció automàtica de mods de PZ amb LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Nota:** Aquesta traducció encara no és compatible. El contingut autoritzat és la [versió xinesa](../../README.md).

---

*Aquest projecte de traducció és impulsat i mantingut per l'eina [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Taula de continguts

- [Idiomes destí suportats](#idiomes-destí-suportats)
- [Com instal·lar i utilitzar](#com-instal·lar-i-utilitzar)
- [Progrés de la traducció](#progrés-de-la-traducció)
- [Com contribuir](#com-contribuir)
- [Eines i estructura de directoris (per a desenvolupadors)](#eines-i-estructura-de-directoris-(per-a-desenvolupadors))
- [Drets d'autor i llicència](#drets-d'autor-i-llicència)
- [Agraïments](#agraïments)
- [Programari de tercers](#programari-de-tercers)

---

## Idiomes destí suportats

| Llengua | Nom local | Codi ISO | Codi al joc | Suport | Comentaris |
|------|------|------|------|------|------|
| Àrab | العربية | `ar` | `AR` | ❌ | Falta de crèdits de token |
| Català | català | `ca` | `CA` | ❌ | Falta de crèdits de token |
| Xinès tradicional | 繁體中文 | `zh-hant` | `CH` | ❌ | Falta de crèdits de token |
| Xinès simplificat | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Txec | čeština | `cs` | `CS` | ❌ | Falta de crèdits de token |
| Danès | dansk | `da` | `DA` | ❌ | Falta de crèdits de token |
| Alemany | Deutsch | `de` | `DE` | ✅ | |
| Anglès | English | `en` | `EN` | ✅ | |
| Espanyol | español | `es` | `ES` | ❌ | Falta de crèdits de token |
| Finès | suomi | `fi` | `FI` | ❌ | Falta de crèdits de token |
| Francès | français | `fr` | `FR` | ✅ | |
| Hongarès | magyar | `hu` | `HU` | ❌ | Falta de crèdits de token |
| Indonesi | Bahasa Indonesia | `id` | `ID` | ❌ | Falta de crèdits de token |
| Italià | italiano | `it` | `IT` | ❌ | Falta de crèdits de token |
| Japonès | 日本語 | `ja` | `JP` | ✅ | |
| Coreà | 한국어 | `ko` | `KO` | ❌ | Falta de crèdits de token |
| Neerlandès | Nederlands | `nl` | `NL` | ❌ | Falta de crèdits de token |
| Noruec | norsk | `no` | `NO` | ❌ | Falta de crèdits de token |
| Tagal | Tagalog | `tl` | `PH` | ❌ | Falta de crèdits de token |
| Polonès | polski | `pl` | `PL` | ❌ | Falta de crèdits de token |
| Portuguès (Portugal) | português | `pt` | `PT` | ❌ | Falta de crèdits de token |
| Portuguès (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Falta de crèdits de token |
| Romanès | română | `ro` | `RO` | ❌ | Falta de crèdits de token |
| Rus | русский | `ru` | `RU` | ❌ | Falta de crèdits de token |
| Tailandès | ภาษาไทย | `th` | `TH` | ❌ | Falta de crèdits de token |
| Turc | Türkçe | `tr` | `TR` | ❌ | Falta de crèdits de token |
| Ucraïnès | українська | `uk` | `UA` | ❌ | Falta de crèdits de token |

**Total**: 27 idiomes planificats | **Suportats**: 5 | **Pendents**: 22

---

## Com instal·lar i utilitzar

Guia per a jugadors que volen utilitzar el paquet de traducció dins del joc.

1. Ves a la pàgina de Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Fes clic a "Subscriure's".
3. Inicia el joc, activa aquest mod de traducció al menú de Mods.
4. El text de traducció dels mods carregats posteriorment sobreescriu els anteriors, així que aquest mod de traducció s'ha de carregar després dels mods de joc.
5. Gaudeix!

---

## Progrés de la traducció

[➡️ Progrés de la traducció](../progress/progress_ca.md)

---

## Com contribuir

Acceptem contribucions! Correccions de traducció, noves funcionalitats, plantilles o traduccions de referència.

Les crides a l'API LLM per a traduccions tenen costos de tokens. El vostre suport ajuda el projecte a funcionar de manera sostenible!

Read the [Contributing Guide](../contributing/contributing_ca.md) for details.

---

## Eines i estructura de directoris (per a desenvolupadors)

Aquesta secció va dirigida a desenvolupadors que vulguin entendre el funcionament intern de l'automatització del projecte.

### Directoris del projecte

| Directori | Descripció |
|------|------|
| `src/` | Codi font del pipeline de traducció .NET 10, 15 mòduls |
| `config/` | Configuració del pipeline (LLM, Steam, paràmetres RAG, etc.) |
| `data/` | Dades d'execució: metadades de mods, embeddings, memòria cau |
| `translation_ref/` | Traduccions de referència com a context LLM |
| `base_game_keys/` | Claus de traducció del joc base per a deduplicació |
| `final_outputs/` | Sortida final en format mod PZ |
| `docs/` | Documentació: progrés, contribució, especificacions del pipeline |
| `temp/` | Fitxers temporals del pipeline |
| `src/prompt_templates/` | Plantilles de prompts LLM |

### Mòduls del pipeline (ordre d'execució)

| Pas | Mòdul | Funció |
|------|------|------|
| 1 | `ConfigReader` | Carregar configuració/secrets/idiomes |
| 2 | `RepoDataLoader` | Carregar referències i memòria cau |
| 3 | `ModIdCollector` | Recollir IDs de mods del Workshop |
| 4 | `ModInfoFetcher` | Obtenir metadades de Steam |
| 5 | `ModDownloader` | Baixar mods via steamcmd |
| 6 | `ContentExtractor` | Analitzar fitxers de traducció → `TranslationEntry` |
| 7 | `ContentChecker` | Revisió de seguretat del contingut |
| 8 | `EmbeddingFetcher` | Calcular vectors d'embedding de text |
| 9 | `TranslationBatcher` | Crear lots de traducció |
| 10 | `RagContextRetriever` | Recuperar contextos RAG |
| 11 | `LLMTranslator` | Executar traducció LLM |
| 12 | `ResultWriter` | Escriure a data/ i translation_ref/ |
| 13 | `FinalOutputWriter` | Generar sortida final en format mod PZ |
| 14 | `ProgressReporter` | Generar informes de progrés |

### Stack tecnològic

- **Llenguatge**: C# (.NET 10)
- **Plataforma objectiu**: GitHub Actions Linux x64 runner
- **Proves**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurable)
- **Embedding**: Vectorització de text per a cerca de similitud RAG
- **Revisió de contingut**: Revisió de seguretat multinivell impulsada per LLM

Documentació tècnica detallada: [Pipeline TranslationEntry](../pipeline/translation_entry_pipeline_ca.md)

---

## Drets d'autor i llicència

© 2025 Project Babel i tots els autors. Tots els drets reservats.

### Contingut (textos, imatges)

Llicenciat sota **CC BY-NC-SA 4.0**.

- **Atribució**: Indicar modificacions basades en «Project Babel», amb enllaços al repositori i Workshop
- **No comercial**: Ús comercial prohibit
- **Compartir igual**: Les modificacions s'han de publicar sota la mateixa llicència

### Codi

El codi a `src/` està sota llicència **GPL-3.0**.

---

## Agraïments

| Mod de referència | Autor | Pàgina |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Moltes gràcies als autors anteriors!**

---

## Programari de tercers

Aquest projecte utilitza programes i biblioteques de tercers, els drets d'autor pertanyen als seus respectius desenvolupadors.
