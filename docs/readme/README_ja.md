# Project Babel — PZ Mod LLM 自動翻訳

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>他の言語</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*本翻訳プロジェクトは [Project Babel](https://github.com/PZProjectBabel/project_babel) ツールセットによって運営・保守されています。*

---

## 目次

- [対応対象言語](#対応対象言語)
- [インストールと使用方法](#インストールと使用方法)
- [翻訳の進捗](#翻訳の進捗)
- [貢献方法](#貢献方法)
- [ツールとディレクトリ構造 (開発者向け)](#ツールとディレクトリ構造-開発者向け)
- [著作権とライセンス](#著作権とライセンス)
- [謝辞](#謝辞)
- [サードパーティソフトウェア](#サードパーティソフトウェア)

---

## 対応対象言語

| 言語 | 現地名 | ISOコード | ゲーム内コード | 対応 | 備考 |
|------|------|------|------|------|------|
| アラビア語 | العربية | `ar` | `AR` | ❌ | トークン残高不足 |
| カタルーニャ語 | català | `ca` | `CA` | ❌ | トークン残高不足 |
| 繁体字中国語 | 繁體中文 | `zh-hant` | `CH` | ❌ | トークン残高不足 |
| 簡体字中国語 | 简体中文 | `zh-hans` | `CN` | ✅ | |
| チェコ語 | čeština | `cs` | `CS` | ❌ | トークン残高不足 |
| デンマーク語 | dansk | `da` | `DA` | ❌ | トークン残高不足 |
| ドイツ語 | Deutsch | `de` | `DE` | ✅ | |
| 英語 | English | `en` | `EN` | ✅ | |
| スペイン語 | español | `es` | `ES` | ❌ | トークン残高不足 |
| フィンランド語 | suomi | `fi` | `FI` | ❌ | トークン残高不足 |
| フランス語 | français | `fr` | `FR` | ✅ | |
| ハンガリー語 | magyar | `hu` | `HU` | ❌ | トークン残高不足 |
| インドネシア語 | Bahasa Indonesia | `id` | `ID` | ❌ | トークン残高不足 |
| イタリア語 | italiano | `it` | `IT` | ❌ | トークン残高不足 |
| 日本語 | 日本語 | `ja` | `JP` | ✅ | |
| 韓国語 | 한국어 | `ko` | `KO` | ❌ | トークン残高不足 |
| オランダ語 | Nederlands | `nl` | `NL` | ❌ | トークン残高不足 |
| ノルウェー語 | norsk | `no` | `NO` | ❌ | トークン残高不足 |
| タガログ語 | Tagalog | `tl` | `PH` | ❌ | トークン残高不足 |
| ポーランド語 | polski | `pl` | `PL` | ❌ | トークン残高不足 |
| ポルトガル語（ポルトガル） | português | `pt` | `PT` | ❌ | トークン残高不足 |
| ポルトガル語（ブラジル） | português do Brasil | `pt-br` | `PTBR` | ❌ | トークン残高不足 |
| ルーマニア語 | română | `ro` | `RO` | ❌ | トークン残高不足 |
| ロシア語 | русский | `ru` | `RU` | ❌ | トークン残高不足 |
| タイ語 | ภาษาไทย | `th` | `TH` | ❌ | トークン残高不足 |
| トルコ語 | Türkçe | `tr` | `TR` | ❌ | トークン残高不足 |
| ウクライナ語 | українська | `uk` | `UA` | ❌ | トークン残高不足 |

**合計**: 27 言語計画 | **対応済み**: 5 | **未対応**: 22

---

## インストールと使用方法

ゲーム内で翻訳パックを使用したいプレイヤー向けのガイドです。

1. Steam Workshopページにアクセス: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. 「サブスクライブ」をクリック。
3. ゲームを起動し、Modsメニューでこの翻訳MODを有効にします。
4. 後から読み込まれたMODの翻訳テキストが優先されるため、この翻訳MODはゲームプレイMODより後に読み込む必要があります。
5. お楽しみください！

---

## 翻訳の進捗

[➡️ 翻訳の進捗](../progress/progress_ja.md)

---

## 貢献方法

翻訳の修正、新機能、プロンプトテンプレート、参考翻訳など、あらゆる貢献を歓迎します！

LLM APIを使用した翻訳にはトークン料金が発生します。プロジェクトの長期安定的な運営のために、ご支援をお願いいたします！

詳しくは [貢献ガイド](../contributing/contributing_ja.md) をお読みください。

---

## ツールとディレクトリ構造 (開発者向け)

このセクションは、プロジェクトの自動化の仕組みを理解したい開発者向けです。

### プロジェクトディレクトリ

| ディレクトリ | 説明 |
|------|------|
| `src/` | .NET 10 翻訳パイプラインのソースコード、15モジュール |
| `config/` | パイプライン設定 (LLM、Steam、RAGパラメータ等) |
| `data/` | ランタイムデータ: MODメタデータ、埋め込み、翻訳キャッシュ |
| `translation_ref/` | LLMコンテキスト用の参考翻訳データ |
| `base_game_keys/` | 重複排除用のベースゲーム翻訳キー |
| `final_outputs/` | 最終的なPZ MOD形式の翻訳出力 |
| `docs/` | プロジェクト文書: 進捗、貢献、パイプライン仕様 |
| `temp/` | パイプライン一時ファイル |
| `src/prompt_templates/` | LLMプロンプトテンプレート |

### パイプラインモジュール（実行順）

| ステップ | モジュール | 機能 |
|------|------|------|
| 1 | `ConfigReader` | 設定/シークレット/言語の読み込み |
| 2 | `RepoDataLoader` | 参考翻訳と翻訳キャッシュの読み込み |
| 3 | `ModIdCollector` | Workshop MOD IDの収集 |
| 4 | `ModInfoFetcher` | Steamメタデータの取得 |
| 5 | `ModDownloader` | steamcmd経由でMODをダウンロード |
| 6 | `ContentExtractor` | MOD翻訳ファイルの解析 → `TranslationEntry` |
| 7 | `ContentChecker` | コンテンツ安全性審査 |
| 8 | `EmbeddingFetcher` | テキスト埋め込みベクトルの計算 |
| 9 | `TranslationBatcher` | 翻訳バッチの作成 |
| 10 | `RagContextRetriever` | RAGコンテキストの取得 |
| 11 | `LLMTranslator` | LLM翻訳の実行 |
| 12 | `ResultWriter` | data/ と translation_ref/ への書き込み |
| 13 | `FinalOutputWriter` | 最終PZ MOD出力の生成 |
| 14 | `ProgressReporter` | 進捗レポートの生成 |

### 技術スタック

- **言語**: C# (.NET 10)
- **ターゲットプラットフォーム**: GitHub Actions Linux x64 ランナー
- **テスト**: xUnit (Windows x64)
- **LLM**: DeepSeek API (設定可能)
- **埋め込み**: RAG類似検索のためのテキストベクトル化
- **コンテンツ審査**: LLM駆動の多層安全性審査

詳細な技術文書: [TranslationEntry パイプライン](../pipeline/translation_entry_pipeline_ja.md)

---

## 著作権とライセンス

© 2025 Project Babel および各作者。無断複写・転載を禁じます。

### コンテンツ（テキスト、画像）

**CC BY-NC-SA 4.0** の下でライセンスされます。

- **表示**: 「Project Babel」に基づく改変であることを明記し、リポジトリとWorkshopのリンクを添付
- **非営利**: 商業利用の禁止
- **継承**: 改変物は同一ライセンスで公開すること

### コード

`src/` 以下のコードは **GPL-3.0** の下でライセンスされます。

---

## 謝辞

| 参考MOD | 作者 | ページ |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**上記の作者の皆様に心より感謝いたします！**

---

## サードパーティソフトウェア

本プロジェクトはサードパーティのプログラムやライブラリを使用しており、著作権は各開発者に帰属します。

