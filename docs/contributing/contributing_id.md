# Panduan Kontribusi (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Daftar Isi

- [1. Sebelum Memulai](#1-sebelum-memulai)
- [2. Bagaimana Saya Dapat Berkontribusi?](#2-bagaimana-saya-dapat-berkontribusi)
- [3. Menyediakan Aturan Terjemahan, Kamus Istilah, Memperbaiki Prompt Sistem](#3-menyediakan-aturan-terjemahan-kamus-istilah-memperbaiki-prompt-sistem)
- [4. Menyediakan Korpora yang Dikoreksi Manusia](#4-menyediakan-korpora-yang-dikoreksi-manusia)
- [5. Kontribusi Pipeline dan Pengembangan Alat](#5-kontribusi-pipeline-dan-pengembangan-alat)
- [6. Hak Cipta dan Perjanjian Lisensi](#6-hak-cipta-dan-perjanjian-lisensi)
  - [6.1 Prinsip Dasar: Anda mempertahankan hak cipta, sekaligus memberikan lisensi kepada proyek untuk digunakan](#61-prinsip-dasar-anda-mempertahankan-hak-cipta-sekaligus-memberikan-lisensi-kepada-proyek-untuk-digunakan)
  - [6.2 Lisensi untuk Teks dan Gambar (CC BY-NC-SA 4.0)](#62-lisensi-untuk-teks-dan-gambar-cc-by-nc-sa-40)
  - [6.3 Lisensi untuk Skrip dan Kode Alat (GPL-3.0)](#63-lisensi-untuk-skrip-dan-kode-alat-gpl-30)
  - [6.4 Hak cipta karya upstream dan game asli](#64-hak-cipta-karya-upstream-dan-game-asli)
- [7. Komunikasi dan Kolaborasi](#7-komunikasi-dan-kolaborasi)
- [8. Dukungan Dana](#8-dukungan-dana)

---

Sungguh terima kasih Anda bersedia berkontribusi pada **Project Babel - Proyek Terjemahan Otomatis LLM Mod untuk Project Zomboid**! Baik itu memperbaiki kesalahan, menambahkan fitur baru, menulis templat prompt, atau menyediakan terjemahan referensi!

Memanggil API LLM untuk menerjemahkan memerlukan biaya token, agar proyek dapat berjalan stabil dalam jangka panjang, semoga Anda dapat membantu dengan murah hati!

> ⚠️ **Pengingat Penting:**
> Sebelum mengirimkan konten apa pun ke repositori ini, pastikan untuk membaca dan memahami bagian "Perjanjian Hak Cipta dan Lisensi".
> Setelah dikirimkan dan digabungkan, itu berarti Anda menyetujui ketentuan lisensi yang sesuai.

---

## 1. Sebelum Memulai

Silakan baca `README.md` proyek terlebih dahulu, untuk memahami:
- Tujuan keseluruhan dan status terkini proyek ini;
- Bagaimana pemain biasa menggunakan proyek ini (memudahkan Anda menguji sendiri);
- Detail teknis proyek.

---

## 2. Bagaimana Saya Dapat Berkontribusi?

Anda dapat memilih satu atau lebih cara untuk berpartisipasi berdasarkan minat dan keterampilan Anda:

- Menyediakan aturan terjemahan untuk bahasa target
- Menyediakan kamus istilah terjemahan untuk bahasa target
- Memperbaiki prompt sistem
- Menyediakan korpus teks terjemahan yang telah dikoreksi secara manual
- Memperbaiki modul pipeline (.NET) dan skrip otomatisasi
- Melaporkan masalah, mengajukan saran perbaikan (jelaskan di Issues)
- Memberikan dukungan dana untuk pemanggilan LLM

Berikut adalah penjelasan untuk beberapa skenario kontribusi utama.

---

## 3. Menyediakan Aturan Terjemahan, Kamus Istilah, Memperbaiki Prompt Sistem

Templat prompt pipeline terletak di `src/prompt_templates/`, dengan struktur sebagai berikut:

- `system_prompt_translate_engine.txt`: Prompt sistem mesin terjemahan global (digunakan bersama untuk semua bahasa);
- `<kode bahasa>/translation_dictionary_<kode bahasa>.json`: Kamus istilah untuk bahasa tersebut;
- `<kode bahasa>/translation_schema_<kode bahasa>.md`: Aturan terjemahan dan batasan gaya untuk bahasa tersebut.

Langkah kontribusi:

1. Buat subdirektori untuk bahasa Anda di bawah `src/prompt_templates/`, tambahkan kamus istilah dan file aturan terjemahan;
2. Jika perlu menyesuaikan perilaku terjemahan global, ubah `system_prompt_translate_engine.txt` (perhatikan akan memengaruhi semua bahasa);
3. Uji coba lokal untuk mengonfirmasi efeknya;
4. Kirim PR.

---

## 4. Menyediakan Korpora yang Dikoreksi Manusia

Jika Anda adalah pembuat mod terjemahan dan bersedia menyediakan korpora terjemahan Anda sebagai referensi penerjemahan LLM, ajukan permohonan di Issue. Anda perlu memberikan informasi berikut:

- ID Mod dari mod terjemahan Anda dan bahasa target terjemahan;
- Tangkapan layar halaman belakang mod terjemahan Anda untuk membuktikan bahwa Anda adalah pembuat mod;
- Nyatakan dengan jelas di Issue bahwa Anda bersedia menyediakan korpora terjemahan;
- Jika ada situasi khusus (lisensi khusus, dll.), harap sebutkan juga;
- Pastikan korpora yang Anda sediakan memiliki kualitas yang tinggi.

Dengan otorisasi Anda, proyek akan mencantumkan mod Anda dalam daftar `config/ref_translation_mods.json` referensi mod terjemahan, dan pipeline akan secara otomatis menyinkronkan teks terjemahan Anda sebagai korpora referensi RAG.

---

## 5. Kontribusi Pipeline dan Pengembangan Alat

Otomatisasi proyek ini terbagi menjadi dua bagian:

**Modul Pipeline (`src/`, C# / .NET 10)**: Berisi 15 modul yang dieksekusi secara berurutan, ditambah 2 modul independen (`WorkshopMonitor` penemu mod, `DocGenerator` generator dokumen), bertanggung jawab atas seluruh alur dari inisialisasi SteamCMD, unduhan mod, ekstraksi teks, peninjauan konten, perhitungan Embedding, pencarian RAG hingga terjemahan LLM dan output akhir. Lihat [Referensi Teknis](../technical_reference/technical_reference_id.md).

**Skrip Pembantu (.github/)**: Digunakan untuk otomatisasi github.

Jika Anda ingin:

* Memperbaiki bug pada modul pipeline atau skrip yang ada;
* Menambahkan fitur baru atau modul baru ke pipeline;
* Mengoptimalkan kinerja atau struktur kode;
* Meningkatkan template prompt atau strategi RAG;

Anda dapat mengikuti langkah-langkah berikut:

1. Fork repositori ini dan clone ke lokal;
2. Buat cabang baru berdasarkan cabang terbaru;
3. Ubah atau tambahkan file di direktori yang sesuai:
- Modifikasi modul pipeline → `src/<nama_modul>/`;
- Modifikasi alur kerja CI → `.github/workflows/`;
- Modifikasi template Prompt → `src/prompt_templates/`;
4. Sebelum mengirim, usahakan sebisa mungkin:

* Pertahankan gaya kode asli;
* Tambahkan komentar yang diperlukan;
* Jika memungkinkan, sertakan pengujian sederhana atau petunjuk penggunaan;
5. Kirimkan modifikasi melalui PR, dan jelaskan dalam deskripsi:

* Tujuan perubahan;
* Direktori / modul / skrip yang mungkin terpengaruh;
* Apakah melibatkan perubahan yang merusak.

---

## 6. Hak Cipta dan Perjanjian Lisensi

> **Pengingat:**
> Perjanjian hak cipta dan lisensi ini bertujuan untuk melindungi hak dan kepentingan sah proyek, penulis, kontributor, dan pemain, menghindari kesalahpahaman karena "kesepakatan diam" atau "default". Harap baca dengan saksama.
> Hak cipta dan lisensi mengacu pada konten dalam file README.md, bagian ini hanya memberikan deskripsi yang lebih mudah dipahami.

### 6.1 Prinsip Dasar: Anda mempertahankan hak cipta, sekaligus memberikan lisensi kepada proyek untuk digunakan

* Anda masih memiliki hak cipta atas konten yang Anda buat (terjemahan, gambar, skrip/program, dll.);
* Namun setelah mengirimkan konten ini ke proyek ini dan diterima (digabungkan), Anda setuju untuk memberikan lisensi kepada orang lain untuk menggunakan konten ini sesuai dengan lisensi sumber terbuka/berbagi yang diadopsi oleh proyek ini.

Ini berarti:

* Anda **masih dapat** terus menggunakan dan menampilkan karya Anda di tempat lain;
* Namun Anda **tidak dapat** meminta proyek ini atau pengguna lain yang telah memperoleh karya secara sah untuk "mencabut lisensi" atau "menghapus versi lama" setelah kontribusi digabungkan.

### 6.2 Lisensi untuk Teks dan Gambar (CC BY-NC-SA 4.0)

Untuk konten berikut yang Anda kirimkan:

* Terjemahan teks game, penyempurnaan dan koreksi konten;
* Dokumentasi proyek, teks penjelasan;
* Gambar, sumber daya seni yang dibuat khusus untuk proyek ini;

Setelah diadopsi dan digabungkan ke dalam repositori ini, Anda dianggap setuju:

1. Konten ini dilisensikan di bawah lisensi **Atribusi-NonKomersial-BerbagiSerupa 4.0 Internasional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, disingkat **CC BY-NC-SA 4.0**);
2. Project Babel dan semua pengguna yang memperoleh konten ini dapat, dengan **mematuhi ketentuan CC BY-NC-SA 4.0**:
* Membagikan, menyalin, dan mendistribusikan ulang konten ini;
* Memodifikasi dan menciptakan ulang konten ini untuk penggunaan non-komersial;
3. Anda setuju bahwa, sejauh diizinkan oleh hukum yang berlaku, lisensi ini adalah lisensi **non-eksklusif, global, bebas royalti, dan tidak dapat dicabut**;
4. Bahkan jika Anda keluar atau berhenti berpartisipasi dalam proyek ini di masa mendatang, proyek ini masih dapat terus menggunakan dan mendistribusikan ulang konten terkait yang telah Anda kirimkan dan digabungkan berdasarkan CC BY-NC-SA 4.0.

> Jika Anda tidak menerima metode lisensi di atas, jangan kirimkan kontribusi teks atau gambar ke proyek ini,
> atau komunikasikan terlebih dahulu dengan pemelihara proyek untuk memastikan apakah dapat berkolaborasi dengan cara lain.

### 6.3 Lisensi untuk Skrip dan Kode Alat (GPL-3.0)

Untuk konten yang Anda kirimkan dan diterima:

* Skrip otomatisasi;
* Alat pembangunan/ekspor;
* Kode program lainnya yang digunakan untuk memproses proyek lokalisasi ini;

Dengan tidak ada pernyataan khusus, dianggap Anda setuju:

1. Kode dilisensikan di bawah **GPL-3.0** (GNU General Public License versi 3);
2. Pemelihara proyek dapat memodifikasi, menggabungkan, dan mendistribusikannya dalam batas yang diizinkan oleh GPL-3.0;
3. Anda juga dapat melanjutkan proyek lain berdasarkan kode yang sama, selama mematuhi ketentuan GPL-3.0.

Untuk menghindari konflik lisensi, harap sebisa mungkin:

* Jangan memperkenalkan kode pihak ketiga yang **tidak kompatibel dengan GPL-3.0** tanpa verifikasi;
* Jika benar-benar perlu merujuk pustaka pihak ketiga, jelaskan sumber dan lisensinya dengan jelas di PR, dan konfirmasi kompatibilitasnya.

### 6.4 Hak cipta karya upstream dan game asli

Proyek ini adalah proyek **terjemahan tidak resmi** untuk mod terkait Project Zomboid (Project Zomboid).

* Hak cipta game asli dan masing-masing mod dimiliki oleh penulis/penerbit masing-masing;
* Proyek ini hanya berfokus pada pembuatan dan pengorganisasian terjemahan teks, penyempurnaan, dan sebagian sumber daya pendukung;
* Kontributor, saat mengirimkan konten, harus memastikan:
* Tidak menyalin secara langsung teks terjemahan atau sumber daya seni pihak ketiga yang tidak resmi;
* Menghormati hak penulis asli dan pembuat mod, tidak melakukan repost yang melanggar hak cipta.

---

## 7. Komunikasi dan Kolaborasi

Jika Anda memiliki pertanyaan tentang:

* Ketentuan lisensi;
* Tidak yakin apakah konten tertentu dapat dikontribusikan;
* Ingin melisensikan karya Anda dengan cara khusus (misalnya hanya mengizinkan non-komersial tetapi tidak mengizinkan adaptasi, dll.);

Silakan hubungi pemelihara proyek melalui cara berikut:

* Kirim Issue untuk berdiskusi;
* Kontak lain yang disediakan secara publik oleh pemelihara.

Kami akan berusaha semaksimal mungkin, dengan tetap menghormati hak dan kepentingan semua pihak, untuk menemukan solusi yang menyeimbangkan perkembangan proyek yang sehat.

---

## 8. Dukungan Dana

Selama operasi proyek, karena penambahan mod baru, pembaruan konten teks mod lama, dll., perlu terus-menerus memanggil LLM API untuk menerjemahkan. Dan untuk membatasi perilaku LLM, selain teks mod dasar, juga perlu menyediakan banyak konten prompt (termasuk prompt dasar, aturan terjemahan, glosarium, batasan input-output, hasil kueri semantik, dll.), konten-konten ini akan mengkonsumsi token yang jauh melebihi teks asli. Oleh karena itu, proyek membutuhkan dukungan dana.

Jika Anda bersedia memberikan dukungan dana, silakan hubungi pemelihara proyek. Terima kasih banyak!

---

Terima kasih sekali lagi karena Anda bersedia berkontribusi untuk proyek ini!
Setiap kontribusi Anda akan membuat lebih banyak pemain mendapatkan manfaat!
