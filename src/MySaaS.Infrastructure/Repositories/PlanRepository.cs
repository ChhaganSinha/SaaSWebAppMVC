using Microsoft.EntityFrameworkCore;
using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;
using MySaaS.Infrastructure.Data;

namespace MySaaS.Infrastructure.Repositories;

public class PlanRepository(SaaSAppDbContext dbContext) : IPlanRepository
{
    public Task<List<SubscriptionPlan>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.SubscriptionPlans.AsNoTracking().OrderBy(p => p.PricePerMonth).ToListAsync(cancellationToken);

    public Task<SubscriptionPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
    {
        dbContext.SubscriptionPlans.Add(plan);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
    {
        dbContext.SubscriptionPlans.Update(plan);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
    {
        dbContext.SubscriptionPlans.Remove(plan);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
