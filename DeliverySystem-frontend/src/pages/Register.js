import React, { useState } from 'react';
import axios from 'axios';
import {useNavigate} from "react-router-dom";

function Register() {
    const [role, setRole] = useState('');
    const [formData, setFormData] = useState({
        login: '',
        passwordHash: '',
        carrierDetails: {
            name: '',
            companyName: '',
            contactEmail: '',
            contactPhone: '',
            rating: 0,
            orders : [],
            transports : []
        },
        cargoOwnerDetails: {
            companyName: '',
            contactEmail: '',
            contactPhone: '',
            orders : [],
            cargos : []
        }
    });
    const [message, setMessage] = useState('');
    const [errors, setErrors] = useState([]);
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;

        if (name.startsWith("carrierDetails.") || name.startsWith("cargoOwnerDetails.")) {
            const [section, field] = name.split('.');
            setFormData((prevState) => ({
                ...prevState,
                [section]: {
                    ...prevState[section],
                    [field]: value
                }
            }));
        } else {
            setFormData((prevState) => ({
                ...prevState,
                [name]: value
            }));
        }
    };

    const handleRoleChange = (e) => {
        setRole(e.target.value);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage('');
        setErrors([]);

        let success = false;

        try {
            const userResponse = await axios.post('/api/user/register', {
                login: formData.login,
                passwordHash: formData.password,
                role: role
            }, {
                withCredentials: true
            });

            const userId = userResponse.data.id;
            setMessage('Пользователь зарегистрирован!');

            await axios.post('/api/auth/login', {
                login: formData.login,
                password: formData.password
            }, {
                withCredentials: true
            });

            if (role === 'Carrier') {
                try {
                    await axios.post('/api/carrier', {
                        userId,
                        ...formData.carrierDetails
                    });
                    setMessage('Перевозчик зарегистрирован!');
                    success = true;
                } catch (error) {
                    handleRequestError(error, 'Ошибка при создании перевозчика, пользователь удален.');
                    await axios.delete(`/api/user/${userId}`, { withCredentials: true });
                }
            }

            if (role === 'CargoOwner') {
                try {
                    await axios.post('/api/cargoOwner', {
                        userId,
                        ...formData.cargoOwnerDetails
                    });
                    setMessage('Владелец груза зарегистрирован!');
                    success = true;
                } catch (error) {
                    handleRequestError(error, 'Ошибка при создании владельца груза, пользователь удален.');
                    await axios.delete(`/api/user/${userId}`, { withCredentials: true });
                }
            }

        } catch (error) {
            handleRequestError(error, 'Ошибка при регистрации.');
        }

        if (success) {
            setTimeout(() => navigate('/auth'), 1500);
        }
    };

    const handleRequestError = (error, customMessage) => {
        if (error.response && error.response.data) {
            const errorMessages = Array.isArray(error.response.data)
                ? error.response.data
                : [error.response.data];
            setErrors((prevErrors) => [...prevErrors, ...errorMessages]);
        } else if (error.message) {
            setErrors((prevErrors) => [...prevErrors, error.message]);
        } else {
            setErrors((prevErrors) => [...prevErrors, customMessage]);
        }
        setMessage('');
    };


    return (
        <div className="auth-container">
            <h2>Регистрация</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="login">Логин:</label>
                    <input
                        type="text"
                        id="login"
                        name="login"
                        value={formData.login}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label htmlFor="password">Пароль:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label htmlFor="role">Роль:</label>
                    <select
                        id="role"
                        value={role}
                        onChange={handleRoleChange}
                        required
                    >
                        <option value="">Выберите роль</option>
                        <option value="Carrier">Перевозчик</option>
                        <option value="CargoOwner">Владелец груза</option>
                    </select>
                </div>

                {role === 'Carrier' && (
                    <div>
                        <h2>Детали</h2>
                        <div>
                            <label htmlFor="carrierName">Имя:</label>
                            <input
                                type="text"
                                id="carrierName"
                                name="carrierDetails.name"
                                value={formData.carrierDetails.name}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="carrierCompanyName">Компания:</label>
                            <input
                                type="text"
                                id="carrierCompanyName"
                                name="carrierDetails.companyName"
                                value={formData.carrierDetails.companyName}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="carrierContactEmail">Адрес электронной почты:</label>
                            <input
                                type="email"
                                id="carrierContactEmail"
                                name="carrierDetails.contactEmail"
                                value={formData.carrierDetails.contactEmail}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="carrierContactPhone">Номер телефона:</label>
                            <input
                                type="text"
                                id="carrierContactPhone"
                                name="carrierDetails.contactPhone"
                                value={formData.carrierDetails.contactPhone}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    </div>
                )}

                {role === 'CargoOwner' && (
                    <div className="user-details">
                        <h3>Детали</h3>
                        <div>
                            <label htmlFor="cargoOwnerCompanyName">Компания:</label>
                            <input
                                type="text"
                                id="cargoOwnerCompanyName"
                                name="cargoOwnerDetails.companyName"
                                value={formData.cargoOwnerDetails.companyName}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="cargoOwnerContactEmail">Адрес электронной почты:</label>
                            <input
                                type="email"
                                id="cargoOwnerContactEmail"
                                name="cargoOwnerDetails.contactEmail"
                                value={formData.cargoOwnerDetails.contactEmail}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="cargoOwnerContactPhone">Номер телефона:</label>
                            <input
                                type="text"
                                id="cargoOwnerContactPhone"
                                name="cargoOwnerDetails.contactPhone"
                                value={formData.cargoOwnerDetails.contactPhone}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    </div>
                )}

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

                <button type="submit">Зарегистрироваться</button>
            </form>
        </div>
    );
}

export default Register;
