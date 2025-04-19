/* eslint-disable no-undef */
import axios from 'axios';

const apiClient = axios.create({
    baseURL: `${process.env.API_URL}`,
    headers: {
        "Content-Type": "application/json"
    }
});

apiClient.interceptors.request.use((config) => {
    let token = sessionStorage.getItem('GremlinToken');
    if (token) {
        config.headers.credentials = 'include';
        config.headers.Authorization = `Bearer ${token}`;
        config.headers['Access-Control-Allow-Origin'] = "*";
        config.headers['Content-Type'] = "application/json";
    }
});

export default apiClient;