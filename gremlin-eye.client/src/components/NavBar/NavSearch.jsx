import { useState, useRef } from 'react';
import { Autocomplete, ClickAwayListener, Container, IconButton, InputBase } from '@mui/material';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import Suggestion from './Suggestion';
import useDebounce from '../../hooks/useDebounce';
import apiClient from '../../config/apiClient';

const NavSearch = () => {
    const autocompleteRef = useRef(null);
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
        console.log(`cleanString: ${cleanString}`);
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

    //const handleShowResults = () => setResultVisibility(true);
    const handleHideResults = () => setTimeout(() => setResultVisibility(false), 200);

    return (
        <ClickAwayListener onClickAway={() => handleHideResults()} mouseEvent="onMouseUp">
            <Container ref={autocompleteRef}>
                <Autocomplete
                    open={resultsVisibile}
                    freeSolo={searchValue.length > 1}
                    fullWidth
                    options={suggestions}
                    renderInput={({ InputLabelProps, InputProps, ...params }) => {
                        return (
                            <InputBase
                                {...params}
                                onFocus={() => setResultVisibility(searchValue.length > 1)}
                                ref={InputProps.ref}
                                placeholder="Search"
                                value={searchValue}
                                onChange={(e) => setSearchValue(e.target.value)}
                                endAdornment={
                                    <div style={{ display: "inline", margin: "2px 8px" }}>
                                        <IconButton
                                            size="small"
                                            onClick={() => {
                                                if (searchValue > 1) window.location.href = `/search?&q=${searchValue}`;
                                            }}
                                        >
                                            <FontAwesomeIcon icon={faMagnifyingGlass} />
                                        </IconButton>
                                    </div>
                                }
                            />
                        );
                    }}
                    renderOption={(props, option) => <Suggestion suggestion={option} />}
                    getOptionLabel={(option) => (typeof option === "object" ? option.value : "")}
                    />
            </Container>
        </ClickAwayListener>
    );
};

export default NavSearch;