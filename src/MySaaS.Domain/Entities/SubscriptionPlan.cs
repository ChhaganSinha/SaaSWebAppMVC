namespace MySaaS.Domain.Entities;

public class SubscriptionPlan : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public decimal PricePerMonth { get; set; }
    public int MaxUsers { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
