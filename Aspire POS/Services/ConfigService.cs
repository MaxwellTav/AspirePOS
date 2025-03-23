using Microsoft.EntityFrameworkCore;
using Aspire_POS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using AspNetCoreGeneratedDocument;

namespace Aspire_POS.Services
{
    [Authorize]
    public class ConfigService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public ConfigService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Obtiene todas las configuraciones almacenadas en la base de datos.
        /// </summary>
        public async Task<ConfigMainModel> GetConfigDataAsync(string userName)
        {
            var user = await _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(user))
                return null;

            var hostCredentials = await _context.HostCredentials
                .Where(h => h.UserId == user)
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefaultAsync();

            return new ConfigMainModel
            {
                HostCredentials = hostCredentials
            };
        }

        public async Task UpdateConfigAsync(ConfigMainModel model)
        {
            _cache.TryGetValue("ConfigMain", out ConfigMainModel configCache);

            string userId = configCache.HostCredentials.UserId;

            if (string.IsNullOrEmpty(userId))
                return;

            var hostCredentials = await _context.HostCredentials
                .Where(h => h.UserId == userId)
                .FirstOrDefaultAsync();

            if (hostCredentials != null)
            {
                hostCredentials.ClientKey = model.HostCredentials.ClientKey;
                hostCredentials.ClientSecret = model.HostCredentials.ClientSecret;
                hostCredentials.ApiUrl = model.HostCredentials.ApiUrl;
                hostCredentials.TokenEndpoint = model.HostCredentials.TokenEndpoint;
                hostCredentials.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTokenAsync(string userName, string newToken)
        {
            var userId = await _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            var credentials = await _context.HostCredentials
                .Where(h => h.UserId == userId)
                .FirstOrDefaultAsync();

            if (credentials != null)
            {
                credentials.TokenEndpoint = newToken;
                credentials.UpdatedAt = DateTime.UtcNow;

                _context.Entry(credentials).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }


    }
}
