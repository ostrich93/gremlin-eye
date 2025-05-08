import { Navigate } from 'react-router-dom';
import { useAuthState } from "../contexts/AuthProvider";
import UserRole from '../enums/Role';

export const AdminRoute = ({ children }) => {
    const { user } = useAuthState();
    const token = sessionStorage.getItem('access_token');

    return token && user?.role === UserRole.Admin ? children : <Navigate to='/login' replace />
};

export default AdminRoute;