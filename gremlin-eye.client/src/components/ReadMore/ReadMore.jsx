import { useState } from 'react';
import { Col } from 'react-bootstrap';
import { faArrowTurnDown, faArrowTurnUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import './ReadMore.css';

export default function ReadMore ({ contentElId, children }) {
    const text = children;
    const [isReadMore, setReadMore] = useState(true);
    const [currStyle, setCurrStyle] = useState({
        height: '150px',
        maxHeight: 'none',
        transition: 'height 200ms',
        overflow: 'hidden'
    });

    const toggleReadMore = () => {
        setReadMore(!isReadMore);
        setCurrStyle({
            ...currStyle,
            height: !isReadMore === false ? 'auto' : '150px'
        });
    };

    if (text.length <= 900) {
        return (
            <>
                <p id={contentElId} className="readmore-content" style={{ height: 'auto', maxHeight: 'none' }}>{text}</p>
            </>
        );
    }

    return (
        <>
            <Col className="readmore-container">
                <p id={contentElId} className="readmore-content" style={currStyle} aria-expanded={!isReadMore}>
                    {text}
                </p>
                <a onClick={toggleReadMore} aria-controls={contentElId} style={{ color: 'white', cursor: 'pointer' }}>
                    {isReadMore && (
                        <>
                            <FontAwesomeIcon icon={faArrowTurnDown} /> Expand
                        </>
                    )}
                    {!isReadMore && (
                        <>
                            <FontAwesomeIcon icon={faArrowTurnUp} /> Close
                        </>
                    ) }
                </a>
                <div className="readmore-background gradient" />
            </Col>
        </>
    )
};