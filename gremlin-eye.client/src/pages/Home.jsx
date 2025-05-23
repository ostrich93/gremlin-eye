import { Button, Container, Row, Col } from 'react-bootstrap';

const Home = () => {
    return (
        <Container>
            <Row id="welcoming-banner">
                <Col>
                    <h2>Discover, collect, and analyze games</h2>
                </Col>
                <Row id="home-about">
                    <Col>
                        <h2>What is Gremlin-Eye?</h2>
                        <p>Gremlin-Eye is a site that allows users to virtually manage their game collections. You can update your backlog, write reviews, rate games,
                        record your play sessions, and put games into your wishlist.</p>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Row>
                            <div class="gradient"></div>
                        </Row>
                    </Col>
                </Row>
                <Row id="landing-actions">
                    <Col md="auto" className="auto pr-0">
                        <Button href="/users/register" className="btn-main px-3 py-2">Create a Free Account</Button>
                    </Col>
                    <Col md="auto" className="pl-md-2 my-auto">
                        <p className="mt-2 mt-md-0">or <a href="/login">log in</a> if you have an account</p>
                    </Col>
                </Row>
                <Row className="feature-showcase">
                    <Col>
                        <h2>Track your game collections</h2>
                        <p>You can log the games you have played, are currently playing, and wish to play. You can record your platforms of choice, time spent, etc. in
                       as fine-grained detail as you want.</p>
                    </Col>
                </Row>
                <Row className="feature-showcase">
                    <Col>
                        <h2>Share your opinions with reviews</h2>
                        <p>You can rate games and write reviews if you wish. Your score is tracked and taken into account when determining the average rating of the game.</p>
                    </Col>
                </Row>
                <Row className="feature-showcase">
                    <Col>
                        <h2>Organize your games with lists</h2>
                        <p>Whether its ranked lists for your favorite series, or grouping what games you aim to play this year, you can make lists of game titles with notes.</p>
                    </Col>
                </Row>
            </Row>
        </Container>
    );
};

export default Home;