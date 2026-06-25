using ApartmentsPriceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ApartmentsPriceApi.Data
{
    public class ApartmentsDbContext : DbContext
    {
        public ApartmentsDbContext(DbContextOptions<ApartmentsDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
                .HasKey(s => new 
                { 
                    s.ApartmentId, 
                    s.SubscriberId 
                });

            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Subscriber)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(s => s.SubscriberId);

            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Apartment)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(s => s.ApartmentId);

            modelBuilder.Entity<Subscriber>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Apartment>()
                .HasIndex(a => a.Url)
                .IsUnique();
        }

    }
}
