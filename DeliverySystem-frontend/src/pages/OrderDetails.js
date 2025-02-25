import React, {useState, useEffect} from 'react';
import {useParams, useNavigate, Link} from 'react-router-dom';
import axios from 'axios';
import CargoInfo from "../components/CargoInfo";
import TransportInfo from "../components/TransportInfo";
import ConfirmationModal from '../components/ConfirmationModal';

const OrderDetails = () => {
    const {id} = useParams();
    const navigate = useNavigate();

    const role = localStorage.getItem('role');
    const entityId = localStorage.getItem('entityId');

    const [order, setOrder] = useState({
        id: '',
        cargoOwnerId: role === "CargoOwner" ? entityId : '',
        carrierId: role === "Carrier" ? entityId : '',
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
        orderStatus: '',
        sentDate: '',
        plannedPickupDate: '',
        actualPickupDate: '',
        isOrderConfirmedByCarrier: '',
        isOrderConfirmedByCargoOwner: '',
        isCargoDelivered: ''
    });

    const [loading, setLoading] = useState(true);
    const [errors, setErrors] = useState([]);
    const [message, setMessage] = useState('');
    const [showModal, setShowModal] = useState(false);

    useEffect(() => {
        const fetchOrder = async () => {
            try {
                const response = await axios.get(`/api/order/${id}`);
                const data = response.data;

                setOrder({
                    ...data,
                    sentDate: data.sentDate ? data.sentDate.split('T')[0] : '',
                    plannedPickupDate: data.plannedPickupDate ? data.plannedPickupDate.split('T')[0] : '',
                    actualPickupDate: data.actualPickupDate ? data.actualPickupDate.split('T')[0] : '',
                    cargoOwnerId: role === "CargoOwner" ? parseInt(entityId, 10) : data.cargoOwnerId,
                    carrierId: role === "Carrier" ? parseInt(entityId, 10) : data.carrierId
                });
                setLoading(false);
            } catch (err) {
                setErrors(err.response?.data?.message || 'Ошибка при загрузке данных');
                setLoading(false);
            }
        };

        fetchOrder();
    }, [id]);

    const handleChange = (e) => {
        const {name, value, type, checked} = e.target;

        let parsedValue;

        if (type === 'number') {
            parsedValue = value === '' ? '' : parseInt(value, 10);
        } else if (type === 'checkbox') {
            parsedValue = checked;
        } else {
            parsedValue = value;
        }

        if (name.includes('.')) {
            const [parent, child] = name.split('.');
            setOrder((prevOrder) => ({
                ...prevOrder,
                [parent]: {
                    ...prevOrder[parent],
                    [child]: parsedValue,
                },
            }));
        } else {
            setOrder((prevOrder) => ({
                ...prevOrder,
                [name]: parsedValue,
            }));
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage('');
        setErrors([]);

        const requestData = {
            ...order,
            id: parseInt(order.id, 10),
            cargoOwnerId: parseInt(order.cargoOwnerId, 10),
            carrierId: order.carrierId ? parseInt(order.carrierId, 10) : null,
            price: parseFloat(order.price),
            distance: parseFloat(order.distance),
            cargoId: parseInt(order.cargoId, 10),
            transportId: order.transportId ? parseInt(order.transportId, 10) : null,
            sentDate: order.sentDate ? `${order.sentDate}T00:00:00Z` : null,
            plannedPickupDate: order.plannedPickupDate ? `${order.plannedPickupDate}T00:00:00Z` : null,
            actualPickupDate: order.actualPickupDate ? `${order.actualPickupDate}T00:00:00Z` : null,
            isOrderConfirmedByCargoOwner: Boolean(order.isOrderConfirmedByCargoOwner),
            isOrderConfirmedByCarrier: Boolean(order.isOrderConfirmedByCarrier),
            isCargoDelivered: Boolean(order.isCargoDelivered),
        };

        try {
            await axios.put(`/api/order`, requestData);
            setErrors([]);
            setMessage('Заказ успешно отредактирован!');
            setTimeout(() => navigate('/orders'), 1500);
        } catch (error) {
            if (error.response && error.response.data && Array.isArray(error.response.data)) {
                setErrors(error.response.data);
            } else {
                setErrors(['Ошибка при редактировании заказа.']);
            }
            setMessage('');
        }
    };

    const handleDelete = () => {
        setShowModal(true);
    };

    const confirmDelete = async () => {
        setShowModal(false);

        try {
            await axios.delete(`/api/order/${id}`, { withCredentials: true });
            navigate('/orders');
        } catch (error) {
            setMessage('Ошибка при удалении заказа.');
        }
    };

    const cancelDelete = () => {
        setShowModal(false);
    };

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }

    return (
        <div className="order-details">
            <h3>Редактирование заказа #{order.id}</h3>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="id">ID:</label>
                    <input
                        type="number"
                        id="id"
                        name="id"
                        value={order.id}
                        disabled
                    />
                </div>
                {role !== "CargoOwner" && (
                    <div>
                        <Link to={order.cargoOwnerId ? `/user/CargoOwner/${order.cargoOwnerId}` : '#'}>
                            <label style={{ cursor: order.cargoOwnerId ? 'pointer' : 'default' }}>
                                CargoOwnerId:
                            </label>
                        </Link>
                        <input type="number" value={order.cargoOwnerId} disabled/>
                    </div>
                )}
                {role !== "Carrier" && (
                    <div>
                        <Link to={order.carrierId ? `/user/Carrier/${order.carrierId}` : '#'}>
                            <label style={{ cursor: order.carrierId ? 'pointer' : 'default' }}>
                                CarrierId:
                            </label>
                        </Link>
                        <input type="number" value={order.carrierId} disabled/>
                    </div>
                )}
                <div>
                    <label htmlFor="price">Стоимость:</label>
                    <input
                        type="number"
                        id="price"
                        name="price"
                        value={order.price}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="address-fields">
                    <fieldset>
                        <legend>Адрес отправления</legend>
                        <div>
                            <label htmlFor="senderAddress.country">Страна:</label>
                            <input
                                type="text"
                                id="senderAddress.country"
                                name="senderAddress.country"
                                value={order.senderAddress?.country || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="senderAddress.city">Город:</label>
                            <input
                                type="text"
                                id="senderAddress.city"
                                name="senderAddress.city"
                                value={order.senderAddress?.city || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="senderAddress.street">Улица:</label>
                            <input
                                type="text"
                                id="senderAddress.street"
                                name="senderAddress.street"
                                value={order.senderAddress?.street || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="senderAddress.houseNumber">Дом:</label>
                            <input
                                type="text"
                                id="senderAddress.houseNumber"
                                name="senderAddress.houseNumber"
                                value={order.senderAddress?.houseNumber || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="senderAddress.postalCode">Индекс:</label>
                            <input
                                type="text"
                                id="senderAddress.postalCode"
                                name="senderAddress.postalCode"
                                value={order.senderAddress?.postalCode || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Адрес прибытия</legend>
                        <div>
                            <label htmlFor="recipientAddress.country">Страна:</label>
                            <input
                                type="text"
                                id="recipientAddress.country"
                                name="recipientAddress.country"
                                value={order.recipientAddress?.country || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="recipientAddress.city">Город:</label>
                            <input
                                type="text"
                                id="recipientAddress.city"
                                name="recipientAddress.city"
                                value={order.recipientAddress?.city || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="recipientAddress.street">Улица:</label>
                            <input
                                type="text"
                                id="recipientAddress.street"
                                name="recipientAddress.street"
                                value={order.recipientAddress?.street || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="recipientAddress.houseNumber">Дом:</label>
                            <input
                                type="text"
                                id="recipientAddress.houseNumber"
                                name="recipientAddress.houseNumber"
                                value={order.recipientAddress?.houseNumber || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div>
                            <label htmlFor="recipientAddress.postalCode">Индекс:</label>
                            <input
                                type="text"
                                id="recipientAddress.postalCode"
                                name="recipientAddress.postalCode"
                                value={order.recipientAddress?.postalCode || ''}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    </fieldset>
                </div>
                <div>
                    <label htmlFor="distance">Расстояние:</label>
                    <input
                        type="number"
                        id="distance"
                        name="distance"
                        value={order.distance}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="cargoId">CargoId:</label>
                    <input
                        type="number"
                        id="cargoId"
                        name="cargoId"
                        value={order.cargoId}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="transportId">TransportId:</label>
                    <input
                        type="number"
                        id="transportId"
                        name="transportId"
                        value={order.transportId}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="orderStatus">Статус заказа:</label>
                    <select
                        id="orderStatus"
                        name="orderStatus"
                        value={order.orderStatus}
                        onChange={handleChange}
                    >
                        <option value="Created">Создан</option>
                        <option value="Pending">Ожидание</option>
                        <option value="Confirmed">Подтвержден</option>
                        <option value="InProgress">В процессе</option>
                        <option value="Delivered">Доставлен</option>
                        <option value="Completed">Выполнен</option>
                        <option value="Cancelled">Отменён</option>
                    </select>
                </div>
                <div>
                    <label htmlFor="sentDate">Дата отправки:</label>
                    <input
                        type="date"
                        id="sentDate"
                        name="sentDate"
                        value={order.sentDate}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="plannedPickupDate">Планируемая дата прибытия:</label>
                    <input
                        type="date"
                        id="plannedPickupDate"
                        name="plannedPickupDate"
                        value={order.plannedPickupDate}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="actualPickupDate">Реальная дата прибытия:</label>
                    <input
                        type="date"
                        id="actualPickupDate"
                        name="actualPickupDate"
                        value={order.actualPickupDate || ''}
                        onChange={handleChange}
                    />
                </div>
                <div className="checkbox-container">
                    <label htmlFor="isOrderConfirmedByCarrier">Подтвержден перевозчиком:</label>
                    <input type="checkbox" id="isOrderConfirmedByCarrier" name="isOrderConfirmedByCarrier"
                           checked={order.isOrderConfirmedByCarrier} onChange={handleChange}/>
                </div>
                <div className="checkbox-container">
                    <label htmlFor="isOrderConfirmedByCargoOwner">Подтвержден владельцем груза:</label>
                    <input type="checkbox" id="isOrderConfirmedByCargoOwner" name="isOrderConfirmedByCargoOwner"
                           checked={order.isOrderConfirmedByCargoOwner} onChange={handleChange}/>
                </div>
                <div className="checkbox-container">
                    <label htmlFor="isCargoDelivered">Груз доставлен:</label>
                    <input type="checkbox" id="isCargoDelivered" name="isCargoDelivered"
                           checked={order.isCargoDelivered} onChange={handleChange}/>
                </div>
                <div>
                    <button type="submit">Сохранить изменения</button>
                </div>
            </form>

            <div className="cargo-transport-info">
                {order.cargoId && <CargoInfo cargoId={order.cargoId}/>}
                {order.transportId && <TransportInfo transportId={order.transportId}/>}
            </div>

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

            {showModal && (
                <ConfirmationModal
                    message="Вы уверены, что хотите удалить этот заказ?"
                    onConfirm={confirmDelete}
                    onCancel={cancelDelete}
                />
            )}

            <div className="button-container">
                <button onClick={() => navigate('/orders')}>Назад</button>
                <button onClick={handleDelete}>Удалить заказ</button>
            </div>
        </div>
    );
};

export default OrderDetails;
