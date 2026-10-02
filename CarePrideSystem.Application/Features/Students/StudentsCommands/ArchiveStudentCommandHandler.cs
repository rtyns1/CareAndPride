using MediatR;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Students.StudentsCommands
{
    public class ArchiveStudentCommandHandler : IRequestHandler<ArchiveStudentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;

        public ArchiveStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<bool> Handle(ArchiveStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.Id);
            if (student == null)
                throw new StudentNotFoundException(request.Id);

            student.Archive();
            await _studentRepository.UpdateAsync(student);
            return true;
        }
    }
}
