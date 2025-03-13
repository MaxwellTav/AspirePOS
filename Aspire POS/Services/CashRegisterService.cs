using Aspire_POS.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

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

        public async Task<bool> ProcesarOrdenAsync(OrderRequestModel orden)
        {
            _cache.TryGetValue("ConfigMain", out ConfigMainModel config);
            if (config == null) return false;

            string url = $"{config.HostCredentials.ApiUrl}{PathsModel.ORDERS}";

            var ordenWooCommerce = new
            {
                payment_method = "cod",
                payment_method_title = "Pago en efectivo",
                set_paid = false, // No marcar como pagada
                status = orden.Estado == "on-hold" ? "on-hold" : "processing", // Enviar "on-hold" si es en espera
                billing = new
                {
                    first_name = "Cliente",
                    last_name = "Demo",
                    address_1 = "Calle de ejemplo",
                    city = "Ciudad",
                    country = "MX"
                },
                line_items = orden.Productos.Select(p => new
                {
                    product_id = p.ProductId,
                    quantity = p.Quantity
                }).ToList()
            };

            Console.WriteLine("📤 Orden enviada a WooCommerce:");
            Console.WriteLine(JsonConvert.SerializeObject(ordenWooCommerce, Formatting.Indented));

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(ordenWooCommerce), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.HostCredentials.TokenEndpoint);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

    }
}
