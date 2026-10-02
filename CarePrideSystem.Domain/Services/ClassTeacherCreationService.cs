using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Services
{
    public static class ClassTeacherCreationService
    {
        public static ClassTeacher CreateClassTeacher(
            Guid teacherId,
            Guid classId,
            Guid academicYearId,
            bool isPrimary = false)
        {
            if (teacherId == Guid.Empty)
                throw new ArgumentException("Teacher ID is required.", nameof(teacherId));

            if (classId == Guid.Empty)
                throw new ArgumentException("Class ID is required.", nameof(classId));

            if (academicYearId == Guid.Empty)
                throw new ArgumentException("Academic year ID is required.", nameof(academicYearId));

            return new ClassTeacher(
                teacherId: teacherId,
                classId: classId,
                academicYearId: academicYearId,
                isPrimary: isPrimary,
                assignedAtUtc: DateTime.UtcNow
            );
        }
    }
}