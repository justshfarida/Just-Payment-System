using Application.Dtos;
using FluentValidation;

namespace Application.Validators.BusinessType;

public class CreateBusinessTypeDtoValidator : AbstractValidator<CreateBusinessTypeDto>
{
    public CreateBusinessTypeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
    }
}
