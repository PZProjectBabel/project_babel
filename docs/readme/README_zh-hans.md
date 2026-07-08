# Project Babel - 《僵尸毁灭工程》模组LLM自动翻译项目

> [English](README_en.md) <details><summary>其它语言</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*本翻译项目由 [Project Babel](https://github.com/PZProjectBabel/project_babel) 工具集驱动与维护。*

---

## 目录

- [项目支持的目标翻译语言](#项目支持的目标翻译语言)
- [如何安装和使用](#如何安装和使用)
- [翻译进度](#翻译进度)
- [如何贡献](#如何贡献)
- [工具与目录结构 (面向开发者)](#工具与目录结构-(面向开发者))
- [版权与授权](#版权与授权)
- [致谢](#致谢)
- [第三方程序](#第三方程序)

---

## 项目支持的目标翻译语言

| 语言 | 本地名 | 国际代码 | 游戏内代码 | 是否支持 | 备注 |
|------|------|------|------|------|------|
| 阿拉伯语 | العربية | `ar` | `AR` | ❌ | 经费不足 |
| 加泰罗尼亚语 | català | `ca` | `CA` | ❌ | 经费不足 |
| 繁体中文 | 繁體中文 | `zh-hant` | `CH` | ❌ | 经费不足 |
| 简体中文 | 简体中文 | `zh-hans` | `CN` | ✅ | |
| 捷克语 | čeština | `cs` | `CS` | ❌ | 经费不足 |
| 丹麦语 | dansk | `da` | `DA` | ❌ | 经费不足 |
| 德语 | Deutsch | `de` | `DE` | ✅ | |
| 英语 | English | `en` | `EN` | ✅ | |
| 西班牙语 | español | `es` | `ES` | ❌ | 经费不足 |
| 芬兰语 | suomi | `fi` | `FI` | ❌ | 经费不足 |
| 法语 | français | `fr` | `FR` | ✅ | |
| 匈牙利语 | magyar | `hu` | `HU` | ❌ | 经费不足 |
| 印尼语 | Bahasa Indonesia | `id` | `ID` | ❌ | 经费不足 |
| 意大利语 | italiano | `it` | `IT` | ❌ | 经费不足 |
| 日语 | 日本語 | `ja` | `JP` | ✅ | |
| 韩语 | 한국어 | `ko` | `KO` | ❌ | 经费不足 |
| 荷兰语 | Nederlands | `nl` | `NL` | ❌ | 经费不足 |
| 挪威语 | norsk | `no` | `NO` | ❌ | 经费不足 |
| 他加禄语 | Tagalog | `tl` | `PH` | ❌ | 经费不足 |
| 波兰语 | polski | `pl` | `PL` | ❌ | 经费不足 |
| 葡萄牙语（葡萄牙） | português | `pt` | `PT` | ❌ | 经费不足 |
| 葡萄牙语（巴西） | português do Brasil | `pt-br` | `PTBR` | ❌ | 经费不足 |
| 罗马尼亚语 | română | `ro` | `RO` | ❌ | 经费不足 |
| 俄语 | русский | `ru` | `RU` | ❌ | 经费不足 |
| 泰语 | ภาษาไทย | `th` | `TH` | ❌ | 经费不足 |
| 土耳其语 | Türkçe | `tr` | `TR` | ❌ | 经费不足 |
| 乌克兰语 | українська | `uk` | `UA` | ❌ | 经费不足 |

**总计**：27 种计划语言 | **已支持**：5 种 | **待支持**：22 种

---

## 如何安装和使用

这是为想要在游戏中直接使用本汉化包的玩家准备的指南。

1. 前往我们的 Steam 创意工坊页面：[[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. 点击「订阅」按钮。
3. 启动游戏，在游戏主菜单的「模组」管理中启用本翻译模组。
4. 后启用的模组的翻译文本优先覆盖先启用的模组，因此本翻译模组需在功能模组之后启用（尽量置底）。
5. 享受游戏！

---

## 翻译进度

[➡️ 翻译进度](../progress/progress_zh-hans.md)

---

## 如何贡献

我们欢迎任何人参与贡献，无论是修正一个错误、新增一个功能、撰写提示词模板、亦或是提供参考翻译！

调用LLM API进行翻译是需要为词元付费的，为了项目能够长期稳定运行，希望您能慷慨相助！

---

## 工具与目录结构 (面向开发者)

本节内容面向希望了解项目自动化原理的开发者。

### 项目目录

| 目录 | 说明 |
|------|------|
| `src/` | .NET 10 翻译流水线源码，含 15 个模块 |
| `config/` | 流水线配置文件 (LLM、Steam、RAG 参数等) |
| `data/` | 运行时数据：模组元数据、embedding、翻译缓存 |
| `translation_ref/` | 参考翻译数据，为 LLM 提供翻译参考 |
| `base_game_keys/` | 游戏本体翻译键，用于去重 |
| `final_outputs/` | 最终输出的 PZ 模组格式翻译包 |
| `docs/` | 项目文档：进度报告、贡献指南、流水线说明 |
| `temp/` | 流水线临时文件 |
| `src/prompt_templates/` | LLM 提示词模板 |

### 流水线模块（按执行顺序）

| 步骤 | 模块 | 功能 |
|------|------|------|
| 1 | `ConfigReader` | 加载配置/密钥/语言列表 |
| 2 | `RepoDataLoader` | 加载参考翻译与翻译缓存 |
| 3 | `ModIdCollector` | 收集 Workshop 模组 ID |
| 4 | `ModInfoFetcher` | 获取 Steam 元数据 |
| 5 | `ModDownloader` | 通过 steamcmd 下载模组 |
| 6 | `ContentExtractor` | 解析模组翻译文件 → `TranslationEntry` |
| 7 | `ContentChecker` | 内容安全审查 |
| 8 | `EmbeddingFetcher` | 计算文本 embedding 向量 |
| 9 | `TranslationBatcher` | 创建翻译批次 |
| 10 | `RagContextRetriever` | 检索 RAG 上下文 |
| 11 | `LLMTranslator` | 调用 LLM 执行翻译 |
| 12 | `ResultWriter` | 写入 data/ 与 translation_ref/ |
| 13 | `FinalOutputWriter` | 生成最终 PZ 模组格式输出 |
| 14 | `ProgressReporter` | 生成进度报告 |

### 技术栈

- **语言**: C# (.NET 10)
- **目标平台**: GitHub Actions Linux x64 runner
- **测试**: xUnit (Windows x64)
- **LLM**: DeepSeek API (可配置)
- **Embedding**: 文本向量化用于 RAG 相似检索
- **内容审查**: LLM 驱动的多级安全审核

详细技术文档：[TranslationEntry 流水线](../pipeline/translation_entry_pipeline_zh-hans.md)

---

## 版权与授权

© 2025 Project Babel 及各作者。保留所有权利。

### 文本与图片等内容

采用 **CC BY-NC-SA 4.0** 协议授权。

- **署名**：注明基于『Project Babel』修改，附带仓库与工坊链接
- **非商业**：禁止商业用途
- **相同方式共享**：修改后须以相同协议发布

### 程序代码

`src/` 下代码采用 **GPL-3.0** 授权。

---

## 致谢

| 参考模组 | 作者 | 页面 |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**衷心感谢以上作者！**

---

## 第三方程序

本项目使用了第三方程序、库，版权归对应开发者所有。
