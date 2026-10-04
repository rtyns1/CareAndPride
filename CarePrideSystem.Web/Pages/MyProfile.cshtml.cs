using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages
{
    [Authorize]
    public class MyProfileModel : PageModel
    {
        private readonly ApiClient _api;
        public MyProfileModel(ApiClient api) { _api = api; }

        public UserDto? CurrentUser { get; set; }
        public List<ClassDto> Classes { get; set; } = new();
        public List<SubjectDto> Subjects { get; set; } = new();
        public List<ClassTeacherDto> ClassAssignments { get; set; } = new();
        public List<TeacherSubjectDto> SubjectAssignments { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return;

            try { CurrentUser = await _api.GetAsync<UserDto>($"api/users/{userId}"); } catch { }
            try { Classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new(); } catch { }
            try { Subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new(); } catch { }
            try { ClassAssignments = await _api.GetAsync<List<ClassTeacherDto>>($"api/teacherassignments/teachers/{userId}/classes") ?? new(); } catch { }
            try { SubjectAssignments = await _api.GetAsync<List<TeacherSubjectDto>>($"api/teacherassignments/teachers/{userId}/subjects") ?? new(); } catch { }
        }
    }
}
