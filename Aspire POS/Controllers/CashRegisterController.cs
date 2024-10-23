using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class CashRegisterController : Controller
    {
        public IActionResult Index()
        {
            InitializeViewBags();
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
