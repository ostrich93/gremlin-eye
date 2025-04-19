import { AppRoutes } from './Routes';
import './App.css';
import NavBar from './components/NavBar/NavBar';

function App() {
    return (
        <div>
            <header><NavBar /></header>
            <AppRoutes />
        </div>
    );
}

export default App;