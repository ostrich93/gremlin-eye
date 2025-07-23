import { useState } from 'react';
import apiClient from '../../config/apiClient';

const SyncPlatforms = () => {

    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');

    const importPlatforms = async (e) => {
        e.preventDefault();

        setLoading(true);
        setMessage('Loading...');
        try {
            const res = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/import/platforms`);
            if (res.data > -1)
                setMessage('Imported all platform data from IGDB');
            else
                setMessage('No more platform data to import from IGDB');
        } catch (err) {
            setMessage('Error importing platform data from IGDB');
            console.error(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className='container'>
            <h2 id="title">Sync Platforms</h2>
            {loading && <h3>Loading...</h3>}
            <form onSubmit={importPlatforms}>
                <div className="form-group my-3">
                    <small className="form-text">Imports and syncrhonizes all platform data from IGDB.</small>
                </div>
                <button id="register-button" type='submit' className="btn-main" disabled={loading}>Sync Platforms</button>
            </form>
            <div>
                {message && <p>{message}</p>}
            </div>
        </div>
    );
};

export default SyncPlatforms;