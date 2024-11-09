import React, { useState } from 'react';
import axios from 'axios';
import OrderInfo from './OrderInfo';

const OrderSearch = () => {
    const [orderId, setOrderId] = useState('');
    const [order, setOrder] = useState(null);
    const [notification, setNotification] = useState('');

    const handleSearch = async () => {
        try {
            const response = await axios.get(`/api/order/${orderId}`);
            setOrder(response.data);
            setNotification('');
        } catch (error) {
            setOrder(null);
            setNotification('Ошибка при поиске заказа: заказ не найден');
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

            {notification && (
                <div className="notification" style={{ color: 'red' }}>
                    {notification}
                </div>
            )}
            <OrderInfo order={order} />
        </div>
    );
};

export default OrderSearch;
