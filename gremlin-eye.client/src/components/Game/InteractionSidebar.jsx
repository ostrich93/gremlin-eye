import { useCallback, useEffect, useState } from 'react';
import { Button, ButtonGroup, Card, Col, Row, ToggleButton } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { faHeart, faLayerGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useAuthState } from '../../contexts/AuthProvider';
import InteractiveRatings from './InteractiveRatings';
import GameLogInteractions from './GameLogInteractions';
import GameStatistics from './GameStatistics';
import apiClient from '../../config/apiClient';

//InteractionSidebar contains GameRatings component and 
const InteractionSidebar = ({ gameData }) => {
    const [gameLog, setGameLog] = useState(null);
    const [loading, setLoading] = useState(false);
    const [liked, setLike] = useState(false);
    const { user } = useAuthState();

    useEffect(() => {
        const fetchGameLog = async () => {
            if (user) {
                setLoading(true);
                apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs/${gameData?.id}`)
                    .then((res) => {
                        setGameLog(res.data);
                        setLoading(false);
                    })
                    .catch((err) => {
                        console.error("Error fetching game log: ", err);
                        setLoading(false);
                    });
            };

            fetchGameLog();
        }
    }, [gameData, user]);

    const generateEmptyGameLog = useCallback(() => {
        return {
            logId: -1,
            gameId: gameData.id,
            rating: null,
            playStatus: null,
            isPlayed: false,
            isPlaying: false,
            isBacklog: false,
            isWishList: false
        };
    }, [gameData]);

    const onUpdateGameLogField = useCallback((action, value) => {
        if (gameLog == null) {
            let gLog = generateEmptyGameLog();
            gLog[action] = value;
            apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/log`, { gameLogState: gLog })
                .then((res) => {
                    console.log(res.data);
                    gLog.gameId = res.data;
                    setGameLog(gLog);
                });
            //setGameLog(gLog);
        }
        else {
            const updatedLog = { ...gameLog, [action]: value };
            console.log(updatedLog);
            apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/log`, { gameLogState: updatedLog })
                .then((res) => {
                    setGameLog(updatedLog);
                });
            /*setGameLog({
                ...gameLog,
                [action]: value
            });*/
        }
    }, [gameLog, generateEmptyGameLog])

    const onUpdatePlayedState = useCallback((played) => {
        if (gameLog == null) {
            let gLog = generateEmptyGameLog();
            gLog.playStatus = played ? 0 : null;
            gLog.isPlayed = played;
            setGameLog(gLog);
        }
        else if (played === true && gameLog.playState == null) {
            setGameLog({
                ...gameLog,
                playState: 0,
                isPlayed: played
            });
        }
        else if (played === false) {
            setGameLog({
                ...gameLog,
                playState: null,
                isPlayed: played
            });
        }
    }, [gameLog, generateEmptyGameLog]);

    return (
        <div id="interaction-sidebar" className="col-12 col-sm-5 col-md-cus-30 col-lg-cus-23 col-xl-cus-21 me-sm-3">
            <Row>
                <Col className="col-cover px-sm-0 my-auto mx-auto mb-0 mb-sm-2 mb-lg-0">
                    <Card className="mx-auto game-cover">
                        <Card.Img src={gameData?.coverUrl} loading='lazy' />
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
                                                <Col id={`journal-${gameData?.id}`} className="journal-button-container">
                                                    <Button id="open-game-log-modal-btn" className="btn-main journal-btn mx-auto">
                                                        {gameLog != null ? "Edit your log" : "Log or Review"}
                                                    </Button>
                                                </Col>
                                            </Row>
                                            <InteractiveRatings gameLog={gameLog} onUpdateRating={onUpdateGameLogField} />

                                            <hr className="my-1" />
                                            <GameLogInteractions gameLog={gameLog}
                                                onPlayedUpdate={onUpdatePlayedState}
                                                onOtherUpdate={onUpdateGameLogField}
                                            />
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
                        <GameStatistics gameData={gameData} />
                    </Row>
                </Col>
            </Row>
        </div>
    );
};

export default InteractionSidebar;