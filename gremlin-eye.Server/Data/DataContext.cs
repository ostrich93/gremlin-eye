using gremlin_eye.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<GameLog> GameLogs { get; set; }
        public DbSet<Playthrough> Playthroughs { get; set; }
        public DbSet<PlayLog> PlayLogs { get; set; }
        public DbSet<GameLike> GameLikes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewLike> ReviewLikes { get; set; }
        public DbSet<ReviewComment> ReviewComments { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListEntry> ListEntries { get; set; }
        public DbSet<ListingComment> ListingComments { get; set; }
        public DbSet<ListingLike> ListingLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayLog>()
                .HasOne(plg => plg.Playthrough)
                .WithMany(ply => ply.PlayLogs)
                .HasForeignKey(plg => plg.PlaythroughId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Playthrough>()
                .HasOne(p => p.GameLog)
                .WithMany(g => g.Playthroughs)
                .HasForeignKey(p => p.GameLogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Playthrough)
                .WithOne(p => p.Review)
                .HasForeignKey<Review>(r => r.PlaythroughId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewComment>()
                .HasOne(rc => rc.Review)
                .WithMany(r => r.Comments)
                .HasForeignKey(r => r.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewComment>()
                .HasOne(rc => rc.Author)
                .WithMany(a => a.ReviewComments)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewLike>()
                .HasOne(rl => rl.Review)
                .WithMany(r => r.Likes)
                .HasForeignKey(rl => rl.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewLike>()
                .HasOne(rl => rl.User)
                .WithMany(u => u.ReviewLikes)
                .HasForeignKey(rl => rl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLike>()
                .HasOne(l => l.GameLog)
                .WithOne(g => g.Like)
                .HasForeignKey<GameLike>(l => l.GameLogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLike>()
                .HasOne(l => l.User)
                .WithMany(u => u.GameLikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.User)
                .WithMany(u => u.GameLogs)
                .HasForeignKey(gl => gl.UserId);

            modelBuilder.Entity<ListEntry>()
                .HasOne(le => le.Listing)
                .WithMany(l => l.ListEntries)
                .HasForeignKey(le => le.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListingComment>()
                .HasOne(lc => lc.Listing)
                .WithMany(l => l.Comments)
                .HasForeignKey(lc => lc.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListingComment>()
                .HasOne(lc => lc.Author)
                .WithMany(a => a.ListingComments)
                .HasForeignKey(lc => lc.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListingLike>()
                .HasOne(ll => ll.Listing)
                .WithMany(l => l.Likes)
                .HasForeignKey(ll => ll.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListingLike>()
                .HasOne(l => l.User)
                .WithMany(l => l.ListingLikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Listing>()
                .HasOne(l => l.User)
                .WithMany(u => u.Listings)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
