using fullstack.Typen;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Globalization;
using Formatting = Newtonsoft.Json.Formatting;

namespace fullstack.Services;

// Der ParticipantService ist für die Verwaltung der Participants der Events zuständig und erfordert die Rolle "Task.Apply"
[Authorize(Roles = "Task.Apply")]
public class ParticipantService
{

    // Liefert alle Events ohne Teilnehmer aus der JSON-Datei zurück
    public async Task<IEnumerable<Event>> GetEventsWithoutParticipants()
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/Events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Überschreiben aller Events in der Liste indem alle Participants gelöscht werden
            List<Event> eventsWithoutParticipants = events.Select(e =>
            {
                e.Participants = new List<string>();
                return e;
            }).ToList();

            return SortEventsByDate(eventsWithoutParticipants);
        }
        catch (Exception ex)
        {
            // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
            throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");
        }
    }

    // Fügt einen Participant zu einem Event hinzu
    public async Task AddParticipant(string eventId, string participantName)
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await File.ReadAllTextAsync(jsonFilePath);

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
                    await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
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
        }
        catch (Exception ex)
        {
            // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
            throw new Exception($"Fehler beim Hinzufügen des Teilnehmers: {ex.Message}");
        }
    }

    // Liefert alle Events für einen Participant aus der JSON-Datei zurück
    public async Task<IEnumerable<Event>> GetEventsByParticipant(string participantName)
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await File.ReadAllTextAsync(jsonFilePath);

            // Deserialisieren des JSON-Texts in eine List<Event>
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(jsonText);

            // Filteren der Events für den angegebenen Participant
            List<Event> eventsForParticipant = events.Where(e => e.Participants.Contains(participantName)).ToList();

            // Überschreiben aller Events in der Liste indem alle Participants gelöscht werden
            eventsForParticipant.ForEach(e => e.Participants.Clear());

            return SortEventsByDate(eventsForParticipant);
        }
        catch (Exception ex)
        {
            // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
            throw new Exception($"Fehler beim Holen der Events für einen Teilnehmer: {ex.Message}");
        }
    }

    public async Task RemoveParticipant(string eventId, string participantName)
    {
        try
        {
            // Pfad zur JSON-Datenbank
            string jsonFilePath = "Database/events.json";

            // Asynchnrones Auslesen der JSON-Datenbank
            string jsonText = await File.ReadAllTextAsync(jsonFilePath);

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
                    await File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                }
            }
        }
        catch (Exception ex)
        {
            // Werfen einer Expcetion mit einer Fehlermeldung, falls ein Fehler auftritt
            throw new Exception($"Fehler beim Entfernen des Teilnehmers in dem Event: {ex.Message}");
        }
    }


    // Die Methode SortEventsByDate() sortiert die Events nach Datum aufsteigend
    private IEnumerable<Event> SortEventsByDate(IEnumerable<Event> events)
    {
        try
        {
            // Sortieren der Events nach Datum aufsteigend
            List<Event> sortedEvents = events.OrderBy(e => e.Date != null
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
