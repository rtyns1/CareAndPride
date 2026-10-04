using MediatR;
using CarePrideSystem.Application.DTOs.Grades;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Grades.Queries
{
    public class GetGradesByStudentQueryHandler : IRequestHandler<GetGradesByStudentQuery, IEnumerable<GradeDto>>
    {
        private readonly IGradeRepository _repo;
        public GetGradesByStudentQueryHandler(IGradeRepository repo) { _repo = repo; }

        public async Task<IEnumerable<GradeDto>> Handle(GetGradesByStudentQuery request, CancellationToken ct)
        {
            var list = await _repo.GetByStudentIdAsync(request.StudentId);
            return list.Select(g => new GradeDto
            {
                Id = g.Id, StudentId = g.StudentId, SubjectId = g.SubjectId,
                ClassId = g.ClassId, RecordedByTeacherId = g.RecordedByTeacherId,
                ExamType = g.ExamType, Term = g.Term,
                Score = g.Score, MaxScore = g.MaxScore,
                GradeLetter = g.GradeLetter, Remarks = g.Remarks,
                RecordedAtUtc = g.RecordedAtUtc
            });
        }
    }
}
