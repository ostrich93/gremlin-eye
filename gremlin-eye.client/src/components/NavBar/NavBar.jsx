import { useNavigate, Link } from 'react-router-dom';
import { Button, Col, Container, Nav, Navbar, NavDropdown, Row } from 'react-bootstrap';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useAuthState, useAuthDispatch } from "../../contexts/AuthProvider";
import { logout } from '../../actions/authActions';
import NavSearch from './NavSearch';
import './NavBar.css';

const NavBar = () => {
    //the search bar will be its own component with its own context and probably reducers and will be placed at the end of the navbar
    const { user, refreshToken } = useAuthState();
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
        await logout(dispatch, { refreshToken: refreshToken });
        navigate('/');
    };

    return (
        <header>
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
                                    <NavDropdown.Item as={Link} to={`users/${user.username}`} className="py-1">Profile</NavDropdown.Item>
                                    <NavDropdown.Divider className="my-0" />
                                    <NavDropdown.Item as={Link} to={`users/${user.username}/games`} className="py-1">Played</NavDropdown.Item>
                                    <NavDropdown.Item as={Link} to={`users/${user.username}/games?playTypes=playing`} className="py-1">Playing</NavDropdown.Item>
                                    <NavDropdown.Item as={Link} to={`users/${user.username}/games?playTypes=backlog`} className="py-1">Backlog</NavDropdown.Item>
                                    <NavDropdown.Item as={Link} to={`users/${user.username}/games?playTypes=wishlist`} className="py-1">Wishlist</NavDropdown.Item>
                                    <NavDropdown.Divider className="my-0" />
                                    <NavDropdown.Item as={Link} to={`users/${user.username}/reviews`} className="py-1">Reviews</NavDropdown.Item>
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
                                <Nav.Link as={Link} to='/games/lib' className="d-none d-md-block me-2">Games</Nav.Link>
                            </Nav.Item>
                            <NavSearch />
                            {user && (
                                <Row className="mx-0 mb-2 mb-md-0">
                                    <Col className="my-auto px-0 mx-3 mx-md-0">
                                        <Button id="add-a-game" className="btn-main mb-2 my-sm-0 py-0">
                                            <FontAwesomeIcon icon={faPlus} />
                                            Log a Game
                                        </Button>
                                    </Col>
                                </Row>
                            )}
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </header>
    );
};

export default NavBar;