using MediatR;

namespace CarePrideSystem.Application.Features.Grades.Commands
{
    public class AddGradeCommand : IRequest<Guid>
    {
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid RecordedByTeacherId { get; set; }
        public string ExamType { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string? Remarks { get; set; }
    }
}
