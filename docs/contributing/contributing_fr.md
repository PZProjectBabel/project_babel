# Guide de contribution (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Merci de votre volonté de contribuer au **Project Babel — le projet de traduction automatique par LLM pour les mods de Project Zomboid** ! Qu'il s'agisse de corriger un bug, d'ajouter une fonctionnalité, de rédiger des modèles de prompt ou de fournir des traductions de référence, chaque contribution compte !

L'appel à l'API LLM pour la traduction a un coût en tokens. Pour que le projet puisse fonctionner durablement à long terme, votre généreux soutien est grandement apprécié !

> ⚠️ **Avis important :**
> Avant de soumettre quoi que ce soit à ce dépôt, veuillez lire et comprendre la section « Droits d'auteur et licences ».
> Une fois soumis et fusionné, vous êtes réputé avoir accepté les conditions de licence correspondantes.

---

## Avant de commencer

Veuillez lire le README.md du projet pour comprendre :

- Les objectifs globaux et l'état actuel de ce projet ;
- Comment les joueurs ordinaires utilisent ce projet (pour vos propres tests) ;
- Les détails techniques du projet.

---

## Comment puis-je contribuer ?

Vous pouvez choisir une ou plusieurs façons de participer selon vos intérêts et compétences :

- Fournir des règles de traduction pour une langue cible
- Fournir un dictionnaire terminologique pour une langue cible
- Améliorer les prompts système
- Fournir des corpus de traduction corrigés manuellement
- Améliorer les modules du pipeline (.NET) et les scripts d'automatisation
- Signaler des problèmes et suggérer des améliorations (via les Issues)
- Fournir un soutien financier pour les appels à l'API LLM

Voici des explications pour les principaux scénarios de contribution.

---

## Fournir des règles de traduction, des dictionnaires terminologiques et améliorer les prompts système

Les modèles de prompt du pipeline se trouvent dans src/prompt_templates/, avec la structure suivante :

- system_prompt_translate_engine.txt : le prompt système global du moteur de traduction (commun à toutes les langues) ;
- <code_langue>/translation_dictionary_<code_langue>.json : le dictionnaire terminologique pour cette langue ;
- <code_langue>/translation_schema_<code_langue>.md : les règles de traduction et contraintes stylistiques pour cette langue.

Étapes de contribution :

1. Créez un sous-répertoire sous src/prompt_templates/ pour votre langue et ajoutez les fichiers de dictionnaire et de règles ;
2. Si vous devez ajuster le comportement global de traduction, modifiez system_prompt_translate_engine.txt (attention : cela affecte toutes les langues) ;
3. Testez localement pour confirmer les résultats ;
4. Soumettez une PR.

---

## Fournir des corpus corrigés manuellement

Si vous êtes auteur d'un mod de traduction et souhaitez fournir votre corpus de traduction comme référence pour le LLM, veuillez soumettre une demande via une Issue. Vous devez fournir les informations suivantes :

- Le Mod ID de votre mod de traduction et la langue cible ;
- Une capture d'écran de la page d'administration de votre mod de traduction pour prouver que vous en êtes l'auteur ;
- Une déclaration claire dans l'Issue indiquant que vous acceptez de fournir le corpus de traduction ;
- En cas de circonstances particulières (licence spéciale, etc.), veuillez les expliquer ;
- Veuillez vous assurer que le corpus fourni est de haute qualité.

Avec votre autorisation, le projet ajoutera votre mod à la liste des mods de traduction de référence config/ref_translation_mods.json, et le pipeline synchronisera automatiquement vos textes traduits comme corpus de référence RAG.

---

## Contributions au développement du pipeline et des outils

L'automatisation de ce projet se divise en deux parties :

**Modules du pipeline (src/, C# / .NET 10)** : Contient 15 modules exécutés séquentiellement, couvrant l'ensemble du flux de travail, du téléchargement des mods, de l'extraction de texte, de la vérification de contenu, du calcul d'embeddings, de la recherche RAG jusqu'à la traduction LLM et la sortie finale. Voir la [référence technique](../technical_reference/technical_reference_fr.md) pour plus de détails.

**Scripts auxiliaires (.github/)** : Utilisés pour l'automatisation GitHub.

Si vous souhaitez :

* Corriger des bugs dans les modules du pipeline ou les scripts existants ;
* Ajouter de nouvelles fonctionnalités ou de nouveaux modules au pipeline ;
* Optimiser les performances ou la structure du code ;
* Améliorer les modèles de prompt ou les stratégies RAG ;

Vous pouvez suivre ces étapes :

1. Forkez ce dépôt et clonez-le localement ;
2. Créez une nouvelle branche à partir de la dernière branche ;
3. Modifiez ou ajoutez des fichiers dans les répertoires correspondants :
   - Modifications des modules du pipeline → src/<nom_du_module>/ ;
   - Modifications des scripts → scripts/ ;
   - Modifications des modèles de prompt → src/prompt_templates/ ;
4. Avant de soumettre, veuillez dans la mesure du possible :

   * Conserver le style de code existant ;
   * Ajouter les commentaires nécessaires ;
   * Si possible, inclure des tests simples ou des instructions d'utilisation ;
5. Soumettez les modifications via PR, en expliquant dans la description :

   * L'objectif des modifications ;
   * Les répertoires / modules / scripts potentiellement affectés ;
   * S'il s'agit de modifications avec rupture de compatibilité.

---

## Droits d'auteur et licences

> **Rappel amical :**
> Les conditions de droits d'auteur et de licence visent à protéger les droits et intérêts légitimes du projet, des auteurs, des contributeurs et des joueurs, et à éviter les malentendus résultant d'« accords tacites » ou de « présupposés par défaut ». Veuillez les lire attentivement.
> Les droits d'auteur et les licences sont régis par le contenu du fichier README.md ; cette section ne fournit qu'une description plus accessible.

### 1. Principe de base : Vous conservez les droits d'auteur, tout en accordant une licence d'utilisation au projet

* Vous conservez les droits d'auteur sur le contenu que vous créez (traductions, images, scripts/programmes, etc.) ;
* Cependant, une fois ce contenu soumis à ce projet et accepté (fusionné),
  vous acceptez de concéder aux autres le droit d'utiliser ce contenu selon la licence open-source/partagée adoptée par ce projet.

Cela signifie :

* Vous **pouvez toujours** continuer à utiliser et afficher votre travail ailleurs ;
* Mais vous **ne pouvez pas**, après la fusion de votre contribution, exiger que ce projet ou d'autres utilisateurs ayant légalement obtenu l'œuvre « révoquent la licence » ou « suppriment les versions historiques ».

### 2. Licence des textes, images et contenus similaires (CC BY-NC-SA 4.0)

Pour le contenu suivant que vous soumettez :

* Traductions de textes de jeu, révisions et corrections ;
* Documentation du projet et textes explicatifs ;
* Images et ressources artistiques créées spécifiquement pour ce projet ;

Une fois accepté et fusionné dans ce dépôt, vous êtes réputé accepter que :

1. Ces contenus sont sous licence **Attribution – Pas d'Utilisation Commerciale – Partage dans les Mêmes Conditions 4.0 International**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, abrégé **CC BY-NC-SA 4.0**) ;
2. Project Babel et tous les utilisateurs recevant ce contenu peuvent, **dans le respect des conditions CC BY-NC-SA 4.0** :

   * Partager, copier et redistribuer ce contenu ;
   * Le modifier et créer des œuvres dérivées à des fins non commerciales ;
3. Vous acceptez que cette licence soit **non exclusive, mondiale, libre de redevance et irrévocable** dans les limites permises par la loi applicable ;
4. Même si vous vous retirez ou cessez de participer à ce projet par la suite, le projet peut continuer à utiliser et redistribuer le contenu pertinent que vous avez soumis et qui a été fusionné, conformément à CC BY-NC-SA 4.0.

> Si vous n'acceptez pas les conditions de licence ci-dessus, veuillez ne pas soumettre de contributions textuelles ou d'images à ce projet,
> ou communiquez au préalable avec les responsables du projet pour confirmer si une collaboration est possible sous d'autres modalités.

### 3. Licence des scripts et du code des outils (GPL-3.0)

Pour ce qui suit, que vous soumettez et qui est accepté :

* Scripts d'automatisation ;
* Outils de construction/exportation ;
* Autre code de programme utilisé pour traiter ce projet de traduction ;

En l'absence de déclaration particulière, vous êtes réputé accepter que :

1. Le code est sous licence **GPL-3.0** (GNU General Public License version 3) ;
2. Les responsables du projet peuvent le modifier, le fusionner et le distribuer dans les limites permises par la GPL-3.0 ;
3. Vous pouvez également poursuivre d'autres projets basés sur le même code, tant que vous respectez les conditions de la GPL-3.0.

Pour éviter les conflits de licence, veuillez dans la mesure du possible :

* Ne pas introduire de code tiers **incompatible avec la GPL-3.0** sans confirmation préalable ;
* Si vous devez référencer des bibliothèques tierces, indiquez clairement leur source et leur licence dans la PR, et confirmez leur compatibilité.

### 4. Œuvres amont et droits d'auteur du jeu original

Ce projet est un projet de **traduction non officielle** pour les mods liés à *Project Zomboid* :

* Les droits d'auteur du jeu original et de chaque mod appartiennent à leurs auteurs/éditeurs respectifs ;
* Ce projet se limite à la création et à l'organisation de traductions textuelles, d'ajustements stylistiques et de certaines ressources d'accompagnement ;
* Les contributeurs, en soumettant du contenu, doivent s'assurer :

  * De ne pas copier directement des textes de traduction ou des ressources artistiques de tiers non autorisés ;
  * De respecter les droits des auteurs originaux et des auteurs de mods, et de ne pas effectuer de rediffusion contrefaisante.

---

## Communication et collaboration

Si vous avez :

* Des questions sur les conditions de licence ;
* Des doutes quant à la possibilité de contribuer un certain contenu ;
* Le souhait de licencier votre travail d'une manière particulière (par exemple, usage non commercial uniquement mais sans adaptation autorisée) ;

N'hésitez pas à contacter les responsables du projet via :

* La soumission d'une Issue pour discussion ;
* D'autres moyens de contact publics des responsables.

Nous ferons de notre mieux pour trouver une solution qui concilie le développement sain du projet et le respect des droits et intérêts de toutes les parties.

---

## Soutien financier

Dans le fonctionnement du projet, en raison de l'ajout de nouveaux mods et des mises à jour textuelles des mods existants, l'API LLM doit être appelée en continu pour la traduction. Pour contraindre le comportement du LLM, en plus des textes de base des mods, une grande quantité de contenu de prompt est nécessaire (prompts de base, règles de traduction, tables terminologiques, contraintes d'entrée/sortie, résultats de recherche sémantique, etc.), ce qui consomme bien plus de tokens que les textes originaux. Par conséquent, le projet a besoin d'un soutien financier.

Si vous souhaitez fournir un soutien financier, veuillez contacter les responsables du projet. Merci beaucoup !

---

Merci encore pour votre volonté de contribuer à ce projet !
Chacune de vos contributions profite à davantage de joueurs !
