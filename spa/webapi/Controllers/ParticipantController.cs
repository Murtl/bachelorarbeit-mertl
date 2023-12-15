using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Typen;

namespace webapi.Controllers;

// Die Klasse ParticipantController repräsentiert die REST-Schnittstelle für Teilnehmer
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Task.Apply")]
public class ParticipantController : ControllerBase
{
    // Die Methode GetEventsWithoutParticipants liefert alle Events ohne Participants aus der JSON-Datenbank zurück
    [HttpGet("GetEventsWithoutParticipants")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEventsWithoutParticipants()
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/Events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Projizieren der Events auf eine neue Liste ohne das Feld Participants
            List<Event> eventsWithoutParticipants = events.Select(e => new Event
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Time = e.Time,
                Description = e.Description
            }).ToList();

            return Ok(eventsWithoutParticipants);
        }
        catch (Exception ex)
        {
            // Werfen einer Fehlermeldung, falls ein Fehler auftritt
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Die Methode AddParticipant fügt einen Participant zu einem Event in der JSON-Datenbank hinzu
    [HttpPost("{eventId}/AddParticipant")]
    public async Task<IActionResult> AddParticipant(string eventId, [FromBody] string participantName)
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Suchen nach dem Event mit der übergebenen ID
            Event eventToAddParticipant = events.FirstOrDefault(e => e.Id == eventId);

            if (eventToAddParticipant != null)
            {
                // Überprüfen, ob der Participant bereits existiert
                if (!eventToAddParticipant.Participants.Contains(participantName))
                {
                    // Hinzufügen des Participants zu dem Event
                    eventToAddParticipant.Participants.Add(participantName);

                    // Serialisieren der aktualisierten Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Asynchrones zurückschreiben des JSON-Texts in die Datei
                    await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

                    return Ok("Participant erfolgreich hinzugefügt.");
                }
                else
                {
                    // Werfen einer Fehlermeldung, falls der Participant bereits existiert
                    return BadRequest("Participant existiert bereits im Event.");
                }
            }
            else
            {
                // Werfen einer Fehlermeldung, falls das Event nicht gefunden wurde
                return NotFound("Event nicht gefunden.");
            }
        }
        catch (Exception ex)
        {
            // Werfen einer Fehlermeldung, falls ein Fehler auftritt
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Die Methode GetEventsByParticipant liefert alle Events für einen Participant aus der JSON-Datenbank zurück
    [HttpGet("GetEventsByParticipant/{participantName}")]
    public async Task<IActionResult> GetEventsByParticipant(string participantName)
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Filteren der Events für den angegebenen Participant
            List<Event> eventsForParticipant = events.Where(e => e.Participants.Contains(participantName)).ToList();

            // Projizieren der Events auf eine neue Liste ohne das Feld Participants
            List<Event> eventsWithoutParticipants = eventsForParticipant.Select(e => new Event
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Time = e.Time,
                Description = e.Description
            }).ToList();

            return Ok(eventsWithoutParticipants);
        }
        catch (Exception ex)
        {
            // Werfen einer Fehlermeldung, falls ein Fehler auftritt
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Die Methode RemoveParticipant entfernt einen Participant aus einem Event in der JSON-Datenbank
    [HttpDelete("{eventId}/RemoveParticipant")]
    public async Task<IActionResult> RemoveParticipant(string eventId, [FromBody] string participantName)
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Suchen des Events anhand seiner ID
            Event eventToRemoveParticipant = events.FirstOrDefault(e => e.Id == eventId);

            if (eventToRemoveParticipant != null)
            {
                // Überprüfen, ob der Participant bei dem Event existiert
                if (eventToRemoveParticipant.Participants.Contains(participantName))
                {
                    // Entfernen des Participants aus der Liste
                    eventToRemoveParticipant.Participants.Remove(participantName);

                    // Serialisieren der aktualisierten Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Asynchrones zurückschreiben des JSON-Texts in die Datei
                    await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

                    return Ok("Participant erfolgreich entfernt.");
                }
                else
                {
                    // Werfen einer Fehlermeldung, falls der Participant nicht im Event vorhanden ist
                    return BadRequest("Participant nicht im Event vorhanden.");
                }
            }
            else
            {
                // Werfen einer Fehlermeldung, falls das Event nicht gefunden wurde
                return NotFound("Event nicht gefunden.");
            }
        }
        catch (Exception ex)
        {
            // Werfen einer Fehlermeldung, falls ein Fehler auftritt
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
