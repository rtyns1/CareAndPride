namespace CarePrideSystem.Application.DTOs.Grades
{
    public class CreateGradeDto
    {
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid AcademicYearId { get; set; }
        public string AssessmentType { get; set; } = "Exam";
        public string Title { get; set; } = "";
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; } = 100;
        public string? Remarks { get; set; }
    }
}
