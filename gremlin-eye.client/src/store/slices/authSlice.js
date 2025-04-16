import { createSlice } from "@reduxjs/toolkit";
import { loginThunk, logoutThunk } from "../thunks/authThunks";

const initialState = {
    userId: null,
    role: '',
    username: '',
    token: sessionStorage.getItem("GremlinToken") || '',
    authenticated: false,
    loading: false,
    error: null
};

const authSlice = createSlice({
    name: 'auth',
    initialState: initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(loginThunk.pending, (state) => {
            state.loading = true;
        })
        .addCase(loginThunk.fulfilled, (state, action) => {
            state.loading = false;
            state.userId = action.payload.userId;
            state.username = action.payload.username;
            state.role = action.payload.role;
            state.token = action.payload.token;
            state.authenticated = true;
        })
        .addCase(loginThunk.rejected, (state, action) => {
            state.loading = false;
            console.error("Login failed: ", action.payload);
        })
        .addCase(logoutThunk.fulfilled, (state) => {
            state.loading = false;
            state.userId = null;
            state.username = '';
            state.role = '';
            state.token = '';
            state.authenticated = false;
        })
        .addCase(logoutThunk.rejected, (state, action) => {
            state.loading = false;
            console.error("Logout failed: ", action.payload);
        })
    }
});

export default authSlice.reducer;