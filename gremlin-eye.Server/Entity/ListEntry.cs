using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    public class ListEntry
    {

        [Key]
        [Column("entry_id")]
        public long Id { get; set; }

        [Column("game_id")]
        public long GameId { get; set; } //We want to be able to search for lists that have a specific game in them.

        [Column("listing_id")]
        public long ListingId { get; set; }

        [Column("entry_note")]
        public string? EntryNote { get; set; }

        public Listing Listing { get; set; } = null!;
        public GameData Game { get; set; } = null!;
    }
}
