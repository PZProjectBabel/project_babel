# Project Babel - 《僵尸毁灭工程》模组LLM自动翻译项目

> [English](./docs/readme/README_en.md)  <details><summary>其它语言</summary>[العربية](./docs/readme/README_ar.md) | [català](./docs/readme/README_ca.md) | [繁體中文](./docs/readme/README_zh-hant.md) | [čeština](./docs/readme/README_cs.md) | [dansk](./docs/readme/README_da.md) | [Deutsch](./docs/readme/README_de.md) | [español](./docs/readme/README_es.md) | [suomi](./docs/readme/README_fi.md) | [français](./docs/readme/README_fr.md) | [magyar](./docs/readme/README_hu.md) | [Bahasa Indonesia](./docs/readme/README_id.md) | [italiano](./docs/readme/README_it.md) | [日本語](./docs/readme/README_ja.md) | [한국어](./docs/readme/README_ko.md) | [Nederlands](./docs/readme/README_nl.md) | [norsk](./docs/readme/README_no.md) | [Tagalog](./docs/readme/README_tl.md) | [polski](./docs/readme/README_pl.md) | [português](./docs/readme/README_pt.md) | [português do Brasil](./docs/readme/README_pt-br.md) | [română](./docs/readme/README_ro.md) | [русский](./docs/readme/README_ru.md) | [ภาษาไทย](./docs/readme/README_th.md) | [Türkçe](./docs/readme/README_tr.md) | [українська](./docs/readme/README_uk.md)</details>

---

*本翻译项目由 [Project Babel](https://github.com/PZProjectBabel/project_babel) 工具集驱动与维护。*

---

## 目录

- [项目支持的目标翻译语言](#项目支持的目标翻译语言)
- [如何安装和使用](#如何安装和使用)
- [翻译进度](#翻译进度)
- [如何贡献](#如何贡献)
- [工具与目录结构 (面向开发者)](#工具与目录结构-面向开发者)
- [版权与授权](#版权与授权)
- [致谢](#致谢)
- [第三方程序](#第三方程序)

---

## 项目支持的目标翻译语言

| 语言 | 本地名 | 国际代码 | 游戏内代码 | 是否支持 | 备注 |
|------|------|------|------|------|------|
| 阿拉伯语 | العربية | `ar` | `AR` | ❌ | Token额度不足 |
| 加泰罗尼亚语 | català | `ca` | `CA` | ❌ | Token额度不足 |
| 繁体中文 | 繁體中文 | `zh-hant` | `CH` | ❌ | Token额度不足 |
| 简体中文 | 简体中文 | `zh-hans` | `CN` | ✅ | |
| 捷克语 | čeština | `cs` | `CS` | ❌ | Token额度不足 |
| 丹麦语 | dansk | `da` | `DA` | ❌ | Token额度不足 |
| 德语 | Deutsch | `de` | `DE` | ✅ | |
| 英语 | English | `en` | `EN` | ✅ | |
| 西班牙语 | español | `es` | `ES` | ❌ | Token额度不足 |
| 芬兰语 | suomi | `fi` | `FI` | ❌ | Token额度不足 |
| 法语 | français | `fr` | `FR` | ✅ | |
| 匈牙利语 | magyar | `hu` | `HU` | ❌ | Token额度不足 |
| 印尼语 | Bahasa Indonesia | `id` | `ID` | ❌ | Token额度不足 |
| 意大利语 | italiano | `it` | `IT` | ❌ | Token额度不足 |
| 日语 | 日本語 | `ja` | `JP` | ✅ | |
| 韩语 | 한국어 | `ko` | `KO` | ❌ | Token额度不足 |
| 荷兰语 | Nederlands | `nl` | `NL` | ❌ | Token额度不足 |
| 挪威语 | norsk | `no` | `NO` | ❌ | Token额度不足 |
| 他加禄语 | Tagalog | `tl` | `PH` | ❌ | Token额度不足 |
| 波兰语 | polski | `pl` | `PL` | ❌ | Token额度不足 |
| 葡萄牙语（葡萄牙） | português | `pt` | `PT` | ❌ | Token额度不足 |
| 葡萄牙语（巴西） | português do Brasil | `pt-br` | `PTBR` | ❌ | Token额度不足 |
| 罗马尼亚语 | română | `ro` | `RO` | ❌ | Token额度不足 |
| 俄语 | русский | `ru` | `RU` | ❌ | Token额度不足 |
| 泰语 | ภาษาไทย | `th` | `TH` | ❌ | Token额度不足 |
| 土耳其语 | Türkçe | `tr` | `TR` | ❌ | Token额度不足 |
| 乌克兰语 | українська | `uk` | `UA` | ❌ | Token额度不足 |

**总计**：27 种计划语言 | **已支持**：5 种 | **待支持**：22 种

---

## 如何安装和使用

这是为想要在游戏中直接使用本翻译项目的玩家准备的指南。

1.  前往我们的 Steam 创意工坊页面：[[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  点击「订阅」按钮。
3.  启动游戏，在游戏主菜单的「模组」管理中启用本翻译模组。
4.  后启用的模组的翻译文本优先覆盖先启用的模组，因此本翻译模组需在功能模组之后启用（尽量置底）。
5.  享受游戏！

---

## 翻译进度

**[➡️ 点击此处查看翻译进度](./docs/progress/progress_zh-hans.md)**

---

## 如何贡献

我们欢迎任何人参与贡献，无论是修正一个错误、新增一个功能、撰写提示词模板、亦或是提供参考翻译！

调用LLM API进行翻译是需要为词元付费的，为了项目能够长期稳定运行，希望您能慷慨相助！

详情请阅读 [贡献指南](./docs/contributing/contributing_zh-hans.md)

---

## 工具与目录结构 (面向开发者)

本节内容面向希望了解项目自动化原理的开发者。

### 项目目录

| 目录 | 说明 |
|------|------|
| `src/` | .NET 10 翻译流水线源码，含 15 个模块 |
| `config/` | 流水线配置文件 (LLM、Steam、RAG 参数等) |
| `data/` | 运行时数据：模组元数据、embedding、翻译缓存 |
| `translation_ref/` | 参考翻译数据 (如一汉化组授权模组)，为 LLM 提供翻译参考 |
| `base_game_keys/` | 游戏本体翻译键，用于去重防止覆盖原生文本 |
| `final_outputs/` | 最终输出：`project_babel/` 模组包、`icons/` 图标与 `workshop_descriptions/` 创意工坊描述 |
| `docs/` | 项目文档：进度报告、贡献指南、流水线说明 |
| `temp/` | 流水线临时文件 (每次运行独立目录) |
| `src/prompt_templates/` | LLM 提示词模板 (翻译/内容审查) |

### 流水线模块 (按执行顺序)

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

详细的 [技术参考](./docs/technical_reference/technical_reference_zh-hans.md)。

---

## 版权与授权 (Copyright and License)

本翻译项目的翻译文本内容与相关图片，由 **Project Babel** 与各参与者基于原游戏模组创作或二次创作完成。

© 2025 Project Babel 及各作者保留权利。

### 1. 文本与图片等内容

除非另有特别说明，本仓库中的：

- 游戏内文本翻译、润色与校对内容；
- 项目说明文档、模组内文本翻译；
- 本项目专门制作的图片、美术资源

均采用 **署名-非商业性使用-相同方式共享 4.0 国际**
（Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International，简称 **CC BY-NC-SA 4.0**）协议授权。

这意味着，在遵守以下条件的前提下，您可以自由分享与改编这些内容：

- **署名（BY）**：在明显位置注明“本翻译项目基于『Project Babel』的工作成果进行修改”，并附上本仓库和 Steam 创意工坊链接  
`https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`

- **非商业性使用（NC）**：不得将本项目内容或其改编作品用于任何直接或间接的商业用途
  （包括但不限于付费整合包、付费下载、广告分成等）；
- **相同方式共享（SA）**：若您基于本项目内容进行修改或再创作，必须以 **同样的 CC BY-NC-SA 4.0 协议** 公开发布您的改动版本。

有关本协议的更多信息，请参见：
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.zh-Hans>

*特殊说明：*
- *base_game_keys文件夹内容来自游戏本体，版权归游戏开发商所有！内容用于防止翻译键覆盖游戏键(去重)*
- *translation_ref文件夹内容用于给LLM提供翻译参考，版权归各自模组开发者所有！*

### 2. 程序、脚本与其他开发内容

除非源码文件或目录中另有特别声明，本仓库中用于制作/打包/处理汉化内容的程序代码
（例如 `src/` 目录下的程序代码），
采用 **GNU 通用公共许可证第 3 版（GPL-3.0）** 进行授权。

完整条款请参见本仓库根目录下的 `LICENSE-GPL-3.0` 文件，
或访问 GNU 官网：<https://www.gnu.org/licenses/gpl-3.0.html>。

---

## 致谢

本项目使用了第三方的模组作为目标语言翻译的参考文本，参考文本被发送给LLM进行翻译参考。

| 参考模组名称 | 作者 | 模组页面 |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**对以上作者致以衷心感谢！**

---

## 第三方程序

本项目使用了第三方程序、库，这些第三方程序的版权归对应开发者所有。

