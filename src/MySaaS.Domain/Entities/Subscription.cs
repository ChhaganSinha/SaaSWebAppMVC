namespace MySaaS.Domain.Entities;

public class Subscription : EntityBase
{
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    public Guid SubscriptionPlanId { get; set; }
    public SubscriptionPlan? SubscriptionPlan { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? EndDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
}
