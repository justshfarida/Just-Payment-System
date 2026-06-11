using Application.Commands.BusinessType;
using FluentValidation;

namespace Application.Validators.BusinessType;

public class BusinessTypeIdCommandValidator : AbstractValidator<BusinessTypeIdCommand>
{
    public BusinessTypeIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Business type ID is required.");
    }
}
