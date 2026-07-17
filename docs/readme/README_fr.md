# Projet Babel — Projet de traduction automatique par LLM pour le mod Project Zomboid

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Ce projet de traduction est piloté et maintenu par la boîte à outils [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Table des matières

- [Langues cibles supportées par le projet](#langues-cibles-supportées-par-le-projet)
- [Comment installer et utiliser](#comment-installer-et-utiliser)
- [Progression de la traduction](#progression-de-la-traduction)
- [Comment contribuer](#comment-contribuer)
- [Outils et structure des répertoires (pour les développeurs)](#outils-et-structure-des-répertoires-pour-les-développeurs)
  - [Répertoires du projet](#répertoires-du-projet)
  - [Modules du pipeline (dans l'ordre d'exécution)](#modules-du-pipeline-dans-lordre-dexécution)
  - [Pile technique](#pile-technique)
- [Droits d'auteur et licence](#droits-dauteur-et-licence)
  - [1. Textes, images et autres contenus](#1-textes-images-et-autres-contenus)
  - [2. Programmes, scripts et autres contenus de développement](#2-programmes-scripts-et-autres-contenus-de-développement)
- [Remerciements](#remerciements)
- [Programmes tiers](#programmes-tiers)

---

## Langues cibles supportées par le projet

| Langue | Nom local | Code international | Code en jeu | Supporté | Remarques |
|------|------|------|------|------|------|
| Arabe | العربية | `ar` | `AR` | ❌ | Crédit de tokens insuffisant |
| Catalan | català | `ca` | `CA` | ❌ | Crédit de tokens insuffisant |
| Chinois traditionnel | 繁體中文 | `zh-hant` | `CH` | ❌ | Crédit de tokens insuffisant |
| Chinois simplifié | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tchèque | čeština | `cs` | `CS` | ❌ | Crédit de tokens insuffisant |
| Danois | dansk | `da` | `DA` | ❌ | Crédit de tokens insuffisant |
| Allemand | Deutsch | `de` | `DE` | ✅ | |
| Anglais | English | `en` | `EN` | ✅ | |
| Espagnol | español | `es` | `ES` | ❌ | Crédit de tokens insuffisant |
| Finnois | suomi | `fi` | `FI` | ❌ | Crédit de tokens insuffisant |
| Français | français | `fr` | `FR` | ✅ | |
| Hongrois | magyar | `hu` | `HU` | ❌ | Crédit de tokens insuffisant |
| Indonésien | Bahasa Indonesia | `id` | `ID` | ❌ | Crédit de tokens insuffisant |
| Italien | italiano | `it` | `IT` | ❌ | Crédit de tokens insuffisant |
| Japonais | 日本語 | `ja` | `JP` | ✅ | |
| Coréen | 한국어 | `ko` | `KO` | ❌ | Crédit de tokens insuffisant |
| Néerlandais | Nederlands | `nl` | `NL` | ❌ | Crédit de tokens insuffisant |
| Norvégien | norsk | `no` | `NO` | ❌ | Crédit de tokens insuffisant |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Crédit de tokens insuffisant |
| Polonais | polski | `pl` | `PL` | ❌ | Crédit de tokens insuffisant |
| Portugais (Portugal) | português | `pt` | `PT` | ❌ | Crédit de tokens insuffisant |
| Portugais (Brésil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Crédit de tokens insuffisant |
| Roumain | română | `ro` | `RO` | ❌ | Crédit de tokens insuffisant |
| Russe | русский | `ru` | `RU` | ❌ | Crédit de tokens insuffisant |
| Thaï | ภาษาไทย | `th` | `TH` | ❌ | Crédit de tokens insuffisant |
| Turc | Türkçe | `tr` | `TR` | ❌ | Token insuffisant |
| Ukrainien | українська | `uk` | `UA` | ❌ | Token insuffisant |

**Total** : 27 langues prévues | **Supportées** : 5 | **À supporter** : 22

---

## Comment installer et utiliser

Ce guide est destiné aux joueurs souhaitant utiliser directement ce projet de traduction dans le jeu.

1.  Rendez-vous sur notre page Steam Workshop : [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Cliquez sur le bouton « S'abonner ».
3.  Lancez le jeu et activez ce mod de traduction dans le gestionnaire de « Mods » du menu principal.
4.  Les textes de traduction des mods activés après écrasent ceux des mods activés avant. Par conséquent, ce mod de traduction doit être activé après les mods fonctionnels (de préférence en bas).
5.  Profitez du jeu !

---

## Progression de la traduction

**[➡️ Cliquez ici pour voir la progression](./docs/progress/progress_fr.md)**

---

## Comment contribuer

Nous accueillons toute contribution, que ce soit pour corriger une erreur, ajouter une fonctionnalité, rédiger des modèles de prompts, ou fournir des traductions de référence !

L'appel à l'API LLM pour la traduction nécessite de payer des tokens. Pour que le projet puisse fonctionner à long terme, nous espérons votre généreuse aide !

Veuillez lire le [Guide de contribution](./docs/contributing/contributing_fr.md) pour plus de détails.

---

## Outils et structure des répertoires (pour les développeurs)

Cette section s'adresse aux développeurs souhaitant comprendre les principes d'automatisation du projet.

### Répertoires du projet

| Répertoire | Description |
|------|------|
| `src/` | Code source du pipeline de traduction .NET 10, comprenant 15 modules |
| `config/` | Fichiers de configuration du pipeline (paramètres LLM, Steam, RAG, etc.) |
| `data/` | Données d'exécution : métadonnées des mods, embeddings, cache de traduction |
| `translation_ref/` | Données de traduction de référence (par exemple, mods autorisés par As1), fournissant des références de traduction au LLM |
| `base_game_keys/` | Clés de traduction du jeu de base, utilisées pour dédupliquer et éviter d'écraser le texte natif |
| `final_outputs/` | Sorties finales : paquet de mods `project_babel/`, icônes `icons/` et descriptions du Workshop `workshop_descriptions/` |
| `docs/` | Documentation du projet : rapports de progression, guide de contribution, description du pipeline |
| `temp/` | Fichiers temporaires du pipeline (répertoire indépendant à chaque exécution) |
| `src/prompt_templates/` | Modèles de prompts LLM (traduction / révision de contenu) |

### Modules du pipeline (dans l'ordre d'exécution)

| Étape | Module | Fonction |
|------|------|------|
| 1 | `ConfigReader` | Charger la configuration / les clés / la liste des langues |
| 2 | `RepoDataLoader` | Charger les traductions de référence et le cache de traduction |
| 3 | `ModIdCollector` | Collecter les IDs des mods Workshop |
| 4 | `ModInfoFetcher` | Obtenir les métadonnées Steam |
| 5 | `SteamCmdBootstrapper` | Préparer l'environnement d'exécution steamcmd pour la plateforme actuelle |
| 6 | `ModDownloader` | Télécharger les mods via steamcmd |
| 7 | `ContentExtractor` | Analyser les fichiers de traduction des mods → `TranslationEntry` |
| 8 | `ContentChecker` | Examen de sécurité du contenu (drogue/pornographie/violence) |
| 9 | `EmbeddingFetcher` | Calculer les vecteurs d'embedding du texte |
| 10 | `TranslationBatcher` | Créer des lots de traduction indépendants de la langue cible |
| 11 | `RagContextRetriever` | Récupérer le contexte RAG (clé exacte + similarité d'embedding) |
| 12 | `LLMTranslator` | Appeler le LLM pour effectuer la traduction |
| 13 | `ResultWriter` | Écrire dans data/ et translation_ref/ |
| 14 | `FinalOutputWriter` | Générer la sortie finale au format de mod PZ |
| 15 | `ProgressReporter` | Générer le rapport de progression |

### Pile technique

- **Langage**: C# (.NET 10)
- **Plateforme cible**: GitHub Actions Linux x64 runner
- **Tests**: xUnit (Windows x64)
- **LLM**: DeepSeek API (configurable)
- **Embedding**: Vectorisation du texte pour la recherche de similarité RAG
- **Examen de contenu**: Audit de sécurité multi-niveaux piloté par LLM

Détaillé [Référence technique](./docs/technical_reference/technical_reference_fr.md).

---

## Droits d'auteur et licence

Le contenu du texte traduit et les images associées de ce projet de traduction ont été créés ou adaptés par **Project Babel** et les contributeurs à partir des mods de jeu originaux.

© 2025 Project Babel et les auteurs respectifs réservent tous les droits.

### 1. Textes, images et autres contenus

Sauf indication contraire, dans ce dépôt :

- Traduction, révision et relecture des textes en jeu ;
Documentation du projet, traductions de texte dans les mods；
Images et ressources artistiques spécialement créées pour ce projet；

sont toutes sous licence **Attribution - Pas d’Utilisation Commerciale - Partage dans les Mêmes Conditions 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International,简称 **CC BY-NC-SA 4.0**)．

Cela signifie que, sous réserve du respect des conditions suivantes, vous pouvez librement partager et adapter ces contenus :

- **Attribution (BY)** : Mentionner clairement que « ce projet de traduction est basé sur le travail du « Project Babel » et a été modifié », et fournir un lien vers ce dépôt et la page Steam Workshop `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Pas d’Utilisation Commerciale (NC)** : Ne pas utiliser le contenu de ce projet ou ses adaptations à des fins commerciales directes ou indirectes (y compris, mais sans s’y limiter, les packs intégrés payants, les téléchargements payants, le partage de revenus publicitaires, etc.)；
- **Partage dans les Mêmes Conditions (SA)** : Si vous modifiez ou créez une œuvre dérivée basée sur ce projet, vous devez publier votre version modifiée sous **la même licence CC BY-NC-SA 4.0**．

Pour plus d’informations sur cette licence, veuillez consulter :
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.fr>

*Remarques spéciales :*
- *Le contenu du dossier base_game_keys provient du jeu original, les droits d’auteur appartiennent au développeur du jeu ! Ce contenu est utilisé pour éviter que les clés de traduction n’écrasent les clés du jeu (déduplication)*
- *Le contenu du dossier translation_ref est utilisé pour fournir une référence de traduction au LLM, les droits d’auteur appartiennent aux développeurs des mods respectifs !*

### 2. Programmes, scripts et autres contenus de développement

Sauf indication contraire expresse dans le fichier source ou le répertoire, le code du programme utilisé dans ce dépôt pour la création/l’empaquetage/le traitement du contenu de traduction (par exemple le code du répertoire `src/`) est sous licence **GNU General Public License version 3 (GPL-3.0)**．

Les termes complets figurent dans le fichier `LICENSE` à la racine de ce dépôt (GPL-3.0), ou consultez le site officiel de GNU : <https://www.gnu.org/licenses/gpl-3.0.html>．

---

## Remerciements

Ce projet utilise des mods tiers comme textes de référence pour la traduction de la langue cible. Les textes de référence sont envoyés au LLM pour servir de référence de traduction.

| Nom du mod de référence | Auteur | Page du mod |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Page Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Page Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Page Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Un grand merci à ces auteurs !**

---

## Programmes tiers

Ce projet utilise des programmes et bibliothèques tiers. Les droits d’auteur de ces programmes tiers appartiennent à leurs développeurs respectifs.

