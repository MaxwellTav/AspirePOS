using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Aspire_POS.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Aspire_POS.Models;

namespace Aspire_POS.Controllers
{
    public class LoginController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ConfigService _configService;
        private readonly TokenService _tokenService;
        private readonly IMemoryCache _cache;
        private readonly int daystoexpiresession = 1;

        public LoginController(SignInManager<IdentityUser> signInManager, ConfigService configService, IMemoryCache cache, TokenService tokenService)
        {
            _signInManager = signInManager;
            _configService = configService;
            _tokenService = tokenService;
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
                var configData = await _configService.GetConfigDataAsync(model.UserName);

                if (configData?.HostCredentials != null)
                {
                    var hostCredentials = configData.HostCredentials;

                    //Parche para solucionar problemas de variables nulas (cutre, pero funciona).
                    AspNetUserModel user = new()
                    {
                        UserName = model.UserName,
                        Email = model.UserName
                    };

                    configData.HostCredentials.User = user;

                    _cache.Set("ConfigMain", configData, TimeSpan.FromDays(daystoexpiresession));

                    bool isTokenValid = await _tokenService.IsTokenValid(hostCredentials.TokenEndpoint);

                    if (!isTokenValid)
                    {
                        string clientKey = hostCredentials.ClientKey;
                        string clientSecret = hostCredentials.ClientSecret;

                        string newToken = await _tokenService.RefreshTokenAsync(clientKey, clientSecret);

                        if (!string.IsNullOrEmpty(newToken))
                        {
                            await _configService.UpdateTokenAsync(model.UserName ,newToken);
                            hostCredentials.TokenEndpoint = newToken;
                            ShowMessage("Token", "Token renovado.", MessageResponse.Info);
                        }
                    }
                }

                ShowMessage("Inicio de sesión exitoso.", "¡Bienvenido, " + model.UserName + "!");
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
    }
}