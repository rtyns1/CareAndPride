using MediatR;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Classes.ClassesCommands
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, bool>
    {
        private readonly IClassRepository _classRepository;

        public UpdateClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<bool> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var entity = await _classRepository.GetByIdAsync(request.Id);
            if (entity == null) throw new ClassNotFoundException(request.Id);

            entity.UpdateDetails(request.Name, request.GradeLevel, request.Capacity);
            await _classRepository.UpdateAsync(entity);
            return true;
        }
    }
}
