# Guia de contribució (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Gràcies per la teva disposició a contribuir al **Project Babel — el projecte de traducció automàtica amb LLM per a mods de Project Zomboid**! Tant si es tracta de corregir un error, afegir una funció, escriure plantilles de prompt o proporcionar traduccions de referència, cada contribució compta!

L'ús de l'API LLM per a la traducció té un cost en tokens. Perquè el projecte pugui funcionar de manera sostenible a llarg termini, el teu generós suport és molt apreciat!

> ⚠️ **Avís important:**
> Abans d'enviar res a aquest repositori, assegura''t de llegir i comprendre la secció "Drets d''autor i llicències".
> Un cop enviat i fusionat, es considera que acceptes els termes de llicència corresponents.

---

## Abans de començar

Llegeix el `README.md` del projecte per entendre:

- Els objectius generals i l''estat actual d''aquest projecte;
- Com els jugadors habituals fan servir aquest projecte (per a les teves pròpies proves);
- Detalls tècnics del projecte.

---

## Com puc contribuir?

Pots triar una o més maneres de participar segons els teus interessos i habilitats:

- Proporcionar regles de traducció per a un idioma de destinació
- Proporcionar un diccionari terminològic per a un idioma de destinació
- Millorar els prompts del sistema
- Proporcionar corpus de traducció revisats manualment
- Millorar els mòduls del pipeline (.NET) i scripts d''automatització
- Informar de problemes i suggerir millores (a través d''Issues)
- Proporcionar suport financer per a les crides a l''API del LLM

A continuació s''expliquen els principals escenaris de contribució.

---

## Proporcionar regles de traducció, diccionaris terminològics i millorar els prompts del sistema

Les plantilles de prompt del pipeline es troben a `src/prompt_templates/`, amb l''estructura següent:

- `system_prompt_translate_engine.txt`: el prompt del sistema del motor de traducció global (compartit per tots els idiomes);
- `<codi_idioma>/translation_dictionary_<codi_idioma>.json`: el diccionari terminològic per a aquell idioma;
- `<codi_idioma>/translation_schema_<codi_idioma>.md`: les regles de traducció i restriccions d''estil per a aquell idioma.

Passos per contribuir:

1. Crea un subdirectori sota `src/prompt_templates/` per al teu idioma i afegeix els fitxers de diccionari i regles de traducció;
2. Si necessites ajustar el comportament global de traducció, modifica `system_prompt_translate_engine.txt` (nota: afecta tots els idiomes);
3. Prova localment per confirmar els resultats;
4. Envia un PR.

---

## Proporcionar corpus revisats manualment

Si ets autor d''un mod de traducció i estàs disposat a proporcionar el teu corpus de traducció com a referència per al LLM, envia una sol·licitud a través d''un Issue. Has de proporcionar la informació següent:

- El Mod ID del teu mod de traducció i l''idioma de destinació;
- Una captura de pantalla de la pàgina d''administració del teu mod de traducció per demostrar que n''ets l''autor;
- Una declaració clara a l''Issue que estàs disposat a proporcionar el corpus de traducció;
- Si hi ha circumstàncies especials (llicència especial, etc.), explica-les;
- Assegura''t que el corpus proporcionat sigui d''alta qualitat.

Amb la teva autorització, el projecte afegirà el teu mod a la llista de mods de traducció de referència `config/ref_translation_mods.json`, i el pipeline sincronitzarà automàticament els teus textos traduïts com a corpus de referència RAG.

---

## Contribucions al desenvolupament del pipeline i eines

L''automatització d''aquest projecte es divideix en dues parts:

**Mòduls del pipeline (`src/`, C# / .NET 10)**: Conté 15 mòduls executats seqüencialment, responsables del flux complet des de la descàrrega de mods, extracció de text, revisió de contingut, càlcul d''embeddings, recuperació RAG fins a la traducció LLM i la sortida final. Consulta la [referència tècnica](../technical_reference/technical_reference_ca.md) per a més detalls.

**Scripts auxiliars (`.github/`)**: Utilitzats per a l''automatització de GitHub.

Si desitges:

* Corregir errors en mòduls del pipeline o scripts existents;
* Afegir noves funcions o mòduls al pipeline;
* Optimitzar el rendiment o l''estructura del codi;
* Millorar les plantilles de prompt o les estratègies RAG;

Pots seguir aquests passos:

1. Fes un fork d''aquest repositori i clona''l localment;
2. Crea una nova branca des de la branca més recent;
3. Modifica o afegeix fitxers als directoris corresponents:
   - Canvis en mòduls del pipeline → `src/<nom_del_mòdul>/`;
   - Canvis en scripts → `scripts/`;
   - Canvis en plantilles de prompt → `src/prompt_templates/`;
4. Abans d''enviar, intenta:

   * Mantenir l''estil de codi existent;
   * Afegir els comentaris necessaris;
   * Si és possible, incloure proves simples o instruccions d''ús;
5. Envia els canvis via PR, explicant a la descripció:

   * L''objectiu dels canvis;
   * Els directoris / mòduls / scripts que poden veure''s afectats;
   * Si implica canvis que trenquen la compatibilitat.

---

## Drets d''autor i llicències

> **Recordatori amistós:**
> Els termes de drets d''autor i llicències estan dissenyats per protegir els drets i interessos legítims del projecte, autors, contribuïdors i jugadors, i per evitar malentesos derivats d''"acords tàcits" o "presumpcions per defecte". Llegeix-los atentament.
> Els drets d''autor i les llicències es regeixen pel contingut del fitxer README.md; aquesta secció només proporciona una descripció més accessible.

### 1. Principi bàsic: Tu conserves els drets d''autor, i alhora llicencies el projecte per utilitzar la teva obra

* Encara tens els drets d''autor sobre el contingut que crees (traduccions, imatges, scripts/programes, etc.);
* No obstant això, un cop aquest contingut s''envia a aquest projecte i és acceptat (fusionat),
  acceptes llicenciar a altres l''ús d''aquest contingut sota la llicència de codi obert/compartida adoptada per aquest projecte.

Això significa:

* **Encara pots** continuar utilitzant i mostrant la teva obra en altres llocs;
* Però **no pots**, després que la teva contribució sigui fusionada, exigir a aquest projecte o a altres usuaris que hagin obtingut legalment l''obra que "revokin la llicència" o "eliminin versions històriques".

### 2. Llicència de textos, imatges i continguts similars (CC BY-NC-SA 4.0)

Per al contingut següent que enviïs:

* Traduccions de textos del joc, revisions i correccions;
* Documentació del projecte i textos explicatius;
* Imatges i recursos artístics creats específicament per a aquest projecte;

Un cop acceptat i fusionat en aquest repositori, es considera que acceptes que:

1. Aquests continguts es llicencien sota **Reconeixement-NoComercial-CompartirIgual 4.0 Internacional**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreujat **CC BY-NC-SA 4.0**);
2. Project Babel i tots els usuaris que rebin aquest contingut poden, **en compliment dels termes CC BY-NC-SA 4.0**:

   * Compartir, copiar i redistribuir aquest contingut;
   * Modificar-lo i crear obres derivatives amb finalitats no comercials;
3. Acceptes que aquesta llicència és **no exclusiva, mundial, lliure de regalies i irrevocable** en la mesura permesa per la llei aplicable;
4. Fins i tot si posteriorment et retires o deixes de participar en aquest projecte, el projecte pot continuar utilitzant i redistribuint el contingut rellevant que hagis enviat i que hagi estat fusionat, sota CC BY-NC-SA 4.0.

> Si no acceptes els termes de llicència anteriors, no enviïs contribucions de text o imatge a aquest projecte,
> o comunica''t prèviament amb els mantenidors del projecte per confirmar si és possible col·laborar d''una altra manera.

### 3. Llicència d''scripts i codi d''eines (GPL-3.0)

Per al següent que enviïs i sigui acceptat:

* Scripts d''automatització;
* Eines de construcció/exportació;
* Altre codi de programa utilitzat per processar aquest projecte de traducció;

En absència de declaracions especials, es considera que acceptes que:

1. El codi es llicencia sota **GPL-3.0** (GNU General Public License versió 3);
2. Els mantenidors del projecte poden modificar-lo, fusionar-lo i distribuir-lo dins l''àmbit permès per GPL-3.0;
3. Tu també pots continuar altres projectes basats en el mateix codi, sempre que compleixis els termes de GPL-3.0.

Per evitar conflictes de llicència, intenta:

* No introduir codi de tercers **incompatible amb GPL-3.0** sense confirmació prèvia;
* Si necessites fer referència a biblioteques de tercers, indica clarament la seva font i llicència al PR i confirma la compatibilitat.

### 4. Obres anteriors i drets d''autor del joc original

Aquest projecte és un projecte de **traducció no oficial** per a mods relacionats amb *Project Zomboid*:

* Els drets d''autor del joc original i de cada mod pertanyen als seus respectius autors/editors;
* Aquest projecte només implica la creació i organització de traduccions de text, ajustos d''estil i alguns recursos de suport;
* Els contribuïdors, en enviar contingut, han d''assegurar-se:

  * No copiar directament textos de traducció o recursos artístics de tercers no autoritzats;
  * Respectar els drets dels autors originals i dels autors de mods, i no realitzar redistribució infractora.

---

## Comunicació i col·laboració

Si tens:

* Preguntes sobre els termes de la llicència;
* Dubtes sobre si es pot contribuir amb cert contingut;
* El desig de llicenciar la teva obra d''una manera especial (per exemple, només ús no comercial sense adaptació permesa);

No dubtis a contactar amb els mantenidors del projecte a través de:

* Enviament d''un Issue per a discussió;
* Altres mitjans de contacte públics dels mantenidors.

Farem tot el possible per trobar una solució que equilibri el desenvolupament saludable del projecte tot respectant els drets i interessos de totes les parts.

---

## Suport financer

Durant el funcionament del projecte, a causa de l''addició de nous mods i les actualitzacions de text dels mods existents, cal cridar contínuament l''API del LLM per a la traducció. Per restringir el comportament del LLM, a més dels textos bàsics dels mods, es necessita una gran quantitat de contingut de prompt (incloent prompts bàsics, regles de traducció, taules terminològiques, restriccions d''entrada/sortida, resultats de cerca semàntica, etc.), cosa que consumeix molts més tokens que els textos originals. Per tant, el projecte necessita suport financer.

Si desitges proporcionar suport financer, contacta amb els mantenidors del projecte. Moltes gràcies!

---

Una vegada més, gràcies per la teva disposició a contribuir a aquest projecte!
Cada contribució que fas beneficia més jugadors!
