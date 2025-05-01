import axios from 'axios';
import { env } from 'process';

const apiClient = axios.create({
    baseURL: `${env.API_URL}`,
    headers: {
        "Content-Type": "application/json"
    }
});

apiClient.interceptors.request.use((config) => {
    let token = sessionStorage.getItem('access_token');
    if (token) {
        config.headers.credentials = 'include';
        config.headers.Authorization = `Bearer ${token}`;
        config.headers['Access-Control-Allow-Origin'] = "*";
        config.headers['Content-Type'] = "application/json";
    }
});

export default apiClient;