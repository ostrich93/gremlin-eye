import { useRef, useState } from 'react';
import { ButtonGroup, Button, Col, Container, InputGroup, Form, Row, ToggleButton } from 'react-bootstrap';
import ReactModal from 'react-modal';
import Rate from 'rc-rate';
import "rc-rate/assets/index.css";
import { faExclamationTriangle, faRotateLeft, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import formatDate from '../../services/formatDate';

//contained by a Tab in Journal Form
//Playthroughs are simply DRAFTS of forms that get submitted to the endpoint. They can be derived from an existing play log, but can also be created from scratch.
const JournalForm = ({ playthrough, updatePlayLogs, removePlaythrough, updatePlaythrough, gamePlatforms }) => {
    const id = playthrough.playthroughId;

    const [currentEvent, setCurrentEvent] = useState(null);
    const [showPlayLogModal, setShowPlayLogModal] = useState(false);
    const [showDeletePlaythroughWarning, setShowDeletePlaythroughWarning] = useState(false);
    const [showDeletePlaylogWarning, setShowDeletePlaylogWarning] = useState(false);
    const [showDestroyGameLogWarning, setShowDestroyGameLogWarning] = useState(false);
    //const [formatTooltip, showFormatTooltip] = useState(false);

    const [playSessionState, setPlaySessionState] = useState({
        playLogId: null,
        startDate: null,
        endDate: null,
        isStart: false,
        isEnd: false,
        logNote: null,
        hours: 0,
        minutes: 0
    });

    const starRatingRef = useRef(null);
    const guidRef = useRef(-1);

    const handleRating = (rateValue) => {
        if (starRatingRef.current) {
            if (!starRatingRef.current.state) {
                starRatingRef.current.state = {};
            }
            starRatingRef.current.state.value = rateValue;
        }
        updatePlaythrough(id, "rating", rateValue);
    };

    const handleDateSelect = (selectInfo) => {
        let calendarApi = selectInfo.view.calendar;

        calendarApi.unselect();

        calendarApi.addEvent({
            id: guidRef.current--,
            title: "Played",
            start: selectInfo.startStr,
            end: selectInfo.endStr,
            allDay: true,
            extendedProps: {
                logNote: null,
                isStart: false,
                isEnd: false,
                hours: 0,
                minutes: 0
            }
        });
    };

    const handleClickInfo = (clickInfo) => {
        setCurrentEvent({
            playLogId: clickInfo.event.id,
            startDate: clickInfo.event.start,
            endDate: clickInfo.event.end ?? clickInfo.event.start,
            logNote: clickInfo.event.extendedProps.logNote,
            isStart: clickInfo.event.extendedProps.isStart,
            isEnd: clickInfo.event.extendedProps.isEnd,
            hours: clickInfo.event.extendedProps.hours,
            minutes: clickInfo.event.extendedProps.minutes
        });
        setShowPlayLogModal(true);
    };

    const handleDeleteLogEvent = (e) => {
        e.preventDefault();
        currentEvent.remove();
        setShowDeletePlaylogWarning(false);
        setShowPlayLogModal(false);
    };

    const handleDeletePlaythrough = (e) => {
        e.preventDefault();
        removePlaythrough(id);
        setShowDeletePlaythroughWarning(false);
    }

    /*const handleAddEvent = (addInfo) => {
        addPlayLog(id, addInfo);
    };

    const handleRemoveEvent = (removeInfo) => {
        removePlayLog(id, removeInfo.id);
    };*/

    const handleEvents = (events) => {
        updatePlayLogs(id, events);
    };

    const updateCurrentEvent = () => {
        currentEvent.setExtendedProp("logNote", playSessionState?.logNote);
        currentEvent.setExtendedProp("hours", playSessionState?.hours);
        currentEvent.setExtendedProp("minutes", playSessionState?.minutes);
        currentEvent.setExtendedProp("isStart", playSessionState?.isStart);
        currentEvent.setExtendedProp("isEnd", playSessionState?.isEnd);
        setShowPlayLogModal(false);
    }; //fill in later

    return (
        <>
            <Form>
                <Row className="my-3 log-editor-section">
                    <div className="col-7 mb-3 pe-4">
                        <Row>
                            <Form.Group controlId="playthroughTitle" className="col-6 mt-auto">
                                <Form.Label className="mb-0">Log Title</Form.Label>
                                <Form.Control className="gremlin-eye-field w-100" defaultValue={playthrough.logTitle} minLength='1' maxLength='24' placeholder="Title your log" required type="text" />
                                <Form.Control.Feedback as='p' className="subtitle-text mt-1 mb-0" type="invalid">Your title must be at least one character long</Form.Control.Feedback>
                            </Form.Group>
                            <div className="col-auto my-auto">
                                <label className="mb-0" htmlFor="playthroughReplay">Replay</label>
                                <br />
                                <Row className="mt-1">
                                    <Form.Group className="col-auto mx-auto" controlId="playthroughReplay">
                                        <Form.Check id="playthroughReplay" className="gremlin-eye-checkbox me-1">
                                            <Form.Label className="mb-0 checkbox-label">
                                                <FontAwesomeIcon icon={faRotateLeft} />
                                            </Form.Label>
                                        </Form.Check>
                                    </Form.Group>
                                </Row>
                            </div>
                        </Row>
                        <Row className="mt-4">
                            <Form.Group as={Col} controlId="playthroughPlatform">
                                <Form.Label>Platform</Form.Label>
                                <Form.Select id="playthroughPlatform" style={{ width: "100%" }} title="Select release platform">
                                    <option value="" disabled selected hidden>Choose a platform</option>
                                    {gamePlatforms.map((platform) => (
                                        <option key={platform.id} value={platform.id}>{platform.name}</option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                            <Form.Group as={Col} controlId="playthroughOwnership">
                                <Form.Label>Ownership</Form.Label>
                                <Form.Select id="playthroughOwnership" style={{ width: "100%" }} title="Owned, subscription, etc. ">
                                    <option value="" disabled selected hidden>Owned, subscription, etc.</option>
                                    <option value="owned">Owned</option>
                                    <option value="subscribed">Subscription</option>
                                    <option vaue="borrowed">Borrowed</option>
                                    <option value="watched">Watched</option>
                                </Form.Select>
                            </Form.Group>
                        </Row>
                        <Row className="mt-4">
                            <Form.Group as={Col} controlId="playthroughRating">
                                <Form.Label>Rating</Form.Label>
                                <Rate
                                    defaultValue={playthrough.rating ?? 0}
                                    value={playthrough.rating ?? 0}
                                    ref={starRatingRef}
                                    allowHalf
                                    allowClear
                                    onChange={(value) => handleRating(value)}
                                />
                            </Form.Group>
                        </Row>
                        <Row className="mt-3">
                            <Form.Group controlId="playthroughReview" className="col-12">
                                <Form.Label htmlFor="playthroughReview">Review</Form.Label>
                                <Form.Control as="textarea" rows={4} id="playthroughReview" placeholder="Write your thoughts..." onChange={(e) => updatePlaythrough(id, "reviewText", e.target.value)} />
                            </Form.Group>
                        </Row>
                        <Row>
                            <Form.Check className="col-auto me-auto" id="playthroughReviewSpoilers">
                                <Form.Check.Input defaultChecked={playthrough.containsSpoilers} />
                                <Form.Check.Label className="btn btn-small mb-0">Contains spoilers</Form.Check.Label>
                            </Form.Check>
                        </Row>
                        <Row className="mb-4">
                            <div className="col-auto ms-auto">
                                <Button id="delete-playthrough-btn" variant="link" className="subtitle-text" style={{ border: "none" }} onClick={() => setShowDeletePlaythroughWarning(true)}>
                                    <FontAwesomeIcon icon={faTrash} />
                                    Delete this Log
                                </Button>
                            </div>
                        </Row>
                    </div>
                    <Col>
                        <Row>
                            <Col>
                                {<FullCalendar
                                    plugins={[dayGridPlugin, interactionPlugin]}
                                    initialView='dayGridMonth'
                                    editable={true}
                                    selectable={true}
                                    weekends={true}
                                    initialEvents={playthrough.playLogs.map((ev) => {
                                        return {
                                            id: ev.playLogId,
                                            start: ev.startDate,
                                            end: ev.endDate,
                                            title: "Played",
                                            allDay: true,
                                            extendedProps: {
                                                logNote: ev.logNote,
                                                isStart: ev.isStart,
                                                isEnd: ev.isEnd,
                                                hours: ev.hours,
                                                minutes: ev.minutes
                                            }
                                        };
                                    })}
                                    select={handleDateSelect}
                                    eventClick={handleClickInfo}
                                    eventsSet={handleEvents}
                                />}
                            </Col>
                        </Row>
                    </Col>
                </Row>
            </Form>

            
            <ReactModal isOpen={showPlayLogModal} onRequestClose={() => setShowPlayLogModal(false)}>
                <Container id="playlog-modal-content" className="p-0 m-0 modal__content" fluid>
                    <div className="modal-header px-0 py-1">
                        <div className="col-auto pe-1">
                            <p className="mb-0">Play Session</p>
                        </div>
                        <Col className="ps-1 my-auto">
                            <p className="mb-0 subtitle-text my-auto">{playSessionState?.startDate == playSessionState?.endDate ? formatDate(playSessionState?.startDate) : `${formatDate(playSessionState?.startDate)} - ${playSessionState?.endDate}}`}</p>
                        </Col>
                    </div>
                    <div className="modal-body pb-2">
                        <Row>
                            <div className="col-12">
                                <label className="mb-0">Started/Finished</label>
                                <Row>
                                    <Col className="pe-1">
                                        <ButtonGroup>
                                            <ToggleButton id="isStart" className="w-100 text-center mb-0" variant="outline-secondary" type="checkbox" checked={playSessionState?.isStart} value="1" onChange={(e) => setPlaySessionState({ ...playSessionState, isStart: e.currentTarget.checked })} />
                                        </ButtonGroup>
                                    </Col>
                                    <Col className="ps-1">
                                        <ButtonGroup>
                                            <ToggleButton id="isFinish" className="w-100 text-center mb-0" variant="outline-secondary" type="checkbox" checked={playSessionState?.isEnd} value="2" onChange={(e) => setPlaySessionState({ ...playSessionState, isEnd: e.currentTarget.checked })} />
                                        </ButtonGroup>
                                    </Col>
                                </Row>
                            </div>

                            <Row className="mt-3">
                                <Col>
                                    <Form.Group controlId="note">
                                        <Form.Label className="mb-0">Note</Form.Label>
                                        <Form.Control as="textarea" className="p-2 w-100 gremlin-eye-field" name="note" rows={3} placeholder="Leave a note" defaultValue={playSessionState?.logNote} onChange={(e) => setPlaySessionState({ ...playSessionState, logNote: e.currentTarget.value })} />
                                    </Form.Group>
                                </Col>
                            </Row>
                            <Row className="mt-3">
                                <div className="col-auto">
                                    <label className="mb-0" htmlFor="hours">Time Played</label>
                                    <Row className="time-fields">
                                        <div className="col-auto pe-2">
                                            <InputGroup>
                                                <Form.Control aria-described-by="playDateHours" className="gremlin-eye-field hours-field" name="hours" defaultValue={playSessionState?.hours} onChange={(e) => setPlaySessionState({ ...playSessionState, hours: e.currentTarget.value })} />
                                                <InputGroup.Text id="playDateHours">h</InputGroup.Text>
                                            </InputGroup>
                                        </div>
                                        <div className="col-auto ps-0">
                                            <InputGroup>
                                                <Form.Control aria-described-by="playDateMinutes" className="gremlin-eye-field minutes-field" name="minutes" defaultValue={playSessionState?.minutes} onChange={(e) => setPlaySessionState({ ...playSessionState, minutes: e.currentTarget.value })} />
                                                <InputGroup.Text id="playDateMinutes">m</InputGroup.Text>
                                            </InputGroup>
                                        </div>
                                    </Row>
                                </div>
                            </Row>
                        </Row>
                        <hr />
                        <Row>
                            <div className="col-auto my-auto">
                                <Button id="play-date-delete" className="btn-link" onClick={() => setShowDeletePlaylogWarning(true)}>
                                    <FontAwesomeIcon icon={faTrash} />
                                </Button>
                            </div>
                            <div className="col-auto ms-auto pe-0">
                                <Button id="play-date-close" className="py-1" onClick={() => setShowPlayLogModal(false)}>Cancel</Button>
                            </div>
                            <div className="col-auto">
                                <Button id="play-date-update" className="py-1" variant="primary" onClick={updateCurrentEvent}>Save</Button>
                            </div>
                        </Row>
                    </div>
                </Container>
            </ReactModal>


            <ReactModal isOpen={showDeletePlaythroughWarning} onRequestClose={() => setShowDeletePlaythroughWarning(false)}>
                <Container fluid className="modal__content my-0">
                    <Row>
                        <Col>
                            <h5 className="mb-0 main-header">
                                <FontAwesomeIcon icon={faExclamationTriangle} />
                                Warning
                            </h5>
                        </Col>
                    </Row>
                    <Row className="my-2">
                        <Col>
                            <p id="warning-desc" className="mb-0">Delete your data for this log? This includes the associated review and journal entries for this log.</p>
                        </Col>
                    </Row>
                    <Row className="mt-3">
                        <div className="col-auto">
                            <Button id="warning-abort" className="btn-general w-100" onClick={() => setShowDeletePlaythroughWarning(false)}>Return</Button>
                        </div>
                        <div className="col-auto ms-auto pe-0" />
                        <div className="col-auto">
                            <Button id="warning-save" className="btn-main w-100" onClick={handleDeletePlaythrough}>Delete</Button>
                        </div>
                    </Row>
                </Container>
            </ReactModal>


            <ReactModal isOpen={showDeletePlaylogWarning} onRequestClose={() => setShowDeletePlaylogWarning(false)}>
                <Container fluid className="modal__content my-0">
                    <Row>
                        <Col>
                            <h5 className="mb-0 main-header">
                                <FontAwesomeIcon icon={faExclamationTriangle} />
                                Warning
                            </h5>
                        </Col>
                    </Row>
                    <Row className="my-2">
                        <Col>
                            <p id="warning-desc" className="mb-0">Are you sure you want to delete this play session?</p>
                        </Col>
                    </Row>
                    <Row className="mt-3">
                        <div className="col-auto">
                            <Button id="warning-abort" className="btn-general w-100" onClick={() => setShowDeletePlaylogWarning(false)}>Cancel</Button>
                        </div>
                        <div className="col-auto ms-auto pe-0" />
                        <div className="col-auto">
                            <Button id="warning-save" className="btn-main w-100" onClick={handleDeleteLogEvent}>Delete</Button>
                        </div>
                    </Row>
                </Container>
            </ReactModal>


            <ReactModal isOpen={showDestroyGameLogWarning} onRequestClose={() => setShowDestroyGameLogWarning}>
                <Container fluid className="modal__content my-0">
                    <Row>
                        <Col>
                            <h5 className="mb-0 main-header">
                                <FontAwesomeIcon icon={faExclamationTriangle} />
                                Warning
                            </h5>
                        </Col>
                    </Row>
                    <Row className="my-2">
                        <Col>
                            <p id="warning-desc" className="mb-0">Delete your data for this log? This includes all logs, journal entries, reviews, ratings, time tracked, etc.. for this game.</p>
                        </Col>
                    </Row>
                    <Row className="mt-3">
                        <div className="col-auto">
                            <Button id="warning-abort" className="btn-general w-100" onClick={() => setShowDestroyGameLogWarning(false)}>Return</Button>
                        </div>
                        <div className="col-auto ms-auto pe-0" />
                        <div className="col-auto">
                            <Button id="warning-save" className="btn-main w-100">Destroy</Button>
                        </div>
                    </Row>
                </Container>
            </ReactModal>
        </>
    );
};

export default JournalForm;