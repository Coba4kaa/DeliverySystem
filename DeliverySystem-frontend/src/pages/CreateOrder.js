import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function CreateOrder() {
    const [formData, setFormData] = useState({
        cargoOwnerId: '',
        carrierId: '',
        price: '',
        senderAddress: {
            country: '',
            city: '',
            street: '',
            houseNumber: '',
            postalCode: ''
        },
        recipientAddress: {
            country: '',
            city: '',
            street: '',
            houseNumber: '',
            postalCode: ''
        },
        distance: '',
        cargoId: '',
        transportId: '',
        sentDate: '',
        plannedPickupDate: '',
    });
    const [message, setMessage] = useState('');
    const [errors, setErrors] = useState([]);
    const navigate = useNavigate();

    const role = localStorage.getItem('role');
    const entityId = localStorage.getItem('entityId');

    useEffect(() => {
        if (role && entityId) {
            setFormData((prevData) => ({
                ...prevData,
                cargoOwnerId: role === 'CargoOwner' ? entityId : '',
                carrierId: role === 'Carrier' ? entityId : '',
            }));
        }
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        if (name.includes('.')) {
            const [field, subField] = name.split('.');
            setFormData((prevData) => ({
                ...prevData,
                [field]: {
                    ...prevData[field],
                    [subField]: value,
                },
            }));
        } else {
            setFormData((prevData) => ({ ...prevData, [name]: value }));
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage('');
        setErrors([]);

        const formatDateToUTC = (date) => {
            if (!date) return null;
            const utcDate = new Date(`${date}T00:00:00Z`);
            return utcDate.toISOString();
        };

        const payload = {
            cargoOwnerId: formData.cargoOwnerId ? parseInt(formData.cargoOwnerId) : null,
            carrierId: formData.carrierId ? parseInt(formData.carrierId) : null,
            price: formData.price ? parseFloat(formData.price) : null,
            senderAddress: { ...formData.senderAddress },
            recipientAddress: { ...formData.recipientAddress },
            distance: formData.distance ? parseInt(formData.distance) : null,
            cargoId: formData.cargoId ? parseInt(formData.cargoId) : null,
            transportId: formData.transportId ? parseInt(formData.transportId) : null,
            sentDate: formatDateToUTC(formData.sentDate),
            plannedPickupDate: formatDateToUTC(formData.plannedPickupDate),
        };

        try {
            await axios.post('/api/order', payload, {
                withCredentials: true,
            });
            setMessage('Заказ успешно создан!');
            setTimeout(() => navigate('/requests'), 2000);
        } catch (error) {
            if (error.response && error.response.data && Array.isArray(error.response.data)) {
                setErrors(error.response.data);
            } else {
                setErrors(['Ошибка при создании заказа.']);
            }
            setMessage('');
        }
    };

    return (
        <div className="create-order-container">
            <h2>Создать заказ</h2>
            <form onSubmit={handleSubmit}>
                <label>
                    ID владельца груза:
                    <input
                        type="text"
                        name="cargoOwnerId"
                        value={formData.cargoOwnerId}
                        onChange={handleChange}
                        disabled={role === "CargoOwner"}
                    />
                </label>
                <label>
                    ID перевозчика:
                    <input
                        type="text"
                        name="carrierId"
                        value={formData.carrierId}
                        onChange={handleChange}
                        disabled={role === "Carrier"}
                    />
                </label>
                <label>
                    Цена:
                    <input
                        type="number"
                        name="price"
                        value={formData.price}
                        onChange={handleChange}
                        placeholder="Цена"
                        required
                    />
                </label>
                <label>
                    Адрес отправителя:
                    <input
                        type="text"
                        name="senderAddress.country"
                        placeholder="Страна"
                        value={formData.senderAddress.country}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="senderAddress.city"
                        placeholder="Город"
                        value={formData.senderAddress.city}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="senderAddress.street"
                        placeholder="Улица"
                        value={formData.senderAddress.street}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="senderAddress.houseNumber"
                        placeholder="Номер дома"
                        value={formData.senderAddress.houseNumber}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="senderAddress.postalCode"
                        placeholder="Почтовый код"
                        value={formData.senderAddress.postalCode}
                        onChange={handleChange}
                        required
                    />
                </label>
                <label>
                    Адрес получателя:
                    <input
                        type="text"
                        name="recipientAddress.country"
                        placeholder="Страна"
                        value={formData.recipientAddress.country}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="recipientAddress.city"
                        placeholder="Город"
                        value={formData.recipientAddress.city}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="recipientAddress.street"
                        placeholder="Улица"
                        value={formData.recipientAddress.street}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="recipientAddress.houseNumber"
                        placeholder="Номер дома"
                        value={formData.recipientAddress.houseNumber}
                        onChange={handleChange}
                        required
                    />
                    <input
                        type="text"
                        name="recipientAddress.postalCode"
                        placeholder="Почтовый код"
                        value={formData.recipientAddress.postalCode}
                        onChange={handleChange}
                        required
                    />
                </label>
                <label>
                    Расстояние (км):
                    <input
                        type="number"
                        name="distance"
                        value={formData.distance}
                        onChange={handleChange}
                        required
                    />
                </label>
                <label>
                    ID груза:
                    <input
                        type="text"
                        name="cargoId"
                        value={formData.cargoId}
                        onChange={handleChange}
                        placeholder="Введите ID или оставьте пустым"
                    />
                </label>
                <label>
                    ID транспорта:
                    <input
                        type="text"
                        name="transportId"
                        value={formData.transportId}
                        onChange={handleChange}
                        placeholder="Введите ID или оставьте пустым"
                    />
                </label>
                <label>
                    Дата отправки:
                    <input
                        type="date"
                        name="sentDate"
                        value={formData.sentDate}
                        onChange={handleChange}
                    />
                </label>
                <label>
                    Планируемая дата забора:
                    <input
                        type="date"
                        name="plannedPickupDate"
                        value={formData.plannedPickupDate}
                        onChange={handleChange}
                        required
                    />
                </label>

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
                
                <button type="submit">Создать заказ</button>
            </form>
        </div>
    );
}

export default CreateOrder;
