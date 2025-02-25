import React, {useState, useEffect} from 'react';
import axios from 'axios';
import {useNavigate} from 'react-router-dom';
import TransportList from "../components/TransportList";
import CargoList from "../components/CargoList";

function Profile() {
    const [userData, setUserData] = useState(null);
    const [roleData, setRoleData] = useState(null);
    const [message, setMessage] = useState('');
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();
    const [selectedTransportStatus, setSelectedTransportStatus] = useState('');
    const [selectedCargoStatus, setSelectedCargoStatus] = useState('');
    const transportStatuses = ['Available', 'InUse', 'UnderMaintenance'];
    const cargoStatuses = ['Registered', 'InProcess', 'Delivered'];

    useEffect(() => {
        const fetchProfile = async () => {
            const userId = localStorage.getItem('userId');
            const role = localStorage.getItem('role');
            const entityId = localStorage.getItem('entityId');

            if (!userId || !role || !entityId) {
                setMessage('Ошибка: данные пользователя отсутствуют.');
                setLoading(false);
                return;
            }

            try {
                const response = await axios.get(`/api/user/${userId}`, {
                    withCredentials: true,
                });
                const user = response.data;
                setUserData(user);

                if (role === 'Carrier') {
                    const carrierResponse = await axios.get(`/api/carrier/${entityId}`, {
                        withCredentials: true,
                    });
                    setRoleData(carrierResponse.data);
                } else if (role === 'CargoOwner') {
                    const cargoOwnerResponse = await axios.get(`/api/cargoOwner/${entityId}`, {
                        withCredentials: true,
                    });
                    setRoleData(cargoOwnerResponse.data);
                }
            } catch (error) {
                if (error.response && error.response.data) {
                    setMessage(error.response.data.message || 'Ошибка при загрузке профиля.');
                } else {
                    setMessage('Не удалось загрузить профиль.');
                }
            } finally {
                setLoading(false);
            }
        };

        fetchProfile();
    }, []);

    const handleTransportStatusChange = (event) => {
        setSelectedTransportStatus(event.target.value);
    };

    const handleCargoStatusChange = (event) => {
        setSelectedCargoStatus(event.target.value);
    };

    const filteredTransports = roleData?.transports?.filter(transport => {
        return selectedTransportStatus ? transport.status === selectedTransportStatus : true;
    }) || [];

    const filteredCargos = roleData?.cargos?.filter(cargo => {
        return selectedCargoStatus ? cargo.status === selectedCargoStatus : true;
    }) || [];

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }

    if (message) {
        return <div className="profile-container"><p>{message}</p></div>;
    }

    return (
        <div className="profile-container">
            <h2>Профиль</h2>
            <p><strong>Имя пользователя:</strong> {userData.login}</p>
            <p><strong>Роль:</strong> {userData.role}</p>

            {roleData && userData.role === 'Carrier' && (
                <div>
                    <p><strong>Компания:</strong> {roleData.companyName}</p>
                    <p><strong>Email:</strong> {roleData.contactEmail}</p>
                    <p><strong>Телефон:</strong> {roleData.contactPhone}</p>
                    <p><strong>Рейтинг:</strong> {roleData.rating}</p>
                    <h3>Мой транспорт</h3>
                    <div className="filter-container">
                        <label>Выберите статус транспорта:</label>
                        <select value={selectedTransportStatus} onChange={handleTransportStatusChange}>
                            <option value="">Все статусы</option>
                            {transportStatuses.map(status => (
                                <option key={status} value={status}>{status}</option>
                            ))}
                        </select>
                        <button onClick={() => navigate('/register-transport')}>
                            Зарегистрировать транспорт
                        </button>
                    </div>
                    <TransportList transports={filteredTransports}/>
                </div>
            )}

            {roleData && userData.role === 'CargoOwner' && (
                <div>
                    <p><strong>Компания:</strong> {roleData.companyName}</p>
                    <p><strong>Email:</strong> {roleData.contactEmail}</p>
                    <p><strong>Телефон:</strong> {roleData.contactPhone}</p>
                    <h3>Мои грузы</h3>
                    <div className="filter-container">
                        <label>Выберите статус груза:</label>
                        <select value={selectedCargoStatus} onChange={handleCargoStatusChange}>
                            <option value="">Все статусы</option>
                            {cargoStatuses.map(status => (
                                <option key={status} value={status}>{status}</option>
                            ))}
                        </select>
                        <button onClick={() => navigate('/register-cargo')}>
                            Зарегистрировать груз
                        </button>
                    </div>
                    <CargoList cargos={filteredCargos}/>
                </div>
            )}
        </div>
    );
}

export default Profile;
