using Microsoft.EntityFrameworkCore;
using Span.Culturio.Subscriptions.Models;

namespace Span.Culturio.Subscriptions.Data
{
    public class SubscriptionsDbContext : DbContext
    {
        public SubscriptionsDbContext(DbContextOptions<SubscriptionsDbContext> options) : base(options) { }

        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.State).HasMaxLength(20).IsRequired();
                // UserId i PackageId ostaju kao int, ali bez .HasOne navigacija prema drugim modelima
            });
        }
    }
}