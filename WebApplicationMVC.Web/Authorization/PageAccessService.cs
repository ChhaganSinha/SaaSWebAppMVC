using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace WebApplicationMVC.Web.Authorization
{
    public class PageAccessService : IPageAccessService
    {
        private readonly PageAccessOptions _options;

        public PageAccessService(IOptions<PageAccessOptions> options)
        {
            _options = options.Value;
        }

        public IReadOnlyCollection<string> GetUserNames()
        {
            return _options.Users.Keys.ToList();
        }

        public bool HasAccess(string? userName, string pageKey, AccessLevel requiredAccess)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (!_options.Users.TryGetValue(userName, out var pages))
            {
                return false;
            }

            if (!pages.TryGetValue(pageKey, out var access))
            {
                return false;
            }

            return access >= requiredAccess;
        }
    }
}
