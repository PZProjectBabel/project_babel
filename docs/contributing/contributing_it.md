# Guida al Contributo (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Indice

- [1. Prima di iniziare](#1-prima-di-iniziare)
- [2. Come posso contribuire?](#2-come-posso-contribuire)
- [3. Fornire regole di traduzione, dizionario terminologico e migliorare i prompt di sistema](#3-fornire-regole-di-traduzione-dizionario-terminologico-e-migliorare-i-prompt-di-sistema)
- [4. Fornire corpus di revisione manuale](#4-fornire-corpus-di-revisione-manuale)
- [5. Contributi allo sviluppo della pipeline e degli strumenti](#5-contributi-allo-sviluppo-della-pipeline-e-degli-strumenti)
- [6. Copyright e accordo di licenza](#6-copyright-e-accordo-di-licenza)
  - [6.1 Principi di base: Tu mantieni il copyright, mentre autorizzi il progetto a utilizzarlo](#61-principi-di-base-tu-mantieni-il-copyright-mentre-autorizzi-il-progetto-a-utilizzarlo)
  - [6.2 Licenza per testi, immagini e altri contenuti (CC BY-NC-SA 4.0)](#62-licenza-per-testi-immagini-e-altri-contenuti-cc-by-nc-sa-40)
  - [6.3 Licenza per script e codice degli strumenti (GPL-3.0)](#63-licenza-per-script-e-codice-degli-strumenti-gpl-30)
  - [6.4 Diritti d'autore upstream e diritti del gioco originale](#64-diritti-dautore-upstream-e-diritti-del-gioco-originale)
- [7. Comunicazione e collaborazione](#7-comunicazione-e-collaborazione)
- [8. Supporto finanziario](#8-supporto-finanziario)

---

Ti ringraziamo moltissimo per essere disposto a contribuire al **Project Babel - 《僵尸毁灭工程》模组LLM自动翻译项目**! Che si tratti di correggere un errore, aggiungere una nuova funzionalità, scrivere modelli di prompt o fornire traduzioni di riferimento!

Chiamare l'API LLM per la traduzione richiede il pagamento dei token. Per garantire la stabilità a lungo termine del progetto, speriamo che tu possa contribuire generosamente!

> ⚠️ **Avviso importante:**
> Prima di inviare qualsiasi contenuto a questo repository, assicurati di leggere e comprendere la sezione "Accordo sul copyright e sulla licenza".
> Una volta presentato e accettato, si intende che accetti i termini di licenza corrispondenti.

---

## 1. Prima di iniziare

Leggi prima il `README.md` del progetto per capire:
- L'obiettivo generale e lo stato attuale del progetto;
- Come i giocatori normali possono usare questo progetto (per autotest);
- I dettagli tecnici del progetto.

---

## 2. Come posso contribuire?

Puoi partecipare in uno o più modi, in base ai tuoi interessi e competenze:

- Fornire regole di traduzione per la lingua target
- Fornire un dizionario terminologico di traduzione per la lingua target
- Migliorare i prompt di sistema
- Fornire corpus di traduzioni revisionate manualmente
- Migliorare i moduli della pipeline (.NET) e gli script di automazione
- Segnalare problemi o suggerire miglioramenti (nelle Issues)
- Fornire supporto finanziario per le chiamate LLM

Di seguito alcune spiegazioni per i principali scenari di contributo.

---

## 3. Fornire regole di traduzione, dizionario terminologico e migliorare i prompt di sistema

I modelli di prompt della pipeline si trovano in `src/prompt_templates/`, con la seguente struttura:

- `system_prompt_translate_engine.txt`: prompt di sistema del motore di traduzione globale (comune a tutte le lingue);
- `<codice_lingua>/translation_dictionary_<codice_lingua>.json`: dizionario terminologico per quella lingua;
- `<codice_lingua>/translation_schema_<codice_lingua>.md`: regole di traduzione e vincoli di stile per quella lingua.

Passaggi per contribuire:

1. Crea una sottodirectory per la tua lingua in `src/prompt_templates/`, aggiungi il dizionario terminologico e il file delle regole di traduzione;
2. Se necessario, modifica `system_prompt_translate_engine.txt` per regolare il comportamento di traduzione globale (nota: influisce su tutte le lingue);
3. Testa localmente per confermare l'effetto;
4. Invia una PR.

---

## 4. Fornire corpus di revisione manuale

Se sei un creatore di mod di traduzione e desideri fornire il tuo corpus di traduzione come riferimento per la traduzione LLM, apri una richiesta in Issue. Devi fornire i seguenti materiali:

- Il Mod ID del tuo mod di traduzione e la lingua di destinazione della traduzione;
- Uno screenshot della pagina di backend del tuo mod di traduzione, per dimostrare che sei l'autore del mod;
- Indica chiaramente nell'Issue che sei disposto a fornire il corpus di traduzione;
- Se ci sono circostanze speciali (licenze speciali, ecc.), specificale;
- Assicurati che il corpus fornito sia di alta qualità.

Con la tua autorizzazione, il progetto elencherà il tuo mod nell'elenco dei mod di traduzione di riferimento `config/ref_translation_mods.json`, e la pipeline sincronizzerà automaticamente il tuo testo di traduzione come corpus di riferimento RAG.

---

## 5. Contributi allo sviluppo della pipeline e degli strumenti

L'automazione di questo progetto è divisa in due parti:

**Modulo pipeline (`src/`, C# / .NET 10)**: contiene 15 moduli eseguiti in sequenza, responsabili dell'intero flusso dall'inizializzazione di SteamCMD, download del mod, estrazione del testo, revisione dei contenuti, calcolo degli embedding, recupero RAG fino alla traduzione LLM e output finale. Vedi [Riferimento tecnico](../technical_reference/technical_reference_it.md).

**Script ausiliari (`.github/`)**: utilizzati per l'automazione di GitHub.

Se desideri:

* Correggere bug nei moduli o script della pipeline esistenti;
* Aggiungere nuove funzionalità o nuovi moduli alla pipeline;
* Ottimizzare le prestazioni o la struttura del codice;
* Migliorare i template di prompt o la strategia RAG;

Puoi seguire i seguenti passaggi:

1. Forka questo repository e clonalo in locale;
2. Crea un nuovo ramo basato sul ramo più recente;
3. Modifica o aggiungi file nelle directory corrispondenti:
- Modifiche ai moduli pipeline → `src/<nome modulo>/`;
- Modifiche agli script → `scripts/`;
- Modifiche ai template di prompt → `src/prompt_templates/`;
4. Prima di inviare, cerca di:

* Mantenere lo stile di codice originale;
* Aggiungere commenti necessari;
* Se possibile, allegare semplici istruzioni di test o d'uso;
5. Invia le modifiche tramite PR e spiega nella descrizione:

* Scopo delle modifiche;
* Directory/moduli/script potenzialmente interessati;
* Se comporta modifiche distruttive.

---

## 6. Copyright e accordo di licenza

> **Avviso importante:**
> Le clausole di copyright e licenza sono intese a proteggere i diritti legittimi del progetto, degli autori, dei contributori e dei giocatori, evitando malintesi dovuti a "intese tacite" o "implicite". Si prega di leggerle attentamente.
> Il copyright e la licenza fanno riferimento al contenuto del file README.md; questa sezione fornisce solo una descrizione più accessibile.

### 6.1 Principi di base: Tu mantieni il copyright, mentre autorizzi il progetto a utilizzarlo

* Conservi i diritti d'autore sui contenuti che hai creato (traduzioni, immagini, script/programmi, ecc.);
* Ma dopo aver inviato questi contenuti a questo progetto e vengono accettati (uniti), acconsenti a concedere in licenza ad altri l'uso di tali contenuti secondo la licenza open source/condivisa adottata da questo progetto.

Ciò significa:

* **Puoi ancora** continuare a utilizzare ed esporre le tue opere altrove;
* Ma **non puoi** richiedere a questo progetto o ad altri utenti che hanno già ottenuto legalmente le opere di "ritirare la licenza" o "eliminare le versioni storiche" dopo che il contributo è stato unito.

### 6.2 Licenza per testi, immagini e altri contenuti (CC BY-NC-SA 4.0)

Per i seguenti contenuti che invii:

* Traduzioni, revisioni e correzioni di testi di gioco;
* Documentazione del progetto, testi esplicativi;
* Immagini e risorse artistiche create appositamente per questo progetto;

Una volta accettati e uniti a questo repository, si intende che acconsenti a:

1. Questi contenuti sono concessi in licenza secondo i termini della **Creative Commons Attribuzione - Non commerciale - Condividi allo stesso modo 4.0 Internazionale** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abbreviato **CC BY-NC-SA 4.0**);
2. Project Babel e tutti gli utenti che ottengono tali contenuti possono, a condizione di **rispettare i termini di CC BY-NC-SA 4.0**:
* Condividere, copiare e ridistribuire questi contenuti;
* Modificarli e crearne opere derivate per scopi non commerciali;
3. Acconsenti che, nei limiti consentiti dalla legge applicabile, questa licenza è **non esclusiva, mondiale, esente da royalty e irrevocabile**;
4. Anche se in futuro dovessi ritirarti o smettere di partecipare a questo progetto, il progetto potrà continuare a utilizzare e ripubblicare i contenuti pertinenti che hai già inviato e sono stati uniti secondo CC BY-NC-SA 4.0.

> Se non accetti la suddetta modalità di licenza, non inviare contributi testuali o grafici a questo progetto,
> oppure contatta preventivamente i manutentori del progetto per verificare se è possibile collaborare in altro modo.

### 6.3 Licenza per script e codice degli strumenti (GPL-3.0)

Per quanto riguarda ciò che invii e viene accettato:

* Script di automazione;
* Strumenti di costruzione/esportazione;
* Altri codici di programma per la gestione di questo progetto di traduzione;

In assenza di dichiarazioni particolari, si intende che accetti:

1. Il codice è concesso in licenza **GPL-3.0** (GNU General Public License versione 3);
2. I manutentori del progetto possono modificare, unire e distribuire il codice nell'ambito consentito da GPL-3.0;
3. Puoi anche avviare altri progetti basati sullo stesso codice, purché rispetti i termini di GPL-3.0.

Per evitare conflitti di licenza, si prega di:

* Non introdurre codice di terze parti **incompatibile con GPL-3.0** senza averlo verificato;
* Se è necessario utilizzare librerie di terze parti, specificare chiaramente nel PR la loro fonte e licenza, e verificare la compatibilità.

### 6.4 Diritti d'autore upstream e diritti del gioco originale

Questo progetto è un progetto di **traduzione non ufficiale** per le mod relative a *Project Zomboid*:

* I diritti d'autore del gioco originale e di ciascuna mod appartengono ai rispettivi autori/editori;
* Questo progetto si occupa solo della traduzione dei testi, delle modifiche stilistiche e della creazione/organizzazione di alcune risorse correlate;
* I contributori, nell'inviare contenuti, devono assicurarsi di:
* Non copiare direttamente testi o risorse grafiche di traduzioni non autorizzate da terzi;
* Rispettare i diritti degli autori originali e dei creatori delle mod, evitando la distribuzione di materiale protetto da copyright.

---

## 7. Comunicazione e collaborazione

Se hai:

* Dubbi sulle clausole di licenza;
* Incertezze sul fatto che un certo contenuto possa essere contribuito;
* Desideri concedere in licenza il tuo lavoro in modo speciale (ad esempio, solo per uso non commerciale senza consentire modifiche, ecc.);

Contatta i manutentori del progetto tramite i seguenti metodi:

* Aprire una Issue per discuterne;
* Utilizzare altri metodi di contatto pubblicamente disponibili forniti dai manutentori.

Cercheremo, nel rispetto dei diritti di tutte le parti, di trovare una soluzione che tenga conto del sano sviluppo del progetto.

---

## 8. Supporto finanziario

Durante l'esecuzione del progetto, a causa dell'aggiunta di nuove mod o dell'aggiornamento dei testi delle mod esistenti, è necessario chiamare continuamente l'API LLM per la traduzione. Per vincolare il comportamento dell'LLM, oltre ai testi di base delle mod, è necessario fornire una grande quantità di contenuti di prompt (inclusi prompt di base, regole di traduzione, glossari, vincoli di input/output, risultati di query semantiche, ecc.), che consumano token molto superiori rispetto al testo originale. Pertanto, il progetto necessita di supporto finanziario.

Se desideri fornire supporto finanziario, contatta i manutentori del progetto. Grazie mille!

---

Grazie ancora per la tua volontà di contribuire a questo progetto!
Ogni tuo contributo fa sì che più giocatori ne traggano beneficio!
