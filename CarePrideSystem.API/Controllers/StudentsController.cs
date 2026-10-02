using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Application.Features.Students.StudentsCommands;
using CarePrideSystem.Application.Features.Students.Queries;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IGradeRepository _gradeRepo;

        public StudentsController(IMediator mediator, IGradeRepository gradeRepo)
        {
            _mediator = mediator;
            _gradeRepo = gradeRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<Guid>> CreateStudent(CreateStudentDto dto)
        {
            var command = new CreateStudentCommand
            {
                FirstName = dto.FirstName, LastName = dto.LastName, DateOfBirth = dto.DateOfBirth,
                AdmissionNumber = dto.AdmissionNumber, ClassId = dto.ClassId, MedicalConditions = dto.MedicalConditions
            };
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetStudent), new { id }, id);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
            => Ok(await _mediator.Send(new GetStudentByIdQuery { Id = id }));

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
            => Ok(await _mediator.Send(new GetAllStudentsQuery()));

        [HttpGet("class/{classId}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByClass(Guid classId)
            => Ok(await _mediator.Send(new GetStudentByClassQuery { ClassId = classId }));

        [HttpGet("{id}/grades")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<IActionResult> GetStudentGrades(Guid id)
        {
            var grades = await _gradeRepo.GetByStudentIdAsync(id);
            var result = grades.Select(g => new
            {
                g.SubjectId,
                g.Term,
                g.ExamType,
                g.Score,
                g.MaxScore
            });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> UpdateStudent(Guid id, UpdateStudentDto dto)
        {
            await _mediator.Send(new UpdateStudentCommand
            {
                Id = id, FirstName = dto.FirstName, LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth, MedicalConditions = dto.MedicalConditions
            });
            return NoContent();
        }

        [HttpPost("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveStudent(Guid id)
        {
            await _mediator.Send(new ArchiveStudentCommand { Id = id });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            await _mediator.Send(new ArchiveStudentCommand { Id = id });
            return NoContent();
        }
    }
}

