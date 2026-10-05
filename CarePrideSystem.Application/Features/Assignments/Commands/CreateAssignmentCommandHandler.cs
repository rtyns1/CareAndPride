using MediatR;
using CarePrideSystem.Application.Interfaces.Services;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Assignments.Commands
{
    public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, Guid>
    {
        private readonly IAssignmentRepository _repo;
        private readonly IFileStorageService _fileStorage;
        private readonly IClassTeacherRepository _classTeacherRepo;
        private readonly ITeacherSubjectRepository _teacherSubjectRepo;

        public CreateAssignmentCommandHandler(
            IAssignmentRepository repo,
            IFileStorageService fileStorage,
            IClassTeacherRepository classTeacherRepo,
            ITeacherSubjectRepository teacherSubjectRepo)
        {
            _repo = repo;
            _fileStorage = fileStorage;
            _classTeacherRepo = classTeacherRepo;
            _teacherSubjectRepo = teacherSubjectRepo;
        }

        public async Task<Guid> Handle(CreateAssignmentCommand request, CancellationToken ct)
        {
            var yearId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var classAssignments = await _classTeacherRepo.GetByTeacherIdAsync(request.TeacherId);
            var isClassTeacherOfThisClass = classAssignments.Any(ct2 => ct2.ClassId == request.ClassId);

            var subjectAssignments = await _teacherSubjectRepo.GetByTeacherIdAsync(request.TeacherId);
            var teachesThisSubjectHere = subjectAssignments.Any(ts =>
                ts.ClassId == request.ClassId && ts.SubjectId == request.SubjectId);

            if (!isClassTeacherOfThisClass && !teachesThisSubjectHere)
                throw new UnauthorizedAccessException(
                    "Only the class teacher or the subject teacher for this class can upload materials.");

            string? filePath = null;
            if (request.FileContent != null && request.FileContent.Length > 0 && !string.IsNullOrWhiteSpace(request.FileName))
            {
                using var ms = new MemoryStream(request.FileContent);
                filePath = await _fileStorage.SaveFileAsync(ms, request.FileName, "Assignments");
            }

            try
            {
                var assignment = AssignmentCreationService.CreateAssignment(
                    title: request.Title,
                    description: request.Description,
                    examType: request.ExamType,
                    subjectId: request.SubjectId,
                    classId: request.ClassId,
                    teacherId: request.TeacherId,
                    academicYearId: yearId,
                    dueDateUtc: request.DueDateUtc,
                    maxScore: request.MaxScore,
                    filePath: filePath,
                    originalFileName: request.FileName);

                await _repo.AddAsync(assignment);
                return assignment.Id;
            }
            catch
            {
                if (filePath != null)
                {
                    try { await _fileStorage.DeleteFileAsync(filePath); } catch { }
                }
                throw;
            }
        }
    }
}
