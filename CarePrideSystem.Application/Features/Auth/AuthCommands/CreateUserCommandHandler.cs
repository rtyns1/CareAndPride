using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;
using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.UsernameExistsAsync(request.Username))
                throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException($"Email '{request.Email}' is already registered.");

            var user = UserCreationService.CreateUser(
                username: request.Username,
                email: request.Email,
                fullName: request.FullName,
                role: request.Role
            );

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.SetPasswordHash(hashedPassword);

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
