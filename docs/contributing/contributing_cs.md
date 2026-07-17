# Průvodce přispíváním (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Obsah

- [1. Než začnete](#1-než-začnete)
- [2. Jak mohu přispět?](#2-jak-mohu-přispět)
- [3. Poskytnutí pravidel překladu, slovníku termínů, vylepšení systémových promptů](#3-poskytnutí-pravidel-překladu-slovníku-termínů-vylepšení-systémových-promptů)
- [4. Poskytnutí ručně zkontrolovaných dat](#4-poskytnutí-ručně-zkontrolovaných-dat)
- [5. Příspěvky k pipeline a vývoji nástrojů](#5-příspěvky-k-pipeline-a-vývoji-nástrojů)
- [6. Autorská práva a licenční ujednání](#6-autorská-práva-a-licenční-ujednání)
  - [6.1 Základní princip: Vy si ponecháváte autorská práva a zároveň udělujete projektu licenci k použití](#61-základní-princip-vy-si-ponecháváte-autorská-práva-a-zároveň-udělujete-projektu-licenci-k-použití)
  - [6.2 Licence pro texty, obrázky a další obsah (CC BY-NC-SA 4.0)](#62-licence-pro-texty-obrázky-a-další-obsah-cc-by-nc-sa-40)
  - [6.3 Licence pro skripty a nástrojový kód (GPL-3.0)](#63-licence-pro-skripty-a-nástrojový-kód-gpl-30)
  - [6.4 Autorská práva k upstream dílům a původní hře](#64-autorská-práva-k-upstream-dílům-a-původní-hře)
- [7. Komunikace a spolupráce](#7-komunikace-a-spolupráce)
- [8. Finanční podpora](#8-finanční-podpora)

---

Velmi děkujeme, že jste ochoten přispět do projektu **Project Babel - 《僵尸毁灭工程》模组LLM自动翻译项目**! Ať už opravujete chybu, přidáváte novou funkci, píšete šablonu promptu nebo poskytujete referenční překlad!

Volání LLM API pro překlad vyžaduje platbu za tokeny. Aby projekt mohl dlouhodobě stabilně fungovat, doufáme, že budete velkorysí!

> ⚠️ **Důležité upozornění:**
> Před odesláním čehokoli do tohoto repozitáře si prosím přečtěte a pochopte část „Dohoda o autorských právech a licencích“.
> Jakmile je příspěvek odeslán a sloučen, považuje se to za váš souhlas s příslušnými licenčními podmínkami.

---

## 1. Než začnete

Nejprve si přečtěte projektový `README.md`, abyste se dozvěděli:
- Celkový cíl a aktuální stav projektu;
- Jak běžní hráči mohou používat tento projekt (pro snadné vlastní testování);
- Technické podrobnosti projektu.

---

## 2. Jak mohu přispět?

Můžete si vybrat jeden nebo více způsobů účasti podle svých zájmů a dovedností:

- Poskytnout pravidla překladu pro cílový jazyk
- Poskytnout slovník překladových termínů pro cílový jazyk
- Vylepšit systémové prompty
- Poskytnout ručně zkontrolované překladové textové korpusy
- Vylepšit moduly pipeline (.NET) a automatizační skripty
- Nahlásit problémy, navrhnout vylepšení (v Issues)
- Poskytnout finanční podporu na volání LLM

Níže uvádíme některé vysvětlení k hlavním scénářům přispívání.

---

## 3. Poskytnutí pravidel překladu, slovníku termínů, vylepšení systémových promptů

Šablony promptů pipeline jsou umístěny v `src/prompt_templates/` a mají následující strukturu:

- `system_prompt_translate_engine.txt`: Globální systémový prompt překladového enginu (sdílený všemi jazyky);
- `<kód jazyka>/translation_dictionary_<kód jazyka>.json`: Slovník termínů pro daný jazyk;
- `<kód jazyka>/translation_schema_<kód jazyka>.md`: Pravidla překladu a stylové omezení pro daný jazyk.

Kroky pro přispění:

1. Vytvořte podadresář pro svůj jazyk v `src/prompt_templates/` a přidejte soubory slovníků termínů a pravidel překladu;
2. Pokud potřebujete upravit globální chování překladu, upravte `system_prompt_translate_engine.txt` (pozor, ovlivňuje všechny jazyky);
3. Lokálně otestujte a potvrďte účinnost;
4. Odešlete PR.

---

## 4. Poskytnutí ručně zkontrolovaných dat

Pokud jste tvůrcem překladového modu a chcete poskytnout svá překladová data jako referenci pro LLM překlad, podejte žádost v Issue. Budete potřebovat poskytnout následující informace:

- ID vašeho překladového modu a cílový jazyk překladu;
- Snímek obrazovky administrační stránky vašeho překladového modu, který dokáže, že jste autorem modu;
- V Issue jasně uveďte, že souhlasíte s poskytnutím překladových dat;
- Pokud existují zvláštní okolnosti (speciální licence atd.), uveďte je;
- Ujistěte se, že poskytnutá data mají vysokou kvalitu.

S vaším svolením projekt zařadí váš mod do seznamu referenčních překladových modů v `config/ref_translation_mods.json` a pipeline automaticky synchronizuje vaše překladové texty jako referenční data RAG.

---

## 5. Příspěvky k pipeline a vývoji nástrojů

Automatizace tohoto projektu je rozdělena na dvě části:

**Moduly pipeline (`src/`, C# / .NET 10)**: Obsahuje 15 modulů prováděných v pořadí, které zajišťují celý proces od inicializace SteamCMD, stahování modů, extrakce textu, kontroly obsahu, výpočtu Embeddingů, vyhledávání RAG až po LLM překlad a konečný výstup. Podrobnosti viz [technická reference](../technical_reference/technical_reference_cs.md).

**Pomocné skripty (`.github/`)**: Používané pro automatizaci na GitHubu.

Pokud si přejete:

* Opravit chyby ve stávajících modulech pipeline nebo skriptech;
* Přidat nové funkce nebo moduly do pipeline;
* Optimalizovat výkon nebo strukturu kódu;
* Vylepšit šablony promptů nebo strategii RAG;

Můžete postupovat následovně:

1. Forkněte toto úložiště a naklonujte si ho lokálně;
2. Vytvořte novou větev z nejnovější větve;
3. Upravte nebo přidejte soubory v odpovídajících adresářích:
- Úprava modulu pipeline → `src/<název_modulu>/`;
- Úprava skriptů → `scripts/`;
- Úprava šablon promptů → `src/prompt_templates/`;
4. Před odesláním se pokuste:

* Zachovat původní styl kódu;
* Přidat potřebné komentáře;
* Pokud je to možné, přiložte jednoduchý test nebo návod k použití;
5. Odešlete úpravy prostřednictvím PR a v popisu uveďte:

* Účel změny;
* Ovlivněné adresáře / moduly / skripty;
* Zda se jedná o breaking change.

---

## 6. Autorská práva a licenční ujednání

> **Poznámka:**
> Licenční ujednání slouží k ochraně práv projektu, autorů, přispěvatelů a hráčů, aby se předešlo nedorozuměním kvůli „tichému souhlasu“ nebo „výchozím předpokladům“. Přečtěte si prosím pečlivě.
> Autorská práva a licence se řídí obsahem souboru README.md; tato část poskytuje pouze srozumitelnější popis.

### 6.1 Základní princip: Vy si ponecháváte autorská práva a zároveň udělujete projektu licenci k použití

* Stále si zachováváte autorská práva k obsahu, který jste vytvořili (překlady, obrázky, skripty/programy atd.);
* Po odeslání tohoto obsahu do tohoto projektu a jeho přijetí (sloučení) souhlasíte s tím, že udělíte ostatním licenci k použití tohoto obsahu v souladu s licenčními podmínkami open source/sdílení používanými tímto projektem.

To znamená:

* Svá díla **můžete** nadále používat a vystavovat i jinde;
* **Nemůžete** však požadovat, aby projekt nebo jiní uživatelé, kteří dílo legálně získali, „odvolali licenci“ nebo „odstranili historické verze“ poté, co byl váš příspěvek sloučen.

### 6.2 Licence pro texty, obrázky a další obsah (CC BY-NC-SA 4.0)

Pro následující obsah, který předložíte:

* Překlady herních textů, úpravy a korektury;
* Projektová dokumentace, vysvětlující texty;
* Obrázky a grafické zdroje vytvořené speciálně pro tento projekt;

Jakmile jsou přijaty a sloučeny do tohoto repozitáře, považuje se to za váš souhlas:

1. Tento obsah je licencován pod licencí **Uveďte autora-Neužívejte komerčně-Zachovejte licenci 4.0 Mezinárodní** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, zkráceně **CC BY-NC-SA 4.0**);
2. Project Babel a všichni uživatelé, kteří tento obsah obdrželi, mohou za předpokladu **dodržování podmínek CC BY-NC-SA 4.0**:
* Sdílet, kopírovat a dále distribuovat tento obsah;
* Upravovat a přetvářet jej pro nekomerční účely;
3. Souhlasíte, že v rozsahu povoleném platnými právními předpisy je tato licence **neexkluzivní, celosvětová, bez licenčních poplatků a neodvolatelná**;
4. I když později opustíte nebo přestanete přispívat do tohoto projektu, projekt může nadále používat a znovu šířit vámi předložený a sloučený obsah na základě CC BY-NC-SA 4.0.

> Pokud s výše uvedeným licenčním modelem nesouhlasíte, nepředkládejte tomuto projektu textové ani obrazové příspěvky,
> nebo předem komunikujte se správcem projektu a zjistěte, zda je možná spolupráce jiným způsobem.

### 6.3 Licence pro skripty a nástrojový kód (GPL-3.0)

Pro to, co předložíte a bude přijato:

* Automatizační skripty;
* Nástroje pro sestavení/export;
* Další programový kód pro zpracování tohoto lokalizačního projektu;

Pokud není uvedeno jinak, považuje se za váš souhlas:

1. Kód je licencován pod **GPL-3.0** (GNU General Public License verze 3);
2. Správci projektu mohou v rámci povoleném GPL-3.0 kód upravovat, slučovat a distribuovat;
3. Vy také můžete na základě stejného kódu provozovat další projekty, pokud dodržíte podmínky GPL-3.0.

Abyste předešli konfliktům licencí, snažte se co nejvíce:

* Nezavádějte bez ověření kód třetích stran, který **není kompatibilní s GPL-3.0**;
* Pokud je nutné použít knihovnu třetí strany, jasně uveďte v PR její zdroj a licenci a potvrďte kompatibilitu.

### 6.4 Autorská práva k upstream dílům a původní hře

Tento projekt je **neoficiálním překladem** modů pro hru Project Zomboid:

* Autorská práva k původní hře a jednotlivým modům náleží jejich autorům/vydavatelům;
* Tento projekt se zaměřuje pouze na překlad textu, úpravy a zpracování některých doprovodných zdrojů;
* Přispěvatelé by při předkládání obsahu měli zajistit:
* Nekopírovat přímo neautorizované překlady nebo grafické zdroje třetích stran;
* Respektovat práva původních autorů a autorů modů, neporušovat autorská práva.

---

## 7. Komunikace a spolupráce

Pokud máte:

* Otázky ohledně licenčních podmínek;
* Nejste si jisti, zda lze nějaký obsah přispět;
* Chcete svá díla licencovat zvláštním způsobem (např. pouze nekomerčně, bez možnosti úprav apod.);

Kontaktujte správce projektu prostřednictvím:

* Vytvoření Issue k diskusi;
* Jiných veřejně dostupných kontaktů správců.

Pokusíme se najít řešení, které respektuje práva všech stran a zároveň podporuje zdravý rozvoj projektu.

---

## 8. Finanční podpora

Během provozu projektu je kvůli přidávání nových modů a aktualizaci textů starých modů nutné průběžně volat LLM API pro překlad. K omezení chování LLM je kromě základního textu modů potřeba poskytovat velké množství promptů (základní prompt, pravidla překladu, glosář, omezení vstupu/výstupu, výsledky sémantického vyhledávání atd.), což spotřebovává mnohem více tokenů než samotný zdrojový text. Projekt proto potřebuje finanční podporu.

Pokud jste ochotni poskytnout finanční podporu, kontaktujte správce projektu. Mnohokrát děkujeme!

---

Ještě jednou děkujeme, že jste ochotni přispět tomuto projektu!
Tvůj každý příspěvek pomůže více hráčům!
