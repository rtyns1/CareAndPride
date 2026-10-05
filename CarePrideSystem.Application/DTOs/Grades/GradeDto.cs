namespace CarePrideSystem.Application.DTOs.Grades
{
    public class GradeDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid RecordedByTeacherId { get; set; }
        public Guid? AssignmentId { get; set; }
        public string ExamType { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string GradeLetter { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public DateTime RecordedAtUtc { get; set; }
    }
}
