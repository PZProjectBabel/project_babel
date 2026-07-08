# Project Babel — PZ Modları için Otomatik LLM Çevirisi

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [українська](README_uk.md)</details>

> ⚠️ **Not:** Bu çeviri henüz desteklenmemektedir. Yetkili içerik [Çince sürümdür](../../README.md).

---

*Bu çeviri projesi, [Project Babel](https://github.com/PZProjectBabel/project_babel) araç seti tarafından yürütülmekte ve bakımı yapılmaktadır.*

---

## İçindekiler

- [Desteklenen Hedef Diller](#desteklenen-hedef-diller)
- [Kurulum ve Kullanım](#kurulum-ve-kullanım)
- [Çeviri İlerlemesi](#çeviri-i̇lerlemesi)
- [Katkıda Bulunma](#katkıda-bulunma)
- [Araçlar ve Dizin Yapısı (Geliştiriciler için)](#araçlar-ve-dizin-yapısı-(geliştiriciler-için))
- [Telif Hakkı ve Lisans](#telif-hakkı-ve-lisans)
- [Teşekkürler](#teşekkürler)
- [Üçüncü Taraf Yazılımlar](#üçüncü-taraf-yazılımlar)

---

## Desteklenen Hedef Diller

| Dil | Yerel Adı | ISO Kodu | Oyun İçi Kodu | Destekleniyor | Not |
|------|------|------|------|------|------|
| Arapça | العربية | `ar` | `AR` | ❌ | Fon eksikliği |
| Katalanca | català | `ca` | `CA` | ❌ | Fon eksikliği |
| Geleneksel Çince | 繁體中文 | `zh-hant` | `CH` | ❌ | Fon eksikliği |
| Basitleştirilmiş Çince | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Çekçe | čeština | `cs` | `CS` | ❌ | Fon eksikliği |
| Danca | dansk | `da` | `DA` | ❌ | Fon eksikliği |
| Almanca | Deutsch | `de` | `DE` | ✅ | |
| İngilizce | English | `en` | `EN` | ✅ | |
| İspanyolca | español | `es` | `ES` | ❌ | Fon eksikliği |
| Fince | suomi | `fi` | `FI` | ❌ | Fon eksikliği |
| Fransızca | français | `fr` | `FR` | ✅ | |
| Macarca | magyar | `hu` | `HU` | ❌ | Fon eksikliği |
| Endonezce | Bahasa Indonesia | `id` | `ID` | ❌ | Fon eksikliği |
| İtalyanca | italiano | `it` | `IT` | ❌ | Fon eksikliği |
| Japonca | 日本語 | `ja` | `JP` | ✅ | |
| Korece | 한국어 | `ko` | `KO` | ❌ | Fon eksikliği |
| Felemenkçe | Nederlands | `nl` | `NL` | ❌ | Fon eksikliği |
| Norveççe | norsk | `no` | `NO` | ❌ | Fon eksikliği |
| Tagalogca | Tagalog | `tl` | `PH` | ❌ | Fon eksikliği |
| Lehçe | polski | `pl` | `PL` | ❌ | Fon eksikliği |
| Portekizce (Portekiz) | português | `pt` | `PT` | ❌ | Fon eksikliği |
| Portekizce (Brezilya) | português do Brasil | `pt-br` | `PTBR` | ❌ | Fon eksikliği |
| Romence | română | `ro` | `RO` | ❌ | Fon eksikliği |
| Rusça | русский | `ru` | `RU` | ❌ | Fon eksikliği |
| Tayca | ภาษาไทย | `th` | `TH` | ❌ | Fon eksikliği |
| Türkçe | Türkçe | `tr` | `TR` | ❌ | Fon eksikliği |
| Ukraynaca | українська | `uk` | `UA` | ❌ | Fon eksikliği |

**Toplam**: 27 planlanan dil | **Desteklenen**: 5 | **Beklemede**: 22

---

## Kurulum ve Kullanım

Oyun içinde çeviri paketini kullanmak isteyen oyuncular için bir rehber.

1. Steam Workshop sayfasına gidin: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. "Abone Ol" düğmesine tıklayın.
3. Oyunu başlatın, Modlar menüsünden bu çeviri modunu etkinleştirin.
4. Daha sonra yüklenen modların çeviri metni öncekileri geçersiz kılar, bu yüzden bu çeviri modu oynanış modlarından sonra yüklenmelidir.
5. Keyfini çıkarın!

---

## Çeviri İlerlemesi

[➡️ Çeviri İlerlemesi](../progress/progress_tr.md)

---

## Katkıda Bulunma

Katkılarınızı bekliyoruz! Çeviri düzeltmeleri, yeni özellikler, istem şablonları veya referans çeviriler.

LLM API çeviri çağrıları token maliyeti gerektirir. Desteğiniz projenin sürdürülebilir şekilde çalışmasına yardımcı olur!

---

## Araçlar ve Dizin Yapısı (Geliştiriciler için)

Bu bölüm, projenin otomasyon iç yapısını anlamak isteyen geliştiriciler içindir.

### Proje Dizinleri

| Dizin | Açıklama |
|------|------|
| `src/` | .NET 10 çeviri hattı kaynak kodu, 15 modül |
| `config/` | Hat yapılandırması (LLM, Steam, RAG parametreleri vb.) |
| `data/` | Çalışma zamanı verileri: mod meta verileri, embeddingler, çeviri önbelleği |
| `translation_ref/` | LLM bağlamı olarak referans çeviriler |
| `base_game_keys/` | Yinelenenleri önlemek için temel oyun çeviri anahtarları |
| `final_outputs/` | PZ mod formatında nihai çeviri çıktısı |
| `docs/` | Dokümantasyon: ilerleme, katkı, hat özellikleri |
| `temp/` | Geçici hat dosyaları |
| `src/prompt_templates/` | LLM bilgi istemi şablonları |

### Hat Modülleri (yürütme sırası)

| Adım | Modül | İşlev |
|------|------|------|
| 1 | `ConfigReader` | Yapılandırma/gizli anahtarlar/diller yükle |
| 2 | `RepoDataLoader` | Referansları ve çeviri önbelleğini yükle |
| 3 | `ModIdCollector` | Workshop mod kimliklerini topla |
| 4 | `ModInfoFetcher` | Steam meta verilerini al |
| 5 | `ModDownloader` | Modları steamcmd ile indir |
| 6 | `ContentExtractor` | Mod çeviri dosyalarını ayrıştır → `TranslationEntry` |
| 7 | `ContentChecker` | İçerik güvenlik incelemesi |
| 8 | `EmbeddingFetcher` | Metin gömme vektörlerini hesapla |
| 9 | `TranslationBatcher` | Çeviri partileri oluştur |
| 10 | `RagContextRetriever` | RAG bağlamlarını al |
| 11 | `LLMTranslator` | LLM çevirisini yürüt |
| 12 | `ResultWriter` | data/ ve translation_ref/ dizinlerine yaz |
| 13 | `FinalOutputWriter` | Nihai PZ mod çıktısını oluştur |
| 14 | `ProgressReporter` | İlerleme raporları oluştur |

### Teknoloji Yığını

- **Dil**: C# (.NET 10)
- **Hedef Platform**: GitHub Actions Linux x64 runner
- **Testler**: xUnit (Windows x64)
- **LLM**: DeepSeek API (yapılandırılabilir)
- **Embedding**: RAG benzerlik araması için metin vektörleştirme
- **İçerik İncelemesi**: LLM destekli çok seviyeli güvenlik denetimi

Ayrıntılı teknik belgeler: [TranslationEntry İşlem Hattı](../pipeline/translation_entry_pipeline_tr.md)

---

## Telif Hakkı ve Lisans

© 2025 Project Babel ve tüm yazarlar. Tüm hakları saklıdır.

### İçerik (metinler, resimler)

**CC BY-NC-SA 4.0** kapsamında lisanslanmıştır.

- **Atıf**: "Project Babel" temelli değişiklikleri belirtin, repo ve Workshop bağlantılarını ekleyin
- **Gayri-ticari**: Ticari kullanım yasaktır
- **Aynı koşullarla paylaş**: Değişiklikler aynı lisans altında yayınlanmalıdır

### Kod

`src/` altındaki kod **GPL-3.0** ile lisanslanmıştır.

---

## Teşekkürler

| Referans Mod | Yazar | Sayfa |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Yukarıdaki yazarlara derin şükranlarımızı sunarız!**

---

## Üçüncü Taraf Yazılımlar

Bu proje, telif hakları ilgili geliştiricilere ait olan üçüncü taraf programları ve kütüphaneleri kullanır.
