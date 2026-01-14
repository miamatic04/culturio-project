using Microsoft.EntityFrameworkCore;
using Span.Culturio.CultureObjects.Models;

namespace Span.Culturio.CultureObjects.Data
{
    public class CultureObjectsDbContext : DbContext
    {
        public CultureObjectsDbContext(DbContextOptions<CultureObjectsDbContext> options) : base(options) { }

        public DbSet<CultureObject> CultureObjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CultureObject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.ContactEmail).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(250).IsRequired();
                entity.Property(e => e.City).HasMaxLength(250).IsRequired();
            });
        }
    }
}