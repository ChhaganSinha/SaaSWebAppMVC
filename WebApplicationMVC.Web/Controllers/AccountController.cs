using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApplicationMVC.Web.Authorization;
using WebApplicationMVC.Web.Models;

namespace WebApplicationMVC.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPageAccessService _pageAccessService;

        public AccountController(IPageAccessService pageAccessService)
        {
            _pageAccessService = pageAccessService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                AvailableUsers = _pageAccessService.GetUserNames()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableUsers = _pageAccessService.GetUserNames();
                return View(model);
            }

            if (!_pageAccessService.GetUserNames().Contains(model.UserName))
            {
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Select a valid user.");
                model.AvailableUsers = _pageAccessService.GetUserNames();
                return View(model);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
