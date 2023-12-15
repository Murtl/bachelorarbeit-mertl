using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Typen;

namespace webapi.Controllers;

// Die Klasse OrganiserController repräsentiert die REST-Schnittstelle für Organisatoren
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Task.Create")]
public class OrganiserController : ControllerBase
{
    // Die Methode GetAllEvents liefert alle Events aus der JSON-Datenbank zurück
    [HttpGet("GetAllEvents")]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/Events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            return Ok(events);
        }
        catch (Exception ex)
        {
            // Werfen einer Fehlermeldung, falls ein Fehler auftritt
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Die Methode AddEvent fügt ein neues Event in der JSON-Datenbank hinzu
    [HttpPost("AddEvent")]
    public async Task<IActionResult> AddEvent([FromBody] Event newEvent)
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/Events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Hinzufügen des neuen Events zur Liste
            events.Add(newEvent);

            // Serialisieren der aktualisierten Liste in JSON
            string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

            // Asynchrones zurückschreiben des JSON-Texts in die Datei
            await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

            return CreatedAtAction(nameof(AddEvent), newEvent);
        }
        catch (Exception ex)
        {
            // Werfen einer Fehlermeldung, falls ein Fehler auftritt
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Die Methode RemoveEvent entfernt ein Event aus der JSON-Datenbank
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveEvent(string id)
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
            Event eventToRemove = events.FirstOrDefault(e => e.Id == id);

            // Falls das Event gefunden wurde, entfernen aus der Liste
            if (eventToRemove != null)
            {
                events.Remove(eventToRemove);

                // Serialisieren der aktualisierten Liste in JSON
                string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                // Asynchrones zurückschreiben des JSON-Texts in die Datei
                await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

                return Ok("Event erfolgreich entfernt.");
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
