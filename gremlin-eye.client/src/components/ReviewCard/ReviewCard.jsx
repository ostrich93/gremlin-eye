import { useState } from "react";
import { Button, Col, Row, ToggleButton } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import Rate from 'rc-rate';
import "rc-rate/assets/index.css";
import { faComment, faHeart } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const ReviewCard = ({ reviewData, isUserSubpage }) => {

    const [spoilersOpen, setSpoilersOpen] = useState(false);

    const renderCover = () => {
        return (
            <div className="col-2">
                <Row>
                    <Col className="pe-0 pe-md-2">
                        <Link to={`games/${reviewData.gameSlug}`}>
                            <div className="card mx-auto game-cover overlay-hide" data-game_id={reviewData.gameId}>
                                <div className="overflow-wrapper">
                                    <img className="card-img height" src={reviewData.coverUrl} />
                                    <div className="overlay" />
                                </div>
                            </div>
                        </Link>
                    </Col>
                </Row>
            </div>
        );
    };

    const renderAvatar = () => {
        return (
            <div id="avatar" className="col-auto mb-auto pe-0">
                <Link to={`/user/${reviewData.username}`}>
                    <img src={reviewData.avatarUrl.length > 0 ? reviewData.avatarUrl : "/no_avatar.png"} height="40" width="40" />
                </Link>
            </div>
        );
    };

    return (
        <Row className="pt-2 pb-1 review-card">
            <Col>
                <Row>
                    {isUserSubpage ? renderCover() : renderAvatar()}
                    <Col>
                        <Row className="top-bar mb-1">
                            <Col>
                                <Row className="mb-1">
                                    <Link className="mb-0 my-auto d-flex" to={`/user/${reviewData.username}`}>
                                        <div className="col-auto my-auto username-link pe-0 me-n2">
                                            <p className="mb-0">{reviewData.username}</p>
                                        </div>
                                    </Link>
                                </Row>
                                <Row className="mx-0">
                                    {reviewData.rating && (
                                        <div className="col-auto my-auto ps-0 pe-1">
                                            <Row className="star-ratings-static">
                                                <Rate
                                                    defaultValue={reviewData.rating/2}
                                                    value={reviewData.rating/2}
                                                    allowHalf
                                                    disabled={true}
                                                />
                                            </Row>
                                        </div>
                                    )}
                                    <div className="col-auto my-auto game-status ms-2 ps-2 pe-1">
                                        <Link to={`/user/${reviewData.username}/games?type=played&status=${reviewData.playStatus}`}>
                                            <p className="mb-0 play-type">{reviewData.playStatus}</p>
                                        </Link>
                                    </div>
                                    {reviewData.platform && (
                                        <>
                                            <div className="col-auto my-auto ps-0 pe-1 filler-text">
                                                <p className="mb-0">on</p>
                                            </div>
                                            <div className="col-auto my-auto ps-0 pe-1">
                                                <Link className="my-0 ms-auto review-platform" to={`/user/${reviewData.username}/games?played&=platform=${reviewData.platform.slug}`}>
                                                    <p className="mb-0">{reviewData.platform.name}</p>
                                                </Link>
                                            </div>
                                        </>
                                    )}
                                </Row>
                            </Col>
                        </Row>
                        {(reviewData.containsSpoilers && !spoilersOpen) && (
                            <Row className="my-2 spoiler-warning" data-review_id={reviewData.reviewId}>
                                <div className="col-auto mx-auto">
                                    <p className="mb-2 subtitle-text">This review contains spoilers</p>
                                    <Button className="btn-general btn-small d-flex mx-auto" data-review_id={reviewData.reviewId} onClick={() => setSpoilersOpen(true)}>I'm ready</Button>
                                </div>
                            </Row>
                        )}
                        {(!reviewData.containsSpoilers || spoilersOpen) && (
                            <Row className="mt-2 mb-3 review-body" data-review_id={reviewData.reviewId}>
                                <Col>
                                    <div className="position-relative">
                                        <div id={`collapseReview${reviewData.reviewId}`} className="mb-0 card-text">
                                            {reviewData.reviewText}
                                        </div>
                                    </div>
                                </Col>
                            </Row>
                        )}
                        <Row className="review-bottom-bar">
                            <div id={`like_${reviewData.reviewId}`} className="col-auto mt-auto fav-review pe-1">
                                <ToggleButton className="like-heart px-0" variant="link" id="like-review" checked={reviewData.userLikes} value={reviewData.userLikes}>
                                    <p className="my-auto">
                                        <FontAwesomeIcon icon={faHeart} />
                                    </p>
                                </ToggleButton>
                                <p className="mb-0 d-inline-block like-counter">{reviewData.totalLikes}</p>
                            </div>
                            <div className="col-auto my-auto pe-1">
                                <Link className="comments-link" to={`/user/${reviewData.username}/review/${reviewData.reviewId}#comments`}>
                                    <FontAwesomeIcon icon={faComment} />
                                    {reviewData.commentCount}
                                </Link>
                            </div>
                            <div className="col-auto my-auto pe-1">
                                <Link className="open-review-link" to={`/user/${reviewData.username}/review/${reviewData.reviewId}`}>Open Review</Link>
                            </div>
                            {/** Include Expand area here **/ }
                        </Row>
                    </Col>
                </Row>
                <hr />
            </Col>
        </Row>
    );
};

export default ReviewCard;