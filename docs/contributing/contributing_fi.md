# Osallistumisohjeet (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Sisällysluettelo

- [1. Ennen aloittamista](#1-ennen-aloittamista)
- [2. Kuinka voin osallistua?](#2-kuinka-voin-osallistua)
- [3. Tarjoa käännössääntöjä, sanakirjaa, paranna järjestelmäkehotteita](#3-tarjoa-käännössääntöjä-sanakirjaa-paranna-järjestelmäkehotteita)
- [4. Tarjoa käsin tarkistettua korpusta](#4-tarjoa-käsin-tarkistettua-korpusta)
- [5. Putkilinjan ja työkalujen kehitysosuus](#5-putkilinjan-ja-työkalujen-kehitysosuus)
- [6. Tekijänoikeus- ja lisenssisopimus](#6-tekijänoikeus--ja-lisenssisopimus)
  - [6.1 Perusperiaatteet: Säilytät tekijänoikeudet ja samalla annat projektille käyttöluvan](#61-perusperiaatteet-säilytät-tekijänoikeudet-ja-samalla-annat-projektille-käyttöluvan)
  - [6.2 Tekstien ja kuvien jne. sisällön lisensointi (CC BY-NC-SA 4.0)](#62-tekstien-ja-kuvien-jne-sisällön-lisensointi-cc-by-nc-sa-40)
  - [6.3 Skriptien ja työkalujen koodin lisensointi (GPL-3.0)](#63-skriptien-ja-työkalujen-koodin-lisensointi-gpl-30)
  - [6.4 Alkuperäisteokset ja alkuperäisen pelin tekijänoikeudet](#64-alkuperäisteokset-ja-alkuperäisen-pelin-tekijänoikeudet)
- [7. Viestintä ja yhteistyö](#7-viestintä-ja-yhteistyö)
- [8. Taloudellinen tuki](#8-taloudellinen-tuki)

---

Kiitos paljon, että olet valmis osallistumaan **Project Babel - 《僵尸毁灭工程》modin LLM-automaattikäännösprojektiin**! Olipa kyseessä virheen korjaus, uuden toiminnon lisääminen, kehote-mallipohjien kirjoittaminen tai viitekääntämisen tarjoaminen!

LLM API:n kutsuminen käännöstä varten maksaa tokeneista. Jotta projekti voisi toimia pitkäjänteisesti vakaasti, toivomme teidän anteliaalta avulta!

> ⚠️ **Tärkeä huomautus:**
> Ennen kuin lähetät mitään sisältöä tähän varastoon, lue ja ymmärrä "Tekijänoikeus- ja lisenssiehdot" -osio.
> Kun olet lähettänyt ja se on yhdistetty, katsotaan sinun hyväksyneen vastaavat lisenssiehdot.

---

## 1. Ennen aloittamista

Lue ensin projektin `README.md`, saadaksesi tietoa:
- Projektin kokonaistavoite ja nykytila;
- Kuinka tavalliset pelaajat käyttävät tätä projektia (helppoa itsetestausta varten);
- Projektin tekniset yksityiskohdat.

---

## 2. Kuinka voin osallistua?

Voit valita yhden tai useamman tavan osallistua kiinnostuksesi ja taitojesi mukaan:

- Tarjoa kohdekielen käännössääntöjä
- Tarjoa kohdekielen käännössanakirjaa
- Paranna järjestelmän kehotteita
- Tarjoa ihmisen oikolukemia käännöstekstikorpuksia
- Paranna putkistomoduulia (.NET) ja automaatioskriptejä
- Ilmoita ongelmista, ehdota parannuksia (kerro Issueissa)
- Tarjoa taloudellista tukea LLM-kutsuille

Alla on joitain selityksiä tärkeimmistä osallistumistavoista.

---

## 3. Tarjoa käännössääntöjä, sanakirjaa, paranna järjestelmäkehotteita

Putkiston kehotemallit sijaitsevat `src/prompt_templates/`, rakenne on seuraava:

- `system_prompt_translate_engine.txt`: Globaali käännösmoottorin järjestelmäkehotus (yhteinen kaikille kielille);
- `<kielikoodi>/translation_dictionary_<kielikoodi>.json`: Kyseisen kielen termisanakirja;
- `<kielikoodi>/translation_schema_<kielikoodi>.md`: Kyseisen kielen käännössäännöt ja tyylirajoitukset.

Osallistumisvaiheet:

1. Luo alihakemisto kielellesi `src/prompt_templates/` -kansioon, lisää termisanakirja ja käännössääntötiedosto;
2. Jos haluat säätää globaalia käännöskäyttäytymistä, muokkaa `system_prompt_translate_engine.txt` (huomaa, että se vaikuttaa kaikkiin kieliin);
3. Testaa paikallisesti varmistaaksesi tulokset;
4. Lähetä PR.

---

## 4. Tarjoa käsin tarkistettua korpusta

Jos olet käännösmodin tekijä ja haluat tarjota käännöskorpustasi LLM-käännösreferenssiksi, tee pyyntö Issue-sivulla. Sinun on annettava seuraavat tiedot:

- Käännösmodisi Mod ID ja käännöksen kohdekieli;
- Kuvakaappaus käännösmodisi hallintasivusta osoittaaksesi, että olet modin tekijä;
- Ilmoita selkeästi Issue-sivulla, että olet valmis tarjoamaan käännöskorpusta;
- Jos on erityistilanteita (erityislisenssit jne.), kerro ne samalla;
- Varmista, että tarjoamasi korpus on laadukas.

Luvallasi projekti lisää modisi `config/ref_translation_mods.json` -viitekäännösmodiluetteloon, ja putkilinja synkronoi automaattisesti käännöstekstisi RAG-viitekorpukseksi.

---

## 5. Putkilinjan ja työkalujen kehitysosuus

Projektin automatisointi on jaettu kahteen osaan:

**Putkilinjamoduuli (`src/`, C# / .NET 10)**: Sisältää 15 peräkkäin suoritettavaa moduulia, jotka vastaavat koko prosessista SteamCMD-alustuksesta, modin latauksesta, tekstin erotuksesta, sisällön tarkistuksesta, Embedding-laskennasta, RAG-hausta LLM-käännökseen ja lopulliseen tulosteeseen. Katso lisätietoja [teknisestä viitteestä](../technical_reference/technical_reference_fi.md).

**Aputoimintokomentosarjat (.github/)**: Käytetään GitHub-automaatioon.

Jos haluat:

* Korjata olemassa olevan putkilinjamoduulin tai komentosarjan bugeja;
* Lisätä uusia toimintoja tai moduuleja putkilinjaan;
* Optimoida suorituskykyä tai koodirakennetta;
* Parantaa prompt-malleja tai RAG-strategiaa;

Voit toimia seuraavasti:

1. Forkkaa tämä repositorio ja kloonaa se paikallisesti;
2. Luo uusi haara perustuen uusimpaan haaraan;
3. Muokkaa tai lisää tiedostoja vastaavaan hakemistoon:
- Putkilinjamoduulin muutos → `src/<moduulin_nimi>/`;
- Komentosarjan muutos → `scripts/`;
- Prompt-mallin muutos → `src/prompt_templates/`;
4. Ennen lähetystä pyri mahdollisuuksien mukaan:

* Säilytä alkuperäinen koodityyli;
* Lisää tarvittavat kommentit;
* Jos mahdollista, liitä mukaan yksinkertainen testi tai käyttöohje;
5. Lähetä muutokset PR:n kautta ja kuvaile ne kuvauksessa:

* Muutoksen tarkoitus;
* Mahdollisesti vaikuttavat hakemistot / moduulit / skriptit;
* Sisältääkö se muutoksia, jotka rikkovat yhteensopivuuden.

---

## 6. Tekijänoikeus- ja lisenssisopimus

> **Ystävällinen huomautus:**
> Tekijänoikeus- ja lisenssisopimus on suojella projektin, tekijöiden, avustajien ja pelaajien oikeutettuja etuja ja välttää väärinkäsityksiä, jotka johtuvat "hiljaisesta sopimuksesta" tai "oletuksesta". Lue se huolellisesti.
> Tekijänoikeus- ja lisenssisopimus perustuu README.md-tiedoston sisältöön; tämä osio tarjoaa vain helppotajuisemman kuvauksen.

### 6.1 Perusperiaatteet: Säilytät tekijänoikeudet ja samalla annat projektille käyttöluvan

* Sinulla on edelleen tekijänoikeus itse luomaasi sisältöön (käännökset, kuvat, skriptit/ohjelmat jne.);
* Mutta kun olet lähettänyt nämä sisällöt tähän projektiin ja ne on hyväksytty (yhdistetty), hyväksyt, että käyttöoikeus annetaan muille tämän projektin käyttämän avoimen lähdekoodin/jakamislisenssin mukaisesti.

Tämä tarkoittaa:

* Voit **edelleen** käyttää ja esitellä omia teoksiasi muualla;
* Mutta et **voi** vaatia, että tämä projekti tai muut laillisesti teoksen saaneet käyttäjät "peruuttavat käyttöoikeuden" tai "poistavat vanhoja versioita" sen jälkeen, kun panoksesi on yhdistetty.

### 6.2 Tekstien ja kuvien jne. sisällön lisensointi (CC BY-NC-SA 4.0)

Seuraaville lähettämillesi sisällöille:

* Pelitekstien käännökset, viimeistely ja oikoluku;
* Projektin dokumentaatio, selittävät tekstit;
* Nimenomaan tätä projektia varten luodut kuvat ja taideresurssit;

Kun ne on hyväksytty ja yhdistetty tähän arkistoon, katsotaan sinun hyväksyneen:

1. Nämä sisällöt on lisensoitu **Nimeä-EiKaupallinen-JaaSamoin 4.0 Kansainvälinen** -lisenssillä (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, lyhennettynä **CC BY-NC-SA 4.0**);
2. Project Babel ja kaikki käyttäjät, jotka saavat tämän sisällön, voivat **CC BY-NC-SA 4.0 -ehtojen mukaisesti**:
* Jakaa, kopioida ja levittää näitä sisältöjä;
* Muokata ja uudelleenluoda niitä ei-kaupallisissa tarkoituksissa;
3. Hyväksyt, että sovellettavan lain sallimissa rajoissa tämä lisenssi on **yksinoikeudeton, maailmanlaajuinen, rojaltivapaa ja peruuttamaton**;
4. Vaikka myöhemmin poistuisit tai lopettaisit osallistumisen tähän projektiin, projekti voi edelleen käyttää ja julkaista uudelleen jo lähettämäsi ja yhdistetyt sisällöt CC BY-NC-SA 4.0 -lisenssin mukaisesti.

> Jos et hyväksy yllä olevaa lisensointitapaa, älä lähetä teksti- tai kuvasisältöjä tälle projektille,
> tai ota etukäteen yhteyttä projektin ylläpitäjään varmistaaksesi, voitko tehdä yhteistyötä muulla tavalla.

### 6.3 Skriptien ja työkalujen koodin lisensointi (GPL-3.0)

Seuraaville lähettämillesi ja hyväksytyille:

* Automaattiset skriptit;
* Rakennus-/vientityökalut;
* Muut tämän käännösprojektin käsittelyyn tarkoitetut ohjelmakoodit;

Ilman erillistä ilmoitusta katsotaan, että hyväksyt seuraavat:

1. Koodi on lisensoitu **GPL-3.0** (GNU General Public License versio 3) -lisenssillä;
2. Projektin ylläpitäjä voi muokata, yhdistää ja jakaa koodia GPL-3.0 sallimissa rajoissa;
3. Voit myös jatkaa muita projekteja saman koodin pohjalta, kunhan noudatat GPL-3.0 ehtoja.

Lisenssiristiriitojen välttämiseksi pyri mahdollisuuksien mukaan:

* Älä lisää **GPL-3.0:n kanssa yhteensopimattomia** kolmannen osapuolen koodeja ilman varmistusta;
* Jos kolmannen osapuolen kirjasto on tarpeen, kuvaile selkeästi sen lähde ja lisenssi PR:ssä ja varmista yhteensopivuus.

### 6.4 Alkuperäisteokset ja alkuperäisen pelin tekijänoikeudet

Tämä projekti on **epävirallinen käännösprojekti** Project Zomboid -pelin modeille:

* Alkuperäisen pelin ja kunkin modin tekijänoikeudet kuuluvat niiden omistajille/julkaisijoille;
* Tämä projekti keskittyy ainoastaan tekstikäännösten, hienosäätöjen ja osan liittyvien resurssien luomiseen ja järjestämiseen;
* Osallistujien on varmistettava lähettäessään sisältöä:
* Älä kopioi suoraan luvattomia kolmannen osapuolen käännöstekstejä tai graafisia resursseja;
* Kunnioita alkuperäisten tekijöiden ja modin tekijöiden oikeuksia, älä levitä loukkaavaa sisältöä.

---

## 7. Viestintä ja yhteistyö

Jos sinulla on:

* Kysymyksiä lisenssiehdoista;
* Epävarmuutta siitä, voitko antaa tiettyä sisältöä;
* Toiveita lisensoida työsi erityisellä tavalla (esim. sallia vain ei-kaupallinen käyttö ilman muokkausta);

Tervetuloa ottamaan yhteyttä projektin ylläpitäjään seuraavilla tavoilla:

* Luo Issue keskustelua varten;
* Muut ylläpitäjien julkisesti ilmoittamat yhteystiedot.

Pyrimme löytämään ratkaisun, joka kunnioittaa kaikkien osapuolten oikeuksia ja samalla edistää projektin tervettä kehitystä.

---

## 8. Taloudellinen tuki

Projektin toiminnan aikana uusien modien lisäämisen ja vanhojen modien tekstipäivitysten vuoksi LLM APIa on jatkuvasti kutsuttava käännöksiä varten. LLM:n käyttäytymisen ohjaamiseksi modien perustekstien lisäksi tarvitaan runsaasti kehotteita (mukaan lukien peruskehotteet, käännössäännöt, termistö, syöte-/tulostusrajoitukset, semanttiset hakutulokset jne.), jotka kuluttavat paljon enemmän tokeneita kuin alkuperäinen teksti. Siksi projekti tarvitsee taloudellista tukea.

Jos haluat tarjota taloudellista tukea, ota yhteyttä projektin ylläpitäjään. Suuret kiitokset!

---

Kiitos vielä kerran, että olet valmis antamaan panoksesi tälle projektille!
Jokainen panoksesi auttaa useampia pelaajia hyötymään!
