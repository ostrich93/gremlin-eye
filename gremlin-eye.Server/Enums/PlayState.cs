namespace gremlin_eye.Server.Models
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
