namespace gremlin_eye.Server.Repositories
{
    public interface ILikeRepository
    {
        int GetGameLikeCount(long gameId);
    }
}
