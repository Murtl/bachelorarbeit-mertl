import { useUserStore } from "@/stores/userStore";
import type { NavigationGuardWithThis } from "vue-router";

/**
 * @description Authentifizierung und Autorisierung für die Anwendung
 * @param requiredRole Rolle, die der Benutzer haben muss
 */
export function authGuard(
  requiredRole: string,
): NavigationGuardWithThis<undefined> {
  return (to, from, next) => {
    const userStore = useUserStore();
    // Überprüfe, ob der Benutzer authentifiziert ist
    if (!userStore.loggedIn) {
      // Benutzer ist nicht authentifiziert
      next({ path: "/access-denied" });
    } else {
      // Überprüfe, ob der Benutzer die erforderliche Rolle hat
      const userRole = userStore.userRole;
      if (userRole !== requiredRole) {
        // Benutzer hat nicht die erforderliche Rolle, weiterleiten zu einer Fehlerseite oder zur Startseite
        next({ path: "/access-denied" });
      } else {
        // Authentifizierung und Rolle sind erfolgreich
        next();
      }
    }
  };
}
