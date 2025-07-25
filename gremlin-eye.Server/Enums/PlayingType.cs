namespace gremlin_eye.Server.Enums
{
    public enum PlayingType
    {
        Played,
        Playing,
        Backlog,
        Wishlist
    }

    public static class PlayingTypeExtensions
    {
        public static string ToStringValue(this PlayingType playingType)
        {
            return playingType.ToString().ToLower();
        }
    }
}
