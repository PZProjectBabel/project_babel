# Przewodnik po współtworzeniu (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Dziękujemy za chęć wniesienia wkładu w **Project Babel — projekt automatycznego tłumaczenia modów Project Zomboid przy użyciu LLM**! Niezależnie od tego, czy chodzi o naprawienie błędu, dodanie funkcji, pisanie szablonów promptów czy dostarczanie tłumaczeń referencyjnych — każdy wkład ma znaczenie!

Wywołanie API LLM do tłumaczenia wiąże się z kosztem tokenów. Aby projekt mógł działać stabilnie w dłuższej perspektywie, będziemy bardzo wdzięczni za Twoje hojne wsparcie!

> ⚠️ **Ważna informacja:**
> Przed przesłaniem czegokolwiek do tego repozytorium koniecznie przeczytaj i zrozum sekcję „Prawa autorskie i licencje".
> Przesłanie i scalenie oznacza zgodę na odpowiednie warunki licencji.

---

## Zanim zaczniesz

Przeczytaj `README.md` projektu, aby zrozumieć:

- Ogólne cele i bieżący stan projektu;
- Jak zwykli gracze korzystają z tego projektu (do własnych testów);
- Szczegóły techniczne projektu.

---

## Jak mogę wnieść wkład?

Możesz wybrać jeden lub więcej sposobów uczestnictwa w zależności od swoich zainteresowań i umiejętności:

- Dostarczenie reguł tłumaczenia dla języka docelowego
- Dostarczenie słownika terminologicznego dla języka docelowego
- Ulepszanie promptów systemowych
- Dostarczanie ręcznie poprawionych korpusów tłumaczeniowych
- Ulepszanie modułów potoku (.NET) i skryptów automatyzacji
- Zgłaszanie problemów i sugerowanie ulepszeń (poprzez Issues)
- Zapewnienie wsparcia finansowego na wywołania API LLM

Poniżej znajdują się wyjaśnienia głównych scenariuszy współtworzenia.

---

## Dostarczanie reguł tłumaczenia, słowników terminologicznych i ulepszanie promptów systemowych

Szablony promptów potoku znajdują się w `src/prompt_templates/`, o następującej strukturze:

- `system_prompt_translate_engine.txt`: globalny prompt systemowy silnika tłumaczenia (wspólny dla wszystkich języków);
- `<kod_języka>/translation_dictionary_<kod_języka>.json`: słownik terminologiczny dla danego języka;
- `<kod_języka>/translation_schema_<kod_języka>.md`: reguły tłumaczenia i ograniczenia stylistyczne dla danego języka.

Kroki współtworzenia:

1. Utwórz podkatalog w `src/prompt_templates/` dla swojego języka i dodaj pliki słownika oraz reguł tłumaczenia;
2. Jeśli potrzebujesz dostosować globalne zachowanie tłumaczenia, zmodyfikuj `system_prompt_translate_engine.txt` (uwaga: wpływa to na wszystkie języki);
3. Przetestuj lokalnie, aby potwierdzić rezultaty;
4. Prześlij PR.

---

## Dostarczanie ręcznie poprawionych korpusów

Jeśli jesteś autorem moda tłumaczeniowego i jesteś gotów udostępnić swój korpus tłumaczeniowy jako referencję dla LLM, złóż wniosek poprzez Issue. Musisz podać następujące informacje:

- Mod ID Twojego moda tłumaczeniowego oraz język docelowy;
- Zrzut ekranu strony zarządzania Twoim modem tłumaczeniowym jako dowód autorstwa;
- Jasne oświadczenie w Issue, że jesteś gotów udostępnić korpus tłumaczeniowy;
- Jeśli istnieją szczególne okoliczności (specjalna licencja itp.), proszę je wyjaśnić;
- Upewnij się, że dostarczony korpus jest wysokiej jakości.

Za Twoją zgodą projekt doda Twój mod do listy referencyjnych modów tłumaczeniowych `config/ref_translation_mods.json`, a potok automatycznie zsynchronizuje Twoje przetłumaczone teksty jako korpusy referencyjne RAG.

---

## Wkład w rozwój potoku i narzędzi

Automatyzacja w tym projekcie dzieli się na dwie części:

**Moduły potoku (`src/`, C# / .NET 10)**: Zawiera 15 sekwencyjnie wykonywanych modułów, odpowiedzialnych za kompletny proces od pobierania modów, ekstrakcji tekstu, przeglądu treści, obliczania embeddingów, wyszukiwania RAG po tłumaczenie LLM i końcowe wyjście. Szczegóły w [dokumentacji technicznej](../technical_reference/technical_reference_pl.md).

**Skrypty pomocnicze (`.github/`)**: Używane do automatyzacji GitHub.

Jeśli chcesz:

* Naprawić błędy w istniejących modułach potoku lub skryptach;
* Dodać nowe funkcje lub moduły do potoku;
* Zoptymalizować wydajność lub strukturę kodu;
* Ulepszyć szablony promptów lub strategie RAG;

Możesz postępować zgodnie z poniższymi krokami:

1. Zrób forka tego repozytorium i sklonuj je lokalnie;
2. Utwórz nową gałąź z najnowszej gałęzi;
3. Zmodyfikuj lub dodaj pliki w odpowiednich katalogach:
   - Zmiany w modułach potoku → `src/<nazwa_modułu>/`;
   - Zmiany w skryptach → `scripts/`;
   - Zmiany w szablonach promptów → `src/prompt_templates/`;
4. Przed przesłaniem postaraj się:

   * Zachować istniejący styl kodu;
   * Dodać niezbędne komentarze;
   * Jeśli to możliwe, dołączyć proste testy lub instrukcje użytkowania;
5. Prześlij zmiany przez PR, wyjaśniając w opisie:

   * Cel zmian;
   * Katalogi / moduły / skrypty, których mogą dotyczyć zmiany;
   * Czy zmiany powodują utratę kompatybilności.

---

## Prawa autorskie i licencje

> **Przyjazne przypomnienie:**
> Warunki praw autorskich i licencji mają na celu ochronę uzasadnionych praw i interesów projektu, autorów, współtwórców oraz graczy, a także uniknięcie nieporozumień wynikających z „milczących porozumień" lub „domyślnych założeń". Prosimy o uważne przeczytanie.
> Prawa autorskie i licencje są regulowane treścią pliku README.md; ta sekcja zawiera jedynie bardziej przystępny opis.

### 1. Podstawowa zasada: Zachowujesz prawa autorskie, jednocześnie udzielając projektowi licencji na korzystanie

* Nadal posiadasz prawa autorskie do tworzonych przez siebie treści (tłumaczenia, obrazy, skrypty/programy itp.);
* Jednak po przesłaniu tych treści do tego projektu i ich przyjęciu (scaleniu),
  zgadzasz się udzielić innym licencji na korzystanie z tych treści na warunkach licencji open-source/współdzielonej przyjętej przez ten projekt.

Oznacza to:

* **Nadal możesz** używać i prezentować swoją pracę gdzie indziej;
* Ale **nie możesz** po scaleniu swojego wkładu żądać od tego projektu lub innych użytkowników, którzy legalnie uzyskali dzieło, „cofnięcia licencji" lub „usunięcia wersji historycznych".

### 2. Licencjonowanie tekstów, obrazów i podobnych treści (CC BY-NC-SA 4.0)

W odniesieniu do następujących treści, które przesyłasz:

* Tłumaczenia tekstów gier, poprawki stylistyczne i korekty;
* Dokumentacja projektu i teksty objaśniające;
* Obrazy i zasoby artystyczne stworzone specjalnie na potrzeby tego projektu;

Po przyjęciu i scaleniu w tym repozytorium uznaje się, że zgadzasz się na:

1. Licencjonowanie tych treści na warunkach **Uznanie autorstwa-Użycie niekomercyjne-Na tych samych warunkach 4.0 Międzynarodowe**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, w skrócie **CC BY-NC-SA 4.0**);
2. Project Babel i wszyscy użytkownicy otrzymujący te treści mogą, **przestrzegając warunków CC BY-NC-SA 4.0**:

   * Udostępniać, kopiować i redystrybuować te treści;
   * Modyfikować je i tworzyć utwory zależne w celach niekomercyjnych;
3. Wyrażasz zgodę, że w zakresie dozwolonym przez obowiązujące prawo, niniejsza licencja jest **niewyłączna, ogólnoświatowa, bezpłatna i nieodwołalna**;
4. Nawet jeśli później wycofasz się lub przestaniesz uczestniczyć w tym projekcie, projekt może nadal korzystać i redystrybuować odpowiednie treści, które przesłałeś i które zostały scalone, zgodnie z CC BY-NC-SA 4.0.

> Jeśli nie akceptujesz powyższych warunków licencji, nie przesyłaj wkładów tekstowych ani graficznych do tego projektu,
> lub skontaktuj się wcześniej z opiekunami projektu, aby potwierdzić, czy współpraca jest możliwa na innych zasadach.

### 3. Licencjonowanie skryptów i kodu narzędzi (GPL-3.0)

W odniesieniu do następujących elementów, które przesyłasz i które zostają przyjęte:

* Skrypty automatyzacji;
* Narzędzia do budowania/eksportu;
* Inny kod programu używany do przetwarzania tego projektu tłumaczeniowego;

W przypadku braku specjalnych deklaracji uznaje się, że zgadzasz się na:

1. Licencjonowanie kodu na warunkach **GPL-3.0** (GNU General Public License wersja 3);
2. Opiekunowie projektu mogą go modyfikować, scalać i rozpowszechniać w zakresie dozwolonym przez GPL-3.0;
3. Możesz również kontynuować inne projekty oparte na tym samym kodzie, o ile przestrzegasz warunków GPL-3.0.

Aby uniknąć konfliktów licencyjnych, postaraj się:

* Nie wprowadzać kodu stron trzecich **niezgodnego z GPL-3.0** bez uprzedniego potwierdzenia;
* Jeśli musisz odwołać się do bibliotek stron trzecich, wyraźnie wskaż ich źródło i licencję w PR oraz potwierdź zgodność.

### 4. Dzieła nadrzędne i prawa autorskie do oryginalnej gry

Ten projekt jest projektem **nieoficjalnego tłumaczenia** modów związanych z *Project Zomboid*:

* Prawa autorskie do oryginalnej gry i każdego moda należą do ich odpowiednich autorów/wydawców;
* Ten projekt obejmuje wyłącznie tworzenie i organizację tłumaczeń tekstowych, korekt stylistycznych oraz niektórych zasobów towarzyszących;
* Współtwórcy, przesyłając treści, powinni upewnić się, że:

  * Nie kopiują bezpośrednio nieautoryzowanych tłumaczeń tekstów lub zasobów artystycznych osób trzecich;
  * Szanują prawa oryginalnych autorów i autorów modów oraz nie dokonują redystrybucji naruszającej prawa.

---

## Komunikacja i współpraca

Jeśli masz:

* Pytania dotyczące warunków licencji;
* Wątpliwości, czy określone treści mogą być przedmiotem wkładu;
* Chęć licencjonowania swojej pracy w specjalny sposób (np. tylko użycie niekomercyjne, ale bez prawa do adaptacji);

Skontaktuj się z opiekunami projektu poprzez:

* Zgłoszenie Issue do dyskusji;
* Inne publicznie dostępne metody kontaktu opiekunów.

Dołożymy wszelkich starań, aby znaleźć rozwiązanie, które równoważy zdrowy rozwój projektu z poszanowaniem praw i interesów wszystkich stron.

---

## Wsparcie finansowe

W trakcie działania projektu, ze względu na dodawanie nowych modów i aktualizacje tekstów istniejących modów, konieczne jest ciągłe wywoływanie API LLM w celu tłumaczenia. Aby ograniczyć zachowanie LLM, oprócz podstawowych tekstów modów potrzebna jest duża ilość treści promptów (w tym podstawowe prompty, reguły tłumaczenia, tabele terminologiczne, ograniczenia wejścia/wyjścia, wyniki wyszukiwania semantycznego itp.), co zużywa znacznie więcej tokenów niż oryginalne teksty. Dlatego projekt potrzebuje wsparcia finansowego.

Jeśli chcesz zapewnić wsparcie finansowe, skontaktuj się z opiekunami projektu. Dziękujemy bardzo!

---

Jeszcze raz dziękujemy za chęć wniesienia wkładu w ten projekt!
Każdy Twój wkład przynosi korzyści większej liczbie graczy!
