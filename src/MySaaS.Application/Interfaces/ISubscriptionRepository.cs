using MySaaS.Domain.Entities;

namespace MySaaS.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task<List<Subscription>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
    Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default);
    Task DeleteAsync(Subscription subscription, CancellationToken cancellationToken = default);
}
