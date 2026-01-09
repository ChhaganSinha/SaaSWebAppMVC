using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;

namespace MySaaS.Application.Services;

public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
{
    public Task<List<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
        => subscriptionRepository.GetAllAsync(cancellationToken);

    public Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => subscriptionRepository.GetByIdAsync(id, cancellationToken);

    public async Task<Subscription> CreateAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        subscription.UpdatedAt = DateTimeOffset.UtcNow;
        await subscriptionRepository.AddAsync(subscription, cancellationToken);
        return subscription;
    }

    public async Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        subscription.UpdatedAt = DateTimeOffset.UtcNow;
        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    public async Task CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription is null)
        {
            return;
        }

        subscription.Status = SubscriptionStatus.Canceled;
        subscription.EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
        subscription.UpdatedAt = DateTimeOffset.UtcNow;
        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }
}
