import { useQuery } from "@tanstack/react-query";
import apiClient from "../../config/apiClient";

export const useGetGenres = () => {
    useQuery({
        retry: true,
        query: ["genres"],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/genre`);
                await response.data;
            } catch (error) {
                console.error(error);
                //import data
                throw new Error("Failed to get games");
            }
        },
        cacheTime: 1000 * 60 * 5,
    })
};