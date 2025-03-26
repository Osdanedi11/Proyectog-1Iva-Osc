using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieSeriesManagement.Models.Entities;

namespace MovieSeriesManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Content> Contents { get; set; }
        public DbSet<ViewingHistory> ViewingHistories { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<ViewingHistory>()
                .HasOne(vh => vh.User)
                .WithMany(u => u.ViewingHistories)
                .HasForeignKey(vh => vh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ViewingHistory>()
                .HasOne(vh => vh.Content)
                .WithMany(c => c.ViewingHistories)
                .HasForeignKey(vh => vh.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Recommendations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.Content)
                .WithMany(c => c.Recommendations)
                .HasForeignKey(r => r.ContentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

