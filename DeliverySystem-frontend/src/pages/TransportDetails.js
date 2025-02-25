import React, {useState, useEffect} from 'react';
import axios from 'axios';
import {useNavigate, useParams} from 'react-router-dom';
import ConfirmationModal from "../components/ConfirmationModal";

function TransportDetails() {
    const {id} = useParams();
    const [transport, setTransport] = useState({
        id: '',
        carrierId: '',
        serialNumber: '',
        transportType: '',
        volume: '',
        loadCapacity: '',
        averageTransportationSpeed: '',
        status: '',
        locationCity: ''
    });
    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState('');
    const [errors, setErrors] = useState([]);
    const navigate = useNavigate();
    const [showModal, setShowModal] = useState(false);
    const transportStatuses = ['Available', 'InUse', 'UnderMaintenance'];

    useEffect(() => {
        const fetchTransport = async () => {
            try {
                const response = await axios.get(`/api/carrier/transport/${id}`, {withCredentials: true});
                setTransport(response.data);
            } catch (err) {
                setErrors(['Ошибка при загрузке данных о транспорте']);
            } finally {
                setLoading(false);
            }
        };

        fetchTransport();
    }, [id]);

    const handleChange = (e) => {
        const {name, value} = e.target;
        setTransport(prevTransport => ({
            ...prevTransport,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const updatedTransport = {
                id: transport.id,
                carrierId: transport.carrierId,
                serialNumber: transport.serialNumber,
                transportType: transport.transportType,
                volume: transport.volume,
                loadCapacity: transport.loadCapacity,
                averageTransportationSpeed: transport.averageTransportationSpeed,
                status: transport.status,
                locationCity: transport.locationCity
            };

            await axios.put(`/api/carrier/transport`, updatedTransport, {withCredentials: true});
            setErrors([]);
            setMessage('Транспорт успешно отредактирован!');
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
            await axios.delete(`/api/carrier/transport/${id}`, {withCredentials: true});
            navigate('/profile');
        } catch (err) {
            setErrors(['Ошибка при удалении транспорта']);
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
        <div className="transport-details">
            <h2>Детали транспорта</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="id">ID:</label>
                    <input
                        type="number"
                        id="id"
                        name="id"
                        value={transport.id}
                        onChange={handleChange}
                        disabled
                    />
                </div>
                <div>
                    <label htmlFor="carrierId">Carrier ID:</label>
                    <input
                        type="number"
                        id="carrierId"
                        name="carrierId"
                        value={transport.carrierId}
                        onChange={handleChange}
                        disabled
                    />
                </div>
                <div>
                    <label htmlFor="serialNumber">Серийный номер:</label>
                    <input
                        type="text"
                        id="serialNumber"
                        name="serialNumber"
                        value={transport.serialNumber}
                        onChange={handleChange}
                        disabled
                    />
                </div>
                <div>
                    <label htmlFor="transportType">Тип транспорта:</label>
                    <input
                        type="text"
                        id="transportType"
                        name="transportType"
                        value={transport.transportType}
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
                        value={transport.volume}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="loadCapacity">Грузоподъемность (кг):</label>
                    <input
                        type="number"
                        id="loadCapacity"
                        name="loadCapacity"
                        value={transport.loadCapacity}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="averageTransportationSpeed">Средняя скорость (км/ч):</label>
                    <input
                        type="number"
                        id="averageTransportationSpeed"
                        name="averageTransportationSpeed"
                        value={transport.averageTransportationSpeed}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="status">Статус:</label>
                    <select
                        id="status"
                        name="status"
                        value={transport.status}
                        onChange={handleChange}
                        required
                    >
                        {transportStatuses.map((status, index) => (
                            <option key={index} value={status}>{status}</option>
                        ))}
                    </select>
                </div>
                <div>
                    <label htmlFor="locationCity">Город:</label>
                    <input
                        type="text"
                        id="locationCity"
                        name="locationCity"
                        value={transport.locationCity}
                        onChange={handleChange}
                        required
                    />
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
                    message="Вы уверены, что хотите удалить этот транспорт?"
                    onConfirm={confirmDelete}
                    onCancel={cancelDelete}
                />
            )}
            
            <button onClick={() => navigate('/profile')}>Назад</button>
            <button onClick={handleDelete}>Удалить транспорт</button>
        </div>
    );
}

export default TransportDetails;
