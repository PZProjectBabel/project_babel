# Průvodce přispíváním (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Děkujeme za vaši ochotu přispět k **Project Babel — projektu automatického překladu módů Project Zomboid pomocí LLM**! Ať už jde o opravu chyby, přidání funkce, psaní šablon promptů nebo poskytování referenčních překladů — každý příspěvek se počítá!

Volání LLM API pro překlad stojí tokeny. Aby projekt mohl dlouhodobě fungovat, je vaše štědrá podpora velmi vítána!

> ⚠️ **Důležité upozornění:**
> Před odesláním čehokoliv do tohoto repozitáře si prosím přečtěte a pochopte sekci „Autorská práva a licence".
> Odesláním a sloučením se má za to, že souhlasíte s odpovídajícími licenčními podmínkami.

---

## Než začnete

Přečtěte si prosím `README.md` projektu, abyste pochopili:

- Celkové cíle a aktuální stav tohoto projektu;
- Jak běžní hráči používají tento projekt (pro vaše vlastní testy);
- Technické detaily projektu.

---

## Jak mohu přispět?

Můžete si vybrat jeden nebo více způsobů účasti podle svých zájmů a dovedností:

- Poskytnout překladová pravidla pro cílový jazyk
- Poskytnout terminologický slovník pro cílový jazyk
- Vylepšit systémové prompty
- Poskytnout ručně korigované překladové korpusy
- Vylepšit moduly pipeline (.NET) a automatizační skripty
- Hlásit problémy a navrhovat vylepšení (prostřednictvím Issues)
- Poskytnout finanční podporu pro volání LLM API

Níže jsou vysvětleny hlavní scénáře přispívání.

---

## Poskytování překladových pravidel, terminologických slovníků a vylepšování systémových promptů

Šablony promptů pipeline se nacházejí v `src/prompt_templates/` s následující strukturou:

- `system_prompt_translate_engine.txt`: globální systémový prompt překladového enginu (sdílený všemi jazyky);
- `<kód_jazyka>/translation_dictionary_<kód_jazyka>.json`: terminologický slovník pro daný jazyk;
- `<kód_jazyka>/translation_schema_<kód_jazyka>.md`: překladová pravidla a stylová omezení pro daný jazyk.

Kroky přispění:

1. Vytvořte podadresář v `src/prompt_templates/` pro váš jazyk a přidejte soubory slovníku a pravidel;
2. Pokud potřebujete upravit globální chování překladu, upravte `system_prompt_translate_engine.txt` (pozor: ovlivňuje všechny jazyky);
3. Otestujte lokálně a potvrďte výsledky;
4. Odešlete PR.

---

## Poskytování ručně korigovaných korpusů

Pokud jste autorem překladového módu a jste ochotni poskytnout svůj překladový korpus jako referenci pro LLM, zašlete žádost prostřednictvím Issue. Musíte poskytnout následující informace:

- Mod ID vašeho překladového módu a cílový jazyk;
- Snímek obrazovky administrační stránky vašeho módu jako důkaz autorství;
- Jasné prohlášení v Issue, že jste ochotni poskytnout překladový korpus;
- Pokud existují zvláštní okolnosti (zvláštní licence atd.), vysvětlete je;
- Ujistěte se, že poskytnutý korpus je vysoce kvalitní.

S vaším svolením projekt přidá váš mód do seznamu referenčních překladových módů `config/ref_translation_mods.json` a pipeline automaticky synchronizuje vaše přeložené texty jako referenční korpusy RAG.

---

## Příspěvky k vývoji pipeline a nástrojů

Automatizace v tomto projektu je rozdělena do dvou částí:

**Moduly pipeline (`src/`, C# / .NET 10)**: Obsahuje 15 sekvenčně prováděných modulů odpovědných za kompletní pracovní postup od stahování módů, extrakce textu, kontroly obsahu, výpočtu embeddingů, vyhledávání RAG až po překlad LLM a finální výstup. Podrobnosti viz [technická reference](../technical_reference/technical_reference_cs.md).

**Pomocné skripty (`.github/`)**: Používají se pro automatizaci GitHubu.

Pokud si přejete:

* Opravit chyby ve stávajících modulech pipeline nebo skriptech;
* Přidat nové funkce nebo moduly do pipeline;
* Optimalizovat výkon nebo strukturu kódu;
* Vylepšit šablony promptů nebo strategie RAG;

Můžete postupovat podle těchto kroků:

1. Forkněte tento repozitář a naklonujte ho lokálně;
2. Vytvořte novou větev z nejnovější větve;
3. Upravte nebo přidejte soubory v odpovídajících adresářích:
   - Změny modulů pipeline → `src/<název_modulu>/`;
   - Změny skriptů → `scripts/`;
   - Změny šablon promptů → `src/prompt_templates/`;
4. Před odesláním se pokuste:

   * Zachovat stávající styl kódu;
   * Přidat potřebné komentáře;
   * Pokud možno, přiložit jednoduché testy nebo návod k použití;
5. Odešlete změny prostřednictvím PR a v popisu uveďte:

   * Účel změn;
   * Adresáře / moduly / skripty, které mohou být ovlivněny;
   * Zda se jedná o změny porušující kompatibilitu.

---

## Autorská práva a licence

> **Přátelské připomenutí:**
> Podmínky autorských práv a licencí jsou navrženy k ochraně oprávněných práv a zájmů projektu, autorů, přispěvatelů a hráčů a k zamezení nedorozumění vyplývajících z „tichých dohod" nebo „výchozích předpokladů". Přečtěte si je prosím pečlivě.
> Autorská práva a licence se řídí obsahem souboru README.md; tato sekce poskytuje pouze přístupnější popis.

### 1. Základní princip: Ponecháváte si autorská práva a zároveň licencujete projekt k použití vašeho díla

* Stále vlastníte autorská práva k obsahu, který vytváříte (překlady, obrázky, skripty/programy atd.);
* Avšak jakmile je tento obsah odeslán do tohoto projektu a přijat (sloučen),
  souhlasíte s licencováním ostatním k použití tohoto obsahu pod open-source/sdílenou licencí přijatou tímto projektem.

To znamená:

* **Stále můžete** pokračovat v používání a zobrazování svého díla jinde;
* Ale **nemůžete** po sloučení svého příspěvku požadovat, aby tento projekt nebo jiní uživatelé, kteří dílo legálně získali, „odvolali licenci" nebo „smazali historické verze".

### 2. Licence textů, obrázků a podobného obsahu (CC BY-NC-SA 4.0)

Pro následující obsah, který odešlete:

* Překlady herních textů, stylistické úpravy a korektury;
* Projektová dokumentace a vysvětlující texty;
* Obrázky a umělecké zdroje vytvořené speciálně pro tento projekt;

Po přijetí a sloučení do tohoto repozitáře se má za to, že souhlasíte s tím, že:

1. Tento obsah je licencován pod **Uveďte původ-Neužívejte komerčně-Zachovejte licenci 4.0 Mezinárodní**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, zkráceně **CC BY-NC-SA 4.0**);
2. Project Babel a všichni uživatelé, kteří obdrží tento obsah, mohou při **dodržení podmínek CC BY-NC-SA 4.0**:

   * Sdílet, kopírovat a redistribuovat tento obsah;
   * Upravovat jej a vytvářet odvozená díla pro nekomerční účely;
3. Souhlasíte, že v rozsahu povoleném platnými zákony je tato licence **nevýhradní, celosvětová, bezplatná a neodvolatelná**;
4. I když později odstoupíte nebo přestanete se účastnit tohoto projektu, projekt může nadále používat a redistribuovat příslušný obsah, který jste odeslali a který byl sloučen, podle CC BY-NC-SA 4.0.

> Pokud nesouhlasíte s výše uvedenými licenčními podmínkami, neposílejte prosím textové nebo obrazové příspěvky do tohoto projektu,
> nebo se předem spojte se správci projektu a ověřte, zda je možná spolupráce jiným způsobem.

### 3. Licence skriptů a kódu nástrojů (GPL-3.0)

Pro následující, co odešlete a co je přijato:

* Automatizační skripty;
* Nástroje pro sestavení/export;
* Jiný programový kód používaný ke zpracování tohoto překladového projektu;

Při absenci zvláštních prohlášení se má za to, že souhlasíte s tím, že:

1. Kód je licencován pod **GPL-3.0** (GNU General Public License verze 3);
2. Správci projektu jej mohou upravovat, slučovat a distribuovat v rozsahu povoleném GPL-3.0;
3. Můžete také pokračovat v dalších projektech založených na stejném kódu, pokud dodržíte podmínky GPL-3.0.

Aby se předešlo licenčním konfliktům, snažte se:

* Nezavádět kód třetích stran **nekompatibilní s GPL-3.0** bez předchozího ověření;
* Pokud potřebujete odkazovat na knihovny třetích stran, jasně uveďte jejich zdroj a licenci v PR a potvrďte kompatibilitu.

### 4. Původní díla a autorská práva k původní hře

Tento projekt je projektem **neoficiálního překladu** módů souvisejících s *Project Zomboid*:

* Autorská práva k původní hře a každému módu patří jejich příslušným autorům/vydavatelům;
* Tento projekt zahrnuje pouze vytváření a organizaci textových překladů, stylistických úprav a některých doprovodných zdrojů;
* Přispěvatelé by při odesílání obsahu měli zajistit:

  * Nepřímo nekopírovat neautorizované překladové texty nebo umělecké zdroje třetích stran;
  * Respektovat práva původních autorů a autorů módů a neprovádět redistribuci porušující práva.

---

## Komunikace a spolupráce

Pokud máte:

* Dotazy k licenčním podmínkám;
* Nejasnosti, zda lze určitý obsah přispět;
* Přání licencovat své dílo zvláštním způsobem (např. pouze nekomerční použití bez povolené úpravy);

Neváhejte kontaktovat správce projektu prostřednictvím:

* Odeslání Issue k diskusi;
* Jiných veřejně dostupných kontaktních metod správců.

Uděláme vše pro to, abychom našli řešení, které vyváží zdravý vývoj projektu s respektováním práv a zájmů všech zúčastněných stran.

---

## Finanční podpora

Během provozu projektu je kvůli přidávání nových módů a aktualizacím textů stávajících módů nutné neustále volat LLM API pro překlad. K omezení chování LLM je kromě základních textů módů zapotřebí velké množství obsahu promptů (včetně základních promptů, překladových pravidel, terminologických tabulek, omezení vstupu/výstupu, výsledků sémantického vyhledávání atd.), což spotřebovává mnohem více tokenů než původní texty. Proto projekt potřebuje finanční podporu.

Pokud chcete poskytnout finanční podporu, kontaktujte prosím správce projektu. Děkujeme!

---

Ještě jednou děkujeme za vaši ochotu přispět k tomuto projektu!
Každý váš příspěvek prospívá více hráčům!
