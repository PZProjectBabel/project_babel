# Katkı Rehberi (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## İçindekiler

- [1. Başlamadan Önce](#1-başlamadan-önce)
- [2. Nasıl Katkıda Bulunabilirim?](#2-nasıl-katkıda-bulunabilirim)
- [3. Çeviri Kuralları, Terim Sözlükleri ve Sistem Promptlarını İyileştirme](#3-çeviri-kuralları-terim-sözlükleri-ve-sistem-promptlarını-İyileştirme)
- [4. Elle düzeltilmiş derlem sağlama](#4-elle-düzeltilmiş-derlem-sağlama)
- [5. Boru hattı ve araç geliştirme katkıları](#5-boru-hattı-ve-araç-geliştirme-katkıları)
- [6. Telif Hakkı ve Lisans Sözleşmesi](#6-telif-hakkı-ve-lisans-sözleşmesi)
  - [6.1 Temel İlke: Telif Hakkınızı Saklı Tutarken Projenin Kullanımına İzin Verirsiniz](#61-temel-İlke-telif-hakkınızı-saklı-tutarken-projenin-kullanımına-İzin-verirsiniz)
  - [6.2 Metin ve Görseller Gibi İçeriklerin Lisansı (CC BY-NC-SA 4.0)](#62-metin-ve-görseller-gibi-İçeriklerin-lisansı-cc-by-nc-sa-40)
  - [6.3 Betik ve Araç Kodlarının Lisansı (GPL-3.0)](#63-betik-ve-araç-kodlarının-lisansı-gpl-30)
  - [6.4 Üst akış eserleri ve orijinal oyun telif hakkı](#64-üst-akış-eserleri-ve-orijinal-oyun-telif-hakkı)
- [7. İletişim ve İş Birliği](#7-İletişim-ve-İş-birliği)
- [8. Finansal Destek](#8-finansal-destek)

---

**Project Babel - Zomboid İmha Oyunu Modu LLM Otomatik Çeviri Projesi**'ne katkıda bulunmaya istekli olduğunuz için çok teşekkür ederiz! İster bir hatayı düzeltmek, ister yeni bir özellik eklemek, ister bir prompt şablonu yazmak, isterse de referans çeviri sağlamak olsun!

LLM API'sini kullanarak çeviri yapmak token başına ücretlidir. Projenin uzun vadeli ve istikrarlı bir şekilde çalışabilmesi için cömertçe yardım etmenizi umuyoruz!

> ⚠️ **Önemli Uyarı:**
> Depoya herhangi bir içerik göndermeden önce lütfen "Telif Hakkı ve Lisans Anlaşması" bölümünü okuyup anladığınızdan emin olun.
> Gönderildikten ve birleştirildikten sonra, ilgili lisans koşullarını kabul etmiş sayılırsınız.

---

## 1. Başlamadan Önce

Lütfen önce projenin `README.md` dosyasını okuyun, şunları öğrenin:
- Bu projenin genel hedefi ve mevcut durumu;
- Sıradan oyuncuların bu projeyi nasıl kullanacağı (kendinizi test etmeniz için);
- Proje teknik detayları.

---

## 2. Nasıl Katkıda Bulunabilirim?

İlgi alanlarınıza ve becerilerinize göre bir veya daha fazla şekilde katılabilirsiniz:

- Hedef dil için çeviri kuralları sağlamak
- Hedef dil için çeviri terim sözlüğü sağlamak
- Sistem promptlarını iyileştirmek
- İnsan tarafından düzeltilmiş çeviri metinleri sağlamak
- İş hattı modüllerini (.NET) ve otomasyon betiklerini iyileştirmek
- Sorun bildirmek, iyileştirme önerileri sunmak (Issues'ta açıklayarak)
- LLM çağrılarına mali destek sağlamak

Aşağıda ana katkı senaryoları hakkında bazı açıklamalar bulunmaktadır.

---

## 3. Çeviri Kuralları, Terim Sözlükleri ve Sistem Promptlarını İyileştirme

İş hattının prompt şablonları `src/prompt_templates/` konumunda bulunur, yapısı şöyledir:

- `system_prompt_translate_engine.txt`: Genel çeviri motoru sistem promptu (tüm diller tarafından paylaşılır);
- `<dil_kodu>/translation_dictionary_<dil_kodu>.json`: Bu dilin terim sözlüğü;
- `<dil_kodu>/translation_schema_<dil_kodu>.md`: Bu dilin çeviri kuralları ve stil kısıtlamaları.

Katkı adımları:

1. `src/prompt_templates/` altında diliniz için bir alt dizin oluşturun, terim sözlüğü ve çeviri kuralları dosyasını ekleyin;
2. Genel çeviri davranışını ayarlamak istiyorsanız `system_prompt_translate_engine.txt` dosyasını değiştirin (tüm dilleri etkilediğini unutmayın);
3. Yerel testle etkisini doğrulayın;
4. PR gönderin.

---

## 4. Elle düzeltilmiş derlem sağlama

Çeviri modu yapımcısıysanız ve çeviri derleminizi LLM çeviri referansı olarak sağlamak istiyorsanız, lütfen Issue'da başvuruda bulunun. Aşağıdaki bilgileri sağlamanız gerekmektedir:

- Çeviri modunuzun Mod ID'si ve çeviri hedef dili;
- Mod yazarı olduğunuzu kanıtlamak için çeviri modunuzun arka plan sayfasının ekran görüntüsü;
- Issue'da çeviri derlemini sağlamaya istekli olduğunuzu açıkça belirtin;
- Özel durumlar varsa (özel lisanslama vb.), lütfen ayrıca belirtin;
- Sağladığınız derlemin yüksek kalitede olduğundan emin olun.

Yetkiniz doğrultusunda proje, modunuzu `config/ref_translation_mods.json` referans çeviri modları listesine ekleyecek ve boru hattı, çeviri metinlerinizi RAG referans derlemi olarak otomatik olarak senkronize edecektir.

---

## 5. Boru hattı ve araç geliştirme katkıları

Bu projenin otomasyonu iki bölüme ayrılmıştır:

**Boru hattı modülü (`src/`, C# / .NET 10)**： SteamCMD başlatma, mod indirme, metin çıkarma, içerik denetimi, Embedding hesaplama, RAG arama ve LLM çevirisi ile nihai çıktıya kadar sırayla yürütülen 15 modül içerir. Ayrıntılar için bkz. [Teknik Referans](../technical_reference/technical_reference_tr.md).

**Yardımcı betikler (.github/)**： GitHub otomasyonu için kullanılır.

Eğer şunları istiyorsanız:

* Mevcut boru hattı modüllerindeki veya betiklerdeki hataları düzeltmek;
* Boru hattına yeni özellikler veya yeni modüller eklemek;
* Performansı veya kod yapısını optimize etmek;
* Prompt şablonlarını veya RAG stratejisini iyileştirmek;

Aşağıdaki adımları izleyebilirsiniz:

1. Bu depoyu forklayın ve yerel bilgisayarınıza klonlayın;
2. En son daldan yeni bir dal oluşturun;
3. İlgili dizinde dosyaları değiştirin veya ekleyin:
- Boru hattı modülü değişiklikleri → `src/<modül_adı>/`;
- Betik değişiklikleri → `scripts/`;
- Prompt şablonu değişiklikleri → `src/prompt_templates/`;
4. Göndermeden önce lütfen mümkün olduğunca:

* Mevcut kod stilini koruyun;
* Gerekli yorumları ekleyin;
* Mümkünse basit bir test veya kullanım kılavuzu ekleyin;
5. Değişikliği PR ile gönderin ve açıklamada şunları belirtin:

* Değişikliğin amacı;
* Etkilenen dizinler / modüller / betikler;
* Kırıcı değişiklik içerip içermediği.

---

## 6. Telif Hakkı ve Lisans Sözleşmesi

> **Önemli Uyarı:**
> Telif hakkı ve lisans sözleşmesi, projenin, yazarların, katkıda bulunanların ve oyuncuların yasal haklarını korumak için tasarlanmıştır; "uyum" veya "varsayılan" nedeniyle yanlış anlaşılmaları önlemek amacıyla lütfen dikkatlice okuyun.
> Telif hakkı ve lisans, README.md dosyasındaki içerik esas alınır; bu bölüm yalnızca daha anlaşılır bir açıklama sağlar.

### 6.1 Temel İlke: Telif Hakkınızı Saklı Tutarken Projenin Kullanımına İzin Verirsiniz

* Kendi oluşturduğunuz içerikler (çeviriler, resimler, betikler/programlar vb.) üzerindeki telif hakkınız hâlâ size aittir;
* Ancak bu içerikleri bu depoya gönderip kabul edildikten (birleştirildikten) sonra, projenin benimsediği açık kaynak/paylaşım lisansı kapsamında başkalarına bu içerikleri kullanma izni vermiş olursunuz.

Bu şu anlama gelir:

* Çalışmalarınızı başka yerlerde kullanmaya ve sergilemeye **devam edebilirsiniz**;
* Ancak katkınız birleştirildikten sonra, projeden veya içeriği yasal olarak edinmiş diğer kullanıcılardan "yetkiyi geri çekmesini" veya "eski sürümleri silmesini" **talep edemezsiniz**.

### 6.2 Metin ve Görseller Gibi İçeriklerin Lisansı (CC BY-NC-SA 4.0)

Gönderdiğiniz aşağıdaki içerikler için:

* Oyun metinlerinin çevirisi, düzeltmesi ve redaksiyonu;
* Proje belgeleri, açıklayıcı metinler;
* Proje için özel olarak oluşturulmuş resimler, sanat kaynakları;

Bu depo tarafından kabul edilip birleştirildiğinde, şunları kabul etmiş sayılırsınız:

1. Bu içerikler **Atıf-GayriTicari-AynıLisanslaPaylaş 4.0 Uluslararası** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, kısaca **CC BY-NC-SA 4.0**) lisansı altında lisanslanır;
2. Project Babel ve bu içeriklere erişen tüm kullanıcılar, **CC BY-NC-SA 4.0 hükümlerine uymak koşuluyla**:
* Bu içerikleri paylaşabilir, kopyalayabilir ve yeniden dağıtabilir;
* Ticari olmayan amaçlarla değiştirebilir ve yeniden oluşturabilir;
3. Geçerli yasaların izin verdiği ölçüde, bu lisansın **münhasır olmayan, küresel, telifsiz ve geri alınamaz** bir izin olduğunu kabul edersiniz;
4. Gelecekte projeden ayrılsanız veya katkıda bulunmayı bıraksanız bile, proje halihazırda gönderdiğiniz ve birleştirilmiş olan ilgili içerikleri CC BY-NC-SA 4.0 kapsamında kullanmaya ve yeniden yayımlamaya devam edebilir.

> Yukarıdaki lisanslama şeklini kabul etmiyorsanız, lütfen bu projeye metin veya görsel katkılar göndermeyin,
> veya proje sorumlusuyla önceden iletişime geçerek başka bir şekilde iş birliği yapılıp yapılamayacağını teyit edin.

### 6.3 Betik ve Araç Kodlarının Lisansı (GPL-3.0)

Gönderip kabul edilen aşağıdakiler için:

* Otomasyon betikleri;
* Derleme/Dışa aktarma araçları;
* Bu çeviri projesini işlemek için kullanılan diğer program kodları;

Özel bir bildirim olmadığı takdirde, aşağıdakileri kabul etmiş sayılırsınız:

1. Kod, **GPL-3.0** (GNU Genel Kamu Lisansı 3. Sürüm) ile lisanslanmıştır;
2. Proje bakımcıları, GPL-3.0'ın izin verdiği ölçüde, bunu değiştirebilir, birleştirebilir ve dağıtabilir;
3. Ayrıca, GPL-3.0 hükümlerine uyduğunuz sürece, aynı koda dayanarak başka projeler geliştirebilirsiniz.

Lisans çakışmalarını önlemek için lütfen mümkün olduğunca:

* Onaylamadan, **GPL-3.0 ile uyumsuz** üçüncü taraf kodları eklemeyin;
* Gerçekten üçüncü taraf bir kütüphaneyi kullanmanız gerekiyorsa, PR'da kaynağını ve lisansını açıkça belirtin ve uyumluluğunu onaylayın.

### 6.4 Üst akış eserleri ve orijinal oyun telif hakkı

Bu proje, 《僵尸毁灭工程》(Project Zomboid) ile ilgili modların **resmi olmayan çeviri** projesidir:

* Orijinal oyun ve her bir modun telif hakkı, ilgili yazarlarına/yayıncılarına aittir;
* Bu proje yalnızca metin çevirisi, düzenleme ve bazı yardımcı kaynakların oluşturulması ve düzenlenmesini kapsar;
* Katkıda bulunanlar, içerik gönderirken aşağıdakileri sağlamalıdır:
* Yetkisi olmayan üçüncü taraf çeviri metinlerini veya sanat kaynaklarını doğrudan kopyalamamak;
* Orijinal yazarların ve mod yapımcılarının haklarına saygı göstermek, telif hakkını ihlal eden şekilde yeniden yayınlamamak.

---

## 7. İletişim ve İş Birliği

Eğer aşağıdaki konularda:

* Lisans hükümleri hakkında sorularınız varsa;
* Belirli bir içeriğin katkıda bulunulup bulunulamayacağından emin değilseniz;
* Çalışmalarınızı özel bir şekilde lisanslamak istiyorsanız (örneğin yalnızca ticari olmayan kullanıma izin vermek ancak uyarlamaya izin vermemek vb.);

Proje bakımcılarıyla aşağıdaki yollarla iletişime geçebilirsiniz:

* Tartışmak için Issue açın;
* Diğer bakımcıların kamuya açık iletişim bilgileri.

Herkesin haklarına saygı göstererek, projenin sağlıklı gelişimini de gözeten bir çözüm bulmaya çalışacağız.

---

## 8. Finansal Destek

Proje çalışırken, yeni modlar eklenmesi, eski modların metin içeriklerinin güncellenmesi vb. nedenlerle sürekli olarak LLM API'sini kullanarak çeviri yapmak gerekmektedir. LLM davranışını sınırlamak için, temel mod metinlerine ek olarak, çok sayıda istem içeriği (temel istemler, çeviri kuralları, terim sözlüğü, giriş/çıkış kısıtlamaları, anlamsal sorgu sonuçları vb.) sağlanması gerekir; bu içerikler, orijinal metnin token'larından çok daha fazla tüketir. Bu nedenle proje finansal desteğe ihtiyaç duymaktadır.

Eğer finansal destek sağlamak isterseniz, proje bakımcılarıyla iletişime geçin. Çok teşekkürler!

---

Projeye katkıda bulunmayı kabul ettiğiniz için tekrar teşekkür ederiz!
Her katkınız, daha fazla oyuncunun faydalanmasını sağlayacak!
