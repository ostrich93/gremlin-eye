import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
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
        <div className="flex row justify-content-md center">
            <h2 id="title">Register</h2>
            {error ? (<><p>{error}</p></>) : null}
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
                    <button id="register-button" type='submit' disabled={loading || password !== passwordConfirmation}>Register</button>
                </div>
            </form>
        </div>
    )
};

export default Register;