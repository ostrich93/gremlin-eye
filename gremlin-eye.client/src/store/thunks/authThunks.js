/* eslint-disable no-undef */
import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';

export const loginThunk = createAsyncThunk(
    'auth/login',
    async function ({ username, password }, thunkAPI) {
        try {
            const response = await axios.post(`${process.env.API_URL}/user/login`, { username: username, password: password }, {
                withCredentials: true
            });

            if (response.data) {
                sessionStorage.setItem("GremlinToken", response.data.token);
            }
            return {
                userId: response.data.userId,
                username: response.data.username,
                role: response.data.role,
                token: response.data.token
            };
        } catch (error) {
            return thunkAPI.rejectWithValue(error.response?.data || "Failed to login");
        }
    }
);

export const logoutThunk = createAsyncThunk(
    'auth/logout',
    async function (_, thunkAPI) {
        try {
            const response = await axios.post(`${process.env.API_URL}/user/logout`);
            return response;
        } catch (error) {
            return thunkAPI.rejectWithValue(error.response?.data || "Failed to logout");
        }
    }
)