namespace CarePrideSystem.Domain.Entities
{
    public class Class
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int GradeLevel { get; private set; }
        public int Capacity { get; private set; }
        public Guid AcademicYearId { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Class() { }

        internal Class(Guid id, string name, int gradeLevel, int capacity,
                       Guid academicYearId, bool isActive, DateTime createdAtUtc)
        {
            Id = id;
            Name = name;
            GradeLevel = gradeLevel;
            Capacity = capacity;
            AcademicYearId = academicYearId;
            IsActive = isActive;
            CreatedAtUtc = createdAtUtc;
        }

        public void UpdateDetails(string name, int gradeLevel, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Class name is required.", nameof(name));
            if (gradeLevel < 0 || gradeLevel > 13)
                throw new ArgumentException("Grade level must be between 0 and 13.", nameof(gradeLevel));
            if (capacity < 1)
                throw new ArgumentException("Capacity must be at least 1.", nameof(capacity));

            Name = name;
            GradeLevel = gradeLevel;
            Capacity = capacity;
        }

        public void Activate()
        {
            if (IsActive) throw new InvalidOperationException("Class is already active.");
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive) throw new InvalidOperationException("Class is already inactive.");
            IsActive = false;
        }
    }
}
