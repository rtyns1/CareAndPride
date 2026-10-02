using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpiresAt) GenerateToken(User user);
    }
}
