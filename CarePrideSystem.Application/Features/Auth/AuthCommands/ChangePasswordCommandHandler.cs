using MediatR;
using CarePrideSystem.Application.Exceptions;
using CarePrideSystem.Application.Interfaces.Services;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 8)
                throw new InvalidOperationException("New password must be at least 8 characters.");

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) throw new InvalidCredentialsException();

            if (!_hasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                throw new InvalidOperationException("Current password is incorrect.");

            user.SetPasswordHash(_hasher.HashPassword(request.NewPassword));
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
