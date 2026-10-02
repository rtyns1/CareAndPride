namespace CarePrideSystem.Application.DTOs.Students
{
    public class CreateStudentDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string AdmissionNumber { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
        public string? MedicalConditions { get; set; }
    }
}
