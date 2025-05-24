import { Button, ButtonGroup, Card, Container, Col, Row, Spinner, ToggleButton } from 'react-bootstrap';
import Rate from 'rc-rate';
import "rc-rate/assets/index.css";
import { faAlignRight, faBook, faGamepad, faGift, faHeart, faPlay, faLayerGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { useAuthState } from "../contexts/AuthProvider";
import apiClient from '../config/apiClient';
import ReviewChart from '../components/Game/ReviewChart';
import './GamePage.css';

const GamePage = () => {
    const starCountRef = useRef(null);
    const { user } = useAuthState();
    const { slug } = useParams();
    const [gameData, setGameData] = useState(null);
    const [gameStats, setStats] = useState(null);
    const [gameId, setGameId] = useState(-1);
    const [playLog, setPlayLog] = useState(null);
    const [loading, setLoading] = useState(false);
    const [rating, setRating] = useState(0);
    const [liked, setLike] = useState(false);
    const [coverUrl, setCover] = useState(null);

    useEffect(() => {
        const fetchGame = async () => {
            setLoading(true);
            apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/${slug}`)
                .then((res) => {
                    setGameData(res.data);
                    setGameId(res.data.id);
                    setStats(res.data.stats ?? {
                        playedCount: 0,
                        playingCount: 0,
                        backlogCount: 0,
                        wishlistCount: 0,
                        averageRating: null,
                        ratingsCount: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
                    });
                    if (res.data.coverUrl) {
                        setCover(res.data.coverUrl)
                    }
                    if (user) {
                        setPlayLog(res.data.gameLog);
                    }
                    setLoading(false);
                })
                .catch((err) => {
                    console.error("Error fetching game data: ", err);
                    setLoading(false);
                });
        };

        fetchGame();

    }, [user, slug]);

    const getPlayStateColor = () => {
        if (playLog == null || !playLog.isPlayed)
            return 'gray';
        switch (playLog.playStatus) {
            case 0:
                return '#ea377a';
            case 1:
                return 'green';
            case 2:
                return 'blue';
            case 3:
                return 'orange';
            case 4:
                return 'red';
            default:
                return 'gray';
        }
    };

    return (
        <Container>
            <Row id="game-banner-art">
                <div id="gradient"></div>
                <Col className="px-0">
                    {loading && !gameData && (
                        <Spinner animation="border" />
                    ) }
                    {!loading && gameData && (
                        <img src={gameData?.bannerUrl} loading='lazy' />
                    )}
                </Col>
            </Row>
            <Row id="game-profile">
                <Col>
                    <Row id="game-body">
                        <div id="interaction-sidebar" className="col-12 col-sm-5 col-md-cus-30 col-lg-cus-23 col-xl-cus-21 me-sm-3">
                            <Row>
                                <Col className="col-cover px-sm-0 my-auto mx-auto mb-0 mb-sm-2 mb-lg-0">
                                    <Card className="mx-auto game-cover">
                                        <Card.Img src={coverUrl} loading='lazy' />
                                    </Card>
                                </Col>
                                <Col className="col col-sm-12 mt-3 mt-sm-3">
                                    <Row>
                                        <div id="logging-sidebar-section" className="col-7 col-sm-12">
                                            <Row className="mb-3">
                                                {!user && (
                                                    <Col className="text-center">
                                                        <p id="sign-in-text" className="mx-auto mb-0">
                                                            <Link to="/login">Log in</Link> to access rating features
                                                        </p>
                                                    </Col>
                                                )}
                                                {user && sessionStorage.getItem('access_token') && (
                                                    <Col>
                                                        <div className="side-section">
                                                            <Row>
                                                                <Col id={`journal-${gameId}`} className="journal-button-container">
                                                                    <Button id="open-game-log-modal-btn" className="btn-main journal-btn mx-auto">
                                                                        {playLog != null ? "Edit your log" : "Log or Review"}
                                                                    </Button>
                                                                </Col>
                                                            </Row>
                                                            <Row id={`rating${gameId}`} className="my-2 star-rating star-rating-game">
                                                                <Rate
                                                                    defaultValue={rating}
                                                                    value={rating}
                                                                    ref={starCountRef}
                                                                    allowHalf
                                                                    allowClear
                                                                    onChange={(value) => {
                                                                        setRating(value);
                                                                        if (starCountRef.current) {
                                                                            if (!starCountRef.current.state) {
                                                                                starCountRef.current.state = {};
                                                                            }
                                                                            starCountRef.current.state.value = value;
                                                                        }
                                                                    }}
                                                                />
                                                            </Row>
                                                            <hr className="my-1" />
                                                            <Row id="buttons" className="mx-0">
                                                                <Col id="play" className="px-0 play-btn-container mt-auto">
                                                                    <Button variant="link" className="mx-auto">
                                                                        <FontAwesomeIcon icon={faGamepad} size="2x" color={getPlayStateColor()} />
                                                                        <br />
                                                                        <p className="label">Played</p>
                                                                    </Button>
                                                                </Col>
                                                                <Col id="playing" className="px-0 playing-btn-container mt-auto">
                                                                    <Button variant="link" className="mx-auto">
                                                                        <FontAwesomeIcon icon={faPlay} size="2x" color={(playLog && playLog.isPlaying) ? '#ea377a' : 'gray' } />
                                                                        <br />
                                                                        <p className="label">Playing</p>
                                                                    </Button>
                                                                </Col>
                                                                <Col id="backlog" className="px-0 backlog-btn-container mt-auto">
                                                                    <Button variant="link" className="mx-auto">
                                                                        <FontAwesomeIcon icon={faBook} size="2x" color={(playLog && playLog.isBacklog) ? '#ea377a' : 'gray'} />
                                                                        <br />
                                                                        <p className="label">Backlog</p>
                                                                    </Button>
                                                                </Col>
                                                                <Col id="wishlist" className="px-0 wishlist-btn-container mt-auto">
                                                                    <Button variant="link" className="mx-auto">
                                                                        <FontAwesomeIcon icon={faGift} size="2x" color={(playLog && playLog.isWishlist) ? '#ea377a' : 'gray'} />
                                                                        <br />
                                                                        <p className="label">Wishlist</p>
                                                                    </Button>
                                                                </Col>
                                                            </Row>
                                                            <Row className="mt-2 d-none d-sm-flex">
                                                                <Col className="pe-1">
                                                                    <Button id="add-to-list" className="w-100" variant="outline-secondary">
                                                                        <FontAwesomeIcon icon={faLayerGroup} />
                                                                        Add to Lists
                                                                    </Button>
                                                                </Col>
                                                                <Col className="auto ps-1">
                                                                    <ButtonGroup>
                                                                        <ToggleButton
                                                                            id="add-to-list"
                                                                            className="w-100"
                                                                            type="checkbox"
                                                                            variant="outline-secondary"
                                                                            checked={liked}
                                                                            value="1"
                                                                            onChange={(e) => setLike(e.currentTarget.checked)}
                                                                        >
                                                                            <FontAwesomeIcon icon={faHeart} />
                                                                        </ToggleButton>
                                                                    </ButtonGroup>
                                                                </Col>
                                                            </Row>
                                                        </div>
                                                    </Col>
                                                )}
                                            </Row>
                                        </div>
                                        <Col className="col-ms-12 col-sm-12 d-none d-sm-flex">
                                            <Row>
                                                <Col>
                                                    <div className="side-section">
                                                        <Row>
                                                            <Col id="game-rating" className="mx-auto game-rating">
                                                                <p className="text-center mt-3 mb-0">Avg Rating</p>
                                                                <h1 className="text-center">{gameData?.averageRating ?? 'N/A'}</h1>
                                                            </Col>
                                                        </Row>
                                                        <Row id="rating-bars-height" className="mx-0">
                                                            <ReviewChart reviewScores={gameStats?.reviewCounts ?? [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]} />
                                                        </Row>
                                                        <hr className="my-2" />
                                                        <Row className="mt-1 log-counters">
                                                            <div className="col-12 mb-1">
                                                                <Link className="plays-counter" to={`/logs/${slug}/plays`}>
                                                                    <Row>
                                                                        <div className="col-auto pe-0">
                                                                            <p>
                                                                                <FontAwesomeIcon icon={faGamepad} color='#ea377a' />
                                                                                Plays
                                                                            </p>
                                                                        </div>
                                                                        <div className="col-auto ms-auto ps-0">
                                                                            <p className="mb-0">{gameStats?.playedCount ?? 0}</p>
                                                                        </div>
                                                                    </Row>
                                                                </Link>
                                                            </div>
                                                            <div className="col-12 mb-1">
                                                                <Link className="plays-counter" to={`/logs/${slug}/playing`}>
                                                                    <Row>
                                                                        <div className="col-auto pe-0">
                                                                            <p>
                                                                                <FontAwesomeIcon icon={faPlay} color='#ea377a' />
                                                                                Playing
                                                                            </p>
                                                                        </div>
                                                                        <div className="col-auto ms-auto ps-0">
                                                                            <p className="mb-0">{gameStats?.playingCount ?? 0}</p>
                                                                        </div>
                                                                    </Row>
                                                                </Link>
                                                            </div>
                                                            <div className="col-12 mb-1">
                                                                <Link className="plays-counter" to={`/logs/${slug}/backlogs`}>
                                                                    <Row>
                                                                        <div className="col-auto pe-0">
                                                                            <p>
                                                                                <FontAwesomeIcon icon={faBook} color='#ea377a' />
                                                                                Backlogs
                                                                            </p>
                                                                        </div>
                                                                        <div className="col-auto ms-auto ps-0">
                                                                            <p className="mb-0">{gameStats?.backlogCount ?? 0}</p>
                                                                        </div>
                                                                    </Row>
                                                                </Link>
                                                            </div>
                                                            <div className="col-12 mb-1">
                                                                <Link className="plays-counter" to={`/logs/${slug}/wishlists`}>
                                                                    <Row>
                                                                        <div className="col-auto pe-0">
                                                                            <p>
                                                                                <FontAwesomeIcon icon={faGift} color='#ea377a' />
                                                                                Plays
                                                                            </p>
                                                                        </div>
                                                                        <div className="col-auto ms-auto ps-0">
                                                                            <p className="mb-0">{gameStats?.wishlistCount ?? 0}</p>
                                                                        </div>
                                                                    </Row>
                                                                </Link>
                                                            </div>
                                                        </Row>
                                                        <hr className="mt-1 mb-2" />
                                                    </div>
                                                </Col>
                                            </Row>
                                        </Col>
                                    </Row>
                                </Col>
                            </Row>
                        </div>
                        <Col>
                            <Row id="title" className="d-none d-sm-flex mx-n1">
                                <div className="col-12 px-1">
                                    <Row>
                                        <div className="col-auto pe-1">
                                            <h1 className="mb-0">{gameData?.name}</h1>
                                        </div>
                                    </Row>
                                </div>
                                <div className="col-auto mt-auto pe-0 ps-1">
                                    <span className="sub-title">
                                        <span className="filler-text">released on </span>
                                        <Link to={`/games/lib/popular/release_year:${gameData?.date}`}>${gameData?.date}</Link>
                                    </span>
                                </div>
                                <div className="col-auto mt-auto pe-0 ps-1">
                                    <span className="filler-text">by</span>
                                </div>
                                {gameData?.companies.map((company) => (
                                    <div key={company.id} className="col-auto sub-title ps-1 pe-0">
                                        <Link to={`/company/${company.slug}`}>{company.name} </Link>
                                    </div>
                                ))}
                            </Row>
                            <Row>
                                <Col id="center-content" className="px-3 mt-lg-2 my-3 my-md-1">
                                    <Row>
                                        <Col>
                                            <div id="collapseSummary" className="mb-2">
                                                <p className="mb-0">{gameData?.summary}</p>
                                            </div>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col className="my-auto">
                                            <hr className="my-auto" />
                                        </Col>
                                    </Row>
                                    <Row className="mt-2 d-none d-md-flex">
                                        <div className="col-4 col-xl-3 pe-1">
                                            <Link to={`/lists/${slug}`}>
                                                <p className="game-page-sidecard">
                                                    <FontAwesomeIcon icon={faLayerGroup} />
                                                    {gameData?.listCount} Lists
                                                </p>
                                            </Link>
                                        </div>
                                        <div className="col-4 col-xl-3 pe-1">
                                            <Link to={`/reviews/everyone/eternity/recent/${slug}`}>
                                                <p className="game-page-sidecard">
                                                    <FontAwesomeIcon icon={faAlignRight} />
                                                    {gameData?.reviewCount} Reviews
                                                </p>
                                            </Link>
                                        </div>
                                        <div className="col-4 col-xl-3 pe-1">
                                            <Link to={`/games/${slug}/likes`}>
                                                <p className="game-page-sidecard">
                                                    <FontAwesomeIcon icon={faHeart} />
                                                    {gameData?.likeCount} Likes
                                                </p>
                                            </Link>
                                        </div>
                                    </Row>
                                    {gameData?.series && (
                                        <>
                                            <Row className="mt-3">
                                                <Col>
                                                    <h2 className="mb-0">Also in series</h2>
                                                </Col>
                                                <Col className="col-auto">
                                                    <Link className="secondary-link smaller-font" to={`/series/${gameData?.series.slug}`}>
                                                        See more
                                                    </Link>
                                                </Col>
                                            </Row>
                                            <Row className="mt-2 mx-n1">
                                                {gameData?.series.games.map((entry) => (
                                                    <Col className="col-cus px-1">
                                                        <Card key={entry.id} className="mx-auto game-cover quick-access">
                                                            <Card.Img src={entry.coverUrl} alt={entry.name} />
                                                            <Link to={`/games/${entry.slug}`} className="cover-link" />
                                                        </Card>
                                                    </Col>
                                                ))}
                                            </Row>
                                        </>
                                    ) }
                                </Col>
                                <Col className="col-12 col-lg-cus-32 mt-1 mt-lg-2">
                                    <Row>
                                        <Col>
                                            <p className="mb-1 subtitle-text">Released on</p>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col>
                                            {gameData?.platforms.map((platform) => (
                                                <Link className="game-page-platform" key={platform.id} to={`/games/lib/popular/release_platform:${platform.slug}`}>{platform.name}</Link>
                                            )) }
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col>
                                            <p className="mb-0 subtitle-text">Genres</p>
                                        </Col>
                                    </Row>
                                    {gameData?.genres && (
                                        <Row>
                                            <Col>
                                                {gameData?.genres.map((genre) => (
                                                    <p key={genre.id} className="genre-tag">
                                                        <Link to={`/games/lib/popular/genre:${genre.slug}`}>{genre.name}</Link>
                                                    </p>
                                                )) }
                                            </Col>
                                        </Row>
                                    )}
                                    <Row className="mt-3">
                                        <Col className="col-auto my-auto pe-1">
                                            <p className="subtitle-text">More info on <a href={`https://www.igdb.com/games/${gameData?.slug}`}>IGDB</a></p>
                                        </Col>
                                        <Col className="my-auto px-1">
                                            <hr className="my-auto" />
                                        </Col>
                                    </Row>
                                </Col>
                            </Row>
                            <Row id="gameReviews" className="mt-4">
                                <Col>
                                    <Row id="review-header">
                                        <Col>
                                            <h2 className="me-auto mb-0">
                                                Reviews
                                                <Link className="secondary-link smaller-font ml-2" to={`/reviews/everyone/week/recent/${gameData?.slug}`}>View More</Link>
                                            </h2>
                                        </Col>
                                        <Col className="my-auto">
                                            <ul id="nav-interactables" className="nav">
                                                <p className="my-auto subtitle-text sort-heading">Sort by</p>
                                            </ul>
                                        </Col>
                                    </Row>
                                    <Row className="mt-2">
                                        <Col id="game-reviews-section">
                                            
                                        </Col>
                                    </Row>
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                </Col>
            </Row>
        </Container>
    );
};

export default GamePage;