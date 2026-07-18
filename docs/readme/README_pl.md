# Project Babel — Automatyczne tłumaczenie modów do Project Zomboid za pomocą LLM

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Ten projekt tłumaczeniowy jest napędzany i utrzymywany przez zestaw narzędzi [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Spis treści

- [Obsługiwane języki docelowe projektu](#obsługiwane-języki-docelowe-projektu)
- [Jak zainstalować i używać](#jak-zainstalować-i-używać)
- [Postęp tłumaczenia](#postęp-tłumaczenia)
- [Jak przyczynić się](#jak-przyczynić-się)
- [Narzędzia i struktura katalogów (dla deweloperów)](#narzędzia-i-struktura-katalogów-dla-deweloperów)
  - [Katalog projektu](#katalog-projektu)
  - [Moduły potoku (w kolejności wykonania)](#moduły-potoku-w-kolejności-wykonania)
  - [Moduły niezależne](#moduły-niezależne)
  - [Stos technologiczny](#stos-technologiczny)
- [Prawa autorskie i licencja](#prawa-autorskie-i-licencja)
  - [1. Tekst, obrazy i inne treści](#1-tekst-obrazy-i-inne-treści)
  - [2. Programy, skrypty i inne treści developerskie](#2-programy-skrypty-i-inne-treści-developerskie)
- [Podziękowania](#podziękowania)
- [Programy innych firm](#programy-innych-firm)

---

## Obsługiwane języki docelowe projektu

| Język | Nazwa lokalna | Kod międzynarodowy | Kod w grze | Obsługiwany | Uwagi |
|------|------|------|------|------|------|
| arabski | العربية | `ar` | `AR` | ❌ | Niewystarczający limit tokenów |
| kataloński | català | `ca` | `CA` | ❌ | Niewystarczający limit tokenów |
| chiński tradycyjny | 繁體中文 | `zh-hant` | `CH` | ❌ | Niewystarczający limit tokenów |
| chiński uproszczony | 简体中文 | `zh-hans` | `CN` | ✅ | |
| czeski | čeština | `cs` | `CS` | ❌ | Niewystarczający limit tokenów |
| duński | dansk | `da` | `DA` | ❌ | Niewystarczający limit tokenów |
| niemiecki | Deutsch | `de` | `DE` | ✅ | |
| angielski | English | `en` | `EN` | ✅ | |
| hiszpański | español | `es` | `ES` | ❌ | Niewystarczający limit tokenów |
| fiński | suomi | `fi` | `FI` | ❌ | Niewystarczający limit tokenów |
| francuski | français | `fr` | `FR` | ✅ | |
| węgierski | magyar | `hu` | `HU` | ❌ | Niewystarczający limit tokenów |
| indonezyjski | Bahasa Indonesia | `id` | `ID` | ❌ | Niewystarczający limit tokenów |
| włoski | italiano | `it` | `IT` | ❌ | Niewystarczający limit tokenów |
| japoński | 日本語 | `ja` | `JP` | ✅ | |
| koreański | 한국어 | `ko` | `KO` | ❌ | Niewystarczający limit tokenów |
| niderlandzki | Nederlands | `nl` | `NL` | ❌ | Niewystarczający limit tokenów |
| norweski | norsk | `no` | `NO` | ❌ | Niewystarczający limit tokenów |
| tagalski | Tagalog | `tl` | `PH` | ❌ | Niewystarczający limit tokenów |
| polski | polski | `pl` | `PL` | ❌ | Niewystarczający limit tokenów |
| portugalski (Portugalia) | português | `pt` | `PT` | ❌ | Niewystarczający limit tokenów |
| portugalski (Brazylia) | português do Brasil | `pt-br` | `PTBR` | ❌ | Niewystarczający limit tokenów |
| rumuński | română | `ro` | `RO` | ❌ | Niewystarczający limit tokenów |
| rosyjski | русский | `ru` | `RU` | ❌ | Niewystarczający limit tokenów |
| tajski | ภาษาไทย | `th` | `TH` | ❌ | Niewystarczający limit tokenów |
| Turecki | Türkçe | `tr` | `TR` | ❌ | Niewystarczający limit tokenów |
| Ukraiński | українська | `uk` | `UA` | ❌ | Niewystarczający limit tokenów |

**Razem**: 27 języków planowanych | **Wspierane**: 5 | **Do wsparcia**: 22

---

## Jak zainstalować i używać

To jest przewodnik dla graczy, którzy chcą bezpośrednio użyć tego projektu tłumaczenia w grze.

1.  Odwiedź naszą stronę Steam Workshop: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Kliknij przycisk "Subskrybuj".
3.  Uruchom grę i włącz ten mod tłumaczenia w menedżerze "Mody" w menu głównym gry.
4.  Tłumaczenia modów włączonych później mają pierwszeństwo, nadpisując te z wcześniejszych modów, dlatego ten mod tłumaczenia powinien być włączony po modach funkcyjnych (najlepiej na końcu).
5.  Ciesz się grą!

---

## Postęp tłumaczenia

**[➡️ Kliknij tutaj, aby zobaczyć postęp tłumaczenia](./docs/progress/progress_pl.md)**

---

## Jak przyczynić się

Zapraszamy każdego do udziału, czy to poprawianie błędów, dodawanie nowych funkcji, pisanie szablonów promptów, czy też dostarczanie referencyjnych tłumaczeń!

Korzystanie z API LLM do tłumaczenia wymaga płatności za tokeny. Aby projekt mógł działać stabilnie w dłuższej perspektywie, mamy nadzieję, że okażesz hojność!

Szczegóły znajdziesz w [Przewodniku kontrybucji](./docs/contributing/contributing_pl.md)

---

## Narzędzia i struktura katalogów (dla deweloperów)

Ta sekcja jest przeznaczona dla deweloperów, którzy chcą zrozumieć zasady automatyzacji projektu.

### Katalog projektu

| Katalog | Opis |
|------|------|
| `src/` | Kod źródłowy potoku tłumaczeń .NET 10, zawierający 15 modułów + 2 moduły niezależne |
| `config/` | Pliki konfiguracyjne potoku (parametry LLM, Steam, RAG itp.) |
| `data/` | Dane wykonawcze: metadane modów, embedding, pamięć podręczna tłumaczeń |
| `translation_ref/` | Referencyjne dane tłumaczeń (np. mody autoryzowane przez As1), dostarczające LLM-owi referencji tłumaczeniowych |
| `base_game_keys/` | Klucze tłumaczeń bazowej gry, używane do unikania nadpisywania oryginalnego tekstu |
| `final_outputs/` | Wyjście końcowe: pakiet modów `project_babel/`, ikony `icons/` oraz opisy warsztatu `workshop_descriptions/` |
| `docs/` | Dokumentacja projektu: raporty postępu, przewodnik kontrybucji, opis potoku |
| `temp/` | Pliki tymczasowe potoku (osobny katalog na każde uruchomienie) |
| `src/prompt_templates/` | Szablony promptów LLM (tłumaczenie/weryfikacja treści) |

### Moduły potoku (w kolejności wykonania)

| Krok | Moduł | Funkcja |
|------|------|------|
| 1 | `ConfigReader` | Ładuje konfigurację/klucze/listę języków |
| 2 | `RepoDataLoader` | Ładuje referencyjne tłumaczenia i pamięć podręczną tłumaczeń |
| 3 | `ModIdCollector` | Zbiera ID modów z Warsztatu |
| 4 | `ModInfoFetcher` | Pobiera metadane Steam |
| 5 | `SteamCmdBootstrapper` | Przygotowuje środowisko uruchomieniowe steamcmd dla bieżącej platformy |
| 6 | `ModDownloader` | Pobiera mody przez steamcmd |
| 7 | `ContentExtractor` | Parsuje pliki tłumaczeń modów → `TranslationEntry` |
| 8 | `ContentChecker` | Sprawdzanie bezpieczeństwa treści (narkotyki/pornografia/przemoc) |
| 9 | `EmbeddingFetcher` | Oblicza wektory osadzeń (embedding) tekstu |
| 10 | `TranslationBatcher` | Tworzy partie tłumaczeń niezależne od języka docelowego |
| 11 | `RagContextRetriever` | Pobiera kontekst RAG (dokładne klucze + podobieństwo osadzeń) |
| 12 | `LLMTranslator` | Wywołuje LLM do wykonania tłumaczenia |
| 13 | `ResultWriter` | Zapisuje do data/ i translation_ref/ |
| 14 | `FinalOutputWriter` | Generuje końcowe wyjście w formacie modów PZ |
| 15 | `ProgressReporter` | Generuje raport postępu |

### Moduły niezależne

| Moduł | Funkcja |
|------|------|
| `WorkshopMonitor` | Okresowo pobiera nowe mody z Steam Workshop, filtruje według liczby subskrypcji i dodaje do `request_for_translation.txt` |
| `DocGenerator` | Generator wielojęzycznej dokumentacji napędzany przez LLM |

### Stos technologiczny

- **Język**: C# (.NET 10)
- **Platforma docelowa**: GitHub Actions Linux x64 runner
- **Testy**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurowalny)
- **Embedding**: Wektoryzacja tekstu do wyszukiwania podobieństw RAG
- **Kontrola treści**: Wielopoziomowa kontrola bezpieczeństwa napędzana przez LLM

Szczegółowe [techniczne odniesienie](./docs/technical_reference/technical_reference_pl.md).

---

## Prawa autorskie i licencja

Treści tłumaczeń i powiązane obrazy tego projektu tłumaczeniowego zostały stworzone lub przerobione przez **Project Babel** oraz poszczególnych uczestników na podstawie oryginalnych modów do gry.

© 2025 Project Babel i poszczególni autorzy. Wszelkie prawa zastrzeżone.

### 1. Tekst, obrazy i inne treści

O ile nie zaznaczono inaczej, w tym repozytorium:

- Tłumaczenia tekstu w grze, poprawki i korekty;
Tłumaczenie dokumentacji projektu i tekstów w modach;
Obrazy i zasoby graficzne stworzone specjalnie dla tego projektu

wszystkie są licencjonowane na **Uznanie autorstwa-Użycie niekomercyjne-Na tych samych warunkach 4.0 Międzynarodowe** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, w skrócie **CC BY-NC-SA 4.0**).

Oznacza to, że możesz swobodnie udostępniać i modyfikować te treści, pod warunkiem przestrzegania następujących warunków:

- **Uznanie autorstwa (BY)**: W widocznym miejscu podaj informację „Ten projekt tłumaczenia opiera się na pracy 『Project Babel』” i dołącz link do tego repozytorium oraz Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Użycie niekomercyjne (NC)**: Nie wolno wykorzystywać treści tego projektu ani ich adaptacji do żadnych bezpośrednich ani pośrednich celów komercyjnych (w tym między innymi płatnych pakietów, płatnych pobrań, udziału w zyskach z reklam itp.);
- **Na tych samych warunkach (SA)**: Jeśli modyfikujesz lub tworzysz dzieła pochodne na podstawie treści tego projektu, musisz opublikować swoją zmodyfikowaną wersję na **tej samej licencji CC BY-NC-SA 4.0**.

Więcej informacji na temat tej licencji można znaleźć pod adresem:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.pl>

*Uwagi specjalne:*
- *Zawartość folderu base_game_keys pochodzi z oryginalnej gry, prawa autorskie należą do jej twórców! Treść służy do zapobiegania nadpisywaniu kluczy tłumaczenia gry (deduplikacja)*
- *Zawartość folderu translation_ref służy jako odniesienie tłumaczeniowe dla LLM, prawa autorskie należą do twórców poszczególnych modów!*

### 2. Programy, skrypty i inne treści developerskie

O ile w plikach źródłowych lub katalogach nie zaznaczono inaczej, kod programu w tym repozytorium używany do tworzenia/pakowania/przetwarzania treści lokalizacji (np. kod w katalogu `src/`) jest licencjonowany na **GNU General Public License wersja 3 (GPL-3.0)**.

Pełne warunki znajdują się w pliku `LICENSE` w katalogu głównym (GPL-3.0) lub na stronie GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Podziękowania

Ten projekt wykorzystuje mody innych firm jako teksty referencyjne do tłumaczenia docelowego języka. Teksty referencyjne są wysyłane do LLM jako pomoc tłumaczeniowa.

| Nazwa modu referencyjnego | Autor | Strona modu |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Strona Warsztatu Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Strona Warsztatu Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Strona Warsztatu Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Serdeczne podziękowania dla wszystkich autorów wymienionych powyżej!**

---

## Programy innych firm

Ten projekt korzysta z programów i bibliotek innych firm. Prawa autorskie do tych programów należą do odpowiednich twórców.

