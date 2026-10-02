namespace CarePrideSystem.Domain.Entities
{
    public class Submission
    {
        public Guid Id { get; private set; }
        public Guid AssignmentId { get; private set; }
        public Guid StudentId { get; private set; }
        public DateTime SubmittedAtUtc { get; private set; }
        public decimal? Score { get; private set; }
        public string? Feedback { get; private set; }
        public bool IsLate { get; private set; }

        private Submission() { }

        internal Submission(Guid id, Guid assignmentId, Guid studentId, DateTime submittedAtUtc,
                            decimal? score, string? feedback, bool isLate)
        {
            Id = id;
            AssignmentId = assignmentId;
            StudentId = studentId;
            SubmittedAtUtc = submittedAtUtc;
            Score = score;
            Feedback = feedback;
            IsLate = isLate;
        }
    }
}
