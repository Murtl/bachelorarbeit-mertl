<script setup lang="ts">
import {msalInstance} from "@/auth/msalConfig";
import {useUserStore} from "@/stores/userStore";
import {onMounted} from "vue";
import {useEventStore} from "@/stores/eventStore";

const eventStore = useEventStore();

const userStore = useUserStore();

onMounted(async () => {
  await msalInstance.initialize();
  const cookieAccounts = msalInstance.getAllAccounts();
  if (cookieAccounts.length > 0 && cookieAccounts[0].idTokenClaims && cookieAccounts[0].idTokenClaims.roles) {
      userStore.userRole = cookieAccounts[0].idTokenClaims.roles[0];
      userStore.loggedIn = true;
      userStore.userName = cookieAccounts[0].username;
      await eventStore.getEvents();
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
    userStore.userName = response.account.username;
    if('roles' in response.idTokenClaims && Array.isArray(response.idTokenClaims.roles)) {
      userStore.userRole = response.idTokenClaims.roles[0];
    }
    await eventStore.getEvents();
  } catch (error) {
    console.error("Fehler bei der Anmeldung", error);
  }
}

async function logout() {
  try {
    await msalInstance.logoutRedirect();
  } catch (error) {
    console.error("Fehler bei der Abmeldung", error);
  }
}
</script>

<template>
  <div v-if="!userStore.loggedIn" class="login-view">
      <h1>Willkommen bei dem Schulungsportal der Schalk Maschinen GmbH</h1>
    <BButton @click="login">Anmelden</BButton>
  </div>
  <div v-else>
    <header class="sticky-top">
      <BNavbar toggleable="lg" type="light" variant="light">
        <BNavbarBrand>Schalk Maschinen GmbH</BNavbarBrand>

        <BNavbarToggle target="nav-collapse"></BNavbarToggle>

        <BCollapse id="nav-collapse" :is-nav="true">
          <BNavbarNav class="mx-auto">
            <BNavItem to="/">Home</BNavItem>
                    <BNavItem to="/new-events" v-if="userStore.userRole === 'Task.Create'">Veranstaltung erstellen</BNavItem>
                    <BNavItem to="/check-events" v-if="userStore.userRole === 'Task.Create'">Teilnehmerzahlen ansehen</BNavItem>
                    <BNavItem to="/apply-events" v-if="userStore.userRole === 'Task.Apply'">Veranstaltung buchen</BNavItem>
                    <BNavItem to="/check-applied-events" v-if="userStore.userRole === 'Task.Apply'">
                      Veranstaltungen ansehen
                    </BNavItem>
          </BNavbarNav>

          <BButton @click="logout" variant="link" class="button-space">Logout</BButton>
        </BCollapse>
      </BNavbar>
    </header>
    <main>
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.login-view{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin-top: 10%;
}

.button-space{
  width: 235px;
  text-align: right;
}

main{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin-top: 10px;
}
</style>
