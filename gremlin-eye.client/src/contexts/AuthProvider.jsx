import { createContext, useContext, useReducer, useEffect } from "react";
import { initialState, AuthReducer } from '../reducers/authReducer';
import apiClient from '../config/apiClient';

const AuthStateContext = createContext(null);
const AuthDispatchContext = createContext(null);

export default function AuthProvider ({ children }) {
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
                        dispatch({ type: "REFRESH_REQUEST" });
                        const res = await apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/auth/refresh`,
                            {
                                accessToken: authState.accessToken,
                                refreshToken: authState.refreshToken
                            },
                            {
                                withCredentials: true
                            });
                        if (res.data) {
                            sessionStorage.setItem("access_token", res.data.accessToken);
                            dispatch({
                                type: "REFRESH_SUCCESS",
                                payload: {
                                    accessToken: res.data.accessToken
                                }
                            });
                        } else {
                            dispatch({
                                type: "REFRESH_ERROR",
                                payload: {
                                    error: `Failed to refresh the access token: ${res.statusText}`
                                }
                            });
                        }
                        if (error.config) {
                            error.config._retry = true;
                            error.config.headers.Authorization = `Bearer ${res.data.accessToken}`;
                            return apiClient(error.config);
                        }
                    } catch (refreshError) {
                        console.error(refreshError);
                        dispatch({
                            type: "REFRESH_ERROR",
                            payload: {
                                error: refreshError
                            }
                        });
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
        <AuthStateContext.Provider value={authState}>
            <AuthDispatchContext.Provider value={dispatch}>
                {children}
            </AuthDispatchContext.Provider>
        </AuthStateContext.Provider>
    );
};

export function useAuthState() {
    const context = useContext(AuthStateContext);

    if (context === undefined) {
        throw new Error("AuthStateContext was used outside the AuthStateProvider.");
    }

    return context;
}

export function useAuthDispatch() {
    const context = useContext(AuthDispatchContext);

    if (context === undefined) {
        throw new Error("AuthDispatchContext was used outside the AuthDispatchProvider.");
    }

    return context;
}