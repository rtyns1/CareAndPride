namespace CarePrideSystem.Application.DTOs.Teachers
{
    public class AssignClassTeacherDto
    {
        public Guid TeacherId { get; set; }
        public Guid ClassId { get; set; }
        public Guid AcademicYearId { get; set; }
        public bool IsPrimary { get; set; }
    }
}
