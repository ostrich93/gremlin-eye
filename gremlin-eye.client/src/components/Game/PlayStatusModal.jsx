import { Button, Col, Container, Row } from 'react-bootstrap';
import { faGamepad, faSquare } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export default function PlayStatusModalContent({ handleStatusChange, togglePlayed }) {

    return (
        <>
            <div className="pt-1 modal__header">
                <h5 id="play-type-modal-title">Set your played status</h5>
            </div>
            <Container fluid className="py-0 modal__content">
                <Row id="played" onClick={(e) => handleStatusChange(e, 0)}>
                    <Col className="col-12 my-auto">
                        <h3 className="mb-0"><FontAwesomeIcon icon={faSquare} color='#ea377a' /> Played</h3>
                        <p className="subtitle-text mb-0">You have played this game with no specifics.</p>
                    </Col>
                </Row>
                <Row id="playing" onClick={(e) => handleStatusChange(e, 1)}>
                    <Col className="col-12 my-auto">
                        <h3 className="mb-0"><FontAwesomeIcon icon={faSquare} color='green' /> Completed</h3>
                        <p className="subtitle-text mb-0">You have beaten the game.</p>
                    </Col>
                </Row>
                <Row id="playing" onClick={(e) => handleStatusChange(e, 2)}>
                    <Col className="col-12 my-auto">
                        <h3 className="mb-0"><FontAwesomeIcon icon={faSquare} color='blue' /> Retired</h3>
                        <p className="subtitle-text mb-0">You are finished with a game that lacks an ending (like e-sports titles).</p>
                    </Col>
                </Row>
                <Row id="playing" onClick={(e) => handleStatusChange(e, 3)}>
                    <Col className="col-12 my-auto">
                        <h3 className="mb-0"><FontAwesomeIcon icon={faSquare} color='orange' /> Shelved</h3>
                        <p className="subtitle-text mb-0">You have not finished the game, but may pick it up later.</p>
                    </Col>
                </Row>
                <Row id="playing" onClick={(e) => handleStatusChange(e, 4)}>
                    <Col className="col-12 my-auto">
                        <h3 className="mb-0"><FontAwesomeIcon icon={faSquare} color='red' /> Abandoned</h3>
                        <p className="subtitle-text mb-0">You have not finished the game and don't plan to change that.</p>
                    </Col>
                </Row>
                <Row className="mt-4">
                    <Col>
                        <Button id="unset-played-button" className="btn-general w-100" onClick={togglePlayed}>
                            <FontAwesomeIcon icon={faGamepad} />Mark as unplayed
                        </Button>
                    </Col>
                </Row>
            </Container>
        </>
    );

};