import { Dropdown, Row } from 'react-bootstrap';
import { faSortUp, faSortDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { orderMapping } from '../utils/constants';

const Sorting = ({ orderOptions, sortOrder, orderBy, update, className}) => {

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

    //include in fields classnames to append onto nav-interacterables?
    return (
        <div id="nav-interactables" className={className ?? "col-auto"}>
            <Row className="h-100">
                <ul className="nav col-auto">
                    <p className="my-auto subtitle-text sort-heading me-1">Sort by</p>
                    <button id="sort-direction-button" className="btn-link my-auto" onClick={handleOrderUpdate}>
                        <FontAwesomeIcon icon={sortOrder === 'asc' ? faSortUp : faSortDown} />
                    </button>
                    <Dropdown title={orderMapping[orderBy]}>
                        <Dropdown.Toggle className="btn-sort btn-sm py-0 px-1" variant="">{orderMapping[orderBy]}</Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-sort">
                            {orderOptions && (
                                orderOptions.map(option => (
                                    <Dropdown.Item id={option.type} key={option.type} onClick={(e) => handleOptionUpdate(e, option)}>{option.name}</Dropdown.Item>
                                ))
                            ) }
                        </Dropdown.Menu>
                    </Dropdown>
                </ul>
            </Row>
        </div>
    );
};

export default Sorting;