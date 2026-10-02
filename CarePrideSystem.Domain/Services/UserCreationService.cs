
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Enums;


namespace CarePrideSystem.Domain.Services
{
    public static class UserCreationService
    {
        public static User CreateUser(
            string username,
            string email,
            string fullName,
            UserRole role)
        {
            // Business rule validation
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.", nameof(username));

            if (!email.Contains('@'))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required.", nameof(fullName));

            // Role-specific rules
            if (role == UserRole.Admin && string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Admin must have a full name.");

            // Create the user
            return new User(
                id: Guid.NewGuid(),
                username: username,
                email: email,
                fullName: fullName,
                role: role,
                isApproved: false,      // Must be approved by Admin
                isActive: true,         // Active by default
                createdAtUtc: DateTime.UtcNow,
                passwordHash: string.Empty // Set later via SetPasswordHash()
            );
        }
    }
}

