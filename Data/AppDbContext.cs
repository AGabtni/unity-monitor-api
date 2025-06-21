using Microsoft.EntityFrameworkCore;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Assets <-> Metrucs
            modelBuilder.Entity<Asset>()
                        .HasMany(a => a.Metrics)
                        .WithOne(m => m.Asset)
                        .HasForeignKey(m => m.AssetId);

            // Store enums as strs
            modelBuilder.Entity<Asset>()
                        .Property(a => a.Type)
                        .HasConversion<string>();

            modelBuilder.Entity<Asset>()
                        .Property(a => a.Status)
                        .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<MetricData> MetricData { get; set; }
        public DbSet<User> User { get; set; }

    }

}