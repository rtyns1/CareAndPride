using System.Net.Http.Headers;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Assignments
{
    [Authorize(Roles = "Admin,Teacher")]
    public class CreateModel : PageModel
    {
        private readonly ApiClient _api;
        public CreateModel(ApiClient api) { _api = api; }

        [BindProperty] public string Title { get; set; } = "";
        [BindProperty] public string? Description { get; set; }
        [BindProperty] public string ExamType { get; set; } = "Assignment";
        [BindProperty] public Guid SubjectId { get; set; }
        [BindProperty] public Guid ClassId { get; set; }
        [BindProperty] public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(7);
        [BindProperty] public decimal MaxScore { get; set; } = 100;
        [BindProperty] public IFormFile? File { get; set; }

        public List<ClassDto> Classes { get; set; } = new();
        public List<SubjectDto> Subjects { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? classId)
        {
            if (classId.HasValue) ClassId = classId.Value;
            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadAsync();

            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorMessage = "Title is required.";
                return Page();
            }
            if (SubjectId == Guid.Empty)
            {
                ErrorMessage = "Select a subject.";
                return Page();
            }
            if (ClassId == Guid.Empty)
            {
                ErrorMessage = "Select a class.";
                return Page();
            }

            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var teacherId))
            {
                ErrorMessage = "Session invalid.";
                return Page();
            }

            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(Title.Trim()), "Title");
                content.Add(new StringContent(Description ?? ""), "Description");
                content.Add(new StringContent(ExamType), "ExamType");
                content.Add(new StringContent(SubjectId.ToString()), "SubjectId");
                content.Add(new StringContent(ClassId.ToString()), "ClassId");
                content.Add(new StringContent(teacherId.ToString()), "TeacherId");
                content.Add(new StringContent(DueDate.ToUniversalTime().ToString("o")), "DueDateUtc");
                content.Add(new StringContent(MaxScore.ToString(System.Globalization.CultureInfo.InvariantCulture)), "MaxScore");

                if (File != null && File.Length > 0)
                {
                    var stream = File.OpenReadStream();
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(
                        string.IsNullOrEmpty(File.ContentType) ? "application/octet-stream" : File.ContentType);
                    content.Add(fileContent, "File", File.FileName);
                }

                await _api.PostMultipartAsync("api/assignments", content);
                return RedirectToPage("/Classes/Details", new { id = ClassId });
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); }
            catch (Exception ex) { ErrorMessage = ex.Message; }

            return Page();
        }

        private async Task LoadAsync()
        {
            try { Classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new(); } catch { }
            try { Subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new(); } catch { }

            if (User.IsInRole("Admin")) return;

            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return;

            var teacherSubjects = new List<TeacherSubjectDto>();
            var classAssignments = new List<ClassTeacherDto>();
            try { teacherSubjects = await _api.GetAsync<List<TeacherSubjectDto>>($"api/teacherassignments/teachers/{userId}/subjects") ?? new(); } catch { }
            try { classAssignments = await _api.GetAsync<List<ClassTeacherDto>>($"api/teacherassignments/teachers/{userId}/classes") ?? new(); } catch { }

            if (ClassId != Guid.Empty)
            {
                var subjectsTaughtHere = teacherSubjects
                    .Where(ts => ts.ClassId == ClassId)
                    .Select(ts => ts.SubjectId)
                    .Distinct()
                    .ToList();

                // Class teachers see all subjects for their class.
                var isClassTeacherHere = classAssignments.Any(ct => ct.ClassId == ClassId);
                if (!isClassTeacherHere)
                    Subjects = Subjects.Where(s => subjectsTaughtHere.Contains(s.Id)).ToList();
            }
            else
            {
                var allowedClassIds = teacherSubjects.Select(ts => ts.ClassId)
                    .Concat(classAssignments.Select(ct => ct.ClassId))
                    .Distinct()
                    .ToList();
                Classes = Classes.Where(c => allowedClassIds.Contains(c.Id)).ToList();
            }
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Upload failed.";
            var idx = message.IndexOf("\"error\":\"");
            if (idx >= 0) { var s = idx + 9; var e = message.IndexOf('"', s); if (e > s) return message.Substring(s, e - s); }
            return message;
        }
    }
}
