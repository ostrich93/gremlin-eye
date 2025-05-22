import { Box, Button, Divider, Grid, MenuItem, Typography } from '@mui/material';
import KeyboardArrowDropdownIcon from '@mui/icons-material/KeyboardArrowDown';
import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuthState, useAuthDispatch } from "../../contexts/AuthProvider";
import { logout } from '../../actions/authActions';
import NavSearch from './NavSearch';
import { DropdownMenu, NavbarContainer, NavbarContent } from './NavBar.styles';

const NavBar = () => {
    //the search bar will be its own component with its own context and probably reducers and will be placed at the end of the navbar
    const { user } = useAuthState();
    const [anchorEl, setAnchorEl] = useState(null);
    const [menuOpen, setMenuOpen] = useState(false);
    const dispatch = useAuthDispatch();
    const navigate = useNavigate();

    const handleClick = (e) => {
        setAnchorEl(e.currentTarget);
        setMenuOpen(true);
    };

    const handleClose = () => {
        setAnchorEl(null);
        setMenuOpen(false);
    };

    const handleLogOut = async () => {
        await logout(dispatch);
        navigate('/');
    };

    return (
        <NavbarContainer>
            <NavbarContent>
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
                    <Grid container spacing={1} columnSpacing={{ xs: 1, sm: 2, md: 2 }}>
                        {user && user.role === 1 && sessionStorage.getItem('access_token') && (
                            <Grid size={2}>
                                <Link to="/admin">Admin</Link>
                            </Grid>
                        )}
                        {user && (
                            <Grid size={2}>
                                <Button
                                    id="navbar-dropdown-button"
                                    aria-haspopup="true"
                                    variant="contained"
                                    disableElevation
                                    onClick={handleClick}
                                    endIcon={<KeyboardArrowDropdownIcon /> }
                                >
                                    {user.username}
                                </Button>
                                <DropdownMenu
                                    id="navbar-dropdown-menu"
                                    MenuListProps={{
                                        'aria-labelledby': 'navbar-dropdown-button'
                                    }}
                                    anchorEl={anchorEl}
                                    open={menuOpen}
                                    onClose={handleClose}
                                >
                                    <MenuItem component={Link} to={`user/${user.username}`}>Profile</MenuItem>
                                    <Divider sx={{ my: 0 }} />
                                    <MenuItem component={Link} to={`user/${user.username}/games`}>Played</MenuItem>
                                    <MenuItem component={Link} to={`user/${user.username}/playing`}>Playing</MenuItem>
                                    <MenuItem component={Link} to={`user/${user.username}/backlog`}>Backlog</MenuItem>
                                    <MenuItem component={Link} to={`user/${user.username}/wishlist`}>Wishlist</MenuItem>
                                    <Divider sx={{ my: 0 }} />
                                    <MenuItem component={Link} to={`user/${user.username}/journal`}>Journal</MenuItem>
                                    <MenuItem component={Link} to={`user/${user.username}/reviews`}>Reviews</MenuItem>
                                    <MenuItem component={Link} to={`user/${user.username}/lists`}>Lists</MenuItem>
                                    <MenuItem component={Link} to={`user/${user.username}/likes`}>Likes</MenuItem>
                                    <MenuItem component={Link} to={'settings'}>Profile</MenuItem>
                                    <MenuItem component={Button} onClick={handleLogOut} sx={{
                                        border: 0
                                        }}>Log Out</MenuItem>
                                </DropdownMenu>
                            </Grid>
                        )}
                        {!user && (
                            <>
                                <Grid size={2}>
                                    <Link to="/login">Log In</Link>
                                </Grid>
                                <Grid size={2}>
                                    <Link to="/users/register">Register</Link>
                                </Grid>
                            </>
                        )}
                        <Grid size={2}>
                            <Link to="/games/lib">Games</Link>
                        </Grid>
                        <Grid size={6}>
                            <NavSearch />
                        </Grid>
                    </Grid>
                </Box>
            </NavbarContent>
        </NavbarContainer>
    );
};

export default NavBar;