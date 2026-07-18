# Guía de Contribución (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Índice

- [1. Antes de comenzar](#1-antes-de-comenzar)
- [2. ¿Cómo puedo contribuir?](#2-cómo-puedo-contribuir)
- [3. Proporcionar reglas de traducción, diccionario de términos y mejorar los prompts del sistema](#3-proporcionar-reglas-de-traducción-diccionario-de-términos-y-mejorar-los-prompts-del-sistema)
- [4. Proporcionar corpus de traducción revisado manualmente](#4-proporcionar-corpus-de-traducción-revisado-manualmente)
- [5. Contribuciones al pipeline y desarrollo de herramientas](#5-contribuciones-al-pipeline-y-desarrollo-de-herramientas)
- [6. Acuerdo de derechos de autor y licencia](#6-acuerdo-de-derechos-de-autor-y-licencia)
  - [6.1 Principio básico: tú conservas los derechos de autor y concedes licencia al proyecto](#61-principio-básico-tú-conservas-los-derechos-de-autor-y-concedes-licencia-al-proyecto)
  - [6.2 Licencia para contenido textual y gráfico (CC BY-NC-SA 4.0)](#62-licencia-para-contenido-textual-y-gráfico-cc-by-nc-sa-40)
  - [6.3 Licencia para scripts y código de herramientas (GPL-3.0)](#63-licencia-para-scripts-y-código-de-herramientas-gpl-30)
  - [6.4 Derechos de autor de obras upstream y del juego original](#64-derechos-de-autor-de-obras-upstream-y-del-juego-original)
- [7. Comunicación y colaboración](#7-comunicación-y-colaboración)
- [8. Apoyo financiero](#8-apoyo-financiero)

---

Muchas gracias por estar dispuesto a contribuir al **Project Babel - 《僵尸毁灭工程》模组LLM自动翻译项目**! Ya sea corregir un error, añadir una nueva función, redactar una plantilla de prompt, o proporcionar una traducción de referencia!

Llamar a la API de LLM para traducir requiere pagar por los tokens. Para que el proyecto pueda operar de manera estable a largo plazo, esperamos su generosa ayuda!

> ⚠️ **Aviso importante:**
> Antes de enviar cualquier contenido a este repositorio, asegúrese de leer y comprender la sección "Acuerdo de derechos de autor y licencia".
> Una vez que se envíe y se fusione, se considerará que acepta los términos de licencia correspondientes.

---

## 1. Antes de comenzar

Lea primero el `README.md` del proyecto para comprender:
- El objetivo general de este proyecto y su estado actual;
- Cómo los jugadores normales pueden usar este proyecto (para que puedas probarlo tú mismo);
- Detalles técnicos del proyecto.

---

## 2. ¿Cómo puedo contribuir?

Puedes participar de una o más maneras según tus intereses y habilidades:

- Proporcionar reglas de traducción para el idioma de destino
- Proporcionar un diccionario de términos de traducción para el idioma de destino
- Mejorar los prompts del sistema
- Proporcionar corpus de texto traducido revisado manualmente
- Mejorar los módulos de la tubería (.NET) y los scripts de automatización
- Reportar problemas, sugerir mejoras (indicar en Issues)
- Proporcionar apoyo financiero para las llamadas de LLM

A continuación, se ofrecen algunas explicaciones sobre los principales escenarios de contribución.

---

## 3. Proporcionar reglas de traducción, diccionario de términos y mejorar los prompts del sistema

Las plantillas de prompt de la tubería se encuentran en `src/prompt_templates/`, con la siguiente estructura:

- `system_prompt_translate_engine.txt`: Prompt del sistema del motor de traducción global (compartido por todos los idiomas);
- `<语言代码>/translation_dictionary_<语言代码>.json`: Diccionario de términos para ese idioma;
- `<语言代码>/translation_schema_<语言代码>.md`: Reglas de traducción y restricciones de estilo para ese idioma.

Pasos para contribuir:

1. Cree un subdirectorio para su idioma en `src/prompt_templates/` y agregue el diccionario de términos y el archivo de reglas de traducción;
2. Si necesita ajustar el comportamiento de traducción global, modifique `system_prompt_translate_engine.txt` (tenga en cuenta que afecta a todos los idiomas);
3. Prueba local para confirmar el efecto;
4. Enviar PR.

---

## 4. Proporcionar corpus de traducción revisado manualmente

Si eres un creador de mods de traducción y deseas proporcionar tu corpus de traducción como referencia para la traducción LLM, por favor inicia una solicitud en Issues. Necesitas proporcionar la siguiente información:

- El Mod ID de tu mod de traducción y el idioma de destino de la traducción;
- Una captura de pantalla de la página de administración de tu mod de traducción para demostrar que eres el autor del mod;
- Indica claramente en el Issue que estás dispuesto a proporcionar el corpus de traducción;
- Si hay circunstancias especiales (licencias especiales, etc.), explícalas también;
- Asegúrate de que el corpus que proporciones sea de alta calidad.

Bajo tu autorización, el proyecto incluirá tu mod en la lista de mods de traducción de referencia en `config/ref_translation_mods.json`, y el pipeline sincronizará automáticamente tu texto traducido como corpus de referencia RAG.

---

## 5. Contribuciones al pipeline y desarrollo de herramientas

La automatización de este proyecto se divide en dos partes:

**Módulo de canalización (`src/`, C# / .NET 10)**: Contiene 15 módulos ejecutados en orden, más 2 módulos independientes (`WorkshopMonitor` descubridor de mods, `DocGenerator` generador de documentos), que se encargan del proceso completo desde la inicialización de SteamCMD, descarga de mods, extracción de texto, revisión de contenido, cálculo de Embedding, búsqueda RAG hasta la traducción LLM y la salida final. Consulte [Referencia técnica](../technical_reference/technical_reference_es.md).

**Scripts auxiliares (.github/)**: Se utilizan para la automatización de GitHub.

Si deseas:

* Corregir errores en módulos del pipeline o scripts existentes;
* Añadir nuevas funciones o nuevos módulos al pipeline;
* Optimizar el rendimiento o la estructura del código;
* Mejorar las plantillas de prompt o la estrategia RAG;

Puedes seguir los siguientes pasos:

1. Haz un fork de este repositorio y clónalo localmente;
2. Crea una nueva rama basada en la rama más reciente;
3. Modifica o añade archivos en el directorio correspondiente:
- Modificación del módulo del pipeline → `src/<nombre_módulo>/`;
- Modificaciones de flujo de trabajo de CI → `.github/workflows/`;
- Modificación de plantillas de prompt → `src/prompt_templates/`;
4. Antes de enviar, intenta:

* Mantener el estilo de código original;
* Añadir comentarios necesarios;
* Si es posible, incluir pruebas simples o instrucciones de uso;
5. Envía las modificaciones a través de un PR y explica en la descripción:

* El propósito del cambio;
* Los directorios / módulos / scripts que pueden verse afectados;
* Si implica cambios disruptivos.

---

## 6. Acuerdo de derechos de autor y licencia

> **Nota importante:**
> El acuerdo de derechos de autor y licencia está diseñado para proteger los derechos e intereses legítimos del proyecto, los autores, los colaboradores y los jugadores, evitando malentendidos por "acuerdo tácito" o "por defecto". Por favor, léelo atentamente.
> Los derechos de autor y la licencia se rigen por el contenido del archivo README.md. Esta sección solo proporciona una descripción más comprensible.

### 6.1 Principio básico: tú conservas los derechos de autor y concedes licencia al proyecto

* Aún conservas los derechos de autor sobre el contenido que creas (traducciones, imágenes, scripts/programas, etc.);
* Pero al enviar este contenido a este proyecto y ser aceptado (fusionado), aceptas licenciar su uso a otros de acuerdo con la licencia de código abierto/compartida adoptada por este proyecto.

Esto significa:

* **Todavía puedes** seguir usando y mostrando tu trabajo en otros lugares;
* Pero **no puedes** exigir que este proyecto u otros usuarios que hayan obtenido legalmente el trabajo "retiren la autorización" o "eliminen versiones históricas" después de que la contribución se haya fusionado.

### 6.2 Licencia para contenido textual y gráfico (CC BY-NC-SA 4.0)

Para el siguiente contenido que envíes:

* Traducciones, revisiones y correcciones de textos del juego;
* Documentación del proyecto, textos explicativos;
* Imágenes y recursos artísticos creados específicamente para este proyecto;

Una vez aceptado y fusionado en este repositorio, se considerará que aceptas:

1. Estos contenidos se licencian bajo **Atribución-NoComercial-CompartirIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviado **CC BY-NC-SA 4.0**);
2. Project Babel y todos los usuarios que obtengan este contenido pueden, bajo la condición de **cumplir con los términos de CC BY-NC-SA 4.0**:
* Compartir, copiar y redistribuir este contenido;
* Modificarlo y recrearlo para usos no comerciales;
3. Aceptas que, dentro de lo permitido por la ley aplicable, esta licencia es **no exclusiva, mundial, libre de regalías e irrevocable**;
4. Incluso si abandonas o dejas de participar en este proyecto en el futuro, este proyecto puede continuar usando y redistribuyendo el contenido relevante que hayas enviado y fusionado según CC BY-NC-SA 4.0.

> Si no aceptas la forma de licencia anterior, no envíes contribuciones de texto o imágenes a este proyecto,
> o comunícate con antelación con los mantenedores del proyecto para confirmar si se puede colaborar de otra manera.

### 6.3 Licencia para scripts y código de herramientas (GPL-3.0)

Para lo que envíes y sea aceptado:

* Guiones de automatización;
* Herramientas de construcción/exportación;
* Otros códigos de programa utilizados para manejar este proyecto de localización;

En ausencia de una declaración especial, se considera que aceptas:

1. El código está licenciado bajo **GPL-3.0** (GNU General Public License versión 3);
2. Los mantenedores del proyecto pueden modificarlo, fusionarlo y distribuirlo dentro del alcance permitido por GPL-3.0;
3. También puedes continuar desarrollando otros proyectos basados en el mismo código, siempre que cumplas con los términos de GPL-3.0.

Para evitar introducir conflictos de licencia, intente:

* No introducir código de terceros **incompatible con GPL-3.0** sin confirmación;
* Si es necesario citar una biblioteca de terceros, indique claramente su origen y licencia en el PR, y confirme su compatibilidad.

### 6.4 Derechos de autor de obras upstream y del juego original

Este proyecto es un proyecto de **traducción no oficial** para los mods relacionados con "Project Zomboid" (Project Zomboid).

* Los derechos de autor del juego original y de cada mod pertenecen a sus respectivos autores/editoriales;
* Este proyecto solo se dedica a la traducción de texto, ajustes de pulido y organización de algunos recursos complementarios;
* Al enviar contenido, los contribuyentes deben asegurarse de:
* No copiar directamente textos de localización o recursos artísticos de terceros no autorizados;
* Respetar los derechos de los autores originales y autores de mods, y no realizar reproducciones que infrinjan derechos.

---

## 7. Comunicación y colaboración

Si tienes dudas sobre:

* Términos de licencia;
* No estás seguro si cierto contenido puede ser contribuido;
* Deseas licenciar tu trabajo de una manera especial (por ejemplo, solo permitir uso no comercial pero no permitir adaptación, etc.);

Bienvenido a contactar a los mantenedores del proyecto a través de los siguientes métodos:

* Enviar un Issue para discutir;
* Otros métodos de contacto proporcionados públicamente por los mantenedores.

Haremos todo lo posible por encontrar una solución que equilibre el desarrollo saludable del proyecto respetando los derechos e intereses de todas las partes.

---

## 8. Apoyo financiero

Durante la ejecución del proyecto, debido a la adición de nuevos mods, actualización de contenido de texto de mods antiguos, etc., es necesario llamar continuamente a la API de LLM para traducción. Y para restringir el comportamiento del LLM, además del texto básico del mod, también se necesita proporcionar una gran cantidad de contenido de prompts (incluyendo prompts base, reglas de traducción, glosarios, restricciones de entrada/salida, resultados de consultas semánticas, etc.), que consumen tokens muy por encima del texto original. Por lo tanto, el proyecto necesita apoyo financiero.

Si deseas proporcionar apoyo financiero, por favor contacta a los mantenedores del proyecto. ¡Muchas gracias!

---

¡Gracias de nuevo por estar dispuesto a contribuir a este proyecto!
Cada una de tus contribuciones beneficiará a más jugadores.
