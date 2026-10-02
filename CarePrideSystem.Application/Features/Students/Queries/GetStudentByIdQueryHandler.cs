using MediatR;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentByIdQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.Id);
            if (student == null)
                throw new StudentNotFoundException(request.Id);

            return new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                AdmissionNumber = student.AdmissionNumber,
                ClassId = student.ClassId,
                MedicalConditions = student.MedicalConditions,
                IsArchived = student.IsArchived,
                CreatedAtUtc = student.CreatedAtUtc
            };
        }
    }
}
