using MediatR;

namespace CarePrideSystem.Application.Features.Students.StudentsCommands
{
    public class UpdateStudentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? MedicalConditions { get; set; }
    }
}
