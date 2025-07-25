import { Col, Row } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import "./UserHeader.css";

const UserHeader = ({ user }) => {
    return (
        <>
            <Row id="profile-header" className="my-3">
                <Col>
                    <Row>
                        <div className="col-auto ms-auto">
                            <div className="avatar avatar-static">
                                <img src={user?.avatarUrl ?? "/no_avatar.png"} width="175" height="175" />
                            </div>
                        </div>
                        <Col className="mt-auto">
                            <Row>
                                <Col className="ps-0">
                                    <Row>
                                        <Col>
                                            <Row>
                                                <div className="col-auto pe-0">
                                                    <h3 className="me-2 mb-0 main-header">{user?.userName}</h3>
                                                </div>
                                            </Row>
                                        </Col>
                                    </Row>
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                    <Row className="mt-3">
                        <Col>
                            <Row id="profile-nav" className="py-1 mx-0">
                                <Col className="col-4 col-sm-4 col-lg-auto my-auto px-1">
                                    <Link className="d-none d-lg-inline" to={`/users/${user?.userName}` }>Profile</Link>
                                </Col>
                                <Col className="col-4 col-sm-4 col-lg-auto my-auto px-1">
                                    <Link className="d-none d-lg-inline" to={`/users/${user?.userName}/games`}>Games</Link>
                                </Col>
                                <Col className="col-4 col-sm-4 col-lg-auto my-auto px-1">
                                    <Link className="d-none d-lg-inline" to={`/users/${user?.userName}/reviews`}>Reviews</Link>
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                </Col>
            </Row>
        </>
    );
}

export default UserHeader;