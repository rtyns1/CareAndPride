using MediatR;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;

        public GetAllStudentsQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllAsync();

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                DateOfBirth = s.DateOfBirth,
                AdmissionNumber = s.AdmissionNumber,
                ClassId = s.ClassId,
                MedicalConditions = s.MedicalConditions,
                IsArchived = s.IsArchived,
                CreatedAtUtc = s.CreatedAtUtc
            });
        }
    }
}
