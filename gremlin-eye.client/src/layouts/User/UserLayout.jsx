import { Outlet } from 'react-router-dom';
import { Container, Spinner } from 'react-bootstrap'
import { useQuery } from '@tanstack/react-query';
import { useParams } from "react-router-dom";
import apiClient from "../../config/apiClient";
import UserHeader from "./UserHeader";

const UserLayout = () => {
    const { username } = useParams();

    const { data, isLoading } = useQuery({
        queryKey: ["user", username],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/user/header/${username}`);
                return response.data;
            } catch (error) {
                console.error(error);
                throw new Error(`Failed to get profile: ${username}`);
            }
        },
        cacheTime: 1000 * 60 * 5,
        staleTime: 1000
    });

    if (isLoading) {
        return (
            <div>
                <Spinner animation="border" />
                <p>Loading...</p>
            </div>
        )
    }

    return (
        <Container>
            <UserHeader user={data} />
            <Outlet context={data} />
        </Container>
    );
};

export default UserLayout;