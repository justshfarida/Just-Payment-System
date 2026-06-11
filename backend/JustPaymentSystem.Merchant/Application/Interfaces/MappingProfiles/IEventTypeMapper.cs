using Application.Dtos;
using Domain.Entitites;

namespace Application.Interfaces.MappingProfiles;

public interface IEventTypeMapper
{
    EventType Map(CreateEventTypeDto request);
    EventTypeResponseDto MapToResponse(EventType eventType);
    void ApplyUpdate(EventType eventType, UpdateEventTypeDto request);
}
