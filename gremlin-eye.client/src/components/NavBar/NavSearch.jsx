import { useState } from 'react';
import { Button, Form, InputGroup, ListGroup } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import useDebounce from '../../hooks/useDebounce';
import apiClient from '../../config/apiClient';
import './NavSearch.css';

const NavSearch = () => {
    const [searchValue, setSearchValue] = useState('');
    const [resultsVisibile, setResultVisibility] = useState(false);
    const [suggestions, setSuggestions] = useState([]);
    const [cache, setCache] = useState({});

    const handleSuggestions = async () => {
        if (cache[searchValue] && resultsVisibile) {
            setSuggestions(cache[searchValue]);
            return;
        }

        const cleanString = encodeURIComponent(searchValue.trim());
        //console.log(`cleanString: ${cleanString}`);
        try {
            setResultVisibility(true);
            const params = new URLSearchParams([['query', cleanString]]);
            const response = await apiClient.get(`${import.meta.env.VITE_APP_BACKEND_URL}/api/games/quick_search`, { params });
            const results = response.data;
            setSuggestions(results);
            setCache((prev) => ({ ...prev, [searchValue]: results }));
        } catch (err) {
            console.error(err);
        }
        
    };

    const handleSearch = () => {
        const cleanedValue = searchValue.trim();
        if (cleanedValue.length > 1) {
            handleSuggestions();
        }
    };

    useDebounce(handleSearch, 300, [searchValue]);

    const handleShowResults = () => setResultVisibility(true);
    const handleHideResults = () => setTimeout(() => setResultVisibility(false), 200);

    return (
        <InputGroup className="search-bar border-right-0">
            <Form.Control
                id="nav-bar-search"
                type="search"
                autoComplete="off"
                onChange={(e) => setSearchValue(e.target.value)}
                onFocus={handleShowResults}
                onBlur={handleHideResults}
                placeholder="Search"
                value={searchValue}
            />
            <Button className="border-left-0 pl-1 pr-2 search-btn" onClick={() => {
                if (searchValue > 1) window.location.href = `/search?&q=${searchValue}`;
            } }>
                <FontAwesomeIcon icon={faMagnifyingGlass} />
            </Button>
            <ListGroup className="autocomplete-suggestions" style={{ position: "absolute", top: "64px" }} >
                {resultsVisibile && searchValue.length > 1 &&
                    suggestions.map((suggestion) => (
                        <ListGroup.Item
                            key={suggestion.id}
                            className="autocomplete-suggestion"
                        >
                            <a href={`/games/${suggestion.slug}`}>
                                {suggestion.value} ({suggestion.year ?? 'TBA'})
                            </a>
                        </ListGroup.Item>
                    ))
                }
            </ListGroup>
        </InputGroup>
    );
};

export default NavSearch;