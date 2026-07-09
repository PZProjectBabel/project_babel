# Project Babel — PZ 모드 LLM 자동 번역

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **참고:** 이 번역은 아직 지원되지 않습니다. 공식 내용은 [중국어 버전](../../README.md)을 기준으로 합니다.

---

*이 번역 프로젝트는 [Project Babel](https://github.com/PZProjectBabel/project_babel) 도구 세트에 의해 운영 및 유지 관리됩니다.*

---

## 목차

- [지원 대상 언어](#지원-대상-언어)
- [설치 및 사용 방법](#설치-및-사용-방법)
- [번역 진행 상황](#번역-진행-상황)
- [기여하기](#기여하기)
- [도구 및 디렉터리 구조 (개발자용)](#도구-및-디렉터리-구조-(개발자용))
- [저작권 및 라이선스](#저작권-및-라이선스)
- [감사의 말](#감사의-말)
- [서드파티 소프트웨어](#서드파티-소프트웨어)

---

## 지원 대상 언어

| 언어 | 현지명 | ISO 코드 | 게임 내 코드 | 지원 여부 | 비고 |
|------|------|------|------|------|------|
| 아랍어 | العربية | `ar` | `AR` | ❌ | 토큰 크레딧 부족 |
| 카탈루냐어 | català | `ca` | `CA` | ❌ | 토큰 크레딧 부족 |
| 번체 중국어 | 繁體中文 | `zh-hant` | `CH` | ❌ | 토큰 크레딧 부족 |
| 간체 중국어 | 简体中文 | `zh-hans` | `CN` | ✅ | |
| 체코어 | čeština | `cs` | `CS` | ❌ | 토큰 크레딧 부족 |
| 덴마크어 | dansk | `da` | `DA` | ❌ | 토큰 크레딧 부족 |
| 독일어 | Deutsch | `de` | `DE` | ✅ | |
| 영어 | English | `en` | `EN` | ✅ | |
| 스페인어 | español | `es` | `ES` | ❌ | 토큰 크레딧 부족 |
| 핀란드어 | suomi | `fi` | `FI` | ❌ | 토큰 크레딧 부족 |
| 프랑스어 | français | `fr` | `FR` | ✅ | |
| 헝가리어 | magyar | `hu` | `HU` | ❌ | 토큰 크레딧 부족 |
| 인도네시아어 | Bahasa Indonesia | `id` | `ID` | ❌ | 토큰 크레딧 부족 |
| 이탈리아어 | italiano | `it` | `IT` | ❌ | 토큰 크레딧 부족 |
| 일본어 | 日本語 | `ja` | `JP` | ✅ | |
| 한국어 | 한국어 | `ko` | `KO` | ❌ | 토큰 크레딧 부족 |
| 네덜란드어 | Nederlands | `nl` | `NL` | ❌ | 토큰 크레딧 부족 |
| 노르웨이어 | norsk | `no` | `NO` | ❌ | 토큰 크레딧 부족 |
| 타갈로그어 | Tagalog | `tl` | `PH` | ❌ | 토큰 크레딧 부족 |
| 폴란드어 | polski | `pl` | `PL` | ❌ | 토큰 크레딧 부족 |
| 포르투갈어 (포르투갈) | português | `pt` | `PT` | ❌ | 토큰 크레딧 부족 |
| 포르투갈어 (브라질) | português do Brasil | `pt-br` | `PTBR` | ❌ | 토큰 크레딧 부족 |
| 루마니아어 | română | `ro` | `RO` | ❌ | 토큰 크레딧 부족 |
| 러시아어 | русский | `ru` | `RU` | ❌ | 토큰 크레딧 부족 |
| 태국어 | ภาษาไทย | `th` | `TH` | ❌ | 토큰 크레딧 부족 |
| 터키어 | Türkçe | `tr` | `TR` | ❌ | 토큰 크레딧 부족 |
| 우크라이나어 | українська | `uk` | `UA` | ❌ | 토큰 크레딧 부족 |

**총계**: 27개 계획 언어 | **지원됨**: 5개 | **대기 중**: 22개

---

## 설치 및 사용 방법

게임 내에서 번역 팩을 사용하려는 플레이어를 위한 가이드입니다.

1. Steam Workshop 페이지로 이동: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. "구독"을 클릭하세요.
3. 게임을 실행하고 Mods 메뉴에서 이 번역 모드를 활성화하세요.
4. 나중에 로드된 모드의 번역 텍스트가 이전 것을 덮어쓰므로, 이 번역 모드는 게임플레이 모드 이후에 로드해야 합니다.
5. 즐기세요!

---

## 번역 진행 상황

[➡️ 번역 진행 상황](../progress/progress_ko.md)

---

## 기여하기

기여를 환영합니다! 번역 수정, 새로운 기능, 프롬프트 템플릿 또는 참조 번역.

LLM API를 사용한 번역에는 토큰 비용이 발생합니다. 프로젝트의 장기적인 운영을 위해 여러분의 지원을 부탁드립니다!

자세한 내용은 [기여 가이드](../contributing/contributing_ko.md)를 참조하세요.

---

## 도구 및 디렉터리 구조 (개발자용)

이 섹션은 프로젝트 자동화의 내부 작동 방식을 이해하려는 개발자를 위한 것입니다.

### 프로젝트 디렉터리

| 디렉터리 | 설명 |
|------|------|
| `src/` | .NET 10 번역 파이프라인 소스 코드, 15개 모듈 |
| `config/` | 파이프라인 설정 (LLM, Steam, RAG 매개변수 등) |
| `data/` | 런타임 데이터: 모드 메타데이터, 임베딩, 번역 캐시 |
| `translation_ref/` | LLM 컨텍스트용 참조 번역 데이터 |
| `base_game_keys/` | 중복 제거용 기본 게임 번역 키 |
| `final_outputs/` | 최종 PZ 모드 형식 번역 출력 |
| `docs/` | 문서: 진행 상황, 기여, 파이프라인 사양 |
| `temp/` | 파이프라인 임시 파일 |
| `src/prompt_templates/` | LLM 프롬프트 템플릿 |

### 파이프라인 모듈 (실행 순서)

| 단계 | 모듈 | 기능 |
|------|------|------|
| 1 | `ConfigReader` | 설정/비밀/언어 로드 |
| 2 | `RepoDataLoader` | 참조 및 번역 캐시 로드 |
| 3 | `ModIdCollector` | Workshop 모드 ID 수집 |
| 4 | `ModInfoFetcher` | Steam 메타데이터 가져오기 |
| 5 | `ModDownloader` | steamcmd를 통해 모드 다운로드 |
| 6 | `ContentExtractor` | 모드 번역 파일 구문 분석 → `TranslationEntry` |
| 7 | `ContentChecker` | 콘텐츠 안전 검토 |
| 8 | `EmbeddingFetcher` | 텍스트 임베딩 벡터 계산 |
| 9 | `TranslationBatcher` | 번역 배치 생성 |
| 10 | `RagContextRetriever` | RAG 컨텍스트 검색 |
| 11 | `LLMTranslator` | LLM 번역 실행 |
| 12 | `ResultWriter` | data/ 및 translation_ref/에 쓰기 |
| 13 | `FinalOutputWriter` | 최종 PZ 모드 출력 생성 |
| 14 | `ProgressReporter` | 진행 보고서 생성 |

### 기술 스택

- **언어**: C# (.NET 10)
- **대상 플랫폼**: GitHub Actions Linux x64 runner
- **테스트**: xUnit (Windows x64)
- **LLM**: DeepSeek API (설정 가능)
- **Embedding**: RAG 유사도 검색을 위한 텍스트 벡터화
- **콘텐츠 검토**: LLM 기반 다단계 안전 감사

상세 기술 문서: [TranslationEntry 파이프라인](../pipeline/translation_entry_pipeline_ko.md)

---

## 저작권 및 라이선스

© 2025 Project Babel 및 모든 저자. 모든 권리 보유.

### 콘텐츠 (텍스트, 이미지)

**CC BY-NC-SA 4.0**에 따라 라이선스가 부여됩니다.

- **저작자 표시**: 「Project Babel」에 기반한 수정 사항을 명시하고, 저장소 및 Workshop 링크 첨부
- **비영리**: 상업적 이용 금지
- **동일조건변경허락**: 수정물은 동일한 라이선스로 공개해야 함

### 코드

`src/` 내 코드는 **GPL-3.0**에 따라 라이선스가 부여됩니다.

---

## 감사의 말

| 참조 모드 | 저자 | 페이지 |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**위 저자분들께 깊은 감사를 드립니다!**

---

## 서드파티 소프트웨어

이 프로젝트는 서드파티 프로그램 및 라이브러리를 사용하며, 저작권은 해당 개발자에게 귀속됩니다.
