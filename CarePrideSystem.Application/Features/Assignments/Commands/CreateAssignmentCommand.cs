using MediatR;

namespace CarePrideSystem.Application.Features.Assignments.Commands
{
    public class CreateAssignmentCommand : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ExamType { get; set; } = "Assignment";
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid TeacherId { get; set; }
        public DateTime DueDateUtc { get; set; }
        public decimal MaxScore { get; set; } = 100;
        public byte[]? FileContent { get; set; }
        public string? FileName { get; set; }
    }
}
