using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarePrideSystem.Application.DTOs.Assignments;
using CarePrideSystem.Application.Features.Assignments.Commands;
using CarePrideSystem.Application.Features.Assignments.Queries;
using CarePrideSystem.Application.Interfaces.Services;

namespace CarePrideSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorage;

        public AssignmentsController(IMediator mediator, IFileStorageService fileStorage)
        {
            _mediator = mediator;
            _fileStorage = fileStorage;
        }

        public class CreateAssignmentForm
        {
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public Guid SubjectId { get; set; }
            public Guid ClassId { get; set; }
            public Guid TeacherId { get; set; }
            public DateTime DueDateUtc { get; set; }
            public decimal MaxScore { get; set; } = 100;
            public string ExamType { get; set; } = "Assignment";
            public IFormFile? File { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [RequestSizeLimit(52428800)]
        public async Task<ActionResult<Guid>> Create([FromForm] CreateAssignmentForm form)
        {
            byte[]? fileBytes = null;
            string? fileName = null;

            if (form.File != null && form.File.Length > 0)
            {
                using var ms = new MemoryStream();
                await form.File.CopyToAsync(ms);
                fileBytes = ms.ToArray();
                fileName = form.File.FileName;
            }

            var id = await _mediator.Send(new CreateAssignmentCommand
            {
                Title = form.Title,
                Description = form.Description,
                ExamType = form.ExamType,
                SubjectId = form.SubjectId,
                ClassId = form.ClassId,
                TeacherId = form.TeacherId,
                DueDateUtc = form.DueDateUtc,
                MaxScore = form.MaxScore,
                FileContent = fileBytes,
                FileName = fileName
            });

            return Ok(id);
        }

        [HttpGet("class/{classId}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> ByClass(Guid classId)
            => Ok(await _mediator.Send(new GetAssignmentsByClassQuery { ClassId = classId }));

        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> ByStudent(Guid studentId)
            => Ok(await _mediator.Send(new GetAssignmentsByStudentQuery { StudentId = studentId }));

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<ActionResult<AssignmentDto>> ById(Guid id)
        {
            var dto = await _mediator.Send(new GetAssignmentByIdQuery { Id = id });
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpGet("{id}/download")]
        [Authorize(Roles = "Admin,Secretary,Teacher")]
        public async Task<IActionResult> Download(Guid id)
        {
            var dto = await _mediator.Send(new GetAssignmentByIdQuery { Id = id });
            if (dto == null || string.IsNullOrEmpty(dto.FilePath)) return NotFound();

            try
            {
                var stream = await _fileStorage.GetFileAsync(dto.FilePath);
                var downloadName = string.IsNullOrWhiteSpace(dto.OriginalFileName)
                    ? $"assignment-{dto.Id}" : dto.OriginalFileName;
                return File(stream, "application/octet-stream", downloadName);
            }
            catch (FileNotFoundException) { return NotFound(); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }
    }
}



