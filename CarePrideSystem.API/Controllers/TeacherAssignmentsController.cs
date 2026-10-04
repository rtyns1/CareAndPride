using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Application.Features.Teachers.TeachersCommands;
using CarePrideSystem.Application.Features.Teachers.Queries;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TeacherAssignmentsController(IMediator mediator) { _mediator = mediator; }

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

        [HttpDelete("subjects")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveSubject([FromQuery] Guid teacherId, [FromQuery] Guid subjectId,
            [FromQuery] Guid classId, [FromQuery] Guid academicYearId)
        {
            await _mediator.Send(new RemoveTeacherSubjectCommand
            {
                TeacherId = teacherId, SubjectId = subjectId,
                ClassId = classId, AcademicYearId = academicYearId
            });
            return NoContent();
        }

        [HttpGet("subjects/{subjectId}/teachers")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<TeacherSubjectDto>>> GetTeachersBySubject(Guid subjectId)
            => Ok(await _mediator.Send(new GetTeachersBySubjectQuery { SubjectId = subjectId }));

        [HttpGet("teachers/{teacherId}/subjects")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<TeacherSubjectDto>>> GetSubjectsForTeacher(Guid teacherId)
            => Ok(await _mediator.Send(new GetTeacherSubjectAssignmentsQuery { TeacherId = teacherId }));

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

        [HttpDelete("classteachers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveClassTeacher([FromQuery] Guid teacherId,
            [FromQuery] Guid classId, [FromQuery] Guid academicYearId)
        {
            await _mediator.Send(new RemoveClassTeacherCommand
            {
                TeacherId = teacherId, ClassId = classId, AcademicYearId = academicYearId
            });
            return NoContent();
        }

        [HttpGet("classes/{classId}/teachers")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<ClassTeacherDto>>> GetClassTeachers(Guid classId)
            => Ok(await _mediator.Send(new GetClassTeachersQuery { ClassId = classId }));

        [HttpGet("teachers/{teacherId}/classes")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<ClassTeacherDto>>> GetClassesForTeacher(Guid teacherId)
            => Ok(await _mediator.Send(new GetTeacherClassAssignmentsQuery { TeacherId = teacherId }));
    }
}
