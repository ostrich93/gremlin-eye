using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Models
{
    public class ListEntry
    {

        [Key]
        [Column("entry_id")]
        public int EntryId { get; set; }

        [Key]
        [Column("game_id")]
        public int GameId { get; set; } //We want to be able to search for lists that have a specific game in them.

        [Column("listing_id")]
        public int ListingId { get; set; }

        [Column("entry_note")]
        public string? EntryNote { get; set; }

        public Listing Listing { get; set; } = null!;
    }
}
