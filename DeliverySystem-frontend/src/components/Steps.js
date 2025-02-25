import React from 'react';

function Steps() {
    return (
        <div className="steps-container">
            <h3 className="steps-title">Как работает наша система:</h3>
            <div className="steps-grid">
                <div className="step">
                    <div className="step-icon">📝📄</div>
                    <p>Создайте заказ на доставку или перевозку</p>
                </div>
                <div className="step">
                    <div className="step-icon">📦📏</div>
                    <p>Уточните параметры груза или транспорта</p>
                </div>
                <div className="step">
                    <div className="step-icon">🤝🧑‍💻</div>
                    <p>Найдите надежного партнера</p>
                </div>
                <div className="step">
                    <div className="step-icon">🚛📍</div>
                    <p>Отслеживайте доставку в реальном времени</p>
                </div>
                <div className="step">
                    <div className="step-icon">📬✅</div>
                    <p>Получите уведомление о доставке и заберите заказ</p>
                </div>
            </div>
        </div>
    );
}

export default Steps;
