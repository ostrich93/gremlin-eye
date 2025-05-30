import apiClient from '../config/apiClient';

const GameService = {
    getGameBySlug: async (slug) => {
        try {
            const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/${slug}`);
            return response.data;
        } catch (error) {
            console.error("Error fetching game data: ", error);
            throw new Error("Failed to retrieve game data.");
        }
    },

    getGameLog: async (gameId) => {
        try {
            const response = apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs/${gameId}`);
            return response.data;
        } catch (error) {
            console.error("Error fetching game log: ", error);
            throw new Error("Failed to retreieve game log.");
        }
    },

    updateGameLog: async (gameLog) => {
        try {
            const response = await apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/log/${gameLog.gameId}`, gameLog);
            return response.data;
        } catch (error) {
            console.error("Error updating gameLog: ", error);
            throw new Error("Failed to update the gameLog");
        }
    }
};

export default GameService;