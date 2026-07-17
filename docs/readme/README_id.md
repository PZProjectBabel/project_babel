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
  - [技术栈](#技术栈)
- [版权与授权](#版权与授权)
  - [1. 文本与图片等内容](#1-文本与图片等内容)
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
| `src/` | Kode sumber pipeline terjemahan .NET 10, berisi 15 modul |
| `config/` | File konfigurasi pipeline (parameter LLM, Steam, RAG, dll.) |
| `data/` | Data runtime: metadata mod, embedding, cache terjemahan |
| `translation_ref/` | Data terjemahan referensi (misalnya mod berlisensi dari Grup Hanhua), menyediakan referensi terjemahan untuk LLM |
| `base_game_keys/` | Kunci terjemahan game asli, digunakan untuk deduplikasi agar tidak menimpa teks asli |
| `final_outputs/` | Output akhir: paket mod `project_babel/`, ikon `icons/`, dan deskripsi workshop `workshop_descriptions/` |
| `docs/` | Dokumentasi proyek: laporan kemajuan, panduan kontribusi, penjelasan pipeline |
| `temp/` | File sementara pipeline (direktori terpisah setiap kali dijalankan) |
| `src/prompt_templates/` | Template prompt LLM (terjemahan/pemeriksaan konten) |

### Modul Pipeline (Berdasarkan Urutan Eksekusi)

| 步骤 | 模块 | 功能 |
|------|------|------|
| 1 | `ConfigReader` | 加载配置/密钥/语言列表 |
| 2 | `RepoDataLoader` | 加载参考翻译与翻译缓存 |
| 3 | `ModIdCollector` | 收集 Workshop 模组 ID |
| 4 | `ModInfoFetcher` | 获取 Steam 元数据 |
| 5 | `SteamCmdBootstrapper` | 准备当前平台的 steamcmd 运行时 |
| 6 | `ModDownloader` | 通过 steamcmd 下载模组 |
| 7 | `ContentExtractor` | 解析模组翻译文件 → `TranslationEntry` |
| 8 | `ContentChecker` | 内容安全审查 (毒品/色情/暴力) |
| 9 | `EmbeddingFetcher` | 计算文本 embedding 向量 |
| 10 | `TranslationBatcher` | 创建目标语言无关的翻译批次 |
| 11 | `RagContextRetriever` | 检索 RAG 上下文 (精确键 + embedding 相似度) |
| 12 | `LLMTranslator` | 调用 LLM 执行翻译 |
| 13 | `ResultWriter` | 写入 data/ 与 translation_ref/ |
| 14 | `FinalOutputWriter` | 生成最终 PZ 模组格式输出 |
| 15 | `ProgressReporter` | 生成进度报告 |

### 技术栈

- **语言**: C# (.NET 10)
- **目标平台**: GitHub Actions Linux x64 runner
- **测试**: xUnit (Windows x64)
- **LLM**: DeepSeek API (可配置)
- **Embedding**: 文本向量化用于 RAG 相似检索
- **内容审查**: LLM 驱动的多级安全审核

详细的 [技术参考](./docs/technical_reference/technical_reference_id.md)。

---

## 版权与授权

本翻译项目的翻译文本内容与相关图片，由 **Project Babel** 与各参与者基于原游戏模组创作或二次创作完成。

© 2025 Project Babel 及各作者保留权利。

### 1. 文本与图片等内容

除非另有特别说明，本仓库中的：

- 游戏内文本翻译、润色与校对内容；
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

