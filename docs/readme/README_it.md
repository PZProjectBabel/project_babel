# Project Babel — Progetto di traduzione automatica LLM della mod di Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Questo progetto di traduzione è gestito e mantenuto dal set di strumenti [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Indice

- [Lingue di traduzione target supportate dal progetto](#lingue-di-traduzione-target-supportate-dal-progetto)
- [Come installare e utilizzare](#come-installare-e-utilizzare)
- [Stato della traduzione](#stato-della-traduzione)
- [Come contribuire](#come-contribuire)
- [Strumenti e struttura delle directory (per sviluppatori)](#strumenti-e-struttura-delle-directory-per-sviluppatori)
  - [Directory del progetto](#directory-del-progetto)
  - [Moduli della pipeline (in ordine di esecuzione)](#moduli-della-pipeline-in-ordine-di-esecuzione)
  - [Moduli indipendenti](#moduli-indipendenti)
  - [Stack tecnologico](#stack-tecnologico)
- [Copyright e Licenza](#copyright-e-licenza)
  - [1. Testo e immagini, ecc.](#1-testo-e-immagini-ecc)
  - [2. Programmi, script e altri contenuti di sviluppo](#2-programmi-script-e-altri-contenuti-di-sviluppo)
- [Ringraziamenti](#ringraziamenti)
- [Programmi di terze parti](#programmi-di-terze-parti)

---

## Lingue di traduzione target supportate dal progetto

| Lingua | Nome locale | Codice internazionale | Codice in-game | Supportato | Note |
|------|------|------|------|------|------|
| Arabo | العربية | `ar` | `AR` | ❌ | Saldo Token insufficiente |
| Catalano | català | `ca` | `CA` | ❌ | Saldo Token insufficiente |
| Cinese tradizionale | 繁體中文 | `zh-hant` | `CH` | ❌ | Saldo Token insufficiente |
| Cinese semplificato | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Ceco | čeština | `cs` | `CS` | ❌ | Saldo Token insufficiente |
| Danese | dansk | `da` | `DA` | ❌ | Saldo Token insufficiente |
| Tedesco | Deutsch | `de` | `DE` | ✅ | |
| Inglese | English | `en` | `EN` | ✅ | |
| Spagnolo | español | `es` | `ES` | ❌ | Saldo Token insufficiente |
| Finlandese | suomi | `fi` | `FI` | ❌ | Saldo Token insufficiente |
| Francese | français | `fr` | `FR` | ✅ | |
| Ungherese | magyar | `hu` | `HU` | ❌ | Saldo Token insufficiente |
| Indonesiano | Bahasa Indonesia | `id` | `ID` | ❌ | Saldo Token insufficiente |
| Italiano | italiano | `it` | `IT` | ❌ | Saldo Token insufficiente |
| Giapponese | 日本語 | `ja` | `JP` | ✅ | |
| Coreano | 한국어 | `ko` | `KO` | ❌ | Saldo Token insufficiente |
| Olandese | Nederlands | `nl` | `NL` | ❌ | Saldo Token insufficiente |
| Norvegese | norsk | `no` | `NO` | ❌ | Saldo Token insufficiente |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Saldo Token insufficiente |
| Polacco | polski | `pl` | `PL` | ❌ | Saldo Token insufficiente |
| Portoghese (Portogallo) | português | `pt` | `PT` | ❌ | Saldo Token insufficiente |
| Portoghese (Brasile) | português do Brasil | `pt-br` | `PTBR` | ❌ | Saldo Token insufficiente |
| Rumeno | română | `ro` | `RO` | ❌ | Saldo Token insufficiente |
| Russo | русский | `ru` | `RU` | ❌ | Saldo Token insufficiente |
| Tailandese | ภาษาไทย | `th` | `TH` | ❌ | Saldo Token insufficiente |
| Turco | Türkçe | `tr` | `TR` | ❌ | Credito token insufficiente |
| Ucraino | українська | `uk` | `UA` | ❌ | Credito token insufficiente |

**Totale**: 27 lingue pianificate | **Supportate**: 5 | **Da supportare**: 22

---

## Come installare e utilizzare

Questa è una guida per i giocatori che vogliono utilizzare direttamente questo progetto di traduzione nel gioco.

1.  Vai alla pagina del nostro Workshop Steam: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Fai clic sul pulsante "Iscriviti".
3.  Avvia il gioco e abilita questo mod di traduzione nella gestione "Mod" del menu principale.
4.  I testi di traduzione dei mod abilitati successivamente sovrascrivono quelli dei mod abilitati in precedenza, quindi questo mod di traduzione deve essere abilitato dopo i mod funzionali (il più in basso possibile).
5.  Goditi il gioco!

---

## Stato della traduzione

**[➡️ Clicca qui per vedere lo stato della traduzione](./docs/progress/progress_it.md)**

---

## Come contribuire

Accogliamo con favore il contributo di chiunque, che si tratti di correggere un errore, aggiungere una funzionalità, scrivere un modello di prompt o fornire una traduzione di riferimento!

Chiamare l'API LLM per la traduzione richiede il pagamento dei token. Affinché il progetto possa funzionare stabilmente a lungo termine, speriamo che possiate generosamente contribuire!

Per i dettagli, leggi la [Guida ai contributi](./docs/contributing/contributing_it.md)

---

## Strumenti e struttura delle directory (per sviluppatori)

Questa sezione è rivolta agli sviluppatori che desiderano comprendere i principi di automazione del progetto.

### Directory del progetto

| Directory | Descrizione |
|------|------|
| `src/` | Codice sorgente della pipeline di traduzione .NET 10, che include 15 moduli + 2 moduli indipendenti |
| `config/` | File di configurazione della pipeline (parametri LLM, Steam, RAG, ecc.) |
| `data/` | Dati di esecuzione: metadati dei mod, embedding, cache di traduzione |
| `translation_ref/` | Dati di traduzione di riferimento (es. mod autorizzati dal gruppo di traduzione Ruyi), fornisce riferimenti di traduzione per LLM |
| `base_game_keys/` | Chiavi di traduzione del gioco base, utilizzate per la deduplicazione e per evitare di sovrascrivere il testo nativo |
| `final_outputs/` | Output finale: pacchetto mod `project_babel/`, icone `icons/` e descrizioni del workshop `workshop_descriptions/` |
| `docs/` | Documentazione del progetto: report di avanzamento, guida ai contributi, spiegazione della pipeline |
| `temp/` | File temporanei della pipeline (directory separata per ogni esecuzione) |
| `src/prompt_templates/` | Modelli di prompt LLM (traduzione/revisione dei contenuti) |

### Moduli della pipeline (in ordine di esecuzione)

| Passo | Modulo | Funzione |
|------|------|------|
| 1 | `ConfigReader` | Carica configurazioni/chiavi/elenco lingue |
| 2 | `RepoDataLoader` | Carica traduzioni di riferimento e cache di traduzione |
| 3 | `ModIdCollector` | Raccoglie ID mod Workshop |
| 4 | `ModInfoFetcher` | Recupera metadati Steam |
| 5 | `SteamCmdBootstrapper` | Prepara il runtime steamcmd per la piattaforma corrente |
| 6 | `ModDownloader` | Scarica mod tramite steamcmd |
| 7 | `ContentExtractor` | Analizza file di traduzione mod → `TranslationEntry` |
| 8 | `ContentChecker` | Revisione sicurezza contenuti (droga/pornografia/violenza) |
| 9 | `EmbeddingFetcher` | Calcola vettori embedding del testo |
| 10 | `TranslationBatcher` | Crea lotti di traduzione indipendenti dalla lingua target |
| 11 | `RagContextRetriever` | Recupera contesto RAG (chiave esatta + similarità embedding) |
| 12 | `LLMTranslator` | Chiama LLM per eseguire traduzione |
| 13 | `ResultWriter` | Scrive in data/ e translation_ref/ |
| 14 | `FinalOutputWriter` | Genera output formato mod PZ finale |
| 15 | `ProgressReporter` | Genera report di progresso |

### Moduli indipendenti

| Modulo | Funzione |
|------|------|
| `WorkshopMonitor` | Recupera periodicamente nuovi mod da Steam Workshop, li filtra per numero di iscrizioni e li aggiunge a `request_for_translation.txt` |
| `DocGenerator` | Generatore di documentazione multilingua basato su LLM |

### Stack tecnologico

- **Linguaggio**: C# (.NET 10)
- **Piattaforma target**: GitHub Actions Linux x64 runner
- **Test**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurabile)
- **Embedding**: Vettorizzazione del testo per ricerca di similarità RAG
- **Revisione contenuti**: Revisione di sicurezza multilivello basata su LLM

Dettagliato [riferimento tecnico](./docs/technical_reference/technical_reference_it.md).

---

## Copyright e Licenza

I contenuti testuali delle traduzioni e le immagini correlate di questo progetto di traduzione sono creati o rielaborati da **Project Babel** e dai vari partecipanti basandosi sui mod di gioco originali.

© 2025 Project Babel e i rispettivi autori. Tutti i diritti riservati.

### 1. Testo e immagini, ecc.

Salvo diversa indicazione, in questo repository:

- Contenuti di traduzione, revisione e correzione dei testi di gioco;
Documentazione del progetto, traduzione dei testi nei mod;
Immagini e risorse artistiche create appositamente per questo progetto

sono tutti concessi in licenza con **Attribuzione - Non commerciale - Condividi allo stesso modo 4.0 Internazionale** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abbreviato **CC BY-NC-SA 4.0**).

Ciò significa che, a condizione di rispettare le seguenti condizioni, puoi condividere e adattare liberamente questi contenuti:

- **Attribuzione (BY)**: Indicare in una posizione evidente "Questo progetto di traduzione è basato sul lavoro di 'Project Babel' ed è stato modificato", e allegare il link a questo repository e a Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Non commerciale (NC)**: Non è consentito utilizzare i contenuti di questo progetto o le opere derivate per scopi commerciali diretti o indiretti (inclusi, ma non limitati a, pacchetti a pagamento, download a pagamento, condivisione di entrate pubblicitarie, ecc.);
- **Condividi allo stesso modo (SA)**: Se modifichi o crei opere derivate basate su questo progetto, devi pubblicare la tua versione modificata con la **stessa licenza CC BY-NC-SA 4.0**.

Per maggiori informazioni su questa licenza, consulta:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.it>

*Note speciali:*
- *Il contenuto della cartella base_game_keys proviene dal gioco originale, il copyright è dei proprietari del gioco! Il contenuto viene utilizzato per evitare che le chiavi di traduzione sovrascrivano le chiavi del gioco (deduplicazione)*
- *Il contenuto della cartella translation_ref viene utilizzato per fornire riferimenti di traduzione all'LLM, il copyright è dei rispettivi sviluppatori dei mod!*

### 2. Programmi, script e altri contenuti di sviluppo

Salvo diversa dichiarazione nei file sorgente o nelle directory, il codice del programma in questo repository utilizzato per creare/pacchettizzare/elaborare i contenuti di traduzione (ad esempio il codice nella directory `src/`) è concesso in licenza con **GNU General Public License versione 3 (GPL-3.0)**.

Per i termini completi, consultare il file `LICENSE` nella directory principale di questo repository (GPL-3.0), o visitare il sito ufficiale GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Ringraziamenti

Questo progetto utilizza mod di terze parti come testi di riferimento per la traduzione nella lingua di destinazione; i testi di riferimento vengono inviati all'LLM come riferimento per la traduzione.

| Nome mod di riferimento | Autore | Pagina del mod |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Pagina Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Pagina Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Pagina Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Un sentito ringraziamento a tutti gli autori sopra menzionati!**

---

## Programmi di terze parti

Questo progetto utilizza programmi e librerie di terze parti; il copyright di questi programmi di terze parti appartiene ai rispettivi sviluppatori.

