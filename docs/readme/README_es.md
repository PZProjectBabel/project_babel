# Project Babel — Traducción automática de mods de PZ con LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Nota:** Esta traducción aún no está soportada. El contenido autorizado es la [versión en chino](../../README.md).

---

*Este proyecto de traducción es impulsado y mantenido por el conjunto de herramientas [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Tabla de contenidos

- [Idiomas de destino soportados](#idiomas-de-destino-soportados)
- [Instalación y uso](#instalación-y-uso)
- [Progreso de la traducción](#progreso-de-la-traducción)
- [Cómo contribuir](#cómo-contribuir)
- [Herramientas y estructura de directorios (para desarrolladores)](#herramientas-y-estructura-de-directorios-para-desarrolladores)
- [Derechos de autor y licencia](#derechos-de-autor-y-licencia)
- [Agradecimientos](#agradecimientos)
- [Software de terceros](#software-de-terceros)

---

## Idiomas de destino soportados

| Idioma | Nombre local | Código ISO | Código en juego | Soportado | Notas |
|------|------|------|------|------|------|
| Árabe | العربية | `ar` | `AR` | ❌ | Falta de créditos de token |
| Catalán | català | `ca` | `CA` | ❌ | Falta de créditos de token |
| Chino tradicional | 繁體中文 | `zh-hant` | `CH` | ❌ | Falta de créditos de token |
| Chino simplificado | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Checo | čeština | `cs` | `CS` | ❌ | Falta de créditos de token |
| Danés | dansk | `da` | `DA` | ❌ | Falta de créditos de token |
| Alemán | Deutsch | `de` | `DE` | ✅ | |
| Inglés | English | `en` | `EN` | ✅ | |
| Español | español | `es` | `ES` | ❌ | Falta de créditos de token |
| Finés | suomi | `fi` | `FI` | ❌ | Falta de créditos de token |
| Francés | français | `fr` | `FR` | ✅ | |
| Húngaro | magyar | `hu` | `HU` | ❌ | Falta de créditos de token |
| Indonesio | Bahasa Indonesia | `id` | `ID` | ❌ | Falta de créditos de token |
| Italiano | italiano | `it` | `IT` | ❌ | Falta de créditos de token |
| Japonés | 日本語 | `ja` | `JP` | ✅ | |
| Coreano | 한국어 | `ko` | `KO` | ❌ | Falta de créditos de token |
| Neerlandés | Nederlands | `nl` | `NL` | ❌ | Falta de créditos de token |
| Noruego | norsk | `no` | `NO` | ❌ | Falta de créditos de token |
| Tagalo | Tagalog | `tl` | `PH` | ❌ | Falta de créditos de token |
| Polaco | polski | `pl` | `PL` | ❌ | Falta de créditos de token |
| Portugués (Portugal) | português | `pt` | `PT` | ❌ | Falta de créditos de token |
| Portugués (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Falta de créditos de token |
| Rumano | română | `ro` | `RO` | ❌ | Falta de créditos de token |
| Ruso | русский | `ru` | `RU` | ❌ | Falta de créditos de token |
| Tailandés | ภาษาไทย | `th` | `TH` | ❌ | Falta de créditos de token |
| Turco | Türkçe | `tr` | `TR` | ❌ | Falta de créditos de token |
| Ucraniano | українська | `uk` | `UA` | ❌ | Falta de créditos de token |

**Total**: 27 idiomas planificados | **Soportados**: 5 | **Pendientes**: 22

---

## Instalación y uso

Guía para jugadores que quieren usar el paquete de traducción en el juego.

1. Ve a la página del Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Haz clic en «Suscribirse».
3. Inicia el juego, activa este mod de traducción en el menú de Mods.
4. El texto de los mods cargados después sobrescribe a los anteriores, así que este mod de traducción debe cargarse después de los mods de juego.
5. ¡Disfruta!

---

## Progreso de la traducción

[➡️ Progreso de la traducción](../progress/progress_es.md)

---

## Cómo contribuir

¡Aceptamos contribuciones! Correcciones de traducción, nuevas funciones, plantillas de prompts o traducciones de referencia.

Las llamadas a la API de LLM para traducción generan costes de tokens. ¡Tu apoyo ayuda a que el proyecto funcione de forma sostenible!

Lee la [Guía de Contribución](../contributing/contributing_es.md) para más detalles.

---

## Herramientas y estructura de directorios (para desarrolladores)

Esta sección está dirigida a desarrolladores que quieran comprender el funcionamiento interno de la automatización del proyecto.

### Directorios del proyecto

| Directorio | Descripción |
|------|------|
| `src/` | Código fuente del pipeline de traducción .NET 10, 15 módulos |
| `config/` | Configuración del pipeline (LLM, Steam, parámetros RAG, etc.) |
| `data/` | Datos de ejecución: metadatos de mods, embeddings, caché de traducción |
| `translation_ref/` | Traducciones de referencia como contexto LLM |
| `base_game_keys/` | Claves de traducción del juego base para deduplicación |
| `final_outputs/` | Salida final en formato de mod PZ |
| `docs/` | Documentación: progreso, contribución, especificaciones del pipeline |
| `temp/` | Archivos temporales del pipeline |
| `src/prompt_templates/` | Plantillas de prompts LLM |

### Módulos del pipeline (orden de ejecución)

| Paso | Módulo | Función |
|------|------|------|
| 1 | `ConfigReader` | Cargar configuración/secretos/idiomas |
| 2 | `RepoDataLoader` | Cargar referencias y caché de traducción |
| 3 | `ModIdCollector` | Recopilar IDs de mods del Workshop |
| 4 | `ModInfoFetcher` | Obtener metadatos de Steam |
| 5 | `ModDownloader` | Descargar mods vía steamcmd |
| 6 | `ContentExtractor` | Analizar archivos de traducción → `TranslationEntry` |
| 7 | `ContentChecker` | Revisión de seguridad del contenido |
| 8 | `EmbeddingFetcher` | Calcular vectores de embedding de texto |
| 9 | `TranslationBatcher` | Crear lotes de traducción |
| 10 | `RagContextRetriever` | Recuperar contextos RAG |
| 11 | `LLMTranslator` | Ejecutar traducción LLM |
| 12 | `ResultWriter` | Escribir en data/ y translation_ref/ |
| 13 | `FinalOutputWriter` | Generar salida final en formato mod PZ |
| 14 | `ProgressReporter` | Generar informes de progreso |

### Stack tecnológico

- **Lenguaje**: C# (.NET 10)
- **Plataforma objetivo**: GitHub Actions Linux x64 runner
- **Pruebas**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurable)
- **Embedding**: Vectorización de texto para búsqueda de similitud RAG
- **Revisión de contenido**: Auditoría de seguridad multinivel impulsada por LLM

Documentación técnica detallada: [Pipeline de TranslationEntry](../pipeline/translation_entry_pipeline_es.md)

---

## Derechos de autor y licencia

© 2025 Project Babel y autores. Todos los derechos reservados.

### Contenido (textos, imágenes)

Bajo licencia **CC BY-NC-SA 4.0**.

- **Atribución**: Indicar modificaciones basadas en «Project Babel», con enlaces al repositorio y Workshop
- **No comercial**: Uso comercial prohibido
- **Compartir igual**: Las modificaciones deben publicarse bajo la misma licencia

### Código

El código bajo `src/` está bajo licencia **GPL-3.0**.

---

## Agradecimientos

| Mod de referencia | Autor | Página |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**¡Muchas gracias a los autores mencionados!**

---

## Software de terceros

Este proyecto utiliza programas y bibliotecas de terceros, cuyos derechos de autor pertenecen a sus respectivos desarrolladores.

