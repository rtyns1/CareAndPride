using MediatR;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Teachers.TeachersCommands
{
    public class AssignClassTeacherCommandHandler : IRequestHandler<AssignClassTeacherCommand, bool>
    {
        private readonly IClassTeacherRepository _repo;
        public AssignClassTeacherCommandHandler(IClassTeacherRepository repo) { _repo = repo; }

        public async Task<bool> Handle(AssignClassTeacherCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.ExistsAsync(request.TeacherId, request.ClassId, request.AcademicYearId))
                throw new InvalidOperationException("This teacher is already assigned to this class for this year.");

            if (request.IsPrimary && await _repo.HasPrimaryClassTeacherAsync(request.ClassId, request.AcademicYearId))
                throw new InvalidOperationException("This class already has a primary class teacher for this year.");

            var entity = ClassTeacherCreationService.CreateClassTeacher(
                request.TeacherId, request.ClassId, request.AcademicYearId, request.IsPrimary);

            await _repo.AddAsync(entity);
            return true;
        }
    }
}
