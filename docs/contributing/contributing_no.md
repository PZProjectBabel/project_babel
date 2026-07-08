# Bidragsguide (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Takk for at du er villig til å bidra til **Project Babel — det LLM-drevne automatiske oversettelsesprosjektet for Project Zomboid-mods**! Enten det er å fikse en feil, legge til en funksjon, skrive prompt-maler eller tilby referanseoversettelser — hvert bidrag teller!

Å kalle LLM API-et for oversettelse koster tokens. For at prosjektet skal kunne drives bærekraftig på lang sikt, settes det stor pris på din generøse støtte!

> ⚠️ **Viktig merknad:**
> Før du sender inn noe til dette repositoriet, må du lese og forstå avsnittet „Opphavsrett og lisensiering".
> Ved innsending og sammenslåing anses du for å ha godtatt de tilsvarende lisensvilkårene.

---

## Før du begynner

Les prosjektets `README.md` for å forstå:

- Prosjektets overordnede mål og nåværende status;
- Hvordan vanlige spillere bruker dette prosjektet (for dine egne tester);
- Tekniske detaljer om prosjektet.

---

## Hvordan kan jeg bidra?

Du kan velge en eller flere måter å delta på basert på dine interesser og ferdigheter:

- Tilby oversettelsesregler for et målspråk
- Tilby en termordbok for et målspråk
- Forbedre systempromptene
- Tilby manuelt korrigerte oversettelseskorpus
- Forbedre pipeline-moduler (.NET) og automatiseringsskript
- Rapportere problemer og foreslå forbedringer (via Issues)
- Gi økonomisk støtte til LLM API-kall

Nedenfor forklares de viktigste bidragsscenarioene.

---

## Tilby oversettelsesregler, termordbøker og forbedre systempromptene

Pipeline-ens prompt-maler ligger i `src/prompt_templates/`, med følgende struktur:

- `system_prompt_translate_engine.txt`: den globale systemprompten for oversettelsesmotoren (delt av alle språk);
- `<språkkode>/translation_dictionary_<språkkode>.json`: termordboken for det aktuelle språket;
- `<språkkode>/translation_schema_<språkkode>.md`: oversettelsesreglene og stilbegrensningene for det aktuelle språket.

Bidragstrinn:

1. Opprett en underkatalog under `src/prompt_templates/` for språket ditt, og legg til ordbok- og regelfilene;
2. Hvis du trenger å justere den globale oversettelsesatferden, endre `system_prompt_translate_engine.txt` (merk: dette påvirker alle språk);
3. Test lokalt for å bekrefte resultatene;
4. Send inn en PR.

---

## Tilby manuelt korrigerte korpus

Hvis du er forfatter av en oversettelsesmod og er villig til å tilby ditt oversettelseskorpus som LLM-oversettelsesreferanse, vennligst send inn en forespørsel via en Issue. Du må oppgi følgende informasjon:

- Mod-ID-en til oversettelsesmoden din og målspråket;
- Et skjermbilde av administrasjonssiden for oversettelsesmoden din som bevis på forfatterskap;
- En tydelig erklæring i Issue-en om at du er villig til å tilby oversettelseskorpuset;
- Hvis det er spesielle omstendigheter (spesiell lisens osv.), vennligst forklar;
- Sørg for at korpuset du tilbyr er av høy kvalitet.

Med din tillatelse vil prosjektet legge til moden din i referanseoversettelsesmodlisten `config/ref_translation_mods.json`, og pipeline-en vil automatisk synkronisere de oversatte tekstene dine som RAG-referansekorpus.

---

## Pipeline- og verktøyutviklingsbidrag

Automatiseringen i dette prosjektet er delt inn i to deler:

**Pipeline-moduler (`src/`, C# / .NET 10)**: Inneholder 15 sekvensielt utførte moduler som er ansvarlige for hele arbeidsflyten fra mod-nedlasting, tekstutvinning, innholdsgjennomgang, embedding-beregning, RAG-henting til LLM-oversettelse og endelig utdata. Se den [tekniske dokumentasjonen](../translation_entry_pipeline_zh-hans.md) for detaljer.

**Hjelpeskript (`.github/`)**: Brukes til GitHub-automatisering.

Hvis du ønsker å:

* Rette feil i eksisterende pipeline-moduler eller skript;
* Legge til nye funksjoner eller moduler i pipeline-en;
* Optimalisere ytelse eller kodestruktur;
* Forbedre prompt-maler eller RAG-strategier;

Kan du følge disse trinnene:

1. Fork dette repositoriet og klon det lokalt;
2. Opprett en ny gren fra den nyeste grenen;
3. Endre eller legg til filer i de tilsvarende katalogene:
   - Pipeline-modulendringer → `src/<modulnavn>/`;
   - Skriptendringer → `scripts/`;
   - Prompt-malendringer → `src/prompt_templates/`;
4. Før innsending, prøv å:

   * Beholde den eksisterende kodestilen;
   * Legge til nødvendige kommentarer;
   * Om mulig, legge ved enkle tester eller bruksanvisninger;
5. Send inn endringer via PR, og forklar i beskrivelsen:

   * Formålet med endringene;
   * Katalogene / modulene / skriptene som kan bli påvirket;
   * Om det innebærer ødeleggende endringer.

---

## Opphavsrett og lisensiering

> **Vennlig påminnelse:**
> Opphavsretts- og lisensvilkårene er utformet for å beskytte de legitime rettighetene og interessene til prosjektet, forfatterne, bidragsyterne og spillerne, og for å unngå misforståelser som følge av „stilltiende avtaler" eller „standardantagelser". Les dem nøye.
> Opphavsrett og lisensiering reguleres av innholdet i README.md-filen; denne delen gir kun en mer tilgjengelig beskrivelse.

### 1. Grunnprinsipp: Du beholder opphavsretten, samtidig som du lisensierer prosjektet til å bruke ditt verk

* Du har fortsatt opphavsretten til innholdet du skaper (oversettelser, bilder, skript/programmer osv.);
* Men når dette innholdet er sendt inn til dette prosjektet og akseptert (slått sammen),
  godtar du å lisensiere andre til å bruke dette innholdet under den åpen kildekode-/delte lisensen som er vedtatt av dette prosjektet.

Dette betyr:

* Du **kan fortsatt** fortsette å bruke og vise ditt verk andre steder;
* Men du **kan ikke**, etter at ditt bidrag er slått sammen, kreve at dette prosjektet eller andre brukere som rettmessig har fått verket, „tilbakekaller lisensen" eller „sletter historiske versjoner".

### 2. Lisensiering av tekster, bilder og lignende innhold (CC BY-NC-SA 4.0)

For følgende innhold du sender inn:

* Oversettelser av spilltekster, revisjoner og korrektur;
* Prosjektdokumentasjon og forklarende tekster;
* Bilder og kunstneriske ressurser laget spesielt for dette prosjektet;

Når det er akseptert og slått sammen i dette repositoriet, anses du for å ha godtatt at:

1. Dette innholdet er lisensiert under **Navngivelse-IkkeKommersiell-DelPåSammeVilkår 4.0 Internasjonal**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, forkortet **CC BY-NC-SA 4.0**);
2. Project Babel og alle brukere som mottar dette innholdet kan, **under overholdelse av CC BY-NC-SA 4.0-vilkårene**:

   * Dele, kopiere og videredistribuere dette innholdet;
   * Endre det og skape avledede verk for ikke-kommersielle formål;
3. Du godtar at denne lisensen er **ikke-eksklusiv, global, royaltyfri og ugjenkallelig** i den grad det er tillatt etter gjeldende lov;
4. Selv om du senere trekker deg eller slutter å delta i dette prosjektet, kan prosjektet fortsette å bruke og videredistribuere det relevante innholdet du har sendt inn og som er slått sammen, under CC BY-NC-SA 4.0.

> Hvis du ikke godtar de ovennevnte lisensvilkårene, vennligst ikke send inn tekst- eller bildebidrag til dette prosjektet,
> eller ta kontakt med prosjektets vedlikeholdere på forhånd for å bekrefte om samarbeid er mulig på annen måte.

### 3. Lisensiering av skript og verktøykode (GPL-3.0)

For følgende som du sender inn og som aksepteres:

* Automatiseringsskript;
* Bygge-/eksportverktøy;
* Annen programkode som brukes til å behandle dette oversettelsesprosjektet;

I mangel av spesielle erklæringer anses du for å ha godtatt at:

1. Koden er lisensiert under **GPL-3.0** (GNU General Public License versjon 3);
2. Prosjektets vedlikeholdere kan endre, slå sammen og distribuere den innenfor rammen tillatt av GPL-3.0;
3. Du kan også fortsette andre prosjekter basert på den samme koden, så lenge du overholder GPL-3.0-vilkårene.

For å unngå lisenskonflikter, prøv å:

* Ikke introdusere tredjepartskode som er **inkompatibel med GPL-3.0** uten forutgående bekreftelse;
* Hvis du må referere til tredjepartsbiblioteker, angi tydelig deres kilde og lisens i PR-en og bekreft kompatibiliteten.

### 4. Oppstrømsverk og opphavsrett til det originale spillet

Dette prosjektet er et **uoffisielt oversettelsesprosjekt** for mods relatert til *Project Zomboid*:

* Opphavsretten til det originale spillet og hver mod tilhører deres respektive forfattere/utgivere;
* Dette prosjektet omfatter kun opprettelse og organisering av tekstoversettelser, stilmessige justeringer og noen ledsagende ressurser;
* Bidragsytere må ved innsending av innhold sørge for:

  * Ikke å kopiere uautoriserte tredjepartsoversettelsestekster eller kunstneriske ressurser direkte;
  * Å respektere rettighetene til originale forfattere og mod-forfattere, og ikke foreta krenkende videredistribusjon.

---

## Kommunikasjon og samarbeid

Hvis du har:

* Spørsmål om lisensvilkårene;
* Usikkerhet om hvorvidt bestemt innhold kan bidras;
* Ønske om å lisensiere ditt verk på en spesiell måte (f.eks. kun ikke-kommersiell bruk uten tillatelse til bearbeidelse);

Ta gjerne kontakt med prosjektets vedlikeholdere via:

* Innsending av en Issue for diskusjon;
* Andre offentlig tilgjengelige kontaktmetoder for vedlikeholderne.

Vi vil gjøre vårt beste for å finne en løsning som balanserer prosjektets sunne utvikling samtidig som vi respekterer alle parters rettigheter og interesser.

---

## Økonomisk støtte

Under prosjektets drift, på grunn av tillegg av nye mods og tekstoppdateringer av eksisterende mods, må LLM API-et kalles kontinuerlig for oversettelse. For å begrense LLM-ens atferd kreves det i tillegg til de grunnleggende mod-tekstene en stor mengde prompt-innhold (inkludert grunnleggende prompter, oversettelsesregler, termtabeller, inn-/utdata-begrensninger, semantiske søkeresultater osv.), noe som forbruker langt flere tokens enn de originale tekstene. Derfor trenger prosjektet økonomisk støtte.

Hvis du ønsker å gi økonomisk støtte, vennligst kontakt prosjektets vedlikeholdere. Tusen takk!

---

Igjen, takk for at du er villig til å bidra til dette prosjektet!
Hvert bidrag du gir kommer flere spillere til gode!
