namespace Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; init; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public bool Equals(Entity<TId>? other)
    {
        return other is not null && EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TId>);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
