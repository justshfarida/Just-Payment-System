using Application.Dtos;

namespace Application.Commands.EventType;

public sealed record UpdateEventTypeCommand(Guid Id, UpdateEventTypeDto Request);
