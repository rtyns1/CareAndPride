using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Services
{
    public static class AssignmentCreationService
    {
        public static Assignment CreateAssignment(
            string title, string? description, string examType,
            Guid subjectId, Guid classId, Guid teacherId, Guid academicYearId,
            DateTime dueDateUtc, decimal maxScore,
            string? filePath = null, string? originalFileName = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
            if (subjectId == Guid.Empty) throw new ArgumentException("Subject is required.");
            if (classId == Guid.Empty) throw new ArgumentException("Class is required.");
            if (teacherId == Guid.Empty) throw new ArgumentException("Teacher is required.");
            if (academicYearId == Guid.Empty) throw new ArgumentException("Academic year is required.");
            if (maxScore <= 0) maxScore = 100;
            if (string.IsNullOrWhiteSpace(examType)) examType = "Assignment";

            return new Assignment(
                Guid.NewGuid(), title.Trim(),
                string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                examType,
                subjectId, classId, teacherId, academicYearId,
                dueDateUtc, maxScore, filePath, originalFileName, DateTime.UtcNow);
        }
    }
}
