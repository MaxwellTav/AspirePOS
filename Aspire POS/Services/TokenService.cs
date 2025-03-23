using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Aspire_POS.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

public class TokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TokenService> _logger;
    private readonly IMemoryCache _cache;

    public TokenService(IHttpClientFactory httpClientFactory, ILogger<TokenService> logger, IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _cache = cache;
    }

    /// <summary>
    /// Verifica si el token es válido haciendo una solicitud a WooCommerce.
    /// </summary>
    public async Task<bool> IsTokenValid(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;

        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            _cache.TryGetValue("ConfigMain", out ConfigMainModel hostCredentials);
            var request = new HttpRequestMessage(HttpMethod.Get, hostCredentials.HostCredentials.ApiUrl + "wp-json/wc/v3/orders");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return false;
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Error al validar el token: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Solicita un nuevo token a WooCommerce.
    /// </summary>
    public async Task<string> RefreshTokenAsync(string username, string password)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            _cache.TryGetValue("ConfigMain", out ConfigMainModel configCache);
            HostCredentialsModel hostCredentials = configCache.HostCredentials;

            _cache.TryGetValue("LoginCredentials", out LoginViewModel loginModel);

            string refreshUrl = hostCredentials.ApiUrl + PathsModel.GETTOKEN;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", loginModel.UserName),
                new KeyValuePair<string, string>("password", loginModel.Password)
            });

            HttpResponseMessage response = await httpClient.PostAsync(refreshUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"❌ Error al refrescar token: {response.StatusCode}");
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = System.Text.Json.JsonDocument.Parse(responseBody);

            return jsonDoc.RootElement.TryGetProperty("token", out var tokenElement)
                ? tokenElement.GetString()
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Excepción al refrescar token: {ex.Message}");
            return null;
        }
    }
}
