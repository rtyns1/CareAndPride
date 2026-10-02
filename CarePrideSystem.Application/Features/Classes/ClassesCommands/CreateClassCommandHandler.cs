using MediatR;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Classes.ClassesCommands
{
    public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Guid>
    {
        private readonly IClassRepository _classRepository;

        public CreateClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<Guid> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            if (await _classRepository.NameExistsAsync(request.Name))
                throw new InvalidOperationException($"Class '{request.Name}' already exists.");

            var entity = ClassCreationService.CreateClass(
                request.Name, request.GradeLevel, request.Capacity, request.AcademicYearId);

            await _classRepository.AddAsync(entity);
            return entity.Id;
        }
    }
}
