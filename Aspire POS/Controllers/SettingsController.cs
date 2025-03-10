using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aspire_POS.Services;
using Aspire_POS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private readonly ConfigService _configService;
        private readonly IMemoryCache _cache;

        public SettingsController(ConfigService configService, IMemoryCache cache)
        {
            _configService = configService;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            InitializeViewBags(false, false, false);
            ConfigMainModel configData = new ConfigMainModel();

            if (!_cache.TryGetValue("ConfigMain", out var cachedUser) || cachedUser is not ConfigMainModel user)
            {
                //Aquí se manejará la excepción de mostrar un mensaje
                //return RedirectToAction("Index", "Login");
                return View(configData);
            }

            configData = await _configService.GetConfigDataAsync(user.HostCredentials.User.UserName);
            return View(configData);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConfig(ConfigMainModel model)
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

        public IActionResult PermissionConfig()
        {
            InitializeViewBags(false, false, false);

            return View();
        }
    }
}
