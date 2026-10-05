using MediatR;
using CarePrideSystem.Application.DTOs.Assignments;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Assignments.Queries
{
    public class GetAssignmentByIdQueryHandler : IRequestHandler<GetAssignmentByIdQuery, AssignmentDto?>
    {
        private readonly IAssignmentRepository _repo;
        public GetAssignmentByIdQueryHandler(IAssignmentRepository repo) { _repo = repo; }

        public async Task<AssignmentDto?> Handle(GetAssignmentByIdQuery request, CancellationToken ct)
        {
            var a = await _repo.GetByIdAsync(request.Id);
            if (a == null) return null;
            return new AssignmentDto
            {
                Id = a.Id, Title = a.Title, Description = a.Description,
                ExamType = a.ExamType,
                SubjectId = a.SubjectId, ClassId = a.ClassId, TeacherId = a.TeacherId,
                DueDateUtc = a.DueDateUtc, MaxScore = a.MaxScore,
                FilePath = a.FilePath, OriginalFileName = a.OriginalFileName,
                HasFile = !string.IsNullOrEmpty(a.FilePath),
                CreatedAtUtc = a.CreatedAtUtc
            };
        }
    }
}
