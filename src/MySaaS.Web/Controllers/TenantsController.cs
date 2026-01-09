using Microsoft.AspNetCore.Mvc;
using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;
using MySaaS.Web.Models;

namespace MySaaS.Web.Controllers;

public class TenantsController(ITenantService tenantService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var tenants = await tenantService.GetAllAsync(cancellationToken);
        return View(tenants);
    }

    public IActionResult Create() => View(new TenantFormModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TenantFormModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var tenant = new Tenant
        {
            Name = model.Name,
            Slug = model.Slug,
            ContactEmail = model.ContactEmail,
            IsActive = model.IsActive
        };

        await tenantService.CreateAsync(tenant, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var tenant = await tenantService.GetByIdAsync(id, cancellationToken);
        if (tenant is null)
        {
            return NotFound();
        }

        var model = new TenantFormModel
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Slug = tenant.Slug,
            ContactEmail = tenant.ContactEmail,
            IsActive = tenant.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TenantFormModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var tenant = await tenantService.GetByIdAsync(id, cancellationToken);
        if (tenant is null)
        {
            return NotFound();
        }

        tenant.Name = model.Name;
        tenant.Slug = model.Slug;
        tenant.ContactEmail = model.ContactEmail;
        tenant.IsActive = model.IsActive;

        await tenantService.UpdateAsync(tenant, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await tenantService.DeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
