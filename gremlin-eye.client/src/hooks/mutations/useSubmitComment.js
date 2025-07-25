import { useMutation, useQueryClient } from "@tanstack/react-query";
import apiClient from "../../config/apiClient";

const submitComment = async (reviewId, commentDraft) => {
    return await apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/reviews/addComment`, { reviewId, commentBody: commentDraft });
};

export const useSubmitComment = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: submitComment,
        onSuccess: (data, variables) => {
            //queryClient.invalidateQueries({ queryKey: ["reviews", data.modelId] });
            queryClient.setQueryData(["reviews", { reviewId: variables.reviewId }], (oldData) => {
                oldData ? {
                    ...oldData,
                    comments: [...oldData.comments, data]
                } : oldData
            });
        }
    });
};