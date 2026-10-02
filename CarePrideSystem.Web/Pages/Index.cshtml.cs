using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Domain.Enums;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApiClient _api;
        public IndexModel(ApiClient api) { _api = api; }

        public int StudentCount { get; set; }
        public int ClassCount { get; set; }
        public int SubjectCount { get; set; }
        public int TeacherCount { get; set; }

        public async Task OnGetAsync()
        {
            try { StudentCount = (await _api.GetAsync<List<StudentDto>>("api/students"))?.Count ?? 0; } catch { }
            try { ClassCount = (await _api.GetAsync<List<ClassDto>>("api/classes"))?.Count ?? 0; } catch { }
            try { SubjectCount = (await _api.GetAsync<List<SubjectDto>>("api/subjects"))?.Count ?? 0; } catch { }
            try
            {
                var users = await _api.GetAsync<List<UserDto>>("api/users");
                if (users != null) TeacherCount = users.Count(u => u.Role == UserRole.Teacher);
            }
            catch { }
        }
    }
}
