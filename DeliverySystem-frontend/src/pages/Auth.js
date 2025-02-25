import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';

function Auth({ setIsAuthenticated }) {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');
    const [errors, setErrors] = useState([]);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setMessage('');
        setErrors([]);

        try {
            const response = await axios.post(
                '/api/auth/login',
                { login, password },
                { withCredentials: true }
            );

            const { id, login: userLogin, role } = response.data;

            localStorage.setItem('userId', id);
            localStorage.setItem('userLogin', userLogin);
            localStorage.setItem('role', role);

            let entityIdResponse;
            if (role === 'Carrier') {
                entityIdResponse = await axios.get(`/api/carrier/user/${id}`, { withCredentials: true });
            } else if (role === 'CargoOwner') {
                entityIdResponse = await axios.get(`/api/cargoOwner/user/${id}`, { withCredentials: true });
            }

            const { id: entityId } = entityIdResponse.data;
            localStorage.setItem('entityId', entityId);

            setIsAuthenticated(true);
            setErrors([])
            setMessage('Успешная авторизация!');
            setTimeout(() => navigate('/profile'), 1500);
        } catch (error) {
            if (error.response) {
                if (Array.isArray(error.response.data)) {
                    setErrors(error.response.data);
                } else if (typeof error.response.data === 'string') {
                    setErrors([error.response.data]);
                } else {
                    setErrors(['Ошибка при авторизации.']);
                }
            } else {
                setErrors(['Ошибка сети. Попробуйте позже.']);
            }
            setMessage('');
        }
    };

    return (
        <div className="auth-container">
            <h2>Авторизация</h2>
            <form onSubmit={handleLogin}>
                <div>
                    <label htmlFor="login">Логин:</label>
                    <input
                        type="text"
                        id="login"
                        value={login}
                        onChange={(e) => setLogin(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="password">Пароль:</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Войти</button>
            </form>

            {message && <p className="success-message">{message}</p>}

            {errors.length > 0 && (
                <div className="error-messages">
                    <ul>
                        {errors.map((error, index) => (
                            <li key={index}>{error}</li>
                        ))}
                    </ul>
                </div>
            )}
            
            <Link to="/register">Зарегистрироваться</Link>
        </div>
    );
}

export default Auth;
