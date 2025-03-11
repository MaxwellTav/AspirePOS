using Aspire_POS.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Aspire_POS.Services
{
    public class InventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public InventoryService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<ProductModel>> GetInventoryAsync()
        {
            if (!_cache.TryGetValue("ConfigMain", out ConfigMainModel config) || config == null)
                return new List<ProductModel>();

            string baseUrl = config.HostCredentials.ApiUrl + PathsModel.PRODUCTS;
            string consumerKey = config.HostCredentials.ClientKey;
            string consumerSecret = config.HostCredentials.ClientSecret;

            string url = $"{baseUrl}?consumer_key={consumerKey}&consumer_secret={consumerSecret}&per_page=100";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(jsonResult);

                if (products != null)
                    return products;
            }

            return new List<ProductModel>();
        }
    }
}
