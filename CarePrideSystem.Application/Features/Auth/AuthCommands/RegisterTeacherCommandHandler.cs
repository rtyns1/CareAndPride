using MediatR;
using BCrypt.Net;
using CarePrideSystem.Domain.Enums;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class RegisterTeacherCommandHandler : IRequestHandler<RegisterTeacherCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public RegisterTeacherCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(RegisterTeacherCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.UsernameExistsAsync(request.Username))
                throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException($"Email '{request.Email}' is already registered.");

            var user = UserCreationService.CreateUser(
                username: request.Username,
                email: request.Email,
                fullName: request.FullName,
                role: UserRole.Teacher);

            user.SetPasswordHash(BCrypt.Net.BCrypt.HashPassword(request.Password));

            await _userRepository.AddAsync(user);
            return user.Id;
        }
    }
}
