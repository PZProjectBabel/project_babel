# Projeto Babel — Projeto de tradução automática LLM para mods de Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Este projeto de tradução é conduzido e mantido pelo conjunto de ferramentas [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Índice

- [Idiomas de tradução alvo suportados](#idiomas-de-tradução-alvo-suportados)
- [Como instalar e usar](#como-instalar-e-usar)
- [Progresso da tradução](#progresso-da-tradução)
- [Como contribuir](#como-contribuir)
- [Ferramentas e Estrutura de Diretórios (para desenvolvedores)](#ferramentas-e-estrutura-de-diretórios-para-desenvolvedores)
  - [Diretórios do projeto](#diretórios-do-projeto)
  - [Módulos do pipeline (em ordem de execução)](#módulos-do-pipeline-em-ordem-de-execução)
  - [Módulos Independentes](#módulos-independentes)
  - [Stack de Tecnologia](#stack-de-tecnologia)
- [Direitos Autorais e Licenciamento](#direitos-autorais-e-licenciamento)
  - [1. Texto, imagens e outros conteúdos](#1-texto-imagens-e-outros-conteúdos)
  - [2. Programas, scripts e outros conteúdos de desenvolvimento](#2-programas-scripts-e-outros-conteúdos-de-desenvolvimento)
- [Agradecimentos](#agradecimentos)
- [Programas de terceiros](#programas-de-terceiros)

---

## Idiomas de tradução alvo suportados

| Idioma | Nome local | Código internacional | Código no jogo | Suportado | Observações |
|------|------|------|------|------|------|
| Árabe | العربية | `ar` | `AR` | ❌ | Saldo de token insuficiente |
| Catalão | català | `ca` | `CA` | ❌ | Saldo de token insuficiente |
| Chinês Tradicional | 繁體中文 | `zh-hant` | `CH` | ❌ | Saldo de token insuficiente |
| Chinês Simplificado | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tcheco | čeština | `cs` | `CS` | ❌ | Saldo de token insuficiente |
| Dinamarquês | dansk | `da` | `DA` | ❌ | Saldo de token insuficiente |
| Alemão | Deutsch | `de` | `DE` | ✅ | |
| Inglês | English | `en` | `EN` | ✅ | |
| Espanhol | español | `es` | `ES` | ❌ | Saldo de token insuficiente |
| Finlandês | suomi | `fi` | `FI` | ❌ | Saldo de token insuficiente |
| Francês | français | `fr` | `FR` | ✅ | |
| Húngaro | magyar | `hu` | `HU` | ❌ | Saldo de token insuficiente |
| Indonésio | Bahasa Indonesia | `id` | `ID` | ❌ | Saldo de token insuficiente |
| Italiano | italiano | `it` | `IT` | ❌ | Saldo de token insuficiente |
| Japonês | 日本語 | `ja` | `JP` | ✅ | |
| Coreano | 한국어 | `ko` | `KO` | ❌ | Saldo de token insuficiente |
| Holandês | Nederlands | `nl` | `NL` | ❌ | Saldo de token insuficiente |
| Norueguês | norsk | `no` | `NO` | ❌ | Saldo de token insuficiente |
| Tagalo | Tagalog | `tl` | `PH` | ❌ | Saldo de token insuficiente |
| Polonês | polski | `pl` | `PL` | ❌ | Saldo de token insuficiente |
| Português (Portugal) | português | `pt` | `PT` | ❌ | Saldo de token insuficiente |
| Português (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Saldo de token insuficiente |
| Romeno | română | `ro` | `RO` | ❌ | Saldo de token insuficiente |
| Russo | русский | `ru` | `RU` | ❌ | Saldo de token insuficiente |
| Tailandês | ภาษาไทย | `th` | `TH` | ❌ | Saldo de token insuficiente |
| Turco | Türkçe | `tr` | `TR` | ❌ | Saldo de tokens insuficiente |
| Ucraniano | українська | `uk` | `UA` | ❌ | Saldo de tokens insuficiente |

**Total**: 27 idiomas planejados | **Suportados**: 5 | **Pendentes**: 22

---

## Como instalar e usar

Este é um guia para jogadores que desejam usar este projeto de tradução diretamente no jogo.

1.  Vá para nossa página da Steam Workshop: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Clique no botão "Inscrever-se".
3.  Inicie o jogo e ative este mod de tradução no gerenciador de "Mods" no menu principal.
4.  Os textos de tradução dos mods ativados posteriormente sobrescrevem os dos mods ativados primeiro. Portanto, este mod de tradução deve ser ativado após os mods de funcionalidade (coloque-o o mais abaixo possível).
5.  Aproveite o jogo!

---

## Progresso da tradução

**[➡️ Clique aqui para ver o progresso da tradução](./docs/progress/progress_pt-br.md)**

---

## Como contribuir

Nós damos as boas-vindas a qualquer pessoa para contribuir, seja corrigindo um erro, adicionando um recurso, escrevendo modelos de prompt ou fornecendo traduções de referência!

Chamar a API do LLM para tradução requer pagamento por tokens. Para que o projeto possa funcionar de forma estável a longo prazo, esperamos sua generosa contribuição!

Consulte o [Guia de Contribuição](./docs/contributing/contributing_pt-br.md) para obter detalhes.

---

## Ferramentas e Estrutura de Diretórios (para desenvolvedores)

Esta seção é destinada a desenvolvedores que desejam entender os princípios de automação do projeto.

### Diretórios do projeto

| Diretório | Descrição |
|------|------|
| `src/` | Código-fonte do pipeline de tradução .NET 10, contendo 15 módulos + 2 módulos independentes |
| `config/` | Arquivos de configuração do pipeline (parâmetros LLM, Steam, RAG, etc.) |
| `data/` | Dados em tempo de execução: metadados de mods, embeddings, cache de tradução |
| `translation_ref/` | Dados de tradução de referência (por exemplo, mods autorizados do grupo de localização As1), fornecendo referência de tradução para o LLM |
| `base_game_keys/` | Chaves de tradução do jogo base, usadas para deduplicação e evitar sobrescrever o texto nativo |
| `final_outputs/` | Saída final: pacote de mods `project_babel/`, ícones em `icons/` e descrições da Workshop em `workshop_descriptions/` |
| `docs/` | Documentação do projeto: relatórios de progresso, guia de contribuição, descrição do pipeline |
| `temp/` | Arquivos temporários do pipeline (diretórios independentes por execução) |
| `src/prompt_templates/` | Modelos de prompt do LLM (tradução/revisão de conteúdo) |

### Módulos do pipeline (em ordem de execução)

| Passo | Módulo | Função |
|------|------|------|
| 1 | `ConfigReader` | Carregar configuração/chaves/lista de idiomas |
| 2 | `RepoDataLoader` | Carregar traduções de referência e cache de tradução |
| 3 | `ModIdCollector` | Coletar IDs de mods do Workshop |
| 4 | `ModInfoFetcher` | Obter metadados do Steam |
| 5 | `SteamCmdBootstrapper` | Preparar runtime steamcmd para a plataforma atual |
| 6 | `ModDownloader` | Baixar mods via steamcmd |
| 7 | `ContentExtractor` | Analisar arquivos de tradução de mods → `TranslationEntry` |
| 8 | `ContentChecker` | Revisão de segurança de conteúdo (drogas/pornografia/violência) |
| 9 | `EmbeddingFetcher` | Calcular vetores de embedding de texto |
| 10 | `TranslationBatcher` | Criar lotes de tradução independentes do idioma alvo |
| 11 | `RagContextRetriever` | Recuperar contexto RAG (chave exata + similaridade de embedding) |
| 12 | `LLMTranslator` | Chamar LLM para executar tradução |
| 13 | `ResultWriter` | Escrever em data/ e translation_ref/ |
| 14 | `FinalOutputWriter` | Gerar saída final no formato de mod PZ |
| 15 | `ProgressReporter` | Gerar relatório de progresso |

### Módulos Independentes

| Módulo | Função |
|------|------|
| `WorkshopMonitor` | Captura periodicamente novos mods da Steam Workshop, filtra por número de inscrições e os adiciona ao `request_for_translation.txt` |
| `DocGenerator` | Gerador de documentação multilíngue orientado por LLM |

### Stack de Tecnologia

- **Linguagem**: C# (.NET 10)
- **Plataforma alvo**: GitHub Actions Linux x64 runner
- **Testes**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurável)
- **Embedding**: Vetorização de texto para recuperação por similaridade RAG
- **Revisão de conteúdo**: Auditoria de segurança em vários níveis impulsionada por LLM

Detalhado [referência técnica](./docs/technical_reference/technical_reference_pt-br.md).

---

## Direitos Autorais e Licenciamento

O conteúdo do texto traduzido e as imagens relacionadas deste projeto de tradução são criados ou recriados pelo **Project Babel** e pelos participantes com base nos mods originais do jogo.

© 2025 Project Babel e seus autores. Todos os direitos reservados.

### 1. Texto, imagens e outros conteúdos

Salvo indicação em contrário, neste repositório:

- Tradução, revisão e correção de texto dentro do jogo;
Documentação do projeto, tradução de textos dentro do mod;
Imagens e recursos artísticos produzidos especificamente para este projeto

são licenciados sob a licença **Atribuição-NãoComercial-CompartilhaIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviada como **CC BY-NC-SA 4.0**).

Isso significa que, desde que você cumpra as seguintes condições, pode compartilhar e adaptar livremente estes conteúdos:

- **Atribuição (BY)**: Em local visível, indique "Este projeto de tradução é baseado no trabalho do 'Project Babel'", e anexe o link deste repositório e da Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Uso não comercial (NC)**: Não utilize o conteúdo deste projeto ou suas adaptações para qualquer finalidade comercial direta ou indireta (incluindo, mas não se limitando a pacotes pagos, downloads pagos, divisão de anúncios, etc.);
- **CompartilhaIgual (SA)**: Se você modificar ou recriar baseado neste conteúdo, deve publicar sua versão modificada sob a **mesma licença CC BY-NC-SA 4.0**.

Para mais informações sobre esta licença, consulte:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.pt_BR>

*Notas especiais:*
- *O conteúdo da pasta base_game_keys vem do jogo base, direitos autorais pertencem ao desenvolvedor do jogo! O conteúdo é usado para evitar que chaves de tradução sobrescrevam chaves do jogo (deduplicação)*
- *O conteúdo da pasta translation_ref é usado para fornecer referência de tradução ao LLM, direitos autorais pertencem aos respectivos desenvolvedores de mods!*

### 2. Programas, scripts e outros conteúdos de desenvolvimento

A menos que declarado de outra forma nos arquivos de código ou diretórios, o código do programa usado para criar/empacotar/processar conteúdos de tradução neste repositório (por exemplo, código no diretório `src/`) é licenciado sob a **GNU General Public License versão 3 (GPL-3.0)**.

Consulte os termos completos no arquivo `LICENSE` na raiz deste repositório (GPL-3.0) ou visite o site oficial da GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Agradecimentos

Este projeto utiliza mods de terceiros como textos de referência para tradução no idioma alvo. Os textos de referência são enviados ao LLM para consulta de tradução.

| Nome do mod de referência | Autor | Página do mod |
|------|------|------|
| [B42] Unificação · Tradução Chinesa | Grupo de Tradução Ruyi (As1) | [Página da Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Unificação · Tradução de Mods | Grupo de Tradução Ruyi (As1) | [Página da Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Unificação · Tradução de Ark | Grupo de Tradução Ruyi (As1) | [Página da Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Agradecemos sinceramente aos autores acima!**

---

## Programas de terceiros

Este projeto utiliza programas e bibliotecas de terceiros, cujos direitos autorais pertencem aos respectivos desenvolvedores.

