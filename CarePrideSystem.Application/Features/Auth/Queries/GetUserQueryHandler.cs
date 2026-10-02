using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Interfaces;
using MediatR;

namespace CarePrideSystem.Application.Features.Auth.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new UserNotFoundException(request.Id);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                IsApproved = user.IsApproved,
                IsActive = user.IsActive,
                CreatedAtUtc = user.CreatedAtUtc,
                LastLoginAtUtc = user.LastLoginAtUtc
            };
        }
    }
}
