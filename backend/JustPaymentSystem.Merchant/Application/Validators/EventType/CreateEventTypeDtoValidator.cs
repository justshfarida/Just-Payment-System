using Application.Dtos;
using FluentValidation;

namespace Application.Validators.EventType;

public class CreateEventTypeDtoValidator : AbstractValidator<CreateEventTypeDto>
{
    public CreateEventTypeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
    }
}
