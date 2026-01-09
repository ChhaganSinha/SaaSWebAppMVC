using Microsoft.EntityFrameworkCore;
using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;
using MySaaS.Infrastructure.Data;

namespace MySaaS.Infrastructure.Repositories;

public class SubscriptionRepository(SaaSAppDbContext dbContext) : ISubscriptionRepository
{
    public Task<List<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Subscriptions
            .Include(s => s.Tenant)
            .Include(s => s.SubscriptionPlan)
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

    public Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Subscriptions
            .Include(s => s.Tenant)
            .Include(s => s.SubscriptionPlan)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        dbContext.Subscriptions.Add(subscription);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        dbContext.Subscriptions.Update(subscription);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        dbContext.Subscriptions.Remove(subscription);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
