using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySaaS.Domain.Entities;
using MySaaS.Infrastructure.Identity;

namespace MySaaS.Infrastructure.Data;

public static class SaaSAppSeeder
{
    public static async Task SeedAsync(
        SaaSAppDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        CancellationToken cancellationToken = default)
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

        if (!await roleManager.RoleExistsAsync(RoleNames.SuperAdmin))
        {
            await roleManager.CreateAsync(new IdentityRole(RoleNames.SuperAdmin));
        }

        if (!await roleManager.RoleExistsAsync(RoleNames.TenantAdmin))
        {
            await roleManager.CreateAsync(new IdentityRole(RoleNames.TenantAdmin));
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        await EnsureSuperAdminAsync(userManager);
        await EnsureTenantAdminsAsync(dbContext, userManager);
    }

    private static async Task EnsureSuperAdminAsync(UserManager<ApplicationUser> userManager)
    {
        const string email = "superadmin@mysaas.local";
        const string password = "Admin123!";

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(user, RoleNames.SuperAdmin))
        {
            await userManager.AddToRoleAsync(user, RoleNames.SuperAdmin);
        }
    }

    private static async Task EnsureTenantAdminsAsync(
        SaaSAppDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        var tenants = await dbContext.Tenants.AsNoTracking().ToListAsync();

        foreach (var tenant in tenants)
        {
            var email = tenant.ContactEmail;
            if (string.IsNullOrWhiteSpace(email))
            {
                continue;
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    TenantId = tenant.Id
                };

                var result = await userManager.CreateAsync(user, "Tenant123!");
                if (!result.Succeeded)
                {
                    continue;
                }
            }

            if (!await userManager.IsInRoleAsync(user, RoleNames.TenantAdmin))
            {
                await userManager.AddToRoleAsync(user, RoleNames.TenantAdmin);
            }
        }
    }
}
