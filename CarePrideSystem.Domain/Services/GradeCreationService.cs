using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Services
{
    public static class GradeCreationService
    {
        public static Grade CreateGrade(
            Guid studentId, Guid subjectId, Guid classId, Guid academicYearId,
            Guid recordedByTeacherId, string examType, string term,
            decimal score, decimal maxScore,
            string? remarks = null, Guid? assignmentId = null)
        {
            if (studentId == Guid.Empty) throw new ArgumentException("Student ID required.");
            if (subjectId == Guid.Empty) throw new ArgumentException("Subject ID required.");
            if (classId == Guid.Empty) throw new ArgumentException("Class ID required.");
            if (academicYearId == Guid.Empty) throw new ArgumentException("Academic year required.");
            if (recordedByTeacherId == Guid.Empty) throw new ArgumentException("Teacher ID required.");
            if (string.IsNullOrWhiteSpace(examType)) examType = "Assignment";
            if (string.IsNullOrWhiteSpace(term)) term = "Term 1";
            if (maxScore <= 0) maxScore = 100;
            if (score < 0) score = 0;
            if (score > maxScore) score = maxScore;

            var letter = ComputeGradeLetter(score, maxScore);

            return new Grade(
                Guid.NewGuid(), studentId, subjectId, classId, academicYearId,
                recordedByTeacherId, assignmentId, examType, term,
                score, maxScore, letter, remarks, DateTime.UtcNow);
        }

        public static string ComputeGradeLetter(decimal score, decimal maxScore)
        {
            var pct = maxScore == 0 ? 0 : (score / maxScore) * 100m;
            if (pct >= 80) return "A";
            if (pct >= 75) return "A-";
            if (pct >= 70) return "B+";
            if (pct >= 65) return "B";
            if (pct >= 60) return "B-";
            if (pct >= 55) return "C+";
            if (pct >= 50) return "C";
            if (pct >= 45) return "C-";
            if (pct >= 40) return "D+";
            if (pct >= 35) return "D";
            return "E";
        }
    }
}
