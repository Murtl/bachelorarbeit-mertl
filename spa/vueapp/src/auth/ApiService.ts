import axios from 'axios'
import {msalInstance} from "@/auth/msalConfig";

/**
 * AApiService is an axios instance that is used to make requests to the backend
 */
const AApiService = axios.create({
    baseURL: 'https://localhost:7236/',
    headers: {
        'Content-Type': 'application/json'
    }
})

/**
 * Interceptor that adds the auth token to the request header
 */
AApiService.interceptors.request.use(async (config) => {
    try{
        const response = await msalInstance.acquireTokenSilent({
            scopes: ['api://94c3b8ee-37e6-49c2-a05f-a78aa50acc89/Files.read'],
            account: msalInstance.getAllAccounts()[0]
        })
        if (response.accessToken) {
            config.headers.Authorization = `Bearer ${response.accessToken}`
        }
    }catch(e: any){
        console.log(e)
    }
    return config
})

export default AApiService