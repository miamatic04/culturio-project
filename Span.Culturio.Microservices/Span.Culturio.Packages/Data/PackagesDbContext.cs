using Microsoft.EntityFrameworkCore;
using Span.Culturio.Packages.Models;

namespace Span.Culturio.Packages.Data
{
    public class PackagesDbContext : DbContext
    {
        public PackagesDbContext(DbContextOptions<PackagesDbContext> options) : base(options) { }

        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageCultureObject> PackageCultureObjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<PackageCultureObject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Package)
                    .WithMany(p => p.PackageCultureObjects)
                    .HasForeignKey(e => e.PackageId);
            });
        }
    }
}