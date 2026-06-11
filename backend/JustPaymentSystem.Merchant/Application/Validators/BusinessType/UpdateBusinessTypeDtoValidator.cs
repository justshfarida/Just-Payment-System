using Application.Dtos;
using FluentValidation;

namespace Application.Validators.BusinessType;

public class UpdateBusinessTypeDtoValidator : AbstractValidator<UpdateBusinessTypeDto>
{
    public UpdateBusinessTypeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
    }
}
