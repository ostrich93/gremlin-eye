namespace gremlin_eye.Server.Entity
{
    //These classes are for the Many-to-Many relationships
    public class GamePlatform
    {
        public long GameId { get; set; }
        public long PlatformId { get; set; }
    }

    public class GameCompany
    {
        public long GameId { get; set; }
        public long CompanyId { get; set; }
    }

    public class GameSeries
    {
        public long GameId { get; set; }
        public long SeriesId { get; set; }
    }

    public class GameGenres
    {
        public long GameId { get; set; }
        public long GenreId { get; set; }
    }
}
