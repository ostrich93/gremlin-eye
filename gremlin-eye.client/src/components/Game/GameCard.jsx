import { Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import classNames from 'classnames';

export const CardContent = ({ game }) => {

    const cardClass = classNames({
        'mx-auto': true,
        'game-cover': true,
        'empty-card': !game || !game?.coverUrl
    });

    const gameTextClass = classNames({
        'game-text-centered': true,
        'unknown-game': !game || !game?.coverUrl
    });

    return (
        <>
            <Card className={cardClass}>
                <Link className="cover-link" to={`/games/${game?.slug}`}></Link>
                <div className="overflow-wrapper">
                    {(!game || !game?.coverUrl) ?
                        <Card.Img className="d-none" /> :
                        <Card.Img className="height" src={game?.coverUrl} alt={game?.name} />
                    }
                    <div className="overlay">
                    </div>
                </div>
                <div className={gameTextClass}>{game?.name ?? ''}</div>
            </Card>
        </>
    );
};

const GameCard = ({ game }) => {
    return (
        <div className="col-cus-5 col-md-cus-6 my-2 px-1 px-md-2">
            <CardContent game={game} />
        </div>
    )
};

export default GameCard;