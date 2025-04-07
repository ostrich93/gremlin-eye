using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Models
{
    [Table("sessions")]
    public class Session
    {
        [Key]
        [Required]
        [Column("session_id")]
        public string SessionId { get; set; } = string.Empty;

        public int UserId { get; set; } //Requried Foreign Key

        [Column("session_num")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SessionNumber { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        //Navigation Properties
        public User User { get; set; } = null!;
    }
}
