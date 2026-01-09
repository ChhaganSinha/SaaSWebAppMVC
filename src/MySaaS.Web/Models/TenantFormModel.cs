using System.ComponentModel.DataAnnotations;

namespace MySaaS.Web.Models;

public class TenantFormModel
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string ContactEmail { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
