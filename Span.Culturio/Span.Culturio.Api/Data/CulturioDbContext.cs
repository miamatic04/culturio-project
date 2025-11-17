using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Models;

namespace Span.Culturio.Api.Data
{
    public class CulturioDbContext : DbContext
    {
        public CulturioDbContext(DbContextOptions<CulturioDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CultureObject> CultureObjects { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageCultureObject> PackageCultureObjects { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
            });

            // CultureObject configuration
            modelBuilder.Entity<CultureObject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.ContactEmail).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(250).IsRequired();
                entity.Property(e => e.City).HasMaxLength(250).IsRequired();
            });

            // Package configuration
            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            });

            // PackageCultureObject configuration
            modelBuilder.Entity<PackageCultureObject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Package)
                    .WithMany(p => p.PackageCultureObjects)
                    .HasForeignKey(e => e.PackageId);
                entity.HasOne(e => e.CultureObject)
                    .WithMany(c => c.PackageCultureObjects)
                    .HasForeignKey(e => e.CultureObjectId);
            });

            // Subscription configuration
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.State).HasMaxLength(20).IsRequired();
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Subscriptions)
                    .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Package)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(e => e.PackageId);
            });
        }
    }
}