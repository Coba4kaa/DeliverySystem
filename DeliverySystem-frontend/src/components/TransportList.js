import React from 'react';
import { useNavigate } from 'react-router-dom';

function TransportList({ transports }) {
    const navigate = useNavigate();

    const handleRowClick = (id) => {
        navigate(`/transport/${id}`);
    };

    const getStatusColor = (status) => {
        switch (status) {
            case 'Available':
                return { backgroundColor: 'lightgreen', color: 'green' };
            case 'InUse':
                return { backgroundColor: 'lightyellow', color: 'orange' };
            case 'UnderMaintenance':
                return { backgroundColor: 'lightblue', color: 'blue' };
            default:
                return {};
        }
    };

    return (
        <div className="transport-list">
            {transports.length === 0 ? (
                <p>У вас нет транспорта.</p>
            ) : (
                <table>
                    <thead>
                    <tr>
                        <th>ID</th>
                        <th>Тип транспорта</th>
                        <th>Серийный номер</th>
                        <th>Объем</th>
                        <th>Грузоподъемность</th>
                        <th>Скорость</th>
                        <th>Статус</th>
                        <th>Город</th>
                    </tr>
                    </thead>
                    <tbody>
                    {transports.map((transport) => (
                        <tr
                            key={transport.id}
                            onClick={() => handleRowClick(transport.id)}
                            className="clickable-row"
                        >
                            <td>{transport.id}</td>
                            <td>{transport.transportType}</td>
                            <td>{transport.serialNumber}</td>
                            <td>{transport.volume} м³</td>
                            <td>{transport.loadCapacity} кг</td>
                            <td>{transport.averageTransportationSpeed} км/ч</td>
                            <td style={getStatusColor(transport.status)}>{transport.status}</td>
                            <td>{transport.locationCity}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default TransportList;
