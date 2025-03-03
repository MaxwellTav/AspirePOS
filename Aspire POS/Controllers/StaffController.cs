using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aspire_POS.Services;
using Aspire_POS.Models;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        private readonly StaffService _staffService;

        public StaffController(StaffService staffService)
        {
            _staffService = staffService;
        }

        public async Task<IActionResult> Index()
        {
            InitializeViewBags(false, false, false);
            StaffMainModel model = await _staffService.GetStaffAsync();
            return View(model);
        }

        #region Diseño
        void InitializeViewBags(bool _navbar, bool _sidebar, bool _footer)
        {
            ViewBag.HideNavbar = _navbar;
            ViewBag.HideSidebar = _sidebar;
            ViewBag.HideFooter = _footer;
        }
        #endregion
    }
}
