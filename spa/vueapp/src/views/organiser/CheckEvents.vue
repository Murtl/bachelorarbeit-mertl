<script setup lang="ts">
//mach einen call an die api und hol dir für die route dummyData die daten über axios get
import { onMounted, ref } from "vue";
import axios, { AxiosError } from "axios";

const test = ref("Initial");

onMounted(async () => {
  try {
    test.value = await axios.get("dummy");
  } catch (e: any) {
    const axiosError = JSON.parse((e as AxiosError).request.response);
    console.log(axiosError);
  }
});
</script>

<template>
  <h1>Check Events</h1>
  <h2>{{ test }}</h2>
</template>
