# Guía de contribución (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

¡Gracias por tu disposición a contribuir al **Project Babel — el proyecto de traducción automática con LLM para mods de Project Zomboid**! Ya sea corrigiendo un error, añadiendo una función, escribiendo plantillas de prompt o proporcionando traducciones de referencia, ¡cada contribución cuenta!

Llamar a la API del LLM para traducir tiene un coste en tokens. Para que el proyecto pueda funcionar de forma sostenible a largo plazo, ¡tu generoso apoyo es muy apreciado!

> ⚠️ **Aviso importante:**
> Antes de enviar cualquier cosa a este repositorio, asegúrate de leer y comprender la sección "Derechos de autor y licencias".
> Una vez enviado y fusionado, se considera que aceptas los términos de licencia correspondientes.

---

## Antes de empezar

Lee el README.md del proyecto para entender:

- Los objetivos generales y el estado actual de este proyecto;
- Cómo usan los jugadores este proyecto (para tus propias pruebas);
- Los detalles técnicos del proyecto.

---

## ¿Cómo puedo contribuir?

Puedes elegir una o varias formas de participar según tus intereses y habilidades:

- Proporcionar reglas de traducción para un idioma de destino
- Proporcionar un diccionario terminológico para un idioma de destino
- Mejorar los prompts del sistema
- Proporcionar corpus de traducción revisados manualmente
- Mejorar los módulos del pipeline (.NET) y los scripts de automatización
- Informar de problemas y sugerir mejoras (a través de Issues)
- Proporcionar apoyo financiero para las llamadas a la API del LLM

A continuación se explican los principales escenarios de contribución.

---

## Proporcionar reglas de traducción, diccionarios terminológicos y mejorar los prompts del sistema

Las plantillas de prompt del pipeline se encuentran en src/prompt_templates/, con la siguiente estructura:

- system_prompt_translate_engine.txt: el prompt del sistema del motor de traducción global (común para todos los idiomas);
- <código_idioma>/translation_dictionary_<código_idioma>.json: el diccionario terminológico para ese idioma;
- <código_idioma>/translation_schema_<código_idioma>.md: las reglas de traducción y restricciones de estilo para ese idioma.

Pasos para contribuir:

1. Crea un subdirectorio bajo src/prompt_templates/ para tu idioma y añade los archivos de diccionario y reglas de traducción;
2. Si necesitas ajustar el comportamiento global de traducción, modifica system_prompt_translate_engine.txt (nota: esto afecta a todos los idiomas);
3. Prueba localmente para confirmar los resultados;
4. Envía un PR.

---

## Proporcionar corpus revisados manualmente

Si eres autor de un mod de traducción y estás dispuesto a proporcionar tu corpus de traducción como referencia para el LLM, presenta una solicitud a través de un Issue. Debes proporcionar la siguiente información:

- El Mod ID de tu mod de traducción y el idioma de destino;
- Una captura de pantalla de la página de administración de tu mod de traducción para demostrar que eres el autor;
- Una declaración clara en el Issue de que estás dispuesto a proporcionar el corpus de traducción;
- Si hay circunstancias especiales (licencia especial, etc.), explícalas;
- Asegúrate de que el corpus proporcionado sea de alta calidad.

Con tu autorización, el proyecto añadirá tu mod a la lista de mods de traducción de referencia config/ref_translation_mods.json, y el pipeline sincronizará automáticamente tus textos traducidos como corpus de referencia RAG.

---

## Contribuciones al desarrollo del pipeline y herramientas

La automatización de este proyecto se divide en dos partes:

**Módulos del pipeline (src/, C# / .NET 10)**: Contiene 15 módulos ejecutados secuencialmente, responsables del flujo completo desde la descarga de mods, extracción de texto, revisión de contenido, cálculo de embeddings, recuperación RAG hasta la traducción LLM y la salida final. Consulta la [referencia técnica](../technical_reference/technical_reference_es.md) para más detalles.

**Scripts auxiliares (.github/)**: Usados para la automatización de GitHub.

Si deseas:

* Corregir errores en módulos del pipeline o scripts existentes;
* Añadir nuevas funciones o módulos al pipeline;
* Optimizar el rendimiento o la estructura del código;
* Mejorar las plantillas de prompt o las estrategias RAG;

Puedes seguir estos pasos:

1. Haz un fork de este repositorio y clónalo localmente;
2. Crea una nueva rama desde la rama más reciente;
3. Modifica o añade archivos en los directorios correspondientes:
   - Cambios en módulos del pipeline → src/<nombre_módulo>/;
   - Cambios en scripts → scripts/;
   - Cambios en plantillas de prompt → src/prompt_templates/;
4. Antes de enviar, intenta:

   * Mantener el estilo de código existente;
   * Añadir los comentarios necesarios;
   * Si es posible, incluir pruebas simples o instrucciones de uso;
5. Envía los cambios mediante PR, explicando en la descripción:

   * El propósito de los cambios;
   * Los directorios / módulos / scripts que pueden verse afectados;
   * Si implica cambios que rompen la compatibilidad.

---

## Derechos de autor y licencias

> **Recordatorio amistoso:**
> Los términos de derechos de autor y licencias están diseñados para proteger los derechos e intereses legítimos del proyecto, los autores, los contribuyentes y los jugadores, y para evitar malentendidos derivados de "acuerdos tácitos" o "presunciones por defecto". Léelos atentamente.
> Los derechos de autor y las licencias se rigen por el contenido del archivo README.md; esta sección solo proporciona una descripción más accesible.

### 1. Principio básico: Tú conservas los derechos de autor, a la vez que otorgas licencia al proyecto para usar tu trabajo

* Sigues teniendo los derechos de autor sobre el contenido que creas (traducciones, imágenes, scripts/programas, etc.);
* Sin embargo, una vez que este contenido se envía a este proyecto y es aceptado (fusionado),
  aceptas licenciar a otros el uso de este contenido bajo la licencia de código abierto/compartida adoptada por este proyecto.

Esto significa:

* **Todavía puedes** seguir usando y mostrando tu trabajo en otros lugares;
* Pero **no puedes**, después de que tu contribución sea fusionada, exigir que este proyecto u otros usuarios que hayan obtenido legalmente el trabajo "revoquen la licencia" o "eliminen versiones históricas".

### 2. Licencia de textos, imágenes y contenidos similares (CC BY-NC-SA 4.0)

Para el siguiente contenido que envíes:

* Traducciones de textos del juego, revisiones y correcciones;
* Documentación del proyecto y textos explicativos;
* Imágenes y recursos artísticos creados específicamente para este proyecto;

Una vez aceptado y fusionado en este repositorio, se considera que aceptas que:

1. Estos contenidos están bajo la licencia **Atribución-NoComercial-CompartirIgual 4.0 Internacional**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviado **CC BY-NC-SA 4.0**);
2. Project Babel y todos los usuarios que reciban este contenido pueden, **cumpliendo los términos de CC BY-NC-SA 4.0**:

   * Compartir, copiar y redistribuir este contenido;
   * Modificarlo y crear obras derivadas con fines no comerciales;
3. Aceptas que esta licencia es **no exclusiva, mundial, libre de regalías e irrevocable** en la medida permitida por la ley aplicable;
4. Incluso si más tarde te retiras o dejas de participar en este proyecto, el proyecto puede seguir usando y redistribuyendo el contenido relevante que hayas enviado y que haya sido fusionado, bajo CC BY-NC-SA 4.0.

> Si no aceptas los términos de licencia anteriores, no envíes contribuciones de texto o imágenes a este proyecto,
> o comunícate previamente con los mantenedores del proyecto para confirmar si es posible colaborar de otra manera.

### 3. Licencia de scripts y código de herramientas (GPL-3.0)

Para lo siguiente que envíes y sea aceptado:

* Scripts de automatización;
* Herramientas de compilación/exportación;
* Otro código de programa utilizado para procesar este proyecto de traducción;

En ausencia de declaraciones especiales, se considera que aceptas que:

1. El código está bajo la licencia **GPL-3.0** (GNU General Public License versión 3);
2. Los mantenedores del proyecto pueden modificarlo, fusionarlo y distribuirlo dentro del alcance permitido por GPL-3.0;
3. Tú también puedes continuar otros proyectos basados en el mismo código, siempre que cumplas los términos de GPL-3.0.

Para evitar conflictos de licencia, intenta:

* No introducir código de terceros **incompatible con GPL-3.0** sin confirmación previa;
* Si necesitas hacer referencia a bibliotecas de terceros, indica claramente su origen y licencia en el PR, y confirma su compatibilidad.

### 4. Obras anteriores y derechos de autor del juego original

Este proyecto es un proyecto de **traducción no oficial** para mods relacionados con *Project Zomboid*:

* Los derechos de autor del juego original y de cada mod pertenecen a sus respectivos autores/editores;
* Este proyecto solo implica la creación y organización de traducciones de texto, ajustes de estilo y algunos recursos complementarios;
* Los contribuyentes, al enviar contenido, deben asegurarse de:

  * No copiar directamente textos de traducción o recursos artísticos de terceros no autorizados;
  * Respetar los derechos de los autores originales y de los mods, y no realizar redistribuciones infractoras.

---

## Comunicación y colaboración

Si tienes:

* Preguntas sobre los términos de la licencia;
* Dudas sobre si cierto contenido puede ser contribuido;
* El deseo de licenciar tu trabajo de una manera especial (por ejemplo, solo uso no comercial pero sin adaptación permitida);

Puedes contactar a los mantenedores del proyecto a través de:

* Enviar un Issue para discutirlo;
* Otros medios de contacto públicos de los mantenedores.

Haremos todo lo posible por encontrar una solución que equilibre el desarrollo saludable del proyecto respetando los derechos e intereses de todas las partes.

---

## Apoyo financiero

Durante el funcionamiento del proyecto, debido a la adición de nuevos mods y las actualizaciones de texto de los mods existentes, es necesario llamar continuamente a la API del LLM para traducir. Para restringir el comportamiento del LLM, además de los textos básicos de los mods, se requiere una gran cantidad de contenido de prompt (incluyendo prompts básicos, reglas de traducción, tablas terminológicas, restricciones de entrada/salida, resultados de búsqueda semántica, etc.), lo que consume muchos más tokens que los textos originales. Por lo tanto, el proyecto necesita apoyo financiero.

Si deseas proporcionar apoyo financiero, contacta a los mantenedores del proyecto. ¡Muchas gracias!

---

¡Gracias de nuevo por tu disposición a contribuir a este proyecto!
¡Cada contribución que haces beneficia a más jugadores!
