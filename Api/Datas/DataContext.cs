using Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<Game> Games { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<CommentImage> CommentImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Interest>().HasKey(x => new { x.UserId, x.GameId });
            modelBuilder.Entity<Interest>()
                .HasOne(x => x.User)
                .WithMany(g => g.Interests)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Interest>()
                .HasOne(x => x.Game)
                .WithMany(e => e.Interests)
                .HasForeignKey(x => x.GameId);

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

            modelBuilder.Entity<Like>().HasKey(x => new { x.UserId, x.PostId });
            modelBuilder.Entity<Like>()
                .HasOne(x => x.User)
                .WithMany(g => g.Likes)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Like>()
                .HasOne(x => x.Post)
                .WithMany(f => f.Likes)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Image>()
                .HasDiscriminator<string>("image_type")
                .HasValue<Image>("image_base")
                .HasValue<UserImage>("image_user")
                .HasValue<GameImage>("image_game")
                .HasValue<PostImage>("image_post")
                .HasValue<CommentImage>("image_comment");
            
            modelBuilder.Entity<GameImage>()
                .HasOne(g => g.Game)
                .WithOne(i => i.Image)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostImage>()
                .HasOne(p => p.Post)
                .WithOne(i => i.Image)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CommentImage>()
                .HasOne(c => c.Comment)
                .WithOne(i => i.Image)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}