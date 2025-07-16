using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Resource entity
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.Capacity).IsRequired();
                entity.Property(e => e.IsAvailable).IsRequired();
            });

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ResourceId).IsRequired();
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
                entity.Property(e => e.BookedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Purpose).IsRequired().HasMaxLength(200);
                entity.Property(e => e.BookingDate).IsRequired();

                // Configure relationship
                entity.HasOne(e => e.Resource)
                      .WithMany(r => r.Bookings)
                      .HasForeignKey(e => e.ResourceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}