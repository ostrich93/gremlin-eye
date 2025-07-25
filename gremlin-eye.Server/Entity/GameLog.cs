﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gremlin_eye.Server.Enums;

namespace gremlin_eye.Server.Entity
{
    [Table("game_logs")]
    public class GameLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("game_log_id")]
        public long Id { get; set; }

        [Column("game_id")]
        public long GameId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; } //required foreign key

        [Column("play_status")]
        public PlayState? PlayStatus { get; set; }

        [Column("is_played")]
        [DefaultValue(false)]
        public bool IsPlayed { get; set; } = false;

        [Column("is_playing")]
        [DefaultValue(false)]
        public bool IsPlaying { get; set; } = false;

        [Column("is_backlog")]
        [DefaultValue(false)]
        public bool IsBacklog { get; set; } = false;

        [Column("is_wishlist")]
        [DefaultValue(false)]
        public bool IsWishlist { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        public AppUser User { get; set; } = null!;
        public GameData Game { get; set; } = null!;
        public virtual ICollection<Playthrough> Playthroughs { get; set; } = new List<Playthrough>();

    }
}
