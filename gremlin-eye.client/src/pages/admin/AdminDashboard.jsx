import { useNavigate } from 'react-router-dom';
import { Box, Button, Grid } from '@mui/material';
import styled from '@emotion/styled';

const Item = styled(Button)({
    backgroundColor: '#fff',
    textAlign: 'center',
});

const AdminDashboard = () => {
    const navigate = useNavigate();
    const onItemClicked = (syncLinkText) => {
        navigate(`/admin/sync${syncLinkText}`);
    }

    return (
        <div id="welcoming-banner">
            <div>
                <h2>Welcome Admins!</h2>
            </div>
            <div id="home-about">
                <p>As an admin, you can synchronize Gremlin-Eye's database with IGDB's through the links listed on this page.</p>
                <p>It is highly recommended that before synchronizing the games, you sychronize the platform, company, series, and genre data.</p>
            </div>
            <Box sx={{flexGrow: 1} }>
                <Grid container rowSpacing={1} columnSpacing={1} >
                    <Grid size={6}>
                        <Item onClick={() => onItemClicked("Companies")}>Sync Companies</Item>
                    </Grid>
                    <Grid size={6}>
                        <Item onClick={() => onItemClicked("Genres")}>Sync Genres</Item>
                    </Grid>
                    <Grid size={6}>
                        <Item onClick={() => onItemClicked("Platforms")}>Sync Platforms</Item>
                    </Grid>
                    <Grid size={6}>
                        <Item onClick={() => onItemClicked("Series")}>Sync Series</Item>
                    </Grid>
                    <Grid size={12}>
                        <Item onClick={() => onItemClicked("Games")}>Sync Games</Item>
                    </Grid>
                </Grid>
            </Box>
        </div>
    );
};

export default AdminDashboard;