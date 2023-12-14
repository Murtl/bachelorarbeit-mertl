<script setup lang="ts">
import { ref, watch } from "vue";
import { useEventStore } from "@/stores/eventStore";
import EventSegment from "@/components/EventSegment.vue";
import { storeToRefs } from "pinia";

const eventStore = useEventStore();
const { events } = storeToRefs(eventStore);
const filteredEvents = ref(eventStore.events);
const eventFilter = ref("");

watch(eventFilter, () => {
  filterEvents();
});

watch(events, (newValue) => {
  filteredEvents.value = newValue;
  filterEvents();
});

/**
 * @description filtert die Events nach dem Namen
 */
function filterEvents() {
  if (eventFilter.value === "") {
    filteredEvents.value = eventStore.events;
  } else {
    filteredEvents.value = eventStore.events.filter((event) => {
      return event.name.toLowerCase().includes(eventFilter.value.toLowerCase());
    });
  }
}
</script>

<template>
  <section class="header-section">
    <BFormInput placeholder="Filtern nach Event..." v-model="eventFilter" />
  </section>
  <section
    class="main-section"
    v-for="(event, index) in filteredEvents"
    :key="index"
  >
    <EventSegment
      :id="event.id"
      :name="event.name"
      :date="event.date"
      :time="event.time"
      :description="event.description"
      button-type="Add"
    />
  </section>
</template>

<style scoped>
.header-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
  gap: 10px;
  width: 70%;
}

.main-section {
  width: 70%;
}
</style>
