<script setup lang="ts">
import {onMounted, ref, watch} from "vue";
import ApiService from "@/auth/ApiService";
import EventSegment from "@/components/EventSegment.vue";
import {useEventStore} from "@/stores/eventStore";
import ParticipantsSegment from "@/components/ParticipantsSegment.vue";

const eventStore = useEventStore();
const filteredEvents = ref(eventStore.events);
const eventFilter = ref("");

watch(eventFilter, (newValue) => {
  if (newValue === "") {
    filteredEvents.value = eventStore.events;
  } else {
    filteredEvents.value = eventStore.events.filter((event) => {
      return event.name.toLowerCase().includes(newValue.toLowerCase());
    });
  }
});


</script>

<template>
  <section class="header-section">
    <BFormInput
        v-model="eventFilter"
        placeholder="Filtern nach Event..."
    />
  </section>
  <section class="main-section" v-for="(event, index) in filteredEvents" :key="index">
    <ParticipantsSegment
        :id="event.id"
        :name="event.name"
        :date="event.date"
        :time="event.time"
        :participants="event.participants ?? []"
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
  width: 50%;
}

.main-section {
  width: 50%;
}
</style>
