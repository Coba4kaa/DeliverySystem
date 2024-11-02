import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';

const OrderList = () => {
    const [orders, setOrders] = useState([]);

    useEffect(() => {
        const fetchOrders = async () => {
            const response = await axios.get('/api/order');
            setOrders(response.data);
        };
        fetchOrders();
    }, []);

    return (
        <div className="main-container">
            <h2>Список заказов</h2>
            <ul>
                {orders.map((order) => (
                    <li key={order.id} className="order-list-item">
                        <Link to={`/orders/${order.id}`}>
                            {order.senderCity} → {order.recipientCity}
                        </Link>
                        <p>Дата забора: {new Date(order.pickupDate).toLocaleDateString()}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OrderList;
