import { Box, Button, Divider, Grid, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import styled from '@emotion/styled';
import { Link } from 'react-router-dom';
import { useAuthState } from "../contexts/AuthProvider";
import apiClient from '../config/apiClient';
import Loading from '../components/Loading/Loading';

const GradientDiv = styled.div`
    background: linear-gradient(to bottom, rgba(125,185,232,0) 0, rgba(22,24,28,1) 100%);
    bottom: 0;
    height: 80%;
    left: 0;
    max-height: 350px;
    position: absolute;
    right: 0;
    z-index: 1;
`;

const GamePage = () => {
    const { user } = useAuthState();
    const { slug } = useParams();
    const [gameData, setGameData] = useState(null);
    const [gameStats, setStats] = useState(null);
    const [gameId, setGameId] = useState(-1);
    const [playLog, setPlayLog] = useState(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchGame = async () => {
            setLoading(true);
            apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/${slug}`)
                .then((res) => {
                    setGameData(res.data);
                    setGameId(res.data.id);
                    setStats(res.data.stats ?? {
                        playedCount: 0,
                        playingCount: 0,
                        backlogCount: 0,
                        wishlistCount: 0,
                        averageRating: null,
                        ratingsCount: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
                    });
                    if (user) {
                        setPlayLog(res.data.gameLog);
                    }
                    setLoading(false);
                })
                .catch((err) => {
                    console.error("Error fetching game data: ", err);
                    setLoading(false);
                });
        };

        fetchGame();

    }, [user, slug]);

    return (
        <Grid container sx={
            {
                maxWidth: "1140px",
                marginLeft: "auto",
                marginRight: "auto",
                paddingLeft: "15px",
                paddingRight: "15px",
                width: "100%"
            }
        }>
            <Grid size={12} sx={
                {
                    alignItems: "center",
                    display: "flex",
                    flexDirection: "unset",
                    flexWrap: "wrap",
                    justifyContent: "center",
                    left: 0,
                    margin: 0,
                    maxHeight: "450px",
                    overflow: "hidden",
                    position: "absolute",
                    top: 0,
                    width: "100%",
                    zIndex: -2
                }
            }>
                <GradientDiv />
                <div class="col px-0">
                    {loading && !gameData && (
                        <Loading />
                    )}
                    {!loading && gameData && gameData.bannerUrl && (
                        <img src={gameData.bannerUrl} style={
                            {
                                height: "auto",
                                transform: "translateY(-25%)",
                                width: "100%",
                                verticalAlign: "middle",
                                borderStyle: "none"
                            }
                        } />
                    )}
                </div>
            </Grid>
        </Grid>
    );
};

export default GamePage;