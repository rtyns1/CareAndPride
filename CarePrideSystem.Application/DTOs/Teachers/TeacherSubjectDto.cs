namespace CarePrideSystem.Application.DTOs.Teachers
{
    public class TeacherSubjectDto
    {
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid AcademicYearId { get; set; }
        public DateTime AssignedAtUtc { get; set; }
    }
}
