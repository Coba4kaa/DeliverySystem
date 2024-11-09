import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import OrderInfo from './OrderInfo';

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
            <OrderInfo order={order} />
        </div>
    );
};

export default OrderDetails;
