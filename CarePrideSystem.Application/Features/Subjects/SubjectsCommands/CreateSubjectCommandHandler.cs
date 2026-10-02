using MediatR;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Subjects.SubjectsCommands
{
    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, Guid>
    {
        private readonly ISubjectRepository _repo;
        public CreateSubjectCommandHandler(ISubjectRepository repo) { _repo = repo; }

        public async Task<Guid> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.CodeExistsAsync(request.Code))
                throw new InvalidOperationException($"Subject code '{request.Code}' already exists.");

            var entity = SubjectCreationService.CreateSubject(request.Name, request.Code, request.Description);
            await _repo.AddAsync(entity);
            return entity.Id;
        }
    }
}
