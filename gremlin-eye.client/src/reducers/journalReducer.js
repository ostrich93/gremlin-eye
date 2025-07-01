export const initialJournalState = {
    gameId: -1,
    showJournalModal: false,
    showPlayStatusModal: false,
    loading: false,
    error: null
};

export const JournalReducer = (state, action) => {
    switch (action.type) {
        case "OPEN_JOURNAL_MODAL":
            return { ...state, gameId: action.payload.gameId, showJournalModal: true };
        case "CLOSE_JOURNAL_MODAL":
            return { ...state, gameId: -1, showJournalModal: false };
        default:
            throw new Error(`Unhandled action type: ${action.type} for Journal Reducer`);
    }
};