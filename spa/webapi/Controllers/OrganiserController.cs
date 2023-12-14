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
        // Pfade und Dateinamen entsprechend deiner Struktur anpassen
        string jsonFilePath = "Database/Events.json";

        try
        {
            // Lese den JSON-Text aus der Datei
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

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

    // Die Methode AddEvent fügt ein neues Event in der JSON-Datenbank hinzu
    [HttpPost("AddEvent")]
    public async Task<IActionResult> AddEvent([FromBody] Event newEvent)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/Events.json";
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Füge das neue Event zur Liste hinzu
            events.Add(newEvent);

            // Serialisiere die aktualisierte Liste in JSON
            string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

            // Schreibe den JSON-Text zurück in die Datei
            await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

            // Gib das hinzugefügte Event als HTTP POST-Antwort zurück
            return CreatedAtAction(nameof(AddEvent), newEvent);
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Die Methode RemoveEvent entfernt ein Event aus der JSON-Datenbank
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveEvent(string id)
    {
        try
        {
            // Lese den vorhandenen JSON-Text aus der Datei
            string jsonFilePath = "Database/events.json";
            string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

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
                await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);

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
}
