import React from 'react';
import Steps from '../components/Steps';
import {useNavigate} from 'react-router-dom';


function Home() {
    const navigate = useNavigate();

    const navigateToCreateOrder = () => {
        navigate('/create-order');
    };
    
    return (
        <div className="home-container">
            <h2 className="home-title">Добро пожаловать в Delivery System!</h2>
            <p className="home-subtitle">Система доставки, которая помогает вам:</p>

            <ul className="home-list">
                <li>🚚 Организовать перевозку грузов различного типа (от мелких посылок до крупных товаров).</li>
                <li>🤝 Найти надежных перевозчиков для транспортировки ваших товаров.</li>
                <li>📍 Отслеживать статус перевозки и получать обновления о местоположении груза в реальном времени.</li>
                <li>📅 Управлять заказами и планировать маршруты для оптимизации доставки.</li>
                <li>💰 Контролировать расходы на транспортировку и выбрать лучшие условия для вашего бизнеса.</li>
            </ul>

            <p className="home-text">Независимо от размера или сложности вашей доставки, наша система поможет сделать процесс простым и удобным.</p>

            <button className="cta-button" onClick={navigateToCreateOrder}>Создать свой первый заказ!</button>

            <Steps />

            <div className="truck-image-container">
                <img src="https://sc01.alicdn.com/kf/H99eec71560b046e287c2ee61e054e029K/206028640/H99eec71560b046e287c2ee61e054e029K.jpg"
                     alt="Грузовик"
                     className="truck-image" />
            </div>
        </div>
    );
}

export default Home;
