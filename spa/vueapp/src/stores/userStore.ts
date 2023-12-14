import { defineStore } from "pinia";
import { ref } from "vue";
import type { Ref } from "vue";

/**
 * @description Store für Benutzerdaten
 */
export const useUserStore = defineStore("userStore", () => {
  const loggedIn: Ref<boolean> = ref(false);
  const userRole: Ref<string> = ref("");
  const userName: Ref<string> = ref("");

  return { loggedIn, userRole, userName };
});
