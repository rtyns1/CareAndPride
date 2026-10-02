using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Teachers
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class DetailsModel : PageModel
    {
        private readonly ApiClient _api;
        public DetailsModel(ApiClient api) { _api = api; }

        public UserDto? Teacher { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            try { Teacher = await _api.GetAsync<UserDto>($"api/users/{id}"); }
            catch { Teacher = null; }
        }
    }
}
