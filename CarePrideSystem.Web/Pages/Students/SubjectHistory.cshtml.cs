using CarePrideSystem.Application.DTOs.Grades;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Students
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class SubjectHistoryModel : PageModel
    {
        private readonly ApiClient _api;
        public SubjectHistoryModel(ApiClient api) { _api = api; }

        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public string StudentName { get; set; } = "";
        public string SubjectName { get; set; } = "";
        public List<GradeDto> Grades { get; set; } = new();
        public decimal Average { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync(Guid id, Guid subjectId)
        {
            StudentId = id;
            SubjectId = subjectId;

            try
            {
                var student = await _api.GetAsync<StudentDto>($"api/students/{id}");
                if (student != null) StudentName = $"{student.FirstName} {student.LastName}";
            }
            catch { }

            try
            {
                var subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects");
                var subject = subjects?.FirstOrDefault(s => s.Id == subjectId);
                if (subject != null) SubjectName = subject.Name;
            }
            catch { }

            try
            {
                var all = await _api.GetAsync<List<GradeDto>>($"api/grades/student/{id}") ?? new();
                Grades = all.Where(g => g.SubjectId == subjectId).ToList();
                if (Grades.Count > 0)
                {
                    Average = Grades.Average(g => (g.Score / g.MaxScore) * 100m);
                }
            }
            catch (Exception ex) { ErrorMessage = ex.Message; }
        }
    }
}
