import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
//import { useAuth } from './contexts/AuthContext';
import AdminRoutes from './routes/AdminRoutes';

const AppRoutes = () => {
    //user will be useful for the User protected routes once they're implemented
    //const { user } = useAuth();

    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/users/register" element={<Register roleType={0} />} />
            <Route path="/admin/register" element={<Register roleType={1} />} />

            <AdminRoutes />
        </Routes>
    );
};

export default AppRoutes;