using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CarePrideSystem.Application.DTOs.Auth;

namespace CarePrideSystem.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        public ApiClient(HttpClient http) { _http = http; }

        public async Task<TokenResponseDto?> LoginAsync(string username, string password)
        {
            var r = await _http.PostAsJsonAsync("api/auth/login", new { username, password });
            if (!r.IsSuccessStatusCode) return null;
            return await r.Content.ReadFromJsonAsync<TokenResponseDto>();
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var r = await _http.GetAsync(url);
            if (r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access denied.");
            r.EnsureSuccessStatusCode();
            if (r.StatusCode == HttpStatusCode.NoContent) return default;
            return await r.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
        {
            var r = await _http.PostAsJsonAsync(url, body);
            if (r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access denied.");
            r.EnsureSuccessStatusCode();
            if (r.StatusCode == HttpStatusCode.NoContent) return default;
            try { return await r.Content.ReadFromJsonAsync<TResponse>(); }
            catch { return default; }
        }

        public async Task PostAsync<TRequest>(string url, TRequest body)
        {
            var r = await _http.PostAsJsonAsync(url, body);
            if (r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access denied.");
            r.EnsureSuccessStatusCode();
        }

        public async Task PostMultipartAsync(string url, MultipartFormDataContent content)
        {
            var r = await _http.PostAsync(url, content);
            if (r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access denied.");
            r.EnsureSuccessStatusCode();
        }

        public async Task PostNoBodyAsync(string url)
        {
            var r = await _http.PostAsync(url, new StringContent(""));
            if (r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access denied.");
            r.EnsureSuccessStatusCode();
        }

        public async Task PutAsync<TRequest>(string url, TRequest body)
        {
            var r = await _http.PutAsJsonAsync(url, body);
            if (r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access denied.");
            r.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string url)
        {
            var r = await _http.DeleteAsync(url);
            if (r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access denied.");
            r.EnsureSuccessStatusCode();
        }
    }
}
