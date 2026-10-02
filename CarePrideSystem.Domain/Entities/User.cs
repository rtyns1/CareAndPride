using CarePrideSystem.Domain.Enums;

namespace CarePrideSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        public bool IsApproved { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public string? TwoFactorSecret { get; private set; }
        public DateTime? LastLoginAtUtc { get; private set; }

        private User() { }

        internal User(
            Guid id,
            string username,
            string email,
            string fullName,
            UserRole role,
            bool isApproved,
            bool isActive,
            DateTime createdAtUtc,
            string? passwordHash = null)
        {
            Id = id;
            Username = username;
            Email = email;
            FullName = fullName;
            Role = role;
            IsApproved = isApproved;
            IsActive = isActive;
            CreatedAtUtc = createdAtUtc;
            PasswordHash = passwordHash ?? string.Empty;
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
            PasswordHash = passwordHash;
        }

        public void Approve()
        {
            if (IsApproved)
                throw new InvalidOperationException("User is already approved.");
            IsApproved = true;
        }

        public void Reject()
        {
            IsActive = false;
            IsApproved = false;
        }

        public void RecordLogin()
        {
            LastLoginAtUtc = DateTime.UtcNow;
        }

        public void EnableTwoFactor(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
                throw new ArgumentException("2FA Secret is required.", nameof(secret));
            TwoFactorSecret = secret;
        }

        public void DisableTwoFactor()
        {
            TwoFactorSecret = null;
        }

        public void UpdateDetails(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required.", nameof(fullName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (!email.Contains('@'))
                throw new ArgumentException("Invalid email format.", nameof(email));

            FullName = fullName;
            Email = email;
        }
    }
}