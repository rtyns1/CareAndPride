using MediatR;
using CarePrideSystem.Application.DTOs.Assignments;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Assignments.Queries
{
    public class GetAssignmentsByClassQueryHandler : IRequestHandler<GetAssignmentsByClassQuery, IEnumerable<AssignmentDto>>
    {
        private readonly IAssignmentRepository _repo;
        public GetAssignmentsByClassQueryHandler(IAssignmentRepository repo) { _repo = repo; }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetAssignmentsByClassQuery request, CancellationToken ct)
        {
            var list = await _repo.GetByClassIdAsync(request.ClassId);
            return list.Select(a => new AssignmentDto
            {
                Id = a.Id, Title = a.Title, Description = a.Description,
                ExamType = a.ExamType,
                SubjectId = a.SubjectId, ClassId = a.ClassId, TeacherId = a.TeacherId,
                DueDateUtc = a.DueDateUtc, MaxScore = a.MaxScore,
                FilePath = a.FilePath, OriginalFileName = a.OriginalFileName,
                HasFile = !string.IsNullOrEmpty(a.FilePath),
                CreatedAtUtc = a.CreatedAtUtc
            });
        }
    }
}
