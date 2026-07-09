# Project Babel — Automatyczne tłumaczenie modów PZ przez LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Uwaga:** To tłumaczenie nie jest jeszcze wspierane. Autorytatywna treść znajduje się w [wersji chińskiej](../../README.md).

---

*Ten projekt tłumaczeniowy jest napędzany i utrzymywany przez narzędzie [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Spis treści

- [Obsługiwane języki docelowe](#obsługiwane-języki-docelowe)
- [Instalacja i użytkowanie](#instalacja-i-użytkowanie)
- [Postęp tłumaczenia](#postęp-tłumaczenia)
- [Współpraca](#współpraca)
- [Narzędzia i struktura katalogów (dla deweloperów)](#narzędzia-i-struktura-katalogów-(dla-deweloperów))
- [Prawa autorskie i licencja](#prawa-autorskie-i-licencja)
- [Podziękowania](#podziękowania)
- [Oprogramowanie stron trzecich](#oprogramowanie-stron-trzecich)

---

## Obsługiwane języki docelowe

| Język | Nazwa lokalna | Kod ISO | Kod w grze | Obsługiwany | Uwagi |
|------|------|------|------|------|------|
| Arabski | العربية | `ar` | `AR` | ❌ | Niewystarczające tokeny |
| Kataloński | català | `ca` | `CA` | ❌ | Niewystarczające tokeny |
| Tradycyjny chiński | 繁體中文 | `zh-hant` | `CH` | ❌ | Niewystarczające tokeny |
| Uproszczony chiński | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Czeski | čeština | `cs` | `CS` | ❌ | Niewystarczające tokeny |
| Duński | dansk | `da` | `DA` | ❌ | Niewystarczające tokeny |
| Niemiecki | Deutsch | `de` | `DE` | ✅ | |
| Angielski | English | `en` | `EN` | ✅ | |
| Hiszpański | español | `es` | `ES` | ❌ | Niewystarczające tokeny |
| Fiński | suomi | `fi` | `FI` | ❌ | Niewystarczające tokeny |
| Francuski | français | `fr` | `FR` | ✅ | |
| Węgierski | magyar | `hu` | `HU` | ❌ | Niewystarczające tokeny |
| Indonezyjski | Bahasa Indonesia | `id` | `ID` | ❌ | Niewystarczające tokeny |
| Włoski | italiano | `it` | `IT` | ❌ | Niewystarczające tokeny |
| Japoński | 日本語 | `ja` | `JP` | ✅ | |
| Koreański | 한국어 | `ko` | `KO` | ❌ | Niewystarczające tokeny |
| Holenderski | Nederlands | `nl` | `NL` | ❌ | Niewystarczające tokeny |
| Norweski | norsk | `no` | `NO` | ❌ | Niewystarczające tokeny |
| Tagalski | Tagalog | `tl` | `PH` | ❌ | Niewystarczające tokeny |
| Polski | polski | `pl` | `PL` | ❌ | Niewystarczające tokeny |
| Portugalski (Portugalia) | português | `pt` | `PT` | ❌ | Niewystarczające tokeny |
| Portugalski (Brazylia) | português do Brasil | `pt-br` | `PTBR` | ❌ | Niewystarczające tokeny |
| Rumuński | română | `ro` | `RO` | ❌ | Niewystarczające tokeny |
| Rosyjski | русский | `ru` | `RU` | ❌ | Niewystarczające tokeny |
| Tajski | ภาษาไทย | `th` | `TH` | ❌ | Niewystarczające tokeny |
| Turecki | Türkçe | `tr` | `TR` | ❌ | Niewystarczające tokeny |
| Ukraiński | українська | `uk` | `UA` | ❌ | Niewystarczające tokeny |

**Łącznie**: 27 planowanych języków | **Obsługiwane**: 5 | **Oczekujące**: 22

---

## Instalacja i użytkowanie

Przewodnik dla graczy, którzy chcą używać pakietu tłumaczeniowego w grze.

1. Przejdź na stronę Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Kliknij "Subskrybuj".
3. Uruchom grę, włącz ten mod tłumaczeniowy w menu Mods.
4. Tekst tłumaczenia z później załadowanych modów nadpisuje wcześniejsze, więc ten mod tłumaczeniowy musi być załadowany po modach rozgrywki.
5. Miłej zabawy!

---

## Postęp tłumaczenia

[➡️ Postęp tłumaczenia](../progress/progress_pl.md)

---

## Współpraca

Zapraszamy do współpracy! Poprawki tłumaczeń, nowe funkcje, szablony promptów lub tłumaczenia referencyjne.

Wywołania API LLM do tłumaczenia wiążą się z kosztami tokenów. Twoje wsparcie pomaga projektowi działać w sposób zrównoważony!

Read the [Contributing Guide](../contributing/contributing_pl.md) for details.

---

## Narzędzia i struktura katalogów (dla deweloperów)

Ta sekcja jest przeznaczona dla programistów, którzy chcą zrozumieć wewnętrzne działanie automatyzacji projektu.

### Katalogi projektu

| Katalog | Opis |
|------|------|
| `src/` | Kod źródłowy potoku tłumaczeń .NET 10, 15 modułów |
| `config/` | Konfiguracja potoku (LLM, Steam, parametry RAG itp.) |
| `data/` | Dane runtime: metadane modów, embeddingi, pamięć podręczna |
| `translation_ref/` | Tłumaczenia referencyjne jako kontekst LLM |
| `base_game_keys/` | Klucze tłumaczeń gry podstawowej do deduplikacji |
| `final_outputs/` | Końcowe wyjście w formacie moda PZ |
| `docs/` | Dokumentacja: postęp, wkład, specyfikacje potoku |
| `temp/` | Pliki tymczasowe potoku |
| `src/prompt_templates/` | Szablony promptów LLM |

### Moduły potoku (kolejność wykonania)

| Krok | Moduł | Funkcja |
|------|------|------|
| 1 | `ConfigReader` | Załaduj konfigurację/sekrety/języki |
| 2 | `RepoDataLoader` | Załaduj odniesienia i pamięć podręczną tłumaczeń |
| 3 | `ModIdCollector` | Zbierz ID modów Workshop |
| 4 | `ModInfoFetcher` | Pobierz metadane Steam |
| 5 | `ModDownloader` | Pobierz mody przez steamcmd |
| 6 | `ContentExtractor` | Analizuj pliki tłumaczeń → `TranslationEntry` |
| 7 | `ContentChecker` | Przegląd bezpieczeństwa treści |
| 8 | `EmbeddingFetcher` | Oblicz wektory osadzania tekstu |
| 9 | `TranslationBatcher` | Utwórz partie tłumaczeń |
| 10 | `RagContextRetriever` | Pobierz konteksty RAG |
| 11 | `LLMTranslator` | Wykonaj tłumaczenie LLM |
| 12 | `ResultWriter` | Zapisz do data/ i translation_ref/ |
| 13 | `FinalOutputWriter` | Wygeneruj końcowe wyjście w formacie moda PZ |
| 14 | `ProgressReporter` | Wygeneruj raporty postępu |

### Stos technologiczny

- **Język**: C# (.NET 10)
- **Platforma docelowa**: GitHub Actions Linux x64 runner
- **Testy**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurowalne)
- **Embedding**: Wektoryzacja tekstu do wyszukiwania podobieństwa RAG
- **Kontrola treści**: Wielopoziomowy audyt bezpieczeństwa oparty na LLM

Szczegółowa dokumentacja techniczna: [Pipeline TranslationEntry](../pipeline/translation_entry_pipeline_pl.md)

---

## Prawa autorskie i licencja

© 2025 Project Babel i wszyscy autorzy. Wszelkie prawa zastrzeżone.

### Treść (teksty, obrazy)

Licencjonowane na **CC BY-NC-SA 4.0**.

- **Uznanie autorstwa**: Wskaż modyfikacje oparte na "Project Babel", z linkami do repozytorium i Workshop
- **Użycie niekomercyjne**: Użycie komercyjne zabronione
- **Na tych samych warunkach**: Modyfikacje muszą być publikowane na tej samej licencji

### Kod

Kod w `src/` jest objęty licencją **GPL-3.0**.

---

## Podziękowania

| Mod referencyjny | Autor | Strona |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Serdeczne podziękowania dla powyższych autorów!**

---

## Oprogramowanie stron trzecich

Ten projekt korzysta z programów i bibliotek stron trzecich, prawa autorskie należą do ich twórców.
