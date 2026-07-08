# Bidragsguide (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Tak for din villighed til at bidrage til **Project Babel — det LLM-drevne automatiske oversættelsesprojekt for Project Zomboid-mods**! Uanset om det er at rette en fejl, tilføje en funktion, skrive prompt-skabeloner eller levere referenceoversættelser — hvert bidrag tæller!

Kald af LLM API''en til oversættelse koster tokens. For at projektet kan køre bæredygtigt på lang sigt, er din generøse støtte meget værdsat!

> ⚠️ **Vigtig bemærkning:**
> Før du indsender noget til dette repository, skal du læse og forstå afsnittet „Ophavsret og licensering".
> Ved indsendelse og sammenfletning anses du for at have accepteret de tilsvarende licensvilkår.

---

## Før du begynder

Læs projektets `README.md` for at forstå:

- Projektets overordnede mål og nuværende status;
- Hvordan almindelige spillere bruger dette projekt (til dine egne tests);
- Tekniske detaljer om projektet.

---

## Hvordan kan jeg bidrage?

Du kan vælge en eller flere måder at deltage på baseret på dine interesser og færdigheder:

- Levere oversættelsesregler for et målsprog
- Levere en termordbog for et målsprog
- Forbedre systemprompts
- Levere manuelt korrigerede oversættelseskorpora
- Forbedre pipeline-moduler (.NET) og automatiseringsscripts
- Rapportere problemer og foreslå forbedringer (via Issues)
- Yde økonomisk støtte til LLM API-kald

Nedenfor forklares de vigtigste bidragsscenarier.

---

## Levering af oversættelsesregler, termordbøger og forbedring af systemprompts

Pipelinens prompt-skabeloner findes i `src/prompt_templates/` med følgende struktur:

- `system_prompt_translate_engine.txt`: den globale systemprompt for oversættelsesmotoren (deles af alle sprog);
- `<sprogkode>/translation_dictionary_<sprogkode>.json`: termordbogen for det pågældende sprog;
- `<sprogkode>/translation_schema_<sprogkode>.md`: oversættelsesregler og stilbegrænsninger for det pågældende sprog.

Bidragstrin:

1. Opret et underkatalog under `src/prompt_templates/` til dit sprog, og tilføj ordbogs- og regelfilerne;
2. Hvis du har brug for at justere den globale oversættelsesadfærd, skal du ændre `system_prompt_translate_engine.txt` (bemærk: dette påvirker alle sprog);
3. Test lokalt for at bekræfte resultaterne;
4. Indsend en PR.

---

## Levering af manuelt korrigerede korpora

Hvis du er forfatter til et oversættelsesmod og er villig til at levere dit oversættelseskorpus som LLM-oversættelsesreference, skal du indsende en anmodning via et Issue. Du skal angive følgende oplysninger:

- Mod ID''et for dit oversættelsesmod og målsproget;
- Et skærmbillede af administrationssiden for dit oversættelsesmod som bevis på forfatterskab;
- En klar erklæring i Issue om, at du er villig til at levere oversættelseskorpuset;
- Hvis der er særlige omstændigheder (særlig licens osv.), bedes du forklare dem;
- Sørg for, at det leverede korpus er af høj kvalitet.

Med din tilladelse vil projektet tilføje dit mod til referencelisten for oversættelsesmods `config/ref_translation_mods.json`, og pipelinen vil automatisk synkronisere dine oversatte tekster som RAG-referencekorpora.

---

## Pipeline- og værktøjsudviklingsbidrag

Automatiseringen i dette projekt er opdelt i to dele:

**Pipeline-moduler (`src/`, C# / .NET 10)**: Indeholder 15 sekventielt udførte moduler, der er ansvarlige for den komplette arbejdsgang fra mod-download, tekstekstraktion, indholdsgennemgang, embedding-beregning, RAG-hentning til LLM-oversættelse og endelig output. Se den [tekniske dokumentation](../translation_entry_pipeline_zh-hans.md) for detaljer.

**Hjælpescripts (`.github/`)**: Anvendes til GitHub-automatisering.

Hvis du ønsker at:

* Rette fejl i eksisterende pipeline-moduler eller scripts;
* Tilføje nye funktioner eller moduler til pipelinen;
* Optimere ydeevne eller kodestruktur;
* Forbedre prompt-skabeloner eller RAG-strategier;

Kan du følge disse trin:

1. Fork dette repository og klon det lokalt;
2. Opret en ny gren fra den nyeste gren;
3. Rediger eller tilføj filer i de tilsvarende mapper:
   - Ændringer i pipeline-moduler → `src/<modulnavn>/`;
   - Ændringer i scripts → `scripts/`;
   - Ændringer i prompt-skabeloner → `src/prompt_templates/`;
4. Før indsendelse, prøv at:

   * Bevare den eksisterende kodestil;
   * Tilføje nødvendige kommentarer;
   * Hvis muligt, vedlægge enkle tests eller brugsanvisninger;
5. Indsend ændringer via PR, og forklar i beskrivelsen:

   * Formålet med ændringerne;
   * De mapper / moduler / scripts, der kan blive påvirket;
   * Om det indebærer brydende ændringer.

---

## Ophavsret og licensering

> **Venlig påmindelse:**
> Ophavsrets- og licensvilkårene er designet til at beskytte projektets, forfatternes, bidragsydernes og spillernes legitime rettigheder og interesser og til at undgå misforståelser som følge af „stiltiende aftaler" eller „standardantagelser". Læs dem venligst omhyggeligt.
> Ophavsret og licensering reguleres af indholdet i README.md-filen; dette afsnit giver kun en mere tilgængelig beskrivelse.

### 1. Grundprincip: Du bevarer ophavsretten, mens du licenserer projektet til at bruge dit værk

* Du har stadig ophavsretten til det indhold, du skaber (oversættelser, billeder, scripts/programmer osv.);
* Men når dette indhold er indsendt til dette projekt og accepteret (sammenflettet),
  accepterer du at licensere andre til at bruge dette indhold under den open-source/delte licens, der er vedtaget af dette projekt.

Dette betyder:

* Du **kan stadig** fortsætte med at bruge og vise dit værk andre steder;
* Men du **kan ikke** efter sammenfletning af dit bidrag kræve, at dette projekt eller andre brugere, der lovligt har fået værket, „tilbagekalder licensen" eller „sletter historiske versioner".

### 2. Licensering af tekster, billeder og lignende indhold (CC BY-NC-SA 4.0)

For følgende indhold, du indsender:

* Oversættelser af spiltekster, revisioner og korrektur;
* Projektdokumentation og forklarende tekster;
* Billeder og kunstneriske ressourcer skabt specifikt til dette projekt;

Når det er accepteret og sammenflettet i dette repository, anses du for at acceptere, at:

1. Dette indhold er licenseret under **Kreditering-Ikke-kommerciel-Deling på samme vilkår 4.0 International**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, forkortet **CC BY-NC-SA 4.0**);
2. Project Babel og alle brugere, der modtager dette indhold, kan under **overholdelse af CC BY-NC-SA 4.0-vilkårene**:

   * Dele, kopiere og videredistribuere dette indhold;
   * Ændre det og skabe afledte værker til ikke-kommercielle formål;
3. Du accepterer, at denne licens er **ikke-eksklusiv, global, royaltyfri og uigenkaldelig** i det omfang, det er tilladt i henhold til gældende lov;
4. Selv hvis du senere trækker dig eller stopper med at deltage i dette projekt, kan projektet fortsætte med at bruge og videredistribuere det relevante indhold, du har indsendt, og som er blevet sammenflettet, under CC BY-NC-SA 4.0.

> Hvis du ikke accepterer ovenstående licensvilkår, bedes du ikke indsende tekst- eller billedbidrag til dette projekt,
> eller kontakt på forhånd projektvedligeholderne for at bekræfte, om samarbejde er muligt på anden vis.

### 3. Licensering af scripts og værktøjskode (GPL-3.0)

For følgende, som du indsender og som accepteres:

* Automatiseringsscripts;
* Build/eksport-værktøjer;
* Anden programkode, der bruges til at behandle dette oversættelsesprojekt;

I mangel af særlige erklæringer anses du for at acceptere, at:

1. Koden er licenseret under **GPL-3.0** (GNU General Public License version 3);
2. Projektvedligeholdere kan ændre, sammenflette og distribuere den inden for det område, der er tilladt af GPL-3.0;
3. Du kan også fortsætte andre projekter baseret på den samme kode, så længe du overholder GPL-3.0-vilkårene.

For at undgå licenskonflikter skal du forsøge at:

* Ikke introducere tredjepartskode, der er **inkompatibel med GPL-3.0**, uden forudgående bekræftelse;
* Hvis du har brug for at henvise til tredjepartsbiblioteker, skal du tydeligt angive deres kilde og licens i PR''en og bekræfte kompatibiliteten.

### 4. Opstrømsværker og ophavsret til det originale spil

Dette projekt er et **uofficielt oversættelsesprojekt** for mods relateret til *Project Zomboid*:

* Ophavsretten til det originale spil og hvert mod tilhører deres respektive forfattere/udgivere;
* Dette projekt involverer kun oprettelse og organisering af tekstoversættelser, stiltilpasninger og nogle ledsagende ressourcer;
* Bidragydere skal ved indsendelse af indhold sikre:

  * Ikke at kopiere uautoriserede tredjepartsoversættelsestekster eller kunstneriske ressourcer direkte;
  * At respektere originale forfatteres og mod-forfatteres rettigheder og ikke foretage krænkende videredistribution.

---

## Kommunikation og samarbejde

Hvis du har:

* Spørgsmål til licensvilkårene;
* Usikkerhed om, hvorvidt bestemt indhold kan bidrages;
* Ønske om at licensere dit værk på en særlig måde (f.eks. kun ikke-kommerciel brug uden tilladelse til bearbejdning);

Er du velkommen til at kontakte projektvedligeholderne via:

* Indsendelse af et Issue til diskussion;
* Andre offentligt tilgængelige kontaktmetoder for vedligeholderne.

Vi vil gøre vores bedste for at finde en løsning, der balancerer projektets sunde udvikling med respekt for alle parters rettigheder og interesser.

---

## Økonomisk støtte

Under projektets drift er det på grund af tilføjelse af nye mods og tekstopdateringer af eksisterende mods nødvendigt løbende at kalde LLM API''en til oversættelse. For at begrænse LLM''ens adfærd kræves der ud over de grundlæggende mod-tekster en stor mængde prompt-indhold (herunder grundlæggende prompts, oversættelsesregler, termtabeller, input/output-begrænsninger, semantiske søgeresultater osv.), hvilket forbruger langt flere tokens end de originale tekster. Derfor har projektet brug for økonomisk støtte.

Hvis du ønsker at yde økonomisk støtte, bedes du kontakte projektvedligeholderne. Mange tak!

---

Endnu en gang tak for din villighed til at bidrage til dette projekt!
Hvert bidrag, du yder, kommer flere spillere til gode!
