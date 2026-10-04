using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Grades;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Students
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class DetailsModel : PageModel
    {
        private static readonly Guid DefaultYear = Guid.Parse("00000000-0000-0000-0000-000000000001");

        private readonly ApiClient _api;
        public DetailsModel(ApiClient api) { _api = api; }

        public StudentDto? Student { get; set; }
        public List<ClassDto> Classes { get; set; } = new();
        public List<SubjectDto> Subjects { get; set; } = new();
        public List<GradeDto> VisibleGrades { get; set; } = new();
        public List<SubjectDto> AllowedSubjectsForGrade { get; set; } = new();
        public bool CanAddGrade { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (!await LoadAsync(id)) return RedirectToPage("/Students/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAddGradeAsync(Guid id, Guid subjectId, string examType, string term,
            decimal score, decimal maxScore, string? remarks)
        {
            if (subjectId == Guid.Empty)
            {
                ErrorMessage = "Please select a subject.";
                await LoadAsync(id);
                return Page();
            }

            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                ErrorMessage = "Session invalid.";
                await LoadAsync(id);
                return Page();
            }

            try
            {
                if (maxScore <= 0) maxScore = 100;
                if (score < 0) score = 0;
                if (score > maxScore) score = maxScore;

                var body = new
                {
                    StudentId = id,
                    SubjectId = subjectId,
                    ClassId = Student!.ClassId,
                    RecordedByTeacherId = userId,
                    ExamType = examType,
                    Term = term,
                    Score = score,
                    MaxScore = maxScore,
                    Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks
                };
                await _api.PostAsync("api/grades", body);
                SuccessMessage = "Grade recorded.";
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); }

            await LoadAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _api.DeleteAsync($"api/students/{id}");
                return RedirectToPage("/Students/Index");
            }
            catch (HttpRequestException ex) { ErrorMessage = ExtractError(ex.Message); }

            await LoadAsync(id);
            return Page();
        }

        private async Task<bool> LoadAsync(Guid id)
        {
            try { Student = await _api.GetAsync<StudentDto>($"api/students/{id}"); }
            catch { Student = null; }
            if (Student == null) return false;

            try { Classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new(); } catch { }
            try { Subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new(); } catch { }

            var userIdClaim = User.FindFirst("user_id")?.Value;
            Guid.TryParse(userIdClaim, out var userId);

            var allGrades = new List<GradeDto>();
            try { allGrades = await _api.GetAsync<List<GradeDto>>($"api/grades/student/{id}") ?? new(); } catch { }

            // Admin / Secretary see everything
            if (User.IsInRole("Admin") || User.IsInRole("Secretary"))
            {
                VisibleGrades = allGrades;
                AllowedSubjectsForGrade = Subjects;
                CanAddGrade = true;
                return true;
            }

            // Teacher: figure out their assignments
            var classAssignments = new List<ClassTeacherDto>();
            var subjectAssignments = new List<TeacherSubjectDto>();
            try { classAssignments = await _api.GetAsync<List<ClassTeacherDto>>($"api/teacherassignments/teachers/{userId}/classes") ?? new(); } catch { }
            try { subjectAssignments = await _api.GetAsync<List<TeacherSubjectDto>>($"api/teacherassignments/teachers/{userId}/subjects") ?? new(); } catch { }

            var isClassTeacherOfThisStudent = classAssignments.Any(ct => ct.ClassId == Student.ClassId);

            // Subjects this teacher teaches in THIS student's class
            var subjectsTaughtHere = subjectAssignments
                .Where(ts => ts.ClassId == Student.ClassId)
                .Select(ts => ts.SubjectId)
                .Distinct()
                .ToList();

            if (isClassTeacherOfThisStudent)
            {
                // Full view: all grades for this student
                VisibleGrades = allGrades;
                AllowedSubjectsForGrade = Subjects.Where(s => subjectsTaughtHere.Contains(s.Id)).ToList();
                CanAddGrade = AllowedSubjectsForGrade.Count > 0;
            }
            else
            {
                // Subject teacher: only grades of the subjects they teach in this class
                VisibleGrades = allGrades.Where(g => subjectsTaughtHere.Contains(g.SubjectId)).ToList();
                AllowedSubjectsForGrade = Subjects.Where(s => subjectsTaughtHere.Contains(s.Id)).ToList();
                CanAddGrade = AllowedSubjectsForGrade.Count > 0;
            }

            return true;
        }

        private static string ExtractError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Request failed.";
            var idx = message.IndexOf("\"error\":\"");
            if (idx >= 0) { var s = idx + 9; var e = message.IndexOf('"', s); if (e > s) return message.Substring(s, e - s); }
            return message;
        }
    }
}
