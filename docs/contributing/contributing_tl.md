# Patnubay sa Pag-ambag (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Talaan ng Nilalaman

- [1. Bago Magsimula](#1-bago-magsimula)
- [2. Paano Ako Makatutulong?](#2-paano-ako-makatutulong)
- [3. Pagbibigay ng mga Panuntunan sa Pagsasalin, Diksyunaryo ng Terminolohiya, at Pagpapabuti ng mga Prompt ng Sistema](#3-pagbibigay-ng-mga-panuntunan-sa-pagsasalin-diksyunaryo-ng-terminolohiya-at-pagpapabuti-ng-mga-prompt-ng-sistema)
- [4. Magbigay ng manu-manong na-proofread na corpus](#4-magbigay-ng-manu-manong-na-proofread-na-corpus)
- [5. Pipeline at Tool Development Contributions](#5-pipeline-at-tool-development-contributions)
- [6. Copyright at Kasunduan ng Paglilisensiya](#6-copyright-at-kasunduan-ng-paglilisensiya)
  - [6.1 Pangunahing Prinsipyo: Nananatili ang iyong copyright, at binibigyan mo ng lisensya ang proyekto na gamitin ito](#61-pangunahing-prinsipyo-nananatili-ang-iyong-copyright-at-binibigyan-mo-ng-lisensya-ang-proyekto-na-gamitin-ito)
  - [6.2 Lisensya ng Teksto at mga Larawan atbp. (CC BY-NC-SA 4.0)](#62-lisensya-ng-teksto-at-mga-larawan-atbp-cc-by-nc-sa-40)
  - [6.3 Lisensya ng Script at Tool Code (GPL-3.0)](#63-lisensya-ng-script-at-tool-code-gpl-30)
  - [6.4 Copyright ng upstream works at orihinal na laro](#64-copyright-ng-upstream-works-at-orihinal-na-laro)
- [7. Komunikasyon at Pakikipagtulungan](#7-komunikasyon-at-pakikipagtulungan)
- [8. Suporta sa Pinansyal](#8-suporta-sa-pinansyal)

---

Maraming salamat sa iyong kagustuhang mag-ambag sa **Project Babel - LLM Automatic Translation Project para sa mod ng Project Zomboid**! Kahit ito ay pagwawasto ng pagkakamali, pagdagdag ng bagong feature, pagsulat ng prompt template, o pagbibigay ng sangguniang pagsasalin!

Ang pagtawag sa LLM API para sa pagsasalin ay nangangailangan ng pagbabayad para sa mga token. Upang ang proyekto ay maging matatag at pangmatagalan, sana ay maging mapagbigay ka!

> ⚠️ **Mahalagang Paalala:**
> Bago magsumite ng anuman sa repositoryong ito, mangyaring basahin at unawain ang seksyong "Kasunduan sa Karapatang-ari at Pahintulot".
> Kapag naisumite na at na-merge, ituturing ito bilang pagsang-ayon mo sa mga kaukulang tuntunin ng pahintulot.

---

## 1. Bago Magsimula

Mangyaring basahin muna ang `README.md` ng proyekto, upang maunawaan:
- Ang pangkalahatang layunin at kasalukuyang estado ng proyekto;
- Paano ginagamit ng mga ordinaryong manlalaro ang proyektong ito (para sa iyong pagsubok);
- Mga teknikal na detalye ng proyekto.

---

## 2. Paano Ako Makatutulong?

Maaari kang pumili ng isa o higit pang paraan upang makilahok, batay sa iyong interes at kakayahan:

- Magbigay ng mga panuntunan sa pagsasalin para sa target na wika
- Magbigay ng terminolohiyang diksyunaryo para sa pagsasalin sa target na wika
- Pagbutihin ang mga prompt ng sistema
- Magbigay ng mga tekstong korpus na may proofread na pagsasalin
- Pagbutihin ang mga module ng pipeline (.NET) at mga automation script
- Mag-ulat ng mga isyu, magmungkahi ng mga pagpapabuti (sa Issues)
- Magbigay ng pinansyal na suporta para sa pagtawag sa LLM

Sa ibaba, may ilang paliwanag tungkol sa mga pangunahing paraan ng pag-ambag.

---

## 3. Pagbibigay ng mga Panuntunan sa Pagsasalin, Diksyunaryo ng Terminolohiya, at Pagpapabuti ng mga Prompt ng Sistema

Ang mga template ng prompt para sa pipeline ay matatagpuan sa `src/prompt_templates/`, at ang istraktura ay ang sumusunod:

- `system_prompt_translate_engine.txt`: Global na prompt ng sistema para sa translation engine (ginagamit ng lahat ng wika);
- `<language_code>/translation_dictionary_<language_code>.json`: Diksyunaryo ng terminolohiya para sa wikang iyon;
- `<language_code>/translation_schema_<language_code>.md`: Mga panuntunan sa pagsasalin at mga hadlang sa estilo para sa wikang iyon.

Mga hakbang sa pag-ambag:

1. Sa ilalim ng `src/prompt_templates/`, lumikha ng subdirectory para sa iyong wika, at idagdag ang diksyunaryo ng terminolohiya at file ng mga panuntunan sa pagsasalin;
2. Kung kailangan ayusin ang pandaigdigang pag-uugali ng pagsasalin, baguhin ang `system_prompt_translate_engine.txt` (tandaan na makakaapekto ito sa lahat ng wika);
3. Kumpirmahin ang epekto sa lokal na pagsubok;
4. Isumite ang PR.

---

## 4. Magbigay ng manu-manong na-proofread na corpus

Kung ikaw ay isang tagapagsalin ng mod, at handang ibigay ang iyong translation corpus bilang LLM translation reference, mangyaring magsumite ng kahilingan sa Issue. Kailangan mong ibigay ang sumusunod na impormasyon:

- Ang Mod ID ng iyong translation mod at ang target na wika ng pagsasalin;
- Screenshot ng backend page ng iyong translation mod upang patunayan na ikaw ang mod author;
- Malinaw na ipahayag sa Issue na handa kang ibigay ang translation corpus;
- Kung may mga espesyal na sitwasyon (espesyal na lisensya, atbp.), mangyaring ipaliwanag din;
- Pakitiyak na ang corpus na iyong ibinibigay ay may mataas na kalidad.

Sa ilalim ng iyong awtorisasyon, ilalagay ng proyekto ang iyong mod sa `config/ref_translation_mods.json` reference translation mod list, at awtomatikong isi-sync ng pipeline ang iyong translation text bilang RAG reference corpus.

---

## 5. Pipeline at Tool Development Contributions

Ang automation ng proyektong ito ay nahahati sa dalawang bahagi:

**Pipeline module (`src/`, C# / .NET 10)**: Naglalaman ng 15 modules na sunud-sunod na isinasagawa, responsable para sa kumpletong proseso mula sa SteamCMD initialization, mod download, text extraction, content review, Embedding computation, RAG retrieval hanggang sa LLM translation at final output. Tingnan ang [teknikal na sanggunian](../technical_reference/technical_reference_tl.md).

**Auxiliary scripts (`.github/`)**: Ginagamit para sa automation ng GitHub.

Kung nais mo:

* Ayusin ang mga Bug sa kasalukuyang pipeline modules o scripts;
* Magdagdag ng bagong feature o bagong module sa pipeline;
* I-optimize ang performance o code structure;
* Pahusayin ang prompt template o RAG strategy;

Maaaring sundin ang mga sumusunod na hakbang:

1. Fork ang repository na ito at i-clone ito sa lokal;
2. Gumawa ng bagong branch batay sa pinakabagong branch;
3. Baguhin o magdagdag ng mga file sa kaukulang direktoryo:
- Pipeline module modification → `src/<模块名>/`;
- Script modification → `scripts/`;
- Prompt template modification → `src/prompt_templates/`;
4. Bago mag-ambag, pakiusap subukang:

* Panatilihin ang orihinal na style ng code;
* Magdagdag ng kinakailangang mga komento;
* Kung may kondisyon, mag-attach ng simpleng pagsubok o gabay sa paggamit;
5. Sa pamamagitan ng PR isumite ang pagbabago, at sa deskripsyon ay ipaliwanag:

* Layunin ng pagbabago;
* Mga direktoryo / module / script na maaaring maapektuhan;
* Kung may kinalaman sa breaker na pagbabago.

---

## 6. Copyright at Kasunduan ng Paglilisensiya

> **Paalala:**
> Ang kasunduan sa copyright at paglilisensiya ay para protektahan ang mga lehitimong karapatan ng proyekto, mga may-akda, mga kontribyutor, at mga manlalaro, upang maiwasan ang maling pagkaunawa dahil sa "pagkakasundo" o "default". Mangyaring basahin nang mabuti.
> Ang copyright at paglilisensiya ay batay sa nilalaman ng README.md file, ang seksyong ito ay nagbibigay lamang ng mas madaling maunawaang paglalarawan.

### 6.1 Pangunahing Prinsipyo: Nananatili ang iyong copyright, at binibigyan mo ng lisensya ang proyekto na gamitin ito

* May karapatan ka pa rin sa copyright ng iyong sariling nilikha (pagsasalin, larawan, script/programa, atbp.);
* Ngunit pagkatapos mong isumite ang mga ito sa proyektong ito at tanggapin (merge), sumasang-ayon ka na ibigay ang lisensya sa iba na gamitin ang mga ito ayon sa open-source/shared license na ginagamit ng proyektong ito.

Ibig sabihin nito:

* Maaari mo pa ring gamitin at ipakita ang iyong sariling gawa sa ibang lugar;
* Ngunit **hindi mo** maaaring hilingin sa proyektong ito o sa iba pang legal na nakakuha ng mga gawa na "bawiin ang lisensya" o "burahin ang mga lumang bersyon" pagkatapos ma-merge ang iyong kontribusyon.

### 6.2 Lisensya ng Teksto at mga Larawan atbp. (CC BY-NC-SA 4.0)

Para sa sumusunod na nilalaman na iyong isinumite:

* Pagsasalin ng teksto ng laro, pagpapabuti at pag-proofread;
* Dokumentasyon ng proyekto, mga paliwanag na teksto;
* Mga larawan at artistic resource na espesyal na nilikha para sa proyektong ito;

Kapag tinanggap at na-merge sa repositoryong ito, ituturing na sumasang-ayon ka na:

1. Ang mga nilalamang ito ay lisensyado sa ilalim ng **Attribution-NonCommercial-ShareAlike 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, pinaikling **CC BY-NC-SA 4.0**);
2. Ang Project Babel at lahat ng user na nakakuha ng nilalamang ito ay maaaring, sa ilalim ng **pagsunod sa mga tuntunin ng CC BY-NC-SA 4.0**:
* Ibahagi, kopyahin, at muling ipamahagi ang mga nilalamang ito;
* Baguhin at muling likhain ang mga ito sa ilalim ng di-komersyal na paggamit;
3. Sumasang-ayon ka na, sa lawak na pinapayagan ng naaangkop na batas, ang lisensyang ito ay isang **di-eksklusibo, pandaigdigan, walang royalty, at hindi mababawi** na pahintulot;
4. Kahit na umalis ka o huminto sa paglahok sa proyektong ito sa hinaharap, ang proyektong ito ay maaaring patuloy na gamitin at muling ipalathala ang iyong isinumite at na-merge na kaugnay na nilalaman alinsunod sa CC BY-NC-SA 4.0.

> Kung hindi mo tinatanggap ang nabanggit na paraan ng paglilisensya, huwag magsumite ng teksto o larawang kontribusyon sa proyektong ito,
> o makipag-ugnayan nang maaga sa tagapangalaga ng proyekto upang kumpirmahin kung maaari kang makipagtulungan sa ibang paraan.

### 6.3 Lisensya ng Script at Tool Code (GPL-3.0)

Para sa iyong isinumite at tinanggap:

* Mga automated na script;
* Mga tool sa pagbuo/pag-export;
* Iba pang code ng programa para sa paghawak ng pagsasaling ito;

Sa kawalan ng espesyal na pahayag, itinuturing mong sumang-ayon ka:

1. Ang code ay lisensyado sa ilalim ng **GPL-3.0** (GNU General Public License version 3);
2. Ang tagapangalaga ng proyekto ay maaaring magbago, pagsamahin, at ipamahagi ito sa loob ng saklaw na pinapayagan ng GPL-3.0;
3. Maaari ka ring magpatuloy sa iba pang mga proyekto batay sa parehong code, basta sumunod sa mga tuntunin ng GPL-3.0.

Upang maiwasan ang mga salungatan sa lisensya, subukang:

* Huwag magpakilala ng third-party code na **hindi tugma sa GPL-3.0** nang walang kumpirmasyon;
* Kung kailangan talagang sumangguni sa isang third-party library, malinaw na ipaliwanag ang pinagmulan at lisensya nito sa PR, at kumpirmahin ang pagiging tugma nito.

### 6.4 Copyright ng upstream works at orihinal na laro

Ang proyektong ito ay isang **hindi opisyal na pagsasalin** ng mga mod na may kaugnayan sa "Project Zomboid":

* Ang copyright ng orihinal na laro at bawat mod ay pagmamay-ari ng kani-kanilang may-akda/tagapaglathala;
* Ang proyektong ito ay lumilikha at nag-oorganisa lamang ng pagsasalin ng teksto, pagsasaayos ng polish, at ilang kasamang mapagkukunan;
* Kapag nagsusumite ng nilalaman, dapat tiyakin ng kontribyutor na:
* Huwag direktang kopyahin ang hindi awtorisadong third-party na isinalin na teksto o artistikong mapagkukunan;
* Igalang ang mga karapatan ng orihinal na may-akda at mod author, at huwag mag-repost na lumalabag.

---

## 7. Komunikasyon at Pakikipagtulungan

Kung ikaw ay may katanungan tungkol sa:

* Mga tuntunin sa lisensya;
* Hindi sigurado kung ang isang partikular na nilalaman ay maaaring i-ambag;
* Nais na lisensyahan ang iyong gawa sa isang espesyal na paraan (halimbawa, pinapayagan lamang ang di-komersyal ngunit hindi ang pagbabago, atbp.);

Malugod naming tinatanggap ang pakikipag-ugnayan sa tagapangalaga ng proyekto sa pamamagitan ng:

* Magsumite ng Issue para talakayin;
* Iba pang mga paraan ng pakikipag-ugnayan na ibinibigay ng tagapangalaga.

Susubukan naming makahanap ng solusyon na isinasaalang-alang ang malusog na pag-unlad ng proyekto habang iginagalang ang mga karapatan ng lahat ng partido.

---

## 8. Suporta sa Pinansyal

Sa pagpapatakbo ng proyekto, dahil sa mga bagong mod at pag-update ng nilalaman ng teksto sa mga lumang mod, kailangan ng patuloy na pagtawag sa LLM API para sa pagsasalin. At upang pigilan ang pag-uugali ng LLM, bukod sa pangunahing teksto ng mod, kailangan ding magbigay ng maraming nilalaman ng prompt (kabilang ang pangunahing prompt, mga panuntunan sa pagsasalin, glossary, mga hadlang sa input/output, mga resulta ng semantic query, atbp.), na kumukonsumo ng mga token na higit pa sa orihinal na teksto. Samakatuwid, ang proyekto ay nangangailangan ng suportang pinansyal.

Kung nais mong magbigay ng suportang pinansyal, makipag-ugnayan sa tagapangalaga ng proyekto. Maraming salamat!

---

Muli, salamat sa iyong kahandaang mag-ambag sa proyektong ito!
Ang bawat ambag mo ay magbibigay-daan sa mas maraming manlalaro na makinabang!
