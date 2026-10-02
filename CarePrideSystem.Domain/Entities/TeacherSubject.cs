namespace CarePrideSystem.Domain.Entities
{
    public class TeacherSubject
    {
        public Guid TeacherId { get; private set; }
        public Guid SubjectId { get; private set; }
        public Guid ClassId { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public DateTime AssignedAtUtc { get; private set; }

        private TeacherSubject() { }

        internal TeacherSubject(Guid teacherId, Guid subjectId, Guid classId, Guid academicYearId, DateTime assignedAtUtc)
        {
            TeacherId = teacherId;
            SubjectId = subjectId;
            ClassId = classId;
            AcademicYearId = academicYearId;
            AssignedAtUtc = assignedAtUtc;
        }
    }
}
