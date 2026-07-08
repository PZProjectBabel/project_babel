# Bijdraaggids (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Hartelijk dank voor je bereidheid om bij te dragen aan **Project Babel — het LLM-aangedreven automatische vertalingsproject voor Project Zomboid-mods**! Of het nu gaat om het oplossen van een bug, het toevoegen van een functie, het schrijven van prompt-sjablonen of het aanleveren van referentievertalingen — elke bijdrage telt!

Het aanroepen van de LLM API voor vertaling kost tokens. Om het project op lange termijn duurzaam te laten draaien, wordt je gulle steun zeer op prijs gesteld!

> ⚠️ **Belangrijke mededeling:**
> Voordat je iets indient bij deze repository, lees en begrijp de sectie "Auteursrecht & Licenties" zorgvuldig.
> Na indiening en merging word je geacht akkoord te zijn gegaan met de bijbehorende licentievoorwaarden.

---

## Voordat je begint

Lees de `README.md` van het project om het volgende te begrijpen:

- De overkoepelende doelstellingen en huidige status van dit project;
- Hoe gewone spelers dit project gebruiken (voor je eigen tests);
- Technische details van het project.

---

## Hoe kan ik bijdragen?

Je kunt op basis van je interesses en vaardigheden op een of meer manieren deelnemen:

- Vertaalregels voor een doeltaal aanleveren
- Een terminologisch woordenboek voor een doeltaal aanleveren
- De systeem-prompts verbeteren
- Handmatig gecorrigeerde vertaalcorpora aanleveren
- Pijplijnmodules (.NET) en automatiseringsscripts verbeteren
- Problemen melden en verbeteringen voorstellen (via Issues)
- Financiële steun bieden voor LLM API-aanroepen

Hieronder volgen uitleg voor de belangrijkste bijdraagscenario's.

---

## Vertaalregels, terminologische woordenboeken aanleveren en systeem-prompts verbeteren

De prompt-sjablonen van de pijplijn bevinden zich in `src/prompt_templates/`, met de volgende structuur:

- `system_prompt_translate_engine.txt`: de globale systeem-prompt van de vertaalengine (gedeeld door alle talen);
- `<taalcode>/translation_dictionary_<taalcode>.json`: het terminologisch woordenboek voor die taal;
- `<taalcode>/translation_schema_<taalcode>.md`: de vertaalregels en stijlbeperkingen voor die taal.

Bijdraagstappen:

1. Maak een submap aan onder `src/prompt_templates/` voor jouw taal en voeg de woordenboek- en regelbestanden toe;
2. Als je het globale vertaalgedrag wilt aanpassen, wijzig dan `system_prompt_translate_engine.txt` (let op: dit beïnvloedt alle talen);
3. Test lokaal om de resultaten te bevestigen;
4. Dien een PR in.

---

## Handmatig gecorrigeerde corpora aanleveren

Als je auteur bent van een vertaalmod en bereid bent je vertaalcorpus als LLM-vertaalreferentie aan te leveren, dien dan een verzoek in via een Issue. Je moet de volgende informatie verstrekken:

- De Mod ID van je vertaalmod en de doeltaal;
- Een screenshot van de beheerpagina van je vertaalmod om te bewijzen dat je de auteur bent;
- Een duidelijke verklaring in de Issue dat je bereid bent het vertaalcorpus aan te leveren;
- Als er bijzondere omstandigheden zijn (speciale licentie, enz.), leg deze dan uit;
- Zorg ervoor dat het aangeleverde corpus van hoge kwaliteit is.

Met jouw toestemming zal het project je mod toevoegen aan de referentievertalingsmodlijst `config/ref_translation_mods.json`, en de pijplijn zal je vertaalde teksten automatisch synchroniseren als RAG-referentiecorpora.

---

## Pijplijn- en toolontwikkelingsbijdragen

De automatisering in dit project is opgedeeld in twee delen:

**Pijplijnmodules (`src/`, C# / .NET 10)**: Bevat 15 sequentieel uitgevoerde modules, verantwoordelijk voor de volledige workflow van mod-downloaden, tekstextractie, inhoudscontrole, embedding-berekening, RAG-ophaling tot LLM-vertaling en uiteindelijke output. Zie de [technische documentatie](../translation_entry_pipeline_zh-hans.md) voor details.

**Hulpscripts (`.github/`)**: Gebruikt voor GitHub-automatisering.

Als je het volgende wilt:

* Bugs in bestaande pijplijnmodules of scripts oplossen;
* Nieuwe functies of modules aan de pijplijn toevoegen;
* Prestaties of codestructuur optimaliseren;
* Prompt-sjablonen of RAG-strategieën verbeteren;

Kun je deze stappen volgen:

1. Fork deze repository en kloon deze lokaal;
2. Maak een nieuwe branch aan vanaf de nieuwste branch;
3. Wijzig of voeg bestanden toe in de bijbehorende mappen:
   - Wijzigingen in pijplijnmodules → `src/<modulenaam>/`;
   - Wijzigingen in scripts → `scripts/`;
   - Wijzigingen in prompt-sjablonen → `src/prompt_templates/`;
4. Probeer voor het indienen:

   * De bestaande codeerstijl te behouden;
   * Noodzakelijke opmerkingen toe te voegen;
   * Indien mogelijk eenvoudige tests of gebruiksinstructies bij te voegen;
5. Dien wijzigingen in via PR, met uitleg in de beschrijving:

   * Het doel van de wijzigingen;
   * De mappen / modules / scripts die mogelijk beïnvloed worden;
   * Of het breaking changes betreft.

---

## Auteursrecht & Licenties

> **Vriendelijke herinnering:**
> De auteursrecht- en licentievoorwaarden zijn bedoeld om de legitieme rechten en belangen van het project, auteurs, bijdragers en spelers te beschermen, en om misverstanden door "stilzwijgende overeenstemming" of "standaardaannames" te voorkomen. Lees ze zorgvuldig.
> Auteursrecht en licenties worden beheerst door de inhoud van het README.md-bestand; deze sectie biedt slechts een toegankelijkere beschrijving.

### 1. Basisprincipe: Je behoudt het auteursrecht, terwijl je het project een licentie verleent om je werk te gebruiken

* Je behoudt het auteursrecht op de inhoud die je maakt (vertalingen, afbeeldingen, scripts/programma's, enz.);
* Echter, zodra deze inhoud bij dit project is ingediend en geaccepteerd (gemerged),
  ga je ermee akkoord anderen te licenseren om deze inhoud te gebruiken onder de door dit project aangenomen open-source/gedeelde licentie.

Dit betekent:

* Je **kunt nog steeds** je werk elders blijven gebruiken en tonen;
* Maar je **kunt niet**, nadat je bijdrage is gemerged, van dit project of andere gebruikers die het werk rechtmatig hebben verkregen eisen dat zij "de licentie intrekken" of "historische versies verwijderen".

### 2. Licentie van teksten, afbeeldingen en soortgelijke inhoud (CC BY-NC-SA 4.0)

Voor de volgende door jou ingediende inhoud:

* Vertalingen van spelteksten, revisies en correcties;
* Projectdocumentatie en toelichtende teksten;
* Afbeeldingen en artistieke middelen specifiek gemaakt voor dit project;

Na acceptatie en merging in deze repository word je geacht akkoord te gaan met het volgende:

1. Deze inhoud wordt gelicentieerd onder **Naamsvermelding-NietCommercieel-GelijkDelen 4.0 Internationaal**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, afgekort **CC BY-NC-SA 4.0**);
2. Project Babel en alle gebruikers die deze inhoud ontvangen mogen, **met inachtneming van de CC BY-NC-SA 4.0-voorwaarden**:

   * Deze inhoud delen, kopiëren en herdistribueren;
   * Deze inhoud wijzigen en afgeleide werken maken voor niet-commerciële doeleinden;
3. Je stemt ermee in dat deze licentie **niet-exclusief, wereldwijd, royaltyvrij en onherroepelijk** is voor zover toegestaan door de toepasselijke wetgeving;
4. Zelfs als je later terugtreedt of stopt met deelname aan dit project, mag het project de relevante inhoud die je hebt ingediend en die is gemerged, blijven gebruiken en herdistribueren onder CC BY-NC-SA 4.0.

> Als je de bovenstaande licentievoorwaarden niet aanvaardt, dien dan geen tekst- of beeldbijdragen in bij dit project,
> of neem vooraf contact op met de projectbeheerders om te bevestigen of samenwerking op een andere manier mogelijk is.

### 3. Licentie van scripts en toolcode (GPL-3.0)

Voor het volgende dat je indient en dat wordt geaccepteerd:

* Automatiseringsscripts;
* Build/export tools;
* Overige programmacode gebruikt voor de verwerking van dit vertalingsproject;

Bij afwezigheid van speciale verklaringen word je geacht akkoord te gaan met het volgende:

1. De code wordt gelicentieerd onder **GPL-3.0** (GNU General Public License versie 3);
2. Projectbeheerders mogen deze wijzigen, mergen en distribueren binnen het door GPL-3.0 toegestane bereik;
3. Je kunt ook andere projecten voortzetten op basis van dezelfde code, zolang je voldoet aan de GPL-3.0-voorwaarden.

Om licentieconflicten te voorkomen, probeer:

* Geen code van derden te introduceren die **niet compatibel is met GPL-3.0** zonder voorafgaande bevestiging;
* Als je moet verwijzen naar bibliotheken van derden, vermeld dan duidelijk hun bron en licentie in de PR en bevestig de compatibiliteit.

### 4. Upstreamwerken en auteursrecht van het originele spel

Dit project is een **onofficieel vertalingsproject** voor mods gerelateerd aan *Project Zomboid*:

* Het auteursrecht van het originele spel en elke mod behoort toe aan de respectievelijke auteurs/uitgevers;
* Dit project omvat alleen het maken en organiseren van tekstvertalingen, stijlaanpassingen en enkele ondersteunende middelen;
* Bijdragers moeten er bij het indienen van inhoud voor zorgen dat:

  * Geen ongeautoriseerde vertaalteksten of artistieke middelen van derden rechtstreeks worden gekopieerd;
  * De rechten van oorspronkelijke auteurs en mod-auteurs worden gerespecteerd en geen inbreukmakende herdistributie plaatsvindt.

---

## Communicatie & Samenwerking

Als je:

* Vragen hebt over de licentievoorwaarden;
* Twijfelt of bepaalde inhoud kan worden bijgedragen;
* Je werk op een speciale manier wilt licenseren (bijv. alleen niet-commercieel gebruik maar geen bewerking toegestaan);

Neem dan gerust contact op met de projectbeheerders via:

* Het indienen van een Issue voor discussie;
* Andere openbaar beschikbare contactmethoden van de beheerders.

We zullen ons best doen om een oplossing te vinden die de gezonde ontwikkeling van het project in evenwicht brengt met respect voor de rechten en belangen van alle partijen.

---

## Financiële steun

Tijdens de projectwerking moet de LLM API voortdurend worden aangeroepen voor vertaling vanwege nieuwe mods en tekstupdates van bestaande mods. Om het LLM-gedrag te sturen, is naast de basismodteksten een grote hoeveelheid promptinhoud nodig (inclusief basisprompts, vertaalregels, terminologietabellen, invoer-/uitvoerbeperkingen, semantische zoekresultaten, enz.), die veel meer tokens verbruikt dan de oorspronkelijke teksten. Daarom heeft het project financiële steun nodig.

Als je financiële steun wilt bieden, neem dan contact op met de projectbeheerders. Hartelijk dank!

---

Nogmaals bedankt voor je bereidheid om bij te dragen aan dit project!
Elke bijdrage die je levert, komt ten goede aan meer spelers!
