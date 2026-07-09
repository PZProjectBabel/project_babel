# Project Babel — Terjemahan Otomatis Mod PZ dengan LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Catatan:** Terjemahan ini belum didukung. Konten resmi mengacu pada [versi bahasa Tionghoa](../../README.md).

---

*Proyek terjemahan ini didukung dan dikelola oleh alat [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Daftar Isi

- [Bahasa Target yang Didukung](#bahasa-target-yang-didukung)
- [Cara Instal & Menggunakan](#cara-instal--menggunakan)
- [Kemajuan Terjemahan](#kemajuan-terjemahan)
- [Berkontribusi](#berkontribusi)
- [Alat & Struktur Direktori (untuk Pengembang)](#alat--struktur-direktori-(untuk-pengembang))
- [Hak Cipta & Lisensi](#hak-cipta--lisensi)
- [Ucapan Terima Kasih](#ucapan-terima-kasih)
- [Perangkat Lunak Pihak Ketiga](#perangkat-lunak-pihak-ketiga)

---

## Bahasa Target yang Didukung

| Bahasa | Nama Lokal | Kode ISO | Kode Dalam Game | Didukung | Catatan |
|------|------|------|------|------|------|
| Arab | العربية | `ar` | `AR` | ❌ | Kredit token tidak mencukupi |
| Katalan | català | `ca` | `CA` | ❌ | Kredit token tidak mencukupi |
| Tionghoa Tradisional | 繁體中文 | `zh-hant` | `CH` | ❌ | Kredit token tidak mencukupi |
| Tionghoa Sederhana | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Ceko | čeština | `cs` | `CS` | ❌ | Kredit token tidak mencukupi |
| Denmark | dansk | `da` | `DA` | ❌ | Kredit token tidak mencukupi |
| Jerman | Deutsch | `de` | `DE` | ✅ | |
| Inggris | English | `en` | `EN` | ✅ | |
| Spanyol | español | `es` | `ES` | ❌ | Kredit token tidak mencukupi |
| Finlandia | suomi | `fi` | `FI` | ❌ | Kredit token tidak mencukupi |
| Prancis | français | `fr` | `FR` | ✅ | |
| Hungaria | magyar | `hu` | `HU` | ❌ | Kredit token tidak mencukupi |
| Indonesia | Bahasa Indonesia | `id` | `ID` | ❌ | Kredit token tidak mencukupi |
| Italia | italiano | `it` | `IT` | ❌ | Kredit token tidak mencukupi |
| Jepang | 日本語 | `ja` | `JP` | ✅ | |
| Korea | 한국어 | `ko` | `KO` | ❌ | Kredit token tidak mencukupi |
| Belanda | Nederlands | `nl` | `NL` | ❌ | Kredit token tidak mencukupi |
| Norwegia | norsk | `no` | `NO` | ❌ | Kredit token tidak mencukupi |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Kredit token tidak mencukupi |
| Polandia | polski | `pl` | `PL` | ❌ | Kredit token tidak mencukupi |
| Portugis (Portugal) | português | `pt` | `PT` | ❌ | Kredit token tidak mencukupi |
| Portugis (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Kredit token tidak mencukupi |
| Rumania | română | `ro` | `RO` | ❌ | Kredit token tidak mencukupi |
| Rusia | русский | `ru` | `RU` | ❌ | Kredit token tidak mencukupi |
| Thailand | ภาษาไทย | `th` | `TH` | ❌ | Kredit token tidak mencukupi |
| Turki | Türkçe | `tr` | `TR` | ❌ | Kredit token tidak mencukupi |
| Ukraina | українська | `uk` | `UA` | ❌ | Kredit token tidak mencukupi |

**Total**: 27 bahasa yang direncanakan | **Didukung**: 5 | **Tertunda**: 22

---

## Cara Instal & Menggunakan

Panduan untuk pemain yang ingin menggunakan paket terjemahan dalam game.

1. Buka halaman Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Klik "Subscribe".
3. Jalankan game, aktifkan mod terjemahan ini di menu Mods.
4. Teks terjemahan dari mod yang dimuat belakangan menimpa yang sebelumnya, jadi mod terjemahan ini harus dimuat setelah mod gameplay.
5. Selamat menikmati!

---

## Kemajuan Terjemahan

[➡️ Kemajuan Terjemahan](../progress/progress_id.md)

---

## Berkontribusi

Kami menerima kontribusi: perbaikan terjemahan, fitur baru, templat prompt, atau terjemahan referensi.

Panggilan API LLM untuk terjemahan memerlukan biaya token. Dukungan Anda membantu proyek berjalan berkelanjutan!

Read the [Contributing Guide](../contributing/contributing_id.md) for details.

---

## Alat & Struktur Direktori (untuk Pengembang)

Bagian ini ditujukan untuk pengembang yang ingin memahami cara kerja otomatisasi proyek.

### Direktori Proyek

| Direktori | Deskripsi |
|------|------|
| `src/` | Kode sumber pipeline terjemahan .NET 10, 15 modul |
| `config/` | Konfigurasi pipeline (LLM, Steam, parameter RAG, dll.) |
| `data/` | Data runtime: metadata mod, embedding, cache terjemahan |
| `translation_ref/` | Terjemahan referensi sebagai konteks LLM |
| `base_game_keys/` | Kunci terjemahan game dasar untuk deduplikasi |
| `final_outputs/` | Output akhir dalam format mod PZ |
| `docs/` | Dokumentasi: kemajuan, kontribusi, spesifikasi pipeline |
| `temp/` | File sementara pipeline |
| `src/prompt_templates/` | Template prompt LLM |

### Modul Pipeline (urutan eksekusi)

| Langkah | Modul | Fungsi |
|------|------|------|
| 1 | `ConfigReader` | Muat konfigurasi/rahasia/bahasa |
| 2 | `RepoDataLoader` | Muat referensi dan cache terjemahan |
| 3 | `ModIdCollector` | Kumpulkan ID mod Workshop |
| 4 | `ModInfoFetcher` | Ambil metadata Steam |
| 5 | `ModDownloader` | Unduh mod melalui steamcmd |
| 6 | `ContentExtractor` | Parse file terjemahan mod → `TranslationEntry` |
| 7 | `ContentChecker` | Tinjauan keamanan konten |
| 8 | `EmbeddingFetcher` | Hitung vektor embedding teks |
| 9 | `TranslationBatcher` | Buat batch terjemahan |
| 10 | `RagContextRetriever` | Ambil konteks RAG |
| 11 | `LLMTranslator` | Jalankan terjemahan LLM |
| 12 | `ResultWriter` | Tulis ke data/ dan translation_ref/ |
| 13 | `FinalOutputWriter` | Hasilkan output akhir format mod PZ |
| 14 | `ProgressReporter` | Hasilkan laporan kemajuan |

### Stack Teknologi

- **Bahasa**: C# (.NET 10)
- **Platform Target**: GitHub Actions Linux x64 runner
- **Pengujian**: xUnit (Windows x64)
- **LLM**: DeepSeek API (dapat dikonfigurasi)
- **Embedding**: Vektorisasi teks untuk pencarian kemiripan RAG
- **Tinjauan Konten**: Audit keamanan multi-level berbasis LLM

Dokumentasi teknis terperinci: [Pipeline TranslationEntry](../pipeline/translation_entry_pipeline_id.md)

---

## Hak Cipta & Lisensi

© 2025 Project Babel dan semua penulis. Semua hak dilindungi.

### Konten (teks, gambar)

Dilisensikan di bawah **CC BY-NC-SA 4.0**.

- **Atribusi**: Cantumkan modifikasi berbasis "Project Babel", dengan tautan repo & Workshop
- **Non-komersial**: Penggunaan komersial dilarang
- **BerbagiSerupa**: Modifikasi harus dipublikasikan di bawah lisensi yang sama

### Kode

Kode di bawah `src/` dilisensikan di bawah **GPL-3.0**.

---

## Ucapan Terima Kasih

| Mod Referensi | Pembuat | Halaman |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Terima kasih yang tulus kepada para pembuat di atas!**

---

## Perangkat Lunak Pihak Ketiga

Proyek ini menggunakan program dan pustaka pihak ketiga, hak cipta milik pengembang masing-masing.
