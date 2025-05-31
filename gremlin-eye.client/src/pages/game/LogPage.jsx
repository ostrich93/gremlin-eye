import { useParams } from "react-router";
import { useAuthState } from "../../contexts/AuthProvider";
import InteractionSidebar from "../../components/Game/InteractionSidebar";

const LogPage = () => {
    const { user } = useAuthState();
    const { slug } = useParams();

    return (
        <div>
            {!user && (
                <h1>Log In first to see the game log</h1>
            )}
            {user && (
                <InteractionSidebar slug={slug} />
            )}
        </div>
    )
};

export default LogPage;