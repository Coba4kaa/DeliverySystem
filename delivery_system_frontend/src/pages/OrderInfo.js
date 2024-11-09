import React from 'react';

const OrderInfo = ({ order }) => {
    if (!order) return null;

    return (
        <div className="order-info">
            <h3>Детали заказа</h3>
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

export default OrderInfo;
