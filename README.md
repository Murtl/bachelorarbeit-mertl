# bachelorarbeit-mertl
Dieses Repo beinhaltet alle durchgeführten Experimente in meiner Bachelorarbeit "Ist die Trennung zwischen Frontend und Backend in webbasierten Systemen noch zeitgemäß?".

# Kurze Erklärung
Die Ordner spiegeln die drei durchgeführten Experimente in der Bachelorarbeit wider. Es wurde dieselbe Anwendung mit drei unterschiedlichen Architekturen und Ansätzen entwickelt.
Bei dem Anwendungsfall ging es um das Erstellen einer Schulungsanwendung für die "Schalk Maschinen GmbH" bei der Mitarbeiter über ihre Microsoft Accounts Zugriff bekommen.

# Ausführen der Anwendungen
Damit die unterschiedlichen Anwendungen lokal ausgeführt werden, ist `Visual Studio` und ebenfalls `Microsoft Azure` nötig. Nachfolgend werden die Schritte kurz erläutert:
- Entwicklungsumgebung herunterladen `Visual Studio`
- Repo klonen `git clone ...`
- Repo in der Entwicklungsumgebung öffnen (Je nachdem welche der drei Anwendungen geöffnet werden soll, muss entsprechend die `.sln`-Datei geöffnet werden)
- Die Anwendung in `Microsoft Azure` erstellen ([Tutorial von Microsoft](https://learn.microsoft.com/de-de/entra/identity-platform/quickstart-register-app)) und in der `appsettings.json` die Platzhalter (markiert mit `<...>`) füllen (in der SPA muss auch die `msalConfig.ts` mit enstprechenden Werten gefüllt werden)
- Zusätzlich in `Microsoft Azure` zwei Rollen erstellen mit den Werten `Task.Create` (Organiser) und `Task.Apply` (Participant) und zuweisen zu zugeordneten Microsofts Accounts der Anwendung ([Tutorial von Microsoft](https://learn.microsoft.com/de-de/entra/identity-platform/howto-add-app-roles-in-apps)) 
- Anwendung über `Visual Studio` starten und im Browser benutzen (einloggen mit vorher in `Microsoft Azure` zugewiesenen Accounts mit Rollen)
