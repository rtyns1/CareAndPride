using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Services
{
    public static class ClassCreationService
    {
        public static Class CreateClass(string name, int gradeLevel, int capacity, Guid academicYearId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Class name is required.", nameof(name));
            if (gradeLevel < 0 || gradeLevel > 13)
                throw new ArgumentException("Grade level must be between 0 and 13.", nameof(gradeLevel));
            if (capacity < 1)
                throw new ArgumentException("Capacity must be at least 1.", nameof(capacity));
            if (academicYearId == Guid.Empty)
                throw new ArgumentException("Academic year ID is required.", nameof(academicYearId));

            return new Class(
                id: Guid.NewGuid(),
                name: name,
                gradeLevel: gradeLevel,
                capacity: capacity,
                academicYearId: academicYearId,
                isActive: true,
                createdAtUtc: DateTime.UtcNow
            );
        }
    }
}
