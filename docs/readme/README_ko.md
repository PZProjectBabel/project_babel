# Project Babel — 《僵尸毁灭工程》 모드 LLM 자동 번역 프로젝트

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*본 번역 프로젝트는 [Project Babel](https://github.com/PZProjectBabel/project_babel) 도구 세트에 의해 구동 및 유지 관리됩니다.*

---

## 목차

- [프로젝트가 지원하는 대상 번역 언어](#프로젝트가-지원하는-대상-번역-언어)
- [설치 및 사용 방법](#설치-및-사용-방법)
- [번역 진행 상황](#번역-진행-상황)
- [기여하는 방법](#기여하는-방법)
- [도구와 디렉터리 구조 (개발자용)](#도구와-디렉터리-구조-개발자용)
  - [프로젝트 디렉터리](#프로젝트-디렉터리)
  - [파이프라인 모듈 (실행 순서)](#파이프라인-모듈-실행-순서)
  - [독립 모듈](#독립-모듈)
  - [기술 스택](#기술-스택)
- [저작권 및 라이선스](#저작권-및-라이선스)
  - [1. 텍스트 및 이미지 등 콘텐츠](#1-텍스트-및-이미지-등-콘텐츠)
  - [2. 프로그램, 스크립트 및 기타 개발 콘텐츠](#2-프로그램-스크립트-및-기타-개발-콘텐츠)
- [감사의 말](#감사의-말)
- [타사 프로그램](#타사-프로그램)

---

## 프로젝트가 지원하는 대상 번역 언어

| 언어 | 현지 이름 | 국제 코드 | 게임 내 코드 | 지원 여부 | 비고 |
|------|------|------|------|------|------|
| 아랍어 | العربية | `ar` | `AR` | ❌ | Token 부족 |
| 카탈루냐어 | català | `ca` | `CA` | ❌ | Token 부족 |
| 중국어 번체 | 繁體中文 | `zh-hant` | `CH` | ❌ | Token 부족 |
| 중국어 간체 | 简体中文 | `zh-hans` | `CN` | ✅ | |
| 체코어 | čeština | `cs` | `CS` | ❌ | Token 부족 |
| 덴마크어 | dansk | `da` | `DA` | ❌ | Token 부족 |
| 독일어 | Deutsch | `de` | `DE` | ✅ | |
| 영어 | English | `en` | `EN` | ✅ | |
| 스페인어 | español | `es` | `ES` | ❌ | Token 부족 |
| 핀란드어 | suomi | `fi` | `FI` | ❌ | Token 부족 |
| 프랑스어 | français | `fr` | `FR` | ✅ | |
| 헝가리어 | magyar | `hu` | `HU` | ❌ | Token 부족 |
| 인도네시아어 | Bahasa Indonesia | `id` | `ID` | ❌ | Token 부족 |
| 이탈리아어 | italiano | `it` | `IT` | ❌ | Token 부족 |
| 일본어 | 日本語 | `ja` | `JP` | ✅ | |
| 한국어 | 한국어 | `ko` | `KO` | ❌ | Token 부족 |
| 네덜란드어 | Nederlands | `nl` | `NL` | ❌ | Token 부족 |
| 노르웨이어 | norsk | `no` | `NO` | ❌ | Token 부족 |
| 타갈로그어 | Tagalog | `tl` | `PH` | ❌ | Token 부족 |
| 폴란드어 | polski | `pl` | `PL` | ❌ | Token 부족 |
| 포르투갈어(포르투갈) | português | `pt` | `PT` | ❌ | Token 부족 |
| 포르투갈어(브라질) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token 부족 |
| 루마니아어 | română | `ro` | `RO` | ❌ | Token 부족 |
| 러시아어 | русский | `ru` | `RU` | ❌ | Token 부족 |
| 태국어 | ภาษาไทย | `th` | `TH` | ❌ | Token 부족 |
| 터키어 | Türkçe | `tr` | `TR` | ❌ | 토큰 한도 부족 |
| 우크라이나어 | українська | `uk` | `UA` | ❌ | 토큰 한도 부족 |

**총계**: 27개 계획 언어 | **지원됨**: 5개 | **대기 중**: 22개

---

## 설치 및 사용 방법

이것은 게임에서 직접 이 번역 프로젝트를 사용하려는 플레이어를 위한 안내서입니다.

1.  Steam 창작마당 페이지로 이동: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  「구독」 버튼을 클릭합니다.
3.  게임을 시작하고, 메인 메뉴의 「모드」 관리에서 이 번역 모드를 활성화합니다.
4.  나중에 활성화된 모드의 번역 텍스트가 먼저 활성화된 모드를 덮어쓰므로, 이 번역 모드는 기능 모드 이후에 활성화해야 합니다 (가능한 한 아래에 배치).
5.  게임을 즐기세요!

---

## 번역 진행 상황

**[➡️ 번역 진행 상황 보려면 여기를 클릭](./docs/progress/progress_ko.md)**

---

## 기여하는 방법

우리는 누구나 기여하는 것을 환영합니다. 오류 수정, 기능 추가, 프롬프트 템플릿 작성, 또는 참고 번역 제공 등 무엇이든 환영합니다!

LLM API를 호출하여 번역하려면 토큰 비용이 필요합니다. 프로젝트가 장기적으로 안정적으로 운영될 수 있도록 넉넉한 도움을 부탁드립니다!

자세한 내용은 [기여 가이드](./docs/contributing/contributing_ko.md)를 읽어주세요.

---

## 도구와 디렉터리 구조 (개발자용)

이 섹션은 프로젝트 자동화 원리를 알고 싶은 개발자를 대상으로 합니다.

### 프로젝트 디렉터리

| 디렉터리 | 설명 |
|------|------|
| `src/` | .NET 10 번역 파이프라인 소스 코드, 15개 모듈 + 2개 독립 모듈 포함 |
| `config/` | 파이프라인 구성 파일 (LLM, Steam, RAG 매개변수 등) |
| `data/` | 런타임 데이터: 모드 메타데이터, 임베딩, 번역 캐시 |
| `translation_ref/` | 참조 번역 데이터 (예: As1 한글화 그룹의 허가 모드), LLM에 번역 참조 제공 |
| `base_game_keys/` | 게임 본체 번역 키, 중복 제거 및 원본 텍스트 덮어쓰기 방지용 |
| `final_outputs/` | 최종 출력: `project_babel/` 모드 팩, `icons/` 아이콘 및 `workshop_descriptions/` 창작마당 설명 |
| `docs/` | 프로젝트 문서: 진행 보고서, 기여 가이드, 파이프라인 설명 |
| `temp/` | 파이프라인 임시 파일 (실행마다 독립 디렉터리) |
| `src/prompt_templates/` | LLM 프롬프트 템플릿 (번역/콘텐츠 검토) |

### 파이프라인 모듈 (실행 순서)

| 단계 | 모듈 | 기능 |
|------|------|------|
| 1 | `ConfigReader` | 구성/키/언어 목록 로드 |
| 2 | `RepoDataLoader` | 참조 번역 및 번역 캐시 로드 |
| 3 | `ModIdCollector` | Workshop 모드 ID 수집 |
| 4 | `ModInfoFetcher` | Steam 메타데이터 획득 |
| 5 | `SteamCmdBootstrapper` | 현재 플랫폼의 steamcmd 런타임 준비 |
| 6 | `ModDownloader` | steamcmd를 통해 모드 다운로드 |
| 7 | `ContentExtractor` | 모드 번역 파일 파싱 → `TranslationEntry` |
| 8 | `ContentChecker` | 콘텐츠 안전 심사 (마약/음란/폭력) |
| 9 | `EmbeddingFetcher` | 텍스트 embedding 벡터 계산 |
| 10 | `TranslationBatcher` | 대상 언어 독립적인 번역 배치 생성 |
| 11 | `RagContextRetriever` | RAG 컨텍스트 검색 (정확한 키 + embedding 유사도) |
| 12 | `LLMTranslator` | LLM 호출하여 번역 실행 |
| 13 | `ResultWriter` | data/ 및 translation_ref/에 쓰기 |
| 14 | `FinalOutputWriter` | 최종 PZ 모드 형식 출력 생성 |
| 15 | `ProgressReporter` | 진행 보고서 생성 |

### 독립 모듈

| 모듈 | 기능 |
|------|------|
| `WorkshopMonitor` | 정기적으로 Steam Workshop 새 모드를 가져와 구독 수로 필터링하여 `request_for_translation.txt`에 병합 |
| `DocGenerator` | LLM 기반 다국어 문서 생성기 |

### 기술 스택

- **언어**: C# (.NET 10)
- **대상 플랫폼**: GitHub Actions Linux x64 runner
- **테스트**: xUnit (Windows x64)
- **LLM**: DeepSeek API (구성 가능)
- **Embedding**: 텍스트 벡터화는 RAG 유사성 검색에 사용됨
- **내용 검토**: LLM 기반 다단계 보안 심사

자세한 [기술 참조](./docs/technical_reference/technical_reference_ko.md)

---

## 저작권 및 라이선스

본 번역 프로젝트의 번역 텍스트 내용과 관련 이미지는 **Project Babel** 및 각 참여자가 원본 게임 모드를 기반으로 창작 또는 2차 창작하여 완성되었습니다.

© 2025 Project Babel 및 모든 저자에게 권리가 있습니다.

### 1. 텍스트 및 이미지 등 콘텐츠

특별히 명시되지 않는 한, 본 저장소의:

- 게임 내 텍스트 번역, 다듬기 및 교정 내용;
프로젝트 설명 문서, 모드 내 텍스트 번역;
본 프로젝트에서 특별히 제작한 이미지, 미술 리소스

모두 **저작자표시-비영리-동일조건변경허락 4.0 국제** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, 약칭 **CC BY-NC-SA 4.0**) 라이선스로 제공됩니다.

이는 다음 조건을 준수하는 한, 이 내용을 자유롭게 공유 및 개작할 수 있음을 의미합니다:

- **저작자표시 (BY)**: 눈에 띄는 위치에 "본 번역 프로젝트는 'Project Babel'의 작업 결과를 기반으로 수정되었습니다"라고 명시하고, 본 저장소 및 Steam 창작마당 링크를 첨부하세요   `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **비영리 (NC)**: 본 프로젝트의 내용이나 이를 기반으로 한 2차 창작물을 직접적 또는 간접적인 상업적 목적(유료 통합팩, 유료 다운로드, 광고 수익 분배 등을 포함하되 이에 국한되지 않음)에 사용할 수 없습니다;
- **동일조건변경허락 (SA)**: 본 프로젝트의 내용을 기반으로 수정하거나 재창작하는 경우, **동일한 CC BY-NC-SA 4.0 라이선스**로 변경된 버전을 공개적으로 배포해야 합니다.

본 라이선스에 대한 자세한 내용은 다음을 참조하십시오:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.ko>

*특별 설명:*
- *base_game_keys 폴더의 내용은 게임 본체에서 가져온 것이며, 저작권은 게임 개발사에 있습니다! 번역 키가 게임 키를 덮어쓰는 것을 방지(중복 제거)하기 위한 내용입니다.*
- *translation_ref 폴더의 내용은 LLM에 번역 참고 자료를 제공하기 위한 것이며, 저작권은 각 모드 개발자에게 있습니다!*

### 2. 프로그램, 스크립트 및 기타 개발 콘텐츠

소스 파일이나 디렉터리에 별도 명시가 없는 한, 본 저장소에서 한글화 콘텐츠를 제작/패키징/처리하는 데 사용되는 프로그램 코드(예: `src/` 디렉터리의 프로그램 코드)는 **GNU 일반 공중 사용 허가서 제3판 (GPL-3.0)** 에 따라 사용이 허가됩니다.

전체 조항은 본 저장소 루트 디렉터리의 `LICENSE` 파일 (GPL-3.0)을 참조하거나 GNU 공식 웹사이트 <https://www.gnu.org/licenses/gpl-3.0.html>를 방문하십시오.

---

## 감사의 말

본 프로젝트는 타사의 모드를 대상 언어 번역의 참고 텍스트로 사용하며, 참고 텍스트는 LLM에 전송되어 번역 참고 자료로 사용됩니다.

| 참조 모드 이름 | 작성자 | 모드 페이지 |
|------|------|------|
| [B42] 통일·중문 한글화 | 여일 한글화 그룹 (As1) | [창작마당 페이지](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] 통일·모드 한글화 | 여일 한글화 그룹 (As1) | [창작마당 페이지](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] 통일·방주 한글화 | 여일 한글화 그룹 (As1) | [창작마당 페이지](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**위 작성자분들께 진심으로 감사드립니다!**

---

## 타사 프로그램

본 프로젝트는 타사 프로그램 및 라이브러리를 사용하며, 이 타사 프로그램의 저작권은 해당 개발자에게 있습니다.

