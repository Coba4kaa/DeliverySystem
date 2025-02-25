import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

function Header({ isAuthenticated, setIsAuthenticated }) {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.clear();
        setIsAuthenticated(false);
        navigate('/auth');
    };

    return (
        <header className="header">
            <h1>Delivery System</h1>
            <nav className="nav">
                <NavLink to="/" className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
                    Главная
                </NavLink>
                <NavLink to="/requests" className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
                    Заявки
                </NavLink>
                <NavLink to="/orders" className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
                    Мои заказы
                </NavLink>
                {isAuthenticated ? (
                    <>
                        <NavLink to="/profile" className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
                            Профиль
                        </NavLink>
                        <button onClick={handleLogout} className="logout-button">Выйти</button>
                    </>
                ) : (
                    <NavLink to="/auth" className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
                        Войти
                    </NavLink>
                )}
            </nav>
        </header>
    );
}

export default Header;
