import AppRoutes from './Routes';
import './App.css';
import NavBar from './components/NavBar/NavBar';

const App = () => {
    return (
        <>
            <NavBar />
            <AppRoutes />
        </>
    );
}

export default App;