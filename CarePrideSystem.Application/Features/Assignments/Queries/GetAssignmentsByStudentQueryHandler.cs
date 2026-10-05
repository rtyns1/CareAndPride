using MediatR;
using CarePrideSystem.Application.DTOs.Assignments;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Assignments.Queries
{
    public class GetAssignmentsByStudentQueryHandler : IRequestHandler<GetAssignmentsByStudentQuery, IEnumerable<AssignmentDto>>
    {
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IStudentRepository _studentRepo;

        public GetAssignmentsByStudentQueryHandler(IAssignmentRepository assignmentRepo, IStudentRepository studentRepo)
        {
            _assignmentRepo = assignmentRepo;
            _studentRepo = studentRepo;
        }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetAssignmentsByStudentQuery request, CancellationToken ct)
        {
            var student = await _studentRepo.GetByIdAsync(request.StudentId);
            if (student == null) return Enumerable.Empty<AssignmentDto>();

            var list = await _assignmentRepo.GetByClassIdAsync(student.ClassId);
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
