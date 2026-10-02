using MediatR;
using CarePrideSystem.Application.DTOs.Auth;

namespace CarePrideSystem.Application.Features.Auth.Queries
{
    public class GetPendingApprovalQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}