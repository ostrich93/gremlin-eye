import { useNavigate, Link } from 'react-router-dom';
import { Container, Nav, Navbar, NavDropdown } from 'react-bootstrap';
import { useAuthState, useAuthDispatch } from "../../contexts/AuthProvider";
import { logout } from '../../actions/authActions';
import NavSearch from './NavSearch';
import './NavBar.css';

const NavBar = () => {
    //the search bar will be its own component with its own context and probably reducers and will be placed at the end of the navbar
    const { user } = useAuthState();
    //const [anchorEl, setAnchorEl] = useState(null);
    //const [menuOpen, setMenuOpen] = useState(false);
    const dispatch = useAuthDispatch();
    const navigate = useNavigate();

    /*const handleClick = (e) => {
        setAnchorEl(e.currentTarget);
        setMenuOpen(true);
    };

    const handleClose = () => {
        setAnchorEl(null);
        setMenuOpen(false);
    };*/

    const handleLogOut = async () => {
        await logout(dispatch);
        navigate('/');
    };

    return (
        <Navbar expand="md" id="primary-nav" className="hide-border">
            <Container>
                <Navbar.Brand as={Link} to='/' className="me-2 my-auto py-0">Gremlin-Eye</Navbar.Brand>
                <Navbar.Collapse id="navbarSupportedContent" className="mt-2 mt-md-0">
                    <Nav className="ms-auto">
                        {user && user.role == 1 && sessionStorage.getItem('access_token') && (
                            <Nav.Item>
                                <Nav.Link as={Link} to='/admin'>Admin</Nav.Link>
                            </Nav.Item>
                        )}
                        {user && sessionStorage.getItem('access_token') && (
                            <NavDropdown id="navDropdown" title={user.username} className="d-none d-md-block">
                                <NavDropdown.Item as={Link} to={`user/${user.username}`} className="py-1">Profile</NavDropdown.Item>
                                <NavDropdown.Divider className="my-0" />
                                <NavDropdown.Item as={Link} to={`user/${user.username}/games`} className="py-1">Played</NavDropdown.Item>
                                <NavDropdown.Item as={Link} to={`user/${user.username}/playing`} className="py-1">Playing</NavDropdown.Item>
                                <NavDropdown.Item as={Link} to={`user/${user.username}/backlog`} className="py-1">Backlog</NavDropdown.Item>
                                <NavDropdown.Item as={Link} to={`user/${user.username}/wishlist`} className="py-1">Wishlist</NavDropdown.Item>
                                <NavDropdown.Divider className="my-0" />
                                <NavDropdown.Item as={Link} to={`user/${user.username}/journal`} className="py-1">Journal</NavDropdown.Item>
                                <NavDropdown.Item as={Link} to={`user/${user.username}/reviews`} className="py-1">Reviews</NavDropdown.Item>
                                <NavDropdown.Item as={Link} to={`user/${user.username}/lists`} className="py-1">Lists</NavDropdown.Item>
                                <NavDropdown.Item as={Link} to={`user/${user.username}/likes`} className="py-1">Likes</NavDropdown.Item>
                                <NavDropdown.Divider className="my-0" />
                                <NavDropdown.Item className="pt-1 pb-2" onClick={handleLogOut}>
                                    Logout
                                </NavDropdown.Item>
                            </NavDropdown>
                        )}
                        {(!user || !sessionStorage.getItem('access_token')) && (
                            <>
                                <Nav.Item>
                                    <Nav.Link as={Link} to={`/login`}>Log In</Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link as={Link} to={`users/register`}>Register </Nav.Link>
                                </Nav.Item>
                            </>
                        )}
                        <Nav.Item>
                            <Nav.Link as={Link} to='/games' className="d-none d-md-block me-2">Games</Nav.Link>
                        </Nav.Item>
                        <NavSearch />
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default NavBar;