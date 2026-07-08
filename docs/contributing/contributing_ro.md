# Ghid de contribuție (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Vă mulțumim pentru disponibilitatea de a contribui la **Project Babel — proiectul de traducere automată cu LLM pentru modurile Project Zomboid**! Fie că este vorba de corectarea unei erori, adăugarea unei funcționalități, scrierea de șabloane de prompt sau furnizarea de traduceri de referință — fiecare contribuție contează!

Apelarea API-ului LLM pentru traducere costă tokenuri. Pentru ca proiectul să poată funcționa sustenabil pe termen lung, sprijinul dumneavoastră generos este foarte apreciat!

> ⚠️ **Notă importantă:**
> Înainte de a trimite orice în acest depozit, vă rugăm să citiți și să înțelegeți secțiunea „Drepturi de autor și licențiere".
> Prin trimitere și fuzionare, se consideră că ați acceptat termenii de licență corespunzători.

---

## Înainte de a începe

Vă rugăm să citiți `README.md` al proiectului pentru a înțelege:

- Obiectivele generale și starea actuală a acestui proiect;
- Cum folosesc jucătorii obișnuiți acest proiect (pentru propriile teste);
- Detaliile tehnice ale proiectului.

---

## Cum pot contribui?

Puteți alege una sau mai multe modalități de participare în funcție de interesele și abilitățile dumneavoastră:

- Furnizarea de reguli de traducere pentru o limbă țintă
- Furnizarea unui dicționar terminologic pentru o limbă țintă
- Îmbunătățirea prompturilor de sistem
- Furnizarea de corpusuri de traducere corectate manual
- Îmbunătățirea modulelor pipeline (.NET) și a scripturilor de automatizare
- Raportarea problemelor și sugerarea de îmbunătățiri (prin Issues)
- Oferirea de sprijin financiar pentru apelurile API LLM

Mai jos sunt explicații pentru principalele scenarii de contribuție.

---

## Furnizarea de reguli de traducere, dicționare terminologice și îmbunătățirea prompturilor de sistem

Șabloanele de prompt ale pipeline-ului se află în `src/prompt_templates/`, cu următoarea structură:

- `system_prompt_translate_engine.txt`: promptul de sistem global al motorului de traducere (comun tuturor limbilor);
- `<cod_limbă>/translation_dictionary_<cod_limbă>.json`: dicționarul terminologic pentru limba respectivă;
- `<cod_limbă>/translation_schema_<cod_limbă>.md`: regulile de traducere și constrângerile de stil pentru limba respectivă.

Pași de contribuție:

1. Creați un subdirector sub `src/prompt_templates/` pentru limba dumneavoastră și adăugați fișierele dicționar și de reguli;
2. Dacă trebuie să ajustați comportamentul global de traducere, modificați `system_prompt_translate_engine.txt` (atenție: acest lucru afectează toate limbile);
3. Testați local pentru a confirma rezultatele;
4. Trimiteți un PR.

---

## Furnizarea de corpusuri corectate manual

Dacă sunteți autorul unui mod de traducere și sunteți dispus să furnizați corpusul dumneavoastră de traducere ca referință pentru LLM, vă rugăm să trimiteți o cerere printr-un Issue. Trebuie să furnizați următoarele informații:

- ID-ul modului (Mod ID) al modului dumneavoastră de traducere și limba țintă;
- O captură de ecran a paginii de administrare a modului de traducere pentru a dovedi calitatea de autor;
- O declarație clară în Issue că sunteți dispus să furnizați corpusul de traducere;
- Dacă există circumstanțe speciale (licență specială etc.), vă rugăm să le explicați;
- Vă rugăm să vă asigurați că corpusul furnizat este de înaltă calitate.

Cu autorizația dumneavoastră, proiectul va adăuga modul dumneavoastră în lista de moduri de traducere de referință `config/ref_translation_mods.json`, iar pipeline-ul va sincroniza automat textele traduse ca corpusuri de referință RAG.

---

## Contribuții la dezvoltarea pipeline-ului și a instrumentelor

Automatizarea din acest proiect este împărțită în două părți:

**Module pipeline (`src/`, C# / .NET 10)**: Conține 15 module executate secvențial, responsabile pentru fluxul complet de la descărcarea modurilor, extragerea textului, revizuirea conținutului, calculul embedding-urilor, regăsirea RAG până la traducerea LLM și ieșirea finală. Consultați [documentația tehnică](../translation_entry_pipeline_zh-hans.md) pentru detalii.

**Scripturi auxiliare (`.github/`)**: Utilizate pentru automatizarea GitHub.

Dacă doriți să:

* Corectați erori în modulele pipeline sau scripturile existente;
* Adăugați funcționalități sau module noi la pipeline;
* Optimizați performanța sau structura codului;
* Îmbunătățiți șabloanele de prompt sau strategiile RAG;

Puteți urma acești pași:

1. Faceți fork la acest depozit și clonați-l local;
2. Creați o ramură nouă din cea mai recentă ramură;
3. Modificați sau adăugați fișiere în directoarele corespunzătoare:
   - Modificări module pipeline → `src/<nume_modul>/`;
   - Modificări scripturi → `scripts/`;
   - Modificări șabloane prompt → `src/prompt_templates/`;
4. Înainte de trimitere, încercați să:

   * Păstrați stilul de cod existent;
   * Adăugați comentariile necesare;
   * Dacă este posibil, atașați teste simple sau instrucțiuni de utilizare;
5. Trimiteți modificările prin PR, explicând în descriere:

   * Scopul modificărilor;
   * Directoarele / modulele / scripturile care pot fi afectate;
   * Dacă implică modificări cu rupere de compatibilitate.

---

## Drepturi de autor și licențiere

> **Memento amical:**
> Termenii privind drepturile de autor și licențiere sunt concepuți pentru a proteja drepturile și interesele legitime ale proiectului, autorilor, contribuitorilor și jucătorilor și pentru a evita neînțelegerile care decurg din „acorduri tacite" sau „prezumții implicite". Vă rugăm să le citiți cu atenție.
> Drepturile de autor și licențierea sunt guvernate de conținutul fișierului README.md; această secțiune oferă doar o descriere mai accesibilă.

### 1. Principiul de bază: Păstrați drepturile de autor, acordând în același timp proiectului licența de utilizare a operei dumneavoastră

* Dețineți în continuare drepturile de autor asupra conținutului pe care îl creați (traduceri, imagini, scripturi/programe etc.);
* Cu toate acestea, odată ce acest conținut este trimis la acest proiect și acceptat (fuzionat),
  sunteți de acord să licențiați altora utilizarea acestui conținut în baza licenței open-source/partajate adoptate de acest proiect.

Aceasta înseamnă:

* **Puteți în continuare** să utilizați și să afișați opera dumneavoastră în altă parte;
* Dar **nu puteți**, după fuzionarea contribuției dumneavoastră, să solicitați acestui proiect sau altor utilizatori care au obținut legal opera să „revoce licența" sau să „șteargă versiunile istorice".

### 2. Licențierea textelor, imaginilor și conținutului similar (CC BY-NC-SA 4.0)

Pentru următorul conținut pe care îl trimiteți:

* Traduceri de texte de joc, revizuiri și corecturi;
* Documentația proiectului și texte explicative;
* Imagini și resurse artistice create special pentru acest proiect;

Odată acceptat și fuzionat în acest depozit, se consideră că sunteți de acord că:

1. Acest conținut este licențiat sub **Atribuire-Necomercial-Distribuire în Condiții Identice 4.0 Internațional**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, prescurtat **CC BY-NC-SA 4.0**);
2. Project Babel și toți utilizatorii care primesc acest conținut pot, **cu respectarea termenilor CC BY-NC-SA 4.0**:

   * Să partajeze, să copieze și să redistribuie acest conținut;
   * Să îl modifice și să creeze opere derivate în scopuri necomerciale;
3. Sunteți de acord că această licență este **neexclusivă, la nivel mondial, gratuită și irevocabilă** în măsura permisă de legea aplicabilă;
4. Chiar dacă ulterior vă retrageți sau încetați să participați la acest proiect, proiectul poate continua să utilizeze și să redistribuie conținutul relevant pe care l-ați trimis și care a fost fuzionat, în baza CC BY-NC-SA 4.0.

> Dacă nu acceptați termenii de licență de mai sus, vă rugăm să nu trimiteți contribuții de text sau imagini la acest proiect,
> sau comunicați în prealabil cu responsabilii proiectului pentru a confirma dacă este posibilă colaborarea în alt mod.

### 3. Licențierea scripturilor și a codului instrumentelor (GPL-3.0)

Pentru următoarele pe care le trimiteți și sunt acceptate:

* Scripturi de automatizare;
* Instrumente de build/export;
* Alt cod de program utilizat pentru procesarea acestui proiect de traducere;

În absența unor declarații speciale, se consideră că sunteți de acord că:

1. Codul este licențiat sub **GPL-3.0** (GNU General Public License versiunea 3);
2. Responsabilii proiectului îl pot modifica, fuziona și distribui în limitele permise de GPL-3.0;
3. Puteți continua și alte proiecte bazate pe același cod, atâta timp cât respectați termenii GPL-3.0.

Pentru a evita conflictele de licență, încercați să:

* Nu introduceți cod terț **incompatibil cu GPL-3.0** fără confirmare prealabilă;
* Dacă trebuie să faceți referire la biblioteci terțe, indicați clar sursa și licența acestora în PR și confirmați compatibilitatea.

### 4. Operele originale și drepturile de autor ale jocului original

Acest proiect este un proiect de **traducere neoficială** pentru modurile legate de *Project Zomboid*:

* Drepturile de autor ale jocului original și ale fiecărui mod aparțin autorilor/editorilor respectivi;
* Acest proiect implică doar crearea și organizarea traducerilor de text, ajustărilor stilistice și a unor resurse auxiliare;
* Contribuitorii, la trimiterea conținutului, trebuie să se asigure:

  * Să nu copieze direct texte de traducere sau resurse artistice terțe neautorizate;
  * Să respecte drepturile autorilor originali și ale autorilor de moduri și să nu efectueze redistribuiri care încalcă drepturile.

---

## Comunicare și colaborare

Dacă aveți:

* Întrebări despre termenii de licență;
* Incertitudini cu privire la posibilitatea de a contribui cu un anumit conținut;
* Dorința de a licenția opera dumneavoastră într-un mod special (de exemplu, doar utilizare necomercială fără adaptare permisă);

Nu ezitați să contactați responsabilii proiectului prin:

* Trimiterea unui Issue pentru discuție;
* Alte metode de contact publice ale responsabililor.

Vom face tot posibilul pentru a găsi o soluție care să echilibreze dezvoltarea sănătoasă a proiectului, respectând în același timp drepturile și interesele tuturor părților.

---

## Sprijin financiar

În timpul funcționării proiectului, din cauza adăugării de noi moduri și a actualizărilor de text ale modurilor existente, este necesară apelarea continuă a API-ului LLM pentru traducere. Pentru a limita comportamentul LLM, pe lângă textele de bază ale modurilor, este necesară o cantitate mare de conținut de prompt (inclusiv prompturi de bază, reguli de traducere, tabele terminologice, restricții de intrare/ieșire, rezultate ale căutării semantice etc.), ceea ce consumă mult mai multe tokenuri decât textele originale. Prin urmare, proiectul are nevoie de sprijin financiar.

Dacă doriți să oferiți sprijin financiar, vă rugăm să contactați responsabilii proiectului. Vă mulțumim foarte mult!

---

Încă o dată, vă mulțumim pentru disponibilitatea de a contribui la acest proiect!
Fiecare contribuție pe care o faceți aduce beneficii mai multor jucători!
