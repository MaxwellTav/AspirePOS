using Aspire_POS.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Aspire_POS.Services
{
    public class CashRegisterService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public CashRegisterService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            _cache.TryGetValue("ConfigMain", out ConfigMainModel config);
            if (config == null) return new List<ProductModel>();

            string url = $"{config.HostCredentials.ApiUrl}{PathsModel.PRODUCTS}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.HostCredentials.TokenEndpoint); // 🔹 Usa el token JWT

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return new List<ProductModel>();
            }

            string jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductModel>>(jsonResult) ?? new List<ProductModel>();
        }

    }
}
