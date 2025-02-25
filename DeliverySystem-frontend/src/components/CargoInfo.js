import React, { useState, useEffect } from 'react';
import axios from 'axios';

function CargoInfo({ cargoId }) {
    const [cargo, setCargo] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchCargo = async () => {
            try {
                const response = await axios.get(`/api/cargoOwner/cargo/${cargoId}`, { withCredentials: true });
                setCargo(response.data);
            } catch (err) {
                setError('Ошибка при загрузке данных о грузе');
            } finally {
                setLoading(false);
            }
        };

        if (cargoId) {
            fetchCargo();
        }
    }, [cargoId]);

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

    if (!cargo) {
        return <div>Нет данных о грузе</div>;
    }

    return (
        <div className="cargo-info">
            <h2>Детали груза</h2>
            <p><strong>ID:</strong> {cargo.id}</p>
            <p><strong>Cargo Owner ID:</strong> {cargo.cargoOwnerId}</p>
            <p><strong>Тип груза:</strong> {cargo.cargoType}</p>
            <p><strong>Вес (кг):</strong> {cargo.weight}</p>
            <p><strong>Объем (м³):</strong> {cargo.volume}</p>
        </div>
    );
}

export default CargoInfo;
