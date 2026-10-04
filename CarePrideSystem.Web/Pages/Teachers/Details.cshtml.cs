using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Teachers
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class DetailsModel : PageModel
    {
        private const string DefaultAcademicYear = "00000000-0000-0000-0000-000000000001";
        private static readonly Guid DefaultAcademicYearId = Guid.Parse(DefaultAcademicYear);

        private readonly ApiClient _api;
        public DetailsModel(ApiClient api) { _api = api; }

        public UserDto? Teacher { get; set; }
        public List<ClassDto> Classes { get; set; } = new();
        public List<SubjectDto> Subjects { get; set; } = new();
        public List<ClassTeacherDto> ClassAssignments { get; set; } = new();
        public List<TeacherSubjectDto> SubjectAssignments { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            await LoadAllAsync(id);
        }

        public async Task<IActionResult> OnPostAddClassTeacherAsync(Guid id, Guid classId, bool isPrimary)
        {
            if (classId == Guid.Empty)
            {
                ErrorMessage = "Please select a class.";
                await LoadAllAsync(id);
                return Page();
            }

            try
            {
                var body = new
                {
                    TeacherId = id,
                    ClassId = classId,
                    AcademicYearId = DefaultAcademicYearId,
                    IsPrimary = isPrimary
                };
                await _api.PostAsync("api/teacherassignments/classteachers", body);
                SuccessMessage = "Class teacher assignment created.";
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); }

            await LoadAllAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAddSubjectTeacherAsync(Guid id, Guid subjectId, Guid classId)
        {
            if (subjectId == Guid.Empty || classId == Guid.Empty)
            {
                ErrorMessage = "Please select both a subject and a class.";
                await LoadAllAsync(id);
                return Page();
            }

            try
            {
                var body = new
                {
                    TeacherId = id,
                    SubjectId = subjectId,
                    ClassId = classId,
                    AcademicYearId = DefaultAcademicYearId
                };
                await _api.PostAsync("api/teacherassignments/subjects", body);
                SuccessMessage = "Subject teacher assignment created.";
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); }

            await LoadAllAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveClassTeacherAsync(Guid id, Guid classId)
        {
            try
            {
                var url = $"api/teacherassignments/classteachers?teacherId={id}&classId={classId}&academicYearId={DefaultAcademicYearId}";
                await _api.DeleteAsync(url);
                SuccessMessage = "Class teacher assignment removed.";
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); }

            await LoadAllAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveSubjectAsync(Guid id, Guid subjectId, Guid classId)
        {
            try
            {
                var url = $"api/teacherassignments/subjects?teacherId={id}&subjectId={subjectId}&classId={classId}&academicYearId={DefaultAcademicYearId}";
                await _api.DeleteAsync(url);
                SuccessMessage = "Subject teacher assignment removed.";
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); }

            await LoadAllAsync(id);
            return Page();
        }

        private async Task LoadAllAsync(Guid id)
        {
            try { Teacher = await _api.GetAsync<UserDto>($"api/users/{id}"); }
            catch { Teacher = null; }

            try { Classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new(); }
            catch { }

            try { Subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new(); }
            catch { }

            try { ClassAssignments = await _api.GetAsync<List<ClassTeacherDto>>($"api/teacherassignments/teachers/{id}/classes") ?? new(); }
            catch { }

            try { SubjectAssignments = await _api.GetAsync<List<TeacherSubjectDto>>($"api/teacherassignments/teachers/{id}/subjects") ?? new(); }
            catch { }
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Request failed.";
            var idx = message.IndexOf("\"error\":\"");
            if (idx >= 0)
            {
                var start = idx + 9;
                var end = message.IndexOf('"', start);
                if (end > start) return message.Substring(start, end - start);
            }
            return message;
        }
    }
}
