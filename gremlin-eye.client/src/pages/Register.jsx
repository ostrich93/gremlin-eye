import { env } from 'process';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../config/apiClient';
import { useAuth, useAuthDispatch } from "../contexts/AuthContext";
import { login } from '../actions/authActions';

const Register = (props) => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [passwordConfirmation, setPasswordConfirmation] = useState('');
    const [authenticated, setAuthentication] = useState(false);

    const navigate = useNavigate();
    const dispatch = useAuthDispatch();
    const { error } = useAuth();

    const roleType = props.roleType || 0; //This page is used for both normal user registration and admin registration.

    useEffect(() => {
        if (authenticated) {
            navigate("/");
        }
    }, [authenticated, navigate]);

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            let registerResponse = await apiClient.post(`${env.API_URL}/api/user/register`, { username: username, email: email, password: password, passwordConfirmation: passwordConfirmation, role: roleType });

            if (registerResponse) {
                try {
                    let loginResponse = await login(dispatch, { username: username, password: password });
                    if (loginResponse.token) {
                        setAuthentication(true);
                    }
                } catch (error) {
                    throw new Error("Login failed", error);
                }
            } else {
                throw new Error("Register failed");
            }
        } catch (error) {
            console.error("Operation Failed", error);
        }
    };

    return (
        <div className="flex row justify-content-md center">
            <h2 id="title">Register</h2>
            {error ? <p>{error}</p> : null}
            <form onSubmit={handleRegister}>
                <div className="form-group my-3">
                    <input type='text' value={username} placeholder='Username' onChange={e => setUsername(e.target.value)} maxLength='16' name="username" required />
                    <small className="form-text">Maximum length of 16 characters</small>
                </div>
                <div className="form-group my-3">
                    <input type='email' value={email} placeholder='Email address' onChange={e => setEmail(e.target.value)} name="email" required />
                </div>
                <div className="form-group my-3">
                    <input type='password' value={password} placeholder='Password' onChange={e => setPassword(e.target.value)} minLength='6' name="password" required />
                    <small className='form-text'>Minimum length of 6 characters</small>
                </div>
                <div className="form-group my-3">
                    <input type='password' value={passwordConfirmation} placeholder='Password Confirmation' onChange={e => setPasswordConfirmation(e.target.value)} name="passwordConfirmation" required />
                </div>
                <div>
                    <button id="register-button" type='submit' disabled={password !== passwordConfirmation}>Register</button>
                </div>
            </form>
        </div>
    )
};

export default Register;