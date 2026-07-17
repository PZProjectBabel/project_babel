# Mitwirkungsleitfaden (CONTRIBUTING)

> GitHub: [PZProjectBabel/project_babel](https://github.com/PZProjectBabel/project_babel)
> 
> [简体中文](contributing_zh-hans.md) | [English](contributing_en.md) <details><summary>Other Languages</summary>[العربية](contributing_ar.md) | [català](contributing_ca.md) | [繁體中文](contributing_zh-hant.md) | [čeština](contributing_cs.md) | [dansk](contributing_da.md) | [español](contributing_es.md) | [suomi](contributing_fi.md) | [français](contributing_fr.md) | [magyar](contributing_hu.md) | [Bahasa Indonesia](contributing_id.md) | [italiano](contributing_it.md) | [日本語](contributing_ja.md) | [한국어](contributing_ko.md) | [Nederlands](contributing_nl.md) | [norsk](contributing_no.md) | [Tagalog](contributing_tl.md) | [polski](contributing_pl.md) | [português](contributing_pt.md) | [português do Brasil](contributing_pt-br.md) | [română](contributing_ro.md) | [русский](contributing_ru.md) | [ภาษาไทย](contributing_th.md) | [Türkçe](contributing_tr.md) | [українська](contributing_uk.md)</details>

---

Vielen Dank für deine Bereitschaft, zum **Project Babel — dem LLM-gestützten automatischen Übersetzungsprojekt für Project Zomboid-Mods** beizutragen! Ob Fehlerbehebungen, neue Funktionen, Prompt-Vorlagen oder Referenzübersetzungen — jeder Beitrag zählt!

Der Aufruf der LLM-API für Übersetzungen verursacht Token-Kosten. Damit das Projekt langfristig nachhaltig betrieben werden kann, ist deine großzügige Unterstützung sehr willkommen!

> ⚠️ **Wichtiger Hinweis:**  
> Bevor du etwas zu diesem Repository beiträgst, lies bitte den Abschnitt „Urheberrecht & Lizenzierung" sorgfältig durch.  
> Mit der Einreichung und Zusammenführung erklärst du dich mit den entsprechenden Lizenzbedingungen einverstanden.

---

## Bevor du beginnst

Bitte lies die README.md des Projekts, um Folgendes zu verstehen:

- Die übergeordneten Ziele und den aktuellen Stand des Projekts;
- Wie normale Spieler dieses Projekt nutzen (für deine eigenen Tests);
- Technische Details des Projekts.

---

## Wie kann ich beitragen?

Du kannst je nach Interessen und Fähigkeiten auf eine oder mehrere Arten mitwirken:

- Übersetzungsregeln für eine Zielsprache bereitstellen
- Ein Begriffswörterbuch für eine Zielsprache bereitstellen
- Die System-Prompts verbessern
- Manuell korrigierte Übersetzungskorpora bereitstellen
- Pipeline-Module (.NET) und Automatisierungsskripte verbessern
- Probleme melden und Verbesserungsvorschläge einreichen (über Issues)
- Finanzielle Unterstützung für LLM-API-Aufrufe leisten

Im Folgenden werden die wichtigsten Beitragsszenarien erläutert.

---

## Übersetzungsregeln, Begriffswörterbücher und Verbesserung der System-Prompts

Die Prompt-Vorlagen der Pipeline befinden sich in src/prompt_templates/ mit folgender Struktur:

- system_prompt_translate_engine.txt: der globale System-Prompt der Übersetzungs-Engine (für alle Sprachen gemeinsam);
- <Sprachcode>/translation_dictionary_<Sprachcode>.json: das Begriffswörterbuch für diese Sprache;
- <Sprachcode>/translation_schema_<Sprachcode>.md: die Übersetzungsregeln und Stilvorgaben für diese Sprache.

Beitragsschritte:

1. Erstelle ein Unterverzeichnis unter src/prompt_templates/ für deine Sprache und füge die Wörterbuch- und Regelsdateien hinzu;
2. Wenn du das globale Übersetzungsverhalten anpassen möchtest, bearbeite system_prompt_translate_engine.txt (beachte: dies betrifft alle Sprachen);
3. Teste lokal, um die Ergebnisse zu überprüfen;
4. Reiche einen PR ein.

---

## Bereitstellung manuell korrigierter Korpora

Wenn du Autor eines Übersetzungs-Mods bist und bereit bist, dein Übersetzungskorpus als LLM-Übersetzungsreferenz zur Verfügung zu stellen, reiche bitte einen Antrag über ein Issue ein. Du musst folgende Informationen bereitstellen:

- Die Mod-ID deines Übersetzungs-Mods und die Zielsprache;
- Einen Screenshot der Verwaltungsseite deines Übersetzungs-Mods als Nachweis der Autorenschaft;
- Eine klare Erklärung im Issue, dass du bereit bist, das Übersetzungskorpus zur Verfügung zu stellen;
- Falls es besondere Umstände gibt (spezielle Lizenzierung etc.), erläutere diese bitte;
- Bitte stelle sicher, dass das bereitgestellte Korpus von hoher Qualität ist.

Mit deiner Genehmigung wird das Projekt deinen Mod in die Referenzübersetzungs-Modliste config/ref_translation_mods.json aufnehmen, und die Pipeline wird deine Übersetzungstexte automatisch als RAG-Referenzkorpora synchronisieren.

---

## Pipeline- und Tool-Entwicklungsbeiträge

Die Automatisierung in diesem Projekt ist in zwei Teile gegliedert:

**Pipeline-Module (src/, C# / .NET 10)**: Enthält 15 sequenziell ausgeführte Module, die den gesamten Workflow vom Mod-Download, der Textextraktion, Inhaltsprüfung, Embedding-Berechnung, RAG-Abruf bis zur LLM-Übersetzung und Endausgabe abdecken. Siehe die [technische Referenz](../technical_reference/technical_reference_de.md) für Details.

**Hilfsskripte (.github/)**: Werden für die GitHub-Automatisierung verwendet.

Wenn du Folgendes möchtest:

* Fehler in bestehenden Pipeline-Modulen oder Skripten beheben;
* Neue Funktionen oder Module zur Pipeline hinzufügen;
* Die Leistung oder Code-Struktur optimieren;
* Prompt-Vorlagen oder RAG-Strategien verbessern;

Kannst du wie folgt vorgehen:

1. Forke dieses Repository und klone es lokal;
2. Erstelle einen neuen Branch vom neuesten Branch;
3. Ändere oder füge Dateien in den entsprechenden Verzeichnissen hinzu:
   - Pipeline-Modul-Änderungen → src/<Modulname>/;
   - Skript-Änderungen → scripts/;
   - Prompt-Vorlagen-Änderungen → src/prompt_templates/;
4. Bitte vor dem Einreichen:

   * Den bestehenden Code-Stil beibehalten;
   * Notwendige Kommentare hinzufügen;
   * Wenn möglich, einfache Tests oder Nutzungshinweise beifügen;
5. Änderungen per PR einreichen und in der Beschreibung angeben:

   * Zweck der Änderung;
   * Möglicherweise betroffene Verzeichnisse / Module / Skripte;
   * Ob es sich um eine Breaking Change handelt.

---

## Urheberrecht & Lizenzierung

> **Freundlicher Hinweis:**
> Die Urheberrechts- und Lizenzbestimmungen dienen dem Schutz der legitimen Rechte und Interessen des Projekts, der Autoren, Beitragenden und Spieler und sollen Missverständnisse durch „stillschweigende Übereinkunft" oder „Standardannahmen" vermeiden. Bitte lies sie sorgfältig.
> Maßgeblich sind die Bestimmungen in der README.md; dieser Abschnitt bietet nur eine leichter verständliche Beschreibung.

### 1. Grundprinzip: Du behältst das Urheberrecht und lizenzierst das Projekt zur Nutzung

* Du behältst das Urheberrecht an den von dir erstellten Inhalten (Übersetzungen, Bilder, Skripte/Programme usw.);
* Mit der Einreichung und Annahme (Merge) dieser Inhalte in dieses Projekt
  erklärst du dich jedoch damit einverstanden, dass andere diese Inhalte unter der von diesem Projekt angenommenen Open-Source-/Shared-Lizenz nutzen dürfen.

Das bedeutet:

* Du **kannst** deine Werke weiterhin an anderer Stelle nutzen und zeigen;
* Du **kannst** jedoch nach der Zusammenführung deines Beitrags nicht von diesem Projekt oder anderen Nutzern, die das Werk rechtmäßig erhalten haben, verlangen, „die Lizenz zu widerrufen" oder „historische Versionen zu löschen".

### 2. Lizenzierung von Texten, Bildern und ähnlichen Inhalten (CC BY-NC-SA 4.0)

Für die folgenden von dir eingereichten Inhalte:

* Spieltext-Übersetzungen, Überarbeitungen und Korrekturen;
* Projektdokumentation und erläuternde Texte;
* Speziell für dieses Projekt erstellte Bilder und Grafikressourcen;

Mit der Annahme und Zusammenführung in dieses Repository erklärst du dich einverstanden, dass:

1. Diese Inhalte unter **Namensnennung – Nicht-kommerziell – Weitergabe unter gleichen Bedingungen 4.0 International**
   (Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International, kurz **CC BY-NC-SA 4.0**) lizenziert werden;
2. Project Babel und alle Nutzer, die diese Inhalte erhalten, unter **Einhaltung der CC BY-NC-SA 4.0-Bedingungen**:

   * Diese Inhalte teilen, kopieren und weiterverbreiten dürfen;
   * Sie für nicht-kommerzielle Zwecke bearbeiten und neue Werke daraus schaffen dürfen;
3. Du stimmst zu, dass diese Lizenz im Rahmen des geltenden Rechts **nicht-exklusiv, weltweit, lizenzgebührenfrei und unwiderruflich** ist;
4. Selbst wenn du später aus dem Projekt ausscheidest oder deine Teilnahme beendest, darf das Projekt die von dir eingereichten und zusammengeführten Inhalte weiterhin unter CC BY-NC-SA 4.0 nutzen und weiterverbreiten.

> Wenn du die oben genannten Lizenzbedingungen nicht akzeptierst, reiche bitte keine Text- oder Bildbeiträge zu diesem Projekt ein,
> oder stimme dich vorab mit den Projektbetreuern ab, ob eine Zusammenarbeit auf andere Weise möglich ist.

### 3. Lizenzierung von Skripten und Tool-Code (GPL-3.0)

Für das Folgende, das du einreichst und das angenommen wird:

* Automatisierungsskripte;
* Build-/Export-Tools;
* Sonstiger Programmcode zur Verarbeitung dieses Übersetzungsprojekts;

Ohne besondere Erklärung gilt dies als deine Zustimmung, dass:

1. Der Code unter **GPL-3.0** (GNU General Public License Version 3) lizenziert wird;
2. Die Projektbetreuer ihn im Rahmen der GPL-3.0 modifizieren, zusammenführen und verteilen dürfen;
3. Du kannst ebenfalls andere Projekte auf demselben Code aufbauen, solange du die GPL-3.0-Bedingungen einhältst.

Um Lizenzkonflikte zu vermeiden, bitte möglichst:

* Keinen **mit GPL-3.0 inkompatiblen** Drittanbieter-Code ohne vorherige Prüfung einführen;
* Falls du auf Drittanbieter-Bibliotheken zurückgreifen musst, gib deren Quelle und Lizenz im PR klar an und bestätige die Kompatibilität.

### 4. Upstream-Werke und Original-Spiel-Urheberrecht

Dieses Projekt ist ein **inoffizielles Übersetzungsprojekt** für Mods zu *Project Zomboid*:

* Die Urheberrechte am Originalspiel und den einzelnen Mods liegen bei den jeweiligen Autoren/Herausgebern;
* Dieses Projekt umfasst ausschließlich die Erstellung und Aufbereitung von Textübersetzungen, stilistischen Anpassungen und einigen Begleitressourcen;
* Beitragende sollten bei der Einreichung von Inhalten sicherstellen:

  * Keine nicht autorisierten Drittübersetzungstexte oder Grafikressourcen direkt zu kopieren;
  * Die Rechte der Originalautoren und Mod-Autoren zu respektieren und keine urheberrechtsverletzende Weiterverbreitung vorzunehmen.

---

## Kommunikation & Zusammenarbeit

Wenn du:

* Fragen zu den Lizenzbedingungen hast;
* Unsicher bist, ob bestimmte Inhalte beigetragen werden können;
* Deine Werke auf besondere Weise lizenzieren möchtest (z. B. nur nicht-kommerzielle Nutzung, aber keine Bearbeitung erlaubt);

Kontaktiere die Projektbetreuer gerne über:

* Einreichung eines Issues zur Diskussion;
* Andere öffentlich verfügbare Kontaktmöglichkeiten der Betreuer.

Wir werden unser Bestes tun, um unter Wahrung der Rechte und Interessen aller Beteiligten eine Lösung zu finden, die der gesunden Entwicklung des Projekts dient.

---

## Finanzielle Unterstützung

Im Projektbetrieb muss die LLM-API aufgrund neuer Mods und Textaktualisierungen bestehender Mods kontinuierlich für Übersetzungen aufgerufen werden. Um das LLM-Verhalten zu steuern, werden neben den grundlegenden Mod-Texten umfangreiche Prompt-Inhalte benötigt (Basisprompts, Übersetzungsregeln, Begriffstabellen, Ein-/Ausgabe-Beschränkungen, semantische Suchergebnisse etc.), die weit mehr Tokens verbrauchen als die Originaltexte. Daher benötigt das Projekt finanzielle Unterstützung.

Wenn du finanzielle Unterstützung leisten möchtest, kontaktiere bitte die Projektbetreuer. Vielen Dank!

---

Nochmals vielen Dank für deine Bereitschaft, zu diesem Projekt beizutragen!
Jeder deiner Beiträge kommt mehr Spielern zugute!
