<script setup lang="ts">
import HomeView from "@/views/HomeView.vue";
import {msalInstance} from "@/auth/msalConfig";
import {useUserStore} from "@/stores/userStore";
import {onMounted} from "vue";

const userStore = useUserStore();

onMounted(async () => {
  await msalInstance.initialize();
  const cookieAccounts = msalInstance.getAllAccounts();
  if (cookieAccounts.length > 0 && cookieAccounts[0].idTokenClaims && cookieAccounts[0].idTokenClaims.roles) {
      userStore.userRole = cookieAccounts[0].idTokenClaims.roles[0];
      userStore.loggedIn = true;
  } else {
    userStore.loggedIn = false;
    userStore.userRole = '';
  }
})

async function login() {
  try {
    await msalInstance.clearCache()
    const loginRequest = {
      scopes: ['api://94c3b8ee-37e6-49c2-a05f-a78aa50acc89/Files.read']
    }
    const response = await msalInstance.loginPopup(loginRequest);
    userStore.loggedIn = true;
    if('roles' in response.idTokenClaims && Array.isArray(response.idTokenClaims.roles)) {
      userStore.userRole = response.idTokenClaims.roles[0];
    }
  } catch (error) {
    console.error("Fehler bei der Anmeldung", error);
  }
}
</script>

<template>
  <div v-if="!userStore.loggedIn">
      <h1>Willkommen bei dem Schulungsportal der Schalk Maschinen GmbH</h1>
    <button @click="login">Anmelden</button>
  </div>
  <HomeView v-else />
</template>
