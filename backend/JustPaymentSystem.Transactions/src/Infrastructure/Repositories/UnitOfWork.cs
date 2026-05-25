using Application.Common.Interfaces;
using Domain;
using Wolverine;

namespace Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TransactionDbContext _dbContext;
    private readonly IMessageBus _bus;

    public UnitOfWork(TransactionDbContext dbContext, IMessageBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        var res = await _dbContext.SaveChangesAsync(cancellationToken);

        var domainEvents = _dbContext.ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .SelectMany(x => x.Entity.Events)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _bus.PublishAsync(domainEvent);
        }

        foreach (var aggregate in _dbContext.ChangeTracker
                     .Entries<AggregateRoot<Guid>>())
        {
            aggregate.Entity.ClearDomainEvents();
        }

        return res;
    }
}