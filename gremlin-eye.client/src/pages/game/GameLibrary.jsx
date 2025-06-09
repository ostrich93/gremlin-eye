import { useCallback, useState } from 'react';
import { Button, Col, Container, Pagination, Row } from 'react-bootstrap';
import { useSearchParams } from 'react-router-dom';
import { keepPreviousData, useQuery } from '@tanstack/react-query';
//import { faTimesCircle, faSort } from '@fortawesome/free-solid-svg-icons';
//import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import FilterSidebar from '../../components/FilterSidebar';
import GameCard from '../../components/Game/GameCard';
import Sorting from '../../components/Sorting';
import apiClient from '../../config/apiClient';
//import useGetGamesForLibrary from '../../hooks/queries/getGames';

const GameLibrary = () => {
    const [searchParams, setSearchParams] = useSearchParams();
    //const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(searchParams.get('page') || 1);
    const [showFilters, setShowFilters] = useState(false);

    const itemsPerPage = 60;
    const orderOptions = [
        {
            type: "game_title",
            name: "Game Title",
        },
        {
            type: "trending",
            name: "Trending"
        },
        {
            type: "release_date",
            name: "Release Date"
        },
        {
            type: "game_rating",
            name: "Game Rating"
        }
    ];

    const currentYear = searchParams.get('releaseYear');
    const currentGenre = searchParams.get('genre');
    const currentPlatform = searchParams.get('platform');
    const min = parseInt(searchParams.get('min')) || 0;
    const max = searchParams.get('max') || 5;
    const orderBy = searchParams.get('orderBy') || 'trending';
    const sortOrder = searchParams.get('sortOrder') || 'desc';
    
    const { data } = useQuery({
        retry: true,
        queryKey: ["games", "search", page],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/lib`, searchParams);
                return response.data;
            } catch (error) {
                console.error(error);
                //fixed data list to import
                throw new Error("Failed to get games");
            }
        },
        placeholderData: keepPreviousData,
        cacheTime: 1000 * 60 * 5
    });

    const updateQueryParameters = useCallback((params) => {
        const updatedParams = new URLSearchParams(location.search);
        params.forEach(item => {
            updatedParams.set(item.params, item.value)
        });
        setSearchParams(updatedParams);
    }, [setSearchParams]);

    const deleteQueryParameter = (param) => {
        const updatedParams = new URLSearchParams(location.search);
        updatedParams.delete(param);
        updatedParams.delete("page");
        setSearchParams(updatedParams);
    };

    const clearQueryParameters = (param) => {
        const updatedParams = new URLSearchParams(location.search);
        updatedParams.delete("releaseYear");
        updatedParams.delete("genre");
        updatedParams.delete("platform");
        updatedParams.delete("min");
        updatedParams.delete("max");
        updatedParams.delete("page");
        setSearchParams(updatedParams);
    };

    const handleShow = () => setShowFilters(true);
    const handleClose = () => setShowFilters(false);
    
    return (
        <Container>
            <FilterSidebar show={showFilters} onHide={handleClose} update={updateQueryParameters} clear={clearQueryParameters} releaseYear={currentYear} genre={currentGenre} platform={currentPlatform} min={min} max={max} />
            <Row>
                <Col id="filter-tags">
                    <span className="subtitle-text">Filters:</span>

                </Col>
            </Row>

            <Row id="collection-nav">
                <div className="col-12 col-sm-auto my-auto subtitle-text">{data?.totalItems} Games</div>
                <div className="col-auto pe-0 ms-auto">
                    <Button id="sidebar-collapse" onClick={handleShow}>Apply Filters</Button>
                </div>
                <div className="col-auto pe-0 my-auto" />
                <Sorting orderOptions={orderOptions} sortOrder={sortOrder} orderBy={orderBy} update={updateQueryParameters} />
            </Row>
            <Row className="mx-n1">
                {data?.items.map((game) =>{
                    <GameCard key={game.id} id={game.id} game={game} />
                })}
            </Row>
            <Row className="mx-0 mt-2">
                <Pagination>
                    <Pagination.Prev disabled={page <= 1} onClick={() => updateQueryParameters([{ params: "page", value: page - 1 }])} />
                    <Pagination.Item active={page === 1} onClick={() => updateQueryParameters([{ params: "page", value: 1 }])}>1</Pagination.Item>
                    {page - 3 > 1 && (
                        <Pagination.Ellipsis />
                    )}
                    {page > 2 && (
                        Array.from({ length: page - Math.max(page - 3, 2) }, (v, idx) => (
                            <Pagination.Item key={Math.max(page - 3, 2) + idx} active={(Math.max(page - 3, 2) + idx) === page}>{Math.max(page - 3, 2) + idx}</Pagination.Item>
                        )))
                    }
                    {page < Math.ceil(data?.totalItems / itemsPerPage) && (
                        Array.from({ length: Math.min(page + 3, Math.ceil(data?.totalItems / itemsPerPage)) - page }, (v, idx) => (
                            <Pagination.Item key={page + idx} active={idx === 0}>{page + idx}</Pagination.Item>
                        )))
                    }
                    {page < Math.ceil(data?.totalItems / itemsPerPage) - 3 && (
                        <Pagination.Ellipsis />
                    )}
                    <Pagination.Item active={page === Math.ceil(data?.totalItems / itemsPerPage)} onClick={() => updateQueryParameters([{ params: "page", value: Math.ceil(data?.totalItems / itemsPerPage) }])}>{Math.ceil(data?.totalItems / itemsPerPage)}</Pagination.Item>
                    <Pagination.Next disabled={page === Math.ceil(data?.totalItems / itemsPerPage)} onClick={() => updateQueryParameters([{params: "page", value: page + 1}])} />
                </Pagination>
            </Row>
        </Container>
    );
}

export default GameLibrary;