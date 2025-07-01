import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import { QueryClientProvider } from '@tanstack/react-query';
import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css';
import App from './App';
import AuthProvider from './contexts/AuthProvider';
import queryClient from './config/queryClient';
import JournalProvider from './contexts/JournalProvider';
import ReactModal from 'react-modal';

ReactModal.setAppElement('#root');

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <QueryClientProvider client={queryClient}>
            <AuthProvider>
                <JournalProvider>
                    <BrowserRouter>
                        <App />
                    </BrowserRouter>
                </JournalProvider>
            </AuthProvider>
        </QueryClientProvider>
    </StrictMode>,
);