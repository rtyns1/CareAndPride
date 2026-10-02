using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Application.Features.Classes.ClassesCommands;
using CarePrideSystem.Application.Features.Classes.Queries;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClassesController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create(CreateClassDto dto)
        {
            var id = await _mediator.Send(new CreateClassCommand
            {
                Name = dto.Name, GradeLevel = dto.GradeLevel,
                Capacity = dto.Capacity, AcademicYearId = dto.AcademicYearId
            });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<ClassDto>> GetById(Guid id)
            => Ok(await _mediator.Send(new GetClassByIdQuery { Id = id }));

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetAll()
            => Ok(await _mediator.Send(new GetAllClassesQuery()));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, UpdateClassDto dto)
        {
            await _mediator.Send(new UpdateClassCommand
            {
                Id = id, Name = dto.Name, GradeLevel = dto.GradeLevel, Capacity = dto.Capacity
            });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteClassCommand { Id = id });
            return NoContent();
        }
    }
}
