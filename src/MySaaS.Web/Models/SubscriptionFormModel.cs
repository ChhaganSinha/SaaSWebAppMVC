using System.ComponentModel.DataAnnotations;

namespace MySaaS.Web.Models;

public class SubscriptionFormModel
{
    public Guid? Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public Guid SubscriptionPlanId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public DateOnly? EndDate { get; set; }
}
