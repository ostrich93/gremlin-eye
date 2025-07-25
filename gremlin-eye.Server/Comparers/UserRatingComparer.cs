using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Comparers
{
    //The GameData fed in here is data that has the GameLogs of a particular user
    public class UserRatingComparer : Comparer<GameData>
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

            if (xLog.Playthroughs.Count == 0 && yLog.Playthroughs.Count == 0) return 0;
            if (xLog.Playthroughs.Count > 0 && yLog.Playthroughs.Count == 0) return 1;
            if (xLog.Playthroughs.Count == 0 && yLog.Playthroughs.Count > 0) return -1;

            //getLastPlaythrough in x
            Playthrough lastXthrough = xLog.Playthroughs.First();
            foreach (Playthrough xthrough in xLog.Playthroughs)
            {
                if (lastXthrough != xthrough && xthrough.CreatedAt >= lastXthrough.CreatedAt)
                    lastXthrough = xthrough;
            }

            //get last playthrough in y
            Playthrough lastYthrough = yLog.Playthroughs.First();
            foreach (Playthrough ythrough in yLog.Playthroughs)
            {
                if (lastYthrough != ythrough && ythrough.CreatedAt >= lastYthrough.CreatedAt)
                    lastYthrough = ythrough;
            }

            return lastXthrough.Rating - lastYthrough.Rating;
        }
    }
}
