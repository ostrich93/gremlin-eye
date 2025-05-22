namespace gremlin_eye.Server.Enums
{
    public enum PlayMedium
    {
        Owned,
        Subscription,
        Borrowed,
        Watched
    }

    public static class PlayMediumExtensions
    {
        public static string ToStringValue(this PlayMedium medium)
        {
            return medium.ToString().ToLower();
        }
    }
}
