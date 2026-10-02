using FluentValidation;
using CarePrideSystem.Application.DTOs.Classes;

namespace CarePrideSystem.Application.Validators
{
    public class CreateClassDtoValidator : AbstractValidator<CreateClassDto>
    {
        public CreateClassDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.GradeLevel).InclusiveBetween(1, 13);
            RuleFor(x => x.Capacity).GreaterThan(0);
            RuleFor(x => x.AcademicYearId).NotEmpty();
        }
    }
}
