namespace Application.Features.Transactions.Commands.DTOs;

public record TransactionSession(Guid TransactionId, DateTime CreatedAt);
