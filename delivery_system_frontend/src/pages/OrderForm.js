import React, {useState} from 'react';
import axios from 'axios';

const OrderForm = () => {
    const [formData, setFormData] = useState({
        senderCity: '',
        senderAddress: '',
        recipientCity: '',
        recipientAddress: '',
        weight: '',
        pickupDate: '',
    });
    const [notification, setNotification] = useState('');

    const handleChange = (e) => {
        const {name, value} = e.target;
        setFormData({...formData, [name]: value});
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await axios.post('/api/order', formData);
            setNotification('Заказ успешно создан!');
            setFormData({
                senderCity: '',
                senderAddress: '',
                recipientCity: '',
                recipientAddress: '',
                weight: '',
                pickupDate: '',
            });
        } catch (error) {
            setNotification('Ошибка при создании заказа: ' + (error.response ? error.response.data : error.message));
        }

        setTimeout(() => {
            setNotification('');
        }, 15000);
    };

    return (
        <div className="main-container">
            <h2>Создать новый заказ</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="senderCity"
                    placeholder="Город отправителя"
                    value={formData.senderCity}
                    onChange={handleChange}
                    required
                />
                <input
                    type="text"
                    name="senderAddress"
                    placeholder="Адрес отправителя"
                    value={formData.senderAddress}
                    onChange={handleChange}
                    required
                />
                <input
                    type="text"
                    name="recipientCity"
                    placeholder="Город получателя"
                    value={formData.recipientCity}
                    onChange={handleChange}
                    required
                />
                <input
                    type="text"
                    name="recipientAddress"
                    placeholder="Адрес получателя"
                    value={formData.recipientAddress}
                    onChange={handleChange}
                    required
                />
                <input
                    type="number"
                    name="weight"
                    placeholder="Вес груза"
                    value={formData.weight}
                    onChange={handleChange}
                    required
                />
                <input
                    type="date"
                    name="pickupDate"
                    value={formData.pickupDate}
                    onChange={handleChange}
                    required
                />
                <button type="submit">Создать заказ</button>
            </form>

            {notification && (
                <div className="notification"
                     style={{color: notification.includes('успешно') ? 'green' : 'red'}}>
                    {notification}
                </div>
            )}
        </div>
    );
};

export default OrderForm;
