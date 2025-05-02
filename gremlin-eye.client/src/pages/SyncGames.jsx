import { useState } from 'react';
import apiClient from '../config/apiClient';

const SyncGames = () => {
    const [pageNum, setPageNum] = useState(1);
    const [loading, setLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [finishMessage, setFinishMessage] = useState('');

    const importGames = async (e) => {
        try {
            e.preventDefault();

            if (pageNum < 1) {
                throw new Error("Error: The minimum page number is 1. Please set it properly and try again.");
            }

            setLoading(true);
            let response = apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/admin/import`, {
                params: { page: pageNum }
            });

            if (response) {
                setLoading(false);
                setFinishMessage(`Imported Games from IGDB with IDs ${(pageNum - 1) * 500} through ${(pageNum * 500) - 1}`);
                setErrorMessage('');
            }
        } catch (er) {
            setLoading(false);
            setErrorMessage(`Error importing games from IGDB: ${er.message}`);
            console.error(er.message);
        }
    };

    return (
        <div className='container'>
            <h2 id="title">Import Games</h2>
            <form onSubmit={importGames}>
                <div className="form-group my-3">
                    <input type='number' min='1' id="pageNum" name="pageNum" onChange={e => setPageNum(e.target.value)} required disabled={loading} />
                    <small className="form-text">Imports games from IGDB in batches of 500.</small>
                </div>
                <button id="register-button" type='submit' disabled={loading}>Import Games</button>
            </form>
            <div>
                <p>{errorMessage ? errorMessage : finishMessage}</p>
            </div>
        </div>
    );
};

export default SyncGames;