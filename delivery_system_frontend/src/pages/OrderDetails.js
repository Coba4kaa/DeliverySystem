import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';

const OrderDetails = () => {
    const { id } = useParams();
    const [order, setOrder] = useState(null);

    useEffect(() => {
        const fetchOrder = async () => {
            try {
                const response = await axios.get(`/api/order/${id}`);
                setOrder(response.data);
            } catch (error) {
                console.error("Ошибка при загрузке данных:", error);
            }
        };
        fetchOrder();
    }, [id]);

    if (!order) return <div>Loading...</div>;

    return (
        <div className="order-details">
            <h2>Детали заказа</h2>
            <p><strong>ID:</strong> {order.id}</p>
            <p><strong>Город отправителя:</strong> {order.senderCity}</p>
            <p><strong>Адрес отправителя:</strong> {order.senderAddress}</p>
            <p><strong>Город получателя:</strong> {order.recipientCity}</p>
            <p><strong>Адрес получателя:</strong> {order.recipientAddress}</p>
            <p><strong>Вес груза:</strong> {order.weight} кг</p>
            <p><strong>Дата забора груза:</strong> {new Date(order.pickupDate).toLocaleString()}</p>
        </div>
    );
};

export default OrderDetails;
