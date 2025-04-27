import { Route, Outlet } from 'react-router-dom';
import SyncGames from '../pages/SyncGames';
import { useAuth } from "../contexts/AuthContext";

export const AdminRoutes = () => {
    const { user } = useAuth();
    const token = sessionStorage.getItem('GremlinToken');

    if (user && user.role === 1 && token) {
        return <Route path="/sync_games" element={<SyncGames />} />;
    } else {
        return <Outlet />;
    }
};

export default AdminRoutes;