# Project Babel — Projecte de traducció automàtica per LLM del mod Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Aquest projecte de traducció està impulsat i mantingut per [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Taula de continguts

- [Idiomes objectiu de traducció suportats](#idiomes-objectiu-de-traducció-suportats)
- [Com instal·lar i utilitzar](#com-installar-i-utilitzar)
- [Progrés de la traducció](#progrés-de-la-traducció)
- [Com contribuir](#com-contribuir)
- [Eines i estructura de directoris (per a desenvolupadors)](#eines-i-estructura-de-directoris-per-a-desenvolupadors)
  - [Directoris del projecte](#directoris-del-projecte)
  - [Mòduls de la pipeline (per ordre d'execució)](#mòduls-de-la-pipeline-per-ordre-dexecució)
  - [Mòduls independents](#mòduls-independents)
  - [Pila tecnològica](#pila-tecnològica)
- [Drets d'autor i llicència](#drets-dautor-i-llicència)
  - [1. Text, imatges i altres continguts](#1-text-imatges-i-altres-continguts)
  - [2. Programes, scripts i altres continguts de desenvolupament](#2-programes-scripts-i-altres-continguts-de-desenvolupament)
- [Agraïments](#agraïments)
- [Programes de tercers](#programes-de-tercers)

---

## Idiomes objectiu de traducció suportats

| Idioma | Nom local | Codi internacional | Codi dins del joc | Suportat | Notes |
|------|------|------|------|------|------|
| Àrab | العربية | `ar` | `AR` | ❌ | Saldo de Token insuficient |
| Català | català | `ca` | `CA` | ❌ | Saldo de Token insuficient |
| Xinès tradicional | 繁體中文 | `zh-hant` | `CH` | ❌ | Saldo de Token insuficient |
| Xinès simplificat | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Txec | čeština | `cs` | `CS` | ❌ | Saldo de Token insuficient |
| Danès | dansk | `da` | `DA` | ❌ | Saldo de Token insuficient |
| Alemany | Deutsch | `de` | `DE` | ✅ | |
| Anglès | English | `en` | `EN` | ✅ | |
| Espanyol | español | `es` | `ES` | ❌ | Saldo de Token insuficient |
| Finès | suomi | `fi` | `FI` | ❌ | Saldo de Token insuficient |
| Francès | français | `fr` | `FR` | ✅ | |
| Hongarès | magyar | `hu` | `HU` | ❌ | Saldo de Token insuficient |
| Indonesi | Bahasa Indonesia | `id` | `ID` | ❌ | Saldo de Token insuficient |
| Italià | italiano | `it` | `IT` | ❌ | Saldo de Token insuficient |
| Japonès | 日本語 | `ja` | `JP` | ✅ | |
| Coreà | 한국어 | `ko` | `KO` | ❌ | Saldo de Token insuficient |
| Neerlandès | Nederlands | `nl` | `NL` | ❌ | Saldo de Token insuficient |
| Noruec | norsk | `no` | `NO` | ❌ | Saldo de Token insuficient |
| Tagal | Tagalog | `tl` | `PH` | ❌ | Saldo de Token insuficient |
| Polonès | polski | `pl` | `PL` | ❌ | Saldo de Token insuficient |
| Portuguès (Portugal) | português | `pt` | `PT` | ❌ | Saldo de Token insuficient |
| Portuguès (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Saldo de Token insuficient |
| Romanès | română | `ro` | `RO` | ❌ | Saldo de Token insuficient |
| Rus | русский | `ru` | `RU` | ❌ | Saldo de Token insuficient |
| Tailandès | ภาษาไทย | `th` | `TH` | ❌ | Saldo de Token insuficient |
| Turc | Türkçe | `tr` | `TR` | ❌ | Límit de tokens insuficient |
| Ucraïnès | українська | `uk` | `UA` | ❌ | Límit de tokens insuficient |

**Total**: 27 llengües planificades | **Admes**: 5 | **Per admetre**: 22

---

## Com instal·lar i utilitzar

Aquesta és una guia per als jugadors que volen utilitzar directament aquest projecte de traducció al joc.

1.  Aneu a la nostra pàgina del taller de Steam: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Feu clic al botó «Subscriure's».
3.  Inicieu el joc i activeu aquest mod de traducció a la gestió de «Mods» del menú principal.
4.  El text de traducció dels mods activats posteriorment substitueix amb prioritat els mods activats anteriorment, per tant, aquest mod de traducció s'ha d'activar després dels mods funcionals (posar-lo al final si és possible).
5.  Gaudeix del joc!

---

## Progrés de la traducció

**[➡️ Feu clic aquí per veure el progrés de la traducció](./docs/progress/progress_ca.md)**

---

## Com contribuir

Donem la benvinguda a qualsevol persona que vulgui contribuir, ja sigui corregint un error, afegint una funció, redactant plantilles de prompt, o proporcionant traduccions de referència!

Utilitzar l'API LLM per a la traducció requereix pagar per tokens. Perquè el projecte pugui funcionar de manera estable a llarg termini, esperem la vostra generosa ajuda!

Per a més detalls, llegiu la [Guia de contribució](./docs/contributing/contributing_ca.md)

---

## Eines i estructura de directoris (per a desenvolupadors)

Aquesta secció està dirigida als desenvolupadors que volen entendre el principi d'automatització del projecte.

### Directoris del projecte

| Directori | Descripció |
|------|------|
| `src/` | Codi font de la cadena de traducció .NET 10, amb 15 mòduls + 2 mòduls independents |
| `config/` | Fitxers de configuració de la pipeline (paràmetres LLM, Steam, RAG, etc.) |
| `data/` | Dades d'execució: metadades de mods, embeddings, memòria cau de traducció |
| `translation_ref/` | Dades de traducció de referència (com mods autoritzats per As1), proporcionen referència de traducció per a l'LLM |
| `base_game_keys/` | Claus de traducció del joc base, utilitzades per eliminar duplicats i evitar sobreescriure text natiu |
| `final_outputs/` | Sortida final: paquet de mod `project_babel/`, icones `icons/` i descripcions del taller `workshop_descriptions/` |
| `docs/` | Documentació del projecte: informe de progrés, guia de contribució, descripció de la pipeline |
| `temp/` | Fitxers temporals de la pipeline (directori independent per a cada execució) |
| `src/prompt_templates/` | Plantilles de prompt de LLM (traducció/revisió de contingut) |

### Mòduls de la pipeline (per ordre d'execució)

| Pas | Mòdul | Funció |
|------|------|------|
| 1 | `ConfigReader` | Carregar configuració/claus/llista d'idiomes |
| 2 | `RepoDataLoader` | Carregar traduccions de referència i memòria cau de traducció |
| 3 | `ModIdCollector` | Recollir ID de mods del Workshop |
| 4 | `ModInfoFetcher` | Obtenir metadades de Steam |
| 5 | `SteamCmdBootstrapper` | Preparar l'entorn d'execució de steamcmd per a la plataforma actual |
| 6 | `ModDownloader` | Descarregar mods mitjançant steamcmd |
| 7 | `ContentExtractor` | Analitzar fitxers de traducció de mods → `TranslationEntry` |
| 8 | `ContentChecker` | Revisió de seguretat del contingut (drogues/pornografia/violència) |
| 9 | `EmbeddingFetcher` | Calcular vectors d'embedding de text |
| 10 | `TranslationBatcher` | Crear lots de traducció independents de l'idioma de destinació |
| 11 | `RagContextRetriever` | Recuperar context RAG (clau exacta + similitud d'embedding) |
| 12 | `LLMTranslator` | Invocar LLM per realitzar traducció |
| 13 | `ResultWriter` | Escriure a data/ i translation_ref/ |
| 14 | `FinalOutputWriter` | Generar sortida final en format de mod de PZ |
| 15 | `ProgressReporter` | Generar informe de progrés |

### Mòduls independents

| Mòdul | Funció |
|------|------|
| `WorkshopMonitor` | Recull periòdicament nous mods del Steam Workshop, filtra per nombre de subscripcions i afegeix a `request_for_translation.txt` |
| `DocGenerator` | Generador de documentació multilingüe impulsat per LLM |

### Pila tecnològica

- **Llenguatge**: C# (.NET 10)
- **Plataforma objectiu**: GitHub Actions Linux x64 runner
- **Proves**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurable)
- **Embedding**: Vectorització de text per a cerca de similitud RAG
- **Revisió de contingut**: Auditoria de seguretat multinivell impulsada per LLM

Detallat [Referència tècnica](./docs/technical_reference/technical_reference_ca.md).

---

## Drets d'autor i llicència

El contingut de traducció i les imatges relacionades d'aquest projecte de traducció han estat creats o adaptats per **Project Babel** i els col·laboradors basant-se en mods de joc originals.

© 2025 Project Babel i els autors respectius. Tots els drets reservats.

### 1. Text, imatges i altres continguts

Llevat que s'indiqui el contrari, en aquest repositori:

- Traducció, refinament i correcció de text dins del joc;
Documentació del projecte, traducció de text dels mods;
Imatges i recursos artístics fets específicament per a aquest projecte

Tots estan llicenciats sota la **Reconeixement-NoComercial-CompartirIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreujat com **CC BY-NC-SA 4.0**)

Això vol dir que, complint les condicions següents, podeu compartir i adaptar lliurement aquests continguts:

- **Reconeixement (BY)**: Indiqueu de manera clara "Aquest projecte de traducció es basa en el treball de 'Project Babel' i s'ha modificat", i adjunteu l'enllaç al repositori i al taller de Steam `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **No comercial (NC)**: No es pot utilitzar el contingut d'aquest projecte o les seves adaptacions per a cap ús comercial directe o indirecte (incloent, entre d'altres, paquets de pagament, descàrregues de pagament, repartiment de publicitat, etc.);
- **Compartir igual (SA)**: Si modifiqueu o creeu obres derivades d'aquest contingut, heu de publicar les vostres versions modificades sota la **mateixa llicència CC BY-NC-SA 4.0** de manera pública.

Per obtenir més informació sobre aquesta llicència, consulteu:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.ca>

*Notes especials:*
- *El contingut de la carpeta base_game_keys prové del joc base, els drets d'autor pertanyen al desenvolupador del joc! El contingut s'utilitza per evitar que les claus de traducció sobreescriguin les claus del joc (deduplicació)*
- *El contingut de la carpeta translation_ref s'utilitza per proporcionar referències de traducció a l'LLM, els drets d'autor pertanyen als respectius desenvolupadors de mods!*

### 2. Programes, scripts i altres continguts de desenvolupament

Llevat que es declari el contrari en els fitxers de codi font o directoris, el codi del programa d'aquest repositori utilitzat per crear/empaquetar/processar continguts de traducció (per exemple, el codi del directori `src/`) està llicenciat sota la **Llicència Pública General de GNU versió 3 (GPL-3.0)**.

Per als termes complets, consulteu el fitxer `LICENSE` a l'arrel del repositori (GPL-3.0) o visiteu el lloc web oficial de GNU: <https://www.gnu.org/licenses/gpl-3.0.html>

---

## Agraïments

Aquest projecte ha utilitzat mods de tercers com a text de referència per a la traducció a la llengua objectiu. El text de referència s'envia a l'LLM per a la seva consulta de traducció.

| Nom del mod de referència | Autor | Pàgina del mod |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Pàgina del taller](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Pàgina del taller](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Pàgina del taller](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Un agraïment sincer als autors anteriors!**

---

## Programes de tercers

Aquest projecte utilitza programes i biblioteques de tercers, els drets d'autor dels quals pertanyen als seus respectius desenvolupadors.

