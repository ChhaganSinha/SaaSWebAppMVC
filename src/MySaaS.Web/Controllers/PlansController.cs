using Microsoft.AspNetCore.Mvc;
using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;
using MySaaS.Web.Models;

namespace MySaaS.Web.Controllers;

public class PlansController(IPlanService planService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var plans = await planService.GetAllAsync(cancellationToken);
        return View(plans);
    }

    public IActionResult Create() => View(new PlanFormModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PlanFormModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var plan = new SubscriptionPlan
        {
            Name = model.Name,
            PricePerMonth = model.PricePerMonth,
            MaxUsers = model.MaxUsers,
            IsActive = model.IsActive
        };

        await planService.CreateAsync(plan, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var plan = await planService.GetByIdAsync(id, cancellationToken);
        if (plan is null)
        {
            return NotFound();
        }

        var model = new PlanFormModel
        {
            Id = plan.Id,
            Name = plan.Name,
            PricePerMonth = plan.PricePerMonth,
            MaxUsers = plan.MaxUsers,
            IsActive = plan.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, PlanFormModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var plan = await planService.GetByIdAsync(id, cancellationToken);
        if (plan is null)
        {
            return NotFound();
        }

        plan.Name = model.Name;
        plan.PricePerMonth = model.PricePerMonth;
        plan.MaxUsers = model.MaxUsers;
        plan.IsActive = model.IsActive;

        await planService.UpdateAsync(plan, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await planService.DeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
