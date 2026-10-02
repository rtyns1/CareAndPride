using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Subjects
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly ApiClient _api;
        public IndexModel(ApiClient api) { _api = api; }

        public List<SubjectDto> Subjects { get; set; } = new();
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            try { Subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new(); }
            catch (Exception ex) { Error = ex.Message; }
        }
    }
}
