import { createRouter, createWebHistory } from "vue-router";
import { authGuard } from "@/auth/authGuard";

/**
 * @description Router für die Anwendung
 */
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: () => import("../views/HomeView.vue"),
    },
    {
      path: "/check-events",
      name: "check-events",
      component: () => import("../views/organiser/CheckEvents.vue"),
      beforeEnter: authGuard("Task.Create"),
    },
    {
      path: "/new-events",
      name: "new-events",
      component: () => import("../views/organiser/NewEvents.vue"),
      beforeEnter: authGuard("Task.Create"),
    },
    {
      path: "/apply-events",
      name: "apply-events",
      component: () => import("../views/participant/ApplyEvents.vue"),
      beforeEnter: authGuard("Task.Apply"),
    },
    {
      path: "/check-applied-events",
      name: "check-applied-events",
      component: () => import("../views/participant/CheckAppliedEvents.vue"),
      beforeEnter: authGuard("Task.Apply"),
    },
    {
      path: "/access-denied",
      name: "access-denied",
      component: () => import("../views/shared/AccessDenied.vue"),
    },
  ],
});

export default router;
