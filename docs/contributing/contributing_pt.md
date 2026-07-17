# Guia de Contribuição (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Muito obrigado pela tua disponibilidade em contribuir para o **Project Babel — o projeto de tradução automática com LLM para mods de Project Zomboid**! Quer seja corrigir um bug, adicionar uma funcionalidade, escrever modelos de prompt ou fornecer traduções de referência — cada contribuição conta!

Chamar a API do LLM para tradução tem um custo de tokens. Para que o projeto possa funcionar de forma sustentável a longo prazo, o teu generoso apoio é muito apreciado!

> ⚠️ **Aviso Importante:**
> Antes de enviares qualquer coisa para este repositório, lê e compreende a secção "Direitos de Autor e Licenciamento".
> Uma vez enviado e integrado, considera-se que aceitaste os termos de licenciamento correspondentes.

---

## Antes de Começar

Por favor, lê o `README.md` do projeto para compreender:

- Os objetivos gerais e o estado atual deste projeto;
- Como os jogadores comuns usam este projeto (para os teus próprios testes);
- Detalhes técnicos do projeto.

---

## Como Posso Contribuir?

Podes escolher uma ou mais formas de participar com base nos teus interesses e competências:

- Fornecer regras de tradução para um idioma de destino
- Fornecer um dicionário terminológico para um idioma de destino
- Melhorar os prompts do sistema
- Fornecer corpora de tradução revistos manualmente
- Melhorar os módulos do pipeline (.NET) e scripts de automação
- Reportar problemas e sugerir melhorias (via Issues)
- Fornecer apoio financeiro para chamadas à API do LLM

Abaixo estão as explicações para os principais cenários de contribuição.

---

## Fornecer Regras de Tradução, Dicionários Terminológicos e Melhorar Prompts do Sistema

Os modelos de prompt do pipeline estão localizados em `src/prompt_templates/`, com a seguinte estrutura:

- `system_prompt_translate_engine.txt`: o prompt do sistema do motor de tradução global (partilhado por todos os idiomas);
- `<código_idioma>/translation_dictionary_<código_idioma>.json`: o dicionário terminológico para esse idioma;
- `<código_idioma>/translation_schema_<código_idioma>.md`: as regras de tradução e restrições de estilo para esse idioma.

Etapas de contribuição:

1. Cria um subdiretório em `src/prompt_templates/` para o teu idioma e adiciona os ficheiros de dicionário e regras de tradução;
2. Se precisares de ajustar o comportamento global de tradução, modifica `system_prompt_translate_engine.txt` (nota: isto afeta todos os idiomas);
3. Testa localmente para confirmar os resultados;
4. Envia um PR.

---

## Fornecer Corpora Revistos Manualmente

Se és autor de um mod de tradução e estás disposto a fornecer o teu corpus de tradução como referência para o LLM, envia um pedido via Issue. Precisas de fornecer as seguintes informações:

- O Mod ID do teu mod de tradução e o idioma de destino;
- Uma captura de ecrã da página de administração do teu mod de tradução para comprovar que és o autor;
- Uma declaração clara no Issue de que estás disposto a fornecer o corpus de tradução;
- Se houver circunstâncias especiais (licenciamento especial, etc.), explica;
- Certifica-te de que o corpus fornecido é de alta qualidade.

Com a tua autorização, o projeto adicionará o teu mod à lista de mods de tradução de referência `config/ref_translation_mods.json`, e o pipeline sincronizará automaticamente os teus textos traduzidos como corpora de referência RAG.

---

## Contribuições para Desenvolvimento do Pipeline e Ferramentas

A automação neste projeto está dividida em duas partes:

**Módulos do pipeline (`src/`, C# / .NET 10)**: Contém 15 módulos executados sequencialmente, responsáveis pelo fluxo completo desde o download de mods, extração de texto, revisão de conteúdo, cálculo de embeddings, recuperação RAG até à tradução LLM e saída final. Consulta a [referência técnica](../technical_reference/technical_reference_pt.md) para detalhes.

**Scripts auxiliares (`.github/`)**: Usados para automação do GitHub.

Se desejas:

* Corrigir bugs em módulos do pipeline ou scripts existentes;
* Adicionar novas funcionalidades ou módulos ao pipeline;
* Otimizar o desempenho ou a estrutura do código;
* Melhorar modelos de prompt ou estratégias RAG;

Podes seguir estas etapas:

1. Faz um fork deste repositório e clona-o localmente;
2. Cria um novo ramo a partir do ramo mais recente;
3. Modifica ou adiciona ficheiros nos diretórios correspondentes:
   - Alterações em módulos do pipeline → `src/<nome_do_módulo>/`;
   - Alterações em scripts → `scripts/`;
   - Alterações em modelos de prompt → `src/prompt_templates/`;
4. Antes de enviar, tenta:

   * Manter o estilo de código existente;
   * Adicionar os comentários necessários;
   * Se possível, incluir testes simples ou instruções de uso;
5. Envia as alterações via PR, explicando na descrição:

   * O objetivo das alterações;
   * Os diretórios / módulos / scripts que podem ser afetados;
   * Se envolve alterações que quebram compatibilidade.

---

## Direitos de Autor e Licenciamento

> **Lembrete Amigável:**
> Os termos de direitos de autor e licenciamento são concebidos para proteger os direitos e interesses legítimos do projeto, autores, contribuidores e jogadores, e para evitar mal-entendidos decorrentes de "acordos tácitos" ou "pressupostos padrão". Por favor, lê-os atentamente.
> Os direitos de autor e licenciamento são regidos pelo conteúdo do ficheiro README.md; esta secção fornece apenas uma descrição mais acessível.

### 1. Princípio Básico: Tu reténs os direitos de autor, enquanto licencias o projeto para usar o teu trabalho

* Ainda deténs os direitos de autor sobre o conteúdo que crias (traduções, imagens, scripts/programas, etc.);
* No entanto, uma vez que este conteúdo seja enviado para este projeto e aceite (integrado),
  concordas em licenciar outros para usar este conteúdo sob a licença de código aberto/partilhada adotada por este projeto.

Isto significa:

* **Ainda podes** continuar a usar e exibir o teu trabalho noutros lugares;
* Mas **não podes**, após a tua contribuição ser integrada, exigir que este projeto ou outros utilizadores que obtiveram legalmente o trabalho "revoguem a licença" ou "eliminem versões históricas".

### 2. Licenciamento de Textos, Imagens e Conteúdos Semelhantes (CC BY-NC-SA 4.0)

Para o seguinte conteúdo que enviares:

* Traduções de textos do jogo, revisões e correções;
* Documentação do projeto e textos explicativos;
* Imagens e recursos artísticos criados especificamente para este projeto;

Uma vez aceite e integrado neste repositório, considera-se que concordas que:

1. Estes conteúdos são licenciados sob **Atribuição-NãoComercial-CompartilhaIgual 4.0 Internacional**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abreviado **CC BY-NC-SA 4.0**);
2. O Project Babel e todos os utilizadores que receberem este conteúdo podem, **em conformidade com os termos da CC BY-NC-SA 4.0**:

   * Partilhar, copiar e redistribuir este conteúdo;
   * Modificá-lo e criar obras derivadas para fins não comerciais;
3. Concordas que, na medida permitida pela lei aplicável, esta licença é **não exclusiva, mundial, livre de royalties e irrevogável**;
4. Mesmo que posteriormente te retires ou deixes de participar neste projeto, o projeto poderá continuar a usar e redistribuir o conteúdo relevante que enviaste e que foi integrado, sob a CC BY-NC-SA 4.0.

> Se não aceitares os termos de licenciamento acima, não envies contribuições de texto ou imagem para este projeto,
> ou comunica previamente com os mantenedores do projeto para confirmar se a colaboração é possível de outra forma.

### 3. Licenciamento de Scripts e Código de Ferramentas (GPL-3.0)

Para o seguinte que enviares e for aceite:

* Scripts de automação;
* Ferramentas de build/exportação;
* Outro código de programa usado para processar este projeto de tradução;

Na ausência de declarações especiais, considera-se que concordas que:

1. O código é licenciado sob a **GPL-3.0** (GNU General Public License versão 3);
2. Os mantenedores do projeto podem modificá-lo, integrá-lo e distribuí-lo dentro do âmbito permitido pela GPL-3.0;
3. Tu também podes continuar outros projetos baseados no mesmo código, desde que cumpras os termos da GPL-3.0.

Para evitar conflitos de licenciamento, tenta:

* Não introduzir código de terceiros **incompatível com a GPL-3.0** sem confirmação prévia;
* Se precisares de referenciar bibliotecas de terceiros, indica claramente a sua origem e licença no PR e confirma a compatibilidade.

### 4. Obras Anteriores e Direitos de Autor do Jogo Original

Este projeto é um projeto de **tradução não oficial** para mods relacionados com o *Project Zomboid*:

* Os direitos de autor do jogo original e de cada mod pertencem aos seus respetivos autores/editores;
* Este projeto envolve apenas a criação e organização de traduções de texto, ajustes de estilo e alguns recursos de suporte;
* Os contribuidores, ao enviar conteúdo, devem garantir:

  * Não copiar diretamente textos de tradução ou recursos artísticos de terceiros não autorizados;
  * Respeitar os direitos dos autores originais e dos autores de mods, e não realizar redistribuição infratora.

---

## Comunicação e Colaboração

Se tiveres:

* Dúvidas sobre os termos de licenciamento;
* Incerteza sobre se determinado conteúdo pode ser contribuído;
* O desejo de licenciar o teu trabalho de uma maneira especial (por exemplo, apenas uso não comercial, mas sem adaptação permitida);

Sente-te à vontade para contactar os mantenedores do projeto através de:

* Envio de um Issue para discussão;
* Outros meios de contacto públicos dos mantenedores.

Faremos o nosso melhor para encontrar uma solução que equilibre o desenvolvimento saudável do projeto, respeitando os direitos e interesses de todas as partes.

---

## Apoio Financeiro

Durante a operação do projeto, devido à adição de novos mods e atualizações de texto de mods existentes, é necessário chamar continuamente a API do LLM para tradução. Para restringir o comportamento do LLM, além dos textos básicos dos mods, é necessária uma grande quantidade de conteúdo de prompt (incluindo prompts básicos, regras de tradução, tabelas terminológicas, restrições de entrada/saída, resultados de pesquisa semântica, etc.), o que consome muito mais tokens do que os textos originais. Portanto, o projeto precisa de apoio financeiro.

Se desejas fornecer apoio financeiro, contacta os mantenedores do projeto. Muito obrigado!

---

Mais uma vez, obrigado pela tua disponibilidade em contribuir para este projeto!
Cada contribuição que fazes beneficia mais jogadores!
