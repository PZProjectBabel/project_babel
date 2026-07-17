# Guida al contributo (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Grazie per la tua disponibilità a contribuire al **Project Babel — il progetto di traduzione automatica LLM per le mod di Project Zomboid**! Che si tratti di correggere un bug, aggiungere una funzionalità, scrivere template di prompt o fornire traduzioni di riferimento, ogni contributo è importante!

Chiamare l'API LLM per la traduzione ha un costo in token. Affinché il progetto possa funzionare in modo sostenibile a lungo termine, il tuo generoso supporto è molto apprezzato!

> ⚠️ **Avviso importante:**
> Prima di inviare qualsiasi cosa a questo repository, assicurati di leggere e comprendere la sezione "Diritto d'autore e licenze".
> Una volta inviato e mergiato, si considera che tu abbia accettato i corrispondenti termini di licenza.

---

## Prima di iniziare

Leggi il `README.md` del progetto per comprendere:

- Gli obiettivi generali e lo stato attuale di questo progetto;
- Come i normali giocatori utilizzano questo progetto (per i tuoi test);
- I dettagli tecnici del progetto.

---

## Come posso contribuire?

Puoi scegliere uno o più modi per partecipare in base ai tuoi interessi e competenze:

- Fornire regole di traduzione per una lingua di destinazione
- Fornire un dizionario terminologico per una lingua di destinazione
- Migliorare i prompt di sistema
- Fornire corpora di traduzione corretti manualmente
- Migliorare i moduli della pipeline (.NET) e gli script di automazione
- Segnalare problemi e suggerire miglioramenti (tramite Issue)
- Fornire supporto finanziario per le chiamate API LLM

Di seguito sono spiegati i principali scenari di contributo.

---

## Fornire regole di traduzione, dizionari terminologici e migliorare i prompt di sistema

I template di prompt della pipeline si trovano in `src/prompt_templates/`, con la seguente struttura:

- `system_prompt_translate_engine.txt`: il prompt di sistema globale del motore di traduzione (condiviso da tutte le lingue);
- `<codice_lingua>/translation_dictionary_<codice_lingua>.json`: il dizionario terminologico per quella lingua;
- `<codice_lingua>/translation_schema_<codice_lingua>.md`: le regole di traduzione e i vincoli di stile per quella lingua.

Passaggi per contribuire:

1. Crea una sottodirectory in `src/prompt_templates/` per la tua lingua e aggiungi i file del dizionario e delle regole di traduzione;
2. Se devi regolare il comportamento di traduzione globale, modifica `system_prompt_translate_engine.txt` (nota: influisce su tutte le lingue);
3. Testa localmente per confermare i risultati;
4. Invia una PR.

---

## Fornire corpora corretti manualmente

Se sei l'autore di una mod di traduzione e sei disposto a fornire il tuo corpus di traduzione come riferimento per il LLM, invia una richiesta tramite Issue. Devi fornire le seguenti informazioni:

- Il Mod ID della tua mod di traduzione e la lingua di destinazione;
- Uno screenshot della pagina di amministrazione della tua mod di traduzione per dimostrare che sei l'autore;
- Una dichiarazione chiara nell'Issue che sei disposto a fornire il corpus di traduzione;
- Se ci sono circostanze particolari (licenza speciale, ecc.), spiegale;
- Assicurati che il corpus fornito sia di alta qualità.

Con la tua autorizzazione, il progetto aggiungerà la tua mod all'elenco delle mod di traduzione di riferimento `config/ref_translation_mods.json` e la pipeline sincronizzerà automaticamente i tuoi testi tradotti come corpora di riferimento RAG.

---

## Contributi allo sviluppo della pipeline e degli strumenti

L'automazione in questo progetto è divisa in due parti:

**Moduli della pipeline (`src/`, C# / .NET 10)**: Contiene 15 moduli eseguiti in sequenza, responsabili del flusso completo dal download delle mod, estrazione del testo, revisione dei contenuti, calcolo degli embedding, recupero RAG fino alla traduzione LLM e all'output finale. Vedi la [referenza tecnica](../technical_reference/technical_reference_it.md) per i dettagli.

**Script ausiliari (`.github/`)**: Utilizzati per l'automazione GitHub.

Se desideri:

* Correggere bug nei moduli della pipeline o negli script esistenti;
* Aggiungere nuove funzionalità o nuovi moduli alla pipeline;
* Ottimizzare le prestazioni o la struttura del codice;
* Migliorare i template di prompt o le strategie RAG;

Puoi seguire questi passaggi:

1. Fai il fork di questo repository e clonalo localmente;
2. Crea un nuovo branch dal branch più recente;
3. Modifica o aggiungi file nelle directory corrispondenti:
   - Modifiche ai moduli della pipeline → `src/<nome_modulo>/`;
   - Modifiche agli script → `scripts/`;
   - Modifiche ai template di prompt → `src/prompt_templates/`;
4. Prima di inviare, cerca di:

   * Mantenere lo stile di codice esistente;
   * Aggiungere i commenti necessari;
   * Se possibile, includere semplici test o istruzioni per l'uso;
5. Invia le modifiche tramite PR, spiegando nella descrizione:

   * Lo scopo delle modifiche;
   * Le directory / i moduli / gli script che potrebbero essere interessati;
   * Se comporta modifiche che rompono la compatibilità.

---

## Diritto d'autore e licenze

> **Promemoria amichevole:**
> I termini di diritto d'autore e licenza sono progettati per proteggere i diritti e gli interessi legittimi del progetto, degli autori, dei contributori e dei giocatori, ed evitare malintesi derivanti da "accordi taciti" o "presupposti predefiniti". Leggili attentamente.
> Il diritto d'autore e le licenze sono regolati dal contenuto del file README.md; questa sezione fornisce solo una descrizione più accessibile.

### 1. Principio di base: Conservi il diritto d'autore, concedendo in licenza al progetto l'uso della tua opera

* Conservi il diritto d'autore sul contenuto che crei (traduzioni, immagini, script/programmi, ecc.);
* Tuttavia, una volta che questo contenuto viene inviato a questo progetto e accettato (mergiato),
  accetti di concedere in licenza ad altri l'uso di questo contenuto secondo la licenza open-source/condivisa adottata da questo progetto.

Ciò significa:

* **Puoi ancora** continuare a utilizzare e mostrare il tuo lavoro altrove;
* Ma **non puoi**, dopo che il tuo contributo è stato mergiato, chiedere a questo progetto o ad altri utenti che hanno legalmente ottenuto l'opera di "revocare la licenza" o "eliminare le versioni storiche".

### 2. Licenza di testi, immagini e contenuti simili (CC BY-NC-SA 4.0)

Per i seguenti contenuti che invii:

* Traduzioni di testi di gioco, revisioni e correzioni;
* Documentazione del progetto e testi esplicativi;
* Immagini e risorse artistiche create specificamente per questo progetto;

Una volta accettato e mergiato in questo repository, si considera che tu accetti che:

1. Questi contenuti sono concessi in licenza sotto **Attribuzione - Non commerciale - Condividi allo stesso modo 4.0 Internazionale**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abbreviato **CC BY-NC-SA 4.0**);
2. Project Babel e tutti gli utenti che ricevono questo contenuto possono, **nel rispetto dei termini CC BY-NC-SA 4.0**:

   * Condividere, copiare e ridistribuire questo contenuto;
   * Modificarlo e creare opere derivate per scopi non commerciali;
3. Accetti che questa licenza sia **non esclusiva, mondiale, libera da royalty e irrevocabile** nella misura consentita dalla legge applicabile;
4. Anche se in seguito ti ritiri o cessi di partecipare a questo progetto, il progetto può continuare a utilizzare e ridistribuire il contenuto pertinente che hai inviato e che è stato mergiato, in base alla CC BY-NC-SA 4.0.

> Se non accetti i termini di licenza di cui sopra, non inviare contributi di testo o immagini a questo progetto,
> oppure comunica in anticipo con i manutentori del progetto per confermare se è possibile collaborare in altro modo.

### 3. Licenza di script e codice degli strumenti (GPL-3.0)

Per quanto segue che invii e che viene accettato:

* Script di automazione;
* Strumenti di build/esportazione;
* Altro codice di programma utilizzato per elaborare questo progetto di traduzione;

In assenza di dichiarazioni speciali, si considera che tu accetti che:

1. Il codice è concesso in licenza sotto **GPL-3.0** (GNU General Public License versione 3);
2. I manutentori del progetto possono modificarlo, mergiarlo e distribuirlo nell'ambito consentito dalla GPL-3.0;
3. Puoi anche continuare altri progetti basati sullo stesso codice, purché rispetti i termini della GPL-3.0.

Per evitare conflitti di licenza, cerca di:

* Non introdurre codice di terze parti **incompatibile con la GPL-3.0** senza previa conferma;
* Se devi fare riferimento a librerie di terze parti, indica chiaramente la loro origine e licenza nella PR e conferma la compatibilità.

### 4. Opere a monte e diritto d'autore del gioco originale

Questo progetto è un progetto di **traduzione non ufficiale** per le mod relative a *Project Zomboid*:

* Il diritto d'autore del gioco originale e di ciascuna mod appartiene ai rispettivi autori/editori;
* Questo progetto riguarda solo la creazione e l'organizzazione di traduzioni testuali, aggiustamenti stilistici e alcune risorse di supporto;
* I contributori, nell'inviare contenuti, devono assicurarsi di:

  * Non copiare direttamente testi di traduzione o risorse artistiche di terzi non autorizzati;
  * Rispettare i diritti degli autori originali e degli autori delle mod, e non effettuare ridistribuzioni illecite.

---

## Comunicazione e collaborazione

Se hai:

* Domande sui termini di licenza;
* Dubbi sul fatto che determinati contenuti possano essere contribuiti;
* Il desiderio di concedere in licenza il tuo lavoro in modo speciale (ad esempio, solo uso non commerciale ma senza adattamento consentito);

Non esitare a contattare i manutentori del progetto tramite:

* Invio di un Issue per discussione;
* Altri mezzi di contatto pubblici dei manutentori.

Faremo del nostro meglio per trovare una soluzione che bilanci il sano sviluppo del progetto nel rispetto dei diritti e degli interessi di tutte le parti.

---

## Supporto finanziario

Durante il funzionamento del progetto, a causa dell'aggiunta di nuove mod e degli aggiornamenti testuali delle mod esistenti, è necessario chiamare continuamente l'API LLM per la traduzione. Per vincolare il comportamento del LLM, oltre ai testi di base delle mod, è necessaria una grande quantità di contenuto di prompt (inclusi prompt di base, regole di traduzione, tabelle terminologiche, vincoli di input/output, risultati di ricerca semantica, ecc.), che consuma molti più token rispetto ai testi originali. Pertanto, il progetto ha bisogno di supporto finanziario.

Se desideri fornire supporto finanziario, contatta i manutentori del progetto. Grazie mille!

---

Ancora grazie per la tua disponibilità a contribuire a questo progetto!
Ogni tuo contributo porta beneficio a più giocatori!
