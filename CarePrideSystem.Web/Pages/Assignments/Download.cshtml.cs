using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Assignments
{
    [Authorize]
    public class DownloadModel : PageModel
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IHttpContextAccessor _accessor;

        public DownloadModel(IHttpClientFactory httpFactory, IHttpContextAccessor accessor)
        {
            _httpFactory = httpFactory;
            _accessor = accessor;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var token = _accessor.HttpContext?.User.FindFirst("access_token")?.Value;
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var client = _httpFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/assignments/{id}/download");
            if (!response.IsSuccessStatusCode) return NotFound();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
            var filename = response.Content.Headers.ContentDisposition?.FileNameStar
                           ?? response.Content.Headers.ContentDisposition?.FileName
                           ?? $"assignment-{id}";
            filename = filename.Trim('"');

            return File(bytes, contentType, filename);
        }
    }
}
