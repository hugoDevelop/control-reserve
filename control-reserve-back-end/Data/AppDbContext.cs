using control_reserve_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace control_reserve_back_end.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reservation>()
                .HasIndex(r => new { r.SpaceId, r.StartTime, r.EndTime })
                .HasDatabaseName("IX_Reservations_SpaceId_StartTime_EndTime")
                .HasFilter("\"EndTime\" > \"StartTime\"");
        }
    }
}
