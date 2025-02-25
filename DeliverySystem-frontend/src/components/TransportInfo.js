import React, { useState, useEffect } from 'react';
import axios from 'axios';

function TransportInfo({ transportId }) {
    const [transport, setTransport] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchTransport = async () => {
            try {
                const response = await axios.get(`/api/carrier/transport/${transportId}`, { withCredentials: true });
                setTransport(response.data);
            } catch (err) {
                setError('Ошибка при загрузке данных о транспорте');
            } finally {
                setLoading(false);
            }
        };

        if (transportId) {
            fetchTransport();
        }
    }, [transportId]);

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }

    if (error) {
        return <div>{error}</div>;
    }

    if (!transport) {
        return <div>Нет данных о транспорте</div>;
    }

    return (
        <div className="transport-info">
            <h2>Детали транспорта</h2>
            <p><strong>ID:</strong> {transport.id}</p>
            <p><strong>Carrier ID:</strong> {transport.carrierId}</p>
            <p><strong>Серийный номер:</strong> {transport.serialNumber}</p>
            <p><strong>Тип транспорта:</strong> {transport.transportType}</p>
            <p><strong>Объем (м³):</strong> {transport.volume}</p>
            <p><strong>Грузоподъемность (кг):</strong> {transport.loadCapacity}</p>
            <p><strong>Средняя скорость (км/ч):</strong> {transport.averageTransportationSpeed}</p>
            <p><strong>Статус:</strong> {transport.status}</p>
            <p><strong>Город:</strong> {transport.locationCity}</p>
        </div>
    );
}

export default TransportInfo;
