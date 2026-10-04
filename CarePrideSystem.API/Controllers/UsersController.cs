using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Application.Features.Auth.AuthCommands;
using CarePrideSystem.Application.Features.Auth.Queries;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> CreateUser(CreateUserDto dto)
        {
            var command = new CreateUserCommand
            {
                Username = dto.Username, Email = dto.Email,
                FullName = dto.FullName, Role = dto.Role, Password = dto.Password
            };
            var userId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUser), new { id = userId }, userId);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
            => Ok(await _mediator.Send(new GetUserQuery { Id = id }));

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
            => Ok(await _mediator.Send(new GetAllUsersQuery()));

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetPendingApprovals()
            => Ok(await _mediator.Send(new GetPendingApprovalQuery()));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto dto)
        {
            await _mediator.Send(new UpdateUserCommand { Id = id, FullName = dto.FullName, Email = dto.Email });
            return NoContent();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveUser(Guid id)
        {
            await _mediator.Send(new ApproveUserCommand { Id = id });
            return NoContent();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectUser(Guid id)
        {
            await _mediator.Send(new RejectUserCommand { Id = id });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand { Id = id });
            return NoContent();
        }
    }
}
