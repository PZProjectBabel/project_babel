# Ghid de contribuție (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Cuprins

- [1. Înainte de a începe](#1-înainte-de-a-începe)
- [2. Cum pot contribui?](#2-cum-pot-contribui)
- [3. Oferirea de reguli de traducere, dicționare terminologice, îmbunătățirea prompt-urilor sistemului](#3-oferirea-de-reguli-de-traducere-dicționare-terminologice-îmbunătățirea-prompt-urilor-sistemului)
- [4. Furnizarea corpusului de verificare manuală](#4-furnizarea-corpusului-de-verificare-manuală)
- [5. Contribuții la pipeline și dezvoltarea uneltelor](#5-contribuții-la-pipeline-și-dezvoltarea-uneltelor)
- [6. Drepturi de autor și acord de licențiere](#6-drepturi-de-autor-și-acord-de-licențiere)
  - [6.1 Principii de bază: Păstrezi drepturile de autor și acorzi proiectului permisiunea de utilizare](#61-principii-de-bază-păstrezi-drepturile-de-autor-și-acorzi-proiectului-permisiunea-de-utilizare)
  - [6.2 Licențierea textelor și imaginilor (CC BY-NC-SA 4.0)](#62-licențierea-textelor-și-imaginilor-cc-by-nc-sa-40)
  - [6.3 Licențierea scripturilor și codului instrumentelor (GPL-3.0)](#63-licențierea-scripturilor-și-codului-instrumentelor-gpl-30)
  - [6.4 Drepturile de autor ale lucrărilor upstream și ale jocului original](#64-drepturile-de-autor-ale-lucrărilor-upstream-și-ale-jocului-original)
- [7. Comunicare și colaborare](#7-comunicare-și-colaborare)
- [8. Suport financiar](#8-suport-financiar)

---

Îți mulțumim foarte mult că ești dispus(ă) să contribui la **Project Babel - Proiectul de traducere automată LLM pentru mod-ul „Project Zomboid”**! Fie că este vorba de corectarea unei erori, adăugarea unei funcționalități noi, scrierea unui șablon de prompt, sau oferirea unei traduceri de referință!

Apelarea API-ului LLM pentru traducere necesită plata pentru tokeni. Pentru ca proiectul să poată funcționa stabil pe termen lung, sperăm să puteți contribui generos!

> ⚠️ **Avertisment important:**
> Înainte de a trimite orice conținut în acest depozit, asigurați-vă că ați citit și înțeles secțiunea „Acord privind drepturile de autor și licențierea”.
> Odată ce conținutul este trimis și integrat, se consideră că sunteți de acord cu clauzele de licențiere corespunzătoare.

---

## 1. Înainte de a începe

Citiți mai întâi `README.md` al proiectului pentru a afla:
- Obiectivul general și starea actuală a proiectului;
- Cum pot folosi jucătorii obișnuiți acest proiect (pentru a-l testa singur);
- Detalii tehnice ale proiectului.

---

## 2. Cum pot contribui?

Poți participa în una sau mai multe moduri, în funcție de interesele și abilitățile tale:

- Oferirea de reguli de traducere pentru limba țintă
- Oferirea de dicționare terminologice pentru traducerea în limba țintă
- Îmbunătățirea prompt-urilor sistemului
- Oferirea de corpus de traducere revizuit manual
- Îmbunătățirea modulelor pipeline-ului (.NET) și a scripturilor de automatizare
- Raportarea problemelor, sugerarea de îmbunătățiri (în secțiunea Issues)
- Oferirea de sprijin financiar pentru apelurile LLM

Mai jos sunt câteva explicații pentru principalele scenarii de contribuție.

---

## 3. Oferirea de reguli de traducere, dicționare terminologice, îmbunătățirea prompt-urilor sistemului

Șabloanele de prompt ale pipeline-ului se află în `src/prompt_templates/`, structura fiind următoarea:

- `system_prompt_translate_engine.txt`: Prompt-ul de sistem al motorului global de traducere (comun pentru toate limbile);
- `<cod limbă>/translation_dictionary_<cod limbă>.json`: Dicționarul terminologic pentru limba respectivă;
- `<cod limbă>/translation_schema_<cod limbă>.md`: Regulile de traducere și constrângerile de stil pentru limba respectivă.

Pași pentru contribuție:

1. Creează un subdirector pentru limba ta în `src/prompt_templates/`, adaugă dicționarul terminologic și fișierul cu reguli de traducere;
2. Dacă dorești să ajustezi comportamentul global de traducere, modifică `system_prompt_translate_engine.txt` (reține că afectează toate limbile);
3. Testați local pentru a confirma efectul;
4. Trimiteți PR.

---

## 4. Furnizarea corpusului de verificare manuală

Dacă sunteți autorul unui mod de traducere și doriți să furnizați corpusul dvs. de traducere ca referință pentru traducerea LLM, vă rugăm să deschideți o cerere în Issue. Trebuie să furnizați următoarele informații:

- Mod ID-ul modului dvs. de traducere și limba țintă a traducerii;
- Captura de ecran a paginii de administrare a modului dvs. de traducere, pentru a demonstra că sunteți autorul modului;
- Menționați clar în Issue că sunteți dispus să furnizați corpusul de traducere;
- Dacă există circumstanțe speciale (licențiere specială etc.), vă rugăm să le specificați;
- Vă rugăm să vă asigurați că corpusul furnizat are o calitate ridicată.

Sub autorizația dvs., proiectul va adăuga modul dvs. în lista `config/ref_translation_mods.json` a modulelor de traducere de referință, iar pipeline-ul va sincroniza automat textul dvs. tradus ca corpus de referință RAG.

---

## 5. Contribuții la pipeline și dezvoltarea uneltelor

Automatizarea acestui proiect este împărțită în două părți:

**Modulul pipeline (`src/`, C# / .NET 10)**: Conține 15 module executate secvențial, responsabile pentru întregul flux de la inițializarea SteamCMD, descărcarea modulelor, extragerea textului, revizuirea conținutului, calcularea Embedding-urilor, căutarea RAG până la traducerea LLM și ieșirea finală. Vezi [Referință tehnică](../technical_reference/technical_reference_ro.md).

**Scripturi auxiliare (`.github/`)**: Utilizate pentru automatizarea GitHub.

Dacă doriți:

* Remediați bug-urile în modulele sau scripturile pipeline existente;
* Adăugați funcționalități noi sau module noi la pipeline;
* Optimizați performanța sau structura codului;
* Îmbunătățiți șabloanele prompt sau strategia RAG;

Puteți urma următorii pași:

1. Forkați acest depozit și clonați-l local;
2. Creați o ramură nouă pe baza celei mai recente ramuri;
3. Modificați sau adăugați fișiere în directorul corespunzător:
- Modificarea modulelor pipeline → `src/<nume_modul>/`;
- Modificarea scripturilor → `scripts/`;
- Modificarea șabloanelor Prompt → `src/prompt_templates/`;
4. Înainte de a trimite, încercați pe cât posibil:

* Păstrați stilul original al codului;
* Adăugați comentarii necesare;
* Dacă este posibil, atașați teste simple sau instrucțiuni de utilizare;
5. Trimiteți modificările prin PR și explicați în descriere:

* Scopul modificării;
* Directoarele/modulele/scripturile posibil afectate;
* Dacă implică modificări de tip breaking change.

---

## 6. Drepturi de autor și acord de licențiere

> **Notă importantă:**
> Acordul privind drepturile de autor și licențierea are scopul de a proteja drepturile legale ale proiectului, autorilor, contribuitorilor și jucătorilor, evitând neînțelegerile cauzate de „convenție tacită” sau „implicit”. Vă rugăm să citiți cu atenție.
> Drepturile de autor și licențierea se bazează pe conținutul din fișierul README.md, această secțiune oferind doar o descriere mai accesibilă.

### 6.1 Principii de bază: Păstrezi drepturile de autor și acorzi proiectului permisiunea de utilizare

* Păstrezi în continuare drepturile de autor asupra conținutului creat de tine (traduceri, imagini, scripturi/programe etc.);
* Dar, după ce trimiți aceste conținuturi în acest depozit și sunt acceptate (integrate), ești de acord să acorzi altora permisiunea de a le utiliza conform licenței open source/partajate a proiectului.

Aceasta înseamnă:

* Poți **în continuare** să folosești și să expui propriile lucrări în alte locuri;
* Dar **nu poți** să ceri proiectului sau altor utilizatori care au obținut legal lucrările să „retragă autorizația” sau să „șteargă versiunile istorice” după ce contribuția ta a fost integrată.

### 6.2 Licențierea textelor și imaginilor (CC BY-NC-SA 4.0)

Pentru următoarele conținuturi pe care le trimiți:

* Traduceri ale textelor din joc, corecturi și îmbunătățiri;
* Documentația proiectului, texte explicative;
* Imagini, resurse artistice create special pentru acest proiect;

Odată ce sunt acceptate și integrate în acest depozit, se consideră că ești de acord:

1. Aceste conținuturi sunt licențiate sub **Atribuire-Necomercial-Distribuire în condiții identice 4.0 Internațional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, prescurtat **CC BY-NC-SA 4.0**);
2. Project Babel și toți utilizatorii care obțin aceste conținuturi pot, cu condiția **respectării termenilor CC BY-NC-SA 4.0**:
* Să partajeze, copieze și redistribuie aceste conținuturi;
* Să le modifice și să creeze lucrări derivate în scopuri necomerciale;
3. Ești de acord ca, în limita permisă de legea aplicabilă, această licență să fie **neexclusivă, mondială, fără redevențe și irevocabilă**;
4. Chiar dacă te retragi sau încetezi participarea la acest proiect în viitor, proiectul poate continua să utilizeze și să redistribuie conținuturile pe care le-ai trimis și care au fost integrate, în conformitate cu CC BY-NC-SA 4.0.

> Dacă nu acceptați modul de licențiere de mai sus, vă rugăm să nu trimiteți contribuții de tip text sau imagini către acest proiect,
> sau discutați în prealabil cu întreținătorul proiectului pentru a confirma dacă se poate colabora în alt mod.

### 6.3 Licențierea scripturilor și codului instrumentelor (GPL-3.0)

Pentru cele trimise și acceptate:

* Scripturi de automatizare;
* Instrumente de construcție/export;
* Alte coduri de program pentru acest proiect de localizare;

În lipsa unei declarații speciale, se consideră că ești de acord:

1. Codul este licențiat sub **GPL-3.0** (Licența Publică Generală GNU versiunea 3);
2. Întreținătorii proiectului pot modifica, îmbina și distribui codul în limitele permise de GPL-3.0;
3. Poți, de asemenea, continua alte proiecte pe baza aceluiași cod, cu condiția respectării termenilor GPL-3.0.

Pentru a evita conflictele de licențiere, te rugăm să:

* Nu introduceți cod terț **incompatibil cu GPL-3.0** fără a verifica în prealabil;
* Dacă este necesară utilizarea unei biblioteci terțe, specificați clar în PR sursa și licența acesteia și confirmați compatibilitatea.

### 6.4 Drepturile de autor ale lucrărilor upstream și ale jocului original

Acest proiect este un proiect de **traducere neoficială** a unor moduri pentru *Project Zomboid*:

* Drepturile de autor ale jocului original și ale fiecărui mod aparțin autorilor/deținătorilor respectivi;
* Acest proiect creează și organizează doar traduceri, ajustări stilistice și o parte din resursele auxiliare;
* Contribuitorii trebuie să se asigure, la trimiterea conținutului, că:
* Nu copiază direct texte sau resurse grafice de localizare terță neautorizată;
* Respectă drepturile autorilor originali și ale autorilor de moduri, nefăcând distribuiri încălcătoare.

---

## 7. Comunicare și colaborare

Dacă ai întrebări referitoare la:

* Clauzele de licențiere;
* Incertitudini privind posibilitatea de a contribui cu un anumit conținut;
* Dorința de a-ți licenția lucrarea într-un mod special (de exemplu, doar pentru uz necomercial, fără a permite adaptări etc.);

Te rugăm să contactezi întreținătorii proiectului prin:

* Deschiderea unui Issue pentru discuții;
* Alte metode de contact oferite public de către întreținători.

Vom încerca, pe cât posibil, să găsim o soluție care să respecte drepturile tuturor părților, asigurând în același timp dezvoltarea sănătoasă a proiectului.

---

## 8. Suport financiar

Pe parcursul derulării proiectului, datorită adăugării de noi moduri și actualizării conținutului textelor modurilor vechi, este necesară apelarea continuă la API-ul LLM pentru traducere. Pentru a constrânge comportamentul LLM, pe lângă textele de bază ale modurilor, trebuie furnizat și un volum mare de conținut prompt (inclusiv prompturi de bază, reguli de traducere, glosare, constrângeri de intrare/ieșire, rezultate ale interogărilor semantice etc.), care consumă mult mai multe tokenuri decât textul original. Prin urmare, proiectul are nevoie de sprijin financiar.

Dacă dorești să oferi sprijin financiar, te rugăm să contactezi întreținătorii proiectului. Mulțumim mult!

---

Încă o dată, îți mulțumim că ești dispus să contribui la acest proiect!
Fiecare contribuție a ta va face ca mai mulți jucători să beneficieze!
