using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplicationMVC.Web.Authorization;
using WebApplicationMVC.Web.Models;

namespace WebApplicationMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [PageAccess(AccessLevel.View)]
        public IActionResult Index()
        {
            return View();
        }

        [PageAccess(AccessLevel.View)]
        public IActionResult Privacy()
        {
            return View();
        }

        [PageAccess(AccessLevel.Edit)]
        public IActionResult Edit()
        {
            return View();
        }

        [PageAccess(AccessLevel.Full)]
        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
