import React, { useState, useEffect } from 'react';
import {useLocation, useNavigate, useParams} from 'react-router-dom';
import axios from 'axios';

const UserInfo = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { role, id } = useParams();
    const [roleData, setRoleData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                if (role === 'Carrier') {
                    const carrierResponse = await axios.get(`/api/carrier/${id}`, {
                        withCredentials: true,
                    });
                    setRoleData(carrierResponse.data);
                } else if (role === 'CargoOwner') {
                    const cargoOwnerResponse = await axios.get(`/api/cargoOwner/${id}`, {
                        withCredentials: true,
                    });
                    setRoleData(cargoOwnerResponse.data);
                }

                setLoading(false);
            } catch (err) {
                setError(err.response?.data?.message || 'Ошибка при загрузке данных');
                setLoading(false);
            }
        };

        fetchUserData();
    }, [id, role]);

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }
    if (error) return <p>{error}</p>;

    return (
        <div className="user-info">
            <h2>{`Пользователь #${id}`}</h2>
            <p>Роль: {role}</p>

            {roleData && role === 'Carrier' && (
                <div>
                    <h3>Информация о перевозчике</h3>
                    <p><strong>Компания:</strong> {roleData.companyName}</p>
                    <p><strong>Email:</strong> {roleData.contactEmail}</p>
                    <p><strong>Телефон:</strong> {roleData.contactPhone}</p>
                    <p><strong>Рейтинг:</strong> {roleData.rating}</p>
                </div>
            )}

            {roleData && role === 'CargoOwner' && (
                <div>
                    <h3>Информация о владельце груза</h3>
                    <p><strong>Компания:</strong> {roleData.companyName}</p>
                    <p><strong>Email:</strong> {roleData.contactEmail}</p>
                    <p><strong>Телефон:</strong> {roleData.contactPhone}</p>
                </div>
            )}

            <div className="button-container">
                <button onClick={() => navigate(location.state?.from || -1)}>Назад</button>
            </div>
        </div>
    );
};

export default UserInfo;
