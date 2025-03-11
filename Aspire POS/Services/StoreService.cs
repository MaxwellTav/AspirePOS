using Aspire_POS.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace Aspire_POS.Services
{
    public class StoreService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public StoreService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<int> GetProductCountAsync()
        {
            if (!_cache.TryGetValue("ConfigMain", out ConfigMainModel config) || config == null)
                return 0;

            string baseUrl = config.HostCredentials.ApiUrl + PathsModel.PRODUCTS;
            string consumerKey = config.HostCredentials.ClientKey;
            string consumerSecret = config.HostCredentials.ClientSecret;

            string url = $"{baseUrl}?consumer_key={consumerKey}&consumer_secret={consumerSecret}&per_page=1";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                if (response.Headers.TryGetValues("X-WP-Total", out var values))
                {
                    if (int.TryParse(values.FirstOrDefault(), out int productCount))
                    {
                        return productCount;
                    }
                }
            }
            return 0;
        }
    }
}
