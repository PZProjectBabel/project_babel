# Project Babel — Projeto de tradução automática LLM para o mod 《僵尸毁灭工程》

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Este projeto de tradução é conduzido e mantido pelo conjunto de ferramentas [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Índice

- [Idiomas de tradução alvo suportados pelo projeto](#idiomas-de-tradução-alvo-suportados-pelo-projeto)
- [Como instalar e usar](#como-instalar-e-usar)
- [Progresso da tradução](#progresso-da-tradução)
- [Como contribuir](#como-contribuir)
- [Ferramentas e estrutura de diretórios (para desenvolvedores)](#ferramentas-e-estrutura-de-diretórios-para-desenvolvedores)
  - [Diretórios do projeto](#diretórios-do-projeto)
  - [Módulos do pipeline (em ordem de execução)](#módulos-do-pipeline-em-ordem-de-execução)
  - [Módulos Independentes](#módulos-independentes)
  - [Stack tecnológico](#stack-tecnológico)
- [Copyright e licença](#copyright-e-licença)
  - [1. Conteúdo de texto e imagens, etc.](#1-conteúdo-de-texto-e-imagens-etc)
  - [2. Programas, scripts e outros conteúdos de desenvolvimento](#2-programas-scripts-e-outros-conteúdos-de-desenvolvimento)
- [Agradecimentos](#agradecimentos)
- [Programas de Terceiros](#programas-de-terceiros)

---

## Idiomas de tradução alvo suportados pelo projeto

| Idioma | Nome nativo | Código internacional | Código no jogo | Suportado? | Observações |
|------|------|------|------|------|------|
| Árabe | العربية | `ar` | `AR` | ❌ | Saldo insuficiente de tokens |
| Catalão | català | `ca` | `CA` | ❌ | Saldo insuficiente de tokens |
| Chinês tradicional | 繁體中文 | `zh-hant` | `CH` | ❌ | Saldo insuficiente de tokens |
| Chinês simplificado | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tcheco | čeština | `cs` | `CS` | ❌ | Saldo insuficiente de tokens |
| Dinamarquês | dansk | `da` | `DA` | ❌ | Saldo insuficiente de tokens |
| Alemão | Deutsch | `de` | `DE` | ✅ | |
| Inglês | English | `en` | `EN` | ✅ | |
| Espanhol | español | `es` | `ES` | ❌ | Saldo insuficiente de tokens |
| Finlandês | suomi | `fi` | `FI` | ❌ | Saldo insuficiente de tokens |
| Francês | français | `fr` | `FR` | ✅ | |
| Húngaro | magyar | `hu` | `HU` | ❌ | Saldo insuficiente de tokens |
| Indonésio | Bahasa Indonesia | `id` | `ID` | ❌ | Saldo insuficiente de tokens |
| Italiano | italiano | `it` | `IT` | ❌ | Saldo insuficiente de tokens |
| Japonês | 日本語 | `ja` | `JP` | ✅ | |
| Coreano | 한국어 | `ko` | `KO` | ❌ | Saldo insuficiente de tokens |
| Holandês | Nederlands | `nl` | `NL` | ❌ | Saldo insuficiente de tokens |
| Norueguês | norsk | `no` | `NO` | ❌ | Saldo insuficiente de tokens |
| Tagalo | Tagalog | `tl` | `PH` | ❌ | Saldo insuficiente de tokens |
| Polonês | polski | `pl` | `PL` | ❌ | Saldo insuficiente de tokens |
| Português (Portugal) | português | `pt` | `PT` | ❌ | Saldo insuficiente de tokens |
| Português (Brasil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Saldo insuficiente de tokens |
| Romeno | română | `ro` | `RO` | ❌ | Saldo insuficiente de tokens |
| Russo | русский | `ru` | `RU` | ❌ | Saldo insuficiente de tokens |
| Tailandês | ภาษาไทย | `th` | `TH` | ❌ | Saldo insuficiente de tokens |
| Turco | Türkçe | `tr` | `TR` | ❌ | Saldo de tokens insuficiente |
| Ucraniano | українська | `uk` | `UA` | ❌ | Saldo de tokens insuficiente |

**Total**: 27 línguas planejadas | **Suportadas**: 5 | **A serem suportadas**: 22

---

## Como instalar e usar

Este é um guia para jogadores que desejam usar este projeto de tradução diretamente no jogo.

1.  Vá para nossa página da Steam Workshop: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Clique no botão "Inscrever-se".
3.  Inicie o jogo e ative este mod de tradução no gerenciamento de "Mods" no menu principal.
4.  O texto de tradução de mods ativados posteriormente substitui o de mods ativados primeiro, portanto, este mod de tradução deve ser ativado após os mods funcionais (o mais abaixo possível).
5.  Aproveite o jogo!

---

## Progresso da tradução

**[➡️ Clique aqui para ver o progresso](./docs/progress/progress_pt.md)**

---

## Como contribuir

Aceitamos contribuições de qualquer pessoa, seja corrigindo um erro, adicionando uma função, escrevendo modelos de prompt ou fornecendo traduções de referência!

Chamar a API LLM para tradução requer pagamento por tokens. Para que o projeto possa funcionar de forma estável a longo prazo, esperamos sua generosa contribuição!

Para mais detalhes, leia o [Guia de Contribuição](./docs/contributing/contributing_pt.md)

---

## Ferramentas e estrutura de diretórios (para desenvolvedores)

Esta seção é destinada a desenvolvedores que desejam entender o princípio de automação do projeto.

### Diretórios do projeto

| Diretório | Descrição |
|------|------|
| `src/` | Código-fonte do pipeline de tradução .NET 10, contendo 15 módulos + 2 módulos independentes |
| `config/` | Arquivos de configuração do pipeline (parâmetros LLM, Steam, RAG, etc.) |
| `data/` | Dados em tempo de execução: metadados de mods, embeddings, cache de tradução |
| `translation_ref/` | Dados de tradução de referência (como mods autorizados pelo grupo de tradução Yihan), fornecendo referência de tradução para o LLM |
| `base_game_keys/` | Chaves de tradução do jogo base, usadas para deduplicação e evitar sobrescrever texto nativo |
| `final_outputs/` | Saída final: pacote de mod `project_babel/`, ícones `icons/` e descrições da Workshop `workshop_descriptions/` |
| `docs/` | Documentação do projeto: relatórios de progresso, guia de contribuição, descrição do pipeline |
| `temp/` | Arquivos temporários do pipeline (diretório independente a cada execução) |
| `src/prompt_templates/` | Modelos de prompt LLM (tradução/revisão de conteúdo) |

### Módulos do pipeline (em ordem de execução)

| Passos | Módulo | Função |
|------|------|------|
| 1 | `ConfigReader` | Carregar configuração/chave/lista de idiomas |
| 2 | `RepoDataLoader` | Carregar traduções de referência e cache de tradução |
| 3 | `ModIdCollector` | Coletar IDs de mods do Workshop |
| 4 | `ModInfoFetcher` | Obter metadados do Steam |
| 5 | `SteamCmdBootstrapper` | Preparar o runtime do steamcmd para a plataforma atual |
| 6 | `ModDownloader` | Baixar mods via steamcmd |
| 7 | `ContentExtractor` | Analisar arquivos de tradução do mod → `TranslationEntry` |
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
| `WorkshopMonitor` | Captura periodicamente novos mods do Steam Workshop, filtra por número de assinaturas e adiciona a `request_for_translation.txt` |
| `DocGenerator` | Gerador de documentação multilíngue impulsionado por LLM |

### Stack tecnológico

- **Linguagem**: C# (.NET 10)
- **Plataforma alvo**: GitHub Actions Linux x64 runner
- **Testes**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurável)
- **Embedding**: Vetorização de texto para recuperação por similaridade RAG
- **Revisão de conteúdo**: Auditoria de segurança multinível impulsionada por LLM

Detalhado [referência técnica](./docs/technical_reference/technical_reference_pt.md)

---

## Copyright e licença

Os textos traduzidos e imagens relacionadas deste projeto de tradução foram criados ou adaptados pelo **Project Babel** e pelos participantes com base nos mods originais do jogo.

© 2025 Project Babel e seus autores. Todos os direitos reservados.

### 1. Conteúdo de texto e imagens, etc.

A menos que especificado de outra forma, neste repositório:

- Traduções de texto no jogo, revisões e correções;
Documentação do projeto, tradução de textos dentro dos mods;
Imagens e recursos artísticos especialmente criados para este projeto

são licenciados sob **Atribuição-NãoComercial-CompartilhaIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviado como **CC BY-NC-SA 4.0**).

Isso significa que, desde que você cumpra as seguintes condições, pode livremente compartilhar e adaptar estes conteúdos:

- **Atribuição (BY)**: Em local visível, indicar que esta tradução é uma modificação baseada no trabalho do 'Project Babel' e anexar o link deste repositório e da Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **NãoComercial (NC)**: Não utilizar o conteúdo deste projeto ou suas adaptações para qualquer fim comercial, direto ou indireto (incluindo, mas não se limitando a, pacotes pagos, downloads pagos, divisão de publicidade, etc.);
- **CompartilhaIgual (SA)**: Se modificar ou criar obras derivadas com base neste projeto, deverá publicar sua versão modificada sob a **mesma licença CC BY-NC-SA 4.0**.

Para mais informações sobre esta licença, consulte:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.pt>

*Observações especiais:*
- *O conteúdo da pasta base_game_keys é proveniente do jogo original, direitos autorais pertencem aos desenvolvedores do jogo! O conteúdo é usado para evitar que as chaves de tradução sobrescrevam as chaves do jogo (deduplicação)*
- *O conteúdo da pasta translation_ref é utilizado para fornecer referência de tradução ao LLM, os direitos autorais pertencem aos respectivos desenvolvedores dos mods!*

### 2. Programas, scripts e outros conteúdos de desenvolvimento

Salvo declaração especial em contrário nos arquivos ou diretórios de código-fonte, os códigos de programa (por exemplo, os códigos no diretório `src/`) utilizados para criar/empacotar/processar o conteúdo de tradução chinesa neste repositório são licenciados sob **GNU General Public License versão 3 (GPL-3.0)**.

Para os termos completos, consulte o arquivo `LICENSE` na raiz deste repositório (GPL-3.0) ou visite o site oficial da GNU: <https://www.gnu.org/licenses/gpl-3.0.html>.

---

## Agradecimentos

Este projeto utiliza mods de terceiros como texto de referência para a tradução no idioma alvo. O texto de referência é enviado ao LLM como base para a tradução.

| Nome do Mod de Referência | Autor | Página do Mod |
|------|------|------|
| [B42]Unificação·Tradução Chinesa | Grupo de Tradução Ruyi (As1) | [Página da Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]Unificação·Tradução de Mods | Grupo de Tradução Ruyi (As1) | [Página da Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]Unificação·Tradução de Ark | Grupo de Tradução Ruyi (As1) | [Página da Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Expressamos sinceros agradecimentos aos autores acima!**

---

## Programas de Terceiros

Este projeto utiliza programas e bibliotecas de terceiros. Os direitos autorais desses programas de terceiros pertencem aos seus respectivos desenvolvedores.

