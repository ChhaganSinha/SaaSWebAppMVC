namespace MySaaS.Domain.Entities;

public class Tenant : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
