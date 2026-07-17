# Guide de contribution (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Table des matières

- [1. Avant de commencer](#1-avant-de-commencer)
- [2. Comment puis-je contribuer ?](#2-comment-puis-je-contribuer)
- [3. Fournir des règles de traduction, un dictionnaire terminologique, améliorer les prompts du système](#3-fournir-des-règles-de-traduction-un-dictionnaire-terminologique-améliorer-les-prompts-du-système)
- [4. Fournir des corpus de relecture](#4-fournir-des-corpus-de-relecture)
- [5. Contributions à la pipeline et aux outils](#5-contributions-à-la-pipeline-et-aux-outils)
- [6. Droits d'auteur et licence](#6-droits-dauteur-et-licence)
  - [6.1 Principes de base : vous conservez vos droits d'auteur, tout en accordant une licence au projet](#61-principes-de-base-vous-conservez-vos-droits-dauteur-tout-en-accordant-une-licence-au-projet)
  - [6.2 Licence pour les textes, images et autres contenus (CC BY-NC-SA 4.0)](#62-licence-pour-les-textes-images-et-autres-contenus-cc-by-nc-sa-40)
  - [6.3 Licence pour le code de script et d'outils (GPL-3.0)](#63-licence-pour-le-code-de-script-et-doutils-gpl-30)
  - [6.4 Droits d'auteur des œuvres en amont et du jeu original](#64-droits-dauteur-des-œuvres-en-amont-et-du-jeu-original)
- [7. Communication et collaboration](#7-communication-et-collaboration)
- [8. Soutien financier](#8-soutien-financier)

---

Merci infiniment de vouloir contribuer au **Projet Babel - Projet de traduction automatique LLM du mod 《僵尸毁灭工程》** ! Que ce soit pour corriger une erreur, ajouter une fonctionnalité, rédiger un modèle de prompt, ou fournir une traduction de référence !

L'utilisation de l'API LLM pour la traduction nécessite de payer des tokens. Pour assurer le fonctionnement stable à long terme du projet, nous espérons votre généreux soutien !

> ⚠️ **Avertissement important :**
> Avant de soumettre quoi que ce soit à ce dépôt, veuillez lire et comprendre la section « Convention de droits d'auteur et de licence ».
> Une fois soumis et fusionné, cela sera considéré comme votre acceptation des termes de licence correspondants.

---

## 1. Avant de commencer

Veuillez d'abord lire le `README.md` du projet pour comprendre :
- L'objectif global et l'état actuel du projet ;
- Comment les joueurs ordinaires peuvent utiliser ce projet (pour vos propres tests) ;
- Les détails techniques du projet.

---

## 2. Comment puis-je contribuer ?

Vous pouvez choisir une ou plusieurs façons de participer selon vos intérêts et compétences :

- Fournir des règles de traduction pour la langue cible
- Fournir un dictionnaire terminologique de traduction pour la langue cible
- Améliorer les prompts du système
- Fournir des corpus de traduction relus manuellement
- Améliorer le module pipeline (.NET) et les scripts d'automatisation
- Signaler des problèmes, proposer des améliorations (dans les Issues)
- Fournir un soutien financier pour l'appel de l'API LLM

Ci-dessous, quelques explications sur les principales contributions.

---

## 3. Fournir des règles de traduction, un dictionnaire terminologique, améliorer les prompts du système

Les modèles de prompt du pipeline se trouvent dans `src/prompt_templates/`, avec la structure suivante :

- `system_prompt_translate_engine.txt` : Prompt système du moteur de traduction global (partagé par toutes les langues) ;
- `<code_langue>/translation_dictionary_<code_langue>.json` : Dictionnaire terminologique pour cette langue ;
- `<code_langue>/translation_schema_<code_langue>.md` : Règles de traduction et contraintes de style pour cette langue.

Étapes pour contribuer :

1. Créez un sous-répertoire pour votre langue dans `src/prompt_templates/`, ajoutez le dictionnaire terminologique et le fichier de règles de traduction ;
2. Si vous devez ajuster le comportement de traduction global, modifiez `system_prompt_translate_engine.txt` (cela affecte toutes les langues) ;
3. Testez localement pour confirmer l'effet.
4. Soumettez une PR.

---

## 4. Fournir des corpus de relecture

Si vous êtes un créateur de mod de traduction et souhaitez fournir votre corpus de traduction comme référence pour la traduction LLM, veuillez soumettre une demande dans l'Issue. Vous devez fournir les informations suivantes :

- L'ID de votre mod de traduction ainsi que la langue cible de la traduction ;
- Une capture d'écran de la page d'administration de votre mod de traduction pour prouver que vous en êtes l'auteur ;
- Indiquer clairement dans l'Issue que vous êtes disposé à fournir le corpus de traduction ;
- En cas de circonstances particulières (licence spéciale, etc.), veuillez le préciser ;
- Assurez-vous que le corpus fourni est de haute qualité.

Avec votre autorisation, le projet ajoutera votre mod à la liste des mods de référence dans `config/ref_translation_mods.json`, et le pipeline synchronisera automatiquement votre texte de traduction comme corpus de référence RAG.

---

## 5. Contributions à la pipeline et aux outils

L'automatisation de ce projet est divisée en deux parties :

**Module pipeline (`src/`, C# / .NET 10)** : contient 15 modules exécutés séquentiellement, responsables du processus complet depuis l'initialisation SteamCMD, le téléchargement du mod, l'extraction de texte, la révision de contenu, le calcul d'Embedding, la recherche RAG jusqu'à la traduction LLM et la sortie finale. Voir [Référence technique](../technical_reference/technical_reference_fr.md).

**Scripts auxiliaires (.github/)** : utilisés pour l'automatisation de GitHub.

Si vous souhaitez :

* Corriger des bugs dans les modules ou scripts de pipeline existants ;
* Ajouter de nouvelles fonctionnalités ou de nouveaux modules à la pipeline ;
* Optimiser les performances ou la structure du code ;
* Améliorer les templates de prompt ou la stratégie RAG ;

Vous pouvez suivre les étapes ci-dessous :

1. Fork ce dépôt et clonez-le en local ;
2. Créez une nouvelle branche basée sur la branche la plus récente ;
3. Modifiez ou ajoutez des fichiers dans les répertoires correspondants :
- Modification de module pipeline → `src/<nom_du_module>/` ;
- Modification de script → `scripts/` ;
- Modification de template de prompt → `src/prompt_templates/` ;
4. Avant de soumettre, essayez autant que possible :

* Conserver le style de code original ;
* Ajouter les commentaires nécessaires ;
* Si possible, joindre des tests simples ou des instructions d'utilisation ;
5. Soumettez vos modifications via une PR et expliquez dans la description :

* Le but de la modification ;
* Les répertoires / modules / scripts potentiellement impactés ;
* Si cela implique des changements cassants.

---

## 6. Droits d'auteur et licence

> **Remarque importante :**
> Les clauses de droits d'auteur et de licence visent à protéger les droits légitimes du projet, des auteurs, des contributeurs et des joueurs, afin d'éviter les malentendus dus à des « accords tacites » ou « par défaut ». Veuillez les lire attentivement.
> Les droits d'auteur et la licence sont régis par le contenu du fichier README.md ; cette section ne fournit qu'une description plus accessible.

### 6.1 Principes de base : vous conservez vos droits d'auteur, tout en accordant une licence au projet

* Vous conservez les droits d'auteur sur le contenu que vous créez (traductions, images, scripts/programmes, etc.) ;
* Mais après avoir soumis ce contenu à ce projet et qu'il a été accepté (fusionné), vous acceptez d'accorder une licence à autrui pour utiliser ce contenu conformément à la licence open source/partagée adoptée par ce projet.

Cela signifie :

* Vous **pouvez toujours** continuer à utiliser et à exposer vos œuvres ailleurs ;
* Mais vous **ne pouvez pas** exiger que ce projet ou d'autres utilisateurs ayant légalement obtenu vos œuvres « retirent l'autorisation » ou « suppriment les versions antérieures » après la fusion de votre contribution.

### 6.2 Licence pour les textes, images et autres contenus (CC BY-NC-SA 4.0)

Pour le contenu suivant que vous soumettez :

* Traductions de textes de jeu, révisions et relectures ;
* Documentation du projet, textes explicatifs ;
* Images, ressources artistiques créées spécifiquement pour ce projet ;

Une fois acceptées et fusionnées dans ce dépôt, vous êtes réputé avoir accepté :

1. Ces contenus sont concédés sous licence **Attribution - Pas d'Utilisation Commerciale - Partage dans les Mêmes Conditions 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abrégé **CC BY-NC-SA 4.0**) ;
2. Project Babel et tous les utilisateurs ayant obtenu ce contenu peuvent, sous réserve de **respecter les termes de CC BY-NC-SA 4.0** :
* Partager, copier, redistribuer ces contenus ;
* Les modifier et les recréer pour des usages non commerciaux ;
3. Vous acceptez que, dans les limites autorisées par la loi applicable, cette licence soit **non exclusive, mondiale, libre de redevances et irrévocable** ;
4. Même si vous vous retirez ou cessez de participer à ce projet à l'avenir, ce projet peut continuer à utiliser et à redistribuer le contenu que vous avez soumis et qui a été fusionné, conformément à CC BY-NC-SA 4.0.

> Si vous n'acceptez pas les modalités de licence ci-dessus, veuillez ne pas soumettre de contributions textuelles ou graphiques à ce projet,
> ou discuter au préalable avec les mainteneurs du projet pour confirmer si une collaboration sous d'autres formes est possible.

### 6.3 Licence pour le code de script et d'outils (GPL-3.0)

Pour ce que vous soumettez et qui est accepté :

* Scripts d'automatisation ;
* Outils de construction/exportation ;
* Autres codes de programme pour traiter ce projet de traduction ;

En l'absence de déclaration particulière, il est considéré que vous acceptez :

1. Le code est concédé sous licence **GPL-3.0** (GNU General Public License version 3) ;
2. Les mainteneurs du projet peuvent le modifier, le fusionner et le distribuer dans les limites autorisées par GPL-3.0 ;
3. Vous pouvez également poursuivre d'autres projets basés sur le même code, à condition de respecter les termes de GPL-3.0.

Pour éviter les conflits de licence, veuillez essayer :

* Ne pas introduire de code tiers **incompatible avec GPL-3.0** sans vérification ;
* Si vous devez utiliser une bibliothèque tierce, veuillez clairement indiquer sa source et sa licence dans la PR, et confirmer sa compatibilité.

### 6.4 Droits d'auteur des œuvres en amont et du jeu original

Ce projet est un projet de **traduction non officielle** des mods liés à *Project Zomboid* :

* Les droits d'auteur du jeu original et de chaque mod appartiennent à leurs auteurs/éditeurs respectifs ;
* Ce projet ne crée et n'organise que les traductions, les ajustements de relecture et certaines ressources associées ;
* Les contributeurs doivent s'assurer, lors de la soumission de contenu :
* De ne pas copier directement des textes de traduction ou des ressources graphiques non autorisées de tiers ;
* De respecter les droits des auteurs originaux et des auteurs de mods, et de ne pas effectuer de republication contrefaisante.

---

## 7. Communication et collaboration

Si vous avez des questions sur :

* Les conditions de licence ;
* L'incertitude quant à la possibilité de contribuer un certain contenu ;
* La volonté d'accorder une licence spéciale pour votre œuvre (par exemple, uniquement à des fins non commerciales sans autorisation de modification) ;

Bienvenue pour contacter les mainteneurs du projet par les moyens suivants :

* Soumettre une Issue pour discussion ;
* Autres moyens de contact fournis publiquement par les mainteneurs.

Nous ferons de notre mieux pour trouver une solution qui équilibre le développement sain du projet, dans le respect des droits de toutes les parties.

---

## 8. Soutien financier

Pendant le fonctionnement du projet, en raison de l'ajout de nouveaux mods ou de la mise à jour du contenu textuel des anciens mods, il est nécessaire d'appeler en continu l'API LLM pour la traduction. Et pour contraindre le comportement du LLM, en plus des textes de base des mods, il est également nécessaire de fournir un grand nombre de contenus de prompts (y compris les prompts de base, les règles de traduction, les glossaires, les contraintes d'entrée/sortie, les résultats de requêtes sémantiques, etc.), qui consomment bien plus de tokens que le texte original. Par conséquent, le projet a besoin d'un soutien financier.

Si vous souhaitez apporter un soutien financier, veuillez contacter les mainteneurs du projet. Merci beaucoup !

---

Merci encore d'avoir contribué à ce projet !
Chacune de vos contributions profitera à davantage de joueurs !
