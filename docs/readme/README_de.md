# Project Babel — Automatische LLM-Übersetzung für PZ-Mods

> [简体中文](../../README.md) | [English](README_en.md) <details><summary>Andere Sprachen</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Dieses Übersetzungsprojekt wird vom [Project Babel](https://github.com/PZProjectBabel/project_babel)-Toolset betrieben und gepflegt.*

---

## Inhaltsverzeichnis

- [Unterstützte Zielsprachen](#unterstützte-zielsprachen)
- [Installation & Nutzung](#installation--nutzung)
- [Übersetzungsfortschritt](#übersetzungsfortschritt)
- [Mitwirken](#mitwirken)
- [Tools & Verzeichnisstruktur (für Entwickler)](#tools--verzeichnisstruktur-für-entwickler)
- [Urheberrecht & Lizenz](#urheberrecht--lizenz)
- [Danksagungen](#danksagungen)
- [Drittanbieter-Software](#drittanbieter-software)

---

## Unterstützte Zielsprachen

| Sprache | Lokaler Name | ISO-Code | In-Game-Code | Unterstützt | Anmerkung |
|------|------|------|------|------|------|
| Arabisch | العربية | `ar` | `AR` | ❌ | Token-Guthaben unzureichend |
| Katalanisch | català | `ca` | `CA` | ❌ | Token-Guthaben unzureichend |
| Traditionelles Chinesisch | 繁體中文 | `zh-hant` | `CH` | ❌ | Token-Guthaben unzureichend |
| Vereinfachtes Chinesisch | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tschechisch | čeština | `cs` | `CS` | ❌ | Token-Guthaben unzureichend |
| Dänisch | dansk | `da` | `DA` | ❌ | Token-Guthaben unzureichend |
| Deutsch | Deutsch | `de` | `DE` | ✅ | |
| Englisch | English | `en` | `EN` | ✅ | |
| Spanisch | español | `es` | `ES` | ❌ | Token-Guthaben unzureichend |
| Finnisch | suomi | `fi` | `FI` | ❌ | Token-Guthaben unzureichend |
| Französisch | français | `fr` | `FR` | ✅ | |
| Ungarisch | magyar | `hu` | `HU` | ❌ | Token-Guthaben unzureichend |
| Indonesisch | Bahasa Indonesia | `id` | `ID` | ❌ | Token-Guthaben unzureichend |
| Italienisch | italiano | `it` | `IT` | ❌ | Token-Guthaben unzureichend |
| Japanisch | 日本語 | `ja` | `JP` | ✅ | |
| Koreanisch | 한국어 | `ko` | `KO` | ❌ | Token-Guthaben unzureichend |
| Niederländisch | Nederlands | `nl` | `NL` | ❌ | Token-Guthaben unzureichend |
| Norwegisch | norsk | `no` | `NO` | ❌ | Token-Guthaben unzureichend |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Token-Guthaben unzureichend |
| Polnisch | polski | `pl` | `PL` | ❌ | Token-Guthaben unzureichend |
| Portugiesisch (Portugal) | português | `pt` | `PT` | ❌ | Token-Guthaben unzureichend |
| Portugiesisch (Brasilien) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token-Guthaben unzureichend |
| Rumänisch | română | `ro` | `RO` | ❌ | Token-Guthaben unzureichend |
| Russisch | русский | `ru` | `RU` | ❌ | Token-Guthaben unzureichend |
| Thailändisch | ภาษาไทย | `th` | `TH` | ❌ | Token-Guthaben unzureichend |
| Türkisch | Türkçe | `tr` | `TR` | ❌ | Token-Guthaben unzureichend |
| Ukrainisch | українська | `uk` | `UA` | ❌ | Token-Guthaben unzureichend |

**Gesamt**: 27 geplante Sprachen | **Unterstützt**: 5 | **Ausstehend**: 22

---

## Installation & Nutzung

Eine Anleitung für Spieler, die das Übersetzungspaket im Spiel verwenden möchten.

1. Gehe zur Steam Workshop-Seite: [[B42]ProjectBabel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2. Klicke auf „Abonnieren".
3. Starte das Spiel und aktiviere diesen Übersetzungsmod im Mods-Menü.
4. Übersetzungstexte von später geladenen Mods überschreiben frühere, daher muss dieser Übersetzungsmod nach den Spielmods geladen werden.
5. Viel Spaß!

---

## Übersetzungsfortschritt

[➡️ Übersetzungsfortschritt](../progress/progress_de.md)

---

## Mitwirken

Wir freuen uns über Beiträge – ob Fehlerkorrekturen, neue Funktionen, Prompt-Vorlagen oder Referenzübersetzungen!

Der Aufruf von LLM-APIs für Übersetzungen verursacht Token-Kosten. Ihre Unterstützung hilft dem Projekt, langfristig zu bestehen!

Weitere Details im [Mitwirkungsleitfaden](../contributing/contributing_de.md).

---

## Tools & Verzeichnisstruktur (für Entwickler)

Dieser Abschnitt richtet sich an Entwickler, die die Automatisierung des Projekts verstehen möchten.

### Projektverzeichnisse

| Verzeichnis | Beschreibung |
|------|------|
| `src/` | .NET 10 Übersetzungs-Pipeline-Quellcode, 15 Module |
| `config/` | Pipeline-Konfiguration (LLM, Steam, RAG-Parameter usw.) |
| `data/` | Laufzeitdaten: Mod-Metadaten, Embeddings, Übersetzungs-Cache |
| `translation_ref/` | Referenzübersetzungen als LLM-Kontext |
| `base_game_keys/` | Basisspiel-Übersetzungsschlüssel zur Deduplizierung |
| `final_outputs/` | Endgültige PZ-Mod-Format-Übersetzungsausgabe |
| `docs/` | Projektdokumentation: Fortschritt, Mitwirken, Pipeline |
| `temp/` | Temporäre Pipeline-Dateien |
| `src/prompt_templates/` | LLM-Prompt-Vorlagen |

### Pipeline-Module (in Ausführungsreihenfolge)

| Schritt | Modul | Funktion |
|------|------|------|
| 1 | `ConfigReader` | Konfiguration/Geheimnisse/Sprachen laden |
| 2 | `RepoDataLoader` | Referenz- & Übersetzungs-Cache laden |
| 3 | `ModIdCollector` | Workshop-Mod-IDs sammeln |
| 4 | `ModInfoFetcher` | Steam-Metadaten abrufen |
| 5 | `ModDownloader` | Mods via steamcmd herunterladen |
| 6 | `ContentExtractor` | Mod-Übersetzungsdateien parsen → `TranslationEntry` |
| 7 | `ContentChecker` | Inhaltssicherheitsprüfung |
| 8 | `EmbeddingFetcher` | Text-Embedding-Vektoren berechnen |
| 9 | `TranslationBatcher` | Übersetzungsstapel erstellen |
| 10 | `RagContextRetriever` | RAG-Kontexte abrufen |
| 11 | `LLMTranslator` | LLM-Übersetzung ausführen |
| 12 | `ResultWriter` | In data/ & translation_ref/ schreiben |
| 13 | `FinalOutputWriter` | Endgültige PZ-Mod-Ausgabe generieren |
| 14 | `ProgressReporter` | Fortschrittsberichte generieren |

### Technologie-Stack

- **Sprache**: C# (.NET 10)
- **Zielplattform**: GitHub Actions Linux x64 Runner
- **Tests**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurierbar)
- **Embedding**: Textvektorisierung für RAG-Ähnlichkeitssuche
- **Inhaltsprüfung**: LLM-gesteuerte mehrstufige Sicherheitsprüfung

Detaillierte technische Dokumentation: [TranslationEntry-Pipeline](../pipeline/translation_entry_pipeline_de.md)

---

## Urheberrecht & Lizenz

© 2025 Project Babel und Autoren. Alle Rechte vorbehalten.

### Inhalte (Texte, Bilder)

Lizenziert unter **CC BY-NC-SA 4.0**.

- **Namensnennung**: Auf „Project Babel" basierende Änderungen kennzeichnen, mit Repo- & Workshop-Link
- **Nicht-kommerziell**: Kommerzielle Nutzung untersagt
- **Weitergabe unter gleichen Bedingungen**: Änderungen unter derselben Lizenz veröffentlichen

### Code

Code unter `src/` ist unter **GPL-3.0** lizenziert.

---

## Danksagungen

| Referenzmod | Autor | Seite |
|------|------|------|
| [B42]统一·中文汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42]统一·模组汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42]统一·方舟汉化 | 如一汉化组 (As1) | [Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Herzlichen Dank an die oben genannten Autoren!**

---

## Drittanbieter-Software

Dieses Projekt verwendet Drittanbieter-Programme und -Bibliotheken, deren Urheberrechte bei den jeweiligen Entwicklern liegen.
