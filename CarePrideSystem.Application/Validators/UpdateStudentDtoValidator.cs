using FluentValidation;
using CarePrideSystem.Application.DTOs.Students;

namespace CarePrideSystem.Application.Validators
{
    public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
    {
        public UpdateStudentDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.UtcNow);
        }
    }
}
