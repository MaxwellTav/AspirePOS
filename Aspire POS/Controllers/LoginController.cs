using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.HideNavbar = true;
            ViewBag.HideSidebar = true;
            ViewBag.HideFooter = true;
            return View();
        }

        void InitializeViewBags()
        {
            ViewBag.HideNavbar = false;
            ViewBag.HideSidebar = false;
            ViewBag.HideFooter = false;
        }
    }
}
