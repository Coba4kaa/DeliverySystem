import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function RegisterCargo() {
    const [cargoOwnerId, setCargoOwnerId] = useState(localStorage.getItem('entityId') || '');
    const [weight, setWeight] = useState('');
    const [volume, setVolume] = useState('');
    const [cargoType, setCargoType] = useState('');
    const [message, setMessage] = useState('');
    const [errors, setErrors] = useState([]);
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            const payload = {
                cargoOwnerId: parseInt(cargoOwnerId, 10),
                weight: parseFloat(weight),
                volume: parseFloat(volume),
                cargoType,
            };

            await axios.post('/api/cargoOwner/cargo', payload, { withCredentials: true });
            setMessage('Груз успешно зарегистрирован!');
            setTimeout(() => navigate('/profile'), 2000);
        } catch (error) {
            if (error.response && error.response.data && Array.isArray(error.response.data)) {
                setErrors(error.response.data);
            } else {
                setErrors(['Ошибка регистрации груза.']);
            }
            setMessage('');
        }
    };

    return (
        <div className="register-cargo-container">
            <h2>Регистрация груза</h2>
            <form onSubmit={handleRegister}>
                <label>
                    Вес (кг):
                    <input
                        type="number"
                        step="0.01"
                        value={weight}
                        onChange={(e) => setWeight(e.target.value)}
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
                    Тип груза:
                    <input
                        type="text"
                        value={cargoType}
                        onChange={(e) => setCargoType(e.target.value)}
                        required
                    />
                </label>
                <button type="submit">Зарегистрировать груз</button>
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

export default RegisterCargo;
