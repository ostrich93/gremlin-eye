import apiClient from '../config/apiClient';

export async function login(dispatch, payload) {
    try {
        dispatch({ type: "LOGIN_REQUEST" });
        const response = await apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/auth/login`, payload, {
            withCredentials: true
        });

        if (response.data) {
            const userData = {
                userId: response.data.userId,
                username: response.data.username,
                role: response.data.role
            };
            dispatch({
                type: "LOGIN_SUCCESS", payload:
                {
                    user: userData,
                    accessToken: response.data.accessToken,
                    refreshToken: response.data.refreshToken
                }
            });
            sessionStorage.setItem("current_user", JSON.stringify(userData));
            sessionStorage.setItem("access_token", response.data.accessToken);
            sessionStorage.setItem("refresh_token", response.data.refreshToken);
            return response.data;
        }
        else {
            dispatch({ type: "LOGIN_ERROR", error: `Failed to login: ${response.statusText}` });
        }
    } catch(error) {
        dispatch({ type: "LOGIN_ERROR", error: error });
    }
};

export async function logout(dispatch) {
    try {
        const response = await apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/auth/logout`);
        dispatch({ type: "LOGOUT" });
        console.log("logout success");
        sessionStorage.removeItem("current_user");
        sessionStorage.removeItem("access_token");
        sessionStorage.removeItem("refresh_token");
        return response;
    } catch (error) {
        dispatch({ type: "LOGOUT_ERROR", error: error });
    }
};

export async function refreshAccessToken(dispatch, payload) {
    try {
        dispatch({ type: "REFRESH_REQUEST" });
        const response = await apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/auth/refresh`, payload, {
            withCredentials: true
        });

        if (response.data) {
            dispatch({
                type: "REFRESH_SUCCESS",
                payload: {
                    accessToken: response.data.accessToken
                }
            });
            sessionStorage.setItem("access_token", response.data.accessToken);
            return response.data;
        } else {
            dispatch({ type: "REFRESH_ERROR", error: `Failed to refresh the access token: ${response.statusText}` });
        }
    } catch (error) {
        dispatch({ type: "REFRESH_ERROR", error: error });
    }
};