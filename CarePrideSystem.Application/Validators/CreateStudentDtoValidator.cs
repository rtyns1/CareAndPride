using FluentValidation;
using CarePrideSystem.Application.DTOs.Students;

namespace CarePrideSystem.Application.Validators
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.AdmissionNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.UtcNow);
            RuleFor(x => x.ClassId).NotEmpty();
        }
    }
}
