using MySaaS.Domain.Entities;

namespace MySaaS.Application.Interfaces;

public interface IPlanService
{
    Task<List<SubscriptionPlan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SubscriptionPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);
    Task UpdateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
