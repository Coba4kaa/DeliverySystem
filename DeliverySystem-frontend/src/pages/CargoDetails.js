import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import ConfirmationModal from "../components/ConfirmationModal";

function CargoDetails() {
    const { id } = useParams();
    const [cargo, setCargo] = useState({
        id: '',
        cargoOwnerId: '',
        cargoType: '',
        weight: '',
        volume: ''
    });
    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState('');
    const [errors, setErrors] = useState([]);
    const navigate = useNavigate();
    const cargoStatuses = ['Registered', 'InProcess', 'Delivered'];
    const [showModal, setShowModal] = useState(false);

    useEffect(() => {
        const fetchCargo = async () => {
            try {
                const response = await axios.get(`/api/cargoOwner/cargo/${id}`, { withCredentials: true });
                setCargo(response.data);
            } catch (error) {
                setErrors(['Ошибка при загрузке данных о грузе']);
            } finally {
                setLoading(false);
            }
        };

        fetchCargo();
    }, [id]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setCargo(prevCargo => ({
            ...prevCargo,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const updatedCargo = {
                id: cargo.id,
                cargoOwnerId: cargo.cargoOwnerId,
                cargoType: cargo.cargoType,
                weight: cargo.weight,
                volume: cargo.volume
            };

            await axios.put(`/api/cargoOwner/cargo`, updatedCargo, { withCredentials: true });
            setErrors([]);
            setMessage('Груз успешно отредактирован!');
            setTimeout(() => navigate('/profile'), 2000);
        } catch (error) {
            if (error.response && error.response.data && Array.isArray(error.response.data)) {
                setErrors(error.response.data);
            } else {
                setErrors(['Ошибка при редактировании транспорта.']);
            }
            setMessage('');
        }
    };

    const handleDelete = () => {
        setShowModal(true);
    };
    
    const confirmDelete = async () => {
        setShowModal(false);
        
        try {
            await axios.delete(`/api/cargoOwner/cargo/${id}`, { withCredentials: true });
            navigate('/profile');
        } catch (error) {
            setErrors(['Ошибка при удалении груза']);
        }
    };

    const cancelDelete = () => {
        setShowModal(false);
    };

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }

    return (
        <div className="cargo-details">
            <h2>Детали груза</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="id">ID:</label>
                    <input
                        type="number"
                        id="id"
                        name="id"
                        value={cargo.id}
                        onChange={handleChange}
                        disabled
                    />
                </div>
                <div>
                    <label htmlFor="cargoOwnerId">CargoOwnerId:</label>
                    <input
                        type="number"
                        id="cargoOwnerId"
                        name="cargoOwnerId"
                        value={cargo.cargoOwnerId}
                        onChange={handleChange}
                        disabled
                    />
                </div>
                <div>
                    <label htmlFor="cargoType">Тип груза:</label>
                    <input
                        type="text"
                        id="cargoType"
                        name="cargoType"
                        value={cargo.cargoType}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="weight">Вес (кг):</label>
                    <input
                        type="number"
                        id="weight"
                        name="weight"
                        value={cargo.weight}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="volume">Объем (м³):</label>
                    <input
                        type="number"
                        id="volume"
                        name="volume"
                        value={cargo.volume}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="status">Статус:</label>
                    <select
                        id="status"
                        name="status"
                        value={cargo.status}
                        onChange={handleChange}
                        required
                    >
                        {cargoStatuses.map((status, index) => (
                            <option key={index} value={status}>{status}</option>
                        ))}
                    </select>
                </div>
                <div>
                    <button type="submit">Сохранить изменения</button>
                </div>
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

            {showModal && (
                <ConfirmationModal
                    message="Вы уверены, что хотите удалить этот груз?"
                    onConfirm={confirmDelete}
                    onCancel={cancelDelete}
                />
            )}
            
            <button onClick={() => navigate('/profile')}>Назад</button>
            <button onClick={handleDelete}>Удалить груз</button>
        </div>
    );
}

export default CargoDetails;
