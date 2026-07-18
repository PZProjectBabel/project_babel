# Bidragsgiverveiledning (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Innholdsfortegnelse

- [1. Før du begynner](#1-før-du-begynner)
- [2. Hvordan kan jeg bidra?](#2-hvordan-kan-jeg-bidra)
- [3. Gi oversettelsesregler, ordlister, forbedre systemets prompt-maler](#3-gi-oversettelsesregler-ordlister-forbedre-systemets-prompt-maler)
- [4. Gi manuelt korrekturlesning av korpus](#4-gi-manuelt-korrekturlesning-av-korpus)
- [5. Pipeline- og verktøyutviklingsbidrag](#5-pipeline--og-verktøyutviklingsbidrag)
- [6. Opphavsrett og lisensavtale](#6-opphavsrett-og-lisensavtale)
  - [6.1 Grunnleggende prinsipp: Du beholder opphavsretten, samtidig som du gir prosjektet lisens til å bruke](#61-grunnleggende-prinsipp-du-beholder-opphavsretten-samtidig-som-du-gir-prosjektet-lisens-til-å-bruke)
  - [6.2 Lisens for tekst, bilder og annet innhold (CC BY-NC-SA 4.0)](#62-lisens-for-tekst-bilder-og-annet-innhold-cc-by-nc-sa-40)
  - [6.3 Lisens for skript og verktøykode (GPL-3.0)](#63-lisens-for-skript-og-verktøykode-gpl-30)
  - [6.4 Opphavsrett for oppstrømsverk og originalspill](#64-opphavsrett-for-oppstrømsverk-og-originalspill)
- [7. Kommunikasjon og samarbeid](#7-kommunikasjon-og-samarbeid)
- [8. Økonomisk støtte](#8-økonomisk-støtte)

---

Tusen takk for at du ønsker å bidra til **Project Babel - LLM automatisk oversettelsesprosjekt for «Project Zomboid»-mods**! Enten det er å rette en feil, legge til en funksjon, skrive prompt-maler eller gi referanseoversettelser!

Å kalle på LLM API for oversettelse koster tokens. For at prosjektet skal kunne kjøre stabilt på lang sikt, håper vi du kan være raus og bidra!

> ⚠️ **Viktig påminnelse:**
> Før du leverer noe til dette repositoriet, må du lese og forstå avsnittet om «Opphavsrett og lisensieringsavtale».
> Når du har sendt inn og det er blitt slått sammen, anses det som at du godtar de respektive lisensvilkårene.

---

## 1. Før du begynner

Les først prosjektets `README.md` for å forstå:
- Prosjektets overordnede mål og nåværende status;
- Hvordan vanlige spillere bruker dette prosjektet (for egentesting);
- Prosjektets tekniske detaljer.

---

## 2. Hvordan kan jeg bidra?

Du kan velge en eller flere måter å delta på basert på dine interesser og ferdigheter:

- Gi oversettelsesregler for målspråket
- Gi en ordliste for oversettelse av målspråket
- Forbedre systemets prompt-maler
- Gi manuelt korrigerte oversettelsestekstkorpora
- Forbedre rørledningsmodulen (.NET) og automatiseringsskript
- Rapportere problemer, komme med forbedringsforslag (forklar i Issues)
- Gi økonomisk støtte for å kalle på LLM

Nedenfor gis en kort beskrivelse av de viktigste bidragsscenarioene.

---

## 3. Gi oversettelsesregler, ordlister, forbedre systemets prompt-maler

Rørledningens prompt-maler ligger i `src/prompt_templates/`, med følgende struktur:

- `system_prompt_translate_engine.txt`: Global oversettelsesmotor systemprompt (delt av alle språk);
- `<språkkode>/translation_dictionary_<språkkode>.json`: Ordbok for det språket;
- `<språkkode>/translation_schema_<språkkode>.md`: Oversettelsesregler og stilbegrensninger for det språket.

Bidragstrinn:

1. Opprett en underkatalog for språket ditt i `src/prompt_templates/`, legg til ordbok og oversettelsesregler;
2. Om nødvendig, juster den globale oversettelsesadferden ved å endre `system_prompt_translate_engine.txt` (merk at dette påvirker alle språk);
3. Bekreft effekten med lokal testing;
4. Send inn PR.

---

## 4. Gi manuelt korrekturlesning av korpus

Hvis du er en oversettelsesmod-forfatter og er villig til å gi oversettelseskorpuset ditt som LLM-oversettelsesreferanse, vennligst send inn en forespørsel i Issue. Du må oppgi følgende informasjon:

- Mod ID for oversettelsesmodden din og målspråket for oversettelsen;
- Skjermbilde av bakgrunnssiden til oversettelsesmodden din for å bevise at du er modforfatter;
- Angi tydelig i Issue at du er villig til å gi oversettelseskorpus;
- Hvis det er spesielle forhold (spesiallisens etc.), vennligst oppgi dem;
- Sørg for at korpuset du gir har høy kvalitet.

Under din autorisasjon vil prosjektet inkludere modden din i `config/ref_translation_mods.json` referanseoversettelsesmodliste, og pipelinen vil automatisk synkronisere oversettelsesteksten din som RAG-referansekorpus.

---

## 5. Pipeline- og verktøyutviklingsbidrag

Automatiseringen av dette prosjektet er delt i to deler:

**Pipeline-modulen (`src/`, C# / .NET 10)**：Inneholder 15 moduler som kjøres i rekkefølge, pluss 2 uavhengige moduler (`WorkshopMonitor` moduloppdager, `DocGenerator` dokumentgenerator), som har ansvar for hele prosessen fra SteamCMD initialisering, modulnedlasting, tekstuttrekk, innholdsgranskning, Embedding-beregning, RAG-søk til LLM-oversettelse og endelig utdata. Se [teknisk referanse](../technical_reference/technical_reference_no.md).

**Hjelpeskript (.github/)**: Brukes til GitHub-automatisering.

Hvis du ønsker:

* Fikse feil i eksisterende pipelinemoduler eller skript;
* Legge til nye funksjoner eller nye moduler i pipelinen;
* Optimalisere ytelse eller kodestruktur;
* Forbedre prompt-maler eller RAG-strategier;

Du kan følge disse trinnene:

1. Fork dette repositoriet og klon det lokalt;
2. Opprett en ny gren basert på den nyeste grenen;
3. Endre eller legg til filer i den tilsvarende katalogen:
- Pipelinemodulendring → `src/<modulnavn>/`;
- CI-arbeidsflytmodifikasjon → `.github/workflows/`；
- Prompt-malendring → `src/prompt_templates/`;
4. Før innsending, prøv så langt som mulig:

* Behold original kodestil;
* Legg til nødvendige kommentarer;
* Hvis mulig, legg ved enkle tester eller bruksanvisninger;
5. Send inn endringer via PR, og beskriv i beskrivelsen:

* Formålet med endringen;
* Kataloger / moduler / skript som kan påvirkes;
* Om det innebærer brytende endringer.

---

## 6. Opphavsrett og lisensavtale

> **Vennlig påminnelse:**
> Opphavsretts- og lisensavtalen er for å beskytte prosjektets, forfatternes, bidragsyternes og spillernes rettigheter, og unngå misforståelser på grunn av "stilltiende" eller "forutsetninger". Vennligst les nøye.
> Opphavsrett og lisens er basert på innholdet i README.md-filen; denne delen gir bare en mer forståelig beskrivelse.

### 6.1 Grunnleggende prinsipp: Du beholder opphavsretten, samtidig som du gir prosjektet lisens til å bruke

* Du beholder fortsatt opphavsretten til innhold du har laget (oversettelser, bilder, skript/programmer osv.);
* Men etter at du har sendt inn dette innholdet til prosjektet og det blir akseptert (sammenslått), samtykker du i å lisensiere det til andre i henhold til prosjektets åpen kildekode/deling-lisensavtale.

Dette betyr:

* Du **kan fortsatt** bruke og vise dine egne verk andre steder;
* Men du **kan ikke** kreve at prosjektet eller andre brukere som allerede har lovlig fått verket, "trekker tilbake lisensen" eller "sletter historiske versjoner" etter at bidraget er sammenslått.

### 6.2 Lisens for tekst, bilder og annet innhold (CC BY-NC-SA 4.0)

For følgende innhold du sender inn:

* Oversettelse, forbedring og korrekturlesing av spilltekster;
* Prosjektdokumentasjon, forklarende tekster;
* Bilder og kunstressurser spesielt laget for dette prosjektet;

Når de er akseptert og sammenslått i dette repositoriet, anses det som at du samtykker til:

1. Dette innholdet lisensieres under **Navngivelse-IkkeKommersiell-DelPåSammeVilkår 4.0 Internasjonal** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, forkortet **CC BY-NC-SA 4.0**);
2. Project Babel og alle brukere som får tilgang til dette innholdet, kan, under forutsetning av **å overholde vilkårene i CC BY-NC-SA 4.0**:
* Dele, kopiere og redistribuere dette innholdet;
* Modifisere og gjenskape det i ikke-kommersielle formål;
3. Du samtykker i at, innenfor rammen av gjeldende lov, er denne lisensen **ikke-eksklusiv, global, royaltyfri og ugjenkallelig**;
4. Selv om du senere trekker deg eller slutter å delta i prosjektet, kan prosjektet fortsette å bruke og redistribuere innhold du har sendt inn og som er blitt sammenslått, i henhold til CC BY-NC-SA 4.0.

> Hvis du ikke aksepterer lisensbetingelsene ovenfor, vennligst ikke send inn tekst- eller bildebidrag til dette prosjektet,
> eller kommuniser med prosjektets vedlikeholdere på forhånd for å bekrefte om samarbeid på andre måter er mulig.

### 6.3 Lisens for skript og verktøykode (GPL-3.0)

For det du sender inn og blir akseptert:

* Automatiseringsskript;
* Bygg/eksportverktøy;
* Annen programkode for behandling av dette oversettelsesprosjektet;

Med mindre annet er spesifisert, anses det at du godtar:

1. Koden lisensieres under **GPL-3.0** (GNU General Public License versjon 3);
2. Prosjektvedlikeholderne kan endre, slå sammen og distribuere den innenfor rammene av GPL-3.0;
3. Du kan også fortsette med andre prosjekter basert på samme kode, så lenge du overholder vilkårene i GPL-3.0.

For å unngå lisenskonflikter, prøv så langt som mulig:

* Ikke introduser tredjepartskode som er **inkompatibel med GPL-3.0** uten å ha bekreftet det;
* Hvis du må referere til et tredjepartsbibliotek, vennligst oppgi tydelig kilden og lisensen i PR-en, og bekreft kompatibiliteten.

### 6.4 Opphavsrett for oppstrømsverk og originalspill

Dette prosjektet er et **uoffisielt oversettelsesprosjekt** for mods relatert til *Project Zomboid*:

* Opphavsretten til originalspillet og hver mod tilhører deres respektive forfattere/utgivere;
* Dette prosjektet arbeider kun med tekstoversettelse, språkforbedringer og et utvalg tilhørende ressurser;
* Bidragsytere må sikre når de leverer innhold:
* Ikke kopier uautoriserte tredjepartsoversatte tekster eller grafiske ressurser direkte;
* Respekter rettighetene til originale forfattere og mod-forfattere, ikke foreta krenkende distribusjon.

---

## 7. Kommunikasjon og samarbeid

Hvis du:

* Har spørsmål om lisensvilkårene;
* Er usikker på om et bestemt innhold kan bidras;
* Ønsker å lisensiere arbeidet ditt på en spesiell måte (f.eks. kun tillate ikke-kommersiell bruk uten å tillate endringer);

Velkommen til å kontakte prosjektvedlikeholderen på følgende måter:

* Opprett en Issue for diskusjon;
* Andre offentlig tilgjengelige kontaktmetoder for vedlikeholderen.

Vi vil prøve å finne en løsning som balanserer prosjektets sunne utvikling samtidig som vi respekterer alles rettigheter.

---

## 8. Økonomisk støtte

Under prosjektdriften, på grunn av nye mods, oppdateringer av tekstinnhold i gamle mods osv., må LLM API kontinuerlig kalles for oversettelse. For å begrense LLM-oppførsel, i tillegg til grunnleggende mod-tekst, må store mengder prompt-innhold (inkludert grunnleggende prompt, oversettelsesregler, terminologiliste, input/output-begrensninger, semantiske søkeresultater osv.) også leveres. Dette innholdet vil forbruke langt flere tokens enn den opprinnelige teksten. Derfor trenger prosjektet økonomisk støtte.

Hvis du er villig til å gi økonomisk støtte, vennligst kontakt prosjektvedlikeholderen. Tusen takk!

---

Takk igjen for at du er villig til å bidra til dette prosjektet!
Ditt hvert bidrag vil gavne flere spillere!
