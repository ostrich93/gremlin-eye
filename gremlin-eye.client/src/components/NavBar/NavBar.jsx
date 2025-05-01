import { AppBar, Box, Button, Toolbar, Typography } from '@mui/material';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth, useAuthDispatch } from "../../contexts/AuthContext";
import { logout } from '../../actions/authActions';

const NavBar = () => {
    //the search bar will be its own component with its own context and probably reducers and will be placed at the end of the navbar
    const { user } = useAuth();
    const dispatch = useAuthDispatch();
    const navigate = useNavigate();

    const handleLogOut = async () => {
        await logout(dispatch);
        navigate('/');
    }

    return (
        <>
            <AppBar position="static">
                <Toolbar>
                    <Typography
                        variant="h5" component="a" href="/" sx={{
                            display: 'inline-block',
                            mr: 2,
                            my: 'auto',
                            py: 0,
                            fontWeight: 600,
                            textDecoration: 'none'
                        }}
                    >
                        Gremlin-Eye
                    </Typography>
                    <Box sx={{
                        alignItems: 'center',
                        flexGrow: 1,
                        mt: 0
                    }}>
                        {user && user.role === 1 && sessionStorage.getItem('access_token') && (
                            <Link to="/admin/sync_games">Sync Games</Link>
                        )}
                        {user && (
                            <Button onClick={handleLogOut} sx={{
                                border: 0
                            } }>Log Out</Button>
                        )}
                        {!user && (
                            <>
                                <Link to="/login">Log In</Link>
                                <Link to="/register">Register</Link>
                            </>
                        )}
                        <Link to="/games/lib">Games</Link>
                    </Box>
                </Toolbar>
            </AppBar>
        </>
    );
};

export default NavBar;