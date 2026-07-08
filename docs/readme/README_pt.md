# Project Babel — Tradução automática de mods PZ por LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

> ⚠️ **Nota:** Esta tradução ainda não é suportada. O conteúdo autorizado é a [versão chinesa](../../README.md).

---

*Este projeto de tradução é mantido pelo conjunto de ferramentas [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Índice

- [Línguas-alvo suportadas](#línguas-alvo-suportadas)
- [Instalação e utilização](#instalação-e-utilização)
- [Progresso da tradução](#progresso-da-tradução)
- [Contribuir](#contribuir)
- [Ferramentas e estrutura de diretórios (para desenvolvedores)](#ferramentas-e-estrutura-de-diretórios-(para-desenvolvedores))
- [Direitos de autor e licença](#direitos-de-autor-e-licença)
- [Agradecimentos](#agradecimentos)
- [Software de terceiros](#software-de-terceiros)

---

## Línguas-alvo suportadas

| Língua | Nome local | Código ISO | Código no jogo | Suportada | Notas |
|------|------|------|------|------|------|
| Árabe | العربية | `ar` | `AR` | ❌ | Falta de financiamento |
| Catalão | català | `ca` | `CA` | ❌ | Falta de financiamento |
| Chinês tradicional | 繁體中文 | `zh-hant` | `CH` | ❌ | Falta de financiamento |
| Chinês simplificado | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Checo | čeština | `cs` | `CS` | ❌ | Falta de financiamento |
| Dinamarquês | dansk | `da` | `DA` | ❌ | Falta de financiamento |
| Alemão | Deutsch | `de` | `DE` | ✅ | |
| Inglês | English | `en` | `EN` | ✅ | |
| Espanhol | español | `es` | `ES` | ❌ | Falta de financiamento |
| Finlandês | suomi | `fi` | `FI` | ❌ | Falta de financiamento |
| Francês | français | `fr` | `FR` | ✅ | |
| Húngaro | magyar | `hu` | `HU` | ❌ | Falta de financiamento |
| Indonésio | Bahasa Indonesia | `id` | `ID` | ❌ | Falta de financiamento |
| Italiano | italiano | `it` | `IT` | ❌ | Falta de financiamento |
| Japonês | 日本語 | `ja` | `JP` | ✅ | |
| Coreano | 한국어 | `ko` | `KO` | ❌ | Falta de financiamento |
| Neerlandês | Nederlands | `nl` | `NL` | ❌ | Falta de financiamento |
| Norueguês | norsk | `no` | `NO` | ❌ | Falta de financiamento |
| Tagalo | Tagalog | `tl` | `PH` | ❌ | Falta de financiamento |
| Polaco | polski | `pl` | `PL` | ❌ | Falta de financiamento |
| Português (Portugal) | português | `pt` | `PT` | ❌ | Falta de financiamento |
| Português (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Falta de financiamento |
| Romeno | română | `ro` | `RO` | ❌ | Falta de financiamento |
| Russo | русский | `ru` | `RU` | ❌ | Falta de financiamento |
| Tailandês | ภาษาไทย | `th` | `TH` | ❌ | Falta de financiamento |
| Turco | Türkçe | `tr` | `TR` | ❌ | Falta de financiamento |
| Ucraniano | українська | `uk` | `UA` | ❌ | Falta de financiamento |

**Total**: 27 línguas planeadas | **Suportadas**: 5 | **Pendentes**: 22

---

## Instalação e utilização

Guia para jogadores que desejam usar o pacote de tradução no jogo.

1. Vai para a página do Steam Workshop: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Clica em "Subscrever".
3. Inicia o jogo, ativa este mod de tradução no menu Mods.
4. O texto de tradução dos mods carregados depois sobrescreve os anteriores, por isso este mod de tradução deve ser carregado depois dos mods de jogo.
5. Diverte-te!

---

## Progresso da tradução

[➡️ Progresso da tradução](../progress/progress_pt.md)

---

## Contribuir

Aceitamos contribuições! Correções de tradução, novas funcionalidades, modelos de prompt ou traduções de referência.

As chamadas à API LLM para tradução geram custos de tokens. O seu apoio ajuda o projeto a funcionar de forma sustentável!

---

## Ferramentas e estrutura de diretórios (para desenvolvedores)

Esta secção é destinada a programadores que desejam entender o funcionamento interno da automação do projeto.

### Diretórios do projeto

| Diretório | Descrição |
|------|------|
| `src/` | Código-fonte do pipeline de tradução .NET 10, 15 módulos |
| `config/` | Configuração do pipeline (LLM, Steam, parâmetros RAG, etc.) |
| `data/` | Dados de execução: metadados de mods, embeddings, cache de tradução |
| `translation_ref/` | Traduções de referência como contexto LLM |
| `base_game_keys/` | Chaves de tradução do jogo base para deduplicação |
| `final_outputs/` | Saída final em formato de mod PZ |
| `docs/` | Documentação: progresso, contribuição, especificações do pipeline |
| `temp/` | Ficheiros temporários do pipeline |
| `src/prompt_templates/` | Modelos de prompt LLM |

### Módulos do pipeline (ordem de execução)

| Passo | Módulo | Função |
|------|------|------|
| 1 | `ConfigReader` | Carregar configuração/segredos/idiomas |
| 2 | `RepoDataLoader` | Carregar referências e cache de tradução |
| 3 | `ModIdCollector` | Recolher IDs de mods do Workshop |
| 4 | `ModInfoFetcher` | Obter metadados do Steam |
| 5 | `ModDownloader` | Descarregar mods via steamcmd |
| 6 | `ContentExtractor` | Analisar ficheiros de tradução → `TranslationEntry` |
| 7 | `ContentChecker` | Revisão de segurança do conteúdo |
| 8 | `EmbeddingFetcher` | Calcular vetores de embedding de texto |
| 9 | `TranslationBatcher` | Criar lotes de tradução |
| 10 | `RagContextRetriever` | Recuperar contextos RAG |
| 11 | `LLMTranslator` | Executar tradução LLM |
| 12 | `ResultWriter` | Gravar em data/ e translation_ref/ |
| 13 | `FinalOutputWriter` | Gerar saída final em formato mod PZ |
| 14 | `ProgressReporter` | Gerar relatórios de progresso |

### Stack tecnológico

- **Linguagem**: C# (.NET 10)
- **Plataforma alvo**: GitHub Actions Linux x64 runner
- **Testes**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurável)
- **Embedding**: Vetorização de texto para pesquisa de similaridade RAG
- **Revisão de conteúdo**: Auditoria de segurança multinível orientada por LLM

Documentação técnica detalhada: [Pipeline TranslationEntry](../pipeline/translation_entry_pipeline_pt.md)

---

## Direitos de autor e licença

© 2025 Project Babel e todos os autores. Todos os direitos reservados.

### Conteúdo (textos, imagens)

Licenciado sob **CC BY-NC-SA 4.0**.

- **Atribuição**: Indicar modificações baseadas em «Project Babel», com links do repositório e Workshop
- **Não comercial**: Uso comercial proibido
- **Partilhar igual**: As modificações devem ser publicadas sob a mesma licença

### Código

O código em `src/` está licenciado sob **GPL-3.0**.

---

## Agradecimentos

| Mod de referência | Autor | Página |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Muito obrigado aos autores acima!**

---

## Software de terceiros

Este projeto utiliza programas e bibliotecas de terceiros, cujos direitos de autor pertencem aos respetivos programadores.
