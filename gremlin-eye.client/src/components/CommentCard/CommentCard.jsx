import { Col, Row } from "react-bootstrap";
import { Link } from "react-router-dom";

const CommentCard = ({ commentData }) => {

    return (
        <Row id={`comment-${commentData.commentId}`} className="comment mb-2">
            <div className="col-auto mb-auto pe-0 comment-avatar">
                <Link to={`/users/${commentData.username}`}>
                    <img src={commentData.avatarUrl && commentData.avatarUrl.length > 0 ? commentData.avatarUrl : "/no_avatar.png"} />
                </Link>
            </div>
            <Col className="my-2 ps-2">
                <Row className="mb-2">
                    <div className="col-auto mt-auto pe-2">
                        <Link className="secondary-link" to={`/users/${commentData.username}`}>
                            <p className="mb-0">{commentData.username}</p>
                        </Link>
                    </div>
                    <div className="col-auto mt-auto ps-0 pe-2">
                        <p className="subtitle-text mb-0">{commentData.createdAt}</p>
                    </div>
                </Row>
                <Row className="comment-body-container">
                    <Col className="comment-body">{commentData.commentBody}</Col>
                </Row>
            </Col>
        </Row>
    );
};

export default CommentCard;