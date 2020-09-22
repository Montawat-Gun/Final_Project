using Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<GameGenre> GamesGenres { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Interest>().HasKey(x => new { x.UserId, x.GenreId });
            modelBuilder.Entity<Interest>()
                .HasOne(x => x.User)
                .WithMany(g => g.Interests)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Interest>()
                .HasOne(x => x.Genre)
                .WithMany(e => e.Interests)
                .HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<Follow>().HasKey(x => new { x.FollowingId, x.FollowerId });
            modelBuilder.Entity<Follow>()
                .HasOne(x => x.Follower)
                .WithMany(f => f.Following)
                .HasForeignKey(x => x.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Follow>()
                .HasOne(x => x.Following)
                .WithMany(f => f.Follower)
                .HasForeignKey(x => x.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameGenre>().HasKey(x => new { x.GameId, x.GenreId });
            modelBuilder.Entity<GameGenre>()
                .HasOne(x => x.Game)
                .WithMany(g => g.Genres)
                .HasForeignKey(x => x.GameId);
            modelBuilder.Entity<GameGenre>()
                .HasOne(x => x.Genre)
                .WithMany(f => f.Games)
                .HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<Like>().HasKey(x => new { x.UserId, x.PostId });
            modelBuilder.Entity<Like>()
                .HasOne(x => x.User)
                .WithMany(g => g.Likes)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Like>()
                .HasOne(x => x.Post)
                .WithMany(f => f.Likes)
                .HasForeignKey(x => x.PostId);
        }

        public DbSet<Api.Models.Post> Post { get; set; }

        public DbSet<Api.Models.Comment> Comment { get; set; }
    }
}