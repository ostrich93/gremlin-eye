import "./NotFound.css";

const NotFoundPage = () => {
    return (
        <div className="dialog">
            <div>
                <h1>404 Error</h1>
                <p>The page you are looking for does not exist.</p>
            </div>
            <p>
                Return back to <a href="/">Gremlin-Eye</a>
            </p>
        </div>
    );
};

export default NotFoundPage;