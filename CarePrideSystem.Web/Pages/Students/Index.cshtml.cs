using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Students
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly ApiClient _api;
        public IndexModel(ApiClient api) { _api = api; }

        public List<StudentDto> Students { get; set; } = new();
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            try { Students = await _api.GetAsync<List<StudentDto>>("api/students") ?? new(); }
            catch (Exception ex) { Error = ex.Message; }
        }
    }
}
