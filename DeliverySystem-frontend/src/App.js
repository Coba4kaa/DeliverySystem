import React, { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import axios from 'axios';

import './styles/App.css';
import './styles/Profile.css';
import './styles/Header.css';
import './styles/Orders.css'
import './styles/OrderDetails.css'
import './styles/OrderInfo.css'
import './styles/Auth.css';
import './styles/Home.css'
import './styles/Requests.css'
import './styles/ConfirmationModal.css'

import Auth from './pages/Auth';
import Home from './pages/Home';
import Register from './pages/Register';
import OrderInfo from './pages/OrderInfo';
import Profile from './pages/Profile';
import Requests from './pages/Requests';
import Orders from './pages/Orders';
import CreateOrder from './pages/CreateOrder';
import RegisterTransport from './pages/RegisterTransport';
import RegisterCargo from './pages/RegisterCargo';
import TransportDetails from './pages/TransportDetails';
import CargoDetails from './pages/CargoDetails';
import OrderDetails from './pages/OrderDetails';
import UserInfo from './pages/UserInfo';
import Header from './components/Header';
import Footer from './components/Footer.js'

axios.defaults.baseURL = 'http://localhost:5000';
axios.defaults.withCredentials = true;

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const userId = localStorage.getItem('userId');
        setIsAuthenticated(!!userId);
    }, []);

    return (
        <Router>
            <Header isAuthenticated={isAuthenticated} setIsAuthenticated={setIsAuthenticated} />
            <div className="main-container">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/auth" element={<Auth setIsAuthenticated={setIsAuthenticated} />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/requests" element={<Requests />} />
                    <Route path="/orders" element={<Orders />} />
                    <Route path="/order-info/:id" element={<OrderInfo />} />
                    <Route path="/create-order" element={<CreateOrder />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/register-transport" element={<RegisterTransport />} />
                    <Route path="/register-cargo" element={<RegisterCargo />} />
                    <Route path="/transport/:id" element={<TransportDetails />} />
                    <Route path="/cargo/:id" element={<CargoDetails />} />
                    <Route path="/order/:id" element={<OrderDetails />} />
                    <Route path="/user/:role/:id" element={<UserInfo />} />
                </Routes>
            </div>
            <Footer></Footer>
        </Router>
    );
}

export default App;
