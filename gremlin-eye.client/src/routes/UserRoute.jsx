import { Navigate } from 'react-router-dom';
import { useAuthState } from "../contexts/AuthProvider";
import UserRole from '../enums/Role';

const UserRoute = ({ children }) => {
    const { user } = useAuthState();
    const token = localStorage.getItem('access_token');

    return token && (user?.role === UserRole.User || user?.role === UserRole.Admin) ? children : <Navigate to='/login' replace />
};

export default UserRoute;