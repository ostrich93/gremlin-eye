import { createContext, useContext, useReducer } from "react";
import { initialState, AuthReducer } from '../reducers/authReducer';

export const AuthContext = createContext(null);
export const AuthDispatchContext = createContext(null);

export function useAuth() {
    return useContext(AuthContext);
}

export function useAuthDispatch() {
    return useContext(AuthDispatchContext);
}

export const AuthProvider = ({ children }) => {
    const [userDetails, dispatch] = useReducer(AuthReducer, initialState);

    return (
        <AuthContext.Provider value={userDetails}>
            <AuthDispatchContext.Provider value={dispatch}>
                {children}
            </AuthDispatchContext.Provider>
        </AuthContext.Provider>
    );
};