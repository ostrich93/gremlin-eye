const getUser = () => {
    return sessionStorage.getItem("current_user") ? JSON.parse(sessionStorage.getItem("current_user")) : null;
};

const getAccessToken = () => {
    return sessionStorage.getItem("access_token") || '';
};

const getRefreshToken = () => {
    return sessionStorage.getItem("refresh_token") || '';
}

export const initialState = {
    user: getUser() || null,
    accessToken: getAccessToken(),
    refreshToken: getRefreshToken(),
    loading: false,
    error: null
};

export const AuthReducer = (initialState, action) => {
    switch (action.type) {
        case "LOGIN_REQUEST":
        case "REFRESH_REQUEST":
            return { ...initialState, loading: true };
        case "LOGIN_SUCCESS":
            return {
                ...initialState,
                user: action.payload.user,
                accessToken: action.payload.accessToken,
                refreshToken: action.payload.refreshToken,
                loading: false
            };
        case "REFRESH_SUCCESS":
            return {
                ...initialState,
                accessToken: action.payload.accessToken,
                refreshToken: action.payload.refreshToken,
                loading: false
            };
        case "LOGOUT":
            return {
                ...initialState,
                user: null,
                accessToken: '',
                loading: false
            };
        case "LOGIN_ERROR":
        case "LOGOUT_ERROR":
            return {
                ...initialState,
                loading: false,
                error: action.error
            };
        case "REFRESH_ERROR":
            return {
                ...initialState,
                accessToken: '',
                refreshToken: '',
                loading: false,
                error: action.error
            };
        default:
            throw new Error(`Unhandled action type: ${action.type}`);
    }
};