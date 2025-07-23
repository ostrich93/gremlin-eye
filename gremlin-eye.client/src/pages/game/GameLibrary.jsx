import { useCallback, useState } from 'react';
import { Col, Container, Pagination, Row, Spinner } from 'react-bootstrap';
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
    const [params, setSearchParams] = useSearchParams();
    //const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(parseInt(params.get('page')) || 1);
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
        }
    ];

    const currentYear = params.get('releaseYear');
    const currentGenre = params.get('genre');
    const currentPlatform = params.get('platform');
    const min = parseInt(params.get('min')) || 0;
    const max = parseInt(params.get('max')) || 5;
    const orderBy = params.get('orderBy') || 'trending';
    const sortOrder = params.get('sortOrder') || 'desc';

    const { data, isLoading } = useQuery({
        retry: true,
        queryKey: ["games", ...params],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/lib`, {params});
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
                    <button id="sidebar-collapse" className="btn-sort" onClick={handleShow}>Apply Filters</button>
                </div>
                <div className="col-auto pe-0 my-auto" />
                <Sorting orderOptions={orderOptions} sortOrder={sortOrder} orderBy={orderBy} update={updateQueryParameters} />
            </Row>
            <Row className="mx-n1">
                {isLoading && (<Spinner animation="border" />) }
                {data?.items.map((game) => {
                    return (
                    <GameCard key={game.id} id={game.id} game={game} />
                    )})}
            </Row>
            <Row className="mx-0 mt-2">
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
            </Row>
        </Container>
    );
};

export default GameLibrary;