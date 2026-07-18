# Project Babel — Proyecto de traducción automática LLM para mods de Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Este proyecto de traducción está impulsado y mantenido por el conjunto de herramientas [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Índice

- [Idiomas de traducción objetivo admitidos](#idiomas-de-traducción-objetivo-admitidos)
- [Cómo instalar y usar](#cómo-instalar-y-usar)
- [Progreso de traducción](#progreso-de-traducción)
- [Cómo contribuir](#cómo-contribuir)
- [Herramientas y estructura de directorios (para desarrolladores)](#herramientas-y-estructura-de-directorios-para-desarrolladores)
  - [Directorio del proyecto](#directorio-del-proyecto)
  - [Módulos de la tubería (en orden de ejecución)](#módulos-de-la-tubería-en-orden-de-ejecución)
  - [Módulos independientes](#módulos-independientes)
  - [Stack tecnológico](#stack-tecnológico)
- [Derechos de autor y licencia](#derechos-de-autor-y-licencia)
  - [1. Textos, imágenes y otros contenidos](#1-textos-imágenes-y-otros-contenidos)
  - [2. Programas, scripts y otros contenidos de desarrollo](#2-programas-scripts-y-otros-contenidos-de-desarrollo)
- [Agradecimientos](#agradecimientos)
- [Programas de terceros](#programas-de-terceros)

---

## Idiomas de traducción objetivo admitidos

| Idioma | Nombre local | Código internacional | Código en el juego | Compatible | Notas |
|------|------|------|------|------|------|
| Árabe | العربية | `ar` | `AR` | ❌ | Saldo de tokens insuficiente |
| Catalán | català | `ca` | `CA` | ❌ | Saldo de tokens insuficiente |
| Chino tradicional | 繁體中文 | `zh-hant` | `CH` | ❌ | Saldo de tokens insuficiente |
| Chino simplificado | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Checo | čeština | `cs` | `CS` | ❌ | Saldo de tokens insuficiente |
| Danés | dansk | `da` | `DA` | ❌ | Saldo de tokens insuficiente |
| Alemán | Deutsch | `de` | `DE` | ✅ | |
| Inglés | English | `en` | `EN` | ✅ | |
| Español | español | `es` | `ES` | ❌ | Saldo de tokens insuficiente |
| Finés | suomi | `fi` | `FI` | ❌ | Saldo de tokens insuficiente |
| Francés | français | `fr` | `FR` | ✅ | |
| Húngaro | magyar | `hu` | `HU` | ❌ | Saldo de tokens insuficiente |
| Indonesio | Bahasa Indonesia | `id` | `ID` | ❌ | Saldo de tokens insuficiente |
| Italiano | italiano | `it` | `IT` | ❌ | Saldo de tokens insuficiente |
| Japonés | 日本語 | `ja` | `JP` | ✅ | |
| Coreano | 한국어 | `ko` | `KO` | ❌ | Saldo de tokens insuficiente |
| Neerlandés | Nederlands | `nl` | `NL` | ❌ | Saldo de tokens insuficiente |
| Noruego | norsk | `no` | `NO` | ❌ | Saldo de tokens insuficiente |
| Tagalo | Tagalog | `tl` | `PH` | ❌ | Saldo de tokens insuficiente |
| Polaco | polski | `pl` | `PL` | ❌ | Saldo de tokens insuficiente |
| Portugués (Portugal) | português | `pt` | `PT` | ❌ | Saldo de tokens insuficiente |
| Portugués (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Saldo de tokens insuficiente |
| Rumano | română | `ro` | `RO` | ❌ | Saldo de tokens insuficiente |
| Ruso | русский | `ru` | `RU` | ❌ | Saldo de tokens insuficiente |
| Tailandés | ภาษาไทย | `th` | `TH` | ❌ | Saldo de tokens insuficiente |
| Turco | Türkçe | `tr` | `TR` | ❌ | Saldo de tokens insuficiente |
| Ucraniano | українська | `uk` | `UA` | ❌ | Saldo de tokens insuficiente |

**Total**: 27 idiomas planificados | **Soportados**: 5 | **Pendientes**: 22

---

## Cómo instalar y usar

Esta es una guía para jugadores que quieran usar este proyecto de traducción directamente en el juego.

1.  Ve a nuestra página de Steam Workshop: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Haz clic en el botón «Suscribirse».
3.  Inicia el juego y habilita este mod de traducción en la gestión de «Mods» del menú principal.
4.  Los textos de traducción de los mods habilitados posteriormente sobrescriben a los anteriores, por lo que este mod de traducción debe habilitarse después de los mods funcionales (lo más abajo posible).
5.  ¡Disfruta del juego!

---

## Progreso de traducción

**[➡️ Haz clic aquí para ver el progreso](./docs/progress/progress_es.md)**

---

## Cómo contribuir

Damos la bienvenida a cualquier persona que desee contribuir, ya sea corrigiendo un error, añadiendo una función, escribiendo plantillas de prompt, o proporcionando traducciones de referencia.

Llamar a la API de LLM para traducir requiere pagar por los tokens. Para que el proyecto pueda funcionar de manera estable a largo plazo, ¡esperamos su generosa ayuda!

Para más detalles, lea la [Guía de contribución](./docs/contributing/contributing_es.md)

---

## Herramientas y estructura de directorios (para desarrolladores)

Esta sección está dirigida a los desarrolladores que deseen comprender el principio de automatización del proyecto.

### Directorio del proyecto

| Directorio | Descripción |
|------|------|
| `src/` | Código fuente de la tubería de traducción .NET 10, contiene 15 módulos + 2 módulos independientes |
| `config/` | Archivos de configuración de la tubería (parámetros LLM, Steam, RAG, etc.) |
| `data/` | Datos de ejecución: metadatos de mods, embeddings, caché de traducción |
| `translation_ref/` | Datos de traducción de referencia (mods autorizados por el grupo de localización Ruyi (As1)), proporciona referencias de traducción al LLM |
| `base_game_keys/` | Claves de traducción del juego base, utilizadas para deduplicación y evitar sobrescribir texto nativo |
| `final_outputs/` | Salida final: paquete de mod `project_babel/`, iconos `icons/` y descripciones de Workshop `workshop_descriptions/` |
| `docs/` | Documentación del proyecto: informes de progreso, guías de contribución, descripción de la tubería |
| `temp/` | Archivos temporales de la tubería (directorio independiente por ejecución) |
| `src/prompt_templates/` | Plantillas de prompt del LLM (traducción/revisión de contenido) |

### Módulos de la tubería (en orden de ejecución)

| Paso | Módulo | Función |
|------|------|------|
| 1 | `ConfigReader` | Cargar configuración/claves/lista de idiomas |
| 2 | `RepoDataLoader` | Cargar referencias de traducción y caché de traducción |
| 3 | `ModIdCollector` | Recopilar ID de mods del Workshop |
| 4 | `ModInfoFetcher` | Obtener metadatos de Steam |
| 5 | `SteamCmdBootstrapper` | Preparar el entorno de ejecución de steamcmd para la plataforma actual |
| 6 | `ModDownloader` | Descargar mods mediante steamcmd |
| 7 | `ContentExtractor` | Analizar archivos de traducción de mods → `TranslationEntry` |
| 8 | `ContentChecker` | Revisión de seguridad de contenido (drogas/pornografía/violencia) |
| 9 | `EmbeddingFetcher` | Calcular vectores de embedding de texto |
| 10 | `TranslationBatcher` | Crear lotes de traducción independientes del idioma destino |
| 11 | `RagContextRetriever` | Recuperar contexto RAG (claves exactas + similitud de embedding) |
| 12 | `LLMTranslator` | Invocar LLM para realizar traducción |
| 13 | `ResultWriter` | Escribir en data/ y translation_ref/ |
| 14 | `FinalOutputWriter` | Generar la salida final en formato de mod para PZ |
| 15 | `ProgressReporter` | Generar informe de progreso |

### Módulos independientes

| Módulo | Función |
|------|------|
| `WorkshopMonitor` | Captura periódicamente nuevos mods de Steam Workshop, los filtra por número de suscripciones y los incorpora en `request_for_translation.txt` |
| `DocGenerator` | Generador de documentación multilingüe impulsado por LLM |

### Stack tecnológico

- **Lenguaje**: C# (.NET 10)
- **Plataforma objetivo**: GitHub Actions Linux x64 runner
- **Pruebas**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurable)
- **Embedding**: Vectorización de texto para búsqueda de similitud RAG
- **Revisión de contenido**: Auditoría de seguridad multinivel impulsada por LLM

[Referencia técnica](./docs/technical_reference/technical_reference_es.md) detallada.

---

## Derechos de autor y licencia

El contenido de los textos traducidos y las imágenes relacionadas de este proyecto de traducción han sido creados o adaptados por **Project Babel** y los participantes, basándose en los mods originales del juego.

© 2025 Project Babel y los autores. Todos los derechos reservados.

### 1. Textos, imágenes y otros contenidos

A menos que se indique lo contrario, en este repositorio:

- Contenido de traducción, revisión y corrección de textos del juego;
Traducción de documentos de proyecto y textos dentro de mods;
Imágenes y recursos artísticos creados específicamente para este proyecto.

Todos están licenciados bajo la **Atribución-NoComercial-CompartirIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviado **CC BY-NC-SA 4.0**).

Esto significa que, bajo las siguientes condiciones, puede compartir y adaptar libremente estos contenidos:

- **Atribución (BY)**: Indique claramente "Este proyecto de traducción se basa en el trabajo de 'Project Babel'" y adjunte el enlace de este repositorio y del taller de Steam `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **No comercial (NC)**: No utilice el contenido de este proyecto ni sus adaptaciones con fines comerciales directos o indirectos (incluyendo, pero no limitado a, paquetes de pago, descargas de pago, reparto de publicidad, etc.);
- **Compartir igual (SA)**: Si modifica o recrea basándose en el contenido de este proyecto, debe publicar su versión modificada bajo **la misma licencia CC BY-NC-SA 4.0**.

Para más información sobre esta licencia, consulte:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.es>

*Nota especial:*
- *El contenido de la carpeta base_game_keys proviene del juego base, los derechos de autor pertenecen al desarrollador del juego. El contenido se utiliza para evitar que las claves de traducción sobrescriban las claves del juego (deduplicación).*
- *El contenido de la carpeta translation_ref se utiliza para proporcionar referencias de traducción a la LLM, los derechos de autor pertenecen a los respectivos desarrolladores de mods.*

### 2. Programas, scripts y otros contenidos de desarrollo

A menos que se indique expresamente lo contrario en los archivos fuente o directorios, el código de programa en este repositorio utilizado para crear/empaquetar/procesar contenido de localización (por ejemplo, el código en el directorio `src/`) está licenciado bajo la **GNU General Public License versión 3 (GPL-3.0)**.

Consulte los términos completos en el archivo `LICENSE` en la raíz de este repositorio (GPL-3.0), o visite el sitio web de GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Agradecimientos

Este proyecto utiliza mods de terceros como textos de referencia para la traducción al idioma de destino. Los textos de referencia se envían a la LLM como referencia de traducción.

| Nombre del mod de referencia | Autor | Página del mod |
|------|------|------|
| [B42] Localización china unificada | Grupo de localización Ruyi (As1) | [Página del taller](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Localización de mods unificada | Grupo de localización Ruyi (As1) | [Página del taller](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Localización de Ark unificada | Grupo de localización Ruyi (As1) | [Página del taller](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**¡Un sincero agradecimiento a los autores anteriores!**

---

## Programas de terceros

Este proyecto utiliza programas y bibliotecas de terceros, cuyos derechos de autor pertenecen a sus respectivos desarrolladores.

