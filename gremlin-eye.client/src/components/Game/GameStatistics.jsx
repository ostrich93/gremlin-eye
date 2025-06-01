import { memo } from 'react'; 
import { Col, Row } from 'react-bootstrap';
import { faBook, faGamepad, faGift, faPlay } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import ReviewChart from './ReviewChart';

const GameStatistics = memo(({ gameData }) => {
    return (
        <>
            <div className="col-ms-12 col-sm-12 d-none d-sm-flex">
                <Row>
                    <Col>
                        <div className="side-section">
                            <Row>
                                <Col id="game-rating" className="mx-auto game-rating">
                                    <p className="text-center mt-3 mb-0">Avg Rating</p>
                                    <h1 className="text-center">{gameData?.stats && gameData?.stats?.averageRating ? gameData?.stats?.averageRating : 'N/A'}</h1>
                                </Col>
                            </Row>
                            
                            <ReviewChart reviewScores={gameData?.stats?.ratingCounts ?? [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]} />
                            <hr className="my-2" />
                            <Row className="mt-1 log-counters">
                                <div className="col-12 mb-1">
                                    <Link className="plays-counter" to={`/logs/${gameData?.slug}/plays`}>
                                        <Row>
                                            <div className="col-auto pe-0">
                                                <p>
                                                    <FontAwesomeIcon icon={faGamepad} color='#ea377a' />
                                                    Plays
                                                </p>
                                            </div>
                                            <div className="col-auto ms-auto ps-0">
                                                <p className="mb-0">{gameData?.stats?.playedCount ?? 0}</p>
                                            </div>
                                        </Row>
                                    </Link>
                                </div>
                                <div className="col-12 mb-1">
                                    <Link className="plays-counter" to={`/logs/${gameData?.slug}/playing`}>
                                        <Row>
                                            <div className="col-auto pe-0">
                                                <p>
                                                    <FontAwesomeIcon icon={faPlay} color='#ea377a' />
                                                    Playing
                                                </p>
                                            </div>
                                            <div className="col-auto ms-auto ps-0">
                                                <p className="mb-0">{gameData?.stats?.playingCount ?? 0}</p>
                                            </div>
                                        </Row>
                                    </Link>
                                </div>
                                <div className="col-12 mb-1">
                                    <Link className="plays-counter" to={`/logs/${gameData?.slug}/backlogs`}>
                                        <Row>
                                            <div className="col-auto pe-0">
                                                <p>
                                                    <FontAwesomeIcon icon={faBook} color='#ea377a' />
                                                    Backlogs
                                                </p>
                                            </div>
                                            <div className="col-auto ms-auto ps-0">
                                                <p className="mb-0">{gameData?.stats?.backlogCount ?? 0}</p>
                                            </div>
                                        </Row>
                                    </Link>
                                </div>
                                <div className="col-12 mb-1">
                                    <Link className="plays-counter" to={`/logs/${gameData?.slug}/wishlists`}>
                                        <Row>
                                            <div className="col-auto pe-0">
                                                <p>
                                                    <FontAwesomeIcon icon={faGift} color='#ea377a' />
                                                    Plays
                                                </p>
                                            </div>
                                            <div className="col-auto ms-auto ps-0">
                                                <p className="mb-0">{gameData?.stats?.wishlistCount ?? 0}</p>
                                            </div>
                                        </Row>
                                    </Link>
                                </div>
                            </Row>
                            <hr className="mt-1 mb-2" />
                        </div>
                    </Col>
                </Row>
            </div>
        </>
    );
});

export default GameStatistics;