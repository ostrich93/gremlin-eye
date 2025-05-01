import { Navigate } from 'react-router-dom';
import { useAuth } from "../contexts/AuthContext";

export const AdminRoute = ({ children }) => {
    const { user } = useAuth();
    const token = sessionStorage.getItem('access_token');

    return token && user?.role === 1 ? children : <Navigate to='/login' replace />
};

export default AdminRoute;