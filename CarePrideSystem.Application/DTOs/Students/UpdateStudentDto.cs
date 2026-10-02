namespace CarePrideSystem.Application.DTOs.Students
{
    public class UpdateStudentDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? MedicalConditions { get; set; }
    }
}
