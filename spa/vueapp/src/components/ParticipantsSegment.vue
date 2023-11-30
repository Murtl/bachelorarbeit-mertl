<script setup lang="ts">
interface Props{
  id: string;
  name: string;
  date: string;
  time: string;
  participants: string[];
}

defineProps<Props>();
</script>

<template>
  <div class="event-segment-host">
    <section class="header-section">
      <span class="event-name">{{name}}</span>
      <div class="right-header-section">
        <span class="event-date">{{date}}</span>
        <span class="event-time">{{time}}</span>
      </div>
    </section>
    <section class="participant-section">
      {{ participants.join(", ") }}
    </section>
  </div>
</template>

<style scoped>
.event-segment-host {
  border: 1px solid black;
  padding: 10px;
  box-shadow: 0 0 10px rgba(0,0,0,0.1);
  margin: 5px;
  border-radius: 5px;
}

.header-section {
  display: flex;
  margin-bottom: 10px;
}

.event-name{
  font-weight: bold;
  width: 80%;
}

.right-header-section {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  width: 20%;
}

.participant-section {
  width: 100%;
}
</style>