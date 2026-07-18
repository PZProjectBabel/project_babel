# Przewodnik kontrybucji (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Spis treści

- [1. Zanim zaczniesz](#1-zanim-zaczniesz)
- [2. Jak mogę pomóc?](#2-jak-mogę-pomóc)
- [3. Dostarczanie reguł tłumaczeniowych, słowników terminów i ulepszanie promptów systemowych](#3-dostarczanie-reguł-tłumaczeniowych-słowników-terminów-i-ulepszanie-promptów-systemowych)
- [4. Dostarczanie ręcznie sprawdzonych danych językowych](#4-dostarczanie-ręcznie-sprawdzonych-danych-językowych)
- [5. Wkład w rozwój pipeline'u i narzędzi](#5-wkład-w-rozwój-pipelineu-i-narzędzi)
- [6. Prawa autorskie i umowa licencyjna](#6-prawa-autorskie-i-umowa-licencyjna)
  - [6.1 Podstawowa zasada: Zachowujesz prawa autorskie i jednocześnie udzielasz licencji projektowi](#61-podstawowa-zasada-zachowujesz-prawa-autorskie-i-jednocześnie-udzielasz-licencji-projektowi)
  - [6.2 Licencja treści tekstowych i graficznych (CC BY-NC-SA 4.0)](#62-licencja-treści-tekstowych-i-graficznych-cc-by-nc-sa-40)
  - [6.3 Licencja skryptów i kodu narzędzi (GPL-3.0)](#63-licencja-skryptów-i-kodu-narzędzi-gpl-30)
  - [6.4 Prawa autorskie utworów nadrzędnych i oryginalnej gry](#64-prawa-autorskie-utworów-nadrzędnych-i-oryginalnej-gry)
- [7. Komunikacja i współpraca](#7-komunikacja-i-współpraca)
- [8. Wsparcie finansowe](#8-wsparcie-finansowe)

---

Bardzo dziękujemy za chęć wniesienia wkładu w **Project Babel – mod do gry *Project Zomboid* z automatycznym tłumaczeniem przez LLM**! Możesz poprawić błąd, dodać nową funkcję, napisać szablon promptu lub dostarczyć referencyjne tłumaczenie!

Korzystanie z API LLM do tłumaczenia wiąże się z opłatami za tokeny. Aby projekt mógł działać stabilnie na dłuższą metę, liczymy na Twoją hojną pomoc!

> ⚠️ **Ważne przypomnienie:**
> Przed przesłaniem jakichkolwiek treści do tego repozytorium przeczytaj i zrozum sekcję „Umowa dotycząca praw autorskich i licencji”.
> Po przesłaniu i scaleniu Twojego wkładu uznaje się, że zgadzasz się na odpowiednie warunki licencji.

---

## 1. Zanim zaczniesz

Najpierw przeczytaj plik `README.md` projektu, aby poznać:
- ogólny cel projektu i jego obecny stan;
- jak zwykły gracz może korzystać z projektu (ułatwia to samodzielne testowanie);
- szczegóły techniczne projektu.

---

## 2. Jak mogę pomóc?

Możesz wybrać jeden lub więcej sposobów uczestnictwa, w zależności od swoich zainteresowań i umiejętności:

- Dostarczenie reguł tłumaczeniowych dla języka docelowego
- Dostarczenie słownika terminów tłumaczeniowych dla języka docelowego
- Udoskonalenie promptów systemowych
- Dostarczenie ręcznie poprawionych korpusów tłumaczeniowych
- Udoskonalenie modułów potoku (.NET) i skryptów automatyzacji
- Zgłaszanie problemów i propozycji ulepszeń (w sekcji Issues)
- Wsparcie finansowe na wywołania LLM

Poniżej znajdują się opisy głównych obszarów wkładu.

---

## 3. Dostarczanie reguł tłumaczeniowych, słowników terminów i ulepszanie promptów systemowych

Szablony promptów potoku znajdują się w `src/prompt_templates/`, a ich struktura jest następująca:

- `system_prompt_translate_engine.txt`: globalny prompt systemowy silnika tłumaczeniowego (wspólny dla wszystkich języków);
- `<kod_języka>/translation_dictionary_<kod_języka>.json`: słownik terminów dla danego języka;
- `<kod_języka>/translation_schema_<kod_języka>.md`: reguły tłumaczeniowe i ograniczenia stylistyczne dla danego języka.

Kroki, aby wnieść wkład:

1. Utwórz podkatalog dla swojego języka w `src/prompt_templates/` i dodaj pliki słownika terminów oraz reguł tłumaczeniowych;
2. Jeśli chcesz zmienić globalne zachowanie tłumaczenia, zmodyfikuj `system_prompt_translate_engine.txt` (pamiętaj, że wpływa to na wszystkie języki);
3. Przetestuj lokalnie, aby potwierdzić efekt;
4. Złóż PR.

---

## 4. Dostarczanie ręcznie sprawdzonych danych językowych

Jeśli jesteś twórcą modu tłumaczeniowego i chcesz udostępnić swoje dane tłumaczeniowe jako referencję dla tłumaczania LLM, zgłoś to w Issue. Musisz dostarczyć następujące informacje:

- ID Twojego modu tłumaczeniowego oraz docelowy język tłumaczenia;
- Zrzut ekranu strony backendu Twojego modu tłumaczeniowego, potwierdzający, że jesteś autorem modu;
- Wyraźnie zaznacz w Issue, że chcesz udostępnić dane tłumaczeniowe;
- Jeśli występują szczególne okoliczności (specjalne licencje itp.), również je opisz;
- Upewnij się, że dostarczone dane są wysokiej jakości.

Na podstawie Twojej zgody, projekt doda Twój mod do listy referencyjnych modów tłumaczeniowych w `config/ref_translation_mods.json`, a pipeline automatycznie zsynchronizuje Twoje tłumaczenie jako dane referencyjne RAG.

---

## 5. Wkład w rozwój pipeline'u i narzędzi

Automatyzacja tego projektu składa się z dwóch części:

**Moduł potoku (`src/`, C# / .NET 10)**：Zawiera 15 modułów wykonywanych sekwencyjnie, plus 2 moduły samodzielne (`WorkshopMonitor` do wykrywania modów, `DocGenerator` do generowania dokumentacji), odpowiedzialne za pełny proces od inicjalizacji SteamCMD, pobierania modów, ekstrakcji tekstu, przeglądu treści, obliczania Embedding, wyszukiwania RAG po tłumaczenie LLM i końcowe wyjście. Zobacz [odniesienie techniczne](../technical_reference/technical_reference_pl.md).

**Skrypty pomocnicze (`.github/`)**: Do automatyzacji na GitHubie.

Jeśli chcesz:

* Naprawić błędy w istniejących modułach pipeline'u lub skryptach;
* Dodać nowe funkcje lub moduły do pipeline'u;
* Zoptymalizować wydajność lub strukturę kodu;
* Ulepszyć szablony prompt lub strategię RAG;

Można postępować według następujących kroków:

1. Sforkuj to repozytorium i sklonuj je lokalnie;
2. Utwórz nową gałąź na podstawie najnowszej gałęzi;
3. Zmodyfikuj lub dodaj pliki w odpowiednich katalogach:
- Modyfikacja modułu pipeline'u → `src/<nazwa_modułu>/`;
- Modyfikacja przepływu pracy CI → `.github/workflows/`;
- Modyfikacja szablonów prompt → `src/prompt_templates/`;
4. Przed złożeniem postaraj się:

* Zachować istniejący styl kodu;
* Dodać niezbędne komentarze;
* Jeśli to możliwe, dołączyć proste testy lub instrukcje użytkowania;
5. Prześlij zmiany przez PR i opisz w opisie:

* Cel zmian;
* Katalogi / moduły / skrypty, które mogą być dotknięte;
* Czy wprowadza zmiany łamiące zgodność wsteczną.

---

## 6. Prawa autorskie i umowa licencyjna

> **Uwaga:**
> Umowa licencyjna ma na celu ochronę praw projektów, autorów, współtwórców i graczy, aby uniknąć nieporozumień wynikających z „domniemania” lub „domyślności”. Prosimy uważnie przeczytać.
> Prawa autorskie i licencja określone są w pliku README.md. Ta sekcja zawiera jedynie bardziej przystępny opis.

### 6.1 Podstawowa zasada: Zachowujesz prawa autorskie i jednocześnie udzielasz licencji projektowi

* Nadal zachowujesz prawa autorskie do treści, które stworzyłeś (tłumaczenia, obrazy, skrypty/programy itp.);
* Jednak po przesłaniu tych treści do tego projektu i ich zaakceptowaniu (scaleniu) zgadzasz się udzielić licencji na korzystanie z tych treści innym na warunkach przyjętej przez projekt licencji open source / share-alike.

Oznacza to:

* Nadal **możesz** używać i prezentować swoją pracę w innych miejscach;
* Ale **nie możesz** po scaleniu wkładu żądać od tego projektu ani innych użytkowników, którzy legalnie uzyskali dzieło, „wycofania licencji” lub „usunięcia historycznych wersji”.

### 6.2 Licencja treści tekstowych i graficznych (CC BY-NC-SA 4.0)

Dla następujących treści, które przesłałeś:

* Tłumaczenia tekstów gier, redakcja i korekta;
* Dokumentacja projektu, teksty opisowe;
* Obrazy i zasoby graficzne stworzone specjalnie dla tego projektu;

Po zaakceptowaniu i scaleniu przez to repozytorium uważa się, że wyrażasz zgodę na:

1. Treści te są licencjonowane na zasadzie **Uznanie autorstwa-Użycie niekomercyjne-Na tych samych warunkach 4.0 Międzynarodowe** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, w skrócie **CC BY-NC-SA 4.0**);
2. Project Babel oraz wszyscy użytkownicy, którzy uzyskali te treści, mogą, pod warunkiem **przestrzegania warunków CC BY-NC-SA 4.0**:
* Udostępniać, kopiować i dalej rozpowszechniać te treści;
* Modyfikować i tworzyć dzieła pochodne w celach niekomercyjnych;
3. Zgadzasz się, że w zakresie dozwolonym przez obowiązujące prawo niniejsza licencja jest **niewyłączna, globalna, nieodpłatna i nieodwołalna**;
4. Nawet jeśli w przyszłości wycofasz się lub przestaniesz uczestniczyć w tym projekcie, projekt ten może nadal korzystać i ponownie publikować na podstawie CC BY-NC-SA 4.0 treści, które już przesłałeś i zostały scalone.

> Jeśli nie akceptujesz powyższej licencji, nie przesyłaj do tego projektu wkładów w postaci tekstów lub obrazów,
> lub wcześniej skontaktuj się z opiekunem projektu, aby ustalić, czy współpraca jest możliwa w inny sposób.

### 6.3 Licencja skryptów i kodu narzędzi (GPL-3.0)

Dla następujących, które przesłałeś i zostały zaakceptowane:

* Skrypty automatyzacji;
* Narzędzia do budowania/eksportu;
* Inny kod programu do obsługi tego projektu tłumaczeniowego;

Bez specjalnego oświadczenia zakłada się, że wyrażasz zgodę na:

1. Kod jest licencjonowany na **GPL-3.0** (GNU General Public License wersja 3);
2. Opiekunowie projektu mogą modyfikować, scalać i rozpowszechniać go w zakresie dozwolonym przez GPL-3.0;
3. Możesz również kontynuować inne projekty w oparciu o ten sam kod, pod warunkiem przestrzegania postanowień GPL-3.0.

Aby uniknąć konfliktów licencyjnych, postaraj się:

* Nie wprowadzać kodu stron trzecich, który jest **niezgodny z GPL-3.0**, bez uprzedniego potwierdzenia;
* Jeśli konieczne jest użycie biblioteki innej firmy, jasno opisz w PR jej źródło i licencję oraz potwierdź zgodność.

### 6.4 Prawa autorskie utworów nadrzędnych i oryginalnej gry

Ten projekt jest **nieoficjalnym tłumaczeniem** modów związanych z grą Project Zomboid:

* Prawa autorskie do oryginalnej gry i poszczególnych modów należą do ich autorów/wydawców;
* Niniejszy projekt dotyczy wyłącznie tłumaczenia tekstu, poprawek stylistycznych i części towarzyszących zasobów;
* Wnoszący wkład powinni upewnić się podczas przesyłania treści:
* Nie kopiują bezpośrednio nieautoryzowanych tekstów tłumaczeń lub zasobów graficznych stron trzecich;
* Szanują prawa oryginalnych autorów i twórców modów, nie naruszają praw autorskich.

---

## 7. Komunikacja i współpraca

Jeśli masz pytania dotyczące:

* Warunków licencyjnych;
* Nie jesteś pewien, czy dany fragment można przekazać;
* Chcesz udzielić licencji na swoją pracę w szczególny sposób (np. tylko do użytku niekomercyjnego, bez możliwości adaptacji);

Skontaktuj się z opiekunem projektu w następujący sposób:

* Zgłoś Issue w celu dyskusji;
* Inne publicznie udostępnione dane kontaktowe opiekunów.

Postaramy się znaleźć rozwiązanie, które uwzględnia prawa wszystkich stron, przy jednoczesnym zapewnieniu zdrowego rozwoju projektu.

---

## 8. Wsparcie finansowe

Podczas działania projektu, ze względu na dodawanie nowych modów, aktualizacje treści starych modów itp., konieczne jest ciągłe wywoływanie API LLM do tłumaczenia. Aby ograniczyć zachowanie LLM, oprócz podstawowego tekstu modów, należy dostarczyć dużą ilość treści podpowiedzi (w tym podstawowe podpowiedzi, zasady tłumaczenia, glosariusze, ograniczenia wejścia/wyjścia, wyniki zapytań semantycznych itp.), które zużywają znacznie więcej tokenów niż oryginalny tekst. Dlatego projekt potrzebuje wsparcia finansowego.

Jeśli chcesz wesprzeć finansowo, skontaktuj się z opiekunem projektu. Dziękuję bardzo!

---

Jeszcze raz dziękuję za chęć wniesienia wkładu w ten projekt!
Twoja każda pomoc przynosi korzyści większej liczbie graczy!
