import { useEffect, useState } from "react";
import { Button, Card, Col, Container, Dropdown, Nav, Row, Tab, ToggleButton } from "react-bootstrap";
import ReactModal from "react-modal";
import { faBook, faCaretDown, faCirclePlus, faEllipsisVertical, faExclamationTriangle, faGamepad, faHeart, faGift, faPlay, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import apiClient from "../../config/apiClient";
import formatDate from '../../utils/formatDate';
import JournalForm from "./JournalForm";
import { useJournalDispatch, useJournalState } from "../../contexts/JournalProvider";
import PlayStatusModalContent from "./PlayStatusModal";

//The JournalModal can be accessed either directly from a game page OR by clicking "Log a Game" in the Navbar, searching, and selecting a result.
//In order to work with both cases, the JournalModal takes in a gameId and a userId to load the gamelog and playthroughs

const playStatusEnumStrings = ["played", "completed", "retired", "shelved", "abandoned"];
const playStatusEnumDisplayStrings = ["Played", "Completed", "Retired", "Shelved", "Abandoned"];

const JournalModalContent = () => {
    const { gameId } = useJournalState();
    const dispatch = useJournalDispatch();

    const [gameLogForm, setGameLogForm] = useState(null);
    const [playthroughDrafts, setPlaythroughDrafts] = useState([]);
    const [loading, setLoading] = useState(false);
    const [plIdState, setPlIdState] = useState(-1);
    const [activeKey, setActiveKey] = useState(-1);
    const [currentPlayStatusOption, setCurrentPlayStatusOption] = useState(1);

    const [showJournalCloseWarning, setShowJournalCloseWarning] = useState(false);
    const [showPlayStatusModal, setShowPlayStatusModal] = useState(false);

    useEffect(() => {
        const fetchGameJournal = async () => {
            setLoading(true);
            apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs/edit/${gameId}`)
                .then((res) => {
                    setGameLogForm({
                        logId: res.data.logId,
                        gameId: gameId,
                        gameName: res.data.gameName,
                        releaseDate: res.data.releaseDate,
                        coverUrl: res.data.coverUrl,
                        platforms: res.data.platforms,
                        playStatus: res.data.playStatus ?? null,
                        isPlayed: res.data.isPlayed,
                        isPlaying: res.data.isPlaying,
                        isBacklog: res.data.isBacklog,
                        isWishlist: res.data.isWishlist,
                        playthroughsToDelete: []
                    });
                    if (res.data.playStatus != null) {
                        setCurrentPlayStatusOption(res.data.playStatus);
                    }
                    setPlaythroughDrafts(res.data.playthroughs);
                    setActiveKey(res.data.playthroughs ? res.data.playthroughs[res.data.playthroughs.length - 1].playthroughId : -1);
                    setLoading(false);
                })
                .catch((err) => {
                    console.error("Error fetching game log: ", err);
                    setLoading(false);
                });
        }
        if (gameId > -1)
            fetchGameJournal();
    }, [gameId]);

    const handleCloseJournalModal = () => {
        dispatch({ type: "CLOSE_JOURNAL_MODAL" });
    };

    const addPlaythrough = () => {
        if (playthroughDrafts[playthroughDrafts.length - 1].playthroughId >= 0) {
            let newPlaythrough = {
                playthroughId: plIdState,
                gameId: gameLogForm?.gameId,
                logId: gameLogForm?.logId,
                logTitle: 'Log',
                reviewText: '',
                containsSpoilers: false,
                isReplay: false,
                medium: null,
                rating: 0,
                platform: null,
                playLogs: []
            };
            setPlaythroughDrafts([...playthroughDrafts, newPlaythrough]);
            setActiveKey(plIdState);
        }
        else {
            let newPlaythrough = {
                playthroughId: plIdState - 1,
                gameId: gameLogForm?.gameId,
                logId: gameLogForm?.logId,
                logTitle: 'Log',
                reviewText: '',
                containsSpoilers: false,
                isReplay: false,
                medium: null,
                rating: 0,
                platform: null,
                playLogs: []
            };
            setPlaythroughDrafts([...playthroughDrafts, newPlaythrough]);
            setActiveKey(plIdState - 1);
        }
        setPlIdState(plIdState - 1);
    };

    const removePlaythrough = (playthroughId) => {
        if (playthroughDrafts.length <= 1) {
            handleCloseJournalModal();
            return;
        }
        if (activeKey === playthroughDrafts[playthroughDrafts.length - 1].playthroughId) {
            setActiveKey(playthroughDrafts[playthroughDrafts.length - 2].playthroughId);
        }
        setPlaythroughDrafts(playthroughDrafts => playthroughDrafts.filter(playthrough => playthrough.playthroughId !== playthroughId));
        if (playthroughId >= 0) {
            //console.log([...gameLogForm.playthroughsToDelete, playthroughId]);
            setGameLogForm(gameLogForm => ({
                ...gameLogForm,
                playthroughsToDelete: [...gameLogForm.playthroughsToDelete, playthroughId]
            }));
        }
    };

    const updatePlaythrough = (playthroughId, field, value) => {
        setPlaythroughDrafts(playthroughDrafts => playthroughDrafts.map((playthrough) => {
            if (playthrough.playthroughId === playthroughId) {
                return { ...playthrough, [field]: value };
            }
            else {
                return playthrough;
            }
        }));
    };

    const submitGameLogForm = async () => {
        //submit log
        apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs/${gameId}`,
            {
                logId: gameLogForm?.logId,
                gameId: gameId,
                playStatus: gameLogForm?.playStatus ?? null,
                isPlayed: gameLogForm?.isPlayed,
                isPlaying: gameLogForm?.isPlaying,
                isBacklog: gameLogForm?.isBacklog,
                isWishlist: gameLogForm?.isWishlist,
                playthroughs: playthroughDrafts,
                playthroughsToDelete: gameLogForm?.playthroughsToDelete
            }
        ).then(() => {
            handleCloseJournalModal();
        });
    };

    const destroyGameLogData = async () => {
        if (gameLogForm.logId < 0) {
            handleCloseJournalModal();
        }
        apiClient.delete(`${import.meta.env.VITE_APP_BACKEND_URL}/api/logs/unlog`, { data: { logId: gameLogForm.logId, gameId: gameLogForm.gameId } })
            .then(() => handleCloseJournalModal())
            .catch((err) => console.error(err));
    };

    const updateGameLogStatusValue = (e, statusValue) => {
        e.preventDefault();
        if (statusValue < 0 || statusValue > 4) {
            console.log("If you're seeing this in the console, it's a bug");
            return;
        }
        setCurrentPlayStatusOption(statusValue);
        setGameLogForm({ ...gameLogForm, isPlayed: true, playStatus: statusValue });
        setShowPlayStatusModal(false);
    };

    const markUnplayed = (e) => {
        e.preventDefault();
        setGameLogForm({ ...gameLogForm, playStatus: null, isPlayed: false });
        setShowPlayStatusModal(false);
    };

    const handleDeletePlaylog = (e) => {
        e.preventDefault();
        //apiClient.delete();
        dispatch({ type: "CLOSE_JOURNAL_MODAL" });
    };
    
    const handleJournalWarningClose = () => {
        setShowJournalCloseWarning(false);
        handleCloseJournalModal();
    };

    return (
        <>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-body py-0">
                        <Row id="log-editor-full">
                            {!loading && (
                                <Col>
                                    <Row id="log-editor-library-row" className="pb-2">
                                        <Col id="log-editor-library-column" className="pt-3">
                                            <Row>
                                                <div id="log-editor-game-cover" className="col-1">
                                                    <Card className="mx-auto game-cover">
                                                        <Card.Img src={gameLogForm?.coverUrl} />
                                                    </Card>
                                                </div>
                                                <div className="col-auto my-auto ps-2">
                                                    <Row>
                                                        <Col>
                                                            <h2 className="main-header mb-0">
                                                                {gameLogForm?.gameName}
                                                                <small className="subtitle-text">{formatDate(gameLogForm?.releaseDate)}</small>
                                                            </h2>
                                                        </Col>
                                                    </Row>
                                                    <Row>
                                                        <Col>
                                                            <Row id="log-toggle-buttons" className="mt-2">
                                                                <div id={`play-${gameId}`} className="col-auto pe-1">
                                                                    <Row id="play-status-selectors" className="play-type-bkg">
                                                                        <div className="col-auto pe-0">
                                                                            <Button id="play-status-selector" variant="link" onClick={() => setShowPlayStatusModal(true)}>
                                                                                <FontAwesomeIcon icon={faCaretDown} />
                                                                            </Button>
                                                                        </div>
                                                                        <Col className="ps-0 position-relative">
                                                                            <ToggleButton id="played-toggle-checkbox" className={`play-type-bkg ${gameLogForm?.isPlayed ? playStatusEnumStrings[currentPlayStatusOption] : ""}`} type="checkbox" value={gameLogForm?.isPlayed} checked={gameLogForm?.isPlayed} onChange={() => setGameLogForm({ ...gameLogForm, isPlayed: !gameLogForm.isPlayed })}>
                                                                                <div id="played-status-text">
                                                                                    <FontAwesomeIcon icon={faGamepad} />
                                                                                    <p className="label d-inline-block">
                                                                                        <span id="played-label-title">{playStatusEnumDisplayStrings[currentPlayStatusOption]}</span>
                                                                                    </p>
                                                                                </div>
                                                                            </ToggleButton>
                                                                        </Col>
                                                                    </Row>
                                                                </div>
                                                                <div id={`playing-${gameId}`} className="col-auto pe-1">
                                                                    <ToggleButton className="btn-playing" id="playing-toggle-checkbox" type="checkbox" value={gameLogForm?.isPlaying} checked={gameLogForm?.isPlaying} onChange={() => setGameLogForm({ ...gameLogForm, isPlaying: !gameLogForm.isPlaying })}>
                                                                        <FontAwesomeIcon icon={faPlay} />
                                                                        <p className="label d-inline-block">Playing</p>
                                                                    </ToggleButton>
                                                                </div>
                                                                <div id={`backlog-${gameId}`} className="col-auto pe-1">
                                                                    <ToggleButton id="backlog-toggle-checkbox" className="btn-backlog" type="checkbox" value={gameLogForm?.isBacklog} checked={gameLogForm?.isBacklog} onChange={() => setGameLogForm({ ...gameLogForm, isBacklog: !gameLogForm.isBacklog })}>
                                                                        <FontAwesomeIcon icon={faBook} />
                                                                        <p className="label d-inline-block">Backlog</p>
                                                                    </ToggleButton>
                                                                </div>
                                                                <div id={`wishlist-${gameId}`} className="col-auto pe-1">
                                                                    <ToggleButton id="wishlist-toggle-checkbox" className="btn-wishlist" type="checkbox" value={gameLogForm?.isWishlist} checked={gameLogForm?.isWishlist} onChange={() => setGameLogForm({ ...gameLogForm, isWishlist: !gameLogForm.isWishlist })}>
                                                                        <FontAwesomeIcon icon={faGift} />
                                                                        <p className="label d-inline-block">Wishlist</p>
                                                                    </ToggleButton>
                                                                </div>
                                                            </Row>
                                                        </Col>
                                                    </Row>
                                                </div>
                                                <div className="col-1 my-auto ps-1">
                                                    <Row>
                                                        <div className="log-editor-liked col-auto m-auto">
                                                            <label className="mb-0 pe-1">Like</label>
                                                            <Row>
                                                                <Col>
                                                                    <label className="btn btn-link mb-0">
                                                                        <FontAwesomeIcon icon={faHeart} />
                                                                    </label>
                                                                </Col>
                                                            </Row>
                                                        </div>
                                                    </Row>
                                                </div>
                                                <div className="col-auto my-auto ms-auto">
                                                    <Dropdown>
                                                        <Dropdown.Toggle id="log-extras-dropdown">
                                                            <FontAwesomeIcon icon={faEllipsisVertical} />
                                                        </Dropdown.Toggle>

                                                        <Dropdown.Menu>
                                                            <Dropdown.Item onClick={handleDeletePlaylog}>
                                                                <FontAwesomeIcon icon={faTrash} />
                                                                Destroy all data
                                                            </Dropdown.Item>
                                                        </Dropdown.Menu>
                                                    </Dropdown>
                                                </div>
                                            </Row>
                                        </Col>
                                    </Row>

                                    <Tab.Container id="log-editor-playthrough" activeKey={activeKey} onSelect={(activeKey) => setActiveKey(activeKey)}>
                                        <Row id="log-editor-nav" className="px-3">
                                            <Col>
                                                <Nav variant="underline" id="log-editor-playthrough-nav">
                                                    <div id="playthrough-container">
                                                        {!loading && playthroughDrafts[0] && (playthroughDrafts || []).map((playthrough) => (
                                                            <Nav.Item key={playthrough?.playthroughId} className="col-auto ps-1 collapsable-col">
                                                                <Nav.Link eventKey={playthrough?.playthroughId} title={playthrough?.logTitle} className="btn-link btn-nav">
                                                                    {playthrough.logTitle}
                                                                </Nav.Link>
                                                            </Nav.Item>
                                                        ))}
                                                    </div>
                                                    <div className="col-auto">
                                                        <Row>
                                                            <div className="col-auto ps-1">
                                                                <Button id="new-log-btn" className="btn-link" onClick={addPlaythrough}>
                                                                    <FontAwesomeIcon icon={faCirclePlus} />
                                                                </Button>
                                                            </div>
                                                        </Row>
                                                    </div>
                                                
                                                </Nav>
                                            </Col>
                                        </Row>

                                        <Tab.Content>
                                            {!loading && playthroughDrafts[0] && (playthroughDrafts || []).map(playthrough => (
                                                <Tab.Pane eventKey={playthrough?.playthroughId} key={playthrough?.playthroughId}>
                                                    { /** <MinimalJournalFormComponent playthrough={playthrough} updatePlaythrough={updatePlaythrough} /> **/}
                                                    <JournalForm playthrough={playthrough} removePlaythrough={removePlaythrough} updatePlaythrough={updatePlaythrough} gamePlatforms={gameLogForm?.platforms} destroyGameLog={destroyGameLogData} />
                                                </Tab.Pane>
                                            ))}
                                        </Tab.Content>
                                    </Tab.Container>
                                </Col>

                            ) }
                        </Row>
                        <Row id="log-editor-footer" className="py-2 px-3">
                            <div className="col-auto my-auto pr-0">
                                <Button className="btn-general" onClick={() => setShowJournalCloseWarning(true)}>Cancel</Button>
                            </div>
                            <div id="btn-save-log" className="col-auto my-auto pr-0">
                                <Button className="btn-main save-log" onClick={submitGameLogForm}>Save Changes</Button>
                            </div>
                        </Row>
                    </div>
                </div>
            </div>

            <ReactModal isOpen={showJournalCloseWarning} onRequestClose={handleJournalWarningClose}>
                <Container fluid className="modal__content my-0">
                    <Row>
                        <Col>
                            <h5 className="mb-0 main-header" id="journal-close-warning-modal-title">
                                <FontAwesomeIcon icon={faExclamationTriangle} />
                                Warning
                            </h5>
                        </Col>
                    </Row>
                    <Row className="my-2">
                        <Col>
                            <p id="warning-desc" className="mb-0">Your logs have unsaved changes</p>
                        </Col>
                    </Row>
                    <Row className="mt-3">
                        <div className="col-auto">
                            <Button id="warning-abort" className="btn-general w-100" onClick={handleJournalWarningClose}>Discard</Button>
                        </div>
                        <div className="col-auto ms-auto pe-0">
                            <Button id="warning-cancel" className="btn-general w-100" onClick={() => setShowJournalCloseWarning(false)}>
                                Return to Edit
                            </Button>
                        </div>
                        <div className="col-auto">
                            <Button id="warning-save" className="btn-main w-100">Save</Button>
                        </div>
                    </Row>
                </Container>
            </ReactModal>

            <ReactModal
                isOpen={showPlayStatusModal}
                onRequestClose={() => setShowPlayStatusModal(false)}
                shouldCloseOnOverlayClick={true}
            >
                <PlayStatusModalContent handleStatusChange={updateGameLogStatusValue} togglePlayed={markUnplayed} />
            </ReactModal>
        </>
    );

};

export default JournalModalContent;