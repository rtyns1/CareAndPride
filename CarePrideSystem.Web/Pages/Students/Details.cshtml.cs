using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages.Students
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class DetailsModel : PageModel
    {
        private readonly ApiClient _api;
        public DetailsModel(ApiClient api) { _api = api; }

        public StudentDto? Student { get; set; }
        public string ClassName { get; set; } = "—";
        public List<StudentGradeDto> Grades { get; set; } = new();

        public async Task OnGetAsync(Guid id)
        {
            try { Student = await _api.GetAsync<StudentDto>($"api/students/{id}"); }
            catch { Student = null; return; }

            try
            {
                if (Student != null)
                {
                    var classes = await _api.GetAsync<List<ClassDto>>("api/classes");
                    var cls = classes?.FirstOrDefault(c => c.Id == Student.ClassId);
                    if (cls != null) ClassName = cls.Name;
                }
            }
            catch { }

            try
            {
                var subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new();
                var rawGrades = await _api.GetAsync<List<RawGrade>>($"api/students/{id}/grades") ?? new();

                Grades = rawGrades.Select(g =>
                {
                    var subj = subjects.FirstOrDefault(s => s.Id == g.SubjectId);
                    return new StudentGradeDto
                    {
                        SubjectId = g.SubjectId,
                        SubjectName = subj?.Name ?? "Unknown",
                        SubjectCode = subj?.Code ?? "—",
                        Term = g.Term,
                        AssessmentType = g.AssessmentType,
                        Score = g.Score,
                        MaxScore = g.MaxScore,
                        GradeLetter = ComputeLetter(g.Score, g.MaxScore)
                    };
                }).OrderBy(g => g.SubjectName).ThenBy(g => g.Term).ToList();
            }
            catch { }
        }

        private static string ComputeLetter(decimal score, decimal max)
        {
            if (max <= 0) return "—";
            var pct = score / max * 100;
            if (pct >= 80) return "A";
            if (pct >= 70) return "B";
            if (pct >= 60) return "C";
            if (pct >= 50) return "D";
            return "E";
        }

        public class RawGrade
        {
            public Guid SubjectId { get; set; }
            public string Term { get; set; } = "";
            public string AssessmentType { get; set; } = "";
            public decimal Score { get; set; }
            public decimal MaxScore { get; set; }
        }
    }
}
