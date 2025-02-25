import React, {useState, useEffect} from 'react';
import {useParams, useNavigate, Link} from 'react-router-dom';
import axios from 'axios';
import CargoInfo from "../components/CargoInfo";
import TransportInfo from "../components/TransportInfo";

const OrderInfo = () => {
    const {id} = useParams();
    const navigate = useNavigate();
    const [order, setOrder] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [updateError, setUpdateError] = useState(null);

    const role = localStorage.getItem('role');
    const entityId = parseInt(localStorage.getItem('entityId'), 10);

    useEffect(() => {
        const fetchOrder = async () => {
            try {
                const response = await axios.get(`/api/order/${id}`);
                const data = response.data;
                setOrder({
                    ...data,
                    sentDate: data.sentDate ? data.sentDate.split('T')[0] : '',
                    plannedPickupDate: data.plannedPickupDate ? data.plannedPickupDate.split('T')[0] : '',
                    actualPickupDate: data.actualPickupDate ? data.actualPickupDate.split('T')[0] : ''
                });
                setLoading(false);
            } catch (err) {
                setError(err.response?.data?.message || 'Ошибка при загрузке данных');
                setLoading(false);
            }
        };

        fetchOrder();
    }, [id]);

    const handleAcceptOrder = async () => {
        if (!role || isNaN(entityId)) {
            setUpdateError('Ошибка: не найдены роль или ID пользователя.');
            return;
        }

        try {
            let updatedOrder = {
                ...order,
                cargoOwnerId: role === 'CargoOwner' && !order.cargoOwnerId ? entityId : order.cargoOwnerId,
                carrierId: role === 'Carrier' && !order.carrierId ? entityId : order.carrierId,
                id: parseInt(order.id, 10),
                price: parseFloat(order.price),
                distance: parseFloat(order.distance),
                cargoId: parseInt(order.cargoId, 10),
                transportId: order.transportId ? parseInt(order.transportId, 10) : null,
                sentDate: order.sentDate ? `${order.sentDate}T00:00:00Z` : null,
                plannedPickupDate: order.plannedPickupDate ? `${order.plannedPickupDate}T00:00:00Z` : null,
                actualPickupDate: order.actualPickupDate ? `${order.actualPickupDate}T00:00:00Z` : null,
                isOrderConfirmedByCargoOwner: Boolean(order.isOrderConfirmedByCargoOwner),
                isOrderConfirmedByCarrier: Boolean(order.isOrderConfirmedByCarrier),
                isCargoDelivered: false
            };

            await axios.put(`/api/order`, updatedOrder, {
                headers: {'Content-Type': 'application/json'},
                withCredentials: true
            });

            navigate('/orders');
        } catch (err) {
            setUpdateError(err.response?.data?.message || 'Ошибка при обновлении заказа');
        }
    };

    const formatValue = (value) => value ?? 'Не указано';

    if (loading) {
        return (
            <div className="spinner-container">
                <div className="spinner"></div>
            </div>
        );
    }
    if (error) return <p>{error}</p>;
    if (!order) return <p>Информация о заказе не найдена.</p>;

    const canAcceptOrder =
        (role === 'Carrier' && !order.carrierId) ||
        (role === 'CargoOwner' && !order.cargoOwnerId);

    return (
        <div className="order-info">
            <div className="order-info__text">
                <h3>Детали заказа</h3>
                <p><strong>ID: </strong> {order.id} </p>

                {order.cargoOwnerId && (
                    <p>
                        <Link to={`/user/CargoOwner/${order.cargoOwnerId}`}>
                            <strong>Cargo Owner ID: </strong> #{order.cargoOwnerId}
                        </Link>
                    </p>
                )}

                {order.carrierId && (
                    <p>
                        <Link to={`/user/Carrier/${order.carrierId}`}>
                            <strong>Carrier ID: </strong> #{order.carrierId}
                        </Link>
                    </p>
                )}

                <p><strong>Стоимость: </strong> {formatValue(order.price)} руб </p>
                <p><strong>Адрес
                    отправления: </strong> {formatValue(order.senderAddress?.country)}, {formatValue(order.senderAddress?.city)}, {formatValue(order.senderAddress?.street)}, {formatValue(order.senderAddress?.houseNumber)}
                </p>
                <p><strong>Адрес
                    прибытия: </strong> {formatValue(order.recipientAddress?.country)}, {formatValue(order.recipientAddress?.city)}, {formatValue(order.recipientAddress?.street)}, {formatValue(order.recipientAddress?.houseNumber)}
                </p>
                <p><strong>Расстояние: </strong> {formatValue(order.distance)} км </p>
                <p><strong>CargoId: </strong> {formatValue(order.cargoId)} </p>
                <p><strong>TransportId: </strong> {formatValue(order.transportId)} </p>
                <p><strong>Статус заказа: </strong> {formatValue(order.orderStatus)} </p>
                <p><strong>Дата
                    отправки: </strong> {formatValue(order.sentDate ? new Date(order.sentDate).toLocaleDateString() : '')}
                </p>
                <p><strong>Планируемая дата
                    прибытия: </strong> {formatValue(order.plannedPickupDate ? new Date(order.plannedPickupDate).toLocaleDateString() : '')}
                </p>
                <p><strong>Реальная дата
                    прибытия: </strong> {formatValue(order.actualPickupDate ? new Date(order.actualPickupDate).toLocaleDateString() : '')}
                </p>
            </div>
            <div className="cargo-transport-info">
                {order.cargoId && <CargoInfo cargoId={order.cargoId}/>}
                {order.transportId && <TransportInfo transportId={order.transportId}/>}
            </div>
            <div className="button-container">
                <button onClick={() => navigate('/requests')}>Назад</button>
                {canAcceptOrder && <button onClick={handleAcceptOrder}>Принять заказ</button>}
            </div>
            {updateError && <p className="error">{updateError}</p>}
        </div>
    );
};

export default OrderInfo;
