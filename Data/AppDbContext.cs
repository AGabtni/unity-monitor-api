using Microsoft.EntityFrameworkCore;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<MetricData> MetricData { get; set; }
        public DbSet<User> User { get; set; }

    }

}