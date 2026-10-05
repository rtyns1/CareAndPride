using CarePrideSystem.Application.DTOs.Assignments;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Classes
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class DetailsModel : PageModel
    {
        private readonly ApiClient _api;
        public DetailsModel(ApiClient api) { _api = api; }

        public ClassDto? Class { get; set; }
        public List<StudentDto> Students { get; set; } = new();
        public List<ClassTeacherDto> ClassTeachers { get; set; } = new();
        public List<AssignmentDto> Assignments { get; set; } = new();
        public List<SubjectDto> Subjects { get; set; } = new();

        public async Task OnGetAsync(Guid id)
        {
            try
            {
                Class = await _api.GetAsync<ClassDto>($"api/classes/{id}");
                Students = await _api.GetAsync<List<StudentDto>>($"api/students/class/{id}") ?? new();
                try { ClassTeachers = await _api.GetAsync<List<ClassTeacherDto>>($"api/teacherassignments/classes/{id}/teachers") ?? new(); } catch { }
                try { Assignments = await _api.GetAsync<List<AssignmentDto>>($"api/assignments/class/{id}") ?? new(); } catch { }
                try { Subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new(); } catch { }
            }
            catch { }
        }
    }
}
