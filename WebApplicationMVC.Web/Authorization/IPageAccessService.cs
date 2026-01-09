using System.Collections.Generic;

namespace WebApplicationMVC.Web.Authorization
{
    public interface IPageAccessService
    {
        bool HasAccess(string? userName, string pageKey, AccessLevel requiredAccess);
        IReadOnlyCollection<string> GetUserNames();
    }
}
