# Project Babel — PZ 模組 LLM 自動翻譯

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>其它語言</summary>[العربية](README_ar.md) | [català](README_ca.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **注意：** 目前尚未支援此翻譯。權威內容以[簡體中文版](../../README.md)為準。

---

*本翻譯專案由 [Project Babel](https://github.com/PZProjectBabel/project_babel) 工具集驅動與維護。*

---

## 目錄

- [專案支援的目標翻譯語言](#專案支援的目標翻譯語言)
- [如何安裝和使用](#如何安裝和使用)
- [翻譯進度](#翻譯進度)
- [如何貢獻](#如何貢獻)
- [工具與目錄結構 (面向開發者)](#工具與目錄結構-(面向開發者))
- [版權與授權](#版權與授權)
- [致謝](#致謝)
- [第三方程式](#第三方程式)

---

## 專案支援的目標翻譯語言

| 語言 | 本地名 | 國際代碼 | 遊戲內代碼 | 是否支援 | 備註 |
|------|------|------|------|------|------|
| 阿拉伯語 | العربية | `ar` | `AR` | ❌ | Token額度不足 |
| 加泰隆尼亞語 | català | `ca` | `CA` | ❌ | Token額度不足 |
| 繁體中文 | 繁體中文 | `zh-hant` | `CH` | ❌ | Token額度不足 |
| 簡體中文 | 简体中文 | `zh-hans` | `CN` | ✅ | |
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

## 如何安裝和使用

這是為想要在遊戲中直接使用本漢化包的玩家準備的指南。

1. 前往 Steam 工作坊頁面：[[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. 點擊「訂閱」按鈕。
3. 啟動遊戲，在遊戲主選單的「模組」管理中啟用本翻譯模組。
4. 後啟用的模組的翻譯文本優先覆蓋先啟用的模組，因此本翻譯模組需在功能模組之後啟用。
5. 享受遊戲！

---

## 翻譯進度

[➡️ 翻譯進度](../progress/progress_zh-hant.md)

---

## 如何貢獻

歡迎貢獻！翻譯修正、新功能、提示模板或參考翻譯。

呼叫 LLM API 進行翻譯需要支付詞元費用，為了專案能夠長期穩定運作，希望您能慷慨相助！

詳情請閱讀 [貢獻指南](../contributing/contributing_zh-hant.md)

---

## 工具與目錄結構 (面向開發者)

本節內容面向希望了解專案自動化原理的開發者。

### 專案目錄

| 目錄 | 說明 |
|------|------|
| `src/` | .NET 10 翻譯流水線原始碼，含 15 個模組 |
| `config/` | 流水線設定檔 (LLM、Steam、RAG 參數等) |
| `data/` | 執行階段資料：模組中繼資料、embedding、翻譯快取 |
| `translation_ref/` | 參考翻譯資料，為 LLM 提供翻譯參考 |
| `base_game_keys/` | 遊戲本體翻譯鍵，用於去重 |
| `final_outputs/` | 最終輸出的 PZ 模組格式翻譯包 |
| `docs/` | 專案文件：進度報告、貢獻指南、流水線說明 |
| `temp/` | 流水線暫存檔 |
| `src/prompt_templates/` | LLM 提示詞範本 |

### 流水線模組（按執行順序）

| 步驟 | 模組 | 功能 |
|------|------|------|
| 1 | `ConfigReader` | 載入設定/金鑰/語言列表 |
| 2 | `RepoDataLoader` | 載入參考翻譯與翻譯快取 |
| 3 | `ModIdCollector` | 收集 Workshop 模組 ID |
| 4 | `ModInfoFetcher` | 取得 Steam 中繼資料 |
| 5 | `ModDownloader` | 透過 steamcmd 下載模組 |
| 6 | `ContentExtractor` | 解析模組翻譯檔 → `TranslationEntry` |
| 7 | `ContentChecker` | 內容安全審查 |
| 8 | `EmbeddingFetcher` | 計算文字 embedding 向量 |
| 9 | `TranslationBatcher` | 建立翻譯批次 |
| 10 | `RagContextRetriever` | 檢索 RAG 上下文 |
| 11 | `LLMTranslator` | 呼叫 LLM 執行翻譯 |
| 12 | `ResultWriter` | 寫入 data/ 與 translation_ref/ |
| 13 | `FinalOutputWriter` | 生成最終 PZ 模組格式輸出 |
| 14 | `ProgressReporter` | 生成進度報告 |

### 技術棧

- **語言**: C# (.NET 10)
- **目標平台**: GitHub Actions Linux x64 runner
- **測試**: xUnit (Windows x64)
- **LLM**: DeepSeek API (可設定)
- **Embedding**: 文字向量化用於 RAG 相似檢索
- **內容審查**: LLM 驅動的多級安全審核

詳細技術文件：[TranslationEntry 流水線](../pipeline/translation_entry_pipeline_zh-hant.md)

---

## 版權與授權

© 2025 Project Babel 及各作者。保留所有權利。

### 文字與圖片等內容

採用 **CC BY-NC-SA 4.0** 協議授權。

- **署名**：註明基於『Project Babel』修改，附帶倉庫與工坊連結
- **非商業**：禁止商業用途
- **相同方式共享**：修改後須以相同協議發布

### 程式碼

`src/` 下程式碼採用 **GPL-3.0** 授權。

---

## 致謝

| 參考模組 | 作者 | 頁面 |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**衷心感謝以上作者！**

---

## 第三方程式

本專案使用了第三方程式、函式庫，版權歸對應開發者所有。
