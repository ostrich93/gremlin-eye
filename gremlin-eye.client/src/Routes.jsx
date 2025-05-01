import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import AdminRoute from './routes/AdminRoute';
import SyncGames from './pages/SyncGames';

const AppRoutes = () => {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/users/register" element={<Register roleType={0} />} />
            <Route path="/admin/register" element={<Register roleType={1} />} />

            <Route path="admin/syncGames" element={<AdminRoute><SyncGames /></AdminRoute> } />
        </Routes>
    );
};

export default AppRoutes;