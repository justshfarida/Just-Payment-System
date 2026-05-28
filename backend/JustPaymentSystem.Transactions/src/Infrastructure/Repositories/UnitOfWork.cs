using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Wolverine;

namespace Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TransactionDbContext _db;
    private readonly IMessageBus _bus;
    private IDbContextTransaction _currentTransaction;
    public UnitOfWork(TransactionDbContext dbContext, IMessageBus bus)
    {
        _db = dbContext;
        _bus = bus;
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;
        _currentTransaction = await _db.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _db.SaveChangesAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        var res = await _db.SaveChangesAsync(cancellationToken);

        var domainEvents = _db.ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .SelectMany(x => x.Entity.Events)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _bus.PublishAsync(domainEvent);
        }

        foreach (var aggregate in _db.ChangeTracker
                     .Entries<AggregateRoot<Guid>>())
        {
            aggregate.Entity.ClearDomainEvents();
        }

        return res;
    }

    public void Dispose()
    {
        _db.Dispose();
        _currentTransaction?.Dispose();
    }

}