using System.Collections.Generic;

namespace WebApplicationMVC.Web.Authorization
{
    public class PageAccessOptions
    {
        public Dictionary<string, Dictionary<string, AccessLevel>> Users { get; init; } =
            new(StringComparer.OrdinalIgnoreCase);
    }
}
