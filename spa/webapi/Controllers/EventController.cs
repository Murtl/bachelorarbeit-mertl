using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Typen;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetEvents()
    {
        // Pfade und Dateinamen entsprechend deiner Struktur anpassen
        string jsonFilePath = "Database/Events.json";

        try
        {
            // Lese den JSON-Text aus der Datei
            string jsonText = System.IO.File.ReadAllText(jsonFilePath);

            // Deserialisiere den JSON-Text in ein Array von Event-Objekten
            var events = JsonConvert.DeserializeObject<Event[]>(jsonText);

            // Gib die Events als HTTP GET-Antwort zurück
            return Ok(events);
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [Route("AddEvent")]
    public IActionResult AddEvent([FromBody] Event newEvent)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/Events.json";
            string jsonText = System.IO.File.ReadAllText(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Füge das neue Event zur Liste hinzu
            events.Add(newEvent);

            // Serialisiere die aktualisierte Liste in JSON
            string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

            // Schreibe den JSON-Text zurück in die Datei
            System.IO.File.WriteAllText(jsonFilePath, updatedJsonText);

            // Gib das hinzugefügte Event als HTTP POST-Antwort zurück
            return CreatedAtAction(nameof(AddEvent), newEvent);
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult RemoveEvent(string id)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/events.json";
            string jsonText = System.IO.File.ReadAllText(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Suche das Event anhand seiner ID
            var eventToRemove = events.FirstOrDefault(e => e.Id == id);

            // Falls das Event gefunden wurde, entferne es aus der Liste
            if (eventToRemove != null)
            {
                events.Remove(eventToRemove);

                // Serialisiere die aktualisierte Liste in JSON
                string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                // Schreibe den JSON-Text zurück in die Datei
                System.IO.File.WriteAllText(jsonFilePath, updatedJsonText);

                // Gib eine Erfolgsantwort zurück
                return Ok("Event erfolgreich entfernt.");
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

    [HttpPost("{eventId}/AddParticipant")]
    public IActionResult AddParticipant(string eventId, [FromBody] string participantName)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/events.json";
            string jsonText = System.IO.File.ReadAllText(jsonFilePath);

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
                    System.IO.File.WriteAllText(jsonFilePath, updatedJsonText);

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
    public IActionResult GetEventsByParticipant(string participantName)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/events.json";
            string jsonText = System.IO.File.ReadAllText(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Filtere die Events für den angegebenen Participant
            var eventsForParticipant = events.Where(e => e.Participants.Contains(participantName)).ToList();

            // Gib die gefilterten Events zurück
            return Ok(eventsForParticipant);
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
