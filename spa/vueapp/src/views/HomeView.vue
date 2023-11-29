<script setup lang="ts">
import { ref } from "vue";

const test = ref(5);
</script>

<template>
  <header>
    <nav>
      <RouterLink to="/new-events"> Veranstaltungen verwalten </RouterLink>
      <RouterLink to="/check-events"> Teilnehmerzahlen verwalten </RouterLink>
      <RouterLink to="/apply-events"> Veranstaltungen buchen </RouterLink>
      <RouterLink to="/check-applied-events">
        Veranstaltungen ansehen
      </RouterLink>
    </nav>
  </header>

  <main>
    <h1>Hello World! {{ test }}</h1>
    <button @click="test++">Add</button>
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
