using Domain.Shared.Validations;

namespace Domain.Domains;

public class TransactionAttribute : Entity<Guid>
{
    private TransactionAttribute()
    {
        
    }

    private TransactionAttribute(string value)
    {
        Value = value;
    }

    private TransactionAttribute(Guid transactionId, string value)
    {
        TransactionId = transactionId;
        Value = value;
    }
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; }
    public string Value { get; set; } = null!;

    public static TransactionAttribute Create(string value)
    {
        value.EnsureNotNullOrEmpty();
        return new TransactionAttribute(value);
    }
    public static TransactionAttribute Create(Guid transactionId, string value)
    {
        value.EnsureNotNullOrEmpty();
        return new TransactionAttribute(transactionId, value);
    }
}
