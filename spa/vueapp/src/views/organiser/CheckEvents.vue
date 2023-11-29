<script setup lang="ts">
import { onMounted, ref } from "vue";
import {useUserStore} from "@/stores/userStore";
import ApiService from "@/auth/ApiService";

const test = ref("Initial");
const userStore = useUserStore();

onMounted(async () => {
  try {
    const response = await ApiService.get("dummy");
    test.value = response.data;
  } catch (e: any) {
    console.log(e);
  }
});
</script>

<template>
  <h1>Check Events</h1>
  <h2>{{ test }}</h2>
</template>
