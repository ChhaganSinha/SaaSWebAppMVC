using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySaaS.Application.Interfaces;
using MySaaS.Infrastructure.Identity;
using MySaaS.Web.Models;

namespace MySaaS.Web.Controllers;

[Authorize(Roles = RoleNames.Admins)]
public class UsersController(
    ITenantService tenantService,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var currentUser = await userManager.GetUserAsync(User);
        var isSuperAdmin = User.IsInRole(RoleNames.SuperAdmin);
        var usersQuery = userManager.Users.AsNoTracking();

        if (!isSuperAdmin)
        {
            var tenantId = currentUser?.TenantId;
            usersQuery = usersQuery.Where(user => user.TenantId == tenantId);
        }

        var users = await usersQuery.ToListAsync(cancellationToken);
        var tenants = await tenantService.GetAllAsync(cancellationToken);
        var tenantLookup = tenants.ToDictionary(tenant => tenant.Id, tenant => tenant.Name);
        var userItems = new List<UserListItemViewModel>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            tenantLookup.TryGetValue(user.TenantId ?? Guid.Empty, out var tenantName);

            userItems.Add(new UserListItemViewModel
            {
                Id = user.Id,
                Email = user.Email ?? user.UserName ?? string.Empty,
                TenantName = tenantName ?? "Unassigned",
                Roles = string.Join(", ", roles)
            });
        }

        return View(new UserIndexViewModel { Users = userItems });
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = await BuildCreateModelAsync(new UserCreateViewModel(), cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel model, CancellationToken cancellationToken)
    {
        var currentUser = await userManager.GetUserAsync(User);
        var isSuperAdmin = User.IsInRole(RoleNames.SuperAdmin);

        if (!isSuperAdmin)
        {
            model.TenantId = currentUser?.TenantId;
            if (model.TenantId is null)
            {
                ModelState.AddModelError(nameof(UserCreateViewModel.TenantId), "Tenant assignment is required.");
            }
            if (model.RoleName == RoleNames.SuperAdmin)
            {
                ModelState.AddModelError(nameof(UserCreateViewModel.RoleName), "You cannot assign the SuperAdmin role.");
            }
        }

        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildCreateModelAsync(model, cancellationToken);
            return View(invalidModel);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            TenantId = model.TenantId
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            var invalidModel = await BuildCreateModelAsync(model, cancellationToken);
            return View(invalidModel);
        }

        if (await roleManager.RoleExistsAsync(model.RoleName))
        {
            await userManager.AddToRoleAsync(user, model.RoleName);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<UserCreateViewModel> BuildCreateModelAsync(
        UserCreateViewModel model,
        CancellationToken cancellationToken)
    {
        var isSuperAdmin = User.IsInRole(RoleNames.SuperAdmin);
        var roles = await roleManager.Roles
            .Select(role => role.Name)
            .Where(name => name != null)
            .ToListAsync(cancellationToken);

        if (!isSuperAdmin)
        {
            roles = roles
                .Where(role => role != RoleNames.SuperAdmin)
                .ToList();
        }

        model.Roles = roles
            .Select(role => new SelectListItem(role!, role))
            .ToList();

        if (isSuperAdmin)
        {
            var tenants = await tenantService.GetAllAsync(cancellationToken);
            model.Tenants = tenants
                .Select(tenant => new SelectListItem(tenant.Name, tenant.Id.ToString()))
                .ToList();
        }

        return model;
    }
}
