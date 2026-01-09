using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MySaaS.Web.Models;

public class UserCreateViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Role")]
    public string RoleName { get; set; } = string.Empty;

    [Display(Name = "Tenant")]
    public Guid? TenantId { get; set; }

    public IReadOnlyCollection<SelectListItem> Roles { get; set; } = Array.Empty<SelectListItem>();
    public IReadOnlyCollection<SelectListItem> Tenants { get; set; } = Array.Empty<SelectListItem>();
}
