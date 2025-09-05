import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import apiClient from '../config/apiClient';

const ConfirmPasswordResetPage = () => {

    const { token } = useParams();
    const navigate = useNavigate();
    //form elements: newPassword, newPasswordConfirmation, submit button
    const [password, setPassword] = useState('');
    const [passwordConfirmation, setPasswordConfirmation] = useState('');

    const [message, setMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    //on load, make api call to `${import.meta.env.VITE_APP_BACKEND_URL}/auth/canResetPassword?c=${code}} and if successful, do normal rendering.

    //on submit, send to /updatePassword api endpoint, with body = { validationToken: prop.code, password: password, passwordConfirmation: passwordConfirmation }
    const handleSubmit = (e) => {
        e.preventDefault();

        try {
            apiClient.put(`${import.meta.env.VITE_APP_BACKEND_URL}/auth/updatePassword`, {
                validationToken: token, password: password, passwordConfirmation: passwordConfirmation
            })
                .then((res) => {
                    navigate("/login");
                })
                .catch((err) => {
                    console.log(err.message);
                    setErrorMessage(err.message);
                })
        } catch (err) {
            console.error(err.message);
            setErrorMessage(err.message);
        }
    };

    useEffect(() => {
        const checkToken = async () => {
            try {
                await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/auth/canResetPassword?token=${token}`);  
            } catch (err) {
                console.error(err);
                if (!ignore) {
                    setMessage(err.message);
                }
            }
        }

        let ignore = false;
        checkToken();
        return () => {
            ignore = true;
        }
    }, [token]);

    const renderInvalidPage = () => {
        return (
            <>
                <Row></Row>
                <Row id="password-reset-form">
                    <div>
                        <h2>There was a problem with the password reset token...</h2>
                        <p>{message}</p>
                    </div>
                </Row>
            </>
        );
    }

    return (
        <Container>
            {message ? renderInvalidPage() :
                <>
                    <Row />
                    {errorMessage && <h3>{errorMessage}</h3>}
                    <Row id="password-reset-form">
                        <div className="col-12 mt-2">
                            <h2 className="title">Password Reset</h2>
                            <p className="text-color-secondary">
                                Set up your new password. It must be an alphanumeric string at least 6 characters long.
                            </p>
                        </div>
                        <Col className="col-md-5 mt-2">
                            <Form onSubmit={handleSubmit}>
                                <Form.Group className="my-3">
                                    <Form.Control required type='password' value={password} placeholder='Password' onChange={e => setPassword(e.target.value)} minLength='6' name="password" />
                                    <Form.Text>Minimum length of 6 characters</Form.Text>
                                </Form.Group>
                                <Form.Group className="my-3">
                                    <Form.Control required type='password' value={passwordConfirmation} placeholder='Password Confirmation' onChange={e => setPasswordConfirmation(e.target.value)} name="passwordConfirmation" />
                                </Form.Group>
                                <div>
                                    <Button id="register-button" type='submit' className="btn-main" disabled={password.length < 6 || password !== passwordConfirmation}>Register</Button>
                                </div>
                            </Form>
                        </Col>
                    </Row>
                </>
            }
        </Container>
    );
};

export default ConfirmPasswordResetPage;