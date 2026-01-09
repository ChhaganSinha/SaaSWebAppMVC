using Microsoft.EntityFrameworkCore;
using MySaaS.Domain.Entities;

namespace MySaaS.Infrastructure.Data;

public static class SaaSAppSeeder
{
    public static async Task SeedAsync(SaaSAppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (!await dbContext.SubscriptionPlans.AnyAsync(cancellationToken))
        {
            dbContext.SubscriptionPlans.AddRange(
                new SubscriptionPlan
                {
                    Name = "Starter",
                    PricePerMonth = 29,
                    MaxUsers = 5,
                    IsActive = true
                },
                new SubscriptionPlan
                {
                    Name = "Growth",
                    PricePerMonth = 99,
                    MaxUsers = 25,
                    IsActive = true
                },
                new SubscriptionPlan
                {
                    Name = "Enterprise",
                    PricePerMonth = 299,
                    MaxUsers = 200,
                    IsActive = true
                }
            );
        }

        if (!await dbContext.Tenants.AnyAsync(cancellationToken))
        {
            dbContext.Tenants.Add(new Tenant
            {
                Name = "Contoso Labs",
                Slug = "contoso",
                ContactEmail = "admin@contoso.example",
                IsActive = true
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
