# Guia de Contribuição (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Muito obrigado por sua disposição em contribuir com o **Project Babel — o projeto de tradução automática com LLM para mods de Project Zomboid**! Seja corrigindo um bug, adicionando um recurso, escrevendo modelos de prompt ou fornecendo traduções de referência — cada contribuição importa!

Chamar a API do LLM para tradução tem custo de tokens. Para que o projeto possa funcionar de forma sustentável a longo prazo, seu generoso apoio é muito apreciado!

> ⚠️ **Aviso Importante:**
> Antes de enviar qualquer coisa para este repositório, leia e entenda a seção "Direitos Autorais e Licenciamento".
> Uma vez enviado e mesclado, considera-se que você concorda com os termos de licenciamento correspondentes.

---

## Antes de Começar

Por favor, leia o README.md do projeto para entender:

- Os objetivos gerais e o estado atual deste projeto;
- Como os jogadores comuns usam este projeto (para seus próprios testes);
- Detalhes técnicos do projeto.

---

## Como Posso Contribuir?

Você pode escolher uma ou mais formas de participar com base em seus interesses e habilidades:

- Fornecer regras de tradução para um idioma de destino
- Fornecer um dicionário terminológico para um idioma de destino
- Melhorar os prompts do sistema
- Fornecer corpora de tradução revisados manualmente
- Melhorar os módulos do pipeline (.NET) e scripts de automação
- Relatar problemas e sugerir melhorias (via Issues)
- Fornecer suporte financeiro para chamadas à API do LLM

Abaixo estão as explicações para os principais cenários de contribuição.

---

## Fornecendo Regras de Tradução, Dicionários Terminológicos e Melhorando Prompts do Sistema

Os modelos de prompt do pipeline estão localizados em src/prompt_templates/, com a seguinte estrutura:

- system_prompt_translate_engine.txt: o prompt do sistema do motor de tradução global (compartilhado por todos os idiomas);
- <código_idioma>/translation_dictionary_<código_idioma>.json: o dicionário terminológico para esse idioma;
- <código_idioma>/translation_schema_<código_idioma>.md: as regras de tradução e restrições de estilo para esse idioma.

Etapas de contribuição:

1. Crie um subdiretório em src/prompt_templates/ para o seu idioma e adicione os arquivos de dicionário e regras de tradução;
2. Se precisar ajustar o comportamento global de tradução, modifique system_prompt_translate_engine.txt (nota: isso afeta todos os idiomas);
3. Teste localmente para confirmar os resultados;
4. Envie um PR.

---

## Fornecendo Corpora Revisados Manualmente

Se você é autor de um mod de tradução e está disposto a fornecer seu corpus de tradução como referência para o LLM, envie uma solicitação via Issue. Você precisa fornecer as seguintes informações:

- O Mod ID do seu mod de tradução e o idioma de destino;
- Uma captura de tela da página de administração do seu mod de tradução para comprovar que você é o autor;
- Uma declaração clara no Issue de que você está disposto a fornecer o corpus de tradução;
- Se houver circunstâncias especiais (licenciamento especial, etc.), explique;
- Certifique-se de que o corpus fornecido seja de alta qualidade.

Com sua autorização, o projeto adicionará seu mod à lista de mods de tradução de referência config/ref_translation_mods.json, e o pipeline sincronizará automaticamente seus textos traduzidos como corpora de referência RAG.

---

## Contribuições para Desenvolvimento do Pipeline e Ferramentas

A automação neste projeto é dividida em duas partes:

**Módulos do pipeline (src/, C# / .NET 10)**: Contém 15 módulos executados sequencialmente, responsáveis pelo fluxo completo desde o download de mods, extração de texto, revisão de conteúdo, cálculo de embeddings, recuperação RAG até a tradução LLM e saída final. Consulte a [referência técnica](../technical_reference/technical_reference_pt-br.md) para detalhes.

**Scripts auxiliares (.github/)**: Usados para automação do GitHub.

Se você deseja:

* Corrigir bugs em módulos do pipeline ou scripts existentes;
* Adicionar novos recursos ou módulos ao pipeline;
* Otimizar o desempenho ou a estrutura do código;
* Melhorar modelos de prompt ou estratégias RAG;

Você pode seguir estas etapas:

1. Faça um fork deste repositório e clone-o localmente;
2. Crie uma nova branch a partir da branch mais recente;
3. Modifique ou adicione arquivos nos diretórios correspondentes:
   - Alterações em módulos do pipeline → src/<nome_do_módulo>/;
   - Alterações em scripts → scripts/;
   - Alterações em modelos de prompt → src/prompt_templates/;
4. Antes de enviar, tente:

   * Manter o estilo de código existente;
   * Adicionar os comentários necessários;
   * Se possível, incluir testes simples ou instruções de uso;
5. Envie as alterações via PR, explicando na descrição:

   * O objetivo das alterações;
   * Os diretórios / módulos / scripts que podem ser afetados;
   * Se envolve alterações que quebram compatibilidade.

---

## Direitos Autorais e Licenciamento

> **Lembrete Amigável:**
> Os termos de direitos autorais e licenciamento são projetados para proteger os direitos e interesses legítimos do projeto, autores, contribuidores e jogadores, e para evitar mal-entendidos decorrentes de "acordos tácitos" ou "pressupostos padrão". Por favor, leia-os atentamente.
> Os direitos autorais e licenciamento são regidos pelo conteúdo do arquivo README.md; esta seção fornece apenas uma descrição mais acessível.

### 1. Princípio Básico: Você retém os direitos autorais, enquanto licencia o projeto para usar seu trabalho

* Você ainda detém os direitos autorais sobre o conteúdo que cria (traduções, imagens, scripts/programas, etc.);
* No entanto, uma vez que este conteúdo seja enviado para este projeto e aceito (mesclado),
  você concorda em licenciar outros para usar este conteúdo sob a licença de código aberto/compartilhada adotada por este projeto.

Isso significa:

* Você **ainda pode** continuar usando e exibindo seu trabalho em outros lugares;
* Mas você **não pode**, após sua contribuição ser mesclada, exigir que este projeto ou outros usuários que obtiveram legalmente o trabalho "revoguem a licença" ou "excluam versões históricas".

### 2. Licenciamento de Textos, Imagens e Conteúdos Similares (CC BY-NC-SA 4.0)

Para o seguinte conteúdo que você enviar:

* Traduções de textos do jogo, revisões e correções;
* Documentação do projeto e textos explicativos;
* Imagens e recursos artísticos criados especificamente para este projeto;

Uma vez aceito e mesclado neste repositório, considera-se que você concorda que:

1. Estes conteúdos são licenciados sob **Atribuição-NãoComercial-CompartilhaIgual 4.0 Internacional**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviado **CC BY-NC-SA 4.0**);
2. O Project Babel e todos os usuários que receberem este conteúdo podem, **em conformidade com os termos da CC BY-NC-SA 4.0**:

   * Compartilhar, copiar e redistribuir este conteúdo;
   * Modificá-lo e criar obras derivadas para fins não comerciais;
3. Você concorda que, na medida permitida pela lei aplicável, esta licença é **não exclusiva, mundial, livre de royalties e irrevogável**;
4. Mesmo que você posteriormente se retire ou pare de participar deste projeto, o projeto poderá continuar usando e redistribuindo o conteúdo relevante que você enviou e que foi mesclado, sob a CC BY-NC-SA 4.0.

> Se você não aceitar os termos de licenciamento acima, não envie contribuições de texto ou imagem para este projeto,
> ou comunique-se previamente com os mantenedores do projeto para confirmar se a colaboração é possível de outra forma.

### 3. Licenciamento de Scripts e Código de Ferramentas (GPL-3.0)

Para o seguinte que você enviar e for aceito:

* Scripts de automação;
* Ferramentas de build/exportação;
* Outro código de programa usado para processar este projeto de tradução;

Na ausência de declarações especiais, considera-se que você concorda que:

1. O código é licenciado sob a **GPL-3.0** (GNU General Public License versão 3);
2. Os mantenedores do projeto podem modificá-lo, mesclá-lo e distribuí-lo dentro do escopo permitido pela GPL-3.0;
3. Você também pode continuar outros projetos baseados no mesmo código, desde que cumpra os termos da GPL-3.0.

Para evitar conflitos de licenciamento, tente:

* Não introduzir código de terceiros **incompatível com a GPL-3.0** sem confirmação prévia;
* Se precisar referenciar bibliotecas de terceiros, indique claramente sua origem e licença no PR e confirme a compatibilidade.

### 4. Obras Anteriores e Direitos Autorais do Jogo Original

Este projeto é um projeto de **tradução não oficial** para mods relacionados ao *Project Zomboid*:

* Os direitos autorais do jogo original e de cada mod pertencem aos seus respectivos autores/editores;
* Este projeto envolve apenas a criação e organização de traduções de texto, ajustes de estilo e alguns recursos de suporte;
* Os contribuidores, ao enviar conteúdo, devem garantir:

  * Não copiar diretamente textos de tradução ou recursos artísticos de terceiros não autorizados;
  * Respeitar os direitos dos autores originais e dos autores de mods, e não realizar redistribuição infratora.

---

## Comunicação e Colaboração

Se você tiver:

* Dúvidas sobre os termos de licenciamento;
* Incerteza sobre se determinado conteúdo pode ser contribuído;
* O desejo de licenciar seu trabalho de uma maneira especial (por exemplo, apenas uso não comercial, mas sem adaptação permitida);

Sinta-se à vontade para entrar em contato com os mantenedores do projeto através de:

* Envio de um Issue para discussão;
* Outros meios de contato públicos dos mantenedores.

Faremos o nosso melhor para encontrar uma solução que equilibre o desenvolvimento saudável do projeto, respeitando os direitos e interesses de todas as partes.

---

## Suporte Financeiro

Durante a operação do projeto, devido à adição de novos mods e atualizações de texto de mods existentes, é necessário chamar continuamente a API do LLM para tradução. Para restringir o comportamento do LLM, além dos textos básicos dos mods, é necessária uma grande quantidade de conteúdo de prompt (incluindo prompts básicos, regras de tradução, tabelas terminológicas, restrições de entrada/saída, resultados de busca semântica, etc.), o que consome muito mais tokens do que os textos originais. Portanto, o projeto precisa de suporte financeiro.

Se você deseja fornecer suporte financeiro, entre em contato com os mantenedores do projeto. Muito obrigado!

---

Mais uma vez, obrigado por sua disposição em contribuir com este projeto!
Cada contribuição que você faz beneficia mais jogadores!
