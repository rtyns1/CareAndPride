namespace CarePrideSystem.Domain.Entities
{
    public class ClassTeacher
    {
        public Guid TeacherId { get; private set; }
        public Guid ClassId { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public bool IsPrimary { get; private set; }
        public DateTime AssignedAtUtc { get; private set; }

        private ClassTeacher() { }

        internal ClassTeacher(Guid teacherId, Guid classId, Guid academicYearId, bool isPrimary, DateTime assignedAtUtc)
        {
            TeacherId = teacherId;
            ClassId = classId;
            AcademicYearId = academicYearId;
            IsPrimary = isPrimary;
            AssignedAtUtc = assignedAtUtc;
        }

        public void SetAsPrimary()
        {
            if (IsPrimary) throw new InvalidOperationException("This teacher is already the primary class teacher.");
            IsPrimary = true;
        }

        public void SetAsSecondary()
        {
            if (!IsPrimary) throw new InvalidOperationException("This teacher is already a secondary class teacher.");
            IsPrimary = false;
        }
    }
}
