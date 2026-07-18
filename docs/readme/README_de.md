# Project Babel — LLM-gestütztes automatisches Übersetzungsprojekt für die Mod 《Project Zomboid》

> [English](README_en.md) | [简体中文](../../README.md) <details><summary>Other Languages</summary>[العربية](README_ar.md) | [català](README_ca.md) | [繁體中文](README_zh-hant.md) | [čeština](README_cs.md) | [dansk](README_da.md) | [Deutsch](README_de.md) | [español](README_es.md) | [suomi](README_fi.md) | [français](README_fr.md) | [magyar](README_hu.md) | [Bahasa Indonesia](README_id.md) | [italiano](README_it.md) | [日本語](README_ja.md) | [한국어](README_ko.md) | [Nederlands](README_nl.md) | [norsk](README_no.md) | [Tagalog](README_tl.md) | [polski](README_pl.md) | [português](README_pt.md) | [português do Brasil](README_pt-br.md) | [română](README_ro.md) | [русский](README_ru.md) | [ภาษาไทย](README_th.md) | [Türkçe](README_tr.md) | [українська](README_uk.md)</details>

---

*Dieses Übersetzungsprojekt wird von der Toolsuite [Project Babel](https://github.com/PZProjectBabel/project_babel) betrieben und gewartet.*

---

## Inhaltsverzeichnis

- [Von diesem Projekt unterstützte Zielsprachen](#von-diesem-projekt-unterstützte-zielsprachen)
- [Installation und Nutzung](#installation-und-nutzung)
- [Übersetzungsfortschritt](#übersetzungsfortschritt)
- [Wie man beiträgt](#wie-man-beiträgt)
- [Werkzeuge und Verzeichnisstruktur (für Entwickler)](#werkzeuge-und-verzeichnisstruktur-für-entwickler)
  - [Projektverzeichnis](#projektverzeichnis)
  - [Pipeline-Module (in Ausführungsreihenfolge)](#pipeline-module-in-ausführungsreihenfolge)
  - [Unabhängige Module](#unabhängige-module)
  - [Technologie-Stack](#technologie-stack)
- [Urheberrecht und Lizenz](#urheberrecht-und-lizenz)
  - [1. Texte, Bilder und andere Inhalte](#1-texte-bilder-und-andere-inhalte)
  - [2. Programme, Skripte und andere Entwicklungsinhalte](#2-programme-skripte-und-andere-entwicklungsinhalte)
- [Danksagungen](#danksagungen)
- [Drittanbieterprogramme](#drittanbieterprogramme)

---

## Von diesem Projekt unterstützte Zielsprachen

| Sprache | Lokalname | ISO-Code | Spielinterner Code | Unterstützt | Anmerkungen |
|------|------|------|------|------|------|
| Arabisch | العربية | `ar` | `AR` | ❌ | Token-Kontingent nicht ausreichend |
| Katalanisch | català | `ca` | `CA` | ❌ | Token-Kontingent nicht ausreichend |
| Traditionelles Chinesisch | 繁體中文 | `zh-hant` | `CH` | ❌ | Token-Kontingent nicht ausreichend |
| Vereinfachtes Chinesisch | 简体中文 | `zh-hans` | `CN` | ✅ | |
| Tschechisch | čeština | `cs` | `CS` | ❌ | Token-Kontingent nicht ausreichend |
| Dänisch | dansk | `da` | `DA` | ❌ | Token-Kontingent nicht ausreichend |
| Deutsch | Deutsch | `de` | `DE` | ✅ | |
| Englisch | English | `en` | `EN` | ✅ | |
| Spanisch | español | `es` | `ES` | ❌ | Token-Kontingent nicht ausreichend |
| Finnisch | suomi | `fi` | `FI` | ❌ | Token-Kontingent nicht ausreichend |
| Französisch | français | `fr` | `FR` | ✅ | |
| Ungarisch | magyar | `hu` | `HU` | ❌ | Token-Kontingent nicht ausreichend |
| Indonesisch | Bahasa Indonesia | `id` | `ID` | ❌ | Token-Kontingent nicht ausreichend |
| Italienisch | italiano | `it` | `IT` | ❌ | Token-Kontingent nicht ausreichend |
| Japanisch | 日本語 | `ja` | `JP` | ✅ | |
| Koreanisch | 한국어 | `ko` | `KO` | ❌ | Token-Kontingent nicht ausreichend |
| Niederländisch | Nederlands | `nl` | `NL` | ❌ | Token-Kontingent nicht ausreichend |
| Norwegisch | norsk | `no` | `NO` | ❌ | Token-Kontingent nicht ausreichend |
| Tagalog | Tagalog | `tl` | `PH` | ❌ | Token-Kontingent nicht ausreichend |
| Polnisch | polski | `pl` | `PL` | ❌ | Token-Kontingent nicht ausreichend |
| Portugiesisch (Portugal) | português | `pt` | `PT` | ❌ | Token-Kontingent nicht ausreichend |
| Portugiesisch (Brasilien) | português do Brasil | `pt-br` | `PTBR` | ❌ | Token-Kontingent nicht ausreichend |
| Rumänisch | română | `ro` | `RO` | ❌ | Token-Kontingent nicht ausreichend |
| Russisch | русский | `ru` | `RU` | ❌ | Token-Kontingent nicht ausreichend |
| Thailändisch | ภาษาไทย | `th` | `TH` | ❌ | Token-Kontingent nicht ausreichend |
| Türkisch | Türkçe | `tr` | `TR` | ❌ | Token-Kontingent unzureichend |
| Ukrainisch | українська | `uk` | `UA` | ❌ | Token-Kontingent unzureichend |

**Gesamt**: 27 geplante Sprachen | **Unterstützt**: 5 | **Ausstehend**: 22

---

## Installation und Nutzung

Diese Anleitung richtet sich an Spieler, die dieses Übersetzungsprojekt direkt im Spiel nutzen möchten.

1.  Gehen Sie zu unserer Steam-Workshop-Seite: [[B42]Project Babel](https://steamcommunity.com/sharedfiles/filedetails/?id=3759583822)
2.  Klicken Sie auf die Schaltfläche „Abonnieren".
3.  Starten Sie das Spiel und aktivieren Sie dieses Übersetzungs-Mod im Mod-Manager des Hauptmenüs.
4.  Die Übersetzungstexte von später aktivierten Mods überschreiben priorisiert die von früher aktivierten. Daher muss dieses Übersetzungsmod nach funktionalen Mods aktiviert werden (möglichst weit unten).
5.  Viel Spaß beim Spielen!

---

## Übersetzungsfortschritt

**[➡️ Klicken Sie hier, um den Übersetzungsfortschritt anzuzeigen](./docs/progress/progress_de.md)**

---

## Wie man beiträgt

Wir begrüßen jeden, der einen Beitrag leisten möchte – sei es durch das Korrigieren eines Fehlers, das Hinzufügen einer neuen Funktion, das Schreiben von Prompt-Vorlagen oder das Bereitstellen von Referenzübersetzungen!

Das Aufrufen der LLM-API für Übersetzungen kostet Tokens. Damit das Projekt langfristig stabil läuft, hoffen wir auf Ihre großzügige Unterstützung!

Weitere Informationen finden Sie im [Beitragsleitfaden](./docs/contributing/contributing_de.md)

---

## Werkzeuge und Verzeichnisstruktur (für Entwickler)

Dieser Abschnitt richtet sich an Entwickler, die die Automatisierungsprinzipien des Projekts verstehen möchten.

### Projektverzeichnis

| Verzeichnis | Beschreibung |
|------|------|
| `src/` | .NET 10 Übersetzungspipeline-Quellcode, enthält 15 Module + 2 unabhängige Module |
| `config/` | Pipeline-Konfigurationsdateien (LLM-, Steam-, RAG-Parameter usw.) |
| `data/` | Laufzeitdaten: Mod-Metadaten, Embeddings, Übersetzungs-Cache |
| `translation_ref/` | Referenzübersetzungsdaten (z.B. autorisierte Mods von Übersetzungsgruppen) – dienen als Übersetzungsreferenz für das LLM |
| `base_game_keys/` | Übersetzungsschlüssel des Basisspiels – zur Deduplizierung und Vermeidung von Überschreiben nativer Texte |
| `final_outputs/` | Endgültige Ausgabe: `project_babel/`-Mod-Paket, `icons/`-Symbole und `workshop_descriptions/`-Workshop-Beschreibungen |
| `docs/` | Projektdokumentation: Fortschrittsberichte, Beitragsleitfaden, Pipeline-Beschreibung |
| `temp/` | Temporäre Dateien der Pipeline (pro Durchlauf separates Verzeichnis) |
| `src/prompt_templates/` | LLM-Prompt-Vorlagen (Übersetzung/Inhaltsprüfung) |

### Pipeline-Module (in Ausführungsreihenfolge)

| Schritt | Modul | Funktion |
|------|------|------|
| 1 | `ConfigReader` | Lädt Konfiguration/Schlüssel/Sprachliste |
| 2 | `RepoDataLoader` | Lädt Referenzübersetzungen und Übersetzungs-Cache |
| 3 | `ModIdCollector` | Sammelt Workshop-Mod-ID |
| 4 | `ModInfoFetcher` | Ruft Steam-Metadaten ab |
| 5 | `SteamCmdBootstrapper` | Bereitet die steamcmd-Laufzeit für die aktuelle Plattform vor |
| 6 | `ModDownloader` | Lädt Mods über steamcmd herunter |
| 7 | `ContentExtractor` | Analysiert Mod-Übersetzungsdateien → `TranslationEntry` |
| 8 | `ContentChecker` | Inhaltssicherheitsprüfung (Drogen/Pornographie/Gewalt) |
| 9 | `EmbeddingFetcher` | Berechnet Text-Embedding-Vektoren |
| 10 | `TranslationBatcher` | Erstellt sprachunabhängige Übersetzungschargen |
| 11 | `RagContextRetriever` | Ruft RAG-Kontext ab (genaue Schlüssel + Embedding-Ähnlichkeit) |
| 12 | `LLMTranslator` | Ruft LLM zur Ausführung der Übersetzung auf |
| 13 | `ResultWriter` | Schreibt in data/ und translation_ref/ |
| 14 | `FinalOutputWriter` | Erzeugt finale PZ-Mod-Ausgabe |
| 15 | `ProgressReporter` | Erzeugt Fortschrittsbericht |

### Unabhängige Module

| Modul | Funktion |
|------|------|
| `WorkshopMonitor` | Regelmäßiges Abrufen neuer Mods aus dem Steam Workshop, Filtern nach Abonnementzahl und Aufnahme in `request_for_translation.txt` |
| `DocGenerator` | LLM-gesteuerter mehrsprachiger Dokumentgenerator |

### Technologie-Stack

- **Sprache**: C# (.NET 10)
- **Zielplattform**: GitHub Actions Linux x64 runner
- **Tests**: xUnit (Windows x64)
- **LLM**: DeepSeek API (konfigurierbar)
- **Embedding**: Textvektorisierung für RAG-Ähnlichkeitssuche
- **Inhaltsprüfung**: LLM-gesteuerte mehrstufige Sicherheitsüberprüfung

Ausführliche [Technische Referenz](./docs/technical_reference/technical_reference_de.md)

---

## Urheberrecht und Lizenz

Die übersetzten Texte und zugehörigen Bilder dieses Übersetzungsprojekts wurden von **Project Babel** und den Mitwirkenden basierend auf den Originalspielmods erstellt oder bearbeitet.

© 2025 Project Babel und alle Autoren behalten sich alle Rechte vor.

### 1. Texte, Bilder und andere Inhalte

Sofern nicht anders angegeben, in diesem Repository:

- Übersetzung, Lektorat und Korrektur von Spieltexten;
Projektbeschreibungsdokumente, Modultextübersetzungen;
Speziell für dieses Projekt erstellte Bilder und künstlerische Ressourcen

Werden unter der Lizenz **Namensnennung - Nicht kommerziell - Weitergabe unter gleichen Bedingungen 4.0 International** (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, kurz **CC BY-NC-SA 4.0**) veröffentlicht.

Das bedeutet, dass Sie diese Inhalte unter folgenden Bedingungen frei teilen und anpassen können:

- **Namensnennung (BY)**: An einer gut sichtbaren Stelle angeben: „Dieses Übersetzungsprojekt basiert auf der Arbeit von „Project Babel“ und wurde modifiziert.“, und fügen Sie einen Link zu diesem Repository und der Steam Workshop-Seite hinzu: `https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080`
- **Nicht kommerziell (NC)**: Die Inhalte dieses Projekts oder deren Bearbeitungen dürfen nicht für direkte oder indirekte kommerzielle Zwecke genutzt werden (einschließlich, aber nicht beschränkt auf kostenpflichtige Pakete, kostenpflichtige Downloads, Werbeanteile usw.);
- **Weitergabe unter gleichen Bedingungen (SA)**: Wenn Sie auf der Grundlage dieses Projekts Änderungen oder Bearbeitungen vornehmen, müssen Sie Ihre Änderungen unter **derselben CC BY-NC-SA 4.0 Lizenz** veröffentlichen.

Weitere Informationen zu dieser Lizenz finden Sie unter:
<https://creativecommons.org/licenses/by-nc-sa/4.0/deed.de>

*Besondere Hinweise:*
- *Der Inhalt des Ordners base_game_keys stammt aus dem Spiel selbst, das Urheberrecht liegt beim Spielentwickler! Der Inhalt dient dazu, eine Überschreibung der Spielschlüssel durch Übersetzungsschlüssel zu verhindern (Deduplizierung).*
- *Der Inhalt des Ordners translation_ref dient als Übersetzungsreferenz für das LLM, das Urheberrecht liegt bei den jeweiligen Mod-Entwicklern!*

### 2. Programme, Skripte und andere Entwicklungsinhalte

Sofern in den Quelldateien oder Verzeichnissen nicht anders angegeben, unterliegt der Programmcode in diesem Repository, der zur Erstellung/Verpackung/Verarbeitung von Lokalisierungsinhalten verwendet wird (z. B. der Code im Verzeichnis `src/`), der **GNU General Public License Version 3 (GPL-3.0)**.

Den vollständigen Lizenztext finden Sie in der Datei `LICENSE` im Stammverzeichnis dieses Repositorys (GPL-3.0) oder auf der GNU-Website: <https://www.gnu.org/licenses/gpl-3.0.html>

---

## Danksagungen

Dieses Projekt verwendet Mods von Drittanbietern als Referenztexte für die Übersetzung in die Zielsprache. Die Referenztexte werden an das LLM zur Übersetzungsreferenz gesendet.

| Referenz-Mod-Name | Autor | Mod-Seite |
|------|------|------|
| [B42] Einheitliche chinesische Lokalisierung | Ruyi Lokalisierungsteam (As1) | [Workshop-Seite](https://steamcommunity.com/sharedfiles/filedetails/?id=3556544454) |
| [B42] Einheitliche Mod-Lokalisierung | Ruyi Lokalisierungsteam (As1) | [Workshop-Seite](https://steamcommunity.com/sharedfiles/filedetails/?id=3556540080) |
| [B42] Einheitliche Ark-Lokalisierung | Ruyi Lokalisierungsteam (As1) | [Workshop-Seite](https://steamcommunity.com/sharedfiles/filedetails/?id=3732061188) |

**Herzlichen Dank an die oben genannten Autoren!**

---

## Drittanbieterprogramme

Dieses Projekt verwendet Drittanbieterprogramme und -bibliotheken. Das Urheberrecht dieser Drittanbieterprogramme liegt bei den jeweiligen Entwicklern.

