# Bijdragengids (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Inhoudsopgave

- [1. Voordat je begint](#1-voordat-je-begint)
- [2. Hoe kan ik bijdragen?](#2-hoe-kan-ik-bijdragen)
- [3. Vertaalregels, terminologiewoordenboek en systeemprompts verbeteren](#3-vertaalregels-terminologiewoordenboek-en-systeemprompts-verbeteren)
- [4. Handmatige proefleescorpora verstrekken](#4-handmatige-proefleescorpora-verstrekken)
- [5. Bijdragen aan pijplijn- en toolontwikkeling](#5-bijdragen-aan-pijplijn--en-toolontwikkeling)
- [6. Auteursrecht en licentievoorwaarden](#6-auteursrecht-en-licentievoorwaarden)
  - [6.1 Basisprincipe: u behoudt auteursrecht en verleent tegelijkertijd het project een licentie](#61-basisprincipe-u-behoudt-auteursrecht-en-verleent-tegelijkertijd-het-project-een-licentie)
  - [6.2 Licentie voor tekst en afbeeldingen (CC BY-NC-SA 4.0)](#62-licentie-voor-tekst-en-afbeeldingen-cc-by-nc-sa-40)
  - [6.3 Licentie voor scripts en toolcode (GPL-3.0)](#63-licentie-voor-scripts-en-toolcode-gpl-30)
  - [6.4 Bovenliggende werken en originele spelrechten](#64-bovenliggende-werken-en-originele-spelrechten)
- [7. Communicatie en samenwerking](#7-communicatie-en-samenwerking)
- [8. Financiële ondersteuning](#8-financiële-ondersteuning)

---

Hartelijk dank dat je wilt bijdragen aan **Project Babel - 《僵尸毁灭工程》模组LLM自动翻译项目**! Of het nu gaat om het corrigeren van een fout, het toevoegen van een functie, het schrijven van een prompt-sjabloon, of het leveren van referentievertalingen!

Het aanroepen van de LLM API voor vertalingen kost tokens. Voor de lange termijn stabiliteit van het project hopen we dat je gul kunt bijdragen!

> ⚠️ **Belangrijke herinnering:**
> Voordat je iets naar deze repository indient, lees en begrijp goed de sectie "Copyright en Licentieovereenkomst".
> Zodra je inzending is ingediend en samengevoegd, word je geacht in te stemmen met de bijbehorende licentievoorwaarden.

---

## 1. Voordat je begint

Lees eerst het project `README.md` om het volgende te weten:
- Het algemene doel en de huidige status van dit project;
- Hoe gewone spelers dit project kunnen gebruiken (handig voor je eigen testen);
- Technische details van het project.

---

## 2. Hoe kan ik bijdragen?

Je kunt op basis van je interesses en vaardigheden een of meer manieren kiezen om deel te nemen:

- Vertaalregels voor de doeltaal leveren
- Vertaalterminologiewoordenboek voor de doeltaal leveren
- De prompts van het systeem verbeteren
- Handmatig nagekeken vertaalcorpora leveren
- De pijplijnmodules (.NET) en automatisatie scripts verbeteren
- Problemen melden en verbetervoorstellen doen (in Issues)
- Financiële steun voor LLM API-aanroepen

Hieronder volgt een toelichting op de belangrijkste bijdrages.

---

## 3. Vertaalregels, terminologiewoordenboek en systeemprompts verbeteren

De prompt-sjablonen van de pijplijn bevinden zich in `src/prompt_templates/` met de volgende structuur:

- `system_prompt_translate_engine.txt`: Globale systeemprompt voor de vertaalengine (gedeeld door alle talen);
- `<taalcode>/translation_dictionary_<taalcode>.json`: Het terminologiewoordenboek voor die taal;
- `<taalcode>/translation_schema_<taalcode>.md`: De vertaalregels en stijlbeperkingen voor die taal.

Stappen om bij te dragen:

1. Maak een submap voor jouw taal in `src/prompt_templates/` en voeg het terminologiewoordenboek en vertaalregelbestand toe;
2. Indien nodig het globale vertaalgedrag aanpassen, wijzig `system_prompt_translate_engine.txt` (let op: beïnvloedt alle talen);
3. Bevestig het effect met lokale tests.
4. Dien een PR in.

---

## 4. Handmatige proefleescorpora verstrekken

Als je een vertaalmod-maker bent en je vertaalcorpora als referentie voor LLM-vertalingen wilt aanbieden, dien dan een verzoek in via een Issue. Je dient de volgende informatie te verstrekken:

- De Mod ID van jouw vertaalmod en de doeltaal van de vertaling;
- Een screenshot van de beheerpagina van jouw vertaalmod om te bewijzen dat je de mod-auteur bent;
- Geef in de Issue duidelijk aan dat je bereid bent vertaalcorpora te verstrekken;
- Indien er bijzondere omstandigheden zijn (speciale licenties enz.), vermeld deze dan;
- Zorg ervoor dat de door jou verstrekte corpora van hoge kwaliteit zijn.

Onder jouw toestemming zal het project jouw mod opnemen in de referentielijst van vertaalmods in `config/ref_translation_mods.json`, en de pijplijn zal automatisch jouw vertaaltekst synchroniseren als RAG-referentiecorpora.

---

## 5. Bijdragen aan pijplijn- en toolontwikkeling

De automatisering van dit project is verdeeld in twee delen:

**Pijplijnmodule (`src/`, C# / .NET 10)**: Bevat 15 opeenvolgend uitgevoerde modules, verantwoordelijk voor de volledige stroom van SteamCMD-initialisatie, mod-download, tekstextractie, inhoudscontrole, Embedding-berekening, RAG-ophaling tot LLM-vertaling en uiteindelijke uitvoer. Zie [technische referentie](../technical_reference/technical_reference_nl.md) voor details.

**Hulpscripts (`.github/`)**: Voor de automatisering van GitHub.

Als je wilt:

* Bugs in bestaande pijplijnmodules of scripts repareren;
* Nieuwe functies of modules aan de pijplijn toevoegen;
* Prestaties of codestructuur optimaliseren;
* Prompt-sjablonen of RAG-strategieën verbeteren;

Volg dan de onderstaande stappen:

1. Fork deze repository en clone deze lokaal;
2. Maak een nieuwe branch op basis van de nieuwste branch;
3. Wijzig of voeg bestanden toe in de corresponderende mappen:
- Wijzigingen aan pijplijnmodule → `src/<模块名>/`;
- Scriptaanpassingen → `scripts/`;
- Prompt-sjabloonaanpassingen → `src/prompt_templates/`;
4. Probeer vóór het indienen zoveel mogelijk:

* Houd de oorspronkelijke codestijl aan;
* Voeg de nodige opmerkingen toe;
* Voeg indien mogelijk eenvoudige test- of gebruiksaanwijzingen toe;
5. Dien wijzigingen in via een PR en vermeld in de beschrijving:

* Doel van de wijziging;
* Mogelijk beïnvloede mappen / modules / scripts;
* Of het een breekbare wijziging betreft.

---

## 6. Auteursrecht en licentievoorwaarden

> **Let op:**
> De auteursrecht- en licentievoorwaarden zijn bedoeld om de wettelijke rechten van het project, auteurs, bijdragers en spelers te beschermen, en om misverstanden als gevolg van 'stilzwijgend akkoord' of 'standaard' te voorkomen. Lees het alstublieft zorgvuldig.
> De auteursrecht- en licentievoorwaarden worden bepaald door de inhoud van het README.md-bestand; deze sectie biedt alleen een eenvoudigere uitleg.

### 6.1 Basisprincipe: u behoudt auteursrecht en verleent tegelijkertijd het project een licentie

* U behoudt het auteursrecht op uw eigen creaties (vertalingen, afbeeldingen, scripts/programma's, enz.);
* Maar nadat u deze inhoud aan dit project heeft bijgedragen en deze is geaccepteerd (samengevoegd), stemt u ermee in dat anderen deze inhoud mogen gebruiken volgens de open source/gedeelde licentie van dit project.

Dit betekent:

* U **kunt** uw eigen werk nog steeds elders gebruiken en tentoonstellen;
* Maar u **kunt** niet na het samenvoegen van bijdragen eisen dat dit project of andere gebruikers die het werk legaal hebben verkregen, de licentie 'intrekken' of 'historische versies verwijderen'.

### 6.2 Licentie voor tekst en afbeeldingen (CC BY-NC-SA 4.0)

Voor de volgende inhoud die u bijdraagt:

* Vertaling, herziening en correctie van spelteksten;
* Projectdocumentatie, toelichtende teksten;
* Speciaal voor dit project gemaakte afbeeldingen en artistieke bronnen;

Zodra ze door deze repository zijn geaccepteerd en samengevoegd, wordt u geacht akkoord te gaan met:

1. Deze inhoud is gelicentieerd onder de **Naamsvermelding-NietCommercieel-GelijkDelen 4.0 Internationaal** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, afgekort **CC BY-NC-SA 4.0**);
2. Project Babel en alle gebruikers die deze inhoud verkrijgen, kunnen, onder **naleving van de CC BY-NC-SA 4.0-voorwaarden**:
* Deze inhoud delen, kopiëren en opnieuw verspreiden;
* Deze inhoud wijzigen en opnieuw creëren voor niet-commerciële doeleinden;
3. U stemt ermee in dat, voor zover toegestaan door de toepasselijke wetgeving, deze licentie **niet-exclusief, wereldwijd, royaltyvrij en onherroepelijk** is;
4. Zelfs als u zich later terugtrekt of stopt met deelname aan dit project, kan dit project de door u ingediende en samengevoegde inhoud blijven gebruiken en opnieuw verspreiden onder CC BY-NC-SA 4.0.

> Als u de bovenstaande licentievoorwaarden niet accepteert, dien dan geen tekst- of afbeeldingsbijdragen in bij dit project,
> of neem vooraf contact op met de projectonderhouder om te bevestigen of op een andere manier kan worden samengewerkt.

### 6.3 Licentie voor scripts en toolcode (GPL-3.0)

Voor wat u heeft ingediend en is geaccepteerd:

* Geautomatiseerde scripts;
* Bouw-/exportgereedschappen;
* Andere programmacode voor het verwerken van dit lokalisatieproject;

Tenzij anders vermeld, wordt beschouwd dat u ermee akkoord gaat:

1. De code is gelicentieerd onder **GPL-3.0** (GNU General Public License versie 3);
2. Projectbeheerders kunnen het wijzigen, samenvoegen en distribueren binnen de toegestane grenzen van GPL-3.0;
3. U kunt ook andere projecten voortzetten op basis van dezelfde code, zolang u de voorwaarden van GPL-3.0 naleeft.

Om licentieconflicten te voorkomen, probeer het volgende:

* Introduceer geen code van derden die **niet compatibel is met GPL-3.0** zonder bevestiging;
* Als u een bibliotheek van derden moet gebruiken, vermeld dan duidelijk de bron en licentie in de PR en bevestig de compatibiliteit.

### 6.4 Bovenliggende werken en originele spelrechten

Dit project is een **niet-officiële vertaling** van mods gerelateerd aan 《Project Zomboid》;

* Het originele spel en de mods zelf behoren hun respectievelijke auteurs/uitgevers;
* Dit project richt zich alleen op het vertalen, redigeren en organiseren van tekst en sommige ondersteunende bronnen;
* Bijdragers moeten ervoor zorgen dat bij het indienen van inhoud:
* Geen ongeautoriseerde vertalingen van derden of artistieke bronnen direct kopiëren;
* De rechten van de oorspronkelijke auteurs en mod-makers respecteren en geen inbreukmakende herpublicatie uitvoeren.

---

## 7. Communicatie en samenwerking

Als u vragen heeft over:

* Vragen over de licentievoorwaarden;
* Onzeker of een bepaalde inhoud kan worden bijgedragen;
* Uw werk op een speciale manier wilt licentiëren (bijv. alleen niet-commercieel gebruik zonder aanpassingen toestaan);

Neem contact op met de projectbeheerder via de volgende manieren:

* Dien een Issue in voor discussie;
* Andere openbaar beschikbare contactgegevens van beheerders.

We zullen proberen een oplossing te vinden die rekening houdt met de belangen van alle partijen, terwijl de gezonde ontwikkeling van het project wordt gewaarborgd.

---

## 8. Financiële ondersteuning

Tijdens de werking van het project is het nodig om continu LLM API aan te roepen voor vertaling vanwege nieuwe mods, updates van oude mods, enz. Om het gedrag van de LLM te sturen, moeten naast de basismodteksten ook uitgebreide promptinhoud worden verstrekt (inclusief basisprompts, vertaalregels, terminologie, invoer/uitvoerbeperkingen, semantische zoekresultaten, enz.), wat veel meer tokens verbruikt dan de originele tekst. Daarom heeft het project financiële ondersteuning nodig.

Als u bereid bent financiële steun te verlenen, neem dan contact op met de projectbeheerder. Hartelijk dank!

---

Nogmaals bedankt dat u bereid bent bij te dragen aan dit project!
Elke bijdrage van jou helpt meer spelers!
