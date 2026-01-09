using MySaaS.Domain.Entities;

namespace MySaaS.Application.Interfaces;

public interface ISubscriptionService
{
    Task<List<Subscription>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Subscription> CreateAsync(Subscription subscription, CancellationToken cancellationToken = default);
    Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default);
    Task CancelAsync(Guid id, CancellationToken cancellationToken = default);
}
