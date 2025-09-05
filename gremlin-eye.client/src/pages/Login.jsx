import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Container, Form, Row } from 'react-bootstrap';
import { useAuthState, useAuthDispatch } from '../contexts/AuthProvider';
import { login } from '../actions/authActions';
import useDebounce from '../hooks/useDebounce';

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const navigate = useNavigate();
    const { loading, error, refreshToken } = useAuthState();
    const dispatch = useAuthDispatch();

    const submitLogin = (e) => {
        e.preventDefault();
        login(dispatch, { username: username, password: password });
    };

    const handleLogin = () => {
        if (refreshToken != null && refreshToken.length > 0) {
            navigate("/");
        }
        if (error != null) {
            alert("Failed to login. A user with the credentials inputted could not be found.");
        }
    };

    useDebounce(handleLogin, 200, [refreshToken, navigate, error]);

    return (
        <Container>
            <Row id="log-in" className="justify-content-md-center mt-3 mx-2">
                <div className="col-md-5 mt-2 mx-2 p-3">
                    <h2 id="title" className="text-center">Log In</h2>
                    <Form onSubmit={submitLogin}>
                        <Form.Group className="my-3">
                            <Form.Control type='text' value={username} placeholder='Username' onChange={e => setUsername(e.target.value)} name="username" required disabled={loading} />
                        </Form.Group>
                        <Form.Group className="my-3">
                            <Form.Control type='password' value={password} placeholder='Password' onChange={e => setPassword(e.target.value)} name="password" required disabled={loading} />
                        </Form.Group>
                        <div>
                            <Button id="register-button" type='submit' className="btn-main" disabled={loading || (!username.length || !password.length)}>Log In</Button>
                        </div>
                    </Form>
                    <br />
                    <a className="text-center link mt-2" href="/users/register">If you're a new user, sign up here</a>
                    <br />
                    <a className="text-center link mt-2" href="/users/password/new">Forgot your password?</a>
                </div>
            </Row>
        </Container>
    );
};

export default Login;