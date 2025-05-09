import { useState } from 'react';
import apiClient from '../../config/apiClient';

const SyncCompanies = () => {
    const [pageNum, setPageNum] = useState(1);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');

    const importCompanies = async (e) => {
        e.preventDefault();

        if (pageNum < 1) {
            setMessage("Error: The minimum page number is 1. Please set it properly and try again.");
        }

        setLoading(true);
        setMessage('Loading...');
        try {
            await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/import/companies`, {
                params: { page: pageNum }
            });
            setMessage(`Imported company data from IGDB with IDs ${(pageNum - 1) * 500} through ${(pageNum * 500) - 1}`);
        } catch (err) {
            setMessage(`Error importing company data from IGDB: ${err.message}`);
            console.error(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className='container'>
            <h2 id="title">Sync Companies</h2>
            {loading && <h3>Loading...</h3>}
            <form onSubmit={importCompanies}>
                <div className="form-group my-3">
                    <input type='number' min='1' id="pageNum" name="pageNum" onChange={e => setPageNum(e.target.value)} required disabled={loading} />
                    <small className="form-text">Imports and synchronizes company data from IGDB in batches of 500.</small>
                </div>
                <button id="register-button" type='submit' disabled={loading}>Sync Companies</button>
            </form>
            <div>
                {message && <p>{message}</p>}
            </div>
        </div>
    );
};

export default SyncCompanies;