using Domain.Abstractions;

namespace Domain;

public class AggregateRoot<T> : Entity<T>
    where T : notnull
{
    private List<IDomainEvent> _events = new();

    public void RaiseDomainEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearDomainEvents() => _events.Clear();
    public IReadOnlyList<IDomainEvent> Events => _events.AsReadOnly();
}
