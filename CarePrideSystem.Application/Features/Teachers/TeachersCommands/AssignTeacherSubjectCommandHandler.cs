using MediatR;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Teachers.TeachersCommands
{
    public class AssignTeacherSubjectCommandHandler : IRequestHandler<AssignTeacherSubjectCommand, bool>
    {
        private readonly ITeacherSubjectRepository _repo;
        public AssignTeacherSubjectCommandHandler(ITeacherSubjectRepository repo) { _repo = repo; }

        public async Task<bool> Handle(AssignTeacherSubjectCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.ExistsAsync(request.TeacherId, request.SubjectId, request.ClassId, request.AcademicYearId))
                throw new InvalidOperationException("This teacher is already assigned to this subject in this class for this year.");

            var entity = TeacherSubjectCreationService.CreateTeacherSubject(
                request.TeacherId, request.SubjectId, request.ClassId, request.AcademicYearId);

            await _repo.AddAsync(entity);
            return true;
        }
    }
}
