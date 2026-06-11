using Application.Interfaces.Repositories;
using Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EventTypeRepository : RepositoryBase<EventType>, IEventTypeRepository
{
    private readonly MerchantDbContext _context;

    public EventTypeRepository(MerchantDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<EventType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EventTypes
            .AsNoTracking()
            .OrderBy(eventType => eventType.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<EventType?> GetByIdAsync(Guid id, bool trackChanges = false)
    {
        var query = _context.EventTypes.Where(eventType => eventType.Id == id);

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _context.EventTypes
            .AsNoTracking()
            .AnyAsync(eventType => eventType.Name == name, cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default)
    {
        return _context.EventTypes
            .AsNoTracking()
            .AnyAsync(eventType => eventType.Name == name && eventType.Id != excludeId, cancellationToken);
    }

    public Task<bool> HasWebhookEventsAsync(Guid eventTypeId, CancellationToken cancellationToken = default)
    {
        return _context.WebhookEvents
            .AsNoTracking()
            .AnyAsync(webhookEvent => webhookEvent.EventId == eventTypeId, cancellationToken);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
