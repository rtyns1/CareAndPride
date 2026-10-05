namespace CarePrideSystem.Domain.Entities
{
    public class Grade
    {
        public Guid Id { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid SubjectId { get; private set; }
        public Guid ClassId { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public Guid RecordedByTeacherId { get; private set; }
        public Guid? AssignmentId { get; private set; }
        public string ExamType { get; private set; } = string.Empty;
        public string Term { get; private set; } = string.Empty;
        public decimal Score { get; private set; }
        public decimal MaxScore { get; private set; }
        public string GradeLetter { get; private set; } = string.Empty;
        public string? Remarks { get; private set; }
        public DateTime RecordedAtUtc { get; private set; }

        private Grade() { }

        internal Grade(
            Guid id, Guid studentId, Guid subjectId, Guid classId,
            Guid academicYearId, Guid recordedByTeacherId, Guid? assignmentId,
            string examType, string term, decimal score, decimal maxScore,
            string gradeLetter, string? remarks, DateTime recordedAtUtc)
        {
            Id = id;
            StudentId = studentId;
            SubjectId = subjectId;
            ClassId = classId;
            AcademicYearId = academicYearId;
            RecordedByTeacherId = recordedByTeacherId;
            AssignmentId = assignmentId;
            ExamType = examType;
            Term = term;
            Score = score;
            MaxScore = maxScore;
            GradeLetter = gradeLetter;
            Remarks = remarks;
            RecordedAtUtc = recordedAtUtc;
        }
    }
}
