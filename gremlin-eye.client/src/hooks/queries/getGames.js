import { useQuery, keepPreviousData } from "@tanstack/react-query";
import apiClient from "../../config/apiClient";

const useGetGamesForLibrary = (searchParams) => {
    useQuery({
        retry: true,
        queryKey: ["games", "search", searchParams.get('page') ?? 1],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/lib`, searchParams);
                return response.data;
            } catch (error) {
                console.error(error);
                //fixed data list to import
                throw new Error("Failed to get games");
            }
        },
        placeholderData: keepPreviousData,
        cacheTime: 1000 * 60 * 5
    });
};

export default useGetGamesForLibrary;
