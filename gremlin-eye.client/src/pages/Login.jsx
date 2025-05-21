import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthState, useAuthDispatch } from '../contexts/AuthProvider';
import apiClient from '../config/apiClient';

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const navigate = useNavigate();
    const { loading, error } = useAuthState();
    const dispatch = useAuthDispatch();

    const handleLogin = async (e) => {
        e.preventDefault();
        dispatch({ type: "LOGIN_REQUEST" });
        apiClient.post(`${import.meta.env.VITE_APP_BACKEND_URL}/auth/login`, { username: username, password: password }, {
            withCredentials: true
        })
            .then((res) => {
                const userData = {
                    userId: res.data.userId,
                    username: res.data.username,
                    role: res.data.role
                };
                sessionStorage.setItem("current_user", JSON.stringify(userData));
                sessionStorage.setItem("access_token", res.data.accessToken);
                sessionStorage.setItem("refresh_token", res.data.refreshToken);
                dispatch({
                    type: "LOGIN_SUCCESS", payload: {
                        user: userData,
                        accessToken: res.data.accessToken,
                        refreshToken: res.data.refreshToken
                    }
                });
                navigate("/");
            })
            .catch((err) => {
                console.log(err);
                dispatch({ type: "LOGIN_ERROR", error: err });
            });
    };

    return (
        <div className="flex row justify-content-md center">
            <h2 id="title">Log In</h2>
            {error ? (<><p>{error}</p></>) : null}
            <form onSubmit={handleLogin}>
                <div className="form-group my-3">
                    <input type='text' value={username} placeholder='Username' onChange={e => setUsername(e.target.value)} name="username" required disabled={loading} />
                </div>
                <div className="form-group my-3">
                    <input type='password' value={password} placeholder='Password' onChange={e => setPassword(e.target.value)} name="password" required disabled={loading} />
                </div>
                <div>
                    <button id="register-button" type='submit' disabled={loading || (!username.length || !password.length)}>Log In</button>
                </div>
            </form>
        </div>
    );
};

export default Login;