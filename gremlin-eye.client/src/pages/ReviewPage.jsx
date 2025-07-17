import { useState } from "react";
import { Link, useParams } from "react-router-dom";
import { Button, Card, Col, Container, Row, Spinner } from "react-bootstrap";
import { keepPreviousData, useQuery, useQueryClient, useMutation } from '@tanstack/react-query';
import { faComment } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useAuthState } from "../contexts/AuthProvider";
import apiClient from "../config/apiClient";
import ReviewCard from "../components/ReviewCard/ReviewCard";
import CommentCard from "../components/CommentCard/CommentCard";

//Bug: Comment Submission not reaching endpoint
export default function ReviewPage() {
    const { user } = useAuthState();
    const { reviewId } = useParams();
    const [commentDraft, setCommentDraft] = useState("");
    const queryClient = useQueryClient();

    const { data, isLoading } = useQuery({
        retry: true,
        queryKey: ["reviews", reviewId],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/reviews/${reviewId}`);
                return response.data;
            } catch (error) {
                console.error(error);
                throw new Error("Failed to get review");
            }
        },
        placeholderData: keepPreviousData,
        cacheTime: 1000 * 60 * 5,
        staleTime: 1000 * 60 * 5
    });

    const addCommentMutation = useMutation({
        mutationFn: async (commentBody) => {
            try {
                const response = await apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/reviews/${reviewId}/comment`, { commentBody }, {
                    withCredentials: true
                });

                return response.data;
            } catch (error) {
                console.error(error);
                throw new Error("Failed to submit comment");
            }
        },
        onSuccess: (data) => {
            queryClient.setQueryData(["reviews", { reviewId }], (oldData) => {
                oldData ? {
                    ...oldData,
                    comments: [...oldData.comments, data]
                } : oldData
            });
        }
    });

    const editCommentMutation = useMutation({
        mutationFn: async (commentId, commentBody) => {
            try {
                const response = await apiClient.patch(`${import.meta.env.VITE_APP_BACKEND_URL}/comment/${commentId}`, { commentBody }, {
                    withCredentials: true
                });

                return response.data;
            } catch (error) {
                console.error(error);
                throw new Error("Failed to submit comment");
            }
        },
        onSuccess: (data) => {
            queryClient.setQueryData(["reviews", { reviewId }], (oldData) => {
                oldData ? {
                    ...oldData,
                    comments: oldData.comments.map((comment) => {
                        if (comment.commentId === data.commentId) {
                            return data;
                        }
                    })
                } : oldData
            });
        }
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!commentDraft.trim()) return;

        addCommentMutation.mutate({ commentBody: commentDraft });
    };

    return (
        <Container>
            <Row id="review-sidebar" className="mt-3">
                {!isLoading && (
                    <>
                        <div className="col-12 col-md-2">
                            <Row>
                                <div className="col-6 col-md-12 mx-auto">
                                    <Link to={`/games/${data?.gameSlug}`}>
                                        <Card className="mx-auto game-cover overlay-hide">
                                            <Card.Img src={data?.coverUrl} alt={data?.gameName} />
                                            <div className="overlay" />
                                        </Card>
                                    </Link>
                                </div>
                            </Row>
                        </div>
                    </>)}
                <div className="col-12 col-md">
                    {isLoading && (<Spinner animation="border" />)}
                    {!isLoading && (
                        <>
                            <ReviewCard reviewData={data} isUserSubpage={false} />
                            {user && (
                                <>
                                    <Row>
                                        <Col>
                                            <textarea id="comment-body" className="p-2 w-100 gremlin-eye-field comment-field" name="comment-body" rows="3" placeholder="Leave a comment" onChange={(e) => setCommentDraft(e.target.value)} />
                                        </Col>
                                    </Row>
                                    <Row>
                                        <div className="col-auto ms-auto">
                                            <Button id="comment-submit" className="btn-main" onClick={(e) => handleSubmit(e)}>Comment</Button>
                                        </div>
                                    </Row>
                                    <Row>
                                        <Col>
                                            <h2 id="comments-header">
                                                <FontAwesomeIcon icon={faComment} />
                                                {data?.comments.length} Comments
                                            </h2>
                                            <hr className="mt-0 mb-3" />
                                        </Col>
                                    </Row>
                                </>
                            )}
                            {data?.comments.map((comment) => (
                                <CommentCard key={comment.commentId} commentData={comment} />
                            ))}
                        </>
                    )}
                </div>
            </Row>
        </Container>
    )
}