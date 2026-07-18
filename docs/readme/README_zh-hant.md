# Project Babel — 《殭屍毀滅工程》模組LLM自動翻譯項目

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*本翻譯項目由 [Project Babel](https://github.com/PZProjectBabel/project_babel) 工具集驅動與維護。*

---

## 目錄

- [項目支援的目標翻譯語言](#項目支援的目標翻譯語言)
- [如何安裝與使用](#如何安裝與使用)
- [翻譯進度](#翻譯進度)
- [如何貢獻](#如何貢獻)
- [工具與目錄結構 (面向開發者)](#工具與目錄結構-面向開發者)
  - [專案目錄](#專案目錄)
  - [流水線模組 (按執行順序)](#流水線模組-按執行順序)
  - [獨立模組](#獨立模組)
  - [技術棧](#技術棧)
- [版權與授權](#版權與授權)
  - [1. 文字與圖片等內容](#1-文字與圖片等內容)
  - [2. 程式、腳本與其他開發內容](#2-程式腳本與其他開發內容)
- [致謝](#致謝)
- [第三方程式](#第三方程式)

---

## 項目支援的目標翻譯語言

| 語言 | 本地名稱 | 國際代碼 | 遊戲內代碼 | 是否支援 | 備註 |
|------|------|------|------|------|------|
| 阿拉伯語 | العربية | `ar` | `AR` | ❌ | Token額度不足 |
| 加泰隆尼亞語 | català | `ca` | `CA` | ❌ | Token額度不足 |
| 繁體中文 | 繁體中文 | `zh-hant` | `CH` | ❌ | Token額度不足 |
| 簡體中文 | 簡體中文 | `zh-hans` | `CN` | ✅ | |
| 捷克語 | čeština | `cs` | `CS` | ❌ | Token額度不足 |
| 丹麥語 | dansk | `da` | `DA` | ❌ | Token額度不足 |
| 德語 | Deutsch | `de` | `DE` | ✅ | |
| 英語 | English | `en` | `EN` | ✅ | |
| 西班牙語 | español | `es` | `ES` | ❌ | Token額度不足 |
| 芬蘭語 | suomi | `fi` | `FI` | ❌ | Token額度不足 |
| 法語 | français | `fr` | `FR` | ✅ | |
| 匈牙利語 | magyar | `hu` | `HU` | ❌ | Token額度不足 |
| 印尼語 | Bahasa Indonesia | `id` | `ID` | ❌ | Token額度不足 |
| 義大利語 | italiano | `it` | `IT` | ❌ | Token額度不足 |
| 日語 | 日本語 | `ja` | `JP` | ✅ | |
| 韓語 | 한국어 | `ko` | `KO` | ❌ | Token額度不足 |
| 荷蘭語 | Nederlands | `nl` | `NL` | ❌ | Token額度不足 |
| 挪威語 | norsk | `no` | `NO` | ❌ | Token額度不足 |
| 他加祿語 | Tagalog | `tl` | `PH` | ❌ | Token額度不足 |
| 波蘭語 | polski | `pl` | `PL` | ❌ | Token額度不足 |
| 葡萄牙語（葡萄牙） | português | `pt` | `PT` | ❌ | Token額度不足 |
| 葡萄牙語（巴西） | português do Brasil | `pt-br` | `PTBR` | ❌ | Token額度不足 |
| 羅馬尼亞語 | română | `ro` | `RO` | ❌ | Token額度不足 |
| 俄語 | русский | `ru` | `RU` | ❌ | Token額度不足 |
| 泰語 | ภาษาไทย | `th` | `TH` | ❌ | Token額度不足 |
| 土耳其語 | Türkçe | `tr` | `TR` | ❌ | Token額度不足 |
| 烏克蘭語 | українська | `uk` | `UA` | ❌ | Token額度不足 |

**總計**：27 種計劃語言 | **已支援**：5 種 | **待支援**：22 種

---

## 如何安裝與使用

這是為想要在遊戲中直接使用本翻譯專案的玩家準備的指南。

1.  前往我們的 Steam 創意工坊頁面：[[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  點擊「訂閱」按鈕。
3.  啟動遊戲，在遊戲主選單的「模組」管理中啟用本翻譯模組。
4.  後啟用的模組的翻譯文字優先覆蓋先啟用的模組，因此本翻譯模組需在功能模組之後啟用（盡量置底）。
5.  享受遊戲！

---

## 翻譯進度

[➡️ 點擊此處查看翻譯進度](./docs/progress/progress_zh-hant.md)

---

## 如何貢獻

我們歡迎任何人參與貢獻，無論是修正一個錯誤、新增一個功能、撰寫提示詞模板、抑或是提供參考翻譯！

調用LLM API進行翻譯是需要為詞元付費的，為了專案能夠長期穩定運行，希望您能慷慨相助！

詳情請閱讀 [貢獻指南](./docs/contributing/contributing_zh-hant.md)

---

## 工具與目錄結構 (面向開發者)

本節內容面向希望了解專案自動化原理的開發者。

### 專案目錄

| 目錄 | 說明 |
|------|------|
| `src/` | .NET 10 翻譯流水線原始碼，含 15 個模組 + 2 個獨立模組 |
| `config/` | 流水線設定檔 (LLM、Steam、RAG 參數等) |
| `data/` | 執行時期資料：模組元資料、embedding、翻譯快取 |
| `translation_ref/` | 參考翻譯資料 (如一漢化組授權模組)，為 LLM 提供翻譯參考 |
| `base_game_keys/` | 遊戲本體翻譯鍵，用於去重防止覆蓋原生文字 |
| `final_outputs/` | 最終輸出：`project_babel/` 模組包、`icons/` 圖示與 `workshop_descriptions/` 創意工坊描述 |
| `docs/` | 專案文件：進度報告、貢獻指南、流水線說明 |
| `temp/` | 流水線暫存檔案 (每次執行獨立目錄) |
| `src/prompt_templates/` | LLM 提示詞模板 (翻譯/內容審查) |

### 流水線模組 (按執行順序)

| 步驟 | 模組 | 功能 |
|------|------|------|
| 1 | `ConfigReader` | 載入配置/密鑰/語言列表 |
| 2 | `RepoDataLoader` | 載入參考翻譯與翻譯快取 |
| 3 | `ModIdCollector` | 收集 Workshop 模組 ID |
| 4 | `ModInfoFetcher` | 取得 Steam 元資料 |
| 5 | `SteamCmdBootstrapper` | 準備當前平台的 steamcmd 執行環境 |
| 6 | `ModDownloader` | 透過 steamcmd 下載模組 |
| 7 | `ContentExtractor` | 解析模組翻譯檔案 → `TranslationEntry` |
| 8 | `ContentChecker` | 內容安全審查 (毒品/色情/暴力) |
| 9 | `EmbeddingFetcher` | 計算文字 embedding 向量 |
| 10 | `TranslationBatcher` | 建立目標語言無關的翻譯批次 |
| 11 | `RagContextRetriever` | 檢索 RAG 上下文 (精確鍵 + embedding 相似度) |
| 12 | `LLMTranslator` | 呼叫 LLM 執行翻譯 |
| 13 | `ResultWriter` | 寫入 data/ 與 translation_ref/ |
| 14 | `FinalOutputWriter` | 生成最終 PZ 模組格式輸出 |
| 15 | `ProgressReporter` | 生成進度報告 |

### 獨立模組

| 模組 | 功能 |
|------|------|
| `WorkshopMonitor` | 定時抓取 Steam Workshop 新模組，按訂閱數篩選併入 `request_for_translation.txt` |
| `DocGenerator` | LLM 驅動的多語言文件生成器 |

### 技術棧

- **語言**: C# (.NET 10)
- **目標平台**: GitHub Actions Linux x64 runner
- **測試**: xUnit (Windows x64)
- **LLM**: DeepSeek API (可配置)
- **Embedding**: 文字向量化用於 RAG 相似檢索
- **內容審查**: LLM 驅動的多級安全審核

詳細的 [技術參考](./docs/technical_reference/technical_reference_zh-hant.md)。

---

## 版權與授權

本翻譯專案的翻譯文字內容與相關圖片，由 **Project Babel** 與各參與者基於原遊戲模組創作或二次創作完成。

© 2025 Project Babel 及各作者保留權利。

### 1. 文字與圖片等內容

除非另有特別說明，本倉庫中的：

- 遊戲內文字翻譯、潤色與校對內容；
項目說明文件、模組內文本翻譯；
本項目專門製作的圖片、美術資源

均採用 **署名-非商業性使用-相同方式共享 4.0 國際**（Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International，簡稱 **CC BY-NC-SA 4.0**）協議授權。

這意味著，在遵守以下條件的前提下，您可以自由分享與改編這些內容：

- **署名（BY）**：在明顯位置註明「本翻譯項目基於『Project Babel』的工作成果進行修改」，並附上本倉庫和 Steam 創意工坊連結   `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **非商業性使用（NC）**：不得將本項目內容或其改編作品用於任何直接或間接的商業用途（包括但不限於付費整合包、付費下載、廣告分紅等）；
- **相同方式共享（SA）**：若您基於本項目內容進行修改或再創作，必須以 **同樣的 CC BY-NC-SA 4.0 協議** 公開發布您的改動版本。

有關本協議的更多資訊，請參見：
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.zh-Hant>

*特殊說明：*
- *base_game_keys 資料夾內容來自遊戲本體，版權歸遊戲開發商所有！內容用於防止翻譯鍵覆蓋遊戲鍵（去重）*
- *translation_ref 資料夾內容用於給 LLM 提供翻譯參考，版權歸各自模組開發者所有！*

### 2. 程式、腳本與其他開發內容

除非原始碼檔案或目錄中另有特別聲明，本倉庫中用於製作/打包/處理中文化內容的程式碼（例如 `src/` 目錄下的程式碼），採用 **GNU 通用公共許可證第 3 版（GPL-3.0）** 進行授權。

完整條款請參見本倉庫根目錄下的 `LICENSE` 檔案（GPL-3.0），或造訪 GNU 官網：<https://www.gnu.org/licenses/gpl-3.0.html>。

---

## 致謝

本項目使用了第三方的模組作為目標語言翻譯的參考文本，參考文本被發送給 LLM 進行翻譯參考。

| 參考模組名稱 | 作者 | 模組頁面 |
|------|------|------|
| [B42]統一·中文漢化 | 如一漢化組 (As1) | [創意工坊頁面](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]統一·模組漢化 | 如一漢化組 (As1) | [創意工坊頁面](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]統一·方舟漢化 | 如一漢化組 (As1) | [創意工坊頁面](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**對以上作者致以衷心感謝！**

---

## 第三方程式

本項目使用了第三方程式、庫，這些第三方程式的版權歸對應開發者所有。

