using FluentValidation;
using CarePrideSystem.Application.DTOs.Subjects;

namespace CarePrideSystem.Application.Validators
{
    public class UpdateSubjectDtoValidator : AbstractValidator<UpdateSubjectDto>
    {
        public UpdateSubjectDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        }
    }
}
