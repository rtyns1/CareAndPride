using MediatR;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Teachers.TeachersCommands
{
    public class RemoveClassTeacherCommandHandler : IRequestHandler<RemoveClassTeacherCommand, bool>
    {
        private readonly IClassTeacherRepository _repo;
        public RemoveClassTeacherCommandHandler(IClassTeacherRepository repo) { _repo = repo; }

        public async Task<bool> Handle(RemoveClassTeacherCommand request, CancellationToken cancellationToken)
        {
            if (!await _repo.ExistsAsync(request.TeacherId, request.ClassId, request.AcademicYearId))
                throw new InvalidOperationException("This class teacher assignment does not exist.");

            await _repo.DeleteAsync(request.TeacherId, request.ClassId, request.AcademicYearId);
            return true;
        }
    }
}
