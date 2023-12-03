import {BrowserCacheLocation, PublicClientApplication} from "@azure/msal-browser";

const msalConfig = {
    auth: {
        clientId: '<YOUR_CLIENT_ID>',
        authority: 'https://login.microsoftonline.com/<YOUR_TENANT_ID>',
        redirectUri: 'https://localhost:5173',
        clientCapabilities: ['CP1']
    },
        cache: {
            cacheLocation: BrowserCacheLocation.LocalStorage,
        },
};

export const msalInstance = new PublicClientApplication(msalConfig);
