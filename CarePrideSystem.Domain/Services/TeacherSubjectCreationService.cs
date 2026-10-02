using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Services
{
    public static class TeacherSubjectCreationService
    {
        public static TeacherSubject CreateTeacherSubject(
            Guid teacherId,
            Guid subjectId,
            Guid classId,
            Guid academicYearId)
        {
            if (teacherId == Guid.Empty)
                throw new ArgumentException("Teacher ID is required.", nameof(teacherId));

            if (subjectId == Guid.Empty)
                throw new ArgumentException("Subject ID is required.", nameof(subjectId));

            if (classId == Guid.Empty)
                throw new ArgumentException("Class ID is required.", nameof(classId));

            if (academicYearId == Guid.Empty)
                throw new ArgumentException("Academic year ID is required.", nameof(academicYearId));

            return new TeacherSubject(
                teacherId: teacherId,
                subjectId: subjectId,
                classId: classId,
                academicYearId: academicYearId,
                assignedAtUtc: DateTime.UtcNow
            );
        }
    }
}