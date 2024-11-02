import React from 'react';
import axios from "axios";
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import OrderForm from './pages/OrderForm';
import OrderList from './pages/OrderList';
import OrderSearch from './pages/OrderSearch';
import OrderDetails from "./pages/OrderDetails";
import './App.css';

axios.defaults.baseURL = 'http://localhost:5000';

function App() {
    return (
        <Router>
            <header>
                <h1>Delivery System</h1>
                <nav>
                    <Link to="/">Создать заказ</Link>
                    <Link to="/orders">Все заказы</Link>
                    <Link to="/search">Поиск заказа по ID</Link>
                </nav>
            </header>
            <div className="main-container">
                <Routes>
                    <Route path="/" element={<OrderForm />} />
                    <Route path="/orders" element={<OrderList />} />
                    <Route path="/orders/:id" element={<OrderDetails />} />
                    <Route path="/search" element={<OrderSearch />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
