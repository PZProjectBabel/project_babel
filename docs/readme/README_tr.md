# Project Babel — Zombi Yıkımı Projesi modu LLM otomatik çeviri projesi

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Bu çeviri projesi [Project Babel](https://github.com/PZProjectBabel/project_babel) araç seti tarafından yönlendirilir ve bakımı yapılır.*

---

## İçindekiler

- [Projenin desteklediği hedef çeviri dilleri](#projenin-desteklediği-hedef-çeviri-dilleri)
- [Nasıl Kurulur ve Kullanılır](#nasıl-kurulur-ve-kullanılır)
- [Çeviri İlerlemesi](#çeviri-İlerlemesi)
- [Nasıl Katkıda Bulunulur](#nasıl-katkıda-bulunulur)
- [Araçlar ve Dizin Yapısı (Geliştiriciler İçin)](#araçlar-ve-dizin-yapısı-geliştiriciler-İçin)
  - [Proje Dizinleri](#proje-dizinleri)
  - [Çeviri Hattı Modülleri (Çalıştırma Sırasına Göre)](#çeviri-hattı-modülleri-çalıştırma-sırasına-göre)
  - [Teknoloji Yığını](#teknoloji-yığını)
- [Telif Hakkı ve Lisans](#telif-hakkı-ve-lisans)
  - [1. Metin, görseller ve diğer içerikler](#1-metin-görseller-ve-diğer-içerikler)
  - [2. Program, Betik ve Diğer Geliştirme İçerikleri](#2-program-betik-ve-diğer-geliştirme-İçerikleri)
- [Teşekkürler](#teşekkürler)
- [Üçüncü Taraf Programlar](#üçüncü-taraf-programlar)

---

## Projenin desteklediği hedef çeviri dilleri

| Dil | Yerel Ad | Uluslararası Kod | Oyun İçi Kod | Destekleniyor | Notlar |
|------|------|------|------|------|------|
| Arapça | العربية | `ar` | `AR` | ❌ | Token limiti yetersiz |
| Katalanca | català | `ca` | `CA` | ❌ | Token limiti yetersiz |
| Geleneksel Çince | 繁體中文 | `zh-hant` | `CH` | ❌ | Token limiti yetersiz |
| Basitleştirilmiş Çince | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Çekçe | čeština | `cs` | `CS` | ❌ | Token limiti yetersiz |
| Danca | dansk | `da` | `DA` | ❌ | Token limiti yetersiz |
| Almanca | Deutsch | `de` | `DE` | ✅ | |
| İngilizce | English | `en` | `EN` | ✅ | |
| İspanyolca | español | `es` | `ES` | ❌ | Token limiti yetersiz |
| Fince | suomi | `fi` | `FI` | ❌ | Token limiti yetersiz |
| Fransızca | français | `fr` | `FR` | ✅ | |
| Macarca | magyar | `hu` | `HU` | ❌ | Token limiti yetersiz |
| Endonezce | Bahasa Indonesia | `id` | `ID` | ❌ | Token limiti yetersiz |
| İtalyanca | italiano | `it` | `IT` | ❌ | Token limiti yetersiz |
| Japonca | 日本語 | `ja` | `JP` | ✅ | |
| Korece | 한국어 | `ko` | `KO` | ❌ | Token limiti yetersiz |
| Felemenkçe | Nederlands | `nl` | `NL` | ❌ | Token limiti yetersiz |
| Norveççe | norsk | `no` | `NO` | ❌ | Token limiti yetersiz |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Token limiti yetersiz |
| Lehçe | polski | `pl` | `PL` | ❌ | Token limiti yetersiz |
| Portekizce (Portekiz) | português | `pt` | `PT` | ❌ | Token limiti yetersiz |
| Portekizce (Brezilya) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token limiti yetersiz |
| Rumence | română | `ro` | `RO` | ❌ | Token limiti yetersiz |
| Rusça | русский | `ru` | `RU` | ❌ | Token limiti yetersiz |
| Tayca | ภาษาไทย | `th` | `TH` | ❌ | Token limiti yetersiz |
| Türkçe | Türkçe | `tr` | `TR` | ❌ | Token limiti yetersiz |
| Ukraynaca | українська | `uk` | `UA` | ❌ | Token limiti yetersiz |

**Toplam**: 27 planlanan dil | **Desteklenen**: 5 | **Bekleyen**: 22

---

## Nasıl Kurulur ve Kullanılır

Bu, bu çeviri projesini oyunda doğrudan kullanmak isteyen oyuncular için hazırlanmış bir kılavuzdur.

1.  Steam Atölyesi sayfamıza gidin: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  「Abone Ol」düğmesine tıklayın.
3.  Oyunu başlatın, ana menüdeki 「Modlar」 yönetiminden bu çeviri modunu etkinleştirin.
4.  Sonradan etkinleştirilen modların çeviri metinleri önce etkinleştirilen modlarınkini geçersiz kılar, bu nedenle bu çeviri modu, işlev modlarından sonra etkinleştirilmelidir (mümkün olduğunca alta koyun).
5.  Oyunun tadını çıkarın!

---

## Çeviri İlerlemesi

**[➡️ Çeviri ilerlemesini görmek için buraya tıklayın](./docs/progress/progress_tr.md)**

---

## Nasıl Katkıda Bulunulur

Herkesi katkıda bulunmaya davet ediyoruz; ister bir hatayı düzeltmek, ister yeni bir özellik eklemek, ister prompt şablonları yazmak veya referans çeviri sağlamak olsun!

LLM API'sini kullanarak çeviri yapmak token başına ücretlidir. Projenin uzun vadeli istikrarlı bir şekilde çalışması için cömertçe yardım etmenizi umuyoruz!

Ayrıntılar için [Katkı Kılavuzu](./docs/contributing/contributing_tr.md)

---

## Araçlar ve Dizin Yapısı (Geliştiriciler İçin)

Bu bölüm, projenin otomasyon prensiplerini anlamak isteyen geliştiricilere yöneliktir.

### Proje Dizinleri

| Dizin | Açıklama |
|------|------|
| `src/` | .NET 10 çeviri hattı kaynak kodu, 15 modül içerir |
| `config/` | Çeviri hattı yapılandırma dosyaları (LLM, Steam, RAG parametreleri vb.) |
| `data/` | Çalışma zamanı verileri: mod meta verileri, embedding, çeviri önbelleği |
| `translation_ref/` | Referans çeviri verileri (ör. As1 Çeviri Grubu lisanslı modları), LLM'ye çeviri referansı sağlar |
| `base_game_keys/` | Oyunun temel çeviri anahtarları, yinelenen anahtarları önlemek ve orijinal metni korumak için |
| `final_outputs/` | Son çıktılar: `project_babel/` mod paketi, `icons/` simgeler ve `workshop_descriptions/` Atölye açıklamaları |
| `docs/` | Proje belgeleri: ilerleme raporu, katkı kılavuzu, çeviri hattı açıklaması |
| `temp/` | Çeviri hattı geçici dosyaları (her çalıştırmada bağımsız dizin) |
| `src/prompt_templates/` | LLM prompt şablonları (çeviri/içerik denetimi) |

### Çeviri Hattı Modülleri (Çalıştırma Sırasına Göre)

| Adım | Modül | İşlev |
|------|------|------|
| 1 | `ConfigReader` | Yapılandırma/anahtar/dil listesini yükle |
| 2 | `RepoDataLoader` | Referans çevirileri ve çeviri önbelleğini yükle |
| 3 | `ModIdCollector` | Workshop mod ID'lerini topla |
| 4 | `ModInfoFetcher` | Steam meta verilerini al |
| 5 | `SteamCmdBootstrapper` | Mevcut platform için steamcmd çalışma zamanını hazırla |
| 6 | `ModDownloader` | steamcmd ile modları indir |
| 7 | `ContentExtractor` | Mod çeviri dosyalarını ayrıştır → `TranslationEntry` |
| 8 | `ContentChecker` | İçerik güvenlik denetimi (uyuşturucu/müstehcenlik/şiddet) |
| 9 | `EmbeddingFetcher` | Metin embedding vektörlerini hesapla |
| 10 | `TranslationBatcher` | Dilden bağımsız çeviri grupları oluştur |
| 11 | `RagContextRetriever` | RAG bağlamını al (tam anahtar + embedding benzerliği) |
| 12 | `LLMTranslator` | LLM'yi çağırarak çeviriyi gerçekleştir |
| 13 | `ResultWriter` | data/ ve translation_ref/ dizinine yaz |
| 14 | `FinalOutputWriter` | Son PZ mod formatı çıktısını oluştur |
| 15 | `ProgressReporter` | İlerleme raporu oluştur |

### Teknoloji Yığını

- **Dil**: C# (.NET 10)
- **Hedef platform**: GitHub Actions Linux x64 runner
- **Test**: xUnit (Windows x64)
- **LLM**: DeepSeek API (yapılandırılabilir)
- **Embedding**: RAG benzerlik araması için metin vektörleştirme
- **İçerik denetimi**: LLM destekli çok seviyeli güvenlik incelemesi

Ayrıntılı [teknik referans](./docs/technical_reference/technical_reference_tr.md).

---

## Telif Hakkı ve Lisans

Bu çeviri projesinin çeviri metin içeriği ve ilgili görseller, **Project Babel** ve katılımcılar tarafından orijinal oyun modlarına dayanarak oluşturulmuş veya yeniden düzenlenmiştir.

© 2025 Project Babel ve yazarlar tüm hakları saklıdır.

### 1. Metin, görseller ve diğer içerikler

Aksi açıkça belirtilmedikçe, bu depodakiler:

- Oyun içi metin çevirisi, düzeltme ve düzeltme içeriği;
Proje belgeleri, mod içi metin çevirileri;
Bu projeye özel oluşturulan resimler, sanat kaynakları

Hepsi **Atıf-GayriTicari-AynıŞekildePaylaş 4.0 Uluslararası** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, kısaca **CC BY-NC-SA 4.0**) lisansı ile lisanslanmıştır.

Bu, aşağıdaki koşullara uyulması koşuluyla bu içerikleri özgürce paylaşabileceğiniz ve uyarlayabileceğiniz anlamına gelir:

- **Atıf (BY)** : Belirgin bir yerde "Bu çeviri projesi 'Project Babel'ın çalışmalarına dayanarak değiştirilmiştir" ibaresini ve bu depo ile Steam Atölyesi bağlantısını ekleyin `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **GayriTicari (NC)** : Bu projenin içeriğini veya uyarlamalarını doğrudan veya dolaylı herhangi bir ticari amaçla kullanamazsınız (ücretli paketler, ücretli indirme, reklam geliri paylaşımı vb. dahil ancak bunlarla sınırlı değildir);
- **AynıŞekildePaylaş (SA)** : Bu proje içeriğine dayanarak değişiklik yapar veya yeniden oluşturursanız, değişikliklerinizi **aynı CC BY-NC-SA 4.0 lisansı** ile kamuya açık olarak yayınlamalısınız.

Bu lisans hakkında daha fazla bilgi için bkz.:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.tr>

*Özel notlar:*
- *base_game_keys klasörünün içeriği oyunun kendisinden gelir, telif hakkı oyun geliştiricisine aittir! İçerik, çeviri anahtarlarının oyun anahtarlarını ezmesini önlemek (yinelenenleri kaldırmak) için kullanılır.*
- *translation_ref klasörünün içeriği LLM'ye çeviri referansı sağlamak için kullanılır, telif hakkı ilgili mod geliştiricilerine aittir!*

### 2. Program, Betik ve Diğer Geliştirme İçerikleri

Kaynak dosyalarında veya dizinlerinde aksi belirtilmedikçe, bu depoda Çinceleştirme içeriğini oluşturmak/paketlemek/işlemek için kullanılan program kodu (örneğin `src/` dizinindeki kod) **GNU Genel Kamu Lisansı Sürüm 3 (GPL-3.0)** ile lisanslanmıştır.

Tam koşullar için bu deponun kök dizinindeki `LICENSE` dosyasına (GPL-3.0) veya GNU web sitesine bakın: <https://www.gnu.org/licenses/gpl-3.0.html>

---

## Teşekkürler

Bu proje, hedef dil çevirisi için referans metin olarak üçüncü taraf modları kullanmıştır; referans metinler LLM'ye çeviri referansı olarak gönderilir.

| Referans Mod Adı | Yazar | Mod Sayfası |
|------|------|------|
| [B42] Birleşik · Çince Çeviri | Ruyi Çeviri Grubu (As1) | [Atölye Sayfası](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Birleşik · Mod Çeviri | Ruyi Çeviri Grubu (As1) | [Atölye Sayfası](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Birleşik · Ark Çeviri | Ruyi Çeviri Grubu (As1) | [Atölye Sayfası](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Yukarıdaki yazarlara içten teşekkürlerimizi sunarız!**

---

## Üçüncü Taraf Programlar

Bu proje, üçüncü taraf programlar ve kütüphaneler kullanmaktadır; bu üçüncü taraf programların telif hakkı ilgili geliştiricilere aittir.

