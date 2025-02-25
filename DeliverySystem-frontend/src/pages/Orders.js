import React, {useState, useEffect} from 'react';
import axios from 'axios';
import {Link} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClock, faCheckCircle, faTruck, faBox, faFlagCheckered, faTimesCircle, faPlusCircle } from '@fortawesome/free-solid-svg-icons';

function Orders() {
    const [orders, setOrders] = useState([]);
    const [message, setMessage] = useState('');
    const [noOrdersMessage, setNoOrdersMessage] = useState('');
    const [loading, setLoading] = useState(true);
    const [statusFilter, setStatusFilter] = useState('all');
    const [searchOrderId, setSearchOrderId] = useState('');
    const [searchParticipantId, setSearchParticipantId] = useState('');
    const statusMap = {
        Created: { label: "Создан", icon: faPlusCircle, color: "#3498db" },
        Pending: { label: "Ожидание", icon: faClock, color: "#f1c40f" },
        Confirmed: { label: "Подтвержден", icon: faCheckCircle, color: "#2ecc71" },
        InProgress: { label: "В процессе", icon: faTruck, color: "#e67e22" },
        Delivered: { label: "Доставлен", icon: faBox, color: "#8e44ad" },
        Completed: { label: "Выполнен", icon: faFlagCheckered, color: "#1abc9c" },
        Cancelled: { label: "Отменён", icon: faTimesCircle, color: "#e74c3c" }
    };

    useEffect(() => {
        const fetchOrders = async () => {
            const role = localStorage.getItem('role');
            const entityId = localStorage.getItem('entityId');

            if (!role || !entityId) {
                setMessage('Ошибка: данные пользователя отсутствуют.');
                setLoading(false);
                return;
            }

            try {
                let response;
                if (role === 'Carrier') {
                    response = await axios.get(`/api/order/carrier/${entityId}`, {withCredentials: true});
                } else if (role === 'CargoOwner') {
                    response = await axios.get(`/api/order/cargo-owner/${entityId}`, {withCredentials: true});
                }

                if (response.data.length === 0) {
                    setNoOrdersMessage('У вас нет заказов.');
                } else {
                    setOrders(response.data);
                }
            } catch (error) {
                if (error.response && error.response.data) {
                    setMessage(error.response.data.message || 'Ошибка при загрузке заказов.');
                } else {
                    setMessage('Не удалось загрузить заказы.');
                }
            } finally {
                setLoading(false);
            }
        };

        fetchOrders();
    }, []);

    const filteredOrders = orders.filter(order => {
        const statusMatch = statusFilter === 'all' || order.orderStatus === statusFilter;
        const orderIdMatch = searchOrderId === '' || order.id.toString().includes(searchOrderId);
        const participantIdMatch = searchParticipantId === '' ||
            order.cargoOwnerId.toString().includes(searchParticipantId) ||
            order.carrierId?.toString().includes(searchParticipantId);

        return statusMatch && orderIdMatch && participantIdMatch;
    });

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }

    if (message) {
        return <div>{message}</div>;
    }

    return (
        <div className="my-orders-container">
            <div className="filters-container">
                <h3>Фильтры</h3>
                <label>
                    Статус заказа:
                    <select
                        value={statusFilter}
                        onChange={(e) => setStatusFilter(e.target.value)}
                    >
                        <option value="all">Все</option>
                        <option value="Created">Создан</option>
                        <option value="Pending">Ожидание</option>
                        <option value="Confirmed">Подтвержден</option>
                        <option value="InProgress">В процессе</option>
                        <option value="Delivered">Доставлен</option>
                        <option value="Completed">Выполнен</option>
                        <option value="Cancelled">Отменён</option>
                    </select>
                </label>

                <label>
                    Поиск по ID заказа:
                    <input
                        type="text"
                        placeholder="Введите ID заказа"
                        value={searchOrderId}
                        onChange={(e) => setSearchOrderId(e.target.value)}
                    />
                </label>

                <label>
                    Поиск по ID участника:
                    <input
                        type="text"
                        placeholder="Введите ID участника"
                        value={searchParticipantId}
                        onChange={(e) => setSearchParticipantId(e.target.value)}
                    />
                </label>
            </div>

            <div className="orders-container">
                <h2>Мои заказы</h2>
                
                {noOrdersMessage ? (
                    <p>{noOrdersMessage}</p>
                ) : (
                    <div className="orders-list">
                        {filteredOrders.map(order => {
                            const statusInfo = statusMap[order.orderStatus] || {};
                            let requestIcon = <FontAwesomeIcon icon={statusInfo.icon} className="order-icon" style={{color: statusInfo.color}} />;
                            
                            return (
                                <Link to={`/order/${order.id}`} key={order.id} className="order-list-item-link">
                                    <div className="order-list-item">
                                        {requestIcon}
                                        <p><strong>Номер заказа: </strong>{order.id}</p>
                                        <p><strong>Маршрут: </strong>{order.senderAddress?.city}
                                            <strong> - </strong>{order.recipientAddress?.city}</p>
                                        <p><strong>Стоимость: </strong>{order.price} руб.</p>
                                        <p><strong>Статус заказа: </strong>{statusInfo.label}</p>
                                    </div>
                                </Link>
                            );
                        })}
                    </div>
                )}
            </div>
        </div>
    );
}

export default Orders;