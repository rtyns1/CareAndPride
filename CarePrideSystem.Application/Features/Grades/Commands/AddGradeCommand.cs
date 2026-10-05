using MediatR;

namespace CarePrideSystem.Application.Features.Grades.Commands
{
    public class AddGradeCommand : IRequest<Guid>
    {
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid RecordedByTeacherId { get; set; }
        public Guid? AssignmentId { get; set; }
        public string ExamType { get; set; } = "Assignment";
        public string Term { get; set; } = "Term 1";
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; } = 100;
        public string? Remarks { get; set; }
    }
}
