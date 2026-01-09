using Microsoft.AspNetCore.Mvc;
using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;
using MySaaS.Web.Models;

namespace MySaaS.Web.Controllers;

public class SubscriptionsController(
    ITenantService tenantService,
    IPlanService planService,
    ISubscriptionService subscriptionService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionService.GetAllAsync(cancellationToken);
        return View(subscriptions);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = await BuildEditModelAsync(new SubscriptionFormModel(), cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriptionFormModel form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildEditModelAsync(form, cancellationToken);
            return View(invalidModel);
        }

        var subscription = new Subscription
        {
            TenantId = form.TenantId,
            SubscriptionPlanId = form.SubscriptionPlanId,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            Status = SubscriptionStatus.Active
        };

        await subscriptionService.CreateAsync(subscription, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionService.GetByIdAsync(id, cancellationToken);
        if (subscription is null)
        {
            return NotFound();
        }

        var form = new SubscriptionFormModel
        {
            Id = subscription.Id,
            TenantId = subscription.TenantId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate
        };

        var model = await BuildEditModelAsync(form, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, SubscriptionFormModel form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildEditModelAsync(form, cancellationToken);
            return View(invalidModel);
        }

        var subscription = await subscriptionService.GetByIdAsync(id, cancellationToken);
        if (subscription is null)
        {
            return NotFound();
        }

        subscription.TenantId = form.TenantId;
        subscription.SubscriptionPlanId = form.SubscriptionPlanId;
        subscription.StartDate = form.StartDate;
        subscription.EndDate = form.EndDate;

        await subscriptionService.UpdateAsync(subscription, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await subscriptionService.CancelAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task<SubscriptionEditViewModel> BuildEditModelAsync(
        SubscriptionFormModel form,
        CancellationToken cancellationToken)
    {
        var tenants = await tenantService.GetAllAsync(cancellationToken);
        var plans = await planService.GetAllAsync(cancellationToken);

        return new SubscriptionEditViewModel
        {
            Form = form,
            Tenants = tenants,
            Plans = plans
        };
    }
}
