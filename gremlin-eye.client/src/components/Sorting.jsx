import { Dropdown, Row } from 'react-bootstrap';
import { faSortUp, faSortDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const Sorting = ({ orderOptions, sortOrder, orderBy, update }) => {

    const getOppositeValue = () => {
        if (sortOrder === 'asc') {
            return 'desc';
        }
        return 'asc';
    };

    const handleOptionUpdate = (e, option) => {
        e.preventDefault();
        update([
            { params: "orderBy", value: option.type },
            { params: "page", value: 1 }
        ]);
    };

    const handleOrderUpdate = (e) => {
        e.preventDefault();
        update([{ params: "sortOrder", value: getOppositeValue() }]);
    };

    return (
        <div id="nav-interactables" className="col-auto">
            <Row className="h-100">
                <ul className="nav col-auto">
                    <p className="my-auto subtitle-text sort-heading me-1">Sort by</p>
                    <button id="sort-direction-button" className="my-auto" onClick={handleOrderUpdate}>
                        <FontAwesomeIcon icon={sortOrder === 'asc' ? faSortUp : faSortDown} />
                    </button>
                    <Dropdown title={orderBy}>
                        <Dropdown.Toggle>{orderBy}</Dropdown.Toggle>
                        <Dropdown.Menu>
                            {orderOptions && (
                                orderOptions.map(option => (
                                    <Dropdown.Item id={option.type} key={option.type} onClick={(e) => handleOptionUpdate(e, option)}>{option.name}</Dropdown.Item>
                                ))
                            ) }
                        </Dropdown.Menu>
                        {/*<Dropdown.Item onClick={() => update([{ params: "orderBy", value: "game_title" }, { params: "page", value: 1}])}>Game Title</Dropdown.Item>
                        <Dropdown.Item onClick={() => update([{ params: "orderBy", value: "trending" }, { params: "page", value: 1 }])}>Trending</Dropdown.Item>
                        <Dropdown.Item onClick={() => update([{ params: "orderBy", value: "release_date" }, { params: "page", value: 1 }])}>Release Date</Dropdown.Item>
                        <Dropdown.Item onClick={() => update([{ params: "orderBy", value: "game_rating" }, { params: "page", value: 1 }])}>Game Rating</Dropdown.Item>*/}
                    </Dropdown>
                </ul>
            </Row>
        </div>
    );
};

export default Sorting;