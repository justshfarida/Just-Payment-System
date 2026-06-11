using Application.Dtos;
using Application.Interfaces.MappingProfiles;
using Domain.Entitites;

namespace Application.MappingProfiles;

public class EventTypeMapper : IEventTypeMapper
{
    public EventType Map(CreateEventTypeDto request)
    {
        var now = DateTime.UtcNow;

        return new EventType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public EventTypeResponseDto MapToResponse(EventType eventType)
    {
        return new EventTypeResponseDto
        {
            Id = eventType.Id,
            Name = eventType.Name,
            CreatedAt = eventType.CreatedAt,
            UpdatedAt = eventType.UpdatedAt
        };
    }

    public void ApplyUpdate(EventType eventType, UpdateEventTypeDto request)
    {
        eventType.Name = request.Name;
        eventType.UpdatedAt = DateTime.UtcNow;
    }
}
