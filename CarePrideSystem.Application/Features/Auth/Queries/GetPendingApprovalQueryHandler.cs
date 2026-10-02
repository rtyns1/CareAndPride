using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Interfaces;
using MediatR;

namespace CarePrideSystem.Application.Features.Auth.Queries
{
    public class GetPendingApprovalsQueryHandler : IRequestHandler<GetPendingApprovalQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetPendingApprovalsQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetPendingApprovalQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetPendingApprovalsAsync();

            return users.Select(user => new UserDto
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
            });
        }
    }
}
