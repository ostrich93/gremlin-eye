import { useState } from 'react';
import apiClient from '../../config/apiClient';

const SyncGenres = () => {
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');

    const importGenres = async (e) => {
        e.preventDefault();

        setLoading(true);
        setMessage('Loading...');
        try {
            const res = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/import/genres`);
            if (res.data > -1)
                setMessage('Imported all genre data from IGDB');
            else
                setMessage('No more genre data to import from IGDB');
        } catch (err) {
            setMessage('Error importing genre data from IGDB');
            console.error(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className='container'>
            <h2 id="title">Sync Genres</h2>
            {loading && <h3>Loading...</h3>}
            <form onSubmit={importGenres}>
                <div className="form-group my-3">
                    <small className="form-text">Imports and syncrhonizes all genre data from IGDB.</small>
                </div>
                <button id="register-button" type='submit' className="btn-main" disabled={loading}>Sync Genres</button>
            </form>
            <div>
                {message && <p>{message}</p>}
            </div>
        </div>
    );
};

export default SyncGenres;