using fullstack.Typen;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace fullstack.Services;

[Authorize(Roles = "Task.Apply")]
public class ParticipantService
{

    public async Task<IEnumerable<Event>> GetEventsWithoutParticipants()
    {
        try
        {
            // Pfade und Dateinamen entsprechend deiner Struktur anpassen
            string jsonFilePath = "Database/Events.json";

            // Lese den JSON-Text asynchron aus der Datei
            string jsonText = await File.ReadAllTextAsync(jsonFilePath);

            // Deserialisiere den JSON-Text in ein Array von Event-Objekten
            var events = JsonConvert.DeserializeObject<Event[]>(jsonText);

            // Überschreibe alle Events in der Liste indem alle Participants gelöscht werden
            var eventsWithoutParticipants = events.Select(e =>
            {
                e.Participants = new List<string>();
                return e;
            });

            return events;
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");
        }
    }

    public async Task AddParticipant(string eventId, string participantName)
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

                    // Schreibe den JSON-Text asynchron zurück in die Datei
                    await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                }
                else
                {
                    throw new Exception("Teilnehmer bereits beim Event angemeldet!");
                }
            }
            else
            {
                throw new Exception("Event nicht gefunden!");
            }
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            throw new Exception($"Fehler beim Hinzufügen des Teilnehmers: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Event>> GetEventsByParticipant(string participantName)
    {
        try
        {
            // Pfade und Dateinamen entsprechend deiner Struktur anpassen
            string jsonFilePath = "Database/events.json";

            // Lese den vorhandenen JSON-Text asynchron aus der Datei
            string jsonText = await File.ReadAllTextAsync(jsonFilePath);

            // Deserialisiere den JSON-Text in ein List<Event>
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Filtere die Events für den angegebenen Participant
            var eventsForParticipant = events.Where(e => e.Participants.Contains(participantName)).ToList();

            // Überschreibe alle Events in der Liste indem alle Participants gelöscht werden
            eventsForParticipant.ForEach(e => e.Participants.Clear());

            // Gib die gefilterten Events zurück
            return eventsForParticipant;
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            // Du könntest auch einfach einen leeren List<Event> zurückgeben oder null
            throw new Exception($"Fehler beim Holen der Events für einen Teilnehmer: {ex.Message}");
        }
    }

    public async Task RemoveParticipant(string eventId, string participantName)
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

                    // Schreibe den JSON-Text asynchron zurück in die Datei
                    await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                }
            }
        }
        catch (Exception ex)
        {
            // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
            throw new Exception($"Fehler beim Entfernen des Teilnehmers in dem Event: {ex.Message}");
        }
    }
}
