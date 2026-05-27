namespace Application.Features.Transactions.Commands.DTOs;

public record TransactionSession(Guid TransactionId, string SuccessUrl, string ErrorUrl, DateTime CreatedAt);
