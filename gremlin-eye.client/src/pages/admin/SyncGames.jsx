import { useState } from 'react';
import apiClient from '../../config/apiClient';

const SyncGames = () => {
    const [pageNum, setPageNum] = useState(1);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [lastStart, setLastStart] = useState(0);
    const [lastPage, setLastPage] = useState(1);

    const importGames = async (e) => {
        e.preventDefault();
        if (pageNum < 1) {
            setMessage("Error: The minimum page number is 1. Please set it properly and try again.");
        }

        setLoading(true);
        setMessage('Loading...');
        try {
            const res = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/import/games`, {
                params: { page: pageNum }
            });
            if (res.data == -1) {
                setMessage(`Games with IDs in range of ${(pageNum - 1) * 500} through ${(pageNum * 500) - 1} have already been imported.`);
                setLastStart(0);
                setLastPage(1);
            }
            else if (res.data > -1) {
                if (pageNum - lastPage === 1) {
                    setMessage(`Imported games from IGDB with IDs ${lastStart} through ${res.data}`);
                    setLastStart(res.data + 1);
                    setLastPage(pageNum);
                }
                else {
                    setMessage(`Imported games from IGDB with IDs ${(pageNum - 1) * 500} through ${res.data}`);
                    setLastStart(res.data + 1);
                    setLastPage(pageNum);
                }
            }
            else {
                setMessage(`Imported Games from IGDB with IDs ${(pageNum - 1) * 500} through ${(pageNum * 500) - 1}`);
            }
        } catch (err) {
            setMessage(`Error importing games from IGDB: ${err.message}`);
            setLastPage(1);
            setLastStart(0);
            console.error(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className='container'>
            <h2 id="title">Sync Games</h2>
            {loading && <h3>Loading...</h3> }
            <form onSubmit={importGames}>
                <div className="form-group my-3">
                    <input type='number' min='1' id="pageNum" name="pageNum" onChange={e => setPageNum(e.target.value)} required disabled={loading} />
                    <small className="form-text">Imports and synchronizes games from IGDB in batches of 500.</small>
                </div>
                <button id="register-button" type='submit' className="btn-main" disabled={loading}>Sync Games</button>
            </form>
            <div>
                {message && <p>{message}</p> }
            </div>
        </div>
    );
};

export default SyncGames;