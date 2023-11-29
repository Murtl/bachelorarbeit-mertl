<script setup lang="ts">
import {msalInstance} from "@/auth/msalConfig";
import {useUserStore} from "@/stores/userStore";

const userStore = useUserStore();

async function logout() {
  try {
    const logoutRequest = {
      scopes: ['api://94c3b8ee-37e6-49c2-a05f-a78aa50acc89/Files.read']
    }
    await msalInstance.logoutRedirect(logoutRequest);
  } catch (error) {
    console.error("Fehler bei der Abmeldung", error);
  }
}
</script>

<template>
  <header>
    <nav>
      <RouterLink to="/new-events" v-if="userStore.userRole === 'Task.Create'"> Veranstaltungen verwalten </RouterLink>
      <RouterLink to="/check-events" v-if="userStore.userRole === 'Task.Create'"> Teilnehmerzahlen verwalten </RouterLink>
      <RouterLink to="/apply-events" v-if="userStore.userRole === 'Task.Apply'"> Veranstaltungen buchen </RouterLink>
      <RouterLink to="/check-applied-events" v-if="userStore.userRole === 'Task.Apply'">
        Veranstaltungen ansehen
      </RouterLink>
    </nav>
    <button @click="logout">Logout</button>
  </header>

  <main>
    <RouterView />
  </main>
</template>

<style scoped>
header {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 1rem;
  background-color: #f5f5f5;
}

nav {
  display: flex;
  justify-content: space-around;
  align-items: center;
  width: 100%;
  max-width: 1200px;
}

nav a {
  text-decoration: none;
  color: #333;
  font-weight: bold;
  font-size: 1.2rem;
}

nav a:hover {
  color: lightgray;
}
main {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  padding: 1rem;
}
</style>
