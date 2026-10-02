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

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto dto)
        {
            var command = new LoginCommand
            {
                Username = dto.Username,
                Password = dto.Password
            };

            var result = await _mediator.Send(command);
            return Ok(result);
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
