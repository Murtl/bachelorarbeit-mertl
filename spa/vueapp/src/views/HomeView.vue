<script setup lang="ts">
import { useUserStore } from "@/stores/userStore";

const userStore = useUserStore();
</script>

<template>
  <h2>Willkommen auf dem Schulungsportal der Schalk Maschinen GmbH</h2>
  <h4 v-if="userStore.userRole === 'Task.Create'">
    Hier kannst du neue Veranstaltungen erstellen und Teilnehmer einsehen!
  </h4>
  <h4 v-if="userStore.userRole === 'Task.Apply'">
    Hier kannst du dich für neue Veranstaltungen anmelden und diese ansehen!
  </h4>
  <h4
    v-if="
      userStore.userRole !== 'Task.Apply' &&
      userStore.userRole !== 'Task.Create'
    "
  >
    Es werden keine zusätzlichen Tabs angezeigt, da Sie entweder über keine
    Rolle oder die falsche Rolle verfügen. Bitte nehmen Sie umgehend Kontakt mit
    Ihrer IT-Abteilung auf, um die entsprechende Rolle zu erhalten. Nur so
    können Sie mit diesem Schulungsportal interagieren.
  </h4>
</template>

<style scoped>
h4 {
  text-align: center;
}
</style>
