# Bidragsguide (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Indholdsfortegnelse

- [1. Før du begynder](#1-før-du-begynder)
- [2. Hvordan kan jeg bidrage?](#2-hvordan-kan-jeg-bidrage)
- [3. Give oversættelsesregler, ordbog, forbedre systemprompter](#3-give-oversættelsesregler-ordbog-forbedre-systemprompter)
- [4. Tilvejebringelse af manuelt korrekturlæst korpus](#4-tilvejebringelse-af-manuelt-korrekturlæst-korpus)
- [5. Pipeline og værktøjsudviklingsbidrag](#5-pipeline-og-værktøjsudviklingsbidrag)
- [6. Ophavsret og licensaftale](#6-ophavsret-og-licensaftale)
  - [6.1 Grundlæggende princip: Du beholder ophavsretten og giver samtidig projektet tilladelse til at bruge](#61-grundlæggende-princip-du-beholder-ophavsretten-og-giver-samtidig-projektet-tilladelse-til-at-bruge)
  - [6.2 Licens for tekst, billeder og lignende indhold (CC BY-NC-SA 4.0)](#62-licens-for-tekst-billeder-og-lignende-indhold-cc-by-nc-sa-40)
  - [6.3 Licens for scripts og værktøjskode (GPL-3.0)](#63-licens-for-scripts-og-værktøjskode-gpl-30)
  - [6.4 Ophavsret til overordnede værker og originalspil](#64-ophavsret-til-overordnede-værker-og-originalspil)
- [7. Kommunikation og samarbejde](#7-kommunikation-og-samarbejde)
- [8. Økonomisk støtte](#8-økonomisk-støtte)

---

Mange tak fordi du vil bidrage til **Project Babel - LLM automatisk oversættelsesprojekt for Project Zomboid mods**! Uanset om det er at rette en fejl, tilføje en ny funktion, skrive prompter eller give referenceoversættelser!

Det koster tokens at kalde LLM API for oversættelse. For at projektet kan køre stabilt på lang sigt, håber vi du vil give en generøs håndsrækning!

> ⚠️ **Vigtig påmindelse:**
> Før du indsender noget til dette repository, skal du læse og forstå afsnittet "Copyright og licensaftale".
> Når du indsender, og det bliver flettet, anses det for, at du accepterer de tilhørende licensvilkår.

---

## 1. Før du begynder

Læs først projektets `README.md` for at forstå:
- Projektets overordnede mål og nuværende status;
- Hvordan almindelige spillere bruger projektet (så du kan teste selv);
- Projektets tekniske detaljer.

---

## 2. Hvordan kan jeg bidrage?

Du kan vælge en eller flere måder at deltage på, baseret på dine interesser og færdigheder:

- Give oversættelsesregler for målsproget
- Give en oversættelsesordbog for målsproget
- Forbedre systemets prompt
- Give korrekturlæste oversættelsestekster som korpus
- Forbedre pipeline-moduler (.NET) og automatiseringsscripts
- Rapportere problemer, komme med forbedringsforslag (beskrives i Issues)
- Give økonomisk støtte til LLM-kald

Herunder gives en kort beskrivelse af de vigtigste bidragsområder.

---

## 3. Give oversættelsesregler, ordbog, forbedre systemprompter

Pipeline prompt-skabelonerne er placeret i `src/prompt_templates/` og har følgende struktur:

- `system_prompt_translate_engine.txt`: Global oversættelsesmotor-systemprompt (fælles for alle sprog);
- `<sprogkode>/translation_dictionary_<sprogkode>.json`: Ordliste for det pågældende sprog;
- `<sprogkode>/translation_schema_<sprogkode>.md`: Oversættelsesregler og stilbegrænsninger for sproget.

Bidragstrin:

1. Opret en undermappe til dit sprog under `src/prompt_templates/`, tilføj ordbog og oversættelsesregler;
2. Hvis du skal justere global oversættelsesadfærd, rediger `system_prompt_translate_engine.txt` (bemærk, at det påvirker alle sprog);
3. Lokal test for at bekræfte effekten;
4. Indsend PR.

---

## 4. Tilvejebringelse af manuelt korrekturlæst korpus

Hvis du er oversættelsesmod-forfatter og er villig til at stille dit oversættelseskorpus til rådighed som reference for LLM-oversættelse, skal du indsende en anmodning i Issue. Du skal levere følgende oplysninger:

- Dit oversættelsesmods Mod ID og målsproget for oversættelsen;
- Et skærmbillede af din oversættelsesmods bagside for at bevise, at du er mod-forfatter;
- Angiv tydeligt i Issue, at du er villig til at stille oversættelseskorpus til rådighed;
- Hvis der er særlige omstændigheder (særlig licens osv.), bedes du også oplyse det;
- Sørg for, at det korpus, du stiller til rådighed, er af høj kvalitet.

Under din autorisation vil projektet føje dit mod til listen over referenceoversættelsesmods i `config/ref_translation_mods.json`, og pipelinen vil automatisk synkronisere din oversatte tekst som RAG-referencekorpus.

---

## 5. Pipeline og værktøjsudviklingsbidrag

Automatiseringen af dette projekt er opdelt i to dele:

**Pipeline-modul (`src/`, C# / .NET 10)**: Indeholder 15 moduler, der udføres i rækkefølge, plus 2 uafhængige moduler (`WorkshopMonitor` modulopdager, `DocGenerator` dokumentgenerator), som står for hele processen fra SteamCMD-initialisering, moduldownload, tekstudtræk, indholdsgennemgang, Embedding-beregning, RAG-søgning til LLM-oversættelse og endelig output. Se [Teknisk reference](../technical_reference/technical_reference_da.md).

**Hjælpeskripter (`.github/`)**: Bruges til GitHub-automatisering.

Hvis du ønsker:

* Rettelse af fejl i eksisterende pipelinemoduler eller scripts;
* Tilføjelse af nye funktioner eller nye moduler til pipelinen;
* Optimering af ydeevne eller kodesstruktur;
* Forbedring af prompt-skabeloner eller RAG-strategi;

Kan du følge disse trin:

1. Fork dette repository og klon det lokalt;
2. Opret en ny gren baseret på den seneste gren;
3. Rediger eller tilføj filer i den relevante mappe:
- Pipelinemodulændringer → `src/<modulnavn>/`;
- CI workflow ændring → `.github/workflows/`；
- Prompt-skabelonændringer → `src/prompt_templates/`;
4. Inden indsendelse, så vidt muligt:

* Bevar den eksisterende kodestil;
* Tilføj nødvendige kommentarer;
* Hvis muligt, medtag enkle test- eller brugsanvisninger;
5. Indsend ændringer via PR, og forklar i beskrivelsen:

* Formålet med ændringen;
* Berørte mapper / moduler / scripts;
* Om det medfører brud på ændringer (breaking changes).

---

## 6. Ophavsret og licensaftale

> **Venlig påmindelse:**
> Ophavsret og licensaftale er til for at beskytte de legitime rettigheder for projektet, forfattere, bidragydere og spillere for at undgå misforståelser på grund af "stiltiende aftale" eller "standardantagelse". Læs venligst omhyggeligt.
> Ophavsret og licens er baseret på indholdet i README.md-filen; dette afsnit giver kun en lettere forståelig beskrivelse.

### 6.1 Grundlæggende princip: Du beholder ophavsretten og giver samtidig projektet tilladelse til at bruge

* Du har stadig ophavsret til dit eget skabte indhold (oversættelser, billeder, scripts/programmer osv.);
* Men efter at have indsendt dette indhold til projektet og fået det accepteret (merger), accepterer du at give andre tilladelse til at bruge dette indhold i henhold til den open source/deling licensaftale, som projektet anvender.

Det betyder:

* Du **kan stadig** bruge og vise dit eget værk andre steder;
* Men du **kan ikke** kræve, at projektet eller andre brugere, der lovligt har fået værket, "trækker tilladelsen tilbage" eller "sletter historiske versioner" efter dit bidrag er blevet merger.

### 6.2 Licens for tekst, billeder og lignende indhold (CC BY-NC-SA 4.0)

For følgende indhold, du indsender:

* Oversættelse, redigering og korrekturlæsning af spiltekst;
* Projekt dokumentation, beskrivende tekst;
* Billeder og kunstressourcer skabt specifikt til dette projekt;

Når det er accepteret og merger i dette repository, anses det for, at du accepterer:

1. Dette indhold er licenseret under **Kreditering-IkkeKommerciel-Deling på samme vilkår 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, forkortet **CC BY-NC-SA 4.0**);
2. Project Babel og alle brugere, der har fået indholdet, kan, forudsat at de **overholder CC BY-NC-SA 4.0 vilkårene**:
* Dele, kopiere og videredistribuere dette indhold;
* Ændre og genbruge det til ikke-kommercielle formål;
3. Du accepterer, at inden for rammerne af gældende lov er denne licens **ikke-eksklusiv, verdensomspændende, royaltyfri og uigenkaldelig**;
4. Selv hvis du senere trækker dig eller stopper med at deltage i dette projekt, kan projektet stadig fortsætte med at bruge og genudgive det indhold, du har indsendt og fået merger, i henhold til CC BY-NC-SA 4.0.

> Hvis du ikke accepterer ovenstående licensform, bedes du ikke indsende tekst- eller billedbidrag til dette projekt,
> eller kommunikere med projektvedligeholderen på forhånd for at bekræfte, om samarbejde på andre måder er muligt.

### 6.3 Licens for scripts og værktøjskode (GPL-3.0)

For det, du indsender og som bliver accepteret:

* Automatiseringsscripts;
* Byg-/eksportværktøjer;
* Anden programkode til behandling af dette lokaliseringsprojekt;

Under forudsætning af ingen særlig erklæring, anses det for at være din accept:

1. Koden er licenseret under **GPL-3.0** (GNU General Public License version 3);
2. Projektvedligeholdere kan inden for rammerne af GPL-3.0 ændre, flette og distribuere den;
3. Du kan også fortsætte andre projekter baseret på den samme kode, så længe du overholder vilkårene i GPL-3.0.

For at undgå licenskonflikter, bedes du så vidt muligt:

* Undlad at introducere tredjepartskode, der er **inkompatibel med GPL-3.0**, uden bekræftelse;
* Hvis du har brug for at referere et tredjepartsbibliotek, bedes du i PR'en tydeligt angive dets kilde og licens og bekræfte dets kompatibilitet.

### 6.4 Ophavsret til overordnede værker og originalspil

Dette projekt er et **uofficielt oversættelses**projekt for mods relateret til "Project Zomboid";

* Ophavsretten til originalspillet og hver mod tilhører deres respektive forfattere/udgivere;
* Dette projekt fokuserer kun på tekstoversættelse, sproglig forbedring og organisering af visse tilhørende ressourcer;
* Bidragydere skal sikre, når de indsender indhold:
* Ikke direkte kopiere uautoriserede tredjeparts oversættelsestekster eller kunstressourcer;
* Respektere rettighederne for originale forfattere og mod-forfattere, og ikke udgive krænkende kopier.

---

## 7. Kommunikation og samarbejde

Hvis du:

* Har spørgsmål til licensvilkårene;
* Er usikker på, om et bestemt indhold kan bidrages;
* Ønsker at licensere dit værk på en særlig måde (f.eks. kun tillade ikke-kommerciel brug men ikke åben for ændringer osv.);

Velkommen til at kontakte projektvedligeholderne via følgende metoder:

* Indsend et Issue for at diskutere;
* Andre kontaktoplysninger, der er offentligt tilgængelige fra vedligeholderne.

Vi vil bestræbe os på at finde en løsning, der tager hensyn til alle parters rettigheder og samtidig sikrer projektets sunde udvikling.

---

## 8. Økonomisk støtte

Under projektets drift, på grund af tilføjelse af nye mods og opdatering af tekstindhold i gamle mods, er der behov for løbende at kalde LLM API til oversættelse. For at begrænse LLM-adfærden kræves der ud over den grundlæggende mod-tekst også en stor mængde prompt-indhold (inklusive grundlæggende prompts, oversættelsesregler, ordlister, input-output-begrænsninger, semantiske søgeresultater osv.), som forbruger langt flere tokens end den originale tekst. Derfor har projektet brug for økonomisk støtte.

Hvis du er villig til at yde økonomisk støtte, bedes du kontakte projektvedligeholderne. Mange tak!

---

Endnu engang tak for din villighed til at bidrage til dette projekt!
Din hver eneste bidrag vil gavne flere spillere!
