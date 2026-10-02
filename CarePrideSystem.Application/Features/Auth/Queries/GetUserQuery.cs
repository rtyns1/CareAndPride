using MediatR;
using CarePrideSystem.Application.DTOs.Auth;

namespace CarePrideSystem.Application.Features.Auth.Queries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}