using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Grades;
using CarePrideSystem.Application.Features.Grades.Commands;
using CarePrideSystem.Application.Features.Grades.Queries;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GradesController(IMediator mediator) { _mediator = mediator; }

        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetByStudent(Guid studentId)
            => Ok(await _mediator.Send(new GetGradesByStudentQuery { StudentId = studentId }));

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<Guid>> Add(AddGradeCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
