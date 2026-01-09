using MySaaS.Domain.Entities;

namespace MySaaS.Web.Models;

public class SubscriptionEditViewModel
{
    public SubscriptionFormModel Form { get; init; } = new();
    public List<Tenant> Tenants { get; init; } = [];
    public List<SubscriptionPlan> Plans { get; init; } = [];
}
