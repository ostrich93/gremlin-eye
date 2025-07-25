import { createContext, useContext, useReducer } from 'react';
import { initialJournalState, JournalReducer } from "../reducers/journalReducer";

const JournalContext = createContext(null);
const JournalDispatchContext = createContext(null);

export default function JournalProvider({ children }) {
    const [journalState, dispatch] = useReducer(JournalReducer, initialJournalState);

    return (
        <JournalContext.Provider value={journalState}>
            <JournalDispatchContext.Provider value={dispatch}>
                {children}
            </JournalDispatchContext.Provider>
        </JournalContext.Provider>
    );
};

export function useJournalState() {
    const context = useContext(JournalContext);

    if (context === undefined) {
        throw new Error("JournalContext was used outside the JournalProvider.");
    }

    return context;
}

export function useJournalDispatch() {
    const context = useContext(JournalDispatchContext);

    if (context === undefined) {
        throw new Error("JournalDispatchContext was used outside the JournalProvider.");
    }

    return context;
}