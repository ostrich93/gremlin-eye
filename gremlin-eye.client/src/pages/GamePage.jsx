import { Box, Button, Card, CardMedia, Divider, Grid, Paper, Rating, Typography } from '@mui/material';
import { faBook, faGamepad, faGift, faPlay } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
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

const StyledRating = styled(Rating)({
    '& .MuiRating-iconFilled': {
        color: '#ff6d75',
    },
    '& .MuiRating-iconHover': {
        color: '#ff3d47',
    },
});

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
        <div className="container">
            <Grid id="game-banner-art" container sx={
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
                <Grid size={12}>
                    <GradientDiv />
                    <div className="col px-0">
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
            <Grid id="game-profile" container sx={
                {
                    display: "flex",
                    flexWrap: "wrap",
                    marginLeft: "-15px",
                    marginRight: "-15px",
                    paddingTop: "350px"
                }
            }>
                <Box sx={{
                    flex: '0 0 175px',
                    maxWidth: '175px'

                } }>
                    <Grid sx={{
                        paddingLeft: '15px',
                        paddingRight: '15px',
                        position: 'relative'
                    } }>
                        {loading && !gameData && (
                            <Loading />
                        )}
                        {!loading && gameData && gameData.coverUrl && (
                            <Card sx={
                                {
                                    alignItems: 'center',
                                    backgroundColor: '#30394c',
                                    border: '1px solid #30394c',
                                    borderRadius: '5px',
                                    boxShadow: '0 0 20px 10px rgba(36,40,50,.369)',
                                    marginLeft: 'auto !important',
                                    marginRight: 'auto !important',
                                    marginTop: 'auto !important',
                                    zIndex: '2'
                                }
                            }>
                            
                                <CardMedia
                                    component="img"
                                    image={gameData.coverUrl}
                                />
                            </Card>
                        )}
                    </Grid>
                    <Grid>
                        {!user && (
                            <Paper sx={{
                                textAlign: 'center !important'
                            }}>
                                <Box sx={{
                                    flexBasis: 0,
                                    flexGrow: 1,
                                    maxWidth: '100%'
                                }}>
                                    <p className="mx-auto mb-0" style={{
                                        marginTop: '35px'
                                    }}>
                                        <Link to="/login">Log in</Link> to access rating features
                                    </p>
                                </Box>
                            </Paper>
                        )}
                        {user && sessionStorage.getItem('access_token') && (
                            <Grid container columnSpacing={3} rowSpacing={2}>
                                <Grid size={12}>
                                    <Button
                                        sx={{
                                            backgroundColor: '#ea377a',
                                            color: 'white',
                                            borderStyle: 'none',
                                            display: 'block',
                                            height: '30px',
                                            lineHeight: '0px',
                                            marginTop: '35px',
                                            padding: 0,
                                            width: '100%'
                                        } }
                                    >Edit your Log</Button>
                                </Grid>
                                <Grid id="star-rating-game">
                                    <StyledRating size="large" defaultValue={playLog?.rating ?? 0} precision={0.5} />
                                </Grid>
                                <Grid>
                                    <Divider />
                                </Grid>
                                <Grid size={3}>
                                    <FontAwesomeIcon
                                        icon={faGamepad}
                                    />
                                    <p>Played</p>
                                </Grid>
                                <Grid size={3}>
                                    <FontAwesomeIcon
                                        icon={faPlay}
                                    />
                                    <p>Playing</p>
                                </Grid>
                                <Grid size={3}>
                                    <FontAwesomeIcon
                                        icon={faBook}
                                    />
                                    <p>Backlog</p>
                                </Grid>
                                <Grid size={3}>
                                    <FontAwesomeIcon
                                        icon={faGift}
                                    />
                                    <p>Wishlist</p>
                                </Grid>
                            </Grid>
                        )}
                        <Grid container>
                            <Grid>
                                <p style={
                                    {
                                        textAlign: 'center !important',
                                        marginTop: '1rem !important',
                                        color: 'hsla(0,0%,100%,.6)'
                                    }
                                }>Average Rating</p>
                                <h1>{gameData?.averageRating ?? 'N/A'}</h1>
                            </Grid>

                        </Grid>
                    </Grid>
                </Box>
            </Grid>
        </div>
    );
};

export default GamePage;