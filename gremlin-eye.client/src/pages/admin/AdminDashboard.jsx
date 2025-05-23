import { Button, Container, Row, Col } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

const AdminDashboard = () => {
    const navigate = useNavigate();
    const onItemClicked = (syncLinkText) => {
        navigate(`/admin/sync${syncLinkText}`);
    }

    return (
        <Container>
            <Row id="welcoming-banner">
                <Col>
                    <h2>Welcome Admins!</h2>
                </Col>
                <Row id="home-about">
                    <Col>
                        <p>As an admin, you can synchronize Gremlin-Eye's database with IGDB's through the links listed on this page.</p>
                        <p>It is highly recommended that before synchronizing the games, you sychronize the platform, company, series, and genre data.</p>
                    </Col>
                </Row>
                <Row id="admin-board">
                    <Row>
                        <Col md="auto">
                            <Button className="btn-main px-3 py-2" onClick={() => onItemClicked("Companies")}>Sync Companies</Button>
                        </Col>
                        <Col md="auto">
                            <Button className="btn-main px-3 py-2" onClick={() => onItemClicked("Genres")}>Sync Genres</Button>
                        </Col>
                    </Row>
                    <Row>
                        <Col md="auto">
                            <Button className="btn-main px-3 py-2" onClick={() => onItemClicked("Platforms")}>Sync PLatforms</Button>
                        </Col>
                        <Col md="auto">
                            <Button className="btn-main px-3 py-2" onClick={() => onItemClicked("Series")}>Sync Series</Button>
                        </Col>
                    </Row>
                    <Row>
                        <Col md="auto">
                            <Button className="btn-main px-3 py-2" onClick={() => onItemClicked("Games")}>Sync Games</Button>
                        </Col>
                    </Row>
                </Row>
            </Row>
        </Container>
    );
};

export default AdminDashboard;