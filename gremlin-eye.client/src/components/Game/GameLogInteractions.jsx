import { Button, Col, Row } from 'react-bootstrap';
import { faBook, faGamepad, faGift, faPlay } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useState } from 'react';

const playStateColors = ['#ea377a', 'green', 'blue', 'orange', 'red'];
const defaultPlayedStateColor = 'gray';

const GameLogInteractions = memo(({ gameLog, onPlayedUpdate, onOtherUpdate }) => {
    const [playState, setPlayState] = useState(gameLog?.playState ?? null);
    const [played, setPlayed] = useState(gameLog?.isPlayed ?? false);
    const [playing, setPlaying] = useState(gameLog?.isPlaying ?? false);
    const [backlog, setBacklog] = useState(gameLog?.isBacklog ?? false);
    const [wishlist, setWishlist] = useState(gameLog?.isWishlist ?? false);

    /*useEffect(() => {
        if (gameLog != null) {
            setPlayState(null);

        }
    },[gameLog]);*/

    const handlePlayed = () => {
        //console.log(played);
        //e.preventDefault();
        if (played) {
            setPlayState(0);
        }
        else {
            setPlayState(null);
        }
        const playedToggle = !played;
        setPlayed(playedToggle);
        onPlayedUpdate(playedToggle);
    }

    const handlePlaying = () => {
        //e.preventDefault();
        console.log(typeof gameLog);
        const playToggle = !playing;
        setPlaying(playToggle);
        onOtherUpdate('isPlaying', playToggle);
    };

    const handleBacklog = () => {
        //e.preventDefault();
        const backlogToggle = !backlog;
        setBacklog(backlogToggle);
        onOtherUpdate('isBacklog', backlogToggle);
    }

    const handleWishlist = () => {
        //e.preventDefault();
        const wishlistToggle = !wishlist;
        setWishlist(wishlistToggle);
        onOtherUpdate('isWishlist', wishlistToggle);
    }

    return (
        <Row id="buttons" className="mx-0">
            <Col id="play" className="px-0 play-btn-container mt-auto">
                <Button variant="link" className="mx-auto" onClick={handlePlayed}>
                    <FontAwesomeIcon icon={faGamepad} size="2x" color={(gameLog && played && playState) ? playStateColors[playState] : defaultPlayedStateColor} />
                    <br />
                    <p className="label">Played</p>
                </Button>
            </Col>
            <Col id="playing" className="px-0 playing-btn-container mt-auto">
                <Button variant="link" className="mx-auto" onClick={handlePlaying}>
                    <FontAwesomeIcon icon={faPlay} size="2x" color={(gameLog && playing) ? '#ea377a' : defaultPlayedStateColor} />
                    <br />
                    <p className="label">Playing</p>
                </Button>
            </Col>
            <Col id="backlog" className="px-0 backlog-btn-container mt-auto">
                <Button variant="link" className="mx-auto" onClick={handleBacklog}>
                    <FontAwesomeIcon icon={faBook} size="2x" color={(gameLog && backlog) ? '#ea377a' : defaultPlayedStateColor} />
                    <br />
                    <p className="label">Backlog</p>
                </Button>
            </Col>
            <Col id="wishlist" className="px-0 wishlist-btn-container mt-auto">
                <Button variant="link" className="mx-auto" onClick={handleWishlist}>
                    <FontAwesomeIcon icon={faGift} size="2x" color={(gameLog && wishlist) ? '#ea377a' : defaultPlayedStateColor} />
                    <br />
                    <p className="label">Wishlist</p>
                </Button>
            </Col>
        </Row>
    );
});

export default GameLogInteractions;