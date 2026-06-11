using Application.Commands.EventType;
using Application.Dtos;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.EventType;

public class UpdateEventTypeCommandValidator : AbstractValidator<UpdateEventTypeCommand>
{
    public UpdateEventTypeCommandValidator(
        IValidator<UpdateEventTypeDto> updateDtoValidator,
        IEventTypeRepository eventTypeRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Event type ID is required.");

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                await eventTypeRepository.GetByIdAsync(id, trackChanges: false) is not null)
            .WithMessage("Event type not found.");

        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(updateDtoValidator);

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
                !await eventTypeRepository.ExistsByNameAsync(command.Request.Name, command.Id, cancellationToken))
            .WithMessage("An event type with this name already exists.");
    }
}
