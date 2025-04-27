import AppRoutes from './Routes';
import './App.css';
import NavBar from './components/NavBar/NavBar';

const App = () => {
    return (
        <div>
            <header><NavBar /></header>
            <AppRoutes />
        </div>
    );
}

export default App;