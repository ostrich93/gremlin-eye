import { useQuery } from "@tanstack/react-query";
import apiClient from "../../config/apiClient";

export const useGetPlatforms = () => {
    useQuery({
        retry: true,
        query: ["platforms"],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/platform`);
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