import React, {useEffect, useState} from 'react';
import axios from 'axios';
import {Link, useNavigate} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBox, faTruck } from '@fortawesome/free-solid-svg-icons';

const Requests = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [minPrice, setMinPrice] = useState(0);
    const [maxPrice, setMaxPrice] = useState(10000);
    const [minPriceSlider, setMinPriceSlider] = useState(0);
    const [maxPriceSlider, setMaxPriceSlider] = useState(10000);
    const [senderCity, setSenderCity] = useState('');
    const [recipientCity, setRecipientCity] = useState('');
    const navigate = useNavigate();
    const statusMap = {
        Created: "Создан",
        Pending: "Ожидание",
        Confirmed: "Подтвержден",
        InProgress: "В процессе",
        Delivered: "Доставлен",
        Completed: "Выполнен",
        Cancelled: "Отменён"
    };

    useEffect(() => {
        fetchOrders();
        checkAuth();
    }, []);

    const fetchOrders = async () => {
        setLoading(true);
        try {
            const response = await axios.get('/api/order/status/Created');
            setOrders(response.data);
        } catch (error) {
            console.error('Ошибка загрузки заказов:', error);
            setOrders([]);
        } finally {
            setLoading(false);
        }
    };

    const checkAuth = () => {
        const userId = localStorage.getItem('userId');
        setIsAuthenticated(!!userId);
    };

    const navigateToCreateOrder = () => {
        navigate('/create-order');
    };

    const filteredOrders = orders.filter(order => {
        const priceMatch =
            (minPrice === '' || order.price >= parseFloat(minPrice)) &&
            (maxPrice === '' || order.price <= parseFloat(maxPrice));

        const senderCityMatch = senderCity === '' ||
            order.senderAddress?.city.toLowerCase().includes(senderCity.toLowerCase());

        const recipientCityMatch = recipientCity === '' ||
            order.recipientAddress?.city.toLowerCase().includes(recipientCity.toLowerCase());

        return priceMatch && senderCityMatch && recipientCityMatch;
    });

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }
    
    return (
        <div className="requests-container">
            <div className="filters-container">
                <h3>Фильтры</h3>
                <label>
                    Минимальная цена:
                    <input
                        type="range"
                        min="0"
                        max="10000"
                        value={minPriceSlider}
                        onChange={(e) => {
                            setMinPriceSlider(e.target.value);
                            setMinPrice(e.target.value);
                        }}
                    />
                    <input
                        type="number"
                        value={minPrice}
                        onChange={(e) => {
                            setMinPrice(e.target.value);
                            setMinPriceSlider(e.target.value);
                        }}
                    />
                </label>

                <label>
                    Максимальная цена:
                    <input
                        type="range"
                        min="0"
                        max="10000"
                        value={maxPriceSlider}
                        onChange={(e) => {
                            setMaxPriceSlider(e.target.value);
                            setMaxPrice(e.target.value);
                        }}
                    />
                    <input
                        type="number"
                        value={maxPrice}
                        onChange={(e) => {
                            setMaxPrice(e.target.value);
                            setMaxPriceSlider(e.target.value);
                        }}
                    />
                </label>

                <label>
                    Город отправителя:
                    <input
                        type="text"
                        placeholder="Введите город"
                        value={senderCity}
                        onChange={(e) => setSenderCity(e.target.value)}
                    />
                </label>

                <label>
                    Город получателя:
                    <input
                        type="text"
                        placeholder="Введите город"
                        value={recipientCity}
                        onChange={(e) => setRecipientCity(e.target.value)}
                    />
                </label>

                {isAuthenticated && (
                    <div className="create-order-button-container">
                        <button onClick={navigateToCreateOrder}>Оформить заявку</button>
                    </div>
                )}
            </div>

            <div className="orders-container">
                <h2>Список заявок</h2>
                <div className="orders-list">
                    {filteredOrders.length > 0 ? (
                        filteredOrders.map(order => {
                            let requestIcon = null;

                            if (order.cargoOwnerId) {
                                requestIcon = <FontAwesomeIcon icon={faBox} className="order-icon owner-icon" />;
                            } else if (order.carrierId) {
                                requestIcon = <FontAwesomeIcon icon={faTruck} className="order-icon carrier-icon" />;
                            }

                            return (
                                <Link to={`/order-info/${order.id}`} key={order.id} className="order-list-item-link">
                                    <div className="order-list-item">
                                        {requestIcon}
                                        <p><strong>Номер заказа: </strong>{order.id}</p>
                                        <p><strong>Маршрут: </strong>{order.senderAddress?.city}
                                            <strong> - </strong>{order.recipientAddress?.city}</p>
                                        <p><strong>Стоимость: </strong>{order.price} руб.</p>
                                        <p><strong>Статус заказа: </strong>{statusMap[order.orderStatus] || order.orderStatus}</p>
                                    </div>
                                </Link>
                            );
                        })
                    ) : (
                        <p>Заказы не найдены</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Requests;