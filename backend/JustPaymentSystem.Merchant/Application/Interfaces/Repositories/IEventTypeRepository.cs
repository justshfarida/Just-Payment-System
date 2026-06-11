using Domain.Entitites;
using Domain.Interfaces;

namespace Application.Interfaces.Repositories;

public interface IEventTypeRepository : IRepositoryBase<EventType>
{
    Task<IReadOnlyList<EventType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EventType?> GetByIdAsync(Guid id, bool trackChanges = false);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default);
    Task<bool> HasWebhookEventsAsync(Guid eventTypeId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync();
}
