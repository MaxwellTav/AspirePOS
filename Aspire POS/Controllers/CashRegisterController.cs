using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class CashRegisterController : Controller
    {
        public IActionResult Index()
        {
            InitializeViewBags(true, true, true);
            return View();
        }

        public IActionResult AdminCashier()
        {
            InitializeViewBags(false, false, false);
            return View();
        }

        public IActionResult SeeSesions()
        {
            InitializeViewBags(false, false, false);
            return View();
        }

        /// <summary>
        /// Automáticamente oculta o muestra un componente de la página.
        /// </summary>
        /// <param name="_navbar">Esconderá la barra de navegación superior.</param>
        /// <param name="_sidebar">Esconderá la barra lateral, la cuál está el menú y demás opciones.</param>
        /// <param name="_footer">Esconderá el pie de página.</param>
        void InitializeViewBags(bool _navbar, bool _sidebar, bool _footer)
        {
            ViewBag.HideNavbar = _navbar;
            ViewBag.HideSidebar = _sidebar;
            ViewBag.HideFooter = _footer;
        }
    }
}
