namespace MySaaS.Infrastructure.Identity;

public static class RoleNames
{
    public const string SuperAdmin = "SuperAdmin";
    public const string TenantAdmin = "TenantAdmin";
    public const string TenantUser = "TenantUser";
    public const string Admins = SuperAdmin + "," + TenantAdmin;
}
