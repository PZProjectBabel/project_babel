Project Babel — 『Project Zomboid』Mod LLM自動翻訳プロジェクト

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*本翻訳プロジェクトは [Project Babel](https://github.com/PZProjectBabel/project_babel) ツールセットによって駆動・保守されています。*

---

## 目次

- [プロジェクトがサポートする対象翻訳言語](#プロジェクトがサポートする対象翻訳言語)
- [インストールと使用方法](#インストールと使用方法)
- [翻訳進捗](#翻訳進捗)
- [貢献方法](#貢献方法)
- [ツールとディレクトリ構成（開発者向け）](#ツールとディレクトリ構成開発者向け)
  - [プロジェクトディレクトリ](#プロジェクトディレクトリ)
  - [パイプラインモジュール（実行順）](#パイプラインモジュール実行順)
  - [独立モジュール](#独立モジュール)
  - [技術スタック](#技術スタック)
- [著作権とライセンス](#著作権とライセンス)
  - [1. テキストと画像などのコンテンツ](#1-テキストと画像などのコンテンツ)
  - [2. プログラム、スクリプト、その他の開発コンテンツ](#2-プログラムスクリプトその他の開発コンテンツ)
- [謝辞](#謝辞)
- [サードパーティプログラム](#サードパーティプログラム)

---

## プロジェクトがサポートする対象翻訳言語

| 言語 | 現地名 | 国際コード | ゲーム内コード | 対応状況 | 備考 |
|------|------|------|------|------|------|
| アラビア語 | العربية | `ar` | `AR` | ❌ | トークン枠不足 |
| カタルーニャ語 | català | `ca` | `CA` | ❌ | トークン枠不足 |
| 繁体中国語 | 繁體中文 | `zh-hant` | `CH` | ❌ | トークン枠不足 |
| 簡体中国語 | 简体中文 | `zh-hans` | `CN` | ✅ | |
| チェコ語 | čeština | `cs` | `CS` | ❌ | トークン枠不足 |
| デンマーク語 | dansk | `da` | `DA` | ❌ | トークン枠不足 |
| ドイツ語 | Deutsch | `de` | `DE` | ✅ | |
| 英語 | English | `en` | `EN` | ✅ | |
| スペイン語 | español | `es` | `ES` | ❌ | トークン枠不足 |
| フィンランド語 | suomi | `fi` | `FI` | ❌ | トークン枠不足 |
| フランス語 | français | `fr` | `FR` | ✅ | |
| ハンガリー語 | magyar | `hu` | `HU` | ❌ | トークン枠不足 |
| インドネシア語 | Bahasa Indonesia | `id` | `ID` | ❌ | トークン枠不足 |
| イタリア語 | italiano | `it` | `IT` | ❌ | トークン枠不足 |
| 日本語 | 日本語 | `ja` | `JP` | ✅ | |
| 韓国語 | 한국어 | `ko` | `KO` | ❌ | トークン枠不足 |
| オランダ語 | Nederlands | `nl` | `NL` | ❌ | トークン枠不足 |
| ノルウェー語 | norsk | `no` | `NO` | ❌ | トークン枠不足 |
| タガログ語 | Tagalog | `tl` | `PH` | ❌ | トークン枠不足 |
| ポーランド語 | polski | `pl` | `PL` | ❌ | トークン枠不足 |
| ポルトガル語（ポルトガル） | português | `pt` | `PT` | ❌ | トークン枠不足 |
| ポルトガル語（ブラジル） | português do Brasil | `pt-br` | `PTBR` | ❌ | トークン枠不足 |
| ルーマニア語 | română | `ro` | `RO` | ❌ | トークン枠不足 |
| ロシア語 | русский | `ru` | `RU` | ❌ | トークン枠不足 |
| タイ語 | ภาษาไทย | `th` | `TH` | ❌ | トークン枠不足 |
| トルコ語 | Türkçe | `tr` | `TR` | ❌ | Token不足 |
| ウクライナ語 | українська | `uk` | `UA` | ❌ | Token不足 |

**合計**：27 の計画言語 | **対応済み**：5 種 | **未対応**：22 種

---

## インストールと使用方法

これは、ゲーム内で本翻訳プロジェクトを直接使用したいプレイヤーのためのガイドです。

1.  Steam コミュニティワークショップページにアクセス：[[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  「購読」ボタンをクリック。
3.  ゲームを起動し、メインメニューの「MOD」管理で本翻訳MODを有効にします。
4.  後から有効にしたMODの翻訳テキストが先に有効にしたMODを上書きするため、本翻訳MODは機能MODの後に有効にしてください（できるだけ下に配置）。
5.  ゲームをお楽しみください！

---

## 翻訳進捗

**[➡️ 翻訳進捗はこちらをクリック](./docs/progress/progress_ja.md)**

---

## 貢献方法

バグ修正、機能追加、プロンプトテンプレートの作成、参考翻訳の提供など、あらゆる形での貢献を歓迎します！

LLM APIを利用した翻訳にはトークン料金がかかるため、プロジェクトを長期的に安定して運営するために、ご支援をよろしくお願いいたします！

詳細は[貢献ガイド](./docs/contributing/contributing_ja.md)をお読みください。

---

## ツールとディレクトリ構成（開発者向け）

このセクションは、プロジェクトの自動化の仕組みを理解したい開発者向けです。

### プロジェクトディレクトリ

| ディレクトリ | 説明 |
|------|------|
| `src/` | .NET 10 翻訳パイプラインソースコード、15モジュール + 2独立モジュールを含む |
| `config/` | パイプライン設定ファイル（LLM、Steam、RAGパラメータなど） |
| `data/` | 実行時データ：MODメタデータ、埋め込み、翻訳キャッシュ |
| `translation_ref/` | 参考翻訳データ（如一漢化組の許可を受けたMOD）、LLMに翻訳参考を提供 |
| `base_game_keys/` | ゲーム本体の翻訳キー、重複を防ぎネイティブテキストの上書きを回避 |
| `final_outputs/` | 最終出力：`project_babel/` MODパック、`icons/` アイコン、`workshop_descriptions/` ワークショップ説明 |
| `docs/` | プロジェクトドキュメント：進捗レポート、貢献ガイド、パイプライン説明 |
| `temp/` | パイプライン一時ファイル（実行ごとに独立したディレクトリ） |
| `src/prompt_templates/` | LLMプロンプトテンプレート（翻訳/コンテンツ審査） |

### パイプラインモジュール（実行順）

| ステップ | モジュール | 機能 |
|------|------|------|
| 1 | `ConfigReader` | 設定/鍵/言語リストを読み込む |
| 2 | `RepoDataLoader` | 参照翻訳と翻訳キャッシュを読み込む |
| 3 | `ModIdCollector` | Workshop モッドIDを収集 |
| 4 | `ModInfoFetcher` | Steamメタデータを取得 |
| 5 | `SteamCmdBootstrapper` | 現在のプラットフォームのsteamcmdランタイムを準備 |
| 6 | `ModDownloader` | steamcmd経由でモッドをダウンロード |
| 7 | `ContentExtractor` | モッド翻訳ファイルを解析 → `TranslationEntry` |
| 8 | `ContentChecker` | コンテンツ安全審査 (麻薬/ポルノ/暴力) |
| 9 | `EmbeddingFetcher` | テキスト埋め込みベクトルを計算 |
| 10 | `TranslationBatcher` | 対象言語に依存しない翻訳バッチを作成 |
| 11 | `RagContextRetriever` | RAGコンテキストを検索 (正確なキー + 埋め込み類似度) |
| 12 | `LLMTranslator` | LLMを呼び出して翻訳を実行 |
| 13 | `ResultWriter` | data/ と translation_ref/ に書き込み |
| 14 | `FinalOutputWriter` | 最終PZモッド形式の出力を生成 |
| 15 | `ProgressReporter` | 進捗レポートを生成 |

### 独立モジュール

| モジュール | 機能 |
|------|------|
| `WorkshopMonitor` | 定期的にSteam Workshopの新モッドを取得し、購読数でフィルタリングして `request_for_translation.txt` に追加 |
| `DocGenerator` | LLM駆動の多言語ドキュメント生成器 |

### 技術スタック

- **言語**: C# (.NET 10)
- **対象プラットフォーム**: GitHub Actions Linux x64 runner
- **テスト**: xUnit (Windows x64)
- **LLM**: DeepSeek API (設定可能)
- **埋め込み**: テキストベクトル化によりRAG類似検索に使用
- **コンテンツ審査**: LLM駆動の多段階セキュリティ審査

詳細は [技術参考](./docs/technical_reference/technical_reference_ja.md)。

---

## 著作権とライセンス

本翻訳プロジェクトの翻訳テキストおよび関連画像は、**Project Babel** と各参加者が元のゲームModを基に創作または二次創作したものです。

© 2025 Project Babel および各作者は権利を留保します。

### 1. テキストと画像などのコンテンツ

特に別段の記載がない限り、このリポジトリ内の：

- ゲーム内テキストの翻訳、ブラッシュアップ、校正内容；
プロジェクト説明文書、Mod内テキスト翻訳；
本プロジェクトが特別に制作した画像、美術リソース

すべて **表示-非営利-継承 4.0 国際**（Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International、略称 **CC BY-NC-SA 4.0**）ライセンスの下で提供されます。

これは、以下の条件を遵守することを前提に、これらのコンテンツを自由に共有・改変できることを意味します。

- **表示（BY）**：目立つ位置に「本翻訳プロジェクトは『Project Babel』の作業成果に基づいて修正されています」と明記し、このリポジトリとSteam Workshopリンク `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080` を添付すること。
- **非営利使用（NC）**：本プロジェクトのコンテンツまたはその改変作品を、直接的または間接的な商業目的（有料統合パック、有料ダウンロード、広告収入分配など）に使用してはなりません。
- **継承（SA）**：本プロジェクトのコンテンツに基づいて修正または再創作を行う場合、**同じ CC BY-NC-SA 4.0 ライセンス** で変更バージョンを公開する必要があります。

本ライセンスの詳細については、以下を参照してください。
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.ja>

*特別な注意：*
- *base_game_keysフォルダの内容はゲーム本体からのものであり、著作権はゲーム開発者に帰属します！内容は翻訳キーがゲームキーを上書きするのを防ぐために使用されます（重複排除）*
- *translation_refフォルダの内容はLLMに翻訳参考資料を提供するためのものであり、著作権は各Mod開発者に帰属します！*

### 2. プログラム、スクリプト、その他の開発コンテンツ

ソースファイルまたはディレクトリに別途明記がない限り、このリポジトリ内のローカライズコンテンツの作成/パッケージ化/処理に使用されるプログラムコード（例：`src/` ディレクトリのプログラムコード）は、**GNU General Public License v3.0（GPL-3.0）** の下で提供されます。

完全な条件は、このリポジトリのルートディレクトリにある `LICENSE` ファイル（GPL-3.0）を参照するか、GNU公式サイト <https://www.gnu.org/licenses/gpl-3.0.html> をご覧ください。

---

## 謝辞

本プロジェクトは、対象言語翻訳の参考テキストとしてサードパーティのModを使用しており、参考テキストはLLMに送信されて翻訳の参考にされます。

| 参考Mod名 | 作成者 | Modページ |
|------|------|------|
| [B42]統一・中国語漢化 | 如一漢化組 (As1) | [Workshopページ](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]統一・Mod漢化 | 如一漢化組 (As1) | [Workshopページ](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]統一・方舟漢化 | 如一漢化組 (As1) | [Workshopページ](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**上記の作成者に心より感謝いたします！**

---

## サードパーティプログラム

本プロジェクトは、サードパーティのプログラムやライブラリを使用しています。これらのサードパーティプログラムの著作権は、それぞれの開発者に帰属します。

