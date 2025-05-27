import { Row } from 'react-bootstrap';
import Rate from 'rc-rate';
import "rc-rate/assets/index.css";
import { useRef, useState } from "react";

const InteractiveRatings = ({ gameLog, onUpdateRating }) => {
    const [rateValue, setRateValue] = useState(gameLog?.rating ?? 0);
    const starCountRef = useRef(null);

    const handleRating = (rate) => {
        setRateValue(rate);
        if (starCountRef.current) {
            if (!starCountRef.current.state) {
                starCountRef.current.state = {};
            }
            starCountRef.current.state.value = rate;
        }
        onUpdateRating('rating', 2 * rateValue);
    };

    return (
        <Row id={`rating${gameLog?.gameId}`} className="my-2 star-rating star-rating-game">
            <Rate
                defaultValue={rateValue}
                value={rateValue}
                ref={starCountRef}
                allowHalf
                allowClear
                onChange={(value) => handleRating(value)}
            />
        </Row>
    );

};

export default InteractiveRatings;