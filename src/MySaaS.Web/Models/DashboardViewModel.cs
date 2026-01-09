using MySaaS.Domain.Entities;

namespace MySaaS.Web.Models;

public class DashboardViewModel
{
    public List<Tenant> Tenants { get; init; } = [];
    public List<SubscriptionPlan> Plans { get; init; } = [];
    public List<Subscription> Subscriptions { get; init; } = [];
}
