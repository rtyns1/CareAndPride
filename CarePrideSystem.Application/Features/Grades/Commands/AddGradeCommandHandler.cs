using MediatR;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Grades.Commands
{
    public class AddGradeCommandHandler : IRequestHandler<AddGradeCommand, Guid>
    {
        private readonly IGradeRepository _repo;
        public AddGradeCommandHandler(IGradeRepository repo) { _repo = repo; }

        public async Task<Guid> Handle(AddGradeCommand request, CancellationToken ct)
        {
            var grade = GradeCreationService.CreateGrade(
                request.StudentId, request.SubjectId, request.ClassId,
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                request.RecordedByTeacherId, request.ExamType, request.Term,
                request.Score, request.MaxScore, request.Remarks, request.AssignmentId);

            await _repo.AddAsync(grade);
            return grade.Id;
        }
    }
}
