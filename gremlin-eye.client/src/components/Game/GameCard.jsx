import { Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const GameCard = ({ game }) => {
    return (
        <div className="col-cus-5 col-md-cus-6 my-2 px-1 px-md-2">
            <Card className="mx-auto game-cover">
                <Link className="cover-link" to={`/games/${game.slug}`}></Link>
                <div className="overflow-wrapper">
                    <Card.Img className="height" src={game?.coverUrl} />
                    <div className="overlay">
                    </div>
                </div>
                <div className="game-text-centered">{game.name}</div>
            </Card>
        </div>
    );
};

export default GameCard;