using Microsoft.AspNetCore.Identity;
using MySaaS.Domain.Entities;

namespace MySaaS.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}
