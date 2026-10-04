using MediatR;

namespace CarePrideSystem.Application.Features.Students.StudentsCommands
{
    public class CreateStudentCommand : IRequest<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? AdmissionNumber { get; set; }
        public Guid ClassId { get; set; }
        public string? MedicalConditions { get; set; }
    }
}
