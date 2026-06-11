using Application.Dtos;

namespace Application.Interfaces.Services;

public interface IEventTypeService
{
    Task<IReadOnlyList<EventTypeResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EventTypeResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EventTypeResponseDto> CreateAsync(CreateEventTypeDto request, CancellationToken cancellationToken = default);
    Task<EventTypeResponseDto?> UpdateAsync(Guid id, UpdateEventTypeDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
