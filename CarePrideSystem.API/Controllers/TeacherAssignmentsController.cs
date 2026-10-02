using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Application.Features.Teachers.TeachersCommands;
using CarePrideSystem.Application.Features.Teachers.Queries;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITeacherSubjectRepository _tsRepo;
        private readonly IClassTeacherRepository _ctRepo;

        public TeacherAssignmentsController(IMediator mediator,
            ITeacherSubjectRepository tsRepo, IClassTeacherRepository ctRepo)
        {
            _mediator = mediator;
            _tsRepo = tsRepo;
            _ctRepo = ctRepo;
        }

        [HttpPost("subjects")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignSubject(AssignTeacherSubjectDto dto)
        {
            await _mediator.Send(new AssignTeacherSubjectCommand
            {
                TeacherId = dto.TeacherId, SubjectId = dto.SubjectId,
                ClassId = dto.ClassId, AcademicYearId = dto.AcademicYearId
            });
            return NoContent();
        }

        [HttpPost("classteachers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignClassTeacher(AssignClassTeacherDto dto)
        {
            await _mediator.Send(new AssignClassTeacherCommand
            {
                TeacherId = dto.TeacherId, ClassId = dto.ClassId,
                AcademicYearId = dto.AcademicYearId, IsPrimary = dto.IsPrimary
            });
            return NoContent();
        }

        [HttpGet("subjects/{subjectId}/teachers")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<TeacherSubjectDto>>> GetTeachersBySubject(Guid subjectId)
            => Ok(await _mediator.Send(new GetTeachersBySubjectQuery { SubjectId = subjectId }));

        [HttpGet("classes/{classId}/teachers")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<ClassTeacherDto>>> GetClassTeachers(Guid classId)
            => Ok(await _mediator.Send(new GetClassTeachersQuery { ClassId = classId }));

        [HttpGet("teacher/{teacherId}/subjects")]
        [Authorize]
        public async Task<IActionResult> GetByTeacherSubjects(Guid teacherId)
        {
            var list = await _tsRepo.GetByTeacherIdAsync(teacherId);
            return Ok(list.Select(x => new
            {
                x.TeacherId, x.SubjectId, x.ClassId, x.AcademicYearId, x.AssignedAtUtc
            }));
        }

        [HttpGet("teacher/{teacherId}/classes")]
        [Authorize]
        public async Task<IActionResult> GetByTeacherClasses(Guid teacherId)
        {
            var list = await _ctRepo.GetByTeacherIdAsync(teacherId);
            return Ok(list.Select(x => new
            {
                x.TeacherId, x.ClassId, x.AcademicYearId, x.IsPrimary, x.AssignedAtUtc
            }));
        }
    }
}
