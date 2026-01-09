using Microsoft.EntityFrameworkCore;
using MySaaS.Domain.Entities;

namespace MySaaS.Infrastructure.Data;

public class SaaSAppDbContext(DbContextOptions<SaaSAppDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasIndex(t => t.Slug).IsUnique();
            entity.Property(t => t.Name).HasMaxLength(200).IsRequired();
            entity.Property(t => t.Slug).HasMaxLength(80).IsRequired();
            entity.Property(t => t.ContactEmail).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
            entity.Property(p => p.PricePerMonth).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasOne(s => s.Tenant)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.SubscriptionPlan)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(s => s.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
