# Guia de Contribuição (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Índice

- [1. Antes de começar](#1-antes-de-começar)
- [2. Como posso contribuir?](#2-como-posso-contribuir)
- [3. Fornecer regras de tradução, dicionário de termos e melhorar os prompts do sistema](#3-fornecer-regras-de-tradução-dicionário-de-termos-e-melhorar-os-prompts-do-sistema)
- [4. Fornecer corpus de tradução revisado manualmente](#4-fornecer-corpus-de-tradução-revisado-manualmente)
- [5. Contribuição para pipeline e desenvolvimento de ferramentas](#5-contribuição-para-pipeline-e-desenvolvimento-de-ferramentas)
- [6. Direitos autorais e termos de licenciamento](#6-direitos-autorais-e-termos-de-licenciamento)
  - [6.1 Princípios básicos: você mantém os direitos autorais e concede ao projeto o direito de uso](#61-princípios-básicos-você-mantém-os-direitos-autorais-e-concede-ao-projeto-o-direito-de-uso)
  - [6.2 Licenciamento de texto, imagens e outros conteúdos (CC BY-NC-SA 4.0)](#62-licenciamento-de-texto-imagens-e-outros-conteúdos-cc-by-nc-sa-40)
  - [6.3 Licenciamento de scripts e código de ferramentas (GPL-3.0)](#63-licenciamento-de-scripts-e-código-de-ferramentas-gpl-30)
  - [6.4 Obra original e direitos autorais do jogo original](#64-obra-original-e-direitos-autorais-do-jogo-original)
- [7. Comunicação e colaboração](#7-comunicação-e-colaboração)
- [8. Suporte financeiro](#8-suporte-financeiro)

---

Muito obrigado por estar disposto a contribuir para o **Project Babel - Projeto de tradução automática LLM para mods de Project Zomboid**! Seja corrigindo um erro, adicionando um novo recurso, escrevendo modelos de prompt, ou fornecendo traduções de referência!

Usar a API LLM para tradução exige pagamento por tokens. Para que o projeto possa operar de forma estável a longo prazo, esperamos que você possa ajudar generosamente!

> ⚠️ **Aviso importante:**
> Antes de enviar qualquer conteúdo para este repositório, leia e compreenda a seção "Acordo de Direitos Autorais e Licenciamento".
> Uma vez submetido e mesclado, considera-se que você concorda com os termos de licenciamento correspondentes.

---

## 1. Antes de começar

Por favor, leia o `README.md` do projeto para entender:
- O objetivo geral e o estado atual do projeto;
- Como jogadores comuns podem usar este projeto (para você testar facilmente);
- Detalhes técnicos do projeto.

---

## 2. Como posso contribuir?

Você pode participar escolhendo uma ou mais formas de acordo com seus interesses e habilidades:

- Fornecer regras de tradução para o idioma alvo
- Fornecer dicionário de termos de tradução para o idioma alvo
- Melhorar os prompts do sistema
- Fornecer corpus de tradução revisado por humanos
- Melhorar os módulos do pipeline (.NET) e scripts de automação
- Relatar problemas, sugerir melhorias (explicar nas Issues)
- Fornecer suporte financeiro para chamadas de LLM

Abaixo, algumas explicações sobre os principais cenários de contribuição.

---

## 3. Fornecer regras de tradução, dicionário de termos e melhorar os prompts do sistema

Os modelos de prompt do pipeline estão localizados em `src/prompt_templates/`, com a seguinte estrutura:

- `system_prompt_translate_engine.txt`: prompt do sistema do motor de tradução global (compartilhado por todos os idiomas);
- `<código do idioma>/translation_dictionary_<código do idioma>.json`: dicionário de termos desse idioma;
- `<código do idioma>/translation_schema_<código do idioma>.md`: regras de tradução e restrições de estilo desse idioma.

Etapas para contribuir:

1. Crie um subdiretório para o seu idioma em `src/prompt_templates/`, adicione o dicionário de termos e o arquivo de regras de tradução;
2. Se precisar ajustar o comportamento global de tradução, modifique `system_prompt_translate_engine.txt` (observe que afeta todos os idiomas);
3. Teste local para confirmar o efeito;
4. Envie o PR.

---

## 4. Fornecer corpus de tradução revisado manualmente

Se você é um criador de mods de tradução e está disposto a fornecer seu corpus de tradução como referência para a tradução LLM, abra uma solicitação no Issue. Você precisa fornecer as seguintes informações:

- O Mod ID do seu mod de tradução e o idioma de destino da tradução;
- Uma captura de tela da página de administração do seu mod de tradução para provar que você é o autor do mod;
- Declare claramente no Issue que você está disposto a fornecer o corpus de tradução;
- Se houver circunstâncias especiais (licenciamento especial, etc.), explique também;
- Certifique-se de que o corpus fornecido tenha alta qualidade.

Sob sua autorização, o projeto listará seu mod em `config/ref_translation_mods.json` na lista de mods de tradução de referência, e o pipeline sincronizará automaticamente seu texto traduzido como corpus de referência RAG.

---

## 5. Contribuição para pipeline e desenvolvimento de ferramentas

A automação deste projeto é dividida em duas partes:

**Módulo de pipeline (`src/`, C# / .NET 10)**: Contém 15 módulos executados em sequência, mais 2 módulos independentes (`WorkshopMonitor` para descoberta de mods, `DocGenerator` para geração de documentação), responsáveis por todo o fluxo, desde a inicialização do SteamCMD, download de mods, extração de texto, revisão de conteúdo, cálculo de embeddings, recuperação RAG até a tradução por LLM e saída final. Consulte [Referência técnica](../technical_reference/technical_reference_pt.md).

**Scripts auxiliares (`.github/`)**: Usados para automação do GitHub.

Se você deseja:

* Corrigir bugs nos módulos ou scripts existentes do pipeline;
* Adicionar novas funcionalidades ou novos módulos ao pipeline;
* Otimizar desempenho ou estrutura de código;
* Melhorar modelos de prompt ou estratégias RAG;

Você pode seguir os passos abaixo:

1. Faça um fork deste repositório e clone localmente;
2. Crie um novo branch baseado no branch mais recente;
3. Modifique ou adicione arquivos nos diretórios correspondentes:
- Modificações no módulo do pipeline → `src/<nome_do_módulo>/`;
- Modificação do fluxo de trabalho CI → `.github/workflows/`;
- Modificações em modelos de prompt → `src/prompt_templates/`;
4. Antes de enviar, tente:

* Manter o estilo de código original;
* Adicionar comentários necessários;
* Se possível, incluir testes simples ou instruções de uso;
5. Envie a modificação via PR e explique na descrição:

* Objetivo da alteração;
* Diretórios/módulos/scripts que podem ser afetados;
* Se envolve alteração disruptiva.

---

## 6. Direitos autorais e termos de licenciamento

> **Aviso importante:**
> Os termos de direitos autorais e licenciamento visam proteger os interesses legítimos do projeto, autores, contribuidores e jogadores, evitando mal-entendidos por "tácito acordo" ou "padrão". Por favor, leia atentamente.
> Os termos de direitos autorais e licenciamento têm como referência o conteúdo do arquivo README.md; esta seção fornece apenas uma descrição mais acessível.

### 6.1 Princípios básicos: você mantém os direitos autorais e concede ao projeto o direito de uso

* Você ainda detém os direitos autorais sobre o conteúdo que criar (traduções, imagens, scripts/programas, etc.);
* Mas, ao enviar esses conteúdos para este projeto e serem aceitos (mesclados), você concorda em licenciá-los para que outros os usem de acordo com a licença de código aberto/compartilhamento adotada por este projeto.

Isto significa:

* Você **ainda pode** continuar a usar e exibir seu trabalho em outros lugares;
* Mas você **não pode** exigir que este projeto ou outros usuários que obtiveram legalmente o trabalho "revoguem a autorização" ou "excluam versões históricas" após a contribuição ser mesclada.

### 6.2 Licenciamento de texto, imagens e outros conteúdos (CC BY-NC-SA 4.0)

Para os seguintes conteúdos que você enviar:

* Tradução de texto do jogo, revisão e conteúdo de correção;
* Documentação do projeto, textos explicativos;
* Imagens, recursos artísticos criados especificamente para este projeto;

Uma vez aceitos e mesclados neste repositório, você é considerado como concordando com:

1. Estes conteúdos são licenciados sob a licença **Atribuição-NãoComercial-CompartilhaIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviada como **CC BY-NC-SA 4.0**);
2. O Project Babel e todos os usuários que obtiverem este conteúdo podem, sob a condição de **cumprir os termos do CC BY-NC-SA 4.0**:
* Compartilhar, copiar e redistribuir este conteúdo;
* Modificá-lo e criar obras derivadas apenas para uso não comercial;
3. Você concorda que, dentro dos limites permitidos pela lei aplicável, esta licença é **não exclusiva, mundial, isenta de royalties e irrevogável**;
4. Mesmo que você se retire ou pare de participar deste projeto no futuro, este projeto ainda pode continuar a usar e redistribuir o conteúdo relevante que você já enviou e foi mesclado, de acordo com a CC BY-NC-SA 4.0.

> Se você não aceitar o método de licenciamento acima, por favor, não envie contribuições de texto ou imagem para este projeto,
> ou entre em contato com os mantenedores do projeto com antecedência para confirmar se é possível colaborar de outra forma.

### 6.3 Licenciamento de scripts e código de ferramentas (GPL-3.0)

Para as contribuições que você enviar e forem aceitas:

* Scripts de automação;
* Ferramentas de construção/exportação;
* Outros códigos de programa usados para processar este projeto de tradução;

Na ausência de declaração especial, considera-se que você concorda:

1. O código é licenciado sob **GPL-3.0** (GNU General Public License versão 3);
2. Os mantenedores do projeto podem modificá-lo, mesclá-lo e distribuí-lo dentro dos limites permitidos pela GPL-3.0;
3. Você também pode continuar a desenvolver outros projetos com base no mesmo código, desde que cumpra os termos da GPL-3.0.

Para evitar conflitos de licenciamento, tente ao máximo:

* Não introduzir código de terceiros **incompatível com GPL-3.0** sem confirmação prévia;
* Se precisar referenciar uma biblioteca de terceiros, explique claramente sua origem e licença no PR e confirme sua compatibilidade.

### 6.4 Obra original e direitos autorais do jogo original

Este projeto é uma tradução **não oficial** dos mods relacionados ao jogo *Project Zomboid*:

* Os direitos autorais do jogo original e de cada mod pertencem aos seus respectivos autores/distribuidores;
* Este projeto atua apenas na criação e organização de tradução de texto, ajustes de polimento e alguns recursos complementares;
* Ao enviar conteúdo, os contribuidores devem garantir:
* Não copiar diretamente textos de tradução ou recursos de arte de terceiros não autorizados;
* Respeitar os direitos dos autores originais e dos criadores dos mods, não realizando reprodução infratora.

---

## 7. Comunicação e colaboração

Se você:

* Tiver dúvidas sobre os termos de licenciamento;
* Não tiver certeza se determinado conteúdo pode ser contribuído;
* Desejar licenciar seu trabalho de forma especial (por exemplo, permitir apenas uso não comercial sem autorizar adaptações, etc.);

Entre em contato com os mantenedores do projeto através dos seguintes meios:

* Abrir uma Issue para discussão;
* Outros canais de contato fornecidos publicamente pelos mantenedores.

Faremos o possível para encontrar uma solução que equilibre o desenvolvimento saudável do projeto, respeitando os direitos de todas as partes.

---

## 8. Suporte financeiro

Durante a execução do projeto, devido à adição de novos mods, atualização de conteúdo textual de mods antigos, etc., é necessário continuar chamando a API do LLM para tradução. E para restringir o comportamento do LLM, além do texto básico dos mods, é necessário fornecer uma grande quantidade de conteúdo de prompt (incluindo prompt básico, regras de tradução, glossário, restrições de entrada e saída, resultados de pesquisa semântica, etc.), que consomem muito mais tokens do que o texto original. Portanto, o projeto precisa de suporte financeiro.

Se você estiver disposto a fornecer suporte financeiro, entre em contato com os mantenedores do projeto. Muito obrigado!

---

Mais uma vez, obrigado por sua disposição em contribuir com este projeto!
Cada uma das suas contribuições beneficiará mais jogadores!
