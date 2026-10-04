using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Auth;
using CarePrideSystem.Application.Features.Auth.AuthCommands;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) { _mediator = mediator; }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand { Username = dto.Username, Password = dto.Password });
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<Guid>> Register(RegisterDto dto)
        {
            var id = await _mediator.Send(new RegisterTeacherCommand
            {
                Username = dto.Username, Email = dto.Email,
                FullName = dto.FullName, Password = dto.Password
            });
            return Ok(id);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            await _mediator.Send(new ChangePasswordCommand
            {
                UserId = userId,
                CurrentPassword = dto.CurrentPassword,
                NewPassword = dto.NewPassword
            });
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var fullName = User.FindFirst("FullName")?.Value;
            return Ok(new { userId, username, role, fullName });
        }
    }
}
