using MySaaS.Domain.Entities;

namespace MySaaS.Application.Interfaces;

public interface IPlanRepository
{
    Task<List<SubscriptionPlan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SubscriptionPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);
    Task UpdateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);
    Task DeleteAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);
}
