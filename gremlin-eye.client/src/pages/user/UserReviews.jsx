import { Fragment, useCallback, useState } from 'react';
import { Link, useParams, useSearchParams } from 'react-router-dom';
import { Col, Pagination, Row, Spinner } from 'react-bootstrap';
import { keepPreviousData, useQuery } from '@tanstack/react-query';
import apiClient from '../../config/apiClient';
import Sorting from '../../components/Sorting';
import ReviewCard from '../../components/ReviewCard/ReviewCard';

export default function UserReviews() {
    const { username } = useParams();
    const [params, setSearchParams] = useSearchParams();
    const [page, setPage] = useState(parseInt(params.get('page')) || 1);

    const itemsPerPage = 15;

    const orderOptions = [
        {
            type: "recent",
            name: "Recent"
        }
    ];

    const orderBy = params.get('orderBy') || "recent";
    const sortOrder = params.get('sortOrder') || "desc";

    const { data, isLoading } = useQuery({
        retry: true,
        queryKey: ["reviews", username, ...params],
        queryFn: async () => {
            try {
                const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/reviews/users/${username}`, { params });
                console.log(response.data);
                return response.data;
            } catch (error) {
                console.error(error);
                throw new Error("Failed to get reviews");
            }
        },
        placeholderData: keepPreviousData,
        cacheTime: 1000 * 60 * 5,
        staleTime: 1000
    });

    //reused in GameLibrary, should make it a hook or something like that.
    const updateQueryParameters = useCallback((params) => {
        const updatedParams = new URLSearchParams(location.search);
        params.forEach(item => {
            if (item.value == null) {
                updatedParams.delete(item.params);
            } else {
                updatedParams.set(item.params, item.value);
                if (item.params === "page") {
                    setPage(item.value);
                }
            }
        });
        setSearchParams(updatedParams);
    }, [setSearchParams, setPage]);

    return (
        <>
            <Row className="mt-3 user-reviews">
                <div className="col-auto pe-2">
                    <h2 className="like-count-header"><span className="text-white">{data?.totalItems} Reviews</span></h2>
                </div>
                <div className="col-auto my-auto ps-0">
                    <Sorting orderOptions={orderOptions} sortOrder={sortOrder} orderBy={orderBy} update={updateQueryParameters} />
                </div>
            </Row>
            <hr className="mt-1" />
            <Row className="mt-2 user-reviews">
                <Col>
                    {isLoading && (<Spinner animation="border" />)}
                    {data?.items.map((review) => (
                        <Fragment key={review.reviewId}>
                            <Row className="mb-1 game-name">
                                <div className="col-auto pe-1">
                                    <Link to={`/games/${review.gameName}`}><h3 className="mb-0">{review.gameName}</h3></Link>
                                </div>
                                <div className="col-auto mt-auto ps-1">
                                    
                                </div>
                            </Row>
                            <ReviewCard reviewData={review} isUserSubpage={true} />
                        </Fragment>
                    )) }
                </Col>
            </Row>
            <Row className="mx-0">
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
        </>
    );
}