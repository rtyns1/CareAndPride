namespace CarePrideSystem.Domain.Entities
{
    public class Subject
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Subject() { }

        internal Subject(Guid id, string name, string code, string? description, bool isActive, DateTime createdAtUtc)
        {
            Id = id;
            Name = name;
            Code = code;
            Description = description;
            IsActive = isActive;
            CreatedAtUtc = createdAtUtc;
        }

        public void UpdateDetails(string name, string code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Subject name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Subject code is required.", nameof(code));

            Name = name;
            Code = code;
            Description = description;
        }

        public void Activate()
        {
            if (IsActive) throw new InvalidOperationException("Subject is already active.");
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive) throw new InvalidOperationException("Subject is already inactive.");
            IsActive = false;
        }
    }
}
