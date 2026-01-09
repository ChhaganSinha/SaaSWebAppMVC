using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplicationMVC.Web.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PageAccessAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly AccessLevel _requiredAccess;

        public PageAccessAttribute(AccessLevel requiredAccess)
        {
            _requiredAccess = requiredAccess;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new ChallengeResult();
                return Task.CompletedTask;
            }

            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();
            var pageKey = $"{controller}/{action}";

            var accessService = context.HttpContext.RequestServices.GetService(typeof(IPageAccessService))
                as IPageAccessService;

            if (accessService == null || !accessService.HasAccess(user.Identity?.Name, pageKey, _requiredAccess))
            {
                context.Result = new ForbidResult();
            }

            return Task.CompletedTask;
        }
    }
}
