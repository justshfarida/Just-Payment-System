using Domain.Abstractions;

namespace Domain.Events;

public record TransactionCreated(Guid TransactionId) : IDomainEvent;
