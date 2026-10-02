using MediatR;
using CarePrideSystem.Application.DTOs.Grades;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Grades.Queries
{
    public class GetGradesByStudentQueryHandler : IRequestHandler<GetGradesByStudentQuery, IEnumerable<GradeDto>>
    {
        private readonly IGradeRepository _grades;
        private readonly ISubjectRepository _subjects;
        private readonly IClassRepository _classes;
        private readonly IStudentRepository _students;

        public GetGradesByStudentQueryHandler(IGradeRepository grades, ISubjectRepository subjects,
                                              IClassRepository classes, IStudentRepository students)
        {
            _grades = grades; _subjects = subjects; _classes = classes; _students = students;
        }

        public async Task<IEnumerable<GradeDto>> Handle(GetGradesByStudentQuery r, CancellationToken ct)
        {
            var grades = await _grades.GetByStudentIdAsync(r.StudentId);
            var student = await _students.GetByIdAsync(r.StudentId);
            var subjects = (await _subjects.GetAllAsync()).ToDictionary(s => s.Id, s => s.Name);
            var classes = (await _classes.GetAllAsync()).ToDictionary(c => c.Id, c => c.Name);

            return grades.Select(g => new GradeDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                StudentName = student != null ? $"{student.FirstName} {student.LastName}" : "",
                SubjectId = g.SubjectId,
                SubjectName = subjects.TryGetValue(g.SubjectId, out var sn) ? sn : "",
                ClassId = g.ClassId,
                ClassName = classes.TryGetValue(g.ClassId, out var cn) ? cn : "",
                ExamType = g.ExamType,
                Title = g.ExamType,
                Score = g.Score,
                MaxScore = g.MaxScore,
                GradeLetter = g.GradeLetter,
                Remarks = g.Remarks,
                RecordedAtUtc = g.RecordedAtUtc
            }).OrderByDescending(g => g.RecordedAtUtc).ToList();
        }
    }
}

