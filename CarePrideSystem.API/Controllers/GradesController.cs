using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Grades;
using CarePrideSystem.Application.Features.Grades.GradesCommands;
using CarePrideSystem.Application.Features.Grades.Queries;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly IMediator _m;
        public GradesController(IMediator m) { _m = m; }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<Guid>> Create(CreateGradeDto dto)
        {
            var id = await _m.Send(new CreateGradeCommand
            {
                StudentId = dto.StudentId, SubjectId = dto.SubjectId, ClassId = dto.ClassId,
                TeacherId = dto.TeacherId, AcademicYearId = dto.AcademicYearId,
                AssessmentType = dto.AssessmentType, Title = dto.Title,
                Score = dto.Score, MaxScore = dto.MaxScore, Remarks = dto.Remarks
            });
            return Ok(id);
        }

        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Teacher,Secretary")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> ByStudent(Guid studentId)
            => Ok(await _m.Send(new GetGradesByStudentQuery { StudentId = studentId }));
    }
}
