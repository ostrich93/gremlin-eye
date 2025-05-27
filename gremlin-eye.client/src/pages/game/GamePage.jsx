import { Card, Container, Col, Row, Spinner } from 'react-bootstrap';
import { faAlignRight, faHeart, faLayerGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { useAuthState } from "../../contexts/AuthProvider";
import apiClient from '../../config/apiClient';
import './GamePage.css';
import InteractionSidebar from '../../components/Game/InteractionSidebar';

const GamePage = () => {
    const { user } = useAuthState();
    const { slug } = useParams();
    const [gameData, setGameData] = useState(null);
    /*const [gameStats, setStats] = useState(null);
    const [gameId, setGameId] = useState(-1);
    const [playLog, setPlayLog] = useState(null);*/
    const [loading, setLoading] = useState(false);
    //const [rating, setRating] = useState(0);
    /*const [liked, setLike] = useState(false);
    const [coverUrl, setCover] = useState(null);*/

    useEffect(() => {
        const fetchGame = async () => {
            setLoading(true);
            apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/${slug}`)
                .then((res) => {
                    setGameData({
                        id: res.data.id,
                        name: res.data.name,
                        slug: res.data.slug,
                        coverUrl: res.data.coverUrl,
                        bannerUrl: res.data.bannerUrl,
                        summary: res.data.summary,
                        date: res.data.date,
                        platforms: res.data.platforms,
                        companies: res.data.companies,
                        series: res.data.series,
                        stats: res.data.stats ?? {
                            playedCount: 0,
                            playingCount: 0,
                            backlogCount: 0,
                            wishlistCount: 0,
                            averageRating: null,
                            ratingsCount: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
                        },
                        listCount: res.data.listCount,
                        reviewCount: res.data.reviewCount,
                        likeCount: res.data.likeCount,
                        topReviews: res.data.topReviews,
                        relatedGames: res.data.relatedGames
                    });
                    setLoading(false);
                })
                .catch((err) => {
                    console.error("Error fetching game data: ", err);
                    setLoading(false);
                });
        };

        fetchGame();

    }, [user, slug]);

    /*const getPlayStateColor = () => {
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
    };*/

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
                        <InteractionSidebar gameData={gameData}
                        />
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