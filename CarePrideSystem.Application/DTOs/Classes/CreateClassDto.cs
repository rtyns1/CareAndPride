namespace CarePrideSystem.Application.DTOs.Classes
{
    public class CreateClassDto
    {
        public string Name { get; set; } = string.Empty;
        public int GradeLevel { get; set; }
        public int Capacity { get; set; }
        public Guid AcademicYearId { get; set; }
    }
}
