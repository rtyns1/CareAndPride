using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Classes
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly ApiClient _api;
        public IndexModel(ApiClient api) { _api = api; }

        public List<ClassDto> Classes { get; set; } = new();
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            try { Classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new(); }
            catch (Exception ex) { Error = ex.Message; }
        }
    }
}
