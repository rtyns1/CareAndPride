namespace CarePrideSystem.Domain.Entities
{
    public class Assignment
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string ExamType { get; private set; } = "Assignment";
        public Guid SubjectId { get; private set; }
        public Guid ClassId { get; private set; }
        public Guid TeacherId { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public DateTime DueDateUtc { get; private set; }
        public decimal MaxScore { get; private set; }
        public string? FilePath { get; private set; }
        public string? OriginalFileName { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Assignment() { }

        internal Assignment(
            Guid id, string title, string? description, string examType,
            Guid subjectId, Guid classId, Guid teacherId, Guid academicYearId,
            DateTime dueDateUtc, decimal maxScore,
            string? filePath, string? originalFileName, DateTime createdAtUtc)
        {
            Id = id;
            Title = title;
            Description = description;
            ExamType = string.IsNullOrWhiteSpace(examType) ? "Assignment" : examType;
            SubjectId = subjectId;
            ClassId = classId;
            TeacherId = teacherId;
            AcademicYearId = academicYearId;
            DueDateUtc = dueDateUtc;
            MaxScore = maxScore;
            FilePath = filePath;
            OriginalFileName = originalFileName;
            CreatedAtUtc = createdAtUtc;
        }
    }
}
