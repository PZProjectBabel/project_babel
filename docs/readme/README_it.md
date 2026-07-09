# Project Babel — Traduzione automatica delle mod PZ con LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Nota:** Questa traduzione non è ancora supportata. Il contenuto di riferimento è la [versione cinese](../../README.md).

---

*Questo progetto di traduzione è gestito dallo strumento [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Indice

- [Lingue target supportate](#lingue-target-supportate)
- [Installazione e utilizzo](#installazione-e-utilizzo)
- [Progresso della traduzione](#progresso-della-traduzione)
- [Contribuire](#contribuire)
- [Strumenti e struttura delle directory (per sviluppatori)](#strumenti-e-struttura-delle-directory-(per-sviluppatori))
- [Copyright e licenza](#copyright-e-licenza)
- [Riconoscimenti](#riconoscimenti)
- [Software di terze parti](#software-di-terze-parti)

---

## Lingue target supportate

| Lingua | Nome locale | Codice ISO | Codice in gioco | Supportata | Note |
|------|------|------|------|------|------|
| Arabo | العربية | `ar` | `AR` | ❌ | Crediti token insufficienti |
| Catalano | català | `ca` | `CA` | ❌ | Crediti token insufficienti |
| Cinese tradizionale | 繁體中文 | `zh-hant` | `CH` | ❌ | Crediti token insufficienti |
| Cinese semplificato | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Ceco | čeština | `cs` | `CS` | ❌ | Crediti token insufficienti |
| Danese | dansk | `da` | `DA` | ❌ | Crediti token insufficienti |
| Tedesco | Deutsch | `de` | `DE` | ✅ | |
| Inglese | English | `en` | `EN` | ✅ | |
| Spagnolo | español | `es` | `ES` | ❌ | Crediti token insufficienti |
| Finlandese | suomi | `fi` | `FI` | ❌ | Crediti token insufficienti |
| Francese | français | `fr` | `FR` | ✅ | |
| Ungherese | magyar | `hu` | `HU` | ❌ | Crediti token insufficienti |
| Indonesiano | Bahasa Indonesia | `id` | `ID` | ❌ | Crediti token insufficienti |
| Italiano | italiano | `it` | `IT` | ❌ | Crediti token insufficienti |
| Giapponese | 日本語 | `ja` | `JP` | ✅ | |
| Coreano | 한국어 | `ko` | `KO` | ❌ | Crediti token insufficienti |
| Olandese | Nederlands | `nl` | `NL` | ❌ | Crediti token insufficienti |
| Norvegese | norsk | `no` | `NO` | ❌ | Crediti token insufficienti |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Crediti token insufficienti |
| Polacco | polski | `pl` | `PL` | ❌ | Crediti token insufficienti |
| Portoghese (Portogallo) | português | `pt` | `PT` | ❌ | Crediti token insufficienti |
| Portoghese (Brasile) | português do Brasil | `pt-br` | `PTBR` | ❌ | Crediti token insufficienti |
| Rumeno | română | `ro` | `RO` | ❌ | Crediti token insufficienti |
| Russo | русский | `ru` | `RU` | ❌ | Crediti token insufficienti |
| Thailandese | ภาษาไทย | `th` | `TH` | ❌ | Crediti token insufficienti |
| Turco | Türkçe | `tr` | `TR` | ❌ | Crediti token insufficienti |
| Ucraino | українська | `uk` | `UA` | ❌ | Crediti token insufficienti |

**Totale**: 27 lingue pianificate | **Supportate**: 5 | **In attesa**: 22

---

## Installazione e utilizzo

Guida per i giocatori che vogliono usare il pacchetto di traduzione in gioco.

1. Vai alla pagina Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Clicca su "Iscriviti".
3. Avvia il gioco, abilita questa mod di traduzione nel menu Mod.
4. Il testo di traduzione delle mod caricate dopo sovrascrive le precedenti, quindi questa mod di traduzione deve essere caricata dopo le mod di gioco.
5. Buon divertimento!

---

## Progresso della traduzione

[➡️ Progresso della traduzione](../progress/progress_it.md)

---

## Contribuire

Accettiamo contributi! Correzioni di traduzione, nuove funzionalità, modelli di prompt o traduzioni di riferimento.

Le chiamate API LLM per la traduzione comportano costi in token. Il tuo supporto aiuta il progetto a funzionare in modo sostenibile!

Leggi la [Guida al Contributo](../contributing/contributing_it.md) per i dettagli.

---

## Strumenti e struttura delle directory (per sviluppatori)

Questa sezione è rivolta agli sviluppatori che desiderano comprendere il funzionamento interno dell'automazione del progetto.

### Directory del progetto

| Directory | Descrizione |
|------|------|
| `src/` | Codice sorgente pipeline .NET 10, 15 moduli |
| `config/` | Configurazione pipeline (LLM, Steam, parametri RAG, ecc.) |
| `data/` | Dati runtime: metadati mod, embedding, cache traduzioni |
| `translation_ref/` | Traduzioni di riferimento come contesto LLM |
| `base_game_keys/` | Chiavi di traduzione del gioco base per deduplicazione |
| `final_outputs/` | Output finale in formato mod PZ |
| `docs/` | Documentazione: progresso, contributi, specifiche pipeline |
| `temp/` | File temporanei della pipeline |
| `src/prompt_templates/` | Modelli di prompt LLM |

### Moduli della pipeline (ordine di esecuzione)

| Passo | Modulo | Funzione |
|------|------|------|
| 1 | `ConfigReader` | Carica configurazione/segreti/lingue |
| 2 | `RepoDataLoader` | Carica riferimenti e cache traduzioni |
| 3 | `ModIdCollector` | Raccogli ID mod Workshop |
| 4 | `ModInfoFetcher` | Recupera metadati Steam |
| 5 | `ModDownloader` | Scarica mod via steamcmd |
| 6 | `ContentExtractor` | Analizza file traduzione → `TranslationEntry` |
| 7 | `ContentChecker` | Revisione sicurezza contenuti |
| 8 | `EmbeddingFetcher` | Calcola vettori embedding testo |
| 9 | `TranslationBatcher` | Crea lotti di traduzione |
| 10 | `RagContextRetriever` | Recupera contesti RAG |
| 11 | `LLMTranslator` | Esegui traduzione LLM |
| 12 | `ResultWriter` | Scrivi in data/ e translation_ref/ |
| 13 | `FinalOutputWriter` | Genera output finale formato mod PZ |
| 14 | `ProgressReporter` | Genera report di progresso |

### Stack tecnologico

- **Linguaggio**: C# (.NET 10)
- **Piattaforma target**: GitHub Actions Linux x64 runner
- **Test**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurabile)
- **Embedding**: Vettorizzazione del testo per ricerca di similarità RAG
- **Revisione contenuti**: Audit di sicurezza multilivello guidato da LLM

Documentazione tecnica dettagliata: [Pipeline TranslationEntry](../pipeline/translation_entry_pipeline_it.md)

---

## Copyright e licenza

© 2025 Project Babel e tutti gli autori. Tutti i diritti riservati.

### Contenuti (testi, immagini)

Concesso in licenza sotto **CC BY-NC-SA 4.0**.

- **Attribuzione**: Indicare le modifiche basate su "Project Babel", con link al repository e Workshop
- **Non commerciale**: Uso commerciale vietato
- **Condividi allo stesso modo**: Le modifiche devono essere pubblicate sotto la stessa licenza

### Codice

Il codice in `src/` è concesso in licenza sotto **GPL-3.0**.

---

## Riconoscimenti

| Mod di riferimento | Autore | Pagina |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Sentiti ringraziamenti agli autori sopra!**

---

## Software di terze parti

Questo progetto utilizza programmi e librerie di terze parti, i diritti d'autore appartengono ai rispettivi sviluppatori.
