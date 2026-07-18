# Beitragsrichtlinien (CONTRIBUTING)

> [English](contributing_en.md) | [简体中文](contributing_zh-hans.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [Deutsch](contributing_de.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

## Inhaltsverzeichnis

- [1. Bevor du beginnst](#1-bevor-du-beginnst)
- [2. Wie kann ich beitragen?](#2-wie-kann-ich-beitragen)
- [3. Bereitstellung von Übersetzungsregeln, Fachwörterbüchern und Verbesserung der System-Prompts](#3-bereitstellung-von-übersetzungsregeln-fachwörterbüchern-und-verbesserung-der-system-prompts)
- [4. Manuell korrigiertes Korpus bereitstellen](#4-manuell-korrigiertes-korpus-bereitstellen)
- [5. Pipeline- und Toolentwicklungsbeiträge](#5-pipeline--und-toolentwicklungsbeiträge)
- [6. Urheberrecht und Lizenzvereinbarung](#6-urheberrecht-und-lizenzvereinbarung)
  - [6.1 Grundprinzip: Du behältst das Urheberrecht und gewährst dem Projekt gleichzeitig eine Nutzungslizenz](#61-grundprinzip-du-behältst-das-urheberrecht-und-gewährst-dem-projekt-gleichzeitig-eine-nutzungslizenz)
  - [6.2 Lizenzierung von Texten, Bildern und anderen Inhalten (CC BY-NC-SA 4.0)](#62-lizenzierung-von-texten-bildern-und-anderen-inhalten-cc-by-nc-sa-40)
  - [6.3 Lizenzierung von Skript- und Tool-Code (GPL-3.0)](#63-lizenzierung-von-skript--und-tool-code-gpl-30)
  - [6.4 Urheberrecht der übergeordneten Werke und des Originalspiels](#64-urheberrecht-der-übergeordneten-werke-und-des-originalspiels)
- [7. Kommunikation und Zusammenarbeit](#7-kommunikation-und-zusammenarbeit)
- [8. Finanzielle Unterstützung](#8-finanzielle-unterstützung)

---

Vielen Dank, dass du bereit bist, einen Beitrag zu **Project Babel – LLM-basiertes automatisches Übersetzungsprojekt für Mods von „Project Zomboid“** zu leisten! Sei es das Beheben eines Fehlers, das Hinzufügen einer Funktion, das Erstellen von Prompt-Vorlagen oder das Bereitstellen von Referenzübersetzungen!

Der Aufruf der LLM-API zur Übersetzung verursacht Token-Kosten. Damit das Projekt langfristig stabil laufen kann, hoffen wir auf deine großzügige Unterstützung!

> ⚠️ **Wichtiger Hinweis:**
> Bevor du irgendwelche Inhalte an dieses Repository übermittelst, lies und verstehe bitte den Abschnitt „Urheberrecht und Lizenzvereinbarung“.
> Sobald du deinen Beitrag einreichst und dieser zusammengeführt wird, gilt dies als Zustimmung zu den entsprechenden Lizenzbedingungen.

---

## 1. Bevor du beginnst

Lies zunächst die Projekt-`README.md`, um Folgendes zu verstehen:
- Die Gesamtziele und den aktuellen Stand des Projekts;
- Wie normale Spieler dieses Projekt nutzen können (zur Selbstkontrolle);
- Technische Details des Projekts.

---

## 2. Wie kann ich beitragen?

Du kannst je nach Interesse und Fähigkeiten eine oder mehrere der folgenden Methoden wählen:

- Übersetzungsregeln für die Zielsprache bereitstellen
- Ein Fachwörterbuch für die Zielsprache bereitstellen
- Die System-Prompts verbessern
- Manuell überprüfte Übersetzungstexte als Korpus bereitstellen
- Die Pipeline-Module (.NET) und Automatisierungsskripte verbessern
- Probleme melden und Verbesserungsvorschläge machen (in Issues erläutern)
- Finanzielle Unterstützung für LLM-Aufrufe bereitstellen

Im Folgenden werden die wichtigsten Beitragsszenarien näher erläutert.

---

## 3. Bereitstellung von Übersetzungsregeln, Fachwörterbüchern und Verbesserung der System-Prompts

Die Prompt-Vorlagen der Pipeline befinden sich unter `src/prompt_templates/` und sind wie folgt strukturiert:

- `system_prompt_translate_engine.txt`: System-Prompt für die globale Übersetzungsmaschine (von allen Sprachen gemeinsam genutzt);
- `<sprachcode>/translation_dictionary_<sprachcode>.json`: Fachwörterbuch dieser Sprache;
- `<sprachcode>/translation_schema_<sprachcode>.md`: Übersetzungsregeln und Stilbeschränkungen dieser Sprache.

Schritte für den Beitrag:

1. Erstelle unter `src/prompt_templates/` ein Unterverzeichnis für deine Sprache und füge das Fachwörterbuch und die Übersetzungsregeldatei hinzu;
2. Falls du das globale Übersetzungsverhalten anpassen möchtest, ändere `system_prompt_translate_engine.txt` (beachte, dass dies alle Sprachen betrifft);
3. Lokale Tests bestätigen die Wirkung;
4. PR einreichen.

---

## 4. Manuell korrigiertes Korpus bereitstellen

Wenn Sie ein Übersetzungsmod-Autor sind und bereit sind, Ihr Übersetzungskorpus als Referenz für die LLM-Übersetzung zur Verfügung zu stellen, reichen Sie bitte einen Antrag im Issue ein. Sie müssen die folgenden Informationen bereitstellen:

- Die Mod-ID Ihres Übersetzungsmods und die Zielsprache der Übersetzung;
- Ein Screenshot der Adminseite Ihres Übersetzungsmods, um zu belegen, dass Sie der Mod-Autor sind;
- Geben Sie im Issue klar an, dass Sie bereit sind, das Übersetzungskorpus bereitzustellen;
- Falls besondere Umstände vorliegen (besondere Lizenz usw.), bitte ebenfalls angeben;
- Stellen Sie sicher, dass das von Ihnen bereitgestellte Korpus von hoher Qualität ist.

Mit Ihrer Genehmigung wird das Projekt Ihren Mod in die Liste der Referenzübersetzungsmods in `config/ref_translation_mods.json` aufnehmen, und die Pipeline wird Ihre Übersetzungstexte automatisch als RAG-Referenzkorpus synchronisieren.

---

## 5. Pipeline- und Toolentwicklungsbeiträge

Die Automatisierung dieses Projekts besteht aus zwei Teilen:

**Pipeline-Module (`src/`, C# / .NET 10)**: Enthält 15 sequentiell ausgeführte Module sowie 2 unabhängige Module (`WorkshopMonitor` Modul-Entdecker, `DocGenerator` Dokumentengenerator), die den gesamten Ablauf von der SteamCMD-Initialisierung, Modul-Download, Textextraktion, Inhaltsprüfung, Embedding-Berechnung, RAG-Abfrage bis zur LLM-Übersetzung und endgültigen Ausgabe abdecken. Siehe [Technische Referenz](../technical_reference/technical_reference_de.md).

**Hilfsskripte (`.github/`)**: Für die Automatisierung auf GitHub.

Wenn Sie möchten:

* Fehler in bestehenden Pipeline-Modulen oder Skripten beheben;
* Neue Funktionen oder Module zur Pipeline hinzufügen;
* Leistung oder Code-Struktur optimieren;
* Prompt-Vorlagen oder RAG-Strategien verbessern;

Sie können wie folgt vorgehen:

1. Forken Sie dieses Repository und klonen Sie es lokal;
2. Erstellen Sie einen neuen Branch basierend auf dem neuesten Branch;
3. Ändern oder fügen Sie Dateien im entsprechenden Verzeichnis hinzu:
- Pipeline-Moduländerung → `src/<Modulname>/`;
- CI-Workflow-Änderungen → `.github/workflows/`；
- Prompt-Vorlagenänderung → `src/prompt_templates/`;
4. Bitte versuchen Sie vor dem Absenden:

* Behalten Sie den bestehenden Codestil bei;
* Fügen Sie notwendige Kommentare hinzu;
* Fügen Sie nach Möglichkeit eine einfache Test- oder Gebrauchsanweisung bei;
5. Reiche die Änderung per PR ein und beschreibe in der Beschreibung:

* Zweck der Änderung;
* Betroffene Verzeichnisse / Module / Skripte;
* Ob es sich um eine breaking change handelt.

---

## 6. Urheberrecht und Lizenzvereinbarung

> **Hinweis:**
> Die Urheberrechts- und Lizenzvereinbarung dient dem Schutz der berechtigten Interessen des Projekts, der Autoren, Mitwirkenden und Spieler, um Missverständnisse durch „stillschweigende“ oder „standardmäßige“ Annahmen zu vermeiden. Bitte lesen Sie es sorgfältig.
> Das Urheberrecht und die Lizenzierung richten sich nach dem Inhalt der README.md-Datei. Dieser Abschnitt bietet lediglich eine verständlichere Beschreibung.

### 6.1 Grundprinzip: Du behältst das Urheberrecht und gewährst dem Projekt gleichzeitig eine Nutzungslizenz

* Du behältst das Urheberrecht an den von dir erstellten Inhalten (Übersetzungen, Bilder, Skripte/Programme usw.);
* Aber nachdem du diese Inhalte an dieses Projekt übermittelt und sie übernommen (zusammengeführt) hast, stimmst du zu, diese Inhalte gemäß der von diesem Projekt verwendeten Open-Source-/Shared-Lizenzvereinbarung an andere zu lizenzieren.

Das bedeutet:

* Du **kannst** deine Werke weiterhin an anderen Orten nutzen und präsentieren;
* Du **kannst jedoch nicht** verlangen, dass das Projekt oder andere Benutzer, die das Werk rechtmäßig erhalten haben, nach der Zusammenführung der Beiträge die „Genehmigung widerrufen“ oder „historische Versionen löschen“.

### 6.2 Lizenzierung von Texten, Bildern und anderen Inhalten (CC BY-NC-SA 4.0)

Für die folgenden von dir eingereichten Inhalte:

* Übersetzung, Überarbeitung und Korrektur von Spieltexten;
* Projektdokumentation, erklärende Texte;
* Speziell für dieses Projekt erstellte Bilder, künstlerische Ressourcen;

Sobald sie von diesem Repository übernommen und zusammengeführt werden, giltst du als einverstanden:

1. Diese Inhalte werden unter der **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International** (Kurzform **CC BY-NC-SA 4.0**) Lizenz lizenziert;
2. Project Babel und alle Benutzer, die diese Inhalte erhalten, können unter der **Einhaltung der CC BY-NC-SA 4.0 Bestimmungen**:
* Diese Inhalte teilen, kopieren und weiterverbreiten;
* Sie unter nicht-kommerzieller Nutzung modifizieren und neu erstellen;
3. Du stimmst zu, dass diese Lizenz im Rahmen des geltenden Rechts eine **nicht-exklusive, weltweite, gebührenfreie und unwiderrufliche** Lizenz ist;
4. Selbst wenn du später aus diesem Projekt austrittst oder die Teilnahme beendest, kann dieses Projekt die von dir bereits eingereichten und zusammengeführten Inhalte weiterhin gemäß CC BY-NC-SA 4.0 nutzen und weiterveröffentlichen.

> Wenn du die oben genannte Lizenzierungsweise nicht akzeptierst, reiche bitte keine Text- oder Bildbeiträge zu diesem Projekt ein,
> oder kommuniziere vorher mit dem Projektbetreuer, um zu klären, ob eine Zusammenarbeit auf andere Weise möglich ist.

### 6.3 Lizenzierung von Skript- und Tool-Code (GPL-3.0)

Für die von dir eingereichten und übernommenen:

* Automatisierungsskripte;
* Build-/Export-Tools;
* Andere Programmcode zur Bearbeitung dieses Lokalisierungsprojekts;

Sofern nicht anders angegeben, giltst du zu:

1. Der Code wird unter der **GPL-3.0**-Lizenz (GNU General Public License v3) lizenziert;
2. Projektbetreuer können ihn im Rahmen der GPL-3.0 modifizieren, zusammenführen und verteilen;
3. Du kannst auch auf Basis des gleichen Codes andere Projekte fortsetzen, solange du die Bedingungen der GPL-3.0 einhältst.

Um Lizenzkonflikte zu vermeiden, versuche nach Möglichkeit:

* Keine **mit GPL-3.0 inkompatiblen** Drittanbieter-Codes ohne vorherige Prüfung einzubinden;
* Falls du Drittanbieter-Bibliotheken verwenden musst, gib im PR deren Quelle und Lizenz klar an und bestätige deren Kompatibilität.

### 6.4 Urheberrecht der übergeordneten Werke und des Originalspiels

Dieses Projekt ist ein **inoffizielles Übersetzungsprojekt** für Mods im Zusammenhang mit *Project Zomboid*:

* Das Urheberrecht für das Originalspiel und die einzelnen Mods liegt bei ihren jeweiligen Autoren/Herausgebern;
* Dieses Projekt erstellt und bearbeitet lediglich Textübersetzungen, Verfeinerungen und einige begleitende Ressourcen;
* Mitwirkende sollten bei der Einreichung von Inhalten sicherstellen:
* Keine direkte Kopie von nicht autorisierten Übersetzungstexten oder Grafikressourcen Dritter;
* Respektierung der Rechte der ursprünglichen Autoren und Mod-Autoren, keine Urheberrechtsverletzung durch Weiterverbreitung.

---

## 7. Kommunikation und Zusammenarbeit

Falls du:

* Fragen zu den Lizenzbedingungen hast;
* Dir unsicher bist, ob ein bestimmter Inhalt beigetragen werden kann;
* Deine Arbeit auf eine bestimmte Weise lizenzieren möchtest (z. B. nur nicht-kommerzielle Nutzung ohne Bearbeitungserlaubnis);

Kontaktiere die Projektbetreuer gerne auf folgende Weise:

* Erstelle ein Issue zur Diskussion;
* Über andere öffentlich verfügbare Kontaktinformationen der Betreuer.

Wir werden nach Möglichkeit eine Lösung finden, die die Rechte aller Beteiligten respektiert und die gesunde Entwicklung des Projekts berücksichtigt.

---

## 8. Finanzielle Unterstützung

Während des Projektbetriebs müssen aufgrund neuer Mods oder aktualisierter Textinhalte älterer Mods kontinuierlich LLM-APIs zur Übersetzung aufgerufen werden. Um das Verhalten des LLMs zu steuern, werden neben den grundlegenden Mod-Texten auch umfangreiche Prompt-Inhalte benötigt (einschließlich Basisprompts, Übersetzungsregeln, Glossare, Ein-/Ausgabebeschränkungen, semantische Abfrageergebnisse usw.), die weitaus mehr Tokens als die ursprünglichen Texte verbrauchen. Daher benötigt das Projekt finanzielle Unterstützung.

Wenn du bereit bist, finanzielle Unterstützung zu leisten, kontaktiere bitte die Projektbetreuer. Vielen Dank!

---

Nochmals vielen Dank, dass du zu diesem Projekt beitragen möchtest!
Jeder deiner Beiträge wird noch mehr Spielern zugutekommen!
