import React, { useState } from 'react';
import axios from 'axios';

const OrderSearch = () => {
    const [orderId, setOrderId] = useState('');
    const [order, setOrder] = useState(null);
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const response = await axios.get(`/api/order/${orderId}`);
            setOrder(response.data);
            setError('');
        } catch (error) {
            setOrder(null);
            setError('Заказ не найден');
        }
    };

    return (
        <div className="search-container">
            <h2>Поиск заказа по ID</h2>
            <input
                type="text"
                placeholder="Введите ID заказа"
                value={orderId}
                onChange={(e) => setOrderId(e.target.value)}
            />
            <button onClick={handleSearch}>Найти</button>

            {error && <p className="error-message">{error}</p>}

            {order && (
                <div>
                    <h3>Детали заказа</h3>
                    <p><strong>ID:</strong> {order.id}</p>
                    <p><strong>Город отправителя:</strong> {order.senderCity}</p>
                    <p><strong>Адрес отправителя:</strong> {order.senderAddress}</p>
                    <p><strong>Город получателя:</strong> {order.recipientCity}</p>
                    <p><strong>Адрес получателя:</strong> {order.recipientAddress}</p>
                    <p><strong>Вес груза:</strong> {order.weight} кг</p>
                    <p><strong>Дата забора груза:</strong> {new Date(order.pickupDate).toLocaleString()}</p>
                </div>
            )}
        </div>
    );
};

export default OrderSearch;
