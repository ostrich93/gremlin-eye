const getUser = () => {
    return localStorage.getItem("current_user") ? JSON.parse(localStorage.getItem("current_user")) : null;
};

const getAccessToken = () => {
    return localStorage.getItem("access_token") || '';
};

const getRefreshToken = () => {
    return localStorage.getItem("refresh_token") || '';
}

export const initialState = {
    user: getUser() || null,
    accessToken: getAccessToken(),
    refreshToken: getRefreshToken(),
    loading: false,
    error: null
};

export const AuthReducer = (state, action) => {
    switch (action.type) {
        case "LOGIN_REQUEST":
        case "REFRESH_REQUEST":
        case "REGISTER_REQUEST":
            return { ...state, loading: true };
        case "AUTH_FETCHED":
            return { ...state, loading: false };
        case "LOGIN_SUCCESS":
            return {
                ...state,
                user: action.payload.user,
                accessToken: action.payload.accessToken,
                refreshToken: action.payload.refreshToken,
                loading: false,
                error: null
            };
        case "REFRESH_SUCCESS":
            return {
                ...state,
                accessToken: action.payload.accessToken,
                refreshToken: action.payload.refreshToken,
                loading: false,
                error: null
            };
        case "REGISTER_SUCCESS":
            return initialState;
        case "LOGOUT":
            return {
                ...state,
                user: null,
                accessToken: '',
                refreshToken: '',
                loading: false,
                error: null
            };
        case "LOGIN_ERROR":
        case "LOGOUT_ERROR":
        case "REGISTER_ERROR":
            return {
                ...state,
                loading: false,
                error: action.error
            };
        case "REFRESH_ERROR":
            return {
                ...state,
                accessToken: '',
                refreshToken: '',
                loading: false,
                error: action.error
            };
        default:
            throw new Error(`Unhandled action type: ${action.type}`);
    }
};