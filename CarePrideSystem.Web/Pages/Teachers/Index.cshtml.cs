using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Domain.Enums;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Teachers
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly ApiClient _api;
        public IndexModel(ApiClient api) { _api = api; }

        public List<UserDto> Teachers { get; set; } = new();
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var users = await _api.GetAsync<List<UserDto>>("api/users");
                Teachers = users?.Where(u => u.Role == UserRole.Teacher).ToList() ?? new();
            }
            catch (Exception ex) { Error = ex.Message; }
        }
    }
}
