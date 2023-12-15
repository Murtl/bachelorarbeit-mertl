using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mpa.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace mpa.Controllers
{
    // Der OrganiserController ist für die Verwaltung der Events zuständig und erfordert die Rolle "Task.Create"
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

        // Die Methode GetAllEvents() liest alle Events aus der JSON-Datei aus
        private async Task<IEnumerable<EventModel>> GetAllEvents()
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/Events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<EventModel> events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                return events;
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");
            }
        }

        // Die Methode AddEvent() fügt ein neues Event in der JSON-Datei hinzu
        [HttpPost]
        public async Task<IActionResult> AddEvent(EventModel newEvent)
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/Events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<EventModel> events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Hinzufügen des neuen Events zur Liste mit neuer ID und Platzhalter für die Participants
                newEvent.Id = Guid.NewGuid().ToString();
                newEvent.Participants = new List<string>();
                events.Add(newEvent);

                // Serialisieren der aktualisierten Liste in JSON
                string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                // Asynchrones zurückschreiben des JSON-Texts in die Datei
                await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

                return RedirectToAction("NewEvents");
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Hinzufügen des Events: {ex.Message}");
            }
        }

        // Die Methode RemoveEvent() entfernt ein Event aus der JSON-Datei
        [HttpDelete]
        public async Task<IActionResult> RemoveEvent(string id)
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<EventModel> events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Suchen nach dem Event mit der übergebenen ID
                EventModel eventToRemove = events.FirstOrDefault(e => e.Id == id);

                // Falls das Event gefunden wurde, entfernen aus der Liste
                if (eventToRemove != null)
                {
                    events.Remove(eventToRemove);

                    // Serialisieren der aktualisierten Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Asynchrones zurückschreiben des JSON-Texts in die Datei
                    await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                }

                return RedirectToAction("NewEvents");
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Entfernen des Events: {ex.Message}");
            }
        }

        // Die Methode SortEventsByDate() sortiert die Events nach Datum aufsteigend
        private IEnumerable<EventModel> SortEventsByDate(IEnumerable<EventModel> events)
        {
            try
            {
                // Sortieren der Events nach Datum aufsteigend
                List<EventModel> sortedEvents = events.OrderBy(e => e.Date != null
                    ? DateTime.ParseExact(e.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture)
                    : DateTime.MaxValue).ToList();

                return sortedEvents;
            }catch(Exception)
            {
                Console.WriteLine("Events konnten nicht sortiert werden, da eine fehlerhafte Eingabe für das Datum vorliegt in einem Event!");
                return events;
            }
        }
    }
}
