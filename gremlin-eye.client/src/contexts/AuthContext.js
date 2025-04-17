import { createContext, useReducer } from "react";
import { initialState, AuthReducer } from '../reducers/authReducer';

export const AuthContext = createContext(null);
export const AuthDispatchContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, dispatch] = useReducer(AuthReducer, initialState);

    return (
        <AuthContext.Provider value={user}>
            <AuthDispatchContext.Provider value={dispatch}>
                {children}
            </AuthDispatchContext.Provider>
        </AuthContext.Provider>
    );
};