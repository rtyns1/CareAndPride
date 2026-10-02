namespace CarePrideSystem.Application.DTOs.Teachers
{
    public class ClassTeacherDto
    {
        public Guid TeacherId { get; set; }
        public Guid ClassId { get; set; }
        public Guid AcademicYearId { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime AssignedAtUtc { get; set; }
    }
}
