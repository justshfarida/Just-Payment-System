using Application.Commands.EventType;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.EventType;

public class DeleteEventTypeCommandValidator : AbstractValidator<DeleteEventTypeCommand>
{
    public DeleteEventTypeCommandValidator(IEventTypeRepository eventTypeRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Event type ID is required.");

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                await eventTypeRepository.GetByIdAsync(id, trackChanges: false) is not null)
            .WithMessage("Event type not found.");

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                !await eventTypeRepository.HasWebhookEventsAsync(id, cancellationToken))
            .WithMessage("Event type cannot be deleted because it is linked to one or more webhooks.");
    }
}
