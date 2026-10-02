using MediatR;
using CarePrideSystem.Application.DTOs.Students;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetStudentByClassQueryHandler : IRequestHandler<GetStudentByClassQuery, IEnumerable<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentByClassQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentDto>> Handle(GetStudentByClassQuery request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetByClassIdAsync(request.ClassId);

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
