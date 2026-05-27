namespace Application.Features.Transactions.Commands.DTOs;

public record TransactionSession(Guid TransactionId, string successUrl, string errorUrl, DateTime CreatedAt);
