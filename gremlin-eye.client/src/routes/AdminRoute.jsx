import { Navigate } from 'react-router-dom';
import { useAuthState } from "../contexts/AuthProvider";

export const AdminRoute = ({ children }) => {
    const { user } = useAuthState();
    const token = sessionStorage.getItem('access_token');

    return token && user?.role === 1 ? children : <Navigate to='/login' replace />
};

export default AdminRoute;