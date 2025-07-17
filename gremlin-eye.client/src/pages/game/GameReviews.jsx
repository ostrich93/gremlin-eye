import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { Button, Card, Col, Container, Row } from "react-bootstrap";
import { useAuthState } from "../../contexts/AuthProvider";
import apiClient from "../../config/apiClient";
import ReviewCard from "../../components/ReviewCard/ReviewCard";

export default function GameReviews() {
    const { user } = useAuthState();
    const { slug } = useParams();
    const [coverUrl, setCoverUrl] = useState("");
    const [gameName, setGameName] = useState("");
    const [leftReviews, setLeftReviews] = useState([]);
    const [rightReviews, setRightReviews] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [isEnd, setIsEnd] = useState(false);
    const [loading, setLoading] = useState(false);

    //stateValues for timeFilter and sortOrder    

    useEffect(() => {
        const fetchReviews = async () => {
            setLoading(true);
            apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/reviews/${slug}?page=${currentPage}`)
                .then((res) => {
                    setCoverUrl(res.data.coverUrl);
                    setGameName(res.data.gameName);
                    setLeftReviews(leftReviews => [...leftReviews, ...res.data.reviews.slice(0, 16)]);
                    setRightReviews(rightReviews => [...rightReviews, ...res.data.reviews.slice(16, 32)]);
                    setIsEnd(res.data.isEnd);
                })
                .catch((err) => {
                    console.error("Error fetching review data: ", err);
                })
                .finally(() => {
                    setLoading(false);
                });
        };
        fetchReviews();
    }, [slug, currentPage]);

    return (
        <Container>
            {!loading && gameName.length > 0 && (
                <>
                    <Row className="mt-4">
                        <div className="col-2 col-md mb-2 pe-0">
                            <Link to={`/games/${slug}`}>
                                {/* The Card/Image is too big */}
                                <Card className="mx-auto game-cover overlay-hide">
                                    <Card.Img src={coverUrl} alt={gameName} />
                                    <div className="overlay" />
                                </Card>
                            </Link>
                        </div>
                        <Col className="mt-auto mb-2">
                            <Link to={`games/${slug}`}>
                                <h2>{gameName}</h2>
                            </Link>
                        </Col>
                    </Row>
                    <hr className="my-1" />
                    <Row>
                        <div id="reviews-list-left" className="col-12 col-lg-6">
                            {leftReviews.map((review) => (
                                <ReviewCard key={review.reviewId} reviewData={review} isUserSubpage={false} />
                            ))}
                        </div>
                        <div id="reviews-list-right" className="col-12 col-lg-6">
                            {rightReviews.map((review) => (
                                <ReviewCard key={review.reviewId} reviewData={review} isUserSubpage={false} />
                            ))}
                        </div>
                    </Row>
                    <Row />
                </>
            )}
            {!isEnd && !loading && (
                <Row className="my-2">
                    <div id="div-next-link" className="col-auto mx-auto">
                        <Button id="next-link" className="btn btn-sm btn-general page next" onClick={() => setCurrentPage(currentPage + 1)}>
                            Load more reviews
                        </Button>
                    </div>
                </Row>
            )}
        </Container>
    );

}