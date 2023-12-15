using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mpa.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace mpa.Controllers
{
    // Der ParticipantController ist für die Verwaltung der Participants der Events zuständig und erfordert die Rolle "Task.Apply"
    [Authorize(Roles = "Task.Apply")]
    public class ParticipantController : Controller
    {
        public async Task<IActionResult> ApplyEvents()
        {
            ViewData["EventData"] = SortEventsByDate(await GetAllEventsForParticipant());
            return View();
        }

        public async Task<IActionResult> CheckAppliedEvents()
        {
            ViewData["EventData"] = SortEventsByDate(await GetEventsByParticipant(User.Identity.Name));
            return View();
        }

        // Die Methode GetAllEvents() liest alle Events aus der JSON-Datei aus und projiziert sie auf eine neue Klasse ohne das Feld Participants
        private async Task<IEnumerable<EventModel>> GetAllEventsForParticipant()
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/Events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<EventModel> events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Projizieren der Events auf eine neue Liste ohne das Feld Participants
                List<EventModel> eventsWithoutParticipants = events.Select(e => new EventModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    Time = e.Time,
                    Description = e.Description
                }).ToList();

                return eventsWithoutParticipants;
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");
            }
        }

        // Die Methode GetEventsByParticipant() liest alle Events aus der JSON-Datei aus und filtert sie nach dem angegebenen Participant
        public async Task<IEnumerable<EventModel>> GetEventsByParticipant(string participantName)
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<EventModel> events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Filteren der Events für den angegebenen Participant
                List<EventModel> eventsForParticipant = events.Where(e => e.Participants.Contains(participantName)).ToList();

                // Projizieren der Events auf eine neue Liste ohne das Feld Participants
                List<EventModel> eventsWithoutParticipants = eventsForParticipant.Select(e => new EventModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    Time = e.Time,
                    Description = e.Description
                }).ToList();

                return eventsWithoutParticipants;
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Holen der Events für einen Teilnehmer: {ex.Message}");
            }
        }

        // Die Methode AddParticipant() fügt einen neuen Participant zu einem Event in der JSON-Datei hinzu
        [HttpPost]
        public async Task<IActionResult> AddParticipant(string eventId)
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
                EventModel eventToAddParticipant = events.FirstOrDefault(e => e.Id == eventId);

                if (eventToAddParticipant != null)
                {
                    // Überprüfen, ob der Participant bereits existiert
                    if (!eventToAddParticipant.Participants.Contains(User.Identity.Name))
                    {
                        // Hinzufügen des Participants zu dem Event
                        eventToAddParticipant.Participants.Add(User.Identity.Name);

                        // Serialisieren der aktualisierten Liste in JSON
                        string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                        // Asynchrones zurückschreiben des JSON-Texts in die Datei
                        await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                    }
                    else
                    {
                        // Falls der Participant bereits existiert, wird eine Exception geworfen
                        throw new Exception("Teilnehmer bereits beim Event angemeldet!");
                    }
                }
                else
                {
                    // Falls das Event nicht gefunden wurde, wird eine Exception geworfen
                    throw new Exception("Event nicht gefunden!");
                }
                return RedirectToAction("ApplyEvents");
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Hinzufügen des Teilnehmers: {ex.Message}");
            }
        }

        // Die Methode RemoveParticipant() entfernt einen Participant aus einem Event in der JSON-Datei
        [HttpDelete]
        public async Task<IActionResult> RemoveParticipant(string eventId)
        {
            try
            {
                // Pfad zur JSON-Datenbank
                string jsonFilePath = "Database/events.json";

                // Asynchnrones Auslesen der JSON-Datenbank
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisieren des JSON-Texts in eine List<Event>
                List<EventModel> events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Suchen des Events anhand seiner ID
                EventModel eventToRemoveParticipant = events.FirstOrDefault(e => e.Id == eventId);

                if (eventToRemoveParticipant != null)
                {
                    // Überprüfen, ob der Participant bei dem Event existiert
                    if (eventToRemoveParticipant.Participants.Contains(User.Identity.Name))
                    {
                        // Entfernen des Participants aus der Liste
                        eventToRemoveParticipant.Participants.Remove(User.Identity.Name);

                        // Serialisieren der aktualisierten Liste in JSON
                        string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                        // Asynchrones zurückschreiben des JSON-Texts in die Datei
                        await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                    }
                }
                return RedirectToAction("CheckAppliedEvents");
            }
            catch (Exception ex)
            {
                // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
                throw new Exception($"Fehler beim Entfernen des Teilnehmers in dem Event: {ex.Message}");
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
            }
            catch (Exception)
            {
                Console.WriteLine("Events konnten nicht sortiert werden, da eine fehlerhafte Eingabe für das Datum vorliegt in einem Event!");
                return events;
            }
        }
    }
}
