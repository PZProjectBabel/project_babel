# Katkı Rehberi (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [українська](contributing_uk.md)</details>

---

**Project Babel — Project Zomboid modları için LLM destekli otomatik çeviri projesine** katkıda bulunma isteğiniz için teşekkür ederiz! İster bir hata düzeltmesi, ister yeni bir özellik eklemesi, ister prompt şablonları yazımı, isterse referans çeviriler sağlama olsun — her katkı değerlidir!

LLM API''sini çeviri için çağırmak token maliyeti getirir. Projenin uzun vadede sürdürülebilir şekilde çalışabilmesi için cömert desteğiniz büyük takdir görmektedir!

> ⚠️ **Önemli Uyarı:**
> Bu depoya herhangi bir şey göndermeden önce lütfen "Telif Hakkı ve Lisanslama" bölümünü okuyup anlayın.
> Gönderildikten ve birleştirildikten sonra, ilgili lisans koşullarını kabul etmiş sayılırsınız.

---

## Başlamadan Önce

Lütfen aşağıdakileri anlamak için projenin `README.md` dosyasını okuyun:

- Bu projenin genel hedefleri ve mevcut durumu;
- Sıradan oyuncuların bu projeyi nasıl kullandığı (kendi testleriniz için);
- Projenin teknik detayları.

---

## Nasıl Katkıda Bulunabilirim?

İlgi alanlarınıza ve becerilerinize göre bir veya daha fazla şekilde katılabilirsiniz:

- Bir hedef dil için çeviri kuralları sağlama
- Bir hedef dil için terim sözlüğü sağlama
- Sistem promptlarını iyileştirme
- Manuel olarak düzeltilmiş çeviri derlemleri sağlama
- Pipeline modüllerini (.NET) ve otomasyon betiklerini iyileştirme
- Sorunları bildirme ve iyileştirmeler önerme (Issues aracılığıyla)
- LLM API çağrıları için mali destek sağlama

Aşağıda ana katkı senaryoları için açıklamalar bulunmaktadır.

---

## Çeviri Kuralları, Terim Sözlükleri Sağlama ve Sistem Promptlarını İyileştirme

Pipeline''ın prompt şablonları `src/prompt_templates/` konumunda bulunur ve aşağıdaki yapıya sahiptir:

- `system_prompt_translate_engine.txt`: küresel çeviri motoru sistem promptu (tüm diller ortak);
- `<dil_kodu>/translation_dictionary_<dil_kodu>.json`: o dil için terim sözlüğü;
- `<dil_kodu>/translation_schema_<dil_kodu>.md`: o dil için çeviri kuralları ve stil kısıtlamaları.

Katkı adımları:

1. Diliniz için `src/prompt_templates/` altında bir alt dizin oluşturun ve sözlük ile kural dosyalarını ekleyin;
2. Küresel çeviri davranışını ayarlamanız gerekiyorsa, `system_prompt_translate_engine.txt` dosyasını değiştirin (not: bu tüm dilleri etkiler);
3. Sonuçları doğrulamak için yerel olarak test edin;
4. Bir PR gönderin.

---

## Manuel Olarak Düzeltilmiş Derlemler Sağlama

Bir çeviri modu yazarıysanız ve çeviri derleminizi LLM çeviri referansı olarak sağlamaya istekliyseniz, lütfen bir Issue aracılığıyla başvuruda bulunun. Aşağıdaki bilgileri sağlamanız gerekmektedir:

- Çeviri modunuzun Mod ID''si ve hedef dil;
- Mod yazarı olduğunuzu kanıtlamak için çeviri modunuzun yönetim sayfasının ekran görüntüsü;
- Issue''da çeviri derlemini sağlamaya istekli olduğunuza dair açık bir beyan;
- Özel durumlar varsa (özel lisans vb.), lütfen açıklayın;
- Lütfen sağladığınız derlemin yüksek kalitede olduğundan emin olun.

Yetkinizle, proje modunuzu `config/ref_translation_mods.json` referans çeviri modları listesine ekleyecek ve pipeline, çevrilmiş metinlerinizi otomatik olarak RAG referans derlemleri olarak senkronize edecektir.

---

## Pipeline ve Araç Geliştirme Katkıları

Bu projedeki otomasyon iki bölüme ayrılmıştır:

**Pipeline modülleri (`src/`, C# / .NET 10)**: Mod indirme, metin çıkarma, içerik incelemesi, embedding hesaplama, RAG getirme işleminden LLM çevirisine ve nihai çıktıya kadar tüm iş akışından sorumlu, sıralı olarak yürütülen 15 modül içerir. Ayrıntılar için [teknik referansa](../technical_reference/technical_reference_tr.md) bakın.

**Yardımcı betikler (`.github/`)**: GitHub otomasyonu için kullanılır.

Eğer şunları yapmak isterseniz:

* Mevcut pipeline modüllerindeki veya betiklerdeki hataları düzeltmek;
* Pipeline''a yeni özellikler veya modüller eklemek;
* Performansı veya kod yapısını optimize etmek;
* Prompt şablonlarını veya RAG stratejilerini iyileştirmek;

Bu adımları takip edebilirsiniz:

1. Bu depoyu fork''layın ve yerel olarak klonlayın;
2. En son daldan yeni bir dal oluşturun;
3. İlgili dizinlerdeki dosyaları değiştirin veya ekleyin:
   - Pipeline modülü değişiklikleri → `src/<modül_adı>/`;
   - Betik değişiklikleri → `scripts/`;
   - Prompt şablonu değişiklikleri → `src/prompt_templates/`;
4. Göndermeden önce, lütfen şunlara dikkat edin:

   * Mevcut kod stilini koruyun;
   * Gerekli yorumları ekleyin;
   * Mümkünse, basit testler veya kullanım talimatları ekleyin;
5. Değişiklikleri PR ile gönderin ve açıklamada şunları belirtin:

   * Değişikliklerin amacı;
   * Etkilenebilecek dizinler / modüller / betikler;
   * Geriye dönük uyumluluğu bozan değişiklikler içerip içermediği.

---

## Telif Hakkı ve Lisanslama

> **Dostça Hatırlatma:**
> Telif hakkı ve lisanslama koşulları, projenin, yazarların, katkıda bulunanların ve oyuncuların meşru hak ve çıkarlarını korumak ve "zımni anlaşmalar" veya "varsayılan kabullerden" kaynaklanan yanlış anlamaları önlemek için tasarlanmıştır. Lütfen dikkatlice okuyun.
> Telif hakkı ve lisanslama, README.md dosyasındaki içeriğe tabidir; bu bölüm yalnızca daha anlaşılır bir açıklama sunmaktadır.

### 1. Temel İlke: Telif hakkını saklı tutarken, projeye eserinizi kullanma lisansı verirsiniz

* Oluşturduğunuz içeriğin (çeviriler, resimler, betikler/programlar vb.) telif hakkı hâlâ size aittir;
* Ancak, bu içerik bu projeye gönderilip kabul edildikten (birleştirildikten) sonra,
  bu içeriğin başkaları tarafından bu projenin benimsediği açık kaynak/paylaşımlı lisans altında kullanılmasına lisans vermiş olursunuz.

Bu şu anlama gelir:

* Eserinizi başka yerlerde kullanmaya ve sergilemeye **devam edebilirsiniz**;
* Ancak katkınız birleştirildikten sonra, bu projeden veya eseri yasal olarak edinmiş diğer kullanıcılardan "lisansı iptal etmelerini" veya "geçmiş sürümleri silmelerini" **talep edemezsiniz**.

### 2. Metin, Resim ve Benzeri İçeriklerin Lisanslanması (CC BY-NC-SA 4.0)

Gönderdiğiniz aşağıdaki içerikler için:

* Oyun metni çevirileri, düzeltmeler ve son okumalar;
* Proje dokümantasyonu ve açıklayıcı metinler;
* Bu proje için özel olarak oluşturulmuş resimler ve sanatsal kaynaklar;

Bu depoda kabul edilip birleştirildiğinde, aşağıdakileri kabul etmiş sayılırsınız:

1. Bu içerikler **Atıf-GayriTicari-AynıLisanslaPaylaş 4.0 Uluslararası**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, kısaca **CC BY-NC-SA 4.0**) lisansı altındadır;
2. Project Babel ve bu içeriği alan tüm kullanıcılar, **CC BY-NC-SA 4.0 koşullarına uymak kaydıyla**:

   * Bu içeriği paylaşabilir, kopyalayabilir ve yeniden dağıtabilir;
   * Ticari olmayan amaçlarla değiştirebilir ve türev eserler oluşturabilir;
3. Bu lisansın, geçerli yasaların izin verdiği ölçüde **münhasır olmayan, dünya çapında, telifsiz ve geri alınamaz** olduğunu kabul edersiniz;
4. Daha sonra projeden ayrılsanız veya katılımınızı durdursanız bile, proje gönderdiğiniz ve birleştirilmiş ilgili içeriği CC BY-NC-SA 4.0 kapsamında kullanmaya ve yeniden yayınlamaya devam edebilir.

> Yukarıdaki lisans koşullarını kabul etmiyorsanız, lütfen bu projeye metin veya resim katkısı göndermeyin,
> veya işbirliğinin başka bir şekilde mümkün olup olmadığını teyit etmek için proje bakımcılarıyla önceden iletişime geçin.

### 3. Betiklerin ve Araç Kodunun Lisanslanması (GPL-3.0)

Gönderdiğiniz ve kabul edilen aşağıdakiler için:

* Otomasyon betikleri;
* Derleme/dışa aktarma araçları;
* Bu çeviri projesini işlemek için kullanılan diğer program kodu;

Özel beyanların bulunmaması durumunda, aşağıdakileri kabul etmiş sayılırsınız:

1. Kod **GPL-3.0** (GNU Genel Kamu Lisansı sürüm 3) altında lisanslanmıştır;
2. Proje bakımcıları, GPL-3.0''ın izin verdiği kapsamda kodu değiştirebilir, birleştirebilir ve dağıtabilir;
3. Siz de GPL-3.0 koşullarına uymak kaydıyla aynı koda dayalı diğer projelere devam edebilirsiniz.

Lisans çakışmalarını önlemek için, lütfen:

* Önceden onay almadan **GPL-3.0 ile uyumsuz** üçüncü taraf kodları eklemeyin;
* Üçüncü taraf kütüphanelere başvurmanız gerekiyorsa, PR''da kaynaklarını ve lisanslarını açıkça belirtin ve uyumluluğu teyit edin.

### 4. Üst Eserler ve Orijinal Oyun Telif Hakkı

Bu proje, *Project Zomboid* ile ilgili modlar için **resmî olmayan bir çeviri** projesidir:

* Orijinal oyunun ve her bir modun telif hakkı ilgili yazarlarına/yayıncılarına aittir;
* Bu proje yalnızca metin çevirilerinin, üslup düzeltmelerinin ve bazı yardımcı kaynakların oluşturulmasını ve düzenlenmesini kapsar;
* Katkıda bulunanlar içerik gönderirken şunlardan emin olmalıdır:

  * Yetkisiz üçüncü taraf çeviri metinlerini veya sanatsal kaynakları doğrudan kopyalamamak;
  * Orijinal yazarların ve mod yazarlarının haklarına saygı göstermek ve hak ihlali içeren yeniden dağıtım yapmamak.

---

## İletişim ve İşbirliği

Eğer şunlara sahipseniz:

* Lisans koşulları hakkında sorular;
* Belirli bir içeriğin katkıda bulunulabilir olup olmadığı konusunda belirsizlik;
* Eserinizi özel bir şekilde lisanslama isteği (örneğin, yalnızca ticari olmayan kullanım ancak uyarlamaya izin verilmez);

Proje bakımcılarıyla şu yollarla iletişime geçebilirsiniz:

* Tartışma için bir Issue gönderme;
* Bakımcıların diğer kamuya açık iletişim yöntemleri.

Projenin sağlıklı gelişimini, tüm tarafların hak ve çıkarlarına saygı göstererek dengeleyen bir çözüm bulmak için elimizden geleni yapacağız.

---

## Mali Destek

Projenin işleyişi sırasında, yeni modların eklenmesi ve mevcut modların metin güncellemeleri nedeniyle, çeviri için LLM API''sinin sürekli olarak çağrılması gerekmektedir. LLM''in davranışını kısıtlamak için, temel mod metinlerine ek olarak büyük miktarda prompt içeriği (temel promptlar, çeviri kuralları, terim tabloları, girdi/çıktı kısıtlamaları, anlamsal arama sonuçları vb.) gereklidir ve bu, orijinal metinlerden çok daha fazla token tüketir. Bu nedenle projenin mali desteğe ihtiyacı vardır.

Mali destek sağlamak isterseniz, lütfen proje bakımcılarıyla iletişime geçin. Çok teşekkür ederiz!

---

Bu projeye katkıda bulunma isteğiniz için tekrar teşekkür ederiz!
Yaptığınız her katkı daha fazla oyuncuya fayda sağlar!
