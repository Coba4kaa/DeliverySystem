import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import OrderInfo from './OrderInfo';

const OrderList = () => {
    const [orders, setOrders] = useState([]);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await axios.get('/api/order');
                setOrders(response.data);
            } catch (error) {
                console.error("Ошибка при загрузке заказов:", error);
            }
        };
        fetchOrders();
    }, []);

    return (
        <div className="main-container">
            <h2>Список заказов</h2>
            <ul>
                {orders.map((order) => (
                    <li key={order.id} className="order-list-item">
                        <Link to={`/orders/${order.id}`} className="order-item-link">
                            <OrderInfo order={order} />
                        </Link>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OrderList;
