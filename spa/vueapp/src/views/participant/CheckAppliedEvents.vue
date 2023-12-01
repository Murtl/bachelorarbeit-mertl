<script setup lang="ts">
import {useEventStore} from "@/stores/eventStore";
import EventSegment from "@/components/EventSegment.vue";
import {useUserStore} from "@/stores/userStore";
import {onBeforeMount, onMounted,ref} from "vue";
import type { Ref } from "vue";
import type {EventType} from "@/utils/eventType";

const eventStore = useEventStore();
const appliedEvents: Ref<EventType[]> = ref([]);
const userStore = useUserStore();

onBeforeMount(async () => {
  appliedEvents.value = await eventStore.getEventsOfParticipant(userStore.userName);
})

async function removeParticipantFromEvent(eventId: string) {
  await eventStore.removeParticipant(eventId, userStore.userName);
  appliedEvents.value = appliedEvents.value.filter((event) => {
    return event.id !== eventId;
  });
}

</script>

<template>
  <h4>Hier sind alle deine bevorstehenden Veranstaltungen zu sehen!</h4>
  <section class="main-section" v-for="(event, index) in appliedEvents" :key="index">
    <EventSegment
        :id="event.id"
        :name="event.name"
        :date="event.date"
        :time="event.time"
        :description="event.description"
        button-type="Remove"
        @remove-participant="removeParticipantFromEvent"
    />
  </section>
</template>

<style scoped>
.main-section {
  width: 50%;
}
</style>
