using MediatR;
using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Application.Exceptions;
using CarePrideSystem.Application.Interfaces.Services;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<TokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                throw new InvalidCredentialsException();

            if (!user.IsApproved)
                throw new AccountNotApprovedException();

            if (!user.IsActive)
                throw new AccountDisabledException();

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            user.RecordLogin();
            await _userRepository.UpdateAsync(user);

            var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

            return new TokenResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserId = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
        }
    }
}
