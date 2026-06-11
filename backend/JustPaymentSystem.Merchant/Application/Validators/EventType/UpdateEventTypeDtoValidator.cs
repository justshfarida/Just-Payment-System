using Application.Dtos;
using FluentValidation;

namespace Application.Validators.EventType;

public class UpdateEventTypeDtoValidator : AbstractValidator<UpdateEventTypeDto>
{
    public UpdateEventTypeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
    }
}
