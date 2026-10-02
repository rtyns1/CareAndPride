using FluentValidation;
using CarePrideSystem.Application.DTOs.Classes;

namespace CarePrideSystem.Application.Validators
{
    public class UpdateClassDtoValidator : AbstractValidator<UpdateClassDto>
    {
        public UpdateClassDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.GradeLevel).InclusiveBetween(1, 13);
            RuleFor(x => x.Capacity).GreaterThan(0);
        }
    }
}
