using MediatR;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Grades.GradesCommands
{
    public class CreateGradeCommandHandler : IRequestHandler<CreateGradeCommand, Guid>
    {
        private readonly IGradeRepository _repo;
        public CreateGradeCommandHandler(IGradeRepository repo) { _repo = repo; }

        public async Task<Guid> Handle(CreateGradeCommand r, CancellationToken ct)
        {
            var g = GradeCreationService.CreateGrade(
                r.StudentId, r.SubjectId, r.ClassId, r.TeacherId, r.AcademicYearId,
                r.AssessmentType, r.Title, r.Score, r.MaxScore, r.Remarks);
            await _repo.AddAsync(g);
            return g.Id;
        }
    }
}
