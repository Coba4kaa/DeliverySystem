import React from 'react';
import {useNavigate} from 'react-router-dom';

function CargoList({cargos}) {
    const navigate = useNavigate();

    const handleRowClick = (id) => {
        navigate(`/cargo/${id}`);
    };

    const getStatusColor = (status) => {
        switch (status) {
            case 'Registered':
                return { backgroundColor: 'lightgreen', color: 'green' };
            case 'InProcess':
                return { backgroundColor: 'lightyellow', color: 'orange' };
            case 'Delivered':
                return { backgroundColor: 'lightblue', color: 'blue' };
            default:
                return {};
        }
    };

    return (
        <div className="cargo-list">
            {cargos.length === 0 ? (
                <p>У вас нет грузов.</p>
            ) : (
                <table>
                    <thead>
                    <tr>
                        <th>ID</th>
                        <th>Тип груза</th>
                        <th>Вес</th>
                        <th>Объем</th>
                        <th>Статус</th>
                    </tr>
                    </thead>
                    <tbody>
                    {cargos.map((cargo) => (
                        <tr key={cargo.id}
                            onClick={() => handleRowClick(cargo.id)}
                            className="clickable-row">
                            <td>{cargo.id}</td>
                            <td>{cargo.cargoType}</td>
                            <td>{cargo.weight} кг</td>
                            <td>{cargo.volume} м³</td>
                            <td style={getStatusColor(cargo.status)}>{cargo.status}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default CargoList;
