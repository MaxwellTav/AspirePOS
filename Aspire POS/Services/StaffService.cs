using Aspire_POS.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

        #region Create
        public async Task<bool> CreateUserAsync(UserModel newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            if (!_cache.TryGetValue("HostCredentials", out HostCredentialsModel credentials) || string.IsNullOrEmpty(credentials.ApiUrl))
            {
                return false;
            }

            string apiUrl = credentials.ApiUrl.TrimEnd('/') + "/wp-json/wp/v2/users";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.TokenEndpoint);

                var userData = new
                {
                    username = newUser.Username,
                    email = newUser.Email,
                    password = newUser.Password,
                    roles = newUser.Roles.ToArray()
                };

                string jsonContent = JsonSerializer.Serialize(userData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ Usuario creado exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"❌ Error al crear usuario: {response.StatusCode}");
                    Console.WriteLine(responseBody);
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Error HTTP: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"❌ Error JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado: {ex.Message}");
            }

            return false;
        }
        #endregion

        #region Read
        /// <summary>
        /// Consume la API y obtiene la lista de usuarios del staff.
        /// </summary>
        public async Task<StaffMainModel> GetStaffAsync()
        {
            if (!_cache.TryGetValue("HostCredentials", out HostCredentialsModel credentials) || string.IsNullOrEmpty(credentials.ApiUrl))
            {
                return new StaffMainModel { Staff = new List<UserModel>() };
            }

            string apiUrl = credentials.ApiUrl.TrimEnd('/') + "/wp-json/wp/v2/users";

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
        #endregion

        #region Update
        public async Task<bool> UpdateUserAsync(UserModel updatedUser)
        {
            if (updatedUser == null || updatedUser.Id <= 0)
                throw new ArgumentException("El usuario no es válido.");

            if (!_cache.TryGetValue("HostCredentials", out HostCredentialsModel credentials) || string.IsNullOrEmpty(credentials.ApiUrl))
            {
                return false;
            }

            string apiUrl = credentials.ApiUrl.TrimEnd('/') + "/wp-json/wp/v2/users" + updatedUser.Id;

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.TokenEndpoint);

                var userData = new
                {
                    name = updatedUser.Name,
                    email = updatedUser.Email,
                    roles = updatedUser.Roles?.ToArray() ?? new string[] { }
                };

                string jsonContent = JsonSerializer.Serialize(userData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar usuario: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
