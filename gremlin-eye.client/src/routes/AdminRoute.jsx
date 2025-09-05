import { Navigate } from 'react-router-dom';
import { useAuthState } from "../contexts/AuthProvider";
import UserRole from '../enums/Role';

const AdminRoute = ({ children }) => {
    const { user } = useAuthState();
    const token = localStorage.getItem('access_token');

    return token && user?.role === UserRole.Admin ? children : <Navigate to='/login' replace />
};

export default AdminRoute;