using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Aspire_POS.Models;
using System.Collections;
using System.Net.Http.Headers;

namespace Aspire_POS.Services
{
    public class StaffService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;

        public StaffService(IMemoryCache cache, HttpClient httpClient)
        {
            _cache = cache;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Consume la API y obtiene la lista de usuarios del staff.
        /// </summary>
        public async Task<StaffMainModel> GetStaffAsync()
        {
            if (!_cache.TryGetValue("ApiUrl", out HostCredentialsModel credentials) || string.IsNullOrEmpty(credentials.ApiUrl))
            {
                return new StaffMainModel { Staff = new List<UserModel>() };
            }

            string apiUrl = credentials.ApiUrl.TrimEnd('/') + "/wp/v2/users";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.TokenEndpoint);
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<UserModel> staffList = JsonSerializer.Deserialize<List<UserModel>>(jsonResponse, options) ?? new List<UserModel>();

                return new StaffMainModel { Staff = staffList };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de solicitud HTTP: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error de deserialización JSON: {ex.Message}");
            }

            return new StaffMainModel { Staff = new List<UserModel>() };
        }
    }
}
