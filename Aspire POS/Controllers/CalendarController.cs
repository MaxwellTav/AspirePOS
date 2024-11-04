using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            InitializeViewBags(false, false, false);
            return View();
        }

        void InitializeViewBags(bool _navbar, bool _sidebar, bool _footer)
        {
            ViewBag.HideNavbar = _navbar;
            ViewBag.HideSidebar = _sidebar;
            ViewBag.HideFooter = _footer;
        }
    }
}
