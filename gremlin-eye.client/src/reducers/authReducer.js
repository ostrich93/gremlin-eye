const getUser = () => {
    return sessionStorage.getItem("current_user") ? JSON.parse(sessionStorage.getItem("current_user")) : null;
};

const getToken = () => {
    return sessionStorage.getItem("GremlinToken") || '';
}

export const initialState = {
    user: getUser() || null,
    token: getToken(),
    loading: false,
    error: null
};

export const AuthReducer = (initialState, action) => {
    switch (action.type) {
        case "LOGIN_REQUEST":
            return { ...initialState, loading: true };
        case "LOGIN_SUCCESS":
            return {
                ...initialState,
                user: action.payload.user,
                token: action.payload.user.token,
                loading: false
            }
        case "LOGOUT":
            return {
                ...initialState,
                user: null,
                token: '',
                loading: false
            }
        case "LOGIN_ERROR":
        case "LOGOUT_ERROR":
            return {
                ...initialState,
                loading: false,
                error: action.error
            };
        default:
            throw new Error(`Unhandled action type: ${action.type}`);
    }
};