using System.Security.Claims;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarePrideSystem.Web.Pages
{
    [Authorize(Roles = "Admin,Secretary,Teacher")]
    public class MyClassesModel : PageModel
    {
        private readonly ApiClient _api;
        public MyClassesModel(ApiClient api) { _api = api; }

        public ClassTeacherInfo? ClassTeacherOf { get; set; }
        public List<SubjectAssignment> Subjects { get; set; } = new();
        public List<StudentDto> ClassStudents { get; set; } = new();

        public class ClassTeacherInfo
        {
            public Guid ClassId { get; set; }
            public string ClassName { get; set; } = "";
        }
        public class SubjectAssignment
        {
            public string SubjectName { get; set; } = "";
            public string SubjectCode { get; set; } = "";
            public string ClassName { get; set; } = "";
        }

        public async Task OnGetAsync()
        {
            var userIdStr = User.FindFirst("user_id")?.Value;
            if (!Guid.TryParse(userIdStr, out var userId)) return;

            var classes = await _api.GetAsync<List<ClassDto>>("api/classes") ?? new();
            var subjects = await _api.GetAsync<List<SubjectDto>>("api/subjects") ?? new();

            try
            {
                var cts = await _api.GetAsync<List<ClassTeacherDto>>($"api/teacherassignments/teacher/{userId}/classes") ?? new();
                var primary = cts.FirstOrDefault(c => c.IsPrimary);
                if (primary != null)
                {
                    var cls = classes.FirstOrDefault(c => c.Id == primary.ClassId);
                    if (cls != null)
                    {
                        ClassTeacherOf = new ClassTeacherInfo { ClassId = cls.Id, ClassName = cls.Name };
                        ClassStudents = await _api.GetAsync<List<StudentDto>>($"api/students/class/{cls.Id}") ?? new();
                    }
                }
            }
            catch { }

            try
            {
                var tss = await _api.GetAsync<List<TeacherSubjectDto>>($"api/teacherassignments/teacher/{userId}/subjects") ?? new();
                foreach (var ts in tss)
                {
                    var subj = subjects.FirstOrDefault(s => s.Id == ts.SubjectId);
                    var cls = classes.FirstOrDefault(c => c.Id == ts.ClassId);
                    Subjects.Add(new SubjectAssignment
                    {
                        SubjectName = subj?.Name ?? "Unknown",
                        SubjectCode = subj?.Code ?? "—",
                        ClassName = cls?.Name ?? "—"
                    });
                }
                Subjects = Subjects.OrderBy(s => s.ClassName).ThenBy(s => s.SubjectName).ToList();
            }
            catch { }
        }
    }
}
