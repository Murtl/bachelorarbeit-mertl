import axios from "axios";
import { msalInstance } from "@/auth/msalConfig";

/**
 * AApiService ist eine axios-Instanz, die verwendet wird, um Anfragen an das Backend zu stellen
 */
const AApiService = axios.create({
  baseURL: "https://localhost:7236/",
  headers: {
    "Content-Type": "application/json",
  },
});

/**
 * Interceptor, der den Authentifizierungstoken zum Anfrageheader hinzufügt
 */
AApiService.interceptors.request.use(async (config) => {
  try {
    const response = await msalInstance.acquireTokenSilent({
      scopes: ["api://94c3b8ee-37e6-49c2-a05f-a78aa50acc89/Files.read"],
      account: msalInstance.getAllAccounts()[0],
    });
    if (response.accessToken) {
      config.headers.Authorization = `Bearer ${response.accessToken}`;
    }
  } catch (e: any) {
    console.log(e);
  }
  return config;
});

export default AApiService;
