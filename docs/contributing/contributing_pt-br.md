# Guia de Contribuição (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Índice

- [1. Antes de começar](#1-antes-de-começar)
- [2. Como posso contribuir?](#2-como-posso-contribuir)
- [3. Fornecer regras de tradução, dicionário de termos, melhorar prompts do sistema](#3-fornecer-regras-de-tradução-dicionário-de-termos-melhorar-prompts-do-sistema)
- [4. Fornecer corpus de revisão manual](#4-fornecer-corpus-de-revisão-manual)
- [5. Contribuição para o pipeline e desenvolvimento de ferramentas](#5-contribuição-para-o-pipeline-e-desenvolvimento-de-ferramentas)
- [6. Direitos autorais e licenciamento](#6-direitos-autorais-e-licenciamento)
  - [6.1 Princípios básicos: você mantém os direitos autorais e concede licença ao projeto](#61-princípios-básicos-você-mantém-os-direitos-autorais-e-concede-licença-ao-projeto)
  - [6.2 Licenciamento de textos, imagens e outros conteúdos (CC BY-NC-SA 4.0)](#62-licenciamento-de-textos-imagens-e-outros-conteúdos-cc-by-nc-sa-40)
  - [6.3 Licenciamento de scripts e códigos de ferramentas (GPL-3.0)](#63-licenciamento-de-scripts-e-códigos-de-ferramentas-gpl-30)
  - [6.4 Direitos autorais dos trabalhos upstream e do jogo original](#64-direitos-autorais-dos-trabalhos-upstream-e-do-jogo-original)
- [7. Comunicação e colaboração](#7-comunicação-e-colaboração)
- [8. Suporte financeiro](#8-suporte-financeiro)

---

Muito obrigado por estar disposto a contribuir para o **Project Babel - Projeto de Tradução Automática por LLM para Mods de Project Zomboid**! Seja corrigindo um erro, adicionando um novo recurso, escrevendo modelos de prompt ou fornecendo traduções de referência!

Chamar a API do LLM para tradução requer pagamento por tokens. Para que o projeto possa operar de forma estável a longo prazo, esperamos que você possa ajudar generosamente!

> ⚠️ **Aviso Importante:**
> Antes de enviar qualquer conteúdo para este repositório, leia e compreenda a seção "Acordo de Direitos Autorais e Licenciamento".
> Uma vez enviado e mesclado, você concorda com os termos de licenciamento correspondentes.

---

## 1. Antes de começar

Leia primeiro o `README.md` do projeto para entender:
- O objetivo geral e o estado atual do projeto;
- Como jogadores comuns podem usar este projeto (para autoteste);
- Os detalhes técnicos do projeto.

---

## 2. Como posso contribuir?

Você pode escolher uma ou mais formas de participar de acordo com seus interesses e habilidades:

- Fornecer regras de tradução para o idioma alvo
- Fornecer um dicionário de termos de tradução para o idioma alvo
- Melhorar os prompts do sistema
- Fornecer corpus de tradução revisado manualmente
- Melhorar os módulos do pipeline (.NET) e scripts de automação
- Relatar problemas e sugerir melhorias (nas Issues)
- Fornecer suporte financeiro para chamadas do LLM

Abaixo, algumas explicações sobre os principais cenários de contribuição.

---

## 3. Fornecer regras de tradução, dicionário de termos, melhorar prompts do sistema

Os modelos de prompt do pipeline estão localizados em `src/prompt_templates/`, com a seguinte estrutura:

- `system_prompt_translate_engine.txt`: prompt do sistema do motor de tradução global (compartilhado por todos os idiomas);
- `<código_do_idioma>/translation_dictionary_<código_do_idioma>.json`: dicionário de termos do idioma;
- `<código_do_idioma>/translation_schema_<código_do_idioma>.md`: regras de tradução e restrições de estilo do idioma.

Etapas para contribuir:

1. Crie um subdiretório para seu idioma em `src/prompt_templates/`, adicione o dicionário de termos e o arquivo de regras de tradução;
2. Se precisar ajustar o comportamento global de tradução, modifique `system_prompt_translate_engine.txt` (lembre-se de que afeta todos os idiomas);
3. Teste local para confirmar o efeito;
4. Envie um PR.

---

## 4. Fornecer corpus de revisão manual

Se você é um criador de mods de tradução e está disposto a fornecer seu corpus de tradução como referência para a tradução do LLM, abra uma solicitação no Issue. Você precisa fornecer as seguintes informações:

- O Mod ID do seu mod de tradução e o idioma alvo da tradução;
- Captura de tela da página de administração do seu mod de tradução, para provar que você é o autor do mod;
- Declare claramente no Issue que você está disposto a fornecer o corpus de tradução;
- Se houver circunstâncias especiais (licenciamento especial, etc.), explique também;
- Certifique-se de que o corpus fornecido seja de alta qualidade.

Com sua autorização, o projeto incluirá seu mod na lista de mods de tradução de referência em `config/ref_translation_mods.json`, e o pipeline sincronizará automaticamente seu texto de tradução como corpus de referência RAG.

---

## 5. Contribuição para o pipeline e desenvolvimento de ferramentas

A automação deste projeto é dividida em duas partes:

**Módulo de pipeline (`src/`, C# / .NET 10)**: Contém 15 módulos executados em sequência, responsáveis pelo fluxo completo desde a inicialização do SteamCMD, download de mods, extração de texto, revisão de conteúdo, cálculo de Embedding, recuperação RAG até a tradução LLM e saída final. Consulte [Referência Técnica](../technical_reference/technical_reference_pt-br.md).

**Scripts auxiliares (.github/)**: usados para automação do GitHub.

Se você deseja:

* Corrigir bugs nos módulos ou scripts existentes do pipeline;
* Adicionar novos recursos ou módulos ao pipeline;
* Otimizar desempenho ou estrutura de código;
* Melhorar modelos de prompt ou estratégias RAG;

Você pode seguir os seguintes passos:

1. Faça um fork deste repositório e clone-o localmente;
2. Crie um novo branch a partir do branch mais recente;
3. Modifique ou adicione arquivos nos diretórios correspondentes:
- Modificação do módulo do pipeline → `src/<nome_do_módulo>/`;
- Modificação de scripts → `scripts/`;
- Modificação de templates de prompt → `src/prompt_templates/`;
4. Antes de enviar, tente ao máximo:

* Manter o estilo de código original;
* Adicionar comentários necessários;
* Se possível, incluir testes simples ou instruções de uso;
5. Envie a modificação via PR e descreva na descrição:

* Objetivo da alteração;
* Diretórios/módulos/scripts possivelmente afetados;
* Se envolve alterações de quebra.

---

## 6. Direitos autorais e licenciamento

> **Aviso importante:**
> As regras de direitos autorais e licenciamento visam proteger os interesses legítimos do projeto, autores, colaboradores e jogadores, evitando mal-entendidos por "tácito acordo" ou "padrão". Leia com atenção.
> Os direitos autorais e licenciamento prevalecem conforme o conteúdo do arquivo README.md; esta seção fornece apenas uma descrição mais acessível.

### 6.1 Princípios básicos: você mantém os direitos autorais e concede licença ao projeto

* Você ainda detém os direitos autorais sobre o conteúdo que criar (traduções, imagens, scripts/programas, etc.);
* Mas, ao submeter esse conteúdo a este projeto e ele ser aceito (mesclado), você concorda em licenciá-lo externamente conforme a licença de código aberto/compartilhamento adotada por este projeto.

Isso significa:

* Você **ainda pode** continuar usando e exibindo seu trabalho em outros lugares;
* Mas você **não pode**, após sua contribuição ser mesclada, exigir que este projeto ou outros usuários que obtiveram legalmente o trabalho "retirem a licença" ou "excluam versões históricas".

### 6.2 Licenciamento de textos, imagens e outros conteúdos (CC BY-NC-SA 4.0)

Para o seguinte conteúdo que você submeter:

* Traduções de textos do jogo, revisões e correções;
* Documentação do projeto, textos explicativos;
* Imagens e recursos artísticos criados especificamente para este projeto;

Uma vez adotado e mesclado neste repositório, considera-se que você concorda:

1. Este conteúdo é licenciado sob **Atribuição-NãoComercial-CompartilhaIgual 4.0 Internacional** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviado como **CC BY-NC-SA 4.0**);
2. O Project Babel e todos os usuários que obtiverem este conteúdo podem, **nos termos da CC BY-NC-SA 4.0**:
* Compartilhar, copiar e redistribuir este conteúdo;
* Modificá-lo e recriá-lo em usos não comerciais;
3. Você concorda que, dentro dos limites permitidos pela lei aplicável, esta licença é uma permissão **não exclusiva, mundial, isenta de royalties e irrevogável**;
4. Mesmo que você se retire ou pare de participar deste projeto no futuro, este projeto pode continuar usando e republicando o conteúdo que você já submeteu e foi mesclado, de acordo com a CC BY-NC-SA 4.0.

> Se você não aceitar a forma de licenciamento acima, não envie contribuições de texto ou imagem para este projeto,
> ou comunique-se previamente com os mantenedores do projeto para confirmar se é possível colaborar de outra forma.

### 6.3 Licenciamento de scripts e códigos de ferramentas (GPL-3.0)

Para o seguinte que você submeta e seja aceito:

* Scripts automatizados;
* Ferramentas de construção/exportação;
* Outros códigos de programa para processar este projeto de tradução;

Na ausência de declaração especial, considera-se que você concorda:

1. O código é licenciado sob **GPL-3.0** (GNU General Public License versão 3);
2. Os mantenedores do projeto podem modificá-lo, mesclá-lo e distribuí-lo dentro do escopo permitido pela GPL-3.0;
3. Você também pode continuar outros projetos com base no mesmo código, desde que cumpra os termos da GPL-3.0.

Para evitar conflitos de licenciamento, por favor:

* Não introduza código de terceiros **incompatível com GPL-3.0** sem verificação;
* Se precisar referenciar bibliotecas de terceiros, indique claramente a origem e a licença no PR e confirme a compatibilidade.

### 6.4 Direitos autorais dos trabalhos upstream e do jogo original

Este projeto é uma **tradução não oficial** para mods relacionados ao *Project Zomboid*:

* Os direitos autorais do jogo original e de cada mod pertencem a seus respectivos autores/distribuidores;
* Este projeto cria e organiza apenas tradução de texto, ajustes de refinamento e alguns recursos complementares;
* Ao enviar conteúdo, os contribuidores devem garantir:
* Não copiar diretamente textos de tradução ou recursos gráficos de terceiros não autorizados;
* Respeitar os direitos dos autores originais e dos criadores de mods, não realizando reprodução infratora.

---

## 7. Comunicação e colaboração

Se você:

* Tem dúvidas sobre os termos de licenciamento;
* Não tem certeza se determinado conteúdo pode ser contribuído;
* Deseja licenciar seu trabalho de forma especial (por exemplo, apenas para uso não comercial sem permitir adaptações);

Entre em contato com os mantenedores do projeto através dos seguintes meios:

* Abra uma Issue para discussão;
* Outros meios de contato publicamente fornecidos pelos mantenedores.

Faremos o possível para encontrar uma solução que equilibre os interesses de todas as partes, respeitando os direitos de cada um.

---

## 8. Suporte financeiro

Durante a execução do projeto, devido à adição de novos mods e atualizações de conteúdo textual de mods antigos, é necessário chamar continuamente a API LLM para tradução. E para restringir o comportamento do LLM, além do texto básico dos mods, é necessário fornecer uma grande quantidade de conteúdo de prompt (incluindo prompts base, regras de tradução, glossário, restrições de entrada/saída, resultados de consulta semântica, etc.), que consomem muito mais tokens do que o texto original. Portanto, o projeto precisa de suporte financeiro.

Se você deseja fornecer suporte financeiro, entre em contato com os mantenedores do projeto. Muito obrigado!

---

Mais uma vez, agradecemos por estar disposto a contribuir para este projeto!
Cada uma de suas contribuições beneficiará mais jogadores!
