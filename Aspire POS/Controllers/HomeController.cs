using Aspire_POS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Aspire_POS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            InitializeViewBags();
            return View();
        }

        public IActionResult Privacy()
        {
            InitializeViewBags();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            InitializeViewBags();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        void InitializeViewBags()
        {
            ViewBag.HideNavbar = false;
            ViewBag.HideSidebar = false;
            ViewBag.HideFooter = false;
        }
    }
}
