import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { Col, Row, Spinner } from "react-bootstrap";
import ReviewChart from "../../components/Game/ReviewChart";
import ReviewCard from "../../components/ReviewCard/ReviewCard";
import apiClient from "../../config/apiClient";

export default function UserProfile() {
    const { username } = useParams();
    const [userProfile, setUserProfile] = useState(null);
    const [loading, setLoading] = useState(false);
    const currentYear = new Date().getUTCFullYear();

    useEffect(() => {
        const loadUserProfile = async () => {
            setLoading(true);
            apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/user/profile/${username}`)
                .then((res) => {
                    setUserProfile({
                        username: res.data.username,
                        totalGamesPlayed: res.data.totalGamesPlayed,
                        gamesPlayedThisYear: res.data.gamesPlayedThisYear,
                        gamesBacklogged: res.data.gamesBacklogged,
                        ratingCounts: res.data.ratingCounts,
                        recentReviews: res.data.recentReviews
                    });
                }).catch((err) => {
                    console.error("Error fetching profile details: ", err);
                }).finally(() => {
                    setLoading(false);
                })
        }

        loadUserProfile();
    }, [username]);

    return (
        <>
            <Row className="mt-3">
                <div id="profile-sidebar" className="col-12 col-md-3 col-lg-2">
                    <Row className="mb-3">
                        <Col>
                            <span id="bio-title"></span>
                            <hr className="mt-0 mb-2" />
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <span id="bio-title">Personal Ratings</span>
                            <hr className="mt-0 mb-2" />
                            { loading && (<Spinner animation="border" />) }
                            {!loading && (<ReviewChart reviewScores={userProfile?.ratingCounts ?? [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]} />)}
                        </Col>
                    </Row>
                </div>
                <Col className="ps-md-4">
                    <Row id="profile-stats" className="mt-0">
                        <div className="col-4 col-lg-3 ms-auto">
                            {loading && (<Spinner animation="border" />)}
                            {!loading && (<h1>{userProfile?.totalGamesPlayed}</h1>)}
                            <h4>Total Games Played</h4>
                        </div>
                        <div className="col-4 col-lg-3">
                            {loading && (<Spinner animation="border" />)}
                            {!loading && (<h1>{userProfile?.gamesPlayedThisYear ?? 0}</h1>)}
                            <h4>Played in {currentYear}</h4>
                        </div>
                        <div className="col-4 col-lg-3 me-auto">
                            {loading && (<Spinner animation="border" />)}
                            {!loading && (<h1>{userProfile?.gamesBacklogged}</h1>)}
                            <h4>Games Backlogged</h4>
                        </div>
                    </Row>
                    <hr className="mt-2 mb-0" />
                    <Row className="mt-3">
                        <Col>
                            <h2 className="profile-section-header">Recently Reviewed</h2>
                            <Link className="secondary-link subtitle-text" to={`/users/${username}/reviews`}>See More</Link>
                        </Col>
                    </Row>
                    <Row className="mb-3">
                        <Col>
                            {loading && (<Spinner animation="border" />)}
                            {!loading && userProfile?.recentReviews.map((review) => (
                                <ReviewCard key={review.reviewId} reviewData={review} isUserSubpage={true} />
                            )) }
                        </Col>
                    </Row>
                </Col>
            </Row>
        </>
    )
};