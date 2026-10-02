using CarePrideSystem.Application.DTOs.Grades;

namespace CarePrideSystem.Application.DTOs.Students
{
    public class StudentDetailDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string AdmissionNumber { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = "";
        public string? MedicalConditions { get; set; }
        public bool IsArchived { get; set; }
        public List<GradeDto> Grades { get; set; } = new();
        public decimal AveragePercentage { get; set; }
        public string OverallGrade { get; set; } = "";
    }
}
