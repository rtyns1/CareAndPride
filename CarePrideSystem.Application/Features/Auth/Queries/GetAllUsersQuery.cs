using MediatR;
using CarePrideSystem.Application.DTOs.Auth;

namespace CarePrideSystem.Application.Features.Auth.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}