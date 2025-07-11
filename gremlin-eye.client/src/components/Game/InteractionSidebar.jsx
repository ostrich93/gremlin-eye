import { useEffect, useRef, useState } from 'react';
import { Button, ButtonGroup, Col, Row, Spinner, ToggleButton } from 'react-bootstrap';
import ReactModal from 'react-modal';
//import { Link } from 'react-router-dom';
import Rate from 'rc-rate';
import "rc-rate/assets/index.css";
import { faBook, faGamepad, faGift, faHeart, faLayerGroup, faPlay } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useAuthState } from '../../contexts/AuthProvider';
import { useJournalDispatch, useJournalState } from '../../contexts/JournalProvider';
import apiClient from '../../config/apiClient';
import JournalModalContent from './JournalModal';
import PlayStatusModalContent from './PlayStatusModal';

const playStateColors = ['#ea377a', 'green', 'blue', 'orange', 'red'];
const defaultPlayedStateColor = 'gray';

//InteractionSidebar contains GameRatings component and 
const InteractionSidebar = ({ slug }) => {

    const [logId, setLogId] = useState(-1);
    const [gameId, setGameId] = useState(-1);
    const [rating, setRating] = useState(0);
    const [playStatus, setPlayStatus] = useState(null);
    const [played, setPlayed] = useState(false);
    const [playing, setPlaying] = useState(false);
    const [backlog, setBacklog] = useState(false);
    const [wishlist, setWishlist] = useState(false);
    const [liked, setLike] = useState(false);
    const [loading, setLoading] = useState(false);
    const [playthroughs, setPlaythroughs] = useState([]);

    const [showPlayStatusModal, setShowPlayStatusModal] = useState(false);
    //const [showJournalModal, setShowJournalModal] = useState(false);

    const { user } = useAuthState();
    const { showJournalModal } = useJournalState();
    const dispatch = useJournalDispatch();

    const starCountRef = useRef(null);

    //include JournalModal here

    useEffect(() => {
        const fetchGameLog = async () => {
            setLoading(true);
            apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs/${slug}`)
                .then((res) => {
                    if (res.data) {
                        setLogId(res.data.logId);
                        setGameId(res.data.gameId);
                        setRating(res.data.rating ? res.data.rating / 2 : 0);
                        setPlayStatus(res.data.playStatus);
                        setPlayed(res.data.isPlayed);
                        setPlaying(res.data.isPlaying);
                        setBacklog(res.data.isBacklog);
                        setWishlist(res.data.isWishList);
                        setPlaythroughs(res.data.playthroughs)
                    }
                    setLoading(false);
                })
                .catch((err) => {
                    console.error("Error fetching game log: ", err);
                    setLoading(false);
                });
        }

        if (user != null) {
            fetchGameLog();
        }
    }, [slug, user]);

    const togglePlayed = (e) => {
        e.preventDefault();
        console.log("gameId: ", gameId);
        apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs`, { type: 0, gameId: gameId })
            .then((res) => {
                if (res.data) {
                    if (logId <= -1) {
                        setLogId(res.data);
                    }
                    if (played) { //previous state is played, so turn off playStatus
                        setPlayStatus(null);
                    }
                    else {
                        setPlayStatus(0);
                    }
                    setPlayed(!played);
                    setShowPlayStatusModal(false);
                }
            })
            .catch((err) => {
                console.error(err);
                setShowPlayStatusModal(false);
            });
    };

    const togglePlaying = (e) => {
        e.preventDefault();
        apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs`, { type: 1, gameId: gameId })
            .then((res) => {
                if (res.data) {
                    if (logId > -1) {
                        setLogId(res.data);
                    }
                    setPlaying(!playing);
                }
            });
    };

    const toggleBacklog = (e) => {
        e.preventDefault();
        apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs`, { type: 2, gameId: gameId })
            .then((res) => {
                if (res.data) {
                    if (logId <= -1) {
                        setLogId(res.data);
                    }
                    setBacklog(!backlog);
                }
            });
    };

    const toggleWishlist = (e) => {
        e.preventDefault();
        apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs`, { type: 3, gameId: gameId })
            .then((res) => {
                if (res.data) {
                    if (logId <= -1) {
                        setLogId(res.data);
                    }
                    setWishlist(!wishlist);
                }
            });
    };

    const handleStatusChange = (e, statusValue) => {
        e.preventDefault();
        apiClient.patch(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs/status`, { gameId: gameId, status: statusValue })
            .then((res) => {
                setPlayStatus(statusValue);
                setShowPlayStatusModal(false);
            })
            .catch((err) => {
                console.error(err);
                setShowPlayStatusModal(false);
            });
    };

    const handlePlayedClick = (e) => {
        e.preventDefault();
        if (!played) {
            togglePlayed(e);
        }
        else {
            setShowPlayStatusModal(true);
        }
    };

    const handleClosePlayStatusModal = () => setShowPlayStatusModal(false);
    const handleCloseJournalModal = () => dispatch({ type: "CLOSE_JOURNAL_MODAL" });

    const handleRating = (rateValue) => {
        apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/rate/${gameId}`, { rating: 2 * rateValue })
            .then((res) => {
                if (res.data) {
                    if (logId <= -1) {
                        setLogId(res.data);
                    }
                    if (starCountRef.current) {
                        if (!starCountRef.current.state) {
                            starCountRef.current.state = {};
                        }
                        starCountRef.current.state.value = rateValue;
                    }
                    setRating(rateValue);
                }
            });
    };

    const handleOpenJournal = () => dispatch({ type: "OPEN_JOURNAL_MODAL", payload: { gameId: gameId } });

    return (
        <>
            
            <ReactModal
                isOpen={showPlayStatusModal}
                onRequestClose={handleClosePlayStatusModal}
                shouldCloseOnOverlayClick={true}
            >
                <PlayStatusModalContent handleStatusChange={handleStatusChange} togglePlayed={togglePlayed} />
            </ReactModal>
            <Col>
                <div className="side-section">
                    {loading && (
                        <Spinner animation="border" />
                    )}
                    {!loading && gameId && (
                        <>
                            <Row>
                                <Col id={`journal-${gameId}`} className="journal-button-container">
                                    <Button id="open-game-log-modal-btn" className="btn-main journal-btn mx-auto" onClick={handleOpenJournal}>
                                        {logId != null ? "Edit your log" : "Log or Review"}
                                    </Button>
                                </Col>
                            </Row>

                            <Row id={`rating${gameId}`} className="my-2 star-rating star-rating-game">
                                <Col className="col-auto mx-auto px-0 star-rating-field rate">
                                    <Rate
                                        defaultValue={rating ?? 0}
                                        value={rating ?? 0}
                                        ref={starCountRef}
                                        allowHalf
                                        allowClear
                                        onChange={(value) => handleRating(value)}
                                    />
                                </Col>
                            </Row>

                            <hr className="my-1" />

                            <Row id="buttons" className="mx-0">
                                <Col id="play" className="px-0 mt-auto btn-play-fill">
                                    <Button variant="link" className="btn-play-fill btn-play btn-played mx-auto" onClick={handlePlayedClick}>
                                        <FontAwesomeIcon icon={faGamepad} size="2x" color={(played && playStatus != null) ? playStateColors[playStatus] : defaultPlayedStateColor} />
                                        <br />
                                        <p className="label">Played</p>
                                    </Button>
                                </Col>
                                <Col id="playing" className="px-0 mt-auto">
                                    <Button variant="link" className="btn-play mx-auto" onClick={togglePlaying}>
                                        <FontAwesomeIcon icon={faPlay} size="2x" color={playing ? '#ea377a' : defaultPlayedStateColor} />
                                        <br />
                                        <p className="label">Playing</p>
                                    </Button>
                                </Col>
                                <Col id="backlog" className="px-0 mt-auto">
                                    <Button variant="link" className="btn-play mx-auto" onClick={toggleBacklog}>
                                        <FontAwesomeIcon icon={faBook} size="2x" color={backlog ? '#ea377a' : defaultPlayedStateColor} />
                                        <br />
                                        <p className="label">Backlog</p>
                                    </Button>
                                </Col>
                                <Col id="wishlist" className="px-0 mt-auto">
                                    <Button variant="link" className="btn-play mx-auto" onClick={toggleWishlist}>
                                        <FontAwesomeIcon icon={faGift} size="2x" color={wishlist ? '#ea377a' : defaultPlayedStateColor} />
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
                                <div className="col-auto ps-1">
                                    <ButtonGroup>
                                        <ToggleButton
                                            className="like-game-btn px-0"
                                            type="checkbox"
                                            variant="outline-secondary"
                                            checked={liked}
                                            value="1"
                                            onChange={(e) => setLike(e.currentTarget.checked)}
                                        >
                                            <FontAwesomeIcon icon={faHeart} />
                                        </ToggleButton>
                                    </ButtonGroup>
                                </div>
                            </Row>
                        </>
                    )}
                </div>
                {logId > -1 && (
                    <Row className="my-2 none d-none d-sm-flex">
                        <Col id="log-again-btn">
                            <Button id="open-new-game-log-modal-btn" className="w-100" variant="outline-secondary">
                                <p className="mb-0">Log or review again</p>
                            </Button>
                        </Col>
                    </Row>
                ) }
            </Col>

            <ReactModal
                isOpen={showJournalModal}
                onRequestClose={handleCloseJournalModal}
            >
                <JournalModalContent />
            </ReactModal>
        </>
    );
};

export default InteractionSidebar;