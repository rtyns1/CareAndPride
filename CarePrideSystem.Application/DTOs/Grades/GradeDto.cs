namespace CarePrideSystem.Application.DTOs.Grades
{
    public class GradeDto
    {
        public string Term { get; set; } = string.Empty;
        public string ExamType { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = "";
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = "";
        public string AssessmentType { get; set; } = "";
        public string Title { get; set; } = "";
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string GradeLetter { get; set; } = "";
        public string? Remarks { get; set; }
        public DateTime RecordedAtUtc { get; set; }
    }
}

