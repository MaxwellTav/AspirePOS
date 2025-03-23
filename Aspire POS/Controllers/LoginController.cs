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
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ConfigService _configService;
        private readonly TokenService _tokenService;
        private readonly IMemoryCache _cache;
        private readonly int daystoexpiresession = 1;

        public LoginController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, ConfigService configService, IMemoryCache cache, TokenService tokenService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

                    // 🚀 Obtener el usuario desde la base de datos
                    var identityUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserName == model.UserName);

                    if (identityUser == null)
                    {
                        return View("Error", new { message = "Usuario no encontrado." });
                    }

                    _context.Entry(identityUser).State = EntityState.Detached; // Evita rastreo en EF

                    // 🚀 Mapeo manual si AspNetUserModel NO hereda de IdentityUser
                    var user = new AspNetUserModel
                    {
                        Id = identityUser.Id,
                        UserName = identityUser.UserName,
                        Email = identityUser.Email
                    };

                    // 🚀 Validar que HostCredentials existe antes de asignar el usuario
                    if (configData.HostCredentials == null)
                    {
                        return View("Error", new { message = "No se encontraron credenciales para este usuario." });
                    }

                    configData.HostCredentials.User = user; // ✅ Ahora `user` siempre tiene un valor válido

                    _cache.Set("ConfigMain", configData, TimeSpan.FromDays(daystoexpiresession));
                    _cache.Set("LoginCredentials", model, TimeSpan.FromMinutes(1));

                    bool isTokenValid = await _tokenService.IsTokenValid(hostCredentials.TokenEndpoint);

                    if (!isTokenValid)
                    {
                        string clientKey = hostCredentials.ClientKey;
                        string clientSecret = hostCredentials.ClientSecret;

                        string newToken = await _tokenService.RefreshTokenAsync(clientKey, clientSecret);

                        if (!string.IsNullOrEmpty(newToken))
                        {
                            await _configService.UpdateTokenAsync(model.UserName, newToken);
                            hostCredentials.TokenEndpoint = newToken;
                            ShowMessage("Token", "Token renovado.", MessageResponse.Info);
                        }
                    }
                }

                ShowMessage("Inicio de sesión exitoso.", $"¡Bienvenido, {model.UserName}!");
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