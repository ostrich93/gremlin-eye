import { Routes, Route } from 'react-router-dom';
import GamePage from './pages/game/GamePage';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import AdminRoute from './routes/AdminRoute';
import AdminDashboard from './pages/admin/AdminDashboard';
import SyncCompanies from './pages/admin/SyncCompanies';
import SyncGames from './pages/admin/SyncGames';
import SyncGenres from './pages/admin/SyncGenres';
import SyncPlatforms from './pages/admin/SyncPlatforms';
import SyncSeries from './pages/admin/SyncSeries';
import UserRole from './enums/Role';

const AppRoutes = () => {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/users/register" element={<Register roleType={UserRole.User} />} />
            <Route path="/admin/register" element={<Register roleType={UserRole.Admin} />} />

            <Route path="/admin" element={<AdminRoute ><AdminDashboard /></AdminRoute>} />
            <Route path="admin/syncCompanies" element={<AdminRoute><SyncCompanies /></AdminRoute>} />
            <Route path="admin/syncGames" element={<AdminRoute><SyncGames /></AdminRoute> } />
            <Route path="admin/syncGenres" element={<AdminRoute><SyncGenres /></AdminRoute> } />
            <Route path="admin/syncPlatforms" element={<AdminRoute><SyncPlatforms /></AdminRoute> } />
            <Route path="admin/syncSeries" element={<AdminRoute><SyncSeries /></AdminRoute>} />

            <Route path="/games/:slug" element={<GamePage /> } />
        </Routes>
    );
};

export default AppRoutes;