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
            string admissionNumber;

            if (!string.IsNullOrWhiteSpace(request.AdmissionNumber))
            {
                admissionNumber = request.AdmissionNumber.Trim();
                if (await _studentRepository.AdmissionNumberExistsAsync(admissionNumber))
                    throw new InvalidOperationException($"Admission number '{admissionNumber}' already exists.");
            }
            else
            {
                admissionNumber = await GenerateAdmissionNumberAsync();
            }

            var student = StudentCreationService.CreateStudent(
                firstName: request.FirstName,
                lastName: request.LastName,
                dateOfBirth: request.DateOfBirth,
                admissionNumber: admissionNumber,
                classId: request.ClassId,
                medicalConditions: request.MedicalConditions);

            await _studentRepository.AddAsync(student);
            return student.Id;
        }

        private async Task<string> GenerateAdmissionNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"CP-{year}-";

            for (int seq = 1; seq <= 9999; seq++)
            {
                var candidate = $"{prefix}{seq:D4}";
                if (!await _studentRepository.AdmissionNumberExistsAsync(candidate))
                    return candidate;
            }

            throw new InvalidOperationException("Could not generate a unique admission number for this year.");
        }
    }
}
