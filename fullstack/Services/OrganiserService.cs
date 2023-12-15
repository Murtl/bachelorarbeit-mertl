using fullstack.Typen;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Globalization;

namespace fullstack.Services
{
    // Der OrganiserService ist für die Verwaltung der Events zuständig und erfordert die Rolle "Task.Create"
    [Authorize(Roles = "Task.Create")]
    public class OrganiserService
    {
        // Liefert alle Events aus der JSON-Datei zurück
        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/Events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in einen Array von Event-Objekten
                Event[] events = JsonConvert.DeserializeObject<Event[]>(jsonText);

                // Sortieren der Events nach Datum aufsteigend
                Event[] sortedEvents = events.OrderBy(e => e.Date != ""
                    ? DateTime.ParseExact(e.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture)
                    : DateTime.MaxValue).ToArray();

                return sortedEvents;
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");

            }
        }

        // Fügt ein neues Event zur JSON-Datei hinzu
        public async Task AddEvent(Event newEvent)
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/Events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

                // Hinzufügen des neuen Events zur Liste
                events.Add(newEvent);

                // Serialisieren der aktualisierten Liste in JSON
                string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                // Asynchrones zurückschreiben des JSON-Texts in die Datei
                await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Hinzufügen des Events: {ex.Message}");
            }
        }

        // Entfernt ein Event aus der JSON-Datei
        public async Task RemoveEvent(string id)
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

                // Suchen nach dem Event mit der übergebenen ID
                Event eventToRemove = events.FirstOrDefault(e => e.Id == id);

                // Falls das Event gefunden wurde, entfernen aus der Liste
                if (eventToRemove != null)
                {
                    events.Remove(eventToRemove);

                    // Serialisieren der aktualisierten Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Asynchrones zurückschreiben des JSON-Texts in die Datei
                    await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                }
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Entfernen des Events: {ex.Message}");
            }
        }
    }
}
