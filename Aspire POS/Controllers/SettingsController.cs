using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aspire_POS.Services;
using Aspire_POS.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ConfigService _configService;

        public SettingsController(ConfigService configService)
        {
            _configService = configService;
        }

        public async Task<IActionResult> Index()
        {
            InitializeViewBags(false, false, false);

            // Obtener todas las configuraciones desde la base de datos
            var configData = await _configService.GetConfigDataAsync();
            return View(configData);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConfig(ConfigModel model)
        {
            if (!ModelState.IsValid)
            {
                await _configService.UpdateConfigAsync(model);
                TempData["SuccessMessage"] = "Se han aplicado los cambios";
            }
            else
            { 
                TempData["ErrorMessage"] = "Error al actualizar la configuración";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Automáticamente oculta o muestra un componente de la página.
        /// </summary>
        void InitializeViewBags(bool _navbar, bool _sidebar, bool _footer)
        {
            ViewBag.HideNavbar = _navbar;
            ViewBag.HideSidebar = _sidebar;
            ViewBag.HideFooter = _footer;
        }
    }
}
