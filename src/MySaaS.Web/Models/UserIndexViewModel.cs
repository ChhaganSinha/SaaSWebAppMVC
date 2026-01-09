namespace MySaaS.Web.Models;

public class UserIndexViewModel
{
    public IReadOnlyCollection<UserListItemViewModel> Users { get; set; } = Array.Empty<UserListItemViewModel>();
}

public class UserListItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TenantName { get; set; } = "Unassigned";
    public string Roles { get; set; } = "";
}
