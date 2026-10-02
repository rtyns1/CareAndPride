namespace CarePrideSystem.Application.DTOs.Classes
{
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GradeLevel { get; set; }
        public int Capacity { get; set; }
        public Guid AcademicYearId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
