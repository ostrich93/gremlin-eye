using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
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
        public DbSet<GameData> Games { get; set; }
        public DbSet<CompanyData> Companies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<GenreData> Genres { get; set; }
        public DbSet<PlatformData> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Many to Many Relationships
            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Companies)
                .WithMany(c => c.Games);

            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Series)
                .WithMany(s => s.Games);

            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Genres)
                .WithMany(gn => gn.Games);

            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Platforms)
                .WithMany(p => p.Games);

            //One to Many Relationships
            modelBuilder.Entity<GameData>()
                .HasOne(g => g.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(g => g.ParentId)
                .IsRequired(false);

            modelBuilder.Entity<ListEntry>()
                .HasOne(le => le.Game)
                .WithMany(g => g.ListEntries)
                .HasForeignKey(le => le.GameId);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Game)
                .WithMany(g => g.GameLogs)
                .HasForeignKey(gl => gl.GameId);

            modelBuilder.Entity<PlatformData>()
                .HasMany(p => p.Playthroughs)
                .WithOne(play => play.Platform)
                .HasForeignKey(play => play.PlatformId)
                .IsRequired(false);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId);

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

            modelBuilder.Entity<Playthrough>()
                .HasOne(p => p.Game)
                .WithMany(g => g.Playthroughs)
                .HasForeignKey(p => p.GameId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
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
                .HasOne(l => l.User)
                .WithMany(u => u.GameLikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLike>()
                .HasOne(l => l.Game)
                .WithMany(g => g.Likes)
                .HasForeignKey(l => l.GameId);

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

            //One to One Relationships
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Playthrough)
                .WithOne(p => p.Review)
                .HasForeignKey<Review>(r => r.PlaythroughId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLike>()
                .HasOne(l => l.GameLog)
                .WithOne(g => g.Like)
                .HasForeignKey<GameLike>(l => l.GameLogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
