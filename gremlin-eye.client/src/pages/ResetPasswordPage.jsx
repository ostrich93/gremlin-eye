import { useState } from "react"
import { Button, Col, Form, Container, Row } from 'react-bootstrap';
import apiClient from "../config/apiClient";

const ResetPasswordPage = () => {
    const [emailAddress, setEmailAddress] = useState('');
    const [errorMessage, setErrorMessage] = useState(null);
    const [successMessage, setSuccessMessage] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        apiClient.post(
            `${import.meta.env.VITE_APP_BACKEND_URL}/auth/forgotPassword`,
            emailAddress
        )
            .then((res) => {
                setSuccessMessage("An email has been sent with instructions on how to reset your password");
                setErrorMessage('');
            })
            .catch((err) => {
                console.log(err);
                setErrorMessage(err.message);
                setSuccessMessage('');
            });
    };

    return (
        <Container>
            <Row></Row>
            <Row id="password-reset-form">
                <div className="col-12 mt-2">
                    <h2 className="title">Password Reset</h2>
                    <p className="text-color-secondary">
                        Enter your account's email so we can send you instructions on how to reset your password.
                    </p>
                </div>
                <Col className="col-md-5 mt-2">
                    <Form onSubmit={handleSubmit}>
                        <Form.Group>
                            <Form.Label>Email</Form.Label>
                            <Form.Control id="user_email" className="gremlin-eye-field w-100" type="email" placeholder="name@example.com" value={emailAddress} onChange={e => setEmailAddress(e.target.value)} name="email" />
                        </Form.Group>
                        <div className="actions">
                            <Button type='submit' className="btn-main mt-3">Send reset instructions</Button>
                        </div>
                    </Form>
                    {errorMessage && <h3>{errorMessage}</h3>}
                    {successMessage && <h3>{successMessage}</h3>}
                </Col>
            </Row>
        </Container>
    )
};

export default ResetPasswordPage;