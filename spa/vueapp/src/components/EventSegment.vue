<script setup lang="ts">
import { useEventStore } from "@/stores/eventStore";
import { useUserStore } from "@/stores/userStore";

interface Props {
  /**
   * @description Die ID des Events
   */
  id: string;

  /**
   * @description Der Name des Events
   */
  name: string;

  /**
   * @description Das Datum des Events
   */
  date: string;

  /**
   * @description Die Uhrzeit des Events
   */
  time: string;

  /**
   * @description Die Beschreibung des Events
   */
  description: string;

  /**
   * @description Der Typ des Buttons
   */
  buttonType?: string;
}

defineProps<Props>();

interface Emits {
  /**
   * @description Entfernt einen Teilnehmer aus dem Event
   */
  (event: "removeParticipant", id: string): void;
}
const emit = defineEmits<Emits>();

const eventStore = useEventStore();
const userStore = useUserStore();
</script>

<template>
  <div class="event-segment-host">
    <section class="header-section">
      <span class="event-name">{{ name }}</span>
      <div class="right-header-section">
        <span class="event-date">{{ date }}</span>
        <span class="event-time">{{ time }}</span>
        <BButton
          size="sm"
          variant="primary"
          v-if="buttonType === 'Add'"
          @click="eventStore.addParticipant(id, userStore.userName)"
          >+</BButton
        >
        <BButton
          size="sm"
          variant="danger"
          v-if="buttonType === 'Delete'"
          @click="eventStore.removeEvent(id)"
          >-</BButton
        >
        <BButton
          size="sm"
          variant="danger"
          v-if="buttonType === 'Remove'"
          @click="emit('removeParticipant', id)"
          >-</BButton
        >
      </div>
    </section>
    <section class="describe-section">
      {{ description }}
    </section>
  </div>
</template>

<style scoped>
.event-segment-host {
  border: 1px solid black;
  padding: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  margin: 5px;
  border-radius: 5px;
}

.header-section {
  display: flex;
  margin-bottom: 10px;
}

.event-name {
  font-weight: bold;
  width: 70%;
}

.right-header-section {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  width: 30%;
}

.describe-section {
  width: 100%;
}
</style>
