# Panduan Kontribusi (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Terima kasih atas kesediaan Anda untuk berkontribusi pada **Project Babel — proyek terjemahan otomatis berbasis LLM untuk mod Project Zomboid**! Baik itu memperbaiki bug, menambahkan fitur, menulis templat prompt, atau menyediakan terjemahan referensi — setiap kontribusi berarti!

Memanggil API LLM untuk terjemahan memerlukan biaya token. Agar proyek dapat berjalan secara berkelanjutan dalam jangka panjang, dukungan Anda yang murah hati sangat dihargai!

> ⚠️ **Peringatan Penting:**
> Sebelum mengirimkan apa pun ke repositori ini, pastikan untuk membaca dan memahami bagian "Hak Cipta & Lisensi".
> Setelah dikirim dan digabungkan, Anda dianggap telah menyetujui ketentuan lisensi yang berlaku.

---

## Sebelum Anda Mulai

Silakan baca `README.md` proyek untuk memahami:

- Tujuan keseluruhan dan status terkini proyek ini;
- Bagaimana pemain biasa menggunakan proyek ini (untuk pengujian Anda sendiri);
- Detail teknis proyek.

---

## Bagaimana Saya Dapat Berkontribusi?

Anda dapat memilih satu atau lebih cara untuk berpartisipasi berdasarkan minat dan keterampilan Anda:

- Menyediakan aturan terjemahan untuk bahasa target
- Menyediakan kamus istilah untuk bahasa target
- Meningkatkan prompt sistem
- Menyediakan korpora terjemahan yang dikoreksi secara manual
- Meningkatkan modul pipeline (.NET) dan skrip otomatisasi
- Melaporkan masalah dan menyarankan perbaikan (melalui Issues)
- Memberikan dukungan finansial untuk panggilan API LLM

Di bawah ini adalah penjelasan untuk skenario kontribusi utama.

---

## Menyediakan Aturan Terjemahan, Kamus Istilah, dan Meningkatkan Prompt Sistem

Templat prompt pipeline terletak di `src/prompt_templates/`, dengan struktur berikut:

- `system_prompt_translate_engine.txt`: prompt sistem mesin terjemahan global (digunakan bersama oleh semua bahasa);
- `<kode_bahasa>/translation_dictionary_<kode_bahasa>.json`: kamus istilah untuk bahasa tersebut;
- `<kode_bahasa>/translation_schema_<kode_bahasa>.md`: aturan terjemahan dan batasan gaya untuk bahasa tersebut.

Langkah-langkah kontribusi:

1. Buat subdirektori di bawah `src/prompt_templates/` untuk bahasa Anda dan tambahkan file kamus serta aturan terjemahan;
2. Jika Anda perlu menyesuaikan perilaku terjemahan global, ubah `system_prompt_translate_engine.txt` (catatan: ini memengaruhi semua bahasa);
3. Uji secara lokal untuk mengonfirmasi hasil;
4. Kirim PR.

---

## Menyediakan Korpora yang Dikoreksi Secara Manual

Jika Anda adalah pembuat mod terjemahan dan bersedia menyediakan korpus terjemahan Anda sebagai referensi terjemahan LLM, silakan ajukan permintaan melalui Issue. Anda perlu memberikan informasi berikut:

- Mod ID mod terjemahan Anda dan bahasa target;
- Tangkapan layar halaman administrasi mod terjemahan Anda untuk membuktikan bahwa Anda adalah pembuatnya;
- Pernyataan jelas dalam Issue bahwa Anda bersedia menyediakan korpus terjemahan;
- Jika ada keadaan khusus (lisensi khusus, dll.), mohon jelaskan;
- Pastikan korpus yang Anda sediakan berkualitas tinggi.

Dengan otorisasi Anda, proyek akan menambahkan mod Anda ke daftar mod terjemahan referensi `config/ref_translation_mods.json`, dan pipeline akan secara otomatis menyinkronkan teks terjemahan Anda sebagai korpora referensi RAG.

---

## Kontribusi Pengembangan Pipeline dan Alat

Otomatisasi dalam proyek ini dibagi menjadi dua bagian:

**Modul pipeline (`src/`, C# / .NET 10)**: Berisi 15 modul yang dieksekusi secara berurutan, bertanggung jawab atas alur kerja lengkap dari pengunduhan mod, ekstraksi teks, peninjauan konten, perhitungan embedding, pengambilan RAG hingga terjemahan LLM dan output akhir. Lihat [dokumentasi teknis](../translation_entry_pipeline_zh-hans.md) untuk detailnya.

**Skrip bantuan (`.github/`)**: Digunakan untuk otomatisasi GitHub.

Jika Anda ingin:

* Memperbaiki bug dalam modul pipeline atau skrip yang ada;
* Menambahkan fitur atau modul baru ke pipeline;
* Mengoptimalkan kinerja atau struktur kode;
* Meningkatkan templat prompt atau strategi RAG;

Anda dapat mengikuti langkah-langkah berikut:

1. Fork repositori ini dan klon secara lokal;
2. Buat cabang baru dari cabang terbaru;
3. Ubah atau tambahkan file di direktori yang sesuai:
   - Perubahan modul pipeline → `src/<nama_modul>/`;
   - Perubahan skrip → `scripts/`;
   - Perubahan templat prompt → `src/prompt_templates/`;
4. Sebelum mengirim, usahakan:

   * Mempertahankan gaya kode yang ada;
   * Menambahkan komentar yang diperlukan;
   * Jika memungkinkan, sertakan tes sederhana atau instruksi penggunaan;
5. Kirim perubahan melalui PR, dan jelaskan dalam deskripsi:

   * Tujuan perubahan;
   * Direktori / modul / skrip yang mungkin terpengaruh;
   * Apakah melibatkan perubahan yang merusak kompatibilitas.

---

## Hak Cipta & Lisensi

> **Pengingat Ramah:**
> Ketentuan hak cipta dan lisensi dirancang untuk melindungi hak dan kepentingan yang sah dari proyek, penulis, kontributor, dan pemain, serta untuk menghindari kesalahpahaman yang timbul dari "kesepakatan diam-diam" atau "asumsi default". Harap bacalah dengan saksama.
> Hak cipta dan lisensi diatur oleh konten dalam file README.md; bagian ini hanya memberikan deskripsi yang lebih mudah diakses.

### 1. Prinsip Dasar: Anda mempertahankan hak cipta, sambil melisensikan proyek untuk menggunakan karya Anda

* Anda masih memegang hak cipta atas konten yang Anda buat (terjemahan, gambar, skrip/program, dll.);
* Namun, setelah konten ini dikirim ke proyek ini dan diterima (digabungkan),
  Anda setuju untuk melisensikan kepada pihak lain penggunaan konten ini di bawah lisensi sumber terbuka/bersama yang diadopsi oleh proyek ini.

Ini berarti:

* Anda **masih dapat** terus menggunakan dan menampilkan karya Anda di tempat lain;
* Tetapi Anda **tidak dapat**, setelah kontribusi Anda digabungkan, menuntut proyek ini atau pengguna lain yang telah memperoleh karya secara sah untuk "mencabut lisensi" atau "menghapus versi historis".

### 2. Lisensi Teks, Gambar, dan Konten Serupa (CC BY-NC-SA 4.0)

Untuk konten berikut yang Anda kirimkan:

* Terjemahan teks game, penyempurnaan, dan koreksi;
* Dokumentasi proyek dan teks penjelasan;
* Gambar dan sumber daya artistik yang dibuat khusus untuk proyek ini;

Setelah diterima dan digabungkan ke repositori ini, Anda dianggap menyetujui bahwa:

1. Konten ini dilisensikan di bawah **Atribusi-NonKomersial-BerbagiSerupa 4.0 Internasional**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, disingkat **CC BY-NC-SA 4.0**);
2. Project Babel dan semua pengguna yang menerima konten ini dapat, **dengan mematuhi ketentuan CC BY-NC-SA 4.0**:

   * Berbagi, menyalin, dan mendistribusikan ulang konten ini;
   * Memodifikasinya dan membuat karya turunan untuk tujuan non-komersial;
3. Anda setuju bahwa sejauh diizinkan oleh hukum yang berlaku, lisensi ini bersifat **non-eksklusif, di seluruh dunia, bebas royalti, dan tidak dapat dibatalkan**;
4. Bahkan jika Anda kemudian mundur atau berhenti berpartisipasi dalam proyek ini, proyek dapat terus menggunakan dan mendistribusikan ulang konten relevan yang telah Anda kirim dan telah digabungkan, di bawah CC BY-NC-SA 4.0.

> Jika Anda tidak menerima ketentuan lisensi di atas, mohon jangan mengirim kontribusi teks atau gambar ke proyek ini,
> atau berkomunikasi terlebih dahulu dengan pengelola proyek untuk mengonfirmasi apakah kolaborasi dimungkinkan dengan cara lain.

### 3. Lisensi Skrip dan Kode Alat (GPL-3.0)

Untuk hal berikut yang Anda kirim dan diterima:

* Skrip otomatisasi;
* Alat build/ekspor;
* Kode program lain yang digunakan untuk memproses proyek terjemahan ini;

Dengan tidak adanya pernyataan khusus, Anda dianggap menyetujui bahwa:

1. Kode dilisensikan di bawah **GPL-3.0** (GNU General Public License versi 3);
2. Pengelola proyek dapat memodifikasi, menggabungkan, dan mendistribusikannya dalam lingkup yang diizinkan oleh GPL-3.0;
3. Anda juga dapat melanjutkan proyek lain berdasarkan kode yang sama, selama Anda mematuhi ketentuan GPL-3.0.

Untuk menghindari konflik lisensi, usahakan:

* Tidak memperkenalkan kode pihak ketiga yang **tidak kompatibel dengan GPL-3.0** tanpa konfirmasi sebelumnya;
* Jika Anda perlu merujuk ke pustaka pihak ketiga, nyatakan dengan jelas sumber dan lisensinya di PR dan konfirmasikan kompatibilitasnya.

### 4. Karya Hulu dan Hak Cipta Game Asli

Proyek ini adalah proyek **terjemahan tidak resmi** untuk mod yang terkait dengan *Project Zomboid*:

* Hak cipta game asli dan setiap mod adalah milik penulis/penerbitnya masing-masing;
* Proyek ini hanya melibatkan pembuatan dan pengorganisasian terjemahan teks, penyesuaian gaya, dan beberapa sumber daya pendukung;
* Kontributor, saat mengirimkan konten, harus memastikan:

  * Tidak langsung menyalin teks terjemahan atau sumber daya artistik pihak ketiga yang tidak sah;
  * Menghormati hak penulis asli dan penulis mod, dan tidak melakukan pendistribusian ulang yang melanggar.

---

## Komunikasi & Kolaborasi

Jika Anda memiliki:

* Pertanyaan tentang ketentuan lisensi;
* Keraguan tentang apakah konten tertentu dapat dikontribusikan;
* Keinginan untuk melisensikan karya Anda dengan cara khusus (misalnya, hanya penggunaan non-komersial tanpa adaptasi yang diizinkan);

Jangan ragu untuk menghubungi pengelola proyek melalui:

* Mengirim Issue untuk diskusi;
* Metode kontak publik lainnya dari pengelola.

Kami akan melakukan yang terbaik untuk menemukan solusi yang menyeimbangkan perkembangan proyek yang sehat dengan menghormati hak dan kepentingan semua pihak.

---

## Dukungan Finansial

Selama operasi proyek, karena penambahan mod baru dan pembaruan teks mod yang ada, API LLM perlu dipanggil secara terus-menerus untuk terjemahan. Untuk membatasi perilaku LLM, selain teks mod dasar, diperlukan sejumlah besar konten prompt (termasuk prompt dasar, aturan terjemahan, tabel istilah, batasan input/output, hasil pencarian semantik, dll.), yang mengonsumsi token jauh lebih banyak daripada teks asli. Oleh karena itu, proyek memerlukan dukungan finansial.

Jika Anda ingin memberikan dukungan finansial, silakan hubungi pengelola proyek. Terima kasih banyak!

---

Sekali lagi terima kasih atas kesediaan Anda untuk berkontribusi pada proyek ini!
Setiap kontribusi yang Anda berikan bermanfaat bagi lebih banyak pemain!
