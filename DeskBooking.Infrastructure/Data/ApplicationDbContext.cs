using DeskBooking.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Workspace>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Capacity).IsRequired();
                entity.Property(e => e.Amenities).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList());
                entity.Property(e => e.Photos).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList());
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.UserEmail).IsRequired();
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Workspace)
                    .WithMany(w => w.Bookings)
                    .HasForeignKey(e => e.WorkspaceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 