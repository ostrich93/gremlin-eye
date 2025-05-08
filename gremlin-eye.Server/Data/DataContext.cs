using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
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
        public DbSet<SeriesData> Series { get; set; }
        public DbSet<GenreData> Genres { get; set; }
        public DbSet<PlatformData> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Many to Many Relationships
            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Companies)
                .WithMany(c => c.Games)
                .UsingEntity("GameCompany",
                    l => l.HasOne(typeof(CompanyData)).WithMany().HasForeignKey("CompanyId").HasPrincipalKey(nameof(CompanyData.Id)),
                    r => r.HasOne(typeof(GameData)).WithMany().HasForeignKey("GameId").HasPrincipalKey(nameof(GameData.Id)),
                    j => j.HasKey("GameId", "CompanyId"));

            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Series)
                .WithMany(s => s.Games)
                .UsingEntity("GameSeries",
                    l => l.HasOne(typeof(SeriesData)).WithMany().HasForeignKey("SeriesId").HasPrincipalKey(nameof(SeriesData.Id)),
                    r => r.HasOne(typeof(GameData)).WithMany().HasForeignKey("GameId").HasPrincipalKey(nameof(GameData.Id)),
                    j => j.HasKey("GameId", "SeriesId"));

            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Genres)
                .WithMany(gn => gn.Games)
                .UsingEntity("GameGenres",
                    l => l.HasOne(typeof(GenreData)).WithMany().HasForeignKey("GenreId").HasPrincipalKey(nameof(GenreData.Id)),
                    r => r.HasOne(typeof(GameData)).WithMany().HasForeignKey("GameId").HasPrincipalKey(nameof(GameData.Id)),
                    j => j.HasKey("GameId", "GenreId"));

            modelBuilder.Entity<GameData>()
                .HasMany(g => g.Platforms)
                .WithMany(p => p.Games)
                .UsingEntity("GamePlatforms",
                    l => l.HasOne(typeof(PlatformData)).WithMany().HasForeignKey("PlatformId").HasPrincipalKey(nameof(PlatformData.Id)),
                    r => r.HasOne(typeof(GameData)).WithMany().HasForeignKey("GameId").HasPrincipalKey(nameof(GameData.Id)),
                    j => j.HasKey("GameId", "PlatformId"));

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
                .HasForeignKey(gl => gl.GameId)
                .OnDelete(DeleteBehavior.ClientCascade);

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
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ReviewComment>()
                .HasOne(rc => rc.Review)
                .WithMany(r => r.Comments)
                .HasForeignKey(r => r.ReviewId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewComment>()
                .HasOne(rc => rc.Author)
                .WithMany(a => a.ReviewComments)
                .HasForeignKey(r => r.AuthorId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewLike>()
                .HasOne(rl => rl.Review)
                .WithMany(r => r.Likes)
                .HasForeignKey(rl => rl.ReviewId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewLike>()
                .HasOne(rl => rl.User)
                .WithMany(u => u.ReviewLikes)
                .HasForeignKey(rl => rl.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLike>()
                .HasOne(l => l.User)
                .WithMany(u => u.GameLikes)
                .HasForeignKey(l => l.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLike>()
                .HasOne(l => l.Game)
                .WithMany(g => g.Likes)
                .HasForeignKey(l => l.GameId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.User)
                .WithMany(u => u.GameLogs)
                .HasForeignKey(gl => gl.UserId)
                .IsRequired(true);

            modelBuilder.Entity<ListEntry>()
                .HasOne(le => le.Listing)
                .WithMany(l => l.ListEntries)
                .HasForeignKey(le => le.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListingComment>()
                .HasOne(lc => lc.Listing)
                .WithMany(l => l.Comments)
                .HasForeignKey(lc => lc.ListingId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ListingComment>()
                .HasOne(lc => lc.Author)
                .WithMany(a => a.ListingComments)
                .HasForeignKey(lc => lc.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListingLike>()
                .HasOne(ll => ll.Listing)
                .WithMany(l => l.Likes)
                .HasForeignKey(ll => ll.ListingId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ListingLike>()
                .HasOne(l => l.User)
                .WithMany(l => l.ListingLikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Listing>()
                .HasOne(l => l.User)
                .WithMany(u => u.Listings)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            //One to One Relationships
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Playthrough)
                .WithOne(p => p.Review)
                .HasForeignKey<Review>(r => r.PlaythroughId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
