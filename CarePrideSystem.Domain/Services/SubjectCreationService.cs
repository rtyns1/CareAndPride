using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Services
{
    public static class SubjectCreationService
    {
        public static Subject CreateSubject(
            string name,
            string code,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Subject name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Subject code is required.", nameof(code));

            return new Subject(
                id: Guid.NewGuid(),
                name: name,
                code: code,
                description: description,
                isActive: true,
                createdAtUtc: DateTime.UtcNow
            );
        }
    }
}