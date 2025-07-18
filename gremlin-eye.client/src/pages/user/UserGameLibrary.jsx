import { useCallback, useRef, useState } from "react";
import { Link, useParams, useSearchParams } from "react-router-dom";
import { Button, ButtonGroup, Col, Container, Form, Offcanvas, Pagination, Row, Spinner, ToggleButton, ToggleButtonGroup } from "react-bootstrap";
import { keepPreviousData, useQueries, useQuery } from '@tanstack/react-query';
import Rate from 'rc-rate';
import "rc-rate/assets/index.css";
import { faSquare } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import GameCard from "../../components/Game/GameCard";
import Sorting from "../../components/Sorting";
import apiClient from "../../config/apiClient";

const UserFilterSidebar = ({ show, onHide, update, clear, playTypes, releaseYear, genre, releasePlatform, playedPlatform, playStatus, rating }) => {
    const [currentReleaseDate, setReleaseDate] = useState(releaseYear || null);
    const [currentPlayTypes, setPlayTypes] = useState(playTypes || "played");
    const [currentGenre, setGenre] = useState(genre || null);
    const [currentReleasePlatform, setReleasePlatform] = useState(releasePlatform || null);
    const [currentPlayedPlatform, setPlayedPlatform] = useState(playedPlatform || null);
    const [currentPlayStatus, setPlayStatus] = useState(playStatus || null);
    const [currentRating, setRating] = useState(rating || 0.0);
    const [radioValue, setRadioValue] = useState(0);
    const [checkboxValue, setCheckboxValue] = useState(playTypes?.split(",") || ["played"]);

    const starCountRef = useRef(null);

    const [genresQuery, platformsQuery] = useQueries({
        queries: [
            {
                queryKey: ['genre'],
                queryFn: async () => {
                    try {
                        const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/genre`);
                        return response.data;
                    } catch (error) {
                        console.error(error);
                        //import data
                        throw new Error("Failed to get genres");
                    }
                },
                cacheTime: 1000 * 60 * 5,
                staleTime: 1000 * 60 * 5,
                retry: true
            },
            {
                queryKey: ['platform'],
                queryFn: async () => {
                    try {
                        const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/platform`);
                        return response.data;
                    } catch (error) {
                        console.error(error);
                        //import data
                        throw new Error("Failed to get platforms");
                    }
                },
                cacheTime: 1000 * 60 * 5,
                staleTime: 1000 * 60 * 5,
                retry: true
            }
        ]
    });

    const radios = [
        { name: "Upcoming", value: 1 },
        { name: "Released", value: 2 }
    ];

    const checkboxes = [
        { name: "Played", value: "played"},
        { name: "Playing", value: "playing"},
        { name: "Backlog", value: "backlog"},
        { name: "Wishlist", value: "wishlist"}
    ];

    const handleRating = (rateValue) => {
        if (starCountRef.current) {
            if (!starCountRef.current.state) {
                starCountRef.current.state = {};
            }
            starCountRef.current.state.value = rateValue;
        }
        setRating(2 * rateValue);
    };

    const applyFilters = (e) => {
        e.preventDefault();
        update([
            { params: "playTypes", value: currentPlayTypes },
            { params: "releaseYear", value: currentReleaseDate },
            { params: "genre", value: currentGenre },
            { params: "releasePlatform", value: currentReleasePlatform },
            { params: "playedPlatform", value: currentPlayedPlatform },
            { params: "playStatus", value: currentPlayStatus },
            { params: "rating", value: currentRating },
            { params: "page", value: 1 }
        ]);
    };

    const handleReleaseToggle = (e) => {
        setRadioValue(e.currentTarget.value);
        setReleaseDate(e.currentTarget.name);
    };

    const handlePlayTypesToggle = (val) => {
        setCheckboxValue(val);
        if (val.length == 0) {
            setPlayTypes('played');
        }
        else {
            setPlayTypes(val.join(','));
        }
    };

    return (
        <>
            <Offcanvas show={show} onHide={onHide}>
                <Offcanvas.Header closeButton>
                    <Offcanvas.Title>Filters</Offcanvas.Title>
                </Offcanvas.Header>
                <Offcanvas.Body>
                    <Form>
                        <Form.Group>
                            <Row id="game-status-filters" className="mb-3">
                                <Col>
                                    <ToggleButtonGroup type="checkbox" value={checkboxValue} onChange={handlePlayTypesToggle}>
                                        {checkboxes.map((checkbox, idx) => (
                                            <ToggleButton
                                                key={idx}
                                                id={`filter-type-${checkbox.value}`}
                                                type='checkbox'
                                                name={checkbox.name}
                                                value={checkbox.value}
                                            >
                                                {checkbox.name}
                                            </ToggleButton>
                                        ))}
                                    </ToggleButtonGroup>
                                </Col>
                            </Row>
                        </Form.Group>
                        <Form.Group>
                            <Row>
                                <Col className="filter-header">
                                    <Form.Label>Release Year</Form.Label>
                                </Col>
                            </Row>
                            <ButtonGroup>
                                {radios.map((radio, idx) => (
                                    <ToggleButton
                                        key={idx}
                                        id={`radio-${idx}`}
                                        type='radio'
                                        name={radio.name}
                                        value={radio.value}
                                        checked={radioValue == radio.value}
                                        onChange={handleReleaseToggle}
                                    >
                                        {radio.name}
                                    </ToggleButton>
                                ))}
                            </ButtonGroup>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Genre</Form.Label>
                            <Form.Select onChange={(e) => setGenre(e.currentTarget.value)}>
                                <option></option>
                                {genresQuery.data?.map((gen) => (
                                    <option key={gen.id} id={`genre-${gen.id}`} value={gen.slug}>{gen.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Platform Released On</Form.Label>
                            <Form.Select onChange={(e) => setReleasePlatform(e.currentTarget.value)}>
                                <option></option>
                                {platformsQuery.data?.map((p) => (
                                    <option key={p.id} id={`platform-${p.id}`} value={p.slug}>{p.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Platform Played On</Form.Label>
                            <Form.Select onChange={(e) => setPlayedPlatform(e.currentTarget.value)}>
                                <option></option>
                                {platformsQuery.data?.map((p) => (
                                    <option key={p.id} id={`platform-${p.id}`} value={p.slug}>{p.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Play Status</Form.Label>
                            <Form.Select onChange={(e) => setPlayStatus(e.currentTarget.value)}>
                                <option></option>
                                <option id="completed" value="completed">
                                    <i className="fa-solid fa-square completed" />
                                    <span className="text">Completed</span>
                                </option>
                                <option id="retired" value="retired">
                                    <i className="fa-solid fa-square retired" />
                                    <span className="text">Retired</span>
                                </option>
                                <option id="shelved" value="shelved">
                                    <i className="fa-solid fa-square shelved" />
                                    <span className="text">Shelved</span>
                                </option>
                                <option id="abandoned" value="abandoned">
                                    <i className="fa-solid fa-square abandoned" />
                                    <span className="text">Abandoned</span>
                                </option>
                                <option id="played" value="played">
                                    <i className="fa-solid fa-square played" />
                                    <span className="text">Played</span>
                                </option>
                            </Form.Select>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Rating</Form.Label>
                            <Rate
                                defaultValue={currentRating / 2}
                                value={currentRating / 2}
                                ref={starCountRef}
                                allowHalf
                                allowClear
                                onChange={(value) => handleRating(value)}
                            />
                        </Form.Group>
                        <Row className="mt-4 actions">
                            <Col>
                                <Button id="filters-submit" className="btn-main py-1 mb-2 games-lib" type='submit' onClick={applyFilters}>Update Filters</Button>
                            </Col>
                        </Row>
                        <Row className="actions">
                            <Col>
                                <Button id="filters-clear" className="btn-general btn-small w-100" onClick={clear}>Clear</Button>
                            </Col>
                        </Row>
                    </Form>
                </Offcanvas.Body>
            </Offcanvas>
        </>
    )
};

export default function UserGameLibrary() {
    //type of playStatus is derived from searchParams. If not present, defaults to played
    const { username } = useParams();
    const [params, setSearchParams] = useSearchParams();
    const [page, setPage] = useState(parseInt(params.get('page')) || 1);
    const [showFilters, setShowFilters] = useState(false);

    const itemsPerPage = 40;
    const orderOptions = [
        {
            type: "added",
            name: "When Added"
        },
        {
            type: "last_played",
            name: "Last Played"
        },
        {
            type: "user_rating",
            name: "User Rating"
        },
        {
            type: "game_rating",
            name: "Game Rating"
        },
        {
            type: "game_title",
            name: "Game Title"
        },
        {
            type: "trending",
            name: "Trending"
        },
        {
            type: "release_date",
            name: "Release Date"
        }
    ];

    const currentPlayTypes = params.get('playTypes');
    const currentYear = params.get('releaseYear');
    const currentGenre = params.get('genre');
    const currentReleasePlatform = params.get('releasePlatform');
    const currentPlayedPlatform = params.get('playedPlatform');
    const playStatus = params.get('playStatus');
    const rating = parseInt(params.get('rating')) || 10;
    const orderBy = params.get('orderBy') || 'trending';
    const sortOrder = params.get('sortOrder') || 'desc';

    const { data, isLoading } = useQuery({
        retry: true,
        queryKey: ["games", username, ...params],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/user/${username}`, {
                    params: {
                        releaseYear: params.get('releaseYear'),
                        genre: params.get('genre'),
                        releasePlatform: params.get('releasePlatform'),
                        playedPlatform: params.get('playedPlatform'),
                        playStatus: params.get('playStatus'),
                        playTypes: params.get('playTypes')?.split(',') || ["played"],
                        rating: params.get('rating'),
                        orderBy: params.get('orderBy'),
                        sortOrder: params.get('sortOrder')
                    }
                });
                return response.data;
            } catch (error) {
                console.error(error);
                //fixed data list to import
                throw new Error("Failed to get games");
            }
        },
        placeholderData: keepPreviousData,
        cacheTime: 1000 * 60 * 5,
        staleTime: 1000
    });

    const clearQueryParameters = () => {
        const updatedParams = new URLSearchParams(location.search);
        updatedParams.delete("playTypes");
        updatedParams.delete("releaseYear");
        updatedParams.delete("genre");
        updatedParams.delete("releasePlatform");
        updatedParams.delete("playedPlatform");
        updatedParams.delete("playStatus");
        updatedParams.delete("rating");
        updatedParams.delete("page");
        setSearchParams(updatedParams);
    };

    const updateQueryParameters = useCallback((params) => {
        const updatedParams = new URLSearchParams(location.search);
        params.forEach(item => {
            if (item.value == null) {
                updatedParams.delete(item.params);
            }
            else {
                updatedParams.set(item.params, item.value);
                if (item.params === "page") {
                    setPage(item.value);
                }
            }
        });
        setSearchParams(updatedParams);
    }, [setSearchParams, setPage]);

    const handleShow = () => setShowFilters(true);
    const handleClose = () => setShowFilters(false);

    return (
        <Container>
            <UserFilterSidebar show={showFilters} onHide={handleClose} update={updateQueryParameters} clear={clearQueryParameters} playTypes={currentPlayTypes} releaseYear={currentYear} genre={currentGenre} releasePlatform={currentReleasePlatform} playedPlatform={currentPlayedPlatform} playStatus={playStatus} rating={rating} />
            <Row className="mb-1">
                <div className="col-auto my-auto">
                    <p className="mb-0 subtitle-text">{data?.totalItems} Games</p>
                </div>
                <div id="filter-tags" className="col-auto ms-auto d-none d-md-block">
                    <span className="subtitle-text">Filters:</span>
                </div>
            </Row>

            <Row id="collection-nav">
                <div className="col-auto col-md my-auto mx-auto">
                    <Link className={`fill-in-slim me-1 subtitle-text ${currentPlayTypes?.indexOf("played") !== -1 ? 'filled' : null}`} to={`/users/${username}/games?playTypes=played&orderBy=user_rating&sortBy=desc`}>
                        Played
                    </Link>
                    <Link className={`fill-in-slim me-1 subtitle-text ${currentPlayTypes?.indexOf("playing") !== -1 ? 'filled' : null}`} to={`/users/${username}/games?playTypes=playing&orderBy=added&sortBy=desc`}>
                        Playing
                    </Link>
                    <Link className={`fill-in-slim me-1 subtitle-text ${currentPlayTypes?.indexOf("backlog") !== -1 ? 'filled' : null}`} to={`/users/${username}/games?playTypes=backlog&orderBy=game_title&sortBy=asc`}>
                        Backlog
                    </Link>
                    <Link className={`fill-in-slim me-1 subtitle-text ${currentPlayTypes?.indexOf("wishlist") !== -1 ? 'filled' : null}`} to={`/users/${username}/games?playTypes=wishlist&orderBy=added&sortBy=desc`}>
                        Wishlist
                    </Link>
                </div>
                <div className="col-auto ms-auto pe-0 ps-1">
                    <Button id="sidebar-collapse" className="btn-sort" onClick={handleShow}><span>Apply Filters</span></Button>
                </div>
                <div className="col-auto my-auto pe-0">
                </div>
                <Sorting orderOptions={orderOptions} sortOrder={sortOrder} orderBy={orderBy} update={updateQueryParameters} className="col-auto me-auto me-md-0 pe-1 pe-md-3" />
            </Row>

            <Row id="game-lists" className="my-0 toggle-fade">
                <Col id="user-games-library-container">
                    <Row className="justify-content-center">
                        {isLoading && (<Spinner animation="border" />)}
                        {data?.items.map((game) => (
                            /*<div key={game.id} className="col-cus-4 col-sm-cus-5 col-md-cus-7 col-lg-cus-8 col-xl-cus-10 mt-2 px-1 rating-hover">*/
                            <GameCard key={game.id} game={game} />
                        )) }
                    </Row>
                    <Row className="mx-0 mt-2">
                        {!isLoading && (
                            <Pagination>
                                <Pagination.Prev disabled={page <= 1} onClick={() => updateQueryParameters([{ params: "page", value: page - 1 }])} />
                                {page > 1 && (<Pagination.Item onClick={() => updateQueryParameters([{ params: "page", value: 1 }])}>1</Pagination.Item>)}
                                {page - 3 > 1 && (
                                    <Pagination.Ellipsis />
                                )}
                                {page > 2 && (
                                    Array.from({ length: page - Math.max(page - 3, 2) }, (v, idx) => (
                                        <Pagination.Item key={Math.max(page - 3, 2) + idx} onClick={() => updateQueryParameters([{ params: "page", value: Math.max(page - 3, 2) + idx }])}>{Math.max(page - 3, 2) + idx}</Pagination.Item>
                                    )))
                                }
                                {page < Math.ceil(data?.totalItems / itemsPerPage) && (
                                    Array.from({ length: Math.min(page + 3, Math.ceil(data?.totalItems / itemsPerPage)) - page }, (v, idx) => (
                                        <Pagination.Item key={page + idx + 1} active={idx === 0} onClick={() => updateQueryParameters([{ params: "page", value: page + idx }])}>{page + idx}</Pagination.Item>
                                    )))
                                }
                                {page < Math.ceil(data?.totalItems / itemsPerPage) - 3 && (
                                    <Pagination.Ellipsis />
                                )}
                                <Pagination.Item active={page === Math.ceil(data?.totalItems / itemsPerPage)} onClick={() => updateQueryParameters([{ params: "page", value: Math.ceil(data?.totalItems / itemsPerPage) }])}>{Math.ceil(data?.totalItems / itemsPerPage)}</Pagination.Item>
                                <Pagination.Next disabled={page === Math.ceil(data?.totalItems / itemsPerPage)} onClick={() => updateQueryParameters([{ params: "page", value: page + 1 }])} />
                            </Pagination>
                        )}
                    </Row>
                </Col>
                
            </Row>
        </Container>
    );
};