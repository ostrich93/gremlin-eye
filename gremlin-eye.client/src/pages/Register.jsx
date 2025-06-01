import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import apiClient from '../config/apiClient';
import { useAuthState, useAuthDispatch } from "../contexts/AuthProvider";

const Register = (props) => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [passwordConfirmation, setPasswordConfirmation] = useState('');

    const navigate = useNavigate();
    const dispatch = useAuthDispatch();
    const { loading, error } = useAuthState();

    const roleType = props.roleType || 0; //This page is used for both normal user registration and admin registration.

    const handleRegister = (e) => {
        e.preventDefault();
        dispatch({ type: "REGISTER_REQUEST" });

        apiClient.post(
            `${import.meta.env.VITE_APP_BACKEND_URL}/api/user/register`, {
            username: username, email: email, password: password, passwordConfirmation: passwordConfirmation, role: roleType
        })
            .then((res) => {
                dispatch({ type: "REGISTER_SUCCESS" });
                navigate("/login");
            })
            .catch((err) => {
                console.log(err);
                dispatch({ type: "REGISTER_ERROR", payload: err });
            }); 
    };

    return (
        <Container>
            <Row id="log-in" className="flex justify-content-md center mt-3 mx-2">
                <Col className="col-md-5 mt-2 mx-2 p-3">
                    <h2 id="title" className="text-center">Register</h2>
                    {error ? (<><p>{error}</p></>) : null}
                    <Form onSubmit={handleRegister}>
                        <Form.Group className="my-3">
                            <Form.Control required type='text' value={username} placeholder='Username' onChange={e => setUsername(e.target.value)} maxLength='16' name="username" />
                            <Form.Text>Maximum length of 16 characters</Form.Text>
                        </Form.Group>
                        <Form.Group className="my-3">
                            <Form.Control required type='email' value={email} placeholder='Email address' onChange={e => setEmail(e.target.value)} name="email" />
                        </Form.Group>
                        <Form.Group className="my-3">
                            <Form.Control required type='password' value={password} placeholder='Password' onChange={e => setPassword(e.target.value)} minLength='6' name="password" />
                            <Form.Text>Minimum length of 6 characters</Form.Text>
                        </Form.Group>
                        <Form.Group className="my-3">
                            <Form.Control required type='password' value={passwordConfirmation} placeholder='Password Confirmation' onChange={e => setPasswordConfirmation(e.target.value)} name="passwordConfirmation" />
                        </Form.Group>
                        <div>
                            <Button id="register-button" type='submit' disabled={loading || password !== passwordConfirmation}>Register</Button>
                        </div>
                    </Form>
                </Col>
            </Row>
        </Container>
    )
};

export default Register;