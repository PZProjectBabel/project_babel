# Osallistumisopas (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Kiitos halukkuudestasi osallistua **Project Babel — Project Zomboid -modien LLM-pohjaiseen automaattiseen käännösprojektiin**! Olipa kyseessä virheen korjaus, ominaisuuden lisäys, prompt-mallipohjien kirjoittaminen tai viitekäännösten tarjoaminen — jokainen panos on tärkeä!

LLM API:n kutsuminen käännöstä varten maksaa tokeneita. Jotta projekti voi toimia kestävästi pitkällä aikavälillä, antelias tukesi on erittäin arvostettua!

> ⚠️ **Tärkeä huomautus:**
> Ennen kuin lähetät mitään tähän tietovarastoon, lue ja ymmärrä "Tekijänoikeudet ja lisensointi" -osio.
> Lähettämällä ja yhdistämällä katsot hyväksyneesi vastaavat lisenssiehdot.

---

## Ennen kuin aloitat

Lue projektin `README.md` ymmärtääksesi:

- Tämän projektin yleiset tavoitteet ja nykytilan;
- Miten tavalliset pelaajat käyttävät tätä projektia (omia testejäsi varten);
- Projektin tekniset yksityiskohdat.

---

## Miten voin osallistua?

Voit valita yhden tai useamman tavan osallistua kiinnostuksesi ja taitojesi mukaan:

- Tarjota käännössääntöjä kohdekielelle
- Tarjota termisanasto kohdekielelle
- Parantaa järjestelmäprompteja
- Tarjota manuaalisesti korjattuja käännöskorpuksia
- Parantaa putkimoduuleja (.NET) ja automaatioskriptejä
- Raportoida ongelmista ja ehdottaa parannuksia (Issuet)
- Tarjota taloudellista tukea LLM API -kutsuille

Alla on selitykset tärkeimmistä osallistumisskenaarioista.

---

## Käännössääntöjen, termisanastojen tarjoaminen ja järjestelmäpromptien parantaminen

Putken prompt-mallipohjat sijaitsevat `src/prompt_templates/` -hakemistossa seuraavalla rakenteella:

- `system_prompt_translate_engine.txt`: globaali käännösmoottorin järjestelmäprompti (kaikkien kielten yhteinen);
- `<kielikoodi>/translation_dictionary_<kielikoodi>.json`: kyseisen kielen termisanasto;
- `<kielikoodi>/translation_schema_<kielikoodi>.md`: kyseisen kielen käännössäännöt ja tyylirajoitukset.

Osallistumisvaiheet:

1. Luo alihakemisto `src/prompt_templates/` -hakemiston alle kielellesi ja lisää sanasto- ja sääntötiedostot;
2. Jos sinun on säädettävä globaalia käännöskäyttäytymistä, muokkaa `system_prompt_translate_engine.txt` -tiedostoa (huom: tämä vaikuttaa kaikkiin kieliin);
3. Testaa paikallisesti vahvistaaksesi tulokset;
4. Lähetä PR.

---

## Manuaalisesti korjattujen korpusten tarjoaminen

Jos olet käännösmodin tekijä ja olet halukas tarjoamaan käännöskorpuksesi LLM-käännösviitteeksi, lähetä pyyntö Issuen kautta. Sinun on annettava seuraavat tiedot:

- Käännösmodisi Mod ID ja kohdekieli;
- Kuvakaappaus käännösmodisi hallintasivulta todistaaksesi tekijyyden;
- Selkeä ilmoitus Issuessa, että olet halukas tarjoamaan käännöskorpuksen;
- Jos on erityisiä olosuhteita (erityinen lisenssi jne.), selitä ne;
- Varmista, että tarjoamasi korpus on korkealaatuinen.

Valtuutuksellasi projekti lisää modisi viitekäännösmodien luetteloon `config/ref_translation_mods.json`, ja putki synkronoi automaattisesti käännetyt tekstisi RAG-viitekorpuksina.

---

## Putki- ja työkalujen kehityspanokset

Tämän projektin automaatio on jaettu kahteen osaan:

**Putkimoduulit (`src/`, C# / .NET 10)**: Sisältää 15 peräkkäin suoritettavaa moduulia, jotka vastaavat koko työnkulusta modien lataamisesta, tekstin purkamisesta, sisällöntarkistuksesta, upotuslaskennasta, RAG-hausta LLM-käännökseen ja lopulliseen tulosteeseen. Katso [tekninen dokumentaatio](../translation_entry_pipeline_zh-hans.md) lisätietoja varten.

**Apuskriptit (`.github/`)**: Käytetään GitHub-automaatioon.

Jos haluat:

* Korjata virheitä olemassa olevissa putkimoduuleissa tai skripteissä;
* Lisätä uusia ominaisuuksia tai moduuleja putkeen;
* Optimoida suorituskykyä tai koodirakennetta;
* Parantaa prompt-mallipohjia tai RAG-strategioita;

Voit noudattaa näitä vaiheita:

1. Forkkaa tämä tietovarasto ja kloonaa se paikallisesti;
2. Luo uusi haara uusimmasta haarasta;
3. Muokkaa tai lisää tiedostoja vastaaviin hakemistoihin:
   - Putkimoduulien muutokset → `src/<moduulin_nimi>/`;
   - Skriptimuutokset → `scripts/`;
   - Prompt-mallipohjien muutokset → `src/prompt_templates/`;
4. Ennen lähettämistä, yritä:

   * Säilyttää olemassa oleva koodityyli;
   * Lisätä tarvittavat kommentit;
   * Jos mahdollista, liittää mukaan yksinkertaiset testit tai käyttöohjeet;
5. Lähetä muutokset PR:n kautta ja selitä kuvauksessa:

   * Muutosten tarkoitus;
   * Hakemistot / moduulit / skriptit, joihin voi olla vaikutusta;
   * Sisältääkö se rikkovia muutoksia.

---

## Tekijänoikeudet ja lisensointi

> **Ystävällinen muistutus:**
> Tekijänoikeus- ja lisenssiehdot on suunniteltu suojaamaan projektin, tekijöiden, osallistujien ja pelaajien laillisia oikeuksia ja etuja sekä välttämään väärinkäsityksiä, jotka johtuvat "hiljaisista sopimuksista" tai "oletusarvoisista olettamuksista". Lue ne huolellisesti.
> Tekijänoikeudet ja lisensointi määräytyvät README.md-tiedoston sisällön mukaan; tämä osio tarjoaa vain helpommin lähestyttävän kuvauksen.

### 1. Perusperiaate: Säilytät tekijänoikeudet, samalla kun lisensoit projektin käyttämään työtäsi

* Sinulla on edelleen tekijänoikeus luomaasi sisältöön (käännökset, kuvat, skriptit/ohjelmat jne.);
* Kuitenkin, kun tämä sisältö on lähetetty tähän projektiin ja hyväksytty (yhdistetty),
  hyväksyt lisensoivasi muille tämän sisällön käytön tämän projektin hyväksymän avoimen lähdekoodin/jaetun lisenssin mukaisesti.

Tämä tarkoittaa:

* **Voit edelleen** jatkaa työsi käyttöä ja esittämistä muualla;
* Mutta **et voi** panoksesi yhdistämisen jälkeen vaatia tätä projektia tai muita käyttäjiä, jotka ovat laillisesti saaneet työn, "peruuttamaan lisenssiä" tai "poistamaan historiallisia versioita".

### 2. Tekstien, kuvien ja vastaavan sisällön lisensointi (CC BY-NC-SA 4.0)

Seuraavalle lähettämällesi sisällölle:

* Pelitekstien käännökset, tyylilliset korjaukset ja oikoluku;
* Projektin dokumentaatio ja selittävät tekstit;
* Erityisesti tätä projektia varten luodut kuvat ja taiteelliset resurssit;

Kun se on hyväksytty ja yhdistetty tähän tietovarastoon, katsotaan, että hyväksyt seuraavaa:

1. Tämä sisältö on lisensoitu **Nimeä-EiKaupallinen-JaaSamoin 4.0 Kansainvälinen**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, lyhennettynä **CC BY-NC-SA 4.0**) -lisenssillä;
2. Project Babel ja kaikki tämän sisällön vastaanottavat käyttäjät voivat **CC BY-NC-SA 4.0 -ehtoja noudattaen**:

   * Jakaa, kopioida ja edelleenjakaa tätä sisältöä;
   * Muokata sitä ja luoda johdannaisteoksia ei-kaupallisiin tarkoituksiin;
3. Hyväksyt, että sovellettavan lain sallimissa rajoissa tämä lisenssi on **ei-yksinomainen, maailmanlaajuinen, rojaltivapaa ja peruuttamaton**;
4. Vaikka myöhemmin vetäytyisit tai lopettaisit osallistumisesi tähän projektiin, projekti voi jatkaa lähettämäsi ja yhdistetyn asiaankuuluvan sisällön käyttöä ja edelleenjakamista CC BY-NC-SA 4.0:n mukaisesti.

> Jos et hyväksy yllä olevia lisenssiehtoja, älä lähetä teksti- tai kuvapanoksia tähän projektiin,
> tai ota etukäteen yhteyttä projektin ylläpitäjiin vahvistaaksesi, onko yhteistyö mahdollista muulla tavoin.

### 3. Skriptien ja työkalukoodin lisensointi (GPL-3.0)

Seuraavalle, jonka lähetät ja joka hyväksytään:

* Automaatioskriptit;
* Build/vientityökalut;
* Muu ohjelmakoodi, jota käytetään tämän käännösprojektin käsittelyyn;

Erityisten ilmoitusten puuttuessa katsotaan, että hyväksyt seuraavaa:

1. Koodi on lisensoitu **GPL-3.0**:lla (GNU General Public License versio 3);
2. Projektin ylläpitäjät voivat muokata, yhdistää ja jakaa sitä GPL-3.0:n sallimissa rajoissa;
3. Voit myös jatkaa muita projekteja saman koodin pohjalta, kunhan noudatat GPL-3.0:n ehtoja.

Lisenssiristiriitojen välttämiseksi yritä:

* Olla tuomatta kolmannen osapuolen koodia, joka on **yhteensopimaton GPL-3.0:n kanssa**, ilman ennakkovahvistusta;
* Jos sinun on viitattava kolmannen osapuolen kirjastoihin, ilmoita selvästi niiden lähde ja lisenssi PR:ssä ja vahvista yhteensopivuus.

### 4. Ylävirran teokset ja alkuperäisen pelin tekijänoikeudet

Tämä projekti on *Project Zomboidiin* liittyvien modien **epävirallinen käännösprojekti**:

* Alkuperäisen pelin ja kunkin modin tekijänoikeudet kuuluvat niiden omille tekijöille/julkaisijoille;
* Tämä projekti käsittää vain tekstikäännösten, tyylillisten muokkausten ja joidenkin oheisresurssien luomisen ja järjestämisen;
* Osallistujien on sisältöä lähettäessään varmistettava:

  * Että eivät kopioi suoraan luvattomia kolmannen osapuolen käännöstekstejä tai taiteellisia resursseja;
  * Että kunnioittavat alkuperäisten tekijöiden ja modien tekijöiden oikeuksia eivätkä harjoita oikeuksia loukkaavaa edelleenjakelua.

---

## Viestintä ja yhteistyö

Jos sinulla on:

* Kysymyksiä lisenssiehdoista;
* Epävarmuutta siitä, voidaanko tiettyä sisältöä tarjota;
* Halu lisensoida työsi erityisellä tavalla (esim. vain ei-kaupallinen käyttö ilman muokkausoikeutta);

Ota rohkeasti yhteyttä projektin ylläpitäjiin:

* Lähettämällä Issue keskustelua varten;
* Muiden ylläpitäjien julkisesti saatavilla olevien yhteystietojen kautta.

Teemme parhaamme löytääksemme ratkaisun, joka tasapainottaa projektin tervettä kehitystä kunnioittaen kaikkien osapuolten oikeuksia ja etuja.

---

## Taloudellinen tuki

Projektin toiminnan aikana uusien modien lisäämisen ja olemassa olevien modien tekstipäivitysten vuoksi LLM API:a on kutsuttava jatkuvasti käännöstä varten. LLM:n käyttäytymisen rajoittamiseksi tarvitaan modien perustekstien lisäksi suuri määrä prompt-sisältöä (mukaan lukien peruspromptit, käännössäännöt, termitaulukot, syöttö/tuloste-rajoitukset, semanttiset hakutulokset jne.), mikä kuluttaa huomattavasti enemmän tokeneita kuin alkuperäiset tekstit. Siksi projekti tarvitsee taloudellista tukea.

Jos haluat tarjota taloudellista tukea, ota yhteyttä projektin ylläpitäjiin. Paljon kiitoksia!

---

Vielä kerran kiitos halukkuudestasi osallistua tähän projektiin!
Jokainen panoksesi hyödyttää useampia pelaajia!
