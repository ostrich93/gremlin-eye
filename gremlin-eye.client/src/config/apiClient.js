import axios from 'axios';

const basicRequestInterceptor = (config) => {
    config.headers["Content-Type"] = "application/json";
    config.headers["Accept"] = "application/json";

    const authToken = sessionStorage.getItem('access_token');
    if (authToken) {
        config.headers.Authorization = `Bearer ${authToken}`;
    }
    return config;
};

const configureApiClient = (client) => {
    client.interceptors.request.use(basicRequestInterceptor);
    client.interceptors.response.use(
        (response) => { return response; },
        (error) => {
            const errMessage = error.response?.data?.message || error.message;
            console.error(errMessage);

            return Promise.reject(error);
        }
    );
    client.defaults.baseURL = import.meta.env.VITE_APP_BACKEND_URL;
};

const apiClient = axios.create();
configureApiClient(apiClient);
/*apiClient.interceptors.request.use((config) => {
    let token = sessionStorage.getItem('access_token');
    if (token) {
        config.headers.credentials = 'include';
        config.headers.Authorization = `Bearer ${token}`;
        config.headers['Access-Control-Allow-Origin'] = "*";
        config.headers['Content-Type'] = "application/json";
    }
});*/

export default apiClient;