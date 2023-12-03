using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mpa.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace mpa.Controllers
{
    [Authorize(Roles = "Task.Create")]
    public class OrganiserController : Controller
    {
        public async Task<IActionResult> NewEvents()
        {
            ViewData["EventData"] = SortEventsByDate(await GetAllEvents());
            return View();
        }

        public async Task<IActionResult> CheckEvents()
        {
            ViewData["EventData"] = SortEventsByDate(await GetAllEvents());
            return View();
        }

        private async Task<IEnumerable<EventModel>> GetAllEvents()
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/Events.json";

                // Lese den JSON-Text asynchron aus der Datei
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein Array von Event-Objekten
                var events = JsonConvert.DeserializeObject<EventModel[]>(jsonText);

                return events;
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEvent(EventModel newEvent)
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/Events.json";

                // Lese den vorhandenen JSON-Text asynchron aus der Datei
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein List<Event>
                var events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Füge das neue Event zur Liste hinzu
                newEvent.Id = Guid.NewGuid().ToString();
                newEvent.Participants = new List<string>();
                events.Add(newEvent);

                // Serialisiere die aktualisierte Liste in JSON
                string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                // Schreibe den JSON-Text asynchron zurück in die Datei
                await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

                return RedirectToAction("NewEvents");
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Hinzufügen des Events: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveEvent(string id)
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/events.json";

                // Lese den vorhandenen JSON-Text asynchron aus der Datei
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein List<Event>
                var events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Suche das Event asynchron anhand seiner ID
                var eventToRemove = events.FirstOrDefault(e => e.Id == id);

    
                // Falls das Event gefunden wurde, entferne es aus der Liste
                if (eventToRemove != null)
                {
                    events.Remove(eventToRemove);

                    // Serialisiere die aktualisierte Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Schreibe den JSON-Text asynchron zurück in die Datei
                    await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                }

                return RedirectToAction("NewEvents");
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Entfernen des Events: {ex.Message}");
            }
        }

        private IEnumerable<EventModel> SortEventsByDate(IEnumerable<EventModel> events)
        {
            // Sortiere die Events nach Datum aufsteigend und behandele Null-Werte
            var sortedEvents = events.OrderBy(e => e.Date != null
                ? DateTime.ParseExact(e.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture)
                : DateTime.MaxValue);

            return sortedEvents;
        }
    }
}
