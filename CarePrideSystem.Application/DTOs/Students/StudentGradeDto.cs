namespace CarePrideSystem.Application.DTOs.Students
{
    public class StudentGradeDto
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public string AssessmentType { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string GradeLetter { get; set; } = string.Empty;
    }
}
