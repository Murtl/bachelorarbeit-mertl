using fullstack.Typen;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Globalization;

namespace fullstack.Services
{
    [Authorize(Roles = "Task.Create")]
    public class OrganiserService
    {
        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/Events.json";

                // Lese den JSON-Text asynchron aus der Datei
                string jsonText = await File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein Array von Event-Objekten
                var events = JsonConvert.DeserializeObject<Event[]>(jsonText);

                // Sortiere die Events nach Datum aufsteigend und behandele leere Werte
                var sortedEvents = events.OrderBy(e => e.Date != ""
                    ? DateTime.ParseExact(e.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture)
                    : DateTime.MaxValue);

                return sortedEvents;
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");

            }
        }

        public async Task AddEvent(Event newEvent)
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/Events.json";

                // Lese den vorhandenen JSON-Text asynchron aus der Datei
                string jsonText = await File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein List<Event>
                var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

                // Füge das neue Event zur Liste hinzu
                events.Add(newEvent);

                // Serialisiere die aktualisierte Liste in JSON
                string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                // Schreibe den JSON-Text asynchron zurück in die Datei
                await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Hinzufügen des Events: {ex.Message}");
            }
        }
        public async Task RemoveEvent(string id)
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/events.json";

                // Lese den vorhandenen JSON-Text asynchron aus der Datei
                string jsonText = await File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein List<Event>
                var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

                // Suche das Event asynchron anhand seiner ID
                var eventToRemove = events.FirstOrDefault(e => e.Id == id);

                // Falls das Event gefunden wurde, entferne es aus der Liste
                if (eventToRemove != null)
                {
                    events.Remove(eventToRemove);

                    // Serialisiere die aktualisierte Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Schreibe den JSON-Text asynchron zurück in die Datei
                    await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                }
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Entfernen des Events: {ex.Message}");
            }
        }
    }
}
