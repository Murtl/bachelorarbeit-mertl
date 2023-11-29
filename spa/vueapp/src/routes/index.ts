import { createRouter, createWebHistory } from "vue-router";

/**
 * @description: This is the router file for the application
 */
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/check-events",
      name: "check-events",
      component: () => import("../views/organiser/CheckEvents.vue"),
    },
    {
      path: "/new-events",
      name: "new-events",
      component: () => import("../views/organiser/NewEvents.vue"),
    },
    {
      path: "/apply-events",
      name: "apply-events",
      component: () => import("../views/participant/ApplyEvents.vue"),
    },
    {
      path: "/check-applied-events",
      name: "check-applied-events",
      component: () => import("../views/participant/CheckAppliedEvents.vue"),
    },
  ],
});

export default router;
