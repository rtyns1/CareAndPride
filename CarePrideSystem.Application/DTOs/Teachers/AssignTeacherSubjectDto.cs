namespace CarePrideSystem.Application.DTOs.Teachers
{
    public class AssignTeacherSubjectDto
    {
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid AcademicYearId { get; set; }
    }
}
