using Microsoft.EntityFrameworkCore;
using Aspire_POS.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aspire_POS.Services
{
    [Authorize]
    public class ConfigService
    {
        private readonly ApplicationDbContext _context;

        public ConfigService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las configuraciones almacenadas en la base de datos.
        /// </summary>
        public async Task<ConfigModel> GetConfigDataAsync()
        {
            var hostCredentials = await _context.HostCredentials.FirstOrDefaultAsync();

            return new ConfigModel
            {
                HostCredentials = hostCredentials
            };
        }

        public async Task UpdateConfigAsync(ConfigModel model)
        {
            var hostCredentials = await _context.HostCredentials.FirstOrDefaultAsync();
            if (hostCredentials != null)
            {
                hostCredentials.ClientKey = model.HostCredentials.ClientKey;
                hostCredentials.ClientSecret = model.HostCredentials.ClientSecret;
                hostCredentials.ApiUrl = model.HostCredentials.ApiUrl;

                await _context.SaveChangesAsync();
            }
        }
    }
}
