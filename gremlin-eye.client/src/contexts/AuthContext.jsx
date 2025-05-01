import { createContext, useContext, useReducer, useEffect } from "react";
import { initialState, AuthReducer } from '../reducers/authReducer';
import apiClient from '../config/apiClient';
import { refreshAccessToken } from '../actions/authActions';

export const AuthContext = createContext(null);
export const AuthDispatchContext = createContext(null);

export function useAuth() {
    return useContext(AuthContext);
}

export function useAuthDispatch() {
    return useContext(AuthDispatchContext);
}

export const AuthProvider = ({ children }) => {
    const [authState, dispatch] = useReducer(AuthReducer, initialState);

    useEffect(() => {
        const authInterceptor = apiClient.interceptors.request.use((config) => {
            config.headers.Authorization = !config._retry && authState.accessToken
                ? `Bearer ${authState.accessToken}` : config.headers.Authorization;

            return config;
        });

        return () => apiClient.interceptors.request.eject(authInterceptor); //on dismount
    }, [authState.accessToken]);

    useEffect(() => {
        const refreshInterceptor = apiClient.interceptors.response.use(response => response,
            async (error) => {
                if (error.isAxiosError && error.response.status === 401 && authState.refreshToken && authState.accessToken) {
                    try {
                        const data = await refreshAccessToken(dispatch, {
                            accessToken: authState.accessToken,
                            refreshToken: authState.refreshToken
                        });
                        if (error.config) {
                            error.config._retry = true;
                            error.config.headers.Authorization = `Bearer ${data.accessToken}`;
                            return apiClient(error.config);
                        }
                    } catch (refreshError) {
                        return Promise.reject(refreshError);
                    }
                }
                return Promise.reject(error);
            }
        );
        return () => {
            apiClient.interceptors.response.eject(refreshInterceptor);
        }
    }, [authState.accessToken, authState.refreshToken]);

    return (
        <AuthContext.Provider value={authState}>
            <AuthDispatchContext.Provider value={dispatch}>
                {children}
            </AuthDispatchContext.Provider>
        </AuthContext.Provider>
    );
};