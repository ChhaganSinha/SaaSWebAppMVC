using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySaaS.Application.Interfaces;
using MySaaS.Application.Services;
using MySaaS.Infrastructure.Data;
using MySaaS.Infrastructure.Identity;
using MySaaS.Infrastructure.Repositories;

namespace MySaaS.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSaaSInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SaaSAppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SaaSDatabase")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<SaaSAppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();

        return services;
    }
}
