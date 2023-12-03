using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Typen;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Task.Apply")]
public class ParticipantController : ControllerBase
{
    [HttpGet("GetEventsWithoutParticipants")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEventsWithoutParticipants()
    {
        // Pfade und Dateinamen entsprechend deiner Struktur anpassen
        string jsonFilePath = "Database/Events.json";

        try
        {
            // Lese den JSON-Text aus der Datei
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisiere den JSON-Text in ein Array von Event-Objekten
            var events = JsonConvert.DeserializeObject<Event[]>(jsonText);

            // Projiziere die Events auf eine neue Klasse ohne das Feld Participants
            var eventsWithoutParticipants = events.Select(e => new Event
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Time = e.Time,
                Description = e.Description
            }).ToArray();

            // Gib die Events als HTTP GET-Antwort zurück
            return Ok(eventsWithoutParticipants);
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{eventId}/AddParticipant")]
    public async Task<IActionResult> AddParticipant(string eventId, [FromBody] string participantName)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/events.json";
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Suche das Event anhand seiner ID
            var eventToAddParticipant = events.FirstOrDefault(e => e.Id == eventId);

            // Falls das Event gefunden wurde, füge den Participant hinzu
            if (eventToAddParticipant != null)
            {
                // Überprüfe, ob der Participant bereits existiert
                if (!eventToAddParticipant.Participants.Contains(participantName))
                {
                    // Füge den Participant zur Liste hinzu
                    eventToAddParticipant.Participants.Add(participantName);

                    // Serialisiere die aktualisierte Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Schreibe den JSON-Text zurück in die Datei
                    await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

                    // Gib eine Erfolgsantwort zurück
                    return Ok("Participant erfolgreich hinzugefügt.");
                }
                else
                {
                    return BadRequest("Participant existiert bereits im Event.");
                }
            }
            else
            {
                // Falls das Event nicht gefunden wurde, gib einen Fehler zurück
                return NotFound("Event nicht gefunden.");
            }
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("GetEventsByParticipant/{participantName}")]
    public async Task<IActionResult> GetEventsByParticipant(string participantName)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/events.json";
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Filtere die Events für den angegebenen Participant
            var eventsForParticipant = events.Where(e => e.Participants.Contains(participantName)).ToList();

            // Projiziere die Events auf eine neue Klasse ohne das Feld Participants
            var eventsWithoutParticipants = eventsForParticipant.Select(e => new Event
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Time = e.Time,
                Description = e.Description
            }).ToArray();

            // Gib die Events als HTTP GET-Antwort zurück
            return Ok(eventsWithoutParticipants);
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{eventId}/RemoveParticipant")]
    public async Task<IActionResult> RemoveParticipant(string eventId, [FromBody] string participantName)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/events.json";
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Suche das Event anhand seiner ID
            var eventToRemoveParticipant = events.FirstOrDefault(e => e.Id == eventId);

            // Falls das Event gefunden wurde, entferne den Participant
            if (eventToRemoveParticipant != null)
            {
                // Überprüfe, ob der Participant existiert
                if (eventToRemoveParticipant.Participants.Contains(participantName))
                {
                    // Entferne den Participant aus der Liste
                    eventToRemoveParticipant.Participants.Remove(participantName);

                    // Serialisiere die aktualisierte Liste in JSON
                    string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                    // Schreibe den JSON-Text zurück in die Datei
                    await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

                    // Gib eine Erfolgsantwort zurück
                    return Ok("Participant erfolgreich entfernt.");
                }
                else
                {
                    return BadRequest("Participant nicht im Event vorhanden.");
                }
            }
            else
            {
                // Falls das Event nicht gefunden wurde, gib einen Fehler zurück
                return NotFound("Event nicht gefunden.");
            }
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
