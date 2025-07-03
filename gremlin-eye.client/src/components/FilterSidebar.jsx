import { useState } from 'react';
import { useQueries } from '@tanstack/react-query';
import { Button, ButtonGroup, Col, Form, Offcanvas, Row, ToggleButton } from 'react-bootstrap';
import Nouislider from 'nouislider-react';
import "nouislider-react/node_modules/nouislider/distribute/nouislider.css";
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css";
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import apiClient from '../config/apiClient';

export default function FilterSidebar({ show, onHide, update, clear, releaseYear, genre, platform, min, max }) {
    const [currentReleaseDate, setReleaseDate] = useState(releaseYear || null);
    const [currentGenre, setGenre] = useState(genre || null);
    const [ratingMin, setRatingMin] = useState(min || 0.0);
    const [ratingMax, setRatingMax] = useState(max || 5.0);
    //const [category, selectCategory] = useState(null);
    const [currentPlatform, setPlatform] = useState(platform || null);
    const [radioValue, setRadioValue] = useState(0);

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

    const applyFilters = (e) => {
        e.preventDefault();
        update([
            { params: "releaseYear", value: currentReleaseDate },
            { params: "genre", value: currentGenre },
            { params: "platform", value: currentPlatform },
            { params: "min", value: ratingMin },
            { params: "max", value: ratingMax },
            { params: "page", value: 1 }
        ]);
    }

    const handleReleaseToggle = (e) => {
        setRadioValue(e.currentTarget.value);
        setReleaseDate(e.currentTarget.name);
    };

    const handleUpdate = (value) => {
        setRatingMin(value[0]);
        setRatingMax(value[1]);
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
                                )) }
                            </ButtonGroup>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Genre</Form.Label>
                            <Form.Select onChange={(e) => setGenre(e.currentTarget.value) }>
                                <option></option>
                                {genresQuery.data?.map((gen) => (
                                    <option key={gen.id} id={`genre-${gen.id}`} value={gen.slug}>{gen.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Platform</Form.Label>
                            <Form.Select onChange={(e) => setPlatform(e.currentTarget.value)}>
                                <option></option>
                                {platformsQuery.data?.map((p) => (
                                    <option key={p.id} id={`platform-${p.id}`} value={p.slug}>{p.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Row className="mt-3">
                            <Col className="filter-header">
                                <p>Game Rating</p>
                            </Col>
                        </Row>
                        <Row>
                            <Col>
                                <Nouislider
                                    range={{ min: 0, max: 5 }}
                                    start={[min, max]}
                                    step={0.1}
                                    connect
                                    onSlide={handleUpdate}
                                    id="rating-range-slider"
                                    className="gremlin-eye-slider"
                                />
                                <p className="slider-label">
                                    <FontAwesomeIcon icon={faStar} />
                                    <span>{`${ratingMin} - ${ratingMax}`}</span>
                                </p>
                            </Col>
                        </Row>
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
    );
}