using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Aspire_POS.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Aspire_POS.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ConfigService _configService;
        private readonly IMemoryCache _cache;

        public LoginController(SignInManager<IdentityUser> signInManager, ConfigService configService, IMemoryCache cache)
        {
            _signInManager = signInManager;
            _configService = configService;
            _cache = cache;
        }

        public IActionResult Index()
        {
            InitializeViewBags(true, true, true);
            return View(new LoginViewModel());
        }

        public IActionResult ConfigureKeys()
        {
            InitializeViewBags(true, true, true);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            InitializeViewBags(true, true, true);

            if (!ModelState.IsValid)
                return View("Index", model);

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var configData = await _configService.GetConfigDataAsync();
                if (configData?.HostCredentials != null)
                {
                    _cache.Set("HostCredentials", configData.HostCredentials, TimeSpan.FromDays(1));
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(nameof(model.Password), "Contraseña incorrecta");
            return View("Index", model);
        }

        public async Task<IActionResult> Logout()
        {
            InitializeViewBags(true, true, true);

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        #region Diseño
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
        #endregion
    }
}
