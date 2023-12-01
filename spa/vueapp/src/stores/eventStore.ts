import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Ref } from 'vue'
import type {EventType} from "@/utils/eventType";
import ApiService from "@/auth/ApiService";

/**
 * @description Store for user data
 */
export const useEventStore = defineStore('eventStore', () => {
    const events: Ref<EventType[]> = ref([])

    async function getEvents() {
        try {
            const response = await ApiService.get("/api/Event/GetAllEvents");
            events.value = response.data;
        } catch (e: any) {
            console.log(e);
        }
    }

    async function getEventsWithoutParticipants() {
        try {
            const response = await ApiService.get("/api/Event/GetEventsWithoutParticipants");
            events.value = response.data;
        } catch (e: any) {
            console.log(e);
        }
    }

    async function addEvent(event: EventType) {
        try {
            await ApiService.post("/api/Event/AddEvent", event);
            events.value.push(event)
        } catch (e: any) {
            console.log(e);
        }
    }

    async function removeEvent(eventId: string) {
        try {
            await ApiService.delete(`/api/Event/${eventId}`);
            events.value = events.value.filter(event => event.id !== eventId)
        } catch (e: any) {
            console.log(e);
        }
    }

    async function addParticipant(eventId: string, participant: string) {
        try {
            await ApiService.post(`/api/Event/${eventId}/AddParticipant`, participant);
            const event = events.value.find(event => event.id === eventId)
            if (event) {
                event.participants?.push(participant)
            }
            alert("Du wurdest erfolgreich angemeldet!");
        } catch (e: any) {
            console.log(e);
            alert("Du bist bereits angemeldet!");
        }
    }

    async function getEventsOfParticipant(participant: string): Promise<EventType[]> {
        try {
            const response = await ApiService.get(`/api/Event/GetEventsByParticipant/${participant}`);
            return response.data;
        } catch (e: any) {
            console.log(e);
        }
        return events.value.filter(event => event.participants?.includes(participant))
    }

    async function removeParticipant(eventId: string, participant: string) {
        try {
            await ApiService.delete(`/api/Event/${eventId}/RemoveParticipant`, { data: participant });
            alert("Du wurdest erfolgreich abgemeldet!");
        } catch (e: any) {
            console.log(e);
        }
    }

    return { events, addEvent, removeEvent, getEvents, addParticipant, getEventsOfParticipant, removeParticipant, getEventsWithoutParticipants }
})
