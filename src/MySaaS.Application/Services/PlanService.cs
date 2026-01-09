using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;

namespace MySaaS.Application.Services;

public class PlanService(IPlanRepository planRepository) : IPlanService
{
    public Task<List<SubscriptionPlan>> GetAllAsync(CancellationToken cancellationToken = default)
        => planRepository.GetAllAsync(cancellationToken);

    public Task<SubscriptionPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => planRepository.GetByIdAsync(id, cancellationToken);

    public async Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
    {
        plan.UpdatedAt = DateTimeOffset.UtcNow;
        await planRepository.AddAsync(plan, cancellationToken);
        return plan;
    }

    public async Task UpdateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
    {
        plan.UpdatedAt = DateTimeOffset.UtcNow;
        await planRepository.UpdateAsync(plan, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var plan = await planRepository.GetByIdAsync(id, cancellationToken);
        if (plan is null)
        {
            return;
        }

        await planRepository.DeleteAsync(plan, cancellationToken);
    }
}
