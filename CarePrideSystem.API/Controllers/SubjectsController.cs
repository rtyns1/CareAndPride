using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Application.Features.Subjects.SubjectsCommands;
using CarePrideSystem.Application.Features.Subjects.Queries;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubjectsController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create(CreateSubjectDto dto)
        {
            var id = await _mediator.Send(new CreateSubjectCommand
            {
                Name = dto.Name, Code = dto.Code, Description = dto.Description
            });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<SubjectDto>> GetById(Guid id)
            => Ok(await _mediator.Send(new GetSubjectByIdQuery { Id = id }));

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll()
            => Ok(await _mediator.Send(new GetAllSubjectsQuery()));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, UpdateSubjectDto dto)
        {
            await _mediator.Send(new UpdateSubjectCommand
            {
                Id = id, Name = dto.Name, Code = dto.Code, Description = dto.Description
            });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteSubjectCommand { Id = id });
            return NoContent();
        }
    }
}
