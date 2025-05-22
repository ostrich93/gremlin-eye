const Home = () => {
    return (
        <div className="container">
            <div id="welcoming-banner">
                <div>
                    <h2>Discover, collect, and analyze games</h2>
                </div>
                <div id="home-about">
                    <h2>What is Gremlin-Eye?</h2>
                    <p>Gremlin-Eye is a site that allows users to virtually manage their game collections. You can update your backlog, write reviews, rate games,
                    record your play sessions, and put games into your wishlist.</p>
                </div>
                <div id="feature-about">
                    <h2>Track your game collections</h2>
                    <p>You can log the games you have played, are currently playing, and wish to play. You can record your platforms of choice, time spent, etc. in
                   as fine-grained detail as you want.</p>
                </div>
                <div id="review-about">
                    <h2>Share your opinions with reviews</h2>
                    <p>You can rate games and write reviews if you wish. Your score is tracked and taken into account when determining the average rating of the game.</p>
                </div>
                <div id="lists-about">
                    <h2>Organize your games with lists</h2>
                    <p>Whether its ranked lists for your favorite series, or grouping what games you aim to play this year, you can make lists of game titles with notes.</p>
                </div>
            </div>
        </div>
    );
};

export default Home;