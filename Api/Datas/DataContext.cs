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
        }

        public DbSet<Api.Models.Interest> Interest { get; set; }
    }
}