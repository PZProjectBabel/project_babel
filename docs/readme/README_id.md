# Project Babel — Proyek Terjemahan LLM Otomatis untuk Mod Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Proyek terjemahan ini didukung dan dipelihara oleh toolset [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Daftar Isi

- [Bahasa target yang didukung proyek](#bahasa-target-yang-didukung-proyek)
- [Cara Memasang dan Menggunakan](#cara-memasang-dan-menggunakan)
- [Kemajuan Terjemahan](#kemajuan-terjemahan)
- [Cara Berkontribusi](#cara-berkontribusi)
- [Alat dan Struktur Direktori (Untuk Pengembang)](#alat-dan-struktur-direktori-untuk-pengembang)
  - [Direktori Proyek](#direktori-proyek)
  - [Modul Pipeline (Berdasarkan Urutan Eksekusi)](#modul-pipeline-berdasarkan-urutan-eksekusi)
  - [Modul Independen](#modul-independen)
  - [Tumpukan Teknologi](#tumpukan-teknologi)
- [Hak Cipta dan Lisensi](#hak-cipta-dan-lisensi)
  - [1. Teks dan gambar, dll.](#1-teks-dan-gambar-dll)
  - [2. Program, skrip, dan konten pengembangan lainnya](#2-program-skrip-dan-konten-pengembangan-lainnya)
- [Ucapan Terima Kasih](#ucapan-terima-kasih)
- [Program Pihak Ketiga](#program-pihak-ketiga)

---

## Bahasa target yang didukung proyek

| Bahasa | Nama Lokal | Kode Internasional | Kode Dalam Game | Didukung? | Catatan |
|------|------|------|------|------|------|
| Bahasa Arab | العربية | `ar` | `AR` | ❌ | Token tidak mencukupi |
| Bahasa Katalan | català | `ca` | `CA` | ❌ | Token tidak mencukupi |
| Bahasa Tionghoa Tradisional | 繁體中文 | `zh-hant` | `CH` | ❌ | Token tidak mencukupi |
| Bahasa Tionghoa Sederhana | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Bahasa Ceko | čeština | `cs` | `CS` | ❌ | Token tidak mencukupi |
| Bahasa Denmark | dansk | `da` | `DA` | ❌ | Token tidak mencukupi |
| Bahasa Jerman | Deutsch | `de` | `DE` | ✅ | |
| Bahasa Inggris | English | `en` | `EN` | ✅ | |
| Bahasa Spanyol | español | `es` | `ES` | ❌ | Token tidak mencukupi |
| Bahasa Finlandia | suomi | `fi` | `FI` | ❌ | Token tidak mencukupi |
| Bahasa Prancis | français | `fr` | `FR` | ✅ | |
| Bahasa Hungaria | magyar | `hu` | `HU` | ❌ | Token tidak mencukupi |
| Bahasa Indonesia | Bahasa Indonesia | `id` | `ID` | ❌ | Token tidak mencukupi |
| Bahasa Italia | italiano | `it` | `IT` | ❌ | Token tidak mencukupi |
| Bahasa Jepang | 日本語 | `ja` | `JP` | ✅ | |
| Bahasa Korea | 한국어 | `ko` | `KO` | ❌ | Token tidak mencukupi |
| Bahasa Belanda | Nederlands | `nl` | `NL` | ❌ | Token tidak mencukupi |
| Bahasa Norwegia | norsk | `no` | `NO` | ❌ | Token tidak mencukupi |
| Bahasa Tagalog | Tagalog | `tl` | `PH` | ❌ | Token tidak mencukupi |
| Bahasa Polandia | polski | `pl` | `PL` | ❌ | Token tidak mencukupi |
| Bahasa Portugis (Portugal) | português | `pt` | `PT` | ❌ | Token tidak mencukupi |
| Bahasa Portugis (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token tidak mencukupi |
| Bahasa Rumania | română | `ro` | `RO` | ❌ | Token tidak mencukupi |
| Bahasa Rusia | русский | `ru` | `RU` | ❌ | Token tidak mencukupi |
| Bahasa Thai | ภาษาไทย | `th` | `TH` | ❌ | Token tidak mencukupi |
| Turki | Türkçe | `tr` | `TR` | ❌ | Token tidak mencukupi |
| Ukraina | українська | `uk` | `UA` | ❌ | Token tidak mencukupi |

**Total**: 27 bahasa yang direncanakan | **Didukung**: 5 | **Menunggu dukungan**: 22

---

## Cara Memasang dan Menggunakan

Ini adalah panduan bagi pemain yang ingin langsung menggunakan proyek terjemahan ini dalam game.

1.  Kunjungi halaman Steam Workshop kami: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Klik tombol 'Subscribe'.
3.  Mulai game, aktifkan mod terjemahan ini di menu 'Mod' pada menu utama game.
4.  Teks terjemahan dari mod yang diaktifkan lebih akhir akan menimpa mod yang diaktifkan lebih awal, oleh karena itu mod terjemahan ini harus diaktifkan setelah mod fungsional (sebisa mungkin di posisi paling bawah).
5.  Nikmati permainan!

---

## Kemajuan Terjemahan

**[➡️ Klik di sini untuk melihat kemajuan terjemahan](./docs/progress/progress_id.md)**

---

## Cara Berkontribusi

Kami menyambut siapa pun untuk berkontribusi, baik memperbaiki kesalahan, menambahkan fitur baru, menulis template prompt, atau memberikan terjemahan referensi!

Memanggil API LLM untuk menerjemahkan memerlukan pembayaran per token. Agar proyek ini dapat berjalan stabil dalam jangka panjang, kami berharap Anda dapat membantu dengan murah hati!

Detailnya baca [Panduan Kontribusi](./docs/contributing/contributing_id.md)

---

## Alat dan Struktur Direktori (Untuk Pengembang)

Bagian ini ditujukan bagi pengembang yang ingin memahami prinsip otomatisasi proyek.

### Direktori Proyek

| Direktori | Keterangan |
|------|------|
| `src/` | Kode sumber pipeline terjemahan .NET 10, berisi 15 modul + 2 modul independen |
| `config/` | File konfigurasi pipeline (parameter LLM, Steam, RAG, dll.) |
| `data/` | Data runtime: metadata mod, embedding, cache terjemahan |
| `translation_ref/` | Data terjemahan referensi (misalnya mod berlisensi dari Grup Hanhua), menyediakan referensi terjemahan untuk LLM |
| `base_game_keys/` | Kunci terjemahan game asli, digunakan untuk deduplikasi agar tidak menimpa teks asli |
| `final_outputs/` | Output akhir: paket mod `project_babel/`, ikon `icons/`, dan deskripsi workshop `workshop_descriptions/` |
| `docs/` | Dokumentasi proyek: laporan kemajuan, panduan kontribusi, penjelasan pipeline |
| `temp/` | File sementara pipeline (direktori terpisah setiap kali dijalankan) |
| `src/prompt_templates/` | Template prompt LLM (terjemahan/pemeriksaan konten) |

### Modul Pipeline (Berdasarkan Urutan Eksekusi)

| Langkah | Modul | Fungsi |
|------|------|------|
| 1 | `ConfigReader` | Memuat daftar konfigurasi/kunci/bahasa |
| 2 | `RepoDataLoader` | Memuat referensi terjemahan dan cache terjemahan |
| 3 | `ModIdCollector` | Mengumpulkan ID modul Workshop |
| 4 | `ModInfoFetcher` | Mendapatkan metadata Steam |
| 5 | `SteamCmdBootstrapper` | Menyiapkan runtime steamcmd untuk platform saat ini |
| 6 | `ModDownloader` | Mengunduh modul melalui steamcmd |
| 7 | `ContentExtractor` | Mengurai file terjemahan modul → `TranslationEntry` |
| 8 | `ContentChecker` | Pemeriksaan keamanan konten (narkoba/pornografi/kekerasan) |
| 9 | `EmbeddingFetcher` | Menghitung vektor embedding teks |
| 10 | `TranslationBatcher` | Membuat batch terjemahan yang tidak tergantung bahasa target |
| 11 | `RagContextRetriever` | Mengambil konteks RAG (kunci tepat + kemiripan embedding) |
| 12 | `LLMTranslator` | Memanggil LLM untuk melakukan terjemahan |
| 13 | `ResultWriter` | Menulis ke data/ dan translation_ref/ |
| 14 | `FinalOutputWriter` | Menghasilkan output format modul PZ final |
| 15 | `ProgressReporter` | Menghasilkan laporan kemajuan |

### Modul Independen

| Modul | Fungsi |
|------|------|
| `WorkshopMonitor` | Secara berkala mengambil modul baru dari Steam Workshop, menyaring berdasarkan jumlah langganan, dan menambahkannya ke `request_for_translation.txt` |
| `DocGenerator` | Pembuat dokumen multibahasa yang digerakkan oleh LLM |

### Tumpukan Teknologi

- **Bahasa**: C# (.NET 10)
- **Platform target**: GitHub Actions Linux x64 runner
- **Pengujian**: xUnit (Windows x64)
- **LLM**: DeepSeek API (dapat dikonfigurasi)
- **Embedding**: Vektorisasi teks untuk pencarian kemiripan RAG
- **Pemeriksaan konten**: Audit keamanan multi-level yang digerakkan oleh LLM

Rincian [referensi teknis](./docs/technical_reference/technical_reference_id.md).

---

## Hak Cipta dan Lisensi

Konten teks terjemahan dan gambar terkait dari proyek terjemahan ini dibuat atau dibuat ulang oleh **Project Babel** dan para peserta berdasarkan mod game asli.

© 2025 Project Babel dan masing-masing penulis mempertahankan hak.

### 1. Teks dan gambar, dll.

Kecuali dijelaskan lain, dalam repositori ini:

- Terjemahan teks dalam game, penyempurnaan, dan konten proofreading;
Project documentation, mod translation texts;
Images and art resources specially produced for this project

semuanya dilisensikan di bawah **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International** (disingkat **CC BY-NC-SA 4.0**).

Ini berarti, dengan mematuhi ketentuan berikut, Anda bebas untuk berbagi dan mengadaptasi konten ini:

- **Atribusi (BY)**: Cantumkan di tempat yang jelas bahwa "proyek terjemahan ini didasarkan pada karya 'Project Babel' yang telah dimodifikasi", dan sertakan tautan ke repositori ini serta Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Non-komersial (NC)**: Anda tidak boleh menggunakan konten proyek ini atau karya turunannya untuk tujuan komersial langsung atau tidak langsung (termasuk namun tidak terbatas pada paket berbayar, unduhan berbayar, bagi hasil iklan, dll.);
- **Berbagi Serupa (SA)**: Jika Anda memodifikasi atau membuat ulang berdasarkan konten proyek ini, Anda harus merilis versi modifikasi Anda secara publik di bawah **lisensi CC BY-NC-SA 4.0 yang sama**.

Untuk informasi lebih lanjut tentang lisensi ini, lihat:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.id>

*Catatan khusus:*
- *konten folder base_game_keys berasal dari game asli, hak cipta milik pengembang game! Konten digunakan untuk mencegah kunci terjemahan menimpa kunci game (deduplikasi)*
- *konten folder translation_ref digunakan untuk memberikan referensi terjemahan kepada LLM, hak cipta milik masing-masing pengembang mod!*

### 2. Program, skrip, dan konten pengembangan lainnya

Kecuali dinyatakan lain dalam file atau direktori sumber, kode program di repositori ini yang digunakan untuk membuat/mengemas/memproses konten lokalisasi (misalnya kode program di direktori `src/`) dilisensikan di bawah **GNU General Public License versi 3 (GPL-3.0)**.

Lihat ketentuan lengkap di file `LICENSE` di root repositori ini (GPL-3.0), atau kunjungi situs web GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Ucapan Terima Kasih

Proyek ini menggunakan mod pihak ketiga sebagai teks referensi untuk terjemahan bahasa sasaran. Teks referensi dikirim ke LLM untuk referensi terjemahan.

| Nama Mod Referensi | Penulis | Halaman Mod |
|------|------|------|
| [B42] Terjemahan Mandarin Terpadu | Kelompok Terjemahan Ruyi (As1) | [Halaman Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Terjemahan Mod Terpadu | Kelompok Terjemahan Ruyi (As1) | [Halaman Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Terjemahan Ark Terpadu | Kelompok Terjemahan Ruyi (As1) | [Halaman Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Terima kasih yang sebesar-besarnya kepada para penulis di atas!**

---

## Program Pihak Ketiga

Proyek ini menggunakan program dan pustaka pihak ketiga. Hak cipta program pihak ketiga tersebut dimiliki oleh pengembang masing-masing.

