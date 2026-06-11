using Application.Commands.EventType;
using FluentValidation;

namespace Application.Validators.EventType;

public class EventTypeIdCommandValidator : AbstractValidator<EventTypeIdCommand>
{
    public EventTypeIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Event type ID is required.");
    }
}
