using MediatR;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Students.StudentsCommands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;

        public UpdateStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.Id);
            if (student == null)
                throw new StudentNotFoundException(request.Id);

            student.UpdateDetails(request.FirstName, request.LastName, request.DateOfBirth, request.MedicalConditions);
            await _studentRepository.UpdateAsync(student);
            return true;
        }
    }
}
