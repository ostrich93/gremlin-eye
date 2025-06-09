import { Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const GameCard = ({game}) => {
    return (
        <div className="col-cus-5 col-md-cus-6 my-2 px-1 px-md-2">
            <Card className="mx-auto game-cover">
                <Link className="cover-link" to={`games/${game?.slug}`}></Link>
                <div className="overflow-wrapper">
                    <Card.Img src={game?.coverUrl} loading='lazy' />
                    <div className="overlay">
                        <div className="game-text-centered">{game?.name}</div>
                    </div>
                </div>
            </Card>
        </div>
  );
}

export default GameCard;