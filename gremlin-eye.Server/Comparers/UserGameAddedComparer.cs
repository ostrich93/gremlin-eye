using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Comparers
{
    public class UserGameAddedComparer : Comparer<GameData>
    {
        public override int Compare(GameData? x, GameData? y)
        {
            if (x == null && y != null) return -1;
            if (x != null && y == null) return 1;
            if (x == null && y == null) return 0;
            if (x == y) return 0;

            if (x.GameLogs.Count == 0 && y.GameLogs.Count == 0) return 0;

            //Compare most recent playthrough in game log. User only has one GameLog per game, so only need to take first in list
            GameLog xLog = x.GameLogs.First();
            GameLog yLog = y.GameLogs.First();

            if (xLog.CreatedAt >= yLog.CreatedAt) return 1;
            if (xLog.CreatedAt == yLog.CreatedAt) return 0;

            return -1;
        }
    }
}
