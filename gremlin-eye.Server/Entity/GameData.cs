using gremlin_eye.Server.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    /*This Entity is meant to store some basic data from the IGDB database. The basic idea behind it is that when trying to retrieve info about a game, the API first tries hitting the database.
     * If a game with a matching slug OR id cannot be found, then a call is made to IGDB to retrieve that information, which is then converted into a GameData object and saved.
     * This eases the load on IGDB so that we only call on the images for a game after the initial query, increases performance on our end, and makes it easier to define and organize relationships
     */
    [Table("games")]
    public class GameData : IChecksum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("game_id")]
        public long Id { get; set; }

        [Column("cover_uri")]
        public string CoverUrl { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        [Column("summary")]
        public string Summary { get; set; } = string.Empty;

        [Column("banner_uri")]
        public string BannerUrl { get; set; } = string.Empty; //Taken from one of the screenshots of the game in IGDB

        [Column("game_type")]
        [DefaultValue("main_game")]
        public string GameType { get; set; } = "main_game";

        [Column("game_status")]
        [DefaultValue("released")]
        public string GameStatus { get; set; } = "released";

        [Column("release_date")]
        public DateTimeOffset? ReleaseDate { get; set; } //first release date

        [Column("checksum")]
        public string? Checksum { get; set; }
        //Navigation Properties

        //Non-IGDB relationships
        public virtual ICollection<GameLog> GameLogs { get; set; } = new List<GameLog>();
        public virtual ICollection<Playthrough> Playthroughs { get; set; } = new List<Playthrough>();
        public virtual ICollection<ListEntry> ListEntries { get; set; } = new List<ListEntry>();
        public virtual ICollection<GameLike> Likes { get; set; } = new List<GameLike>();

        //IGDB Data Relationships
        public virtual ICollection<CompanyData> Companies { get; set; } = new List<CompanyData>();
        public virtual ICollection<SeriesData> Series { get; set; } = new List<SeriesData>();
        public virtual ICollection<PlatformData> Platforms { get; set; } = new List<PlatformData>();
        public virtual ICollection<GenreData> Genres { get; set; } = new List<GenreData>();
    }

}
