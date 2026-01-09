using System.ComponentModel.DataAnnotations;

namespace MySaaS.Web.Models;

public class PlanFormModel
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal PricePerMonth { get; set; }

    [Range(1, 10000)]
    public int MaxUsers { get; set; } = 1;

    public bool IsActive { get; set; } = true;
}
