using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySaaS.Application.Interfaces;
using MySaaS.Web.Models;

namespace MySaaS.Web.Controllers;

[Authorize]
public class HomeController(
    ITenantService tenantService,
    IPlanService planService,
    ISubscriptionService subscriptionService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = new DashboardViewModel
        {
            Tenants = await tenantService.GetAllAsync(cancellationToken),
            Plans = await planService.GetAllAsync(cancellationToken),
            Subscriptions = await subscriptionService.GetAllAsync(cancellationToken)
        };

        return View(model);
    }
}
