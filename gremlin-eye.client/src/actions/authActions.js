/* eslint-disable no-undef */
import axios from 'axios';

export async function login(dispatch, payload) {
    try {
        dispatch({ type: "LOGIN_REQUEST" });
        const response = await axios.post(`${process.env.API_URL}/user/login`, payload, {
            withCredentials: true
        });

        if (response.data) {
            const userData = {
                userId: response.data.userId,
                username: response.data.username,
                role: response.data.role,
                token: response.data.token
            };
            dispatch({
                type: "LOGIN_SUCCESS", payload:
                {
                    user: userData,
                    token: response.data.token
                }
            });
            sessionStorage.setItem("current_user", JSON.stringify(userData));
            sessionStorage.setItem("GremlinToken", response.data.token);
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
        const response = await axios.post(`${process.env.API_URL}/user/logout`);
        dispatch({ type: "LOGOUT" });
        sessionStorage.removeItem("current_user");
        sessionStorage.removeItem("GremlinToken");
        return response;
    } catch (error) {
        dispatch({ type: "LOGOUT_ERROR", error: error });
    }
};