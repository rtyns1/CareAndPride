using MediatR;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Services;

namespace CarePrideSystem.Application.Features.Students.StudentsCommands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;

        public CreateStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Guid> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            if (await _studentRepository.AdmissionNumberExistsAsync(request.AdmissionNumber))
                throw new InvalidOperationException($"Admission number '{request.AdmissionNumber}' already exists.");

            var student = StudentCreationService.CreateStudent(
                firstName: request.FirstName,
                lastName: request.LastName,
                dateOfBirth: request.DateOfBirth,
                admissionNumber: request.AdmissionNumber,
                classId: request.ClassId,
                medicalConditions: request.MedicalConditions
            );

            await _studentRepository.AddAsync(student);
            return student.Id;
        }
    }
}
