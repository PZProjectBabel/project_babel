# Gabay sa Pag-aambag (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Maraming salamat sa iyong kagustuhang mag-ambag sa **Project Babel — ang proyektong awtomatikong pagsasalin gamit ang LLM para sa mga mod ng Project Zomboid**! Pag-aayos man ng bug, pagdaragdag ng feature, pagsusulat ng mga prompt template, o pagbibigay ng mga sangguniang pagsasalin — mahalaga ang bawat ambag!

Ang pagtawag sa LLM API para sa pagsasalin ay may gastos sa token. Upang mapanatiling tuluy-tuloy ang proyekto sa mahabang panahon, lubos na pinahahalagahan ang iyong mapagbigay na suporta!

> ⚠️ **Mahalagang Paalala:**
> Bago magsumite ng anuman sa repositoryong ito, tiyaking basahin at unawain ang seksyong "Karapatang-ari at Paglilisensya".
> Sa sandaling maisumite at ma-merge, itinuturing kang sumang-ayon sa kaukulang mga tuntunin ng lisensya.

---

## Bago Ka Magsimula

Mangyaring basahin ang `README.md` ng proyekto upang maunawaan:

- Ang pangkalahatang layunin at kasalukuyang kalagayan ng proyektong ito;
- Paano ginagamit ng mga ordinaryong manlalaro ang proyektong ito (para sa iyong sariling pagsusuri);
- Mga teknikal na detalye ng proyekto.

---

## Paano Ako Makakaambag?

Maaari kang pumili ng isa o higit pang paraan upang lumahok batay sa iyong mga interes at kasanayan:

- Magbigay ng mga panuntunan sa pagsasalin para sa isang target na wika
- Magbigay ng isang diksyunaryo ng terminolohiya para sa isang target na wika
- Pagbutihin ang mga prompt ng sistema
- Magbigay ng mga manu-manong naitamang korpora ng pagsasalin
- Pagbutihin ang mga module ng pipeline (.NET) at mga script ng awtomatisasyon
- Mag-ulat ng mga isyu at magmungkahi ng mga pagpapabuti (sa pamamagitan ng Issues)
- Magbigay ng pinansyal na suporta para sa mga tawag sa LLM API

Narito ang mga paliwanag para sa mga pangunahing senaryo ng pag-aambag.

---

## Pagbibigay ng Mga Panuntunan sa Pagsasalin, Mga Diksyunaryo ng Terminolohiya, at Pagpapabuti ng Mga Prompt ng Sistema

Ang mga prompt template ng pipeline ay matatagpuan sa `src/prompt_templates/`, na may sumusunod na istraktura:

- `system_prompt_translate_engine.txt`: ang pandaigdigang prompt ng sistema ng engine ng pagsasalin (ginagamit ng lahat ng wika);
- `<code_ng_wika>/translation_dictionary_<code_ng_wika>.json`: ang diksyunaryo ng terminolohiya para sa wikang iyon;
- `<code_ng_wika>/translation_schema_<code_ng_wika>.md`: ang mga panuntunan sa pagsasalin at mga hadlang sa istilo para sa wikang iyon.

Mga hakbang sa pag-aambag:

1. Gumawa ng subdirectory sa ilalim ng `src/prompt_templates/` para sa iyong wika at idagdag ang mga file ng diksyunaryo at panuntunan;
2. Kung kailangan mong ayusin ang pandaigdigang pag-uugali ng pagsasalin, baguhin ang `system_prompt_translate_engine.txt` (tandaan: nakakaapekto ito sa lahat ng wika);
3. Subukan nang lokal upang kumpirmahin ang mga resulta;
4. Magsumite ng PR.

---

## Pagbibigay ng Mga Manu-manong Naitamang Korpora

Kung ikaw ay may-akda ng isang mod ng pagsasalin at handang ibigay ang iyong korpus ng pagsasalin bilang sanggunian sa pagsasalin ng LLM, mangyaring magsumite ng kahilingan sa pamamagitan ng isang Issue. Kailangan mong ibigay ang sumusunod na impormasyon:

- Ang Mod ID ng iyong mod ng pagsasalin at ang target na wika;
- Isang screenshot ng pahina ng administrasyon ng iyong mod ng pagsasalin upang patunayan na ikaw ang may-akda;
- Isang malinaw na pahayag sa Issue na handa kang ibigay ang korpus ng pagsasalin;
- Kung may mga espesyal na pangyayari (espesyal na lisensya, atbp.), mangyaring ipaliwanag;
- Tiyaking ang ibibigay na korpus ay may mataas na kalidad.

Sa iyong pahintulot, idaragdag ng proyekto ang iyong mod sa listahan ng mga sangguniang mod ng pagsasalin `config/ref_translation_mods.json`, at awtomatikong isi-sync ng pipeline ang iyong mga isinaling teksto bilang mga sangguniang korpora ng RAG.

---

## Mga Ambag sa Pagpapaunlad ng Pipeline at Mga Kasangkapan

Ang awtomatisasyon sa proyektong ito ay nahahati sa dalawang bahagi:

**Mga module ng pipeline (`src/`, C# / .NET 10)**: Naglalaman ng 15 sunud-sunod na isinasagawang module, na responsable para sa kumpletong daloy ng trabaho mula sa pag-download ng mod, pagkuha ng teksto, pagsusuri ng nilalaman, pagkalkula ng embedding, pagkuha ng RAG hanggang sa pagsasalin ng LLM at panghuling output. Tingnan ang [sangguniang teknikal](../technical_reference/technical_reference_tl.md) para sa mga detalye.

**Mga pantulong na script (`.github/`)**: Ginagamit para sa awtomatisasyon ng GitHub.

Kung nais mong:

* Ayusin ang mga bug sa mga umiiral na module ng pipeline o mga script;
* Magdagdag ng mga bagong feature o module sa pipeline;
* I-optimize ang pagganap o istraktura ng code;
* Pagbutihin ang mga prompt template o mga estratehiya ng RAG;

Maaari mong sundin ang mga hakbang na ito:

1. I-fork ang repositoryong ito at i-clone nang lokal;
2. Gumawa ng bagong branch mula sa pinakabagong branch;
3. Baguhin o magdagdag ng mga file sa mga kaukulang direktoryo:
   - Mga pagbabago sa module ng pipeline → `src/<pangalan_ng_module>/`;
   - Mga pagbabago sa script → `scripts/`;
   - Mga pagbabago sa prompt template → `src/prompt_templates/`;
4. Bago magsumite, subukang:

   * Panatilihin ang umiiral na istilo ng code;
   * Magdagdag ng mga kinakailangang komento;
   * Kung maaari, magsama ng mga simpleng pagsubok o mga tagubilin sa paggamit;
5. Magsumite ng mga pagbabago sa pamamagitan ng PR, at ipaliwanag sa paglalarawan:

   * Ang layunin ng mga pagbabago;
   * Ang mga direktoryo / module / script na maaaring maapektuhan;
   * Kung may kinalaman ito sa mga nakasisirang pagbabago.

---

## Karapatang-ari at Paglilisensya

> **Magiliw na Paalala:**
> Ang mga tuntunin ng karapatang-ari at paglilisensya ay idinisenyo upang protektahan ang mga lehitimong karapatan at interes ng proyekto, mga may-akda, mga nag-aambag, at mga manlalaro, at upang maiwasan ang mga hindi pagkakaunawaan na nagmumula sa "mga lihim na kasunduan" o "mga default na palagay". Mangyaring basahin ito nang mabuti.
> Ang karapatang-ari at paglilisensya ay pinamamahalaan ng nilalaman ng file na README.md; ang seksyong ito ay nagbibigay lamang ng mas madaling maunawaang paglalarawan.

### 1. Pangunahing Prinsipyo: Pinananatili mo ang karapatang-ari, habang nililisensyahan ang proyekto na gamitin ang iyong gawa

* Mayroon ka pa ring karapatang-ari sa nilalamang iyong nilikha (mga pagsasalin, larawan, script/programa, atbp.);
* Gayunpaman, kapag ang nilalamang ito ay naisumite sa proyektong ito at tinanggap (na-merge),
  sumasang-ayon kang lisensyahan ang iba na gamitin ang nilalamang ito sa ilalim ng lisensyang open-source/ibinahaging pinagtibay ng proyektong ito.

Ito ay nangangahulugan na:

* **Maaari mo pa ring** ipagpatuloy ang paggamit at pagpapakita ng iyong gawa sa ibang lugar;
* Ngunit **hindi mo maaari**, pagkatapos ma-merge ang iyong ambag, na hilingin sa proyektong ito o sa ibang mga gumagamit na legal na nakakuha ng gawa na "bawiin ang lisensya" o "burahin ang mga makasaysayang bersyon".

### 2. Paglilisensya ng Mga Teksto, Larawan, at Katulad na Nilalaman (CC BY-NC-SA 4.0)

Para sa sumusunod na nilalaman na iyong isusumite:

* Mga pagsasalin ng teksto ng laro, pagpapakinis, at pagwawasto;
* Dokumentasyon ng proyekto at mga tekstong nagpapaliwanag;
* Mga larawan at artistikong mapagkukunan na partikular na nilikha para sa proyektong ito;

Kapag tinanggap at na-merge sa repositoryong ito, itinuturing kang sumasang-ayon na:

1. Ang nilalamang ito ay nilisensyahan sa ilalim ng **Pagkilala-Hindi Komersyal-Pagbabahagi sa Parehong Paraan 4.0 Pandaigdig**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, dinaglat na **CC BY-NC-SA 4.0**);
2. Ang Project Babel at lahat ng mga gumagamit na tumatanggap ng nilalamang ito ay maaari, **sa pagsunod sa mga tuntunin ng CC BY-NC-SA 4.0**:

   * Magbahagi, kumopya, at muling ipamahagi ang nilalamang ito;
   * Baguhin ito at lumikha ng mga hinalaw na gawa para sa mga di-komersyal na layunin;
3. Sumasang-ayon ka na sa saklaw na pinapayagan ng naaangkop na batas, ang lisensyang ito ay **hindi eksklusibo, pandaigdigan, walang royalty, at hindi mababawi**;
4. Kahit na sa kalaunan ay umatras ka o tumigil sa paglahok sa proyektong ito, maaaring ipagpatuloy ng proyekto ang paggamit at muling pamamahagi ng kaugnay na nilalaman na iyong isinumite at na-merge, sa ilalim ng CC BY-NC-SA 4.0.

> Kung hindi mo tinatanggap ang mga nabanggit na tuntunin ng lisensya, mangyaring huwag magsumite ng mga ambag na teksto o larawan sa proyektong ito,
> o makipag-ugnayan muna sa mga tagapangasiwa ng proyekto upang kumpirmahin kung posible ang pakikipagtulungan sa ibang paraan.

### 3. Paglilisensya ng Mga Script at Code ng Kasangkapan (GPL-3.0)

Para sa mga sumusunod na isusumite mo at tatanggapin:

* Mga script ng awtomatisasyon;
* Mga kasangkapan sa build/pag-export;
* Iba pang code ng programa na ginagamit para sa pagproseso ng proyektong ito ng pagsasalin;

Sa kawalan ng mga espesyal na pahayag, itinuturing kang sumasang-ayon na:

1. Ang code ay nilisensyahan sa ilalim ng **GPL-3.0** (GNU General Public License bersyon 3);
2. Maaaring baguhin, i-merge, at ipamahagi ito ng mga tagapangasiwa ng proyekto sa loob ng saklaw na pinapayagan ng GPL-3.0;
3. Maaari mo ring ipagpatuloy ang iba pang mga proyekto batay sa parehong code, hangga''t sumusunod ka sa mga tuntunin ng GPL-3.0.

Upang maiwasan ang mga salungatan sa lisensya, subukang:

* Huwag magpakilala ng code ng ikatlong partido na **hindi tugma sa GPL-3.0** nang walang naunang kumpirmasyon;
* Kung kailangan mong sumangguni sa mga library ng ikatlong partido, malinaw na sabihin ang kanilang pinagmulan at lisensya sa PR at kumpirmahin ang pagiging tugma.

### 4. Mga Naunang Gawa at Karapatang-ari ng Orihinal na Laro

Ang proyektong ito ay isang proyekto ng **hindi opisyal na pagsasalin** para sa mga mod na may kaugnayan sa *Project Zomboid*:

* Ang karapatang-ari ng orihinal na laro at ng bawat mod ay pagmamay-ari ng kani-kanilang mga may-akda/tagapaglathala;
* Ang proyektong ito ay sumasaklaw lamang sa paglikha at pag-aayos ng mga pagsasalin ng teksto, mga pagsasaayos ng istilo, at ilang mga kaakibat na mapagkukunan;
* Dapat tiyakin ng mga nag-aambag, sa pagsusumite ng nilalaman:

  * Na hindi direktang kumopya ng hindi awtorisadong mga teksto ng pagsasalin o artistikong mapagkukunan ng ikatlong partido;
  * Na igalang ang mga karapatan ng orihinal na mga may-akda at mga may-akda ng mod, at huwag magsagawa ng lumalabag na muling pamamahagi.

---

## Komunikasyon at Pakikipagtulungan

Kung mayroon kang:

* Mga tanong tungkol sa mga tuntunin ng lisensya;
* Kawalan ng katiyakan kung ang isang partikular na nilalaman ay maaaring iambag;
* Pagnanais na lisensyahan ang iyong gawa sa isang espesyal na paraan (hal., gamit pang hindi pangkomersyal lamang nang walang pinapayagang pagbabago);

Huwag mag-atubiling makipag-ugnayan sa mga tagapangasiwa ng proyekto sa pamamagitan ng:

* Pagsumite ng isang Issue para sa talakayan;
* Iba pang pampublikong magagamit na paraan ng pakikipag-ugnayan ng mga tagapangasiwa.

Gagawin namin ang aming makakaya upang makahanap ng solusyon na nagbabalanse sa malusog na pag-unlad ng proyekto habang iginagalang ang mga karapatan at interes ng lahat ng partido.

---

## Pinansyal na Suporta

Sa panahon ng operasyon ng proyekto, dahil sa pagdaragdag ng mga bagong mod at pag-update ng teksto ng mga umiiral na mod, ang LLM API ay kailangang patuloy na tawagan para sa pagsasalin. Upang hadlangan ang pag-uugali ng LLM, bilang karagdagan sa mga batayang teksto ng mod, kailangan ang malaking halaga ng nilalaman ng prompt (kabilang ang mga batayang prompt, panuntunan sa pagsasalin, mga talahanayan ng termino, mga hadlang sa input/output, mga resulta ng semantikong paghahanap, atbp.), na kumokonsumo ng mas maraming token kaysa sa mga orihinal na teksto. Samakatuwid, ang proyekto ay nangangailangan ng pinansyal na suporta.

Kung nais mong magbigay ng pinansyal na suporta, mangyaring makipag-ugnayan sa mga tagapangasiwa ng proyekto. Maraming salamat!

---

Muli, salamat sa iyong kagustuhang mag-ambag sa proyektong ito!
Ang bawat ambag mo ay nakikinabang sa mas maraming manlalaro!
