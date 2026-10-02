using CarePrideSystem;
using CarePrideSystem.Domain.Enums;

namespace CarePrideSystem.Application.DTOs.Auth
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}