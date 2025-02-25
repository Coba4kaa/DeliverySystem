import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function RegisterTransport() {
    const [carrierId, setCarrierId] = useState(localStorage.getItem('entityId') || '');
    const [serialNumber, setSerialNumber] = useState('');
    const [transportType, setTransportType] = useState('');
    const [volume, setVolume] = useState('');
    const [loadCapacity, setLoadCapacity] = useState('');
    const [averageSpeed, setAverageSpeed] = useState('');
    const [locationCity, setLocationCity] = useState('');
    const [message, setMessage] = useState('');
    const [errors, setErrors] = useState([]);
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            const payload = {
                carrierId: parseInt(carrierId, 10),
                serialNumber,
                transportType,
                volume: parseFloat(volume),
                loadCapacity: parseFloat(loadCapacity),
                averageTransportationSpeed: parseInt(averageSpeed, 10),
                status: 'AVAILABLE',
                locationCity,
            };

            await axios.post('/api/carrier/transport', payload, { withCredentials: true });
            setErrors([]);
            setMessage('Транспорт успешно зарегистрирован!');
            setTimeout(() => navigate('/profile'), 2000);
        } catch (error) {
            if (error.response && error.response.data && Array.isArray(error.response.data)) {
                setErrors(error.response.data);
            } else {
                setErrors(['Ошибка регистрации транспорта.']);
            }
            setMessage('');
        }
    };

    return (
        <div className="register-transport-container">
            <h2>Регистрация транспорта</h2>
            <form onSubmit={handleRegister}>
                <label>
                    Серийный номер:
                    <input
                        type="text"
                        value={serialNumber}
                        onChange={(e) => setSerialNumber(e.target.value)}
                        required
                    />
                </label>
                <label>
                    Тип транспорта:
                    <input
                        type="text"
                        value={transportType}
                        onChange={(e) => setTransportType(e.target.value)}
                        required
                    />
                </label>
                <label>
                    Объем (м³):
                    <input
                        type="number"
                        step="0.01"
                        value={volume}
                        onChange={(e) => setVolume(e.target.value)}
                        required
                    />
                </label>
                <label>
                    Грузоподъемность (кг):
                    <input
                        type="number"
                        step="0.01"
                        value={loadCapacity}
                        onChange={(e) => setLoadCapacity(e.target.value)}
                        required
                    />
                </label>
                <label>
                    Средняя скорость (км/ч):
                    <input
                        type="number"
                        value={averageSpeed}
                        onChange={(e) => setAverageSpeed(e.target.value)}
                        required
                    />
                </label>
                <label>
                    Текущее местоположение (город):
                    <input
                        type="text"
                        value={locationCity}
                        onChange={(e) => setLocationCity(e.target.value)}
                        required
                    />
                </label>
                <button type="submit">Зарегистрировать транспорт</button>
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

            <button onClick={() => navigate('/profile')}>Назад</button>
        </div>
    );
}

export default RegisterTransport;