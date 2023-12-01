﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mpa.Models;
using Newtonsoft.Json;

namespace mpa.Controllers
{
    [Authorize(Roles = "Task.Apply")]
    public class ParticipantController : Controller
    {
        public async Task<IActionResult> ApplyEvents()
        {
            ViewData["EventData"] = await GetAllEventsForParticipant();
            return View();
        }

        public async Task<IActionResult> CheckAppliedEvents()
        {
            ViewData["EventData"] = await GetEventsByParticipant(User.Identity.Name);
            return View();
        }

        private async Task<IEnumerable<EventModel>> GetAllEventsForParticipant()
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/Events.json";

                // Lese den JSON-Text asynchron aus der Datei
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein Array von Event-Objekten
                var events = JsonConvert.DeserializeObject<EventModel[]>(jsonText);

                // Projiziere die Events auf eine neue Klasse ohne das Feld Participants
                var eventsWithoutParticipants = events.Select(e => new EventModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    Time = e.Time,
                    Description = e.Description
                }).ToArray();

                return eventsWithoutParticipants;
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Abrufen der Events: {ex.Message}");
            }
        }

        public async Task<IEnumerable<EventModel>> GetEventsByParticipant(string participantName)
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/events.json";

                // Lese den vorhandenen JSON-Text asynchron aus der Datei
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein List<Event>
                var events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Filtere die Events für den angegebenen Participant
                var eventsForParticipant = events.Where(e => e.Participants.Contains(participantName)).ToList();

                // Projiziere die Events auf eine neue Klasse ohne das Feld Participants
                var eventsWithoutParticipants = eventsForParticipant.Select(e => new EventModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    Time = e.Time,
                    Description = e.Description
                }).ToArray();

                // Gib die gefilterten Events zurück
                return eventsWithoutParticipants;
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                // Du könntest auch einfach einen leeren List<Event> zurückgeben oder null
                throw new Exception($"Fehler beim Holen der Events für einen Teilnehmer: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipant(string eventId)
        {
            try
            {
                // Pfade und Dateinamen entsprechend deiner Struktur anpassen
                string jsonFilePath = "Database/events.json";

                // Lese den vorhandenen JSON-Text asynchron aus der Datei
                string jsonText = await System.IO.File.ReadAllTextAsync(jsonFilePath);

                // Deserialisiere den JSON-Text in ein List<Event> asynchron
                var events = JsonConvert.DeserializeObject<List<EventModel>>(jsonText);

                // Suche das Event asynchron anhand seiner ID
                var eventToAddParticipant = events.FirstOrDefault(e => e.Id == eventId);

                // Falls das Event gefunden wurde, füge den Participant hinzu
                if (eventToAddParticipant != null)
                {
                    // Überprüfe, ob der Participant bereits existiert
                    if (!eventToAddParticipant.Participants.Contains(User.Identity.Name))
                    {
                        // Füge den Participant zur Liste hinzu
                        eventToAddParticipant.Participants.Add(User.Identity.Name);

                        // Serialisiere die aktualisierte Liste in JSON
                        string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                        // Schreibe den JSON-Text asynchron zurück in die Datei
                        await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
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

                return RedirectToAction("ApplyEvents");
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Hinzufügen des Teilnehmers: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveParticipant(string eventId)
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
                var eventToRemoveParticipant = events.FirstOrDefault(e => e.Id == eventId);

                // Falls das Event gefunden wurde, entferne den Participant
                if (eventToRemoveParticipant != null)
                {
                    // Überprüfe, ob der Participant existiert
                    if (eventToRemoveParticipant.Participants.Contains(User.Identity.Name))
                    {
                        // Entferne den Participant aus der Liste
                        eventToRemoveParticipant.Participants.Remove(User.Identity.Name);

                        // Serialisiere die aktualisierte Liste in JSON
                        string updatedJsonText = JsonConvert.SerializeObject(events, Formatting.Indented);

                        // Schreibe den JSON-Text asynchron zurück in die Datei
                        await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJsonText);
                    }
                }
                return RedirectToAction("CheckAppliedEvents");
            }
            catch (Exception ex)
            {
                // Behandele Fehler, z.B., wenn die Datei nicht gefunden wird
                throw new Exception($"Fehler beim Entfernen des Teilnehmers in dem Event: {ex.Message}");
            }
        }
    }
}
