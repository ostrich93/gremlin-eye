import { useRef, useState } from 'react';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import ReactModal from 'react-modal';
import Rate from 'rc-rate';
import "rc-rate/assets/index.css";
import { faExclamationTriangle, faRotateLeft, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

//contained by a Tab in Journal Form
//Playthroughs are simply DRAFTS of forms that get submitted to the endpoint. They can be derived from an existing play log, but can also be created from scratch.
const JournalForm = ({ playthrough, removePlaythrough, updatePlaythrough, gamePlatforms, destroyGameLog }) => {
    const id = playthrough.playthroughId;
    const [showDeletePlaythroughWarning, setShowDeletePlaythroughWarning] = useState(false);
    const [showDestroyGameLogWarning, setShowDestroyGameLogWarning] = useState(false);
    //const [formatTooltip, showFormatTooltip] = useState(false);

    const starRatingRef = useRef(null);

    const handleRating = (rateValue) => {
        if (starRatingRef.current) {
            if (!starRatingRef.current.state) {
                starRatingRef.current.state = {};
            }
            starRatingRef.current.state.value = rateValue;
        }
        updatePlaythrough(id, "rating", 2 * rateValue);
    };

    const handleDeletePlaythrough = (e) => {
        e.preventDefault();
        removePlaythrough(id);
        setShowDeletePlaythroughWarning(false);
    }

    const handleDestroyGameLog = () => {
        destroyGameLog();
        setShowDestroyGameLogWarning(false);
    };

    return (
        <>
            <Form>
                <Row className="my-3 log-editor-section">
                    <div className="col-7 mb-3 pe-4">
                        <Row>
                            <Form.Group controlId="playthroughTitle" className="col-6 mt-auto">
                                <Form.Label className="mb-0">Log Title</Form.Label>
                                <Form.Control className="gremlin-eye-field w-100" value={playthrough.logTitle} minLength='1' maxLength='24' placeholder="Title your log" required type="text" onChange={(e) => { updatePlaythrough(id, "logTitle", e.target.value) }} />
                                <Form.Control.Feedback as='p' className="subtitle-text mt-1 mb-0" type="invalid">Your title must be at least one character long</Form.Control.Feedback>
                            </Form.Group>
                            <div className="col-auto my-auto">
                                <label className="mb-0" htmlFor="playthroughReplay">Replay</label>
                                <br />
                                <Row className="mt-1">
                                    <Form.Group className="col-auto mx-auto" controlId="playthroughReplay">
                                        <Form.Check id="playthroughReplay" className="gremlin-eye-checkbox me-1" onClick={(e) => updatePlaythrough(id, "isReplay", e.target.checked)}>
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
                                <Form.Select id="playthroughPlatform" style={{ width: "100%" }} title="Select release platform" value={playthrough.platform ? playthrough.platform.id : ""} onChange={(e) => updatePlaythrough(id, "platform", e.target.value)}>
                                    <option value="" disabled selected hidden>Choose a platform</option>
                                    {gamePlatforms.map((platform) => (
                                        <option key={platform.id} value={platform.id} data-slug={platform.slug}>{platform.name}</option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                            <Form.Group as={Col} controlId="playthroughOwnership">
                                <Form.Label>Ownership</Form.Label>
                                <Form.Select id="playthroughOwnership" style={{ width: "100%" }} value={playthrough.medium ?? ""} title="Owned, subscription, etc. " onChange={(e) => updatePlaythrough(id, "medium", e.target.value)}>
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
                                    defaultValue={playthrough.rating ? playthrough.rating / 2 : 0}
                                    value={playthrough.rating ? playthrough.rating / 2 : 0}
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
                                <Form.Control as="textarea" rows={4} id="playthroughReview" value={playthrough.reviewText} placeholder="Write your thoughts..." onChange={(e) => updatePlaythrough(id, "reviewText", e.target.value)} />
                            </Form.Group>
                        </Row>
                        <Row>
                            <Form.Check className="col-auto me-auto" id="playthroughReviewSpoilers">
                                <Form.Check.Input defaultChecked={playthrough.containsSpoilers} onClick={(e) => updatePlaythrough(id, "containsSpoilers", e.target.checked)} />
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
                        
                    </Col>
                </Row>
            </Form>

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
                            <Button id="warning-abort" className="btn-general w-100" onClick={handleDestroyGameLog}>Return</Button>
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