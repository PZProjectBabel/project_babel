# Guia de contribució (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Taula de continguts

- [1. Abans de començar](#1-abans-de-començar)
- [2. Com puc contribuir?](#2-com-puc-contribuir)
- [3. Proporcionar regles de traducció, diccionari de termes, millorar prompts del sistema](#3-proporcionar-regles-de-traducció-diccionari-de-termes-millorar-prompts-del-sistema)
- [4. Proporcioneu corpus de correcció humana](#4-proporcioneu-corpus-de-correcció-humana)
- [5. Contribució al desenvolupament de la canalització i eines](#5-contribució-al-desenvolupament-de-la-canalització-i-eines)
- [6. Drets d'autor i acords de llicència](#6-drets-dautor-i-acords-de-llicència)
  - [6.1 Principi bàsic: tu conserves els drets d'autor, i alhora autoritzes el projecte a utilitzar-ho](#61-principi-bàsic-tu-conserves-els-drets-dautor-i-alhora-autoritzes-el-projecte-a-utilitzar-ho)
  - [6.2 Llicència per a textos, imatges i altres continguts (CC BY-NC-SA 4.0)](#62-llicència-per-a-textos-imatges-i-altres-continguts-cc-by-nc-sa-40)
  - [6.3 Llicència per a scripts i codi d'eines (GPL-3.0)](#63-llicència-per-a-scripts-i-codi-deines-gpl-30)
  - [6.4 Drets d'autor de les obres originals i del joc original](#64-drets-dautor-de-les-obres-originals-i-del-joc-original)
- [7. Comunicació i col·laboració](#7-comunicació-i-collaboració)
- [8. Suport financer](#8-suport-financer)

---

Moltes gràcies per estar disposat a contribuir al **Project Babel - Projecte de traducció automàtica LLM per al mod de Project Zomboid**! Tant si es tracta de corregir un error, afegir una funció, redactar una plantilla de prompt com de proporcionar una traducció de referència!

Cridar a l'API de LLM per traduir requereix pagar per tokens. Perquè el projecte pugui funcionar de manera estable a llarg termini, esperem que pugueu ajudar generosament!

> ⚠️ **Recordatori important:**
> Abans d'enviar qualsevol contingut a aquest repositori, assegureu-vos de llegir i entendre la secció "Acord de drets d'autor i llicència".
> Un cop enviat i fusionat, es considera que accepteu els termes de llicència corresponents.

---

## 1. Abans de començar

Si us plau, llegiu primer el `README.md` del projecte per entendre:
- L'objectiu general i l'estat actual del projecte;
- Com els jugadors normals poden utilitzar aquest projecte (per facilitar l'auto-prova);
- Detalls tècnics del projecte.

---

## 2. Com puc contribuir?

Podeu triar una o més maneres de participar segons els vostres interessos i habilitats:

- Proporcionar regles de traducció per a l'idioma de destinació
- Proporcionar un diccionari de termes de traducció per a l'idioma de destinació
- Millorar els prompts del sistema
- Proporcionar corpus de text traduït revisat manualment
- Millorar els mòduls de la canonada (.NET) i els scripts d'automatització
- Informar de problemes, suggerir millores (explicar a Issues)
- Proporcionar suport financer per a les crides a LLM

A continuació, es fan algunes explicacions sobre els principals escenaris de contribució.

---

## 3. Proporcionar regles de traducció, diccionari de termes, millorar prompts del sistema

Les plantilles de prompt de la canonada es troben a `src/prompt_templates/`, amb l'estructura següent:

- `system_prompt_translate_engine.txt`: Prompt del sistema del motor de traducció global (compartit per tots els idiomes);
- `<codi_dioma>/translation_dictionary_<codi_dioma>.json`: Diccionari de termes per a aquest idioma;
- `<codi_dioma>/translation_schema_<codi_dioma>.md`: Regles de traducció i restriccions d'estil per a aquest idioma.

Passos per a la contribució:

1. Creeu un subdirectori per al vostre idioma a `src/prompt_templates/` i afegiu el diccionari de termes i el fitxer de regles de traducció;
2. Si cal ajustar el comportament de traducció global, modifiqueu `system_prompt_translate_engine.txt` (tingueu en compte que afecta tots els idiomes);
3. Confirmeu l'efecte amb proves locals;
4. Envieu un PR.

---

## 4. Proporcioneu corpus de correcció humana

Si sou un creador de mods de traducció i voleu proporcionar el vostre corpus de traducció com a referència per a la traducció LLM, si us plau, inicieu una sol·licitud a Issue. Heu de proporcionar la següent informació:

- El Mod ID del vostre mod de traducció i l'idioma objectiu de la traducció;
- Una captura de pantalla de la pàgina d'administració del vostre mod de traducció per demostrar que en sou l'autor;
- Indiqueu clarament a Issue que esteu disposat a proporcionar el corpus de traducció;
- Si hi ha circumstàncies especials (llicència especial, etc.), si us plau, expliqueu-ho juntament;
- Assegureu-vos que el corpus proporcionat sigui d'alta qualitat.

Amb la vostra autorització, el projecte inclourà el vostre mod a la llista de mods de traducció de referència a `config/ref_translation_mods.json`, i la canalització sincronitzarà automàticament el vostre text traduït com a corpus de referència RAG.

---

## 5. Contribució al desenvolupament de la canalització i eines

L'automatització d'aquest projecte es divideix en dues parts:

**Mòdul de pipeline (`src/`, C# / .NET 10)**: conté 15 mòduls executats en seqüència, més 2 mòduls independents (`WorkshopMonitor` descobridor de mods, `DocGenerator` generador de documentació), que s'encarreguen del flux complet des de la inicialització de SteamCMD, descàrrega de mods, extracció de text, revisió de contingut, càlcul d'Embedding, recuperació RAG fins a la traducció LLM i la sortida final. Vegeu [Referència tècnica](../technical_reference/technical_reference_ca.md).

**Scripts auxiliars (.github/)**: utilitzats per a l'automatització de GitHub.

Si voleu:

* Corregir errors als mòduls de canalització o scripts existents;
* Afegir noves funcionalitats o nous mòduls a la canalització;
* Optimitzar el rendiment o l'estructura del codi;
* Millorar les plantilles de prompt o l'estratègia RAG;

Podeu seguir els següents passos:

1. Feu un fork d'aquest repositori i cloneu-lo localment;
2. Creeu una nova branca basada en la branca més recent;
3. Modifiqueu o afegiu fitxers al directori corresponent:
- Modificació de mòdul de canalització → `src/<nom_del_mòdul>/`;
- Modificació del flux de treball CI → `.github/workflows/`;
- Modificació de plantilla de prompt → `src/prompt_templates/`;
4. Abans d'enviar, si us plau, intenteu:

* Mantenir l'estil de codi original;
* Afegir comentaris necessaris;
* Si és possible, acompanyeu amb proves senzilles o instruccions d'ús;
5. Envieu les modificacions mitjançant un PR i expliqueu a la descripció:

* Propòsit del canvi;
* Directoris / mòduls / scripts que podrien veure's afectats;
* Si implica canvis que trenquen la compatibilitat.

---

## 6. Drets d'autor i acords de llicència

> **Avís important:**
> L'acord de drets d'autor i llicència està dissenyat per protegir els drets legítims del projecte, dels autors, dels col·laboradors i dels jugadors, evitant malentesos per "complicitats" o "per defecte". Si us plau, llegiu-lo atentament.
> Els drets d'autor i la llicència es regeixen pel contingut del fitxer README.md; aquesta secció només ofereix una descripció més entenedora.

### 6.1 Principi bàsic: tu conserves els drets d'autor, i alhora autoritzes el projecte a utilitzar-ho

* Continues tenint els drets d'autor sobre el contingut que has creat (traduccions, imatges, scripts/programes, etc.);
* Però després de presentar aquest contingut al projecte i que sigui acceptat (fusionat), acceptes autoritzar-ne l'ús a tercers segons la llicència de codi obert/compartit adoptada per aquest projecte.

Això significa:

* **Encara pots** continuar utilitzant i mostrant les teves obres en altres llocs;
* Però **no pots** exigir al projecte o a altres usuaris que hagin obtingut legalment les obres que "retirin l'autorització" o "eliminin les versions històriques" després que la contribució hagi estat fusionada.

### 6.2 Llicència per a textos, imatges i altres continguts (CC BY-NC-SA 4.0)

Per al contingut següent que presentis:

* Traducció, revisió i correcció de textos del joc;
* Documentació del projecte, text explicatiu;
* Imatges i recursos artístics creats específicament per a aquest projecte;

Un cop acceptats i fusionats en aquest repositori, es considera que acceptes:

1. Aquest contingut es llicencia sota **Reconeixement-NoComercial-CompartirIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreujat **CC BY-NC-SA 4.0**);
2. Project Babel i tots els usuaris que obtinguin aquest contingut poden, sota la condició de **complir els termes de CC BY-NC-SA 4.0**:
* Compartir, copiar i redistribuir aquest contingut;
* Modificar-lo i crear-ne obres derivades per a usos no comercials;
3. Acceptes que, dins del permès per la legislació aplicable, aquesta llicència és **no exclusiva, mundial, lliure de drets d'autor i irrevocable**;
4. Fins i tot si decideixes retirar-te o deixar de participar en aquest projecte en el futur, el projecte pot continuar utilitzant i redistribuint el contingut rellevant que hagis presentat i que hagi estat fusionat, d'acord amb CC BY-NC-SA 4.0.

> Si no acceptes la modalitat de llicència anterior, si us plau no presentis contribucions de text o imatges a aquest projecte,
> o comunica't prèviament amb els mantenidors del projecte per confirmar si es pot col·laborar d'una altra manera.

### 6.3 Llicència per a scripts i codi d'eines (GPL-3.0)

Per al que presentis i sigui acceptat:

* Scripts d'automatització;
* Eines de construcció/exportació;
* Altres codis de programa per a gestionar aquest projecte de traducció;

En absència de declaració especial, es considera que accepteu:

1. El codi es llicencia sota **GPL-3.0** (GNU General Public License versió 3);
2. Els mantenedors del projecte poden modificar, fusionar i distribuir-lo dins dels límits permesos per GPL-3.0;
3. També podeu continuar desenvolupant altres projectes basats en el mateix codi, sempre que compliu amb els termes de GPL-3.0.

Per evitar conflictes de llicència, si us plau, intenteu:

* No introduïu codi de tercers **incompatible amb GPL-3.0** sense verificar-ho;
* Si és necessari fer referència a una biblioteca de tercers, especifiqueu clarament la seva font i llicència al PR, i confirmeu la seva compatibilitat.

### 6.4 Drets d'autor de les obres originals i del joc original

Aquest projecte és un projecte de **traducció no oficial** per als mods relacionats amb el joc Project Zomboid:

* Els drets d'autor del joc original i de cada mod pertanyen als seus respectius autors/editors;
* Aquest projecte només crea i organitza traduccions de text, ajustaments de poliment i alguns recursos complementaris;
* En enviar contingut, els col·laboradors han d'assegurar que:
* No copieu directament textos o recursos artístics de traducció de tercers sense autorització;
* Respecteu els drets dels autors originals i dels creadors de mods, no feu reproduccions infractores.

---

## 7. Comunicació i col·laboració

Si teniu dubtes sobre:

* Clàusules de llicència;
* No esteu segurs si un contingut es pot contribuir;
* Desitgeu llicenciar la vostra obra d'una manera especial (per exemple, només ús no comercial sense permetre adaptacions, etc.);

Benvinguts a contactar amb els mantenedors del projecte a través dels mitjans següents:

* Obriu un Issue per discutir;
* Altres mitjans de contacte proporcionats públicament pels mantenedors.

Intentarem trobar una solució que equilibri el desenvolupament saludable del projecte tot respectant els drets de totes les parts.

---

## 8. Suport financer

Durant l'execució del projecte, a causa de l'addició de nous mods i l'actualització del contingut de text dels mods antics, cal trucar contínuament a l'API de l'LLM per traduir. I per restringir el comportament de l'LLM, a més del text bàsic del mod, cal proporcionar una gran quantitat de contingut de prompt (incloent-hi el prompt bàsic, regles de traducció, glossaris, restriccions d'entrada/sortida, resultats de consultes semàntiques, etc.). Aquest contingut consumeix molts més tokens que el text original. Per tant, el projecte necessita suport financer.

Si esteu disposat a proporcionar suport financer, poseu-vos en contacte amb els mantenedors del projecte. Moltes gràcies!

---

Un altre cop, gràcies per estar disposat a contribuir a aquest projecte!
Cada contribució teva beneficia més jugadors!
