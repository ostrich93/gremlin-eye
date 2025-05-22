namespace gremlin_eye.Server.Enums
{
    public enum PlayState
    {
        Played,
        Completed,
        Retired,
        Shelved,
        Abandoned
    }

    public static class PlayStateExtensions
    {
        public static string ToStringValue(this PlayState playState)
        {
            return playState.ToString().ToLower();
        }
    }
}
