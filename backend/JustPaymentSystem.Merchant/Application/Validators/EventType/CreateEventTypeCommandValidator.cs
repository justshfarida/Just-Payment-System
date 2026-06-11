using Application.Commands.EventType;
using Application.Dtos;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.EventType;

public class CreateEventTypeCommandValidator : AbstractValidator<CreateEventTypeCommand>
{
    public CreateEventTypeCommandValidator(
        IValidator<CreateEventTypeDto> createDtoValidator,
        IEventTypeRepository eventTypeRepository)
    {
        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(createDtoValidator);

        RuleFor(x => x.Request.Name)
            .MustAsync(async (name, cancellationToken) =>
                !await eventTypeRepository.ExistsByNameAsync(name, cancellationToken))
            .WithMessage("An event type with this name already exists.");
    }
}
