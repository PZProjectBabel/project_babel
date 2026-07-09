# Project Babel — Traduction automatique de mods PZ par LLM

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Autres langues</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Ce projet de traduction est piloté et maintenu par l'outil [Project Babel](https://github.com/PZProjectBabel/project_babel).*

---

## Table des matières

- [Langues cibles prises en charge](#langues-cibles-prises-en-charge)
- [Installation et utilisation](#installation-et-utilisation)
- [Progression de la traduction](#progression-de-la-traduction)
- [Contribuer](#contribuer)
- [Outils et structure des répertoires (pour les développeurs)](#outils-et-structure-des-répertoires-pour-les-développeurs)
- [Droits d'auteur et licence](#droits-dauteur-et-licence)
- [Remerciements](#remerciements)
- [Logiciels tiers](#logiciels-tiers)

---

## Langues cibles prises en charge

| Langue | Nom local | Code ISO | Code en jeu | Prise en charge | Remarque |
|------|------|------|------|------|------|
| Arabe | العربية | `ar` | `AR` | ❌ | Crédits de tokens insuffisants |
| Catalan | català | `ca` | `CA` | ❌ | Crédits de tokens insuffisants |
| Chinois traditionnel | 繁體中文 | `zh-hant` | `CH` | ❌ | Crédits de tokens insuffisants |
| Chinois simplifié | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tchèque | čeština | `cs` | `CS` | ❌ | Crédits de tokens insuffisants |
| Danois | dansk | `da` | `DA` | ❌ | Crédits de tokens insuffisants |
| Allemand | Deutsch | `de` | `DE` | ✅ | |
| Anglais | English | `en` | `EN` | ✅ | |
| Espagnol | español | `es` | `ES` | ❌ | Crédits de tokens insuffisants |
| Finnois | suomi | `fi` | `FI` | ❌ | Crédits de tokens insuffisants |
| Français | français | `fr` | `FR` | ✅ | |
| Hongrois | magyar | `hu` | `HU` | ❌ | Crédits de tokens insuffisants |
| Indonésien | Bahasa Indonesia | `id` | `ID` | ❌ | Crédits de tokens insuffisants |
| Italien | italiano | `it` | `IT` | ❌ | Crédits de tokens insuffisants |
| Japonais | 日本語 | `ja` | `JP` | ✅ | |
| Coréen | 한국어 | `ko` | `KO` | ❌ | Crédits de tokens insuffisants |
| Néerlandais | Nederlands | `nl` | `NL` | ❌ | Crédits de tokens insuffisants |
| Norvégien | norsk | `no` | `NO` | ❌ | Crédits de tokens insuffisants |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Crédits de tokens insuffisants |
| Polonais | polski | `pl` | `PL` | ❌ | Crédits de tokens insuffisants |
| Portugais (Portugal) | português | `pt` | `PT` | ❌ | Crédits de tokens insuffisants |
| Portugais (Brésil) | português do Brasil | `pt-br` | `PTBR` | ❌ | Crédits de tokens insuffisants |
| Roumain | română | `ro` | `RO` | ❌ | Crédits de tokens insuffisants |
| Russe | русский | `ru` | `RU` | ❌ | Crédits de tokens insuffisants |
| Thaï | ภาษาไทย | `th` | `TH` | ❌ | Crédits de tokens insuffisants |
| Turc | Türkçe | `tr` | `TR` | ❌ | Crédits de tokens insuffisants |
| Ukrainien | українська | `uk` | `UA` | ❌ | Crédits de tokens insuffisants |

**Total** : 27 langues planifiées | **Prises en charge** : 5 | **En attente** : 22

---

## Installation et utilisation

Guide pour les joueurs souhaitant utiliser le pack de traduction en jeu.

1. Allez sur la page Steam Workshop : [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Cliquez sur « S'abonner ».
3. Lancez le jeu, activez ce mod de traduction dans le menu Mods.
4. Le texte des mods chargés après écrase celui des précédents, ce mod de traduction doit donc être chargé après les mods de jeu.
5. Profitez !

---

## Progression de la traduction

[➡️ Progression de la traduction](../progress/progress_fr.md)

---

## Contribuer

Nous acceptons les contributions : corrections de traduction, nouvelles fonctionnalités, modèles de prompts ou traductions de référence !

L'appel aux API LLM pour la traduction génère des coûts de tokens. Votre soutien aide le projet à fonctionner durablement !

Lisez le [Guide de Contribution](../contributing/contributing_fr.md) pour plus de détails.

---

## Outils et structure des répertoires (pour les développeurs)

Cette section s'adresse aux développeurs souhaitant comprendre l'automatisation du projet.

### Répertoires du projet

| Répertoire | Description |
|------|------|
| `src/` | Code source du pipeline de traduction .NET 10, 15 modules |
| `config/` | Configuration du pipeline (LLM, Steam, paramètres RAG, etc.) |
| `data/` | Données d'exécution : métadonnées des mods, embeddings, cache de traduction |
| `translation_ref/` | Traductions de référence comme contexte LLM |
| `base_game_keys/` | Clés de traduction du jeu de base pour la déduplication |
| `final_outputs/` | Sortie finale au format mod PZ |
| `docs/` | Documentation du projet : progression, contribution, pipeline |
| `temp/` | Fichiers temporaires du pipeline |
| `src/prompt_templates/` | Modèles de prompts LLM |

### Modules du pipeline (ordre d'exécution)

| Étape | Module | Fonction |
|------|------|------|
| 1 | `ConfigReader` | Charger configuration/secrets/langues |
| 2 | `RepoDataLoader` | Charger les références et le cache de traduction |
| 3 | `ModIdCollector` | Collecter les IDs de mods Workshop |
| 4 | `ModInfoFetcher` | Récupérer les métadonnées Steam |
| 5 | `ModDownloader` | Télécharger les mods via steamcmd |
| 6 | `ContentExtractor` | Analyser les fichiers de traduction → `TranslationEntry` |
| 7 | `ContentChecker` | Vérification de sécurité du contenu |
| 8 | `EmbeddingFetcher` | Calculer les vecteurs d'embedding texte |
| 9 | `TranslationBatcher` | Créer des lots de traduction |
| 10 | `RagContextRetriever` | Récupérer les contextes RAG |
| 11 | `LLMTranslator` | Exécuter la traduction LLM |
| 12 | `ResultWriter` | Écrire dans data/ et translation_ref/ |
| 13 | `FinalOutputWriter` | Générer la sortie finale au format mod PZ |
| 14 | `ProgressReporter` | Générer les rapports de progression |

### Stack technique

- **Langage** : C# (.NET 10)
- **Plateforme cible** : GitHub Actions Linux x64 runner
- **Tests** : xUnit (Windows x64)
- **LLM** : DeepSeek API (configurable)
- **Embedding** : Vectorisation de texte pour recherche de similarité RAG
- **Vérification de contenu** : Audit de sécurité multi-niveaux piloté par LLM

Documentation technique détaillée : [Pipeline TranslationEntry](../pipeline/translation_entry_pipeline_fr.md)

---

## Droits d'auteur et licence

© 2025 Project Babel et auteurs. Tous droits réservés.

### Contenu (textes, images)

Sous licence **CC BY-NC-SA 4.0**.

- **Attribution** : Mentionner les modifications basées sur « Project Babel », avec liens repo et Workshop
- **Non commercial** : Usage commercial interdit
- **Partage dans les mêmes conditions** : Les modifications doivent être publiées sous la même licence

### Code

Le code sous `src/` est sous licence **GPL-3.0**.

---

## Remerciements

| Mod de référence | Auteur | Page |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Un grand merci aux auteurs ci-dessus !**

---

## Logiciels tiers

Ce projet utilise des programmes et bibliothèques tiers, dont les droits d'auteur appartiennent à leurs développeurs respectifs.
