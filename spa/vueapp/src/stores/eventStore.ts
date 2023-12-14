import { defineStore } from "pinia";
import { ref } from "vue";
import type { Ref } from "vue";
import type { EventType } from "@/utils/eventType";
import ApiService from "@/auth/ApiService";

/**
 * @description Store für die Events
 */
export const useEventStore = defineStore("eventStore", () => {
  const events: Ref<EventType[]> = ref([]);

  /**
   * @description Holt alle Events vom Backend
   */
  async function getEvents() {
    try {
      const response = await ApiService.get("/api/Organiser/GetAllEvents");
      events.value = response.data;
      sortEventsByDate();
    } catch (e: any) {
      console.log(e);
    }
  }

  /**
   * @description Holt alle Events vom Backend ohne die Teilnehmer
   */
  async function getEventsWithoutParticipants() {
    try {
      const response = await ApiService.get(
        "/api/Participant/GetEventsWithoutParticipants",
      );
      events.value = response.data;
      sortEventsByDate();
    } catch (e: any) {
      console.log(e);
    }
  }

  /**
   * @description Fügt ein Event hinzu
   * @param event Das Event, das hinzugefügt werden soll
   */
  async function addEvent(event: EventType) {
    try {
      await ApiService.post("/api/Organiser/AddEvent", event);
      events.value.push(event);
      sortEventsByDate();
    } catch (e: any) {
      console.log(e);
    }
  }

  /**
   * @description Löscht ein Event
   * @param eventId Die ID des Events, das gelöscht werden soll
   */
  async function removeEvent(eventId: string) {
    try {
      await ApiService.delete(`/api/Organiser/${eventId}`);
      events.value = events.value.filter((event) => event.id !== eventId);
    } catch (e: any) {
      console.log(e);
    }
  }

  /**
   * @description Fügt einen Teilnehmer zu einem Event hinzu
   * @param eventId Die ID des Events, zu dem der Teilnehmer hinzugefügt werden soll
   * @param participant Der Teilnehmer, der hinzugefügt werden soll
   */
  async function addParticipant(eventId: string, participant: string) {
    try {
      await ApiService.post(
        `/api/Participant/${eventId}/AddParticipant`,
        participant,
      );
      const event = events.value.find((event) => event.id === eventId);
      if (event) {
        event.participants?.push(participant);
      }
      alert("Du wurdest erfolgreich angemeldet!");
    } catch (e: any) {
      console.log(e);
      alert("Du bist bereits angemeldet!");
    }
  }

  /**
   * @description Holt alle Events eines Teilnehmers
   * @param participant Der Teilnehmer, dessen Events geholt werden sollen
   */
  async function getEventsOfParticipant(
    participant: string,
  ): Promise<EventType[]> {
    try {
      const response = await ApiService.get(
        `/api/Participant/GetEventsByParticipant/${participant}`,
      );
      return response.data;
    } catch (e: any) {
      console.log(e);
    }
    return events.value.filter(
      (event) => event.participants?.includes(participant),
    );
  }

  /**
   * @description Entfernt einen Teilnehmer von einem Event
   * @param eventId Die ID des Events, von dem der Teilnehmer entfernt werden soll
   * @param participant Der Teilnehmer, der entfernt werden soll
   */
  async function removeParticipant(eventId: string, participant: string) {
    try {
      await ApiService.delete(`/api/Participant/${eventId}/RemoveParticipant`, {
        data: participant,
      });
      alert("Du wurdest erfolgreich abgemeldet!");
    } catch (e: any) {
      console.log(e);
    }
  }

  /**
   * @description Sortiert die Events nach Datum
   */
  function sortEventsByDate() {
    events.value.sort((a, b) => {
      const dateA = a.date.split(".").reverse().join();
      const dateB = b.date.split(".").reverse().join();
      return dateA.localeCompare(dateB);
    });
  }

  return {
    events,
    addEvent,
    removeEvent,
    getEvents,
    addParticipant,
    getEventsOfParticipant,
    removeParticipant,
    getEventsWithoutParticipants,
  };
});
