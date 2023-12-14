<script setup lang="ts">
import { ref, watch } from "vue";
import type { Ref } from "vue";
import { useEventStore } from "@/stores/eventStore";
import EventSegment from "@/components/EventSegment.vue";
import { storeToRefs } from "pinia";
import type { EventType } from "@/utils/eventType";
import { v4 as uuidv4 } from "uuid";

const eventStore = useEventStore();
const { events } = storeToRefs(eventStore);
const filteredEvents = ref(eventStore.events);
const eventFilter = ref("");
const showAddEventModal = ref(false);
const newEvent: Ref<EventType> = ref({
  id: "",
  name: "",
  date: "",
  time: "",
  description: "",
  participants: [],
});

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

/**
 * @description fügt ein neues Event über den eventStore hinzu
 */
async function addEvent() {
  newEvent.value.id = uuidv4();
  await eventStore.addEvent(newEvent.value);
  showAddEventModal.value = false;
  newEvent.value = {
    id: "",
    name: "",
    date: "",
    time: "",
    description: "",
    participants: [],
  };
}
</script>

<template>
  <section class="header-section">
    <BFormInput placeholder="Filtern nach Event..." v-model="eventFilter" />
    <BButton variant="primary" @click="showAddEventModal = true"> + </BButton>
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
      button-type="Delete"
    />
  </section>
  <BModal
    v-model="showAddEventModal"
    title="Neue Veranstaltung erstellen"
    size="lg"
    @ok="addEvent"
    :no-close-on-backdrop="true"
    :no-close-on-esc="true"
  >
    <BContainer>
      <BRow>
        <BCol>
          <BFormGroup label="Veranstaltungsname*">
            <BFormInput
              v-model="newEvent.name"
              placeholder="Veranstaltungsname..."
            />
          </BFormGroup>
        </BCol>
        <BCol>
          <BFormGroup label="Veranstaltungsdatum*">
            <BFormInput
              v-model="newEvent.date"
              placeholder="Datum in Form dd.mm.yyyy..."
            />
          </BFormGroup>
        </BCol>
      </BRow>
      <BRow>
        <BCol>
          <BFormGroup label="Veranstaltungsuhrzeit*">
            <BFormInput
              v-model="newEvent.time"
              placeholder="Uhrzeit in Form xx:xx - yy:yy..."
            />
          </BFormGroup>
        </BCol>
      </BRow>
      <BRow>
        <BCol>
          <BFormGroup label="Veranstaltungsbeschreibung">
            <BFormTextarea
              v-model="newEvent.description"
              placeholder="Veranstaltungsbeschreibung..."
            />
          </BFormGroup>
        </BCol>
      </BRow>
    </BContainer>
  </BModal>
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
