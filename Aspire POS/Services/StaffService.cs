using System.Threading.Tasks;
using Aspire_POS.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Aspire_POS.Services
{
    public class StaffService
    {
        private readonly IMemoryCache _cache;

        public StaffService(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Obtiene las credenciales almacenadas en caché.
        /// </summary>
        public bool TryGetHostCredentials(out HostCredentialsModel credentials)
        {
            return _cache.TryGetValue("HostCredentials", out credentials);
        }

        /// <summary>
        /// Guarda las credenciales en caché.
        /// </summary>
        public void SetHostCredentials(HostCredentialsModel credentials)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(1));

            _cache.Set("HostCredentials", credentials, cacheOptions);
        }
    }
}
