using MediatR;
using CarePrideSystem.Application.DTOs.Grades;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetStudentDetailQueryHandler : IRequestHandler<GetStudentDetailQuery, StudentDetailDto?>
    {
        private readonly IStudentRepository _students;
        private readonly IGradeRepository _grades;
        private readonly ISubjectRepository _subjects;
        private readonly IClassRepository _classes;

        public GetStudentDetailQueryHandler(IStudentRepository students, IGradeRepository grades,
                                            ISubjectRepository subjects, IClassRepository classes)
        {
            _students = students; _grades = grades; _subjects = subjects; _classes = classes;
        }

        public async Task<StudentDetailDto?> Handle(GetStudentDetailQuery r, CancellationToken ct)
        {
            var s = await _students.GetByIdAsync(r.StudentId);
            if (s == null) return null;

            var cls = await _classes.GetByIdAsync(s.ClassId);
            var grades = (await _grades.GetByStudentIdAsync(r.StudentId)).ToList();
            var subjects = (await _subjects.GetAllAsync()).ToDictionary(x => x.Id, x => x.Name);

            var gradeDtos = grades.Select(g => new GradeDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                StudentName = $"{s.FirstName} {s.LastName}",
                SubjectId = g.SubjectId,
                SubjectName = subjects.TryGetValue(g.SubjectId, out var sn) ? sn : "",
                ClassId = g.ClassId,
                ClassName = cls?.Name ?? "",
                ExamType = g.ExamType,
                Title = g.ExamType,
                Score = g.Score,
                MaxScore = g.MaxScore,
                GradeLetter = g.GradeLetter,
                Remarks = g.Remarks,
                RecordedAtUtc = g.RecordedAtUtc
            }).OrderByDescending(g => g.RecordedAtUtc).ToList();

            decimal avgPct = 0;
            string overall = "—";
            if (grades.Any())
            {
                var totalScore = grades.Sum(g => g.Score);
                var totalMax = grades.Sum(g => g.MaxScore);
                if (totalMax > 0)
                {
                    avgPct = Math.Round((totalScore / totalMax) * 100m, 2);
                    overall = avgPct switch
                    {
                        >= 90 => "A*", >= 80 => "A", >= 70 => "B", >= 60 => "C",
                        >= 50 => "D", >= 40 => "E", _ => "U"
                    };
                }
            }

            return new StudentDetailDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                AdmissionNumber = s.AdmissionNumber,
                DateOfBirth = s.DateOfBirth,
                ClassId = s.ClassId,
                ClassName = cls?.Name ?? "",
                MedicalConditions = s.MedicalConditions,
                IsArchived = s.IsArchived,
                Grades = gradeDtos,
                AveragePercentage = avgPct,
                OverallGrade = overall
            };
        }
    }
}

