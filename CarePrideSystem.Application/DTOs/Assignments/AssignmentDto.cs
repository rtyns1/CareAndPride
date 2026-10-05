namespace CarePrideSystem.Application.DTOs.Assignments
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ExamType { get; set; } = "Assignment";
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public DateTime DueDateUtc { get; set; }
        public decimal MaxScore { get; set; }
        public string? FilePath { get; set; }
        public string? OriginalFileName { get; set; }
        public bool HasFile { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
